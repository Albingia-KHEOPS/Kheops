using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.Avenant.ModelesAvenant;
using OPServiceContract.ICommon;
using System.Collections.Generic;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class SuspensionController : ControllersBase<ModeleVisuSuspension>
    {
        //
        // GET: /Suspension/

        public ActionResult Index()
        {

            DisplayBandeau();
            var toReturn = GetSuspensionList("", "", "");
            toReturn.PageTitle = "Périodes de suspension";
            toReturn.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            toReturn.Years = new List<AlbSelectListItem>();
            return View(toReturn);
        }

        [ErrorHandler]
        public ActionResult OpenVisualisationSuspension(string codeOffre, string version, string type, string numAve)
        {
            var model = GetSuspensionList(codeOffre, version, type);

            return PartialView("~/Views/Suspension/VisualisationSuspension.cshtml", model);
        }

        [ErrorHandler]
        public ActionResult RechercheSuspension(string codeOffre, string version)
        {
            var model = GetSuspensionList(codeOffre, version, string.Empty);

            return PartialView("~/Views/Suspension/ListVisuSuspension.cshtml", model.Suspensions);
        }

        [ErrorHandler]
        public ActionResult GetInfosContrat(string codeOffre, string version, string type)
        {
            var model = new ModeleVisuInfosContrat();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var commonOffreClient=client.Channel;
                model = (ModeleVisuInfosContrat)commonOffreClient.GetInfosContrat(codeOffre, version, type);

                return PartialView("~/Views/Suspension/VisuInfosContrat.cshtml", model);
            }
        }

        private static ModeleVisuSuspension GetSuspensionList(string codeOffre, string version, string type)
        {

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var commonOffreClient=client.Channel;
                var model = (ModeleVisuSuspension)commonOffreClient.GetVisuSuspension(codeOffre, version, type);

                return model;
            }
        }

    }
}
