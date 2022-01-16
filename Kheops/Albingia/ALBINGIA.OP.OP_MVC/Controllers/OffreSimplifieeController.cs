using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.Offres.Branches;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ISaisieCreationOffre;
using System.Globalization;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class OffreSimplifieeController: ControllersBase<ModeleOffreSimplifieePage> {
        
        [ErrorHandler]
        public ActionResult Index(string id) {
            id = InitializeParams(id);
            DeleteInfoOffre(id);
            LoadInfoPage(id);

            return View(model);
        }
        [ErrorHandler]
        public ActionResult LoadOffreSimplifie(string branche) {

            model.Offre = new Offre_MetaModel { Branche = new BrancheDto { Code = branche } };
            return PartialView("ExcelContainer", model);
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult Annuler(string codeOffre, string version, string type, string tabGuid, string addParamType, string addParamValue) {
            return RedirectToAction("Index", "InformationsSpecifiquesBranche", new {
                id = AlbParameters.BuildStandardId(new Folder(new[] { codeOffre, version, type }), tabGuid, addParamValue, null)
            });
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string codeOffre, string version, string type, string tabGuid, string addParamType, string addParamValue) {
            return RedirectToAction("Index", "ClauseSimplifiee", new { id = AlbParameters.BuildStandardId(new Folder(new[] { codeOffre, version, type }), tabGuid, addParamValue, null) });
        }
        #region Méthodes privées
        private string GetExcelNameFromFile(string branche) {
            // return string.Format("{0}{1}_OffreSimple.xlsx", MvcApplication.OS_EXCELTEMPLATEFILE, branche);
            return string.Format("{0}_OffreSimple.xlsx", branche);
        }

        protected override void LoadInfoPage(string id) {
            var tId = id.Split('_');
            switch (tId[2]) {
                case "O":
                    model.Offre = new Offre_MetaModel();
                    model.Offre = CacheModels.GetOffreFromCache(tId[0], int.Parse(tId[1]), tId[2]);
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid),
                                                           model.Offre.CodeOffre + "_" + model.Offre.Version + "_" +
                                                           model.Offre.Type, model.NumAvenantPage);
                    model.FileName = GetExcelNameFromFile(model.Offre.Branche.Code);
                    model.SpecificParams = model.Offre.Type + MvcApplication.SPLIT_CONST_HTML + model.Offre.CodeOffre + MvcApplication.SPLIT_CONST_HTML +
                        (model.Offre.Version.HasValue ? model.Offre.Version.Value.ToString(CultureInfo.InvariantCulture) : "0") + MvcApplication.SPLIT_CONST_HTML + model.NumAvenantPage;
                    model.PageTitle = "Offre Simplifiée";
                    break;
                case "P":
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                        var serviceContext = client.Channel;
                        model.Contrat = serviceContext.GetContrat(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                    }
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                    model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid),
                                                           model.Contrat.CodeContrat + "_" +
                                                           model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
                    model.FileName = GetExcelNameFromFile(model.Offre.Branche.Code);
                    model.SpecificParams = model.Contrat.TypePolice + MvcApplication.SPLIT_CONST_HTML + model.Contrat.CodeContrat + MvcApplication.SPLIT_CONST_HTML +
                     (model.Contrat.VersionContrat.ToString(CultureInfo.InvariantCulture)) + MvcApplication.SPLIT_CONST_HTML + model.NumAvenantPage;
                    model.PageTitle = "Offre Simplifiée";
                    break;
            }
            model.NouvelleVersion = tId[4];
        }

        private void DeleteInfoOffre(string id) {
            var tId = id.Split('_');
            //using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>()) {
            //    var serviceContext = client.Channel;
            //    serviceContext.DeleteInfoOffre(tId[0], tId[1], tId[2]);
            //}
        }
        #endregion
    }
}
