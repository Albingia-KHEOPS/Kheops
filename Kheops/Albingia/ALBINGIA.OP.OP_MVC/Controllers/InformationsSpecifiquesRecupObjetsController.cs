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
using OP.WSAS400.DTO.Offres.Risque;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class InformationsSpecifiquesRecupObjetsController : ControllersBase<ModeleInformationsSpecifiquesRecupObjetsPage>
    {
        #region Méthode Publique
        
        [ErrorHandler]
        public ActionResult Index(string id)
        {

            id = InitializeParams(id);
            LoadInfoPage(id);
            var elemPar = id.Split('_');
            var sbParams = new StringBuilder();
            if (model.Offre != null)
                sbParams.Append(model.Offre.Type + MvcApplication.SPLIT_CONST_HTML + elemPar[0].PadLeft(9) + MvcApplication.SPLIT_CONST_HTML);
            else if (model.Contrat != null)
                sbParams.Append(model.Contrat.Type + MvcApplication.SPLIT_CONST_HTML + elemPar[0].PadLeft(9) + MvcApplication.SPLIT_CONST_HTML);
            sbParams.Append(elemPar[1] + MvcApplication.SPLIT_CONST_HTML);
            sbParams.Append(elemPar[3] + MvcApplication.SPLIT_CONST_HTML);
            sbParams.Append(elemPar[4]);
            model.SpecificParameters = sbParams.ToString();
            if (model.Offre != null)
            {
                model.Parameters = string.Format("&branche={0}&section={1}&", model.Offre.Branche.Code, AlbConstantesMetiers.INFORMATIONS_RECUP_OBJETS);
            }
            else if (model.Contrat != null)
            {
                model.Parameters = string.Format("&branche={0}&section={1}&", model.Contrat.Branche, AlbConstantesMetiers.INFORMATIONS_RECUP_OBJETS);
            }
            model.PageTitle = "Informations Spécifiques Objet (Recup)";
            //model.Parameters
            return View(model);
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string branche, string section, string additionalParams, string dataToSave, string splitChars, string strParameters,
            string cible, string job, string codeOffre, string version, string type, string codeRisque, string codeObjet, string tabGuid, string paramRedirect, bool withSave, string modeNavig,
            string addParamType, string addParamValue) {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvn) && modeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Standard) {
                if (withSave) {
                    if (!DbIOParam.SaveISToDB(branche, section, cible, additionalParams, dataToSave, splitChars, strParameters)) {
                        throw new AlbFoncException("Erreur de sauvegarde des informations spécifiques");
                    }
                }
            }

            if (paramRedirect.ContainsChars()) {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            if (cible?.ToLower() == "recherchesaisie") {
                return RedirectToAction(job, cible);
            }
            else if (cible?.ToLower() != "detailsobjetrisque") {
                job = "Index";
                cible = "DetailsRisque";
            }
            return RedirectToAction(job, cible, new {
                id = AlbParameters.BuildFullId(
                    new Folder(new[] { codeOffre, version, type }),
                    cible == "DetailsObjetRisque" ? new[] { codeRisque, codeObjet } : new[] { codeRisque },
                    tabGuid,
                    addParamValue,
                    modeNavig)
            });
        }

        #endregion

        #region Méthode Privée

        protected override void LoadInfoPage(string id)
        {

            string[] tId = id.Split('_');

            if (tId[2] == "O")
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var policeServicesClient=client.Channel;
                    model.Offre = new Offre_MetaModel();
                    model.Offre.LoadOffre(policeServicesClient.OffreGetDto(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig.ParseCode<ModeConsultation>()));
                    //model.Offre.LoadOffre(policeServicesClient.OffreGetDto(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig));
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                }
            }
            else if (tId[2] == "P")
            {
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
                }
            }

            if (model.Offre != null || model.Contrat != null)
            {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }
            model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), tId[0] + "_" + tId[1] + "_" + tId[2]);

            if (!string.IsNullOrEmpty(tId[3].ToString()))
            {
                model.Code = tId[3];
                model.CodeObjet = tId[4];
                model.CodeRisque = tId[3];
                model.LibelleRisque = HttpUtility.UrlDecode(tId[5]);
                model.LibelleObjet = HttpUtility.UrlDecode(tId[6]);
                RisqueDto currentRsq = null;
                if (model.Offre != null)
                    currentRsq = model.Offre.Risques.FirstOrDefault(x => x.Code == Convert.ToInt32(tId[3].ToString()));
                else if (model.Contrat != null)
                    currentRsq = model.Contrat.Risques.FirstOrDefault(x => x.Code == Convert.ToInt32(tId[3].ToString()));

            }

            #region Navigation Arbre
            SetArbreNavigation();
            #endregion

            #region Bandeau
            model.Bandeau = null;
            SetBandeauNavigation(id);
            #endregion

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
        private void SetArbreNavigation()
        {
            if (model.Offre != null)
            {
                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre("Risque", codeRisque: Convert.ToInt32(model.CodeRisque));
            }
            else if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Risque", codeRisque: Convert.ToInt32(model.CodeRisque));
            }
        }
        #endregion

    }
}
