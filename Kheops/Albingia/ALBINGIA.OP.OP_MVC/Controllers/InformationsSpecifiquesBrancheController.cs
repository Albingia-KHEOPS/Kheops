using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.DynamicGuiIS.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using OP.WSAS400.DTO.Clausier;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.IClausesRisquesGaranties;
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class InformationsSpecifiquesBrancheController : InformationsSpecifiquesControllerBase<InformationsSpecifiquesBranche_MetaModel>
    {
        #region méthodes publiques
        
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            id = InitializeParams(id);
            LoadInfoPage();
            return View(model);
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Enregistrement(string message, string branche, string offreSimplifie, string section, string cible, string additionalParams,
            string dataToSave, string splitChars, string strParameters, string argRedirectIS,
            string codeOffre, string version, string type, string tabGuid, string saveCancel, string paramRedirect, string modeNavig, string numAvenant, string addParamType, string addParamValue) {
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvenant)
                && modeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Standard)
            {

                if (!DbIOParam.SaveISToDB(branche, section, cible, additionalParams, dataToSave, splitChars, strParameters))
                {
                    throw new AlbFoncException("Erreur de sauvegarde des informations spécifiques");
                }

                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                    RetGenClauseDto retGenClause = chan.Channel.GenerateClause(type, codeOffre,
                        Convert.ToInt32(version),
                        new ParametreGenClauseDto {
                            ActeGestion = "**",
                            Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale)
                        });

                    if (retGenClause != null && !string.IsNullOrEmpty(retGenClause.MsgErreur)) {
                        throw new AlbFoncException(retGenClause.MsgErreur);
                    }
                }
                
                if (paramRedirect.ContainsChars()) {
                    var tabParamRedirect = paramRedirect.Split('/');
                    return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
                }

                if (bool.TryParse(offreSimplifie, out bool isOffreSimplifie)) {
                    if (isOffreSimplifie && message != "-1") {
                        return RedirectToAction(
                            "Index",
                            "OffreSimplifiee",
                            new {
                                id = AlbParameters.BuildFullId(new Folder(new[] { codeOffre, version, type }), new[] { branche, message }, tabGuid, addParamValue, modeNavig)
                            });
                    }
                }
            }

            string action = "Index";
            string controller = null;
            string[] contextInfos = null;
            if (argRedirectIS.IsEmptyOrNull()) {
                if (!string.IsNullOrEmpty(GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE))) {
                    controller = "AnnulationQuittances";
                }
                else {
                    controller = "ChoixClauses";
                    contextInfos = new[] { "¤InformationsSpecifiquesBranche¤Index¤" + codeOffre + "£" + version + "£" + type };
                }
            }
            else {
                controller = "ChoixClauses";
                if (type != AlbConstantesMetiers.TYPE_OFFRE) {
                    if (!string.IsNullOrEmpty(GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE))) {
                        controller = "AnnulationQuittances";
                    }
                    else {
                        contextInfos = new[] { "¤AnInformationsGenerales¤Index¤" + codeOffre + "£" + version + "£" + type };
                    }
                }
                else {
                    contextInfos = new[] { "¤ModifierOffre¤Index¤" + codeOffre + "£" + version + "£" + type };
                }
            }
            return RedirectToAction(action, controller, new {
                id = AlbParameters.BuildFullId(new Folder(new[] { codeOffre, version, type }), contextInfos, tabGuid, addParamValue, modeNavig),
                returnHome = saveCancel,
                guidTab = tabGuid
            });
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string tabGuid, string modeNavig, string addParamType, string addParamValue) {
            string[] contextInfos = null;

            if (cible == "AnnulationQuittances")
            {
                contextInfos = new[] { "¤AnInformationsGenerales¤Index¤" + codeOffre + "£" + version + "£" + type };
            }
            else
                contextInfos = new[] { "¤ModifierOffre¤Index¤" + codeOffre + "£" + version + "£" + type };

            return RedirectToAction(job, cible, new
            {
                id = AlbParameters.BuildFullId(new Folder(new[] { codeOffre, version, type }), contextInfos, tabGuid, addParamValue, modeNavig),
            });
        }
        #endregion

        #region méthodes privées
        protected override void LoadInfoPage(string context = null)
        {
            switch (model.TypePolicePage)
            {
                case AlbConstantesMetiers.TYPE_OFFRE:
                    model.Offre = new Offre_MetaModel();
                    model.Offre = CacheModels.GetOffreFromCache(model.CodePolicePage, int.Parse(model.VersionPolicePage), model.TypePolicePage, model.ModeNavig.ParseCode<ModeConsultation>());
                    //model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type+"_"+"0");
                    model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type);
                    model.PageTitle = "Informations Spécifiques Offre";
                    break;
                case AlbConstantesMetiers.TYPE_CONTRAT:
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                    {
                        var serviceContext=client.Channel;
                        model.Contrat = serviceContext.GetContrat(model.CodePolicePage, model.VersionPolicePage, model.TypePolicePage, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                    }
                    //model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type+"_"+model.NumAvenantPage);
                    model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type);

                    model.PageTitle = "Informations Spécifiques Contrat";
                    break;
            }
            if (model.Offre != null || model.Contrat != null)
            {
                model.AfficherBandeau = base.DisplayBandeau(true, model.CodePolicePage + "_" + model.VersionPolicePage + "_" + model.TypePolicePage);
                model.AfficherNavigation = model.AfficherBandeau;
            }
            #region Navigation Arbre
            SetArbreNavigation();
            #endregion
            #region Bandeau
            model.Bandeau = null;
            SetBandeauNavigation(model.CodePolicePage + "_" + model.VersionPolicePage + "_" + model.TypePolicePage);
            #endregion
            var sbParams = new StringBuilder();
            var branche = string.Empty;
            if (model.Offre != null)
            {
                sbParams.Append(model.Offre.Type + MvcApplication.SPLIT_CONST_HTML + model.CodePolicePage.PadLeft(9) + MvcApplication.SPLIT_CONST_HTML);
                sbParams.Append(model.VersionPolicePage);
                model.SpecificParameters = sbParams.ToString();
                model.Parameters = string.Format("&branche={0}&section={1}&", model.Offre.Branche.Code,
                                                  AlbConstantesMetiers.INFORMATIONS_ENTETE);
                branche = model.Offre.Branche.Code;
            }
            else if (model.Contrat != null)
            {
                sbParams.Append(model.Contrat.Type + MvcApplication.SPLIT_CONST_HTML + model.CodePolicePage.PadLeft(9) + MvcApplication.SPLIT_CONST_HTML);
                sbParams.Append(model.VersionPolicePage);
                model.SpecificParameters = sbParams.ToString();
                model.Parameters = string.Format("&branche={0}&section={1}&", model.Contrat.Branche,
                                                  AlbConstantesMetiers.INFORMATIONS_ENTETE);
                branche = model.Contrat.Branche;
            }
            //********Traitement de l'offre simplifié
            SetSimpleFolderInfo();
        }

        //SLA (04/02/14) : Desactivation des paramètres, il faudra utiliser 
        //model.CodePolicePage, model.VersionPolicePage, model.TypePolicePage
        //une fois valider
        private void SetSimpleFolderInfo(/*IList<string> tId, string branche*/)
        {
            model.IsSimpleFolder = false;
            model.IsEditSimpleFolder = true;
            model.IsMesageVersionSimpleFolder = true;
        }

        //SLA (04/02/14) : il faudra utiliser 
        //model.CodePolicePage, model.VersionPolicePage, model.TypePolicePage
        //une fois valider et non tId
        //[Obsolete("ZBO:En attente de validation")]
        //private void SetSimpleFolderInfoVersionning(IList<string> tId, string branche)
        //{
        //    using (var wsExcelData = new OffreSimplifieExcelClient())
        //    {
        //        var param = tId[2] + MvcApplication.SPLIT_CONST_HTML + tId[0] + MvcApplication.SPLIT_CONST_HTML + tId[1];
        //        var parameters = ExcelIOParam.GetParams(param, MvcApplication.SPLIT_CONST_HTML);
        //        //---------- paramètres à déterminer celon le contexte
        //        var hParam = ExcelIOParam.PrepareParameter(parameters).ToList();
        //        var stateFolder = EditSimpleFolder(MvcApplication.EXCELXML_PARAMPATH, "OffreSimple",
        //                                                       branche, hParam);

        //        switch (stateFolder)
        //        {
        //            case SimpleFolderState.SFFinalaized:
        //                model.IsSimpleFolder = false;
        //                model.IsEditSimpleFolder = true;
        //                model.IsMesageVersionSimpleFolder = true;
        //                break;
        //            case SimpleFolderState.SFCurrent:
        //                model.IsSimpleFolder = true;
        //                model.IsEditSimpleFolder = false;
        //                break;
        //            case SimpleFolderState.NSFNew:
        //                model.IsSimpleFolder = false;
        //                model.IsEditSimpleFolder = true;
        //                break;
        //            case SimpleFolderState.NSFNewWithVersion:
        //                model.IsSimpleFolder = false;
        //                model.IsEditSimpleFolder = true;
        //                model.IsMesageVersionSimpleFolder = true;
        //                break;
        //        }




        //    }
        //}

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
                        Etape = Navigation_MetaModel.ECRAN_INFOGENERALE,
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
                        Etape = Navigation_MetaModel.ECRAN_INFOGENERALE,
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
                model.NavigationArbre = GetNavigationArbre("InfoGen");
            }
            else if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("InfoGen");
            }
        }

        #endregion
    }
}
