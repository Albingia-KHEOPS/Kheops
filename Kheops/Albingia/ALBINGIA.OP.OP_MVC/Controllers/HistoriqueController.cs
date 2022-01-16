using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ISaisieCreationOffre;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class HistoriqueController : ControllersBase<ModeleHistoriquePage>
    {

        #region Méthodes publique

        //
        // // [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }
              
        [ErrorHandler]
        public ActionResult OpenHistorique(string id)
        {        
            LoadInfoPage(id);
            model.IsModeDivFlottante = true;
            return PartialView("BodyHistorique", model);
        }

        [ErrorHandler]
        public ActionResult ReloadList(string codeAffaire, string version, string type, bool isContract)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext=client.Channel;
                ModeleHistoriquePage model = new ModeleHistoriquePage();
                var result = serviceContext.GetListHistorique(codeAffaire, version, type, isContract);
                if (result != null)
                    model = (ModeleHistoriquePage)result;

                return PartialView("ListHistorique", model);
            }
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string cible, string job, string codeAffaire, string version, string type, string codeAvt, string typeTraitement,string reguleId, string tabGuid, string addparam, string newWindow = "")
        {
            string modeNavig = "modeNavigHmodeNavigConsultOnly";
            if (codeAvt != "0")
            {
                switch (typeTraitement)
                {
                    case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                        cible = "AvenantInfoGenerales";
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                        cible = "EngagementPeriodes";
                        break;
                    default:
                        break;
                }
            }

            if (cible == "RechercheSaisie") {
                return RedirectToAction(job, cible);
            }
            if (cible == "CreationRegularisation" && !string.IsNullOrEmpty(addparam)) {
                modeNavig = "modeNavigSmodeNavigConsultOnly";
                var regulParams = addparam.ToParamDictionary();
                regulParams[AlbParameterName.REGULEID.ToString()] = reguleId;
                addparam = regulParams.RebuildAddParamString();
            }

            if (addparam.IsEmptyOrNull()) {
                return RedirectToAction(job, cible, new { id = string.Format("{0}_{1}_{2}{3}{4}{5}", codeAffaire, version, type, tabGuid, modeNavig, newWindow) });
            }

            return RedirectToAction(job, cible, new { id = string.Format("{0}_{1}_{2}_{3}{4}{5}{6}{7}", codeAffaire, version, type, codeAvt, tabGuid, addparam, modeNavig, newWindow) });
        }

        #endregion

        #region Méthodes privées

        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext=client.Channel;
                ModeleHistoriquePage model = new ModeleHistoriquePage();
                var result = serviceContext.GetListHistorique(tId[0], tId[1], tId[2], true);

                if (result != null)
                    model = (ModeleHistoriquePage)result;

                base.model.CodeAffaire = tId[0];
                base.model.Version = tId[1];
                base.model.Type = tId[2];

                base.model.IsContractuel = model.IsContractuel;
                base.model.ListHistorique = model.ListHistorique;
            }

            if (tId[2] == AlbConstantesMetiers.TYPE_OFFRE)
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var policeServicesClient=client.Channel;
                    model.Offre = new Offre_MetaModel();
                    model.Offre.LoadOffre(policeServicesClient.OffreGetDto(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig.ParseCode<ModeConsultation>()));
                }
            }
            else if (tId[2] == AlbConstantesMetiers.TYPE_CONTRAT)
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext=client.Channel;
                    model.Contrat = serviceContext.GetContrat(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                }
            }

            model.PageTitle = "Historique";
            model.AfficherBandeau = base.DisplayBandeau(true, id);
            model.AfficherNavigation = model.AfficherBandeau;
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
                        Etape = Navigation_MetaModel.ECRAN_HISTORIQUE,
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
                        Etape = Navigation_MetaModel.ECRAN_HISTORIQUE,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                }
            }
        }

        #endregion

    }
}
