using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModeleEngagements;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.SMP;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ISaisieCreationOffre;
using OPServiceContract.ITraitementsFinOffre;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class DetailCalculSMPController : ControllersBase<ModeleDetailCalculSMPPage>
    {
        #region Méthode Publique
        
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            model.PageTitle = "Détails du calcul SMP";
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        [ErrorHandler]
        public ActionResult Recalcule(string argCodeOffre, string argVersion, string argType, string codeAvn, string argLignes, string argIdRisque, string argIdVolet, 
            string tabGuid, string saveCancel, string paramRedirect, string modeNavig)
        {
            SMPdto recuperation = new SMPdto();
            if (!GetIsReadOnly(tabGuid, argCodeOffre + "_" + argVersion + "_" + argType, codeAvn))
            {
                JavaScriptSerializer serialiser = AlbJsExtendConverter<ModeleLigneDetailCalculSMP>.GetSerializer();
                var lignes = serialiser.ConvertToType<List<ModeleLigneDetailCalculSMP>>(serialiser.DeserializeObject(argLignes));

                List<LigneSMPdto> lignesDto = new List<LigneSMPdto>();
                foreach (var item in lignes)
                {
                    lignesDto.Add(ModeleLigneDetailCalculSMP.LoadDto(item));
                }

                SMPdto envoi = new SMPdto { ListeGarantie = lignesDto };

                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var serviceContext=client.Channel;
                    recuperation = serviceContext.RecalculSMP(envoi, argCodeOffre, argVersion, argType, codeAvn, argIdRisque, argIdVolet, modeNavig.ParseCode<ModeConsultation>());
                }
            }

            if (!string.IsNullOrEmpty(paramRedirect))
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            if (saveCancel == "1")
            {
                return RedirectToRoute("/RechercheSaisie/Index");
            }
            return PartialView("LigneGarantie", ChargeListeGarantieSMP(recuperation));
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string paramSMP, string tabGuid, string paramRedirect, string addParamType, string addParamValue)
        {
            if (!string.IsNullOrEmpty(paramRedirect))
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            if (cible == "RechercheSaisie")
                return RedirectToAction(job, cible);
            if (!string.IsNullOrEmpty(paramSMP))
            {
                return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + "_" + paramSMP + tabGuid + BuildAddParamString(addParamType, addParamValue) });
            }
            else
            {
                return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) });
            }
        }

        #endregion

        #region Méthode Privée

        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');

            switch (tId[2])
            {
                case "O":
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                    {
                        var policeServicesClient=client.Channel;
                        model.Offre = new Offre_MetaModel();
                        model.Offre.LoadOffre(policeServicesClient.OffreGetDto(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig.ParseCode<ModeConsultation>()));
                        //model.Offre.LoadOffre(policeServicesClient.OffreGetDto(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig));
                    }
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    break;
                case "P":
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                    {
                        var serviceContext=client.Channel;
                        model.Contrat = serviceContext.GetContrat(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());

                    }
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                    break;
            }

            if (model.Offre != null)
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
            else if (model.Contrat != null)
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);

            if (model.Offre != null || model.Contrat != null)
            {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }

            #region Navigation Arbre
            SetArbreNavigation();
            #endregion

            #region Bandeau
            model.Bandeau = null;
            SetBandeauNavigation(id);
            #endregion

            LoadDataDetail(id);
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
                        Etape = Navigation_MetaModel.ECRAN_ENGAGEMENTS,
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
                        Etape = Navigation_MetaModel.ECRAN_ENGAGEMENTS,
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
                model.NavigationArbre = GetNavigationArbre("Engagement");
            }
            else if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Engagement");
            }
        }

        private void LoadDataDetail(string id)
        {
            string[] tId = id.Split('_');
            string codeOffre = tId[0];
            string version = tId[1];
            string typeOffre = tId[2];
            //-- recuperation des id risque et volet
            string risque = tId[3].Split('¤')[0];
            string ventilation = tId[3].Split('¤')[1];
            model.IdRisque = risque;
            model.IdVolet = ventilation;

            SMPdto result = new SMPdto();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext=client.Channel;
                result = serviceContext.ObtenirDetailSMP(codeOffre, version, typeOffre, model.NumAvenantPage, risque, ventilation, model.ModeNavig.ParseCode<ModeConsultation>());
            }
            model.Entete = ChargeEnteteSMP(result);
            model.Entete.Risque = "Risque " + result.Risque.ToString();
            model.Entete.Ventilation = result.Ventilation;
            model.ListeGarantie = ChargeListeGarantieSMP(result);
        }

        private ModeleEnteteDetailCalculSMP ChargeEnteteSMP(SMPdto argOrigine)
        {
            ModeleEnteteDetailCalculSMP Entete = new ModeleEnteteDetailCalculSMP();
            Entete.NomTraite = argOrigine.NomTraite;
            Entete.Risque = "Risque " + argOrigine.Risque;
            Entete.Ventilation = argOrigine.Ventilation;
            return Entete;
        }

        private ModeleLignesDetailCalculSMP ChargeListeGarantieSMP(SMPdto argOrigine)
        {
            ModeleLignesDetailCalculSMP Ligne = new ModeleLignesDetailCalculSMP();
            if (argOrigine != null)
            {
                Ligne.Types = argOrigine.Types.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                Ligne.SMPtotal = "Risque " + argOrigine.SMPtotal;
                if (argOrigine.ListeGarantie != null && argOrigine.ListeGarantie.Count > 0)
                {
                    Ligne.ListeGarantie = new List<ModeleLigneDetailCalculSMP>();
                    foreach (LigneSMPdto detail in argOrigine.ListeGarantie)
                    {
                        ModeleLigneDetailCalculSMP ligneDetail = new ModeleLigneDetailCalculSMP();
                        ligneDetail.IdGarantie = detail.IdGarantie.ToString();
                        ligneDetail.CheckBox = detail.CheckBox;
                        ligneDetail.NomGarantie = detail.NomGarantie;
                        ligneDetail.LCI = detail.LCI;
                        ligneDetail.SMPcalcule = detail.SMPcalcule.ToString();
                        ligneDetail.Type = detail.Type;
                        ligneDetail.Valeur = detail.Valeur.ToString();
                        ligneDetail.SMPretenu = detail.SMPretenu.ToString();
                        Ligne.ListeGarantie.Add(ligneDetail);
                    }
                }
            }
            return Ligne;
        }

        #endregion
    }
}
