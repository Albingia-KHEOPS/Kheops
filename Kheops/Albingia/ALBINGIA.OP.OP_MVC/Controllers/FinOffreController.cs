using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesFinOffre;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.FinOffre;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class FinOffreController : ControllersBase<ModeleFinOffrePage>
    {
        [ErrorHandler]
        
        public ActionResult Index(string id)
        {
            model.PageTitle = "Informations de fin";
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public void Update(string codeOffre, string version, string type, string argModeleFinOffreInfos, string argModeleFinOffreAnnotation, string tabGuid, string saveCancel, string paramRedirect, string modeNavig, string addParamType, string addParamValue)
        {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvn) && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>())
            {
                JavaScriptSerializer serialiserModeleFinOffreInfos = AlbJsExtendConverter<ModeleFinOffreInfos>.GetSerializer();
                ModeleFinOffreInfos ModeleFinOffreInfos = serialiserModeleFinOffreInfos.ConvertToType<ModeleFinOffreInfos>(serialiserModeleFinOffreInfos.DeserializeObject(argModeleFinOffreInfos));

                if (!ModeleFinOffreInfos.Relance) {
                    ModeleFinOffreInfos.RelanceValeur = 0;
                }

                ModeleFinOffrePage modeleFinOffrePage = new ModeleFinOffrePage()
                {
                    ModeleFinOffreAnnotation = new ModeleFinOffreAnnotation { AnnotationFin = argModeleFinOffreAnnotation },
                    ModeleFinOffreInfos = ModeleFinOffreInfos
                };

                modeleFinOffrePage.ModeleFinOffreAnnotation.AnnotationFin = HttpUtility.UrlDecode(modeleFinOffrePage.ModeleFinOffreAnnotation.AnnotationFin);
                if (!string.IsNullOrEmpty(ModeleFinOffreInfos.Description))
                {
                    ModeleFinOffreInfos.Description = ModeleFinOffreInfos.Description.Replace("\r\n", "<br>").Replace("\n", "<br>");
                }
                else
                {
                    ModeleFinOffreInfos.Description = string.Empty;
                }

                if (modeleFinOffrePage.ModeleFinOffreInfos.Antecedent.Equals("A") && modeleFinOffrePage.ModeleFinOffreInfos.Description.Length == 0)
                {
                    throw new AlbFoncException("La description ne peut être vide si l'antécédent est égal à 'A'");
                }
                if (modeleFinOffrePage.ModeleFinOffreInfos.ValiditeOffre == 0)
                {
                    throw new AlbFoncException("La durée de validité de l'offre doit être supérieure à 0");
                }
                if (modeleFinOffrePage.ModeleFinOffreInfos.DateProjet == null)
                {
                    throw new AlbFoncException("Date du projet obligatoire");
                }
                if (modeleFinOffrePage.ModeleFinOffreInfos.Relance && modeleFinOffrePage.ModeleFinOffreInfos.RelanceValeur == 0)
                {
                    throw new AlbFoncException("La durée de relance doit être supérieure à 0");
                }

                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var finOffreClient=client.Channel;
                    finOffreClient.UpdateFinOffre(codeOffre, version, type, ModeleFinOffrePage.LoadDto(modeleFinOffrePage), GetUser());
                }

                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                 var serviceontext=chan.Channel;
                    RetGenClauseDto retGenClause = serviceontext.GenerateClause(type, codeOffre, Convert.ToInt32(version),
                      new ParametreGenClauseDto
                      {
                          ActeGestion = "**",
                          Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Fin)
                      });
                    if (retGenClause != null && !string.IsNullOrEmpty(retGenClause.MsgErreur))
                    {
                        throw new AlbFoncException(retGenClause.MsgErreur);
                    }
                }
            }



          /*  if (paramRedirect.ContainsChars()) {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }*/

           /* return RedirectToAction("Index", "ChoixClauses", new { id = AlbParameters.BuildFullId(
                new Folder(new[] { codeOffre, version, type }),
                new[] { "¤FinOffre¤Index¤" + codeOffre + "£" + version + "£" + type },
                tabGuid,
                addParamValue,
                modeNavig), returnHome = saveCancel,
                guidTab = tabGuid
            });*/

        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string tabGuid, string modeNavig, string addParamType, string addParamValue)
        {
            return RedirectToAction(job, cible, new {
                id = AlbParameters.BuildStandardId(new Folder(new[] { codeOffre, version, type }), tabGuid, addParamValue, modeNavig)
            });
        }

        #region Méthodes Privées

        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');
            switch (tId[2])
            {
                case "O":
                    using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                    {
                     var CommonOffreClient=chan.Channel;
                        model.Offre = new Offre_MetaModel();
                        model.Offre.LoadInfosOffre(CommonOffreClient.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig));
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                        model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Offre.CodeOffre + "_" + model.Offre.Version.ToString() + "_" + model.Offre.Type, model.NumAvenantPage);
                    }
                    break;
                case "P":
                    using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                    {
                     var CommonOffreClient=chan.Channel;
                        var infosBase = CommonOffreClient.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig);
                        model.Contrat = new ContratDto()
                        {
                            CodeContrat = infosBase.CodeOffre,
                            VersionContrat = Convert.ToInt64(infosBase.Version),
                            Type = infosBase.Type,
                            Branche = infosBase.Branche.Code,
                            BrancheLib = infosBase.Branche.Nom,
                            Cible = infosBase.Branche.Cible.Code,
                            CibleLib = infosBase.Branche.Cible.Nom,
                            CourtierGestionnaire = infosBase.CabinetGestionnaire.Code,
                            Descriptif = infosBase.Descriptif,
                            CodeInterlocuteur = infosBase.CabinetGestionnaire.Code,
                            NomInterlocuteur = infosBase.CabinetGestionnaire.Inspecteur,
                            CodePreneurAssurance = Convert.ToInt32(infosBase.PreneurAssurance.Code),
                            NomPreneurAssurance = infosBase.PreneurAssurance.NomAssure,
                            PeriodiciteCode = infosBase.Periodicite,
                            Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                            Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur
                        };
                    }
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                    model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat.ToString() + "_" + model.Contrat.Type, model.NumAvenantPage);
                    break;
            }

            if (model.Offre != null || model.Contrat != null)
            {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }
            #region Navigation Arbre
            SetArbreNavigation();
            if (tId[2] == "O")
                model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Offre.CodeOffre + "_" + model.Offre.Version.ToString() + "_" + model.Offre.Type, model.NumAvenantPage);
            else
                model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat.ToString() + "_" + model.Contrat.Type, model.NumAvenantPage);
            #endregion
            #region Bandeau
            model.Bandeau = null;
            SetBandeauNavigation(id);
            #endregion
            LoadDataFinOffre();

            model.ModeleFinOffreAnnotation.IsReadOnly = model.ModeleFinOffreInfos.IsReadOnly = model.IsReadOnly;
        }

        private void LoadDataFinOffre()
        {
            ModeleFinOffrePage modele = new ModeleFinOffrePage();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext=client.Channel;
                FinOffreDto result = null;
                if (model.Offre != null)
                    result = serviceContext.InitFinOffre(model.Offre.CodeOffre, model.Offre.Version.ToString(), model.Offre.Type, string.Empty, model.ModeNavig.ParseCode<ModeConsultation>());
                else if (model.Contrat != null)
                    result = serviceContext.InitFinOffre(model.Contrat.CodeContrat, model.Contrat.VersionContrat.ToString(), model.Contrat.Type, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());

                if (result != null)
                    modele = ((ModeleFinOffrePage)result);

                model.ModeleFinOffreInfos = modele.ModeleFinOffreInfos;
                model.ModeleFinOffreAnnotation = modele.ModeleFinOffreAnnotation;
                if (model.Offre != null)
                    model.ModeleFinOffreInfos.Periodicite = model.Offre.Periodicite.Code;
                else if (model.Contrat != null)
                    model.ModeleFinOffreInfos.Periodicite = model.Contrat.PeriodiciteCode;
                List<AlbSelectListItem> antecedents = result.FinOffreInfosDto.Antecedents.Select(
                        m => new AlbSelectListItem
                        {
                            Value = m.Code,
                            Text = !string.IsNullOrEmpty(m.Code) && !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : string.Empty,
                            Selected = false,
                            Title = !string.IsNullOrEmpty(m.Code) && !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : string.Empty
                        }
                    ).ToList();

                if (!string.IsNullOrEmpty(model.ModeleFinOffreInfos.Antecedent))
                {
                    var sItem = antecedents.FirstOrDefault(x => x.Value == model.ModeleFinOffreInfos.Antecedent);
                    if (sItem != null)
                        sItem.Selected = true;
                }
                model.ModeleFinOffreInfos.Antecedents = antecedents;
            }
            if (model.Offre != null)
            {
                if (model.Offre.DateEffetGarantie.HasValue)
                {
                    model.ModeleFinOffreInfos.DateDebStr = model.Offre.DateEffetGarantie.Value.ToString().Split(' ')[0];
                }

            }
            else if (model.Contrat != null)
            {

                if (model.Contrat.DateEffetAnnee != 0 && model.Contrat.DateEffetMois != 0 && model.Contrat.DateEffetJour != 0)
                {
                        var timeDeb = AlbConvert.ConvertIntToTimeMinute(model.Contrat.DateEffetHeure);
                        model.ModeleFinOffreInfos.DateDebStr = new DateTime(model.Contrat.DateEffetAnnee,
                        model.Contrat.DateEffetMois,
                        model.Contrat.DateEffetJour,
                        timeDeb.HasValue ? timeDeb.Value.Hours : 0, timeDeb.HasValue ? timeDeb.Value.Minutes : 0, 0).ToString().Split(' ')[0];
                }
            }
        }
        private void SetBandeauNavigation(string id)
        {

            if (model.AfficherBandeau)
            {
                model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                //Gestion des Etapes
                if (model.Offre != null)
                {
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    model.Navigation = new Navigation_MetaModel
                    {
                        Etape = Navigation_MetaModel.ECRAN_INFOFIN,
                        IdOffre = model.Offre.CodeOffre,
                        Version = model.Offre.Version
                    };
                }
                else if (model.Contrat != null)
                {
                    string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt)
                    {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            model.Bandeau.StyleBandeau = model.ScreenType;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        default:
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                    model.Navigation = new Navigation_MetaModel
                    {
                        Etape = Navigation_MetaModel.ECRAN_INFOFIN,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                }
            }
        }
        private void SetArbreNavigation()
        {
            if (model.Offre != null)
            {
                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre("Fin");
            }
            else if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Fin");
            }
        }
        #endregion
    }
}
