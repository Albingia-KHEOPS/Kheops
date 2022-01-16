using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.DynamicGuiIS.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.Clausier;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class InformationsSpecifiquesRecupGarantiesController : ControllersBase<ModeleInformationsSpecifiquesRecupGarantiesPage>
    {
        #region Méthode Public
        
        [ErrorHandler]
        public ActionResult Index(string id)
        {

            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult RedirectionVersCondition(string branche, string section, string cible, string additionalParams, string dataToSave, string splitChars, string strParameters,
            string argCodeOffre, string argVersion, string argType, string argCodeFormule, string argCodeOption, string argLibelleFormule, string argLettreLibelleFormule, string tabGuid, string paramRedirect, string modeNavig,
            string addParamType, string addParamValue) {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            if (!GetIsReadOnly(tabGuid, argCodeOffre + "_" + argVersion + "_" + argType, numAvn) && modeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Standard) {
                if (!DbIOParam.SaveISToDB(branche, section, cible, additionalParams, dataToSave, splitChars, strParameters)) {
                    throw new AlbFoncException("Erreur de sauvegarde des informations spécifiques");
                }
                int version, codefor, codeopt = 0;
                if (int.TryParse(argVersion, out version) && int.TryParse(argCodeFormule, out codefor) && int.TryParse(argCodeOption, out codeopt)) {
                    using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                        RetGenClauseDto retGenClause = chan.Channel.GenerateClause(argType, argCodeOffre,
                            version,
                            new ParametreGenClauseDto
                            {
                                ActeGestion = "**",
                                Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option),
                                NuFormule = codefor,
                                NuOption = codeopt
                            });

                        if (retGenClause != null && !string.IsNullOrEmpty(retGenClause.MsgErreur)) {
                            throw new AlbFoncException(retGenClause.MsgErreur);
                        }
                    }
                }
            }

            if (paramRedirect.ContainsChars()) {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            return RedirectToAction("Index", "ChoixClauses", new { id = AlbParameters.BuildFullId(
                new Folder(new[] { argCodeOffre, argVersion, argType }),
                new[] { "¤InformationsSpecifiquesGarantie¤Index¤" + argCodeOffre + "£" + argVersion + "£" + argType + "£" + argCodeFormule + "£" + argCodeOption + "£" + argLibelleFormule + "£" + argLettreLibelleFormule },
                tabGuid,
                addParamValue,
                modeNavig)
            });
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string codeFormule, string codeOption, string tabGuid, string modeNavig, string addParamType, string addParamValue)
        {
            if (cible == "RechercheSaisie") {
                return RedirectToAction(job, cible);
            }
            return RedirectToAction(job, cible, new { id = AlbParameters.BuildFullId(
                new Folder(new[] { codeOffre, version, type }),
                new[] { codeFormule, codeOption },
                tabGuid,
                addParamValue,
                modeNavig)
            });
        }

        #endregion

        #region Méthode Privée

        protected override void LoadInfoPage(string id)
        {
            string codeBranche = string.Empty;
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
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                        model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type);
                    }

                    break;
                case "P":
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                    {
                        var serviceContext=client.Channel;
                        model.Contrat = serviceContext.GetContrat(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                        string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                        switch (typeAvt)
                        {
                            case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                                break;
                            case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                                break;
                            case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                                break;
                            default:
                                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                                break;
                        }
                        model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type);
                    }

                    break;
            }
            model.PageTitle = "Informations Spécifiques Garantie (Recup)";
            if (model.Offre != null || model.Contrat != null)
            {
                if (tId[2] == "O")
                    codeBranche = model.Offre.Branche.Code;
                if (tId[2] == "P")
                    codeBranche = model.Contrat.Branche;
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;


            }

            #region Bandeau
            model.Bandeau = null;
            SetBandeauNavigation(id);
            #endregion

            model.CodeFormule = tId[3];
            model.CodeOption = tId[4];
            model.LibelleFormule = tId[5];
            model.LettreLibelleFormule = tId[6];

            #region Navigation Arbre
            SetArbreNavigation();
            #endregion

            var elemPar = id.Split('_');
            var sbParams = new StringBuilder();
            sbParams.Append(elemPar[2] + MvcApplication.SPLIT_CONST_HTML + elemPar[0].PadLeft(9) +
                            MvcApplication.SPLIT_CONST_HTML);
            sbParams.Append(elemPar[1] + MvcApplication.SPLIT_CONST_HTML);
            sbParams.Append(elemPar[3] + MvcApplication.SPLIT_CONST_HTML);
            sbParams.Append(elemPar[4]);
            model.SpecificParameters = sbParams.ToString();

            model.Parameters = string.Format("&branche={0}&section={1}&", codeBranche,
                                                   AlbConstantesMetiers.INFORMATIONS_RECUP_GARANTIES);
        }

        private void SetArbreNavigation()
        {
            if (model.Offre != null)
            {
                //Affichage de la navigation latérale en arboresence
                this.model.NavigationArbre = GetNavigationArbre(
                    etape: "Formule",
                    codeFormule: this.model.CodeFormule.ParseInt().Value,
                    numOption: this.model.CodeOption.ParseInt().Value);
            }
            else if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Formule", codeFormule: Convert.ToInt32(model.CodeFormule));
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
                        Etape = Navigation_MetaModel.ECRAN_RISQUEETGARANTIE,
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
                        Etape = Navigation_MetaModel.ECRAN_RISQUEETGARANTIE,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                }
            }
        }

        #endregion

    }
}
