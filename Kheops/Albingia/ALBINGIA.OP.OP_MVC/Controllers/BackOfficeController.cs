using Albingia.Kheops.OP.Application.Port.Driver;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OPServiceContract.ISaisieCreationOffre;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class BackOfficeController : ControllersBase<ModeleBackOffice>
    {
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
          model.PageTitle = "Administration";
          model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
          DisplayBandeau();
          return View(model);
        }

        [ErrorHandler]
        public string DeleteOffre(string codeOffre, string version, string type)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext=client.Channel;
                return serviceContext.DeleteOffre(codeOffre, version, type);
            }
        }

        [ErrorHandler]
        public void RefreshAllCaches() {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IReferentielPort>()) {
                client.Channel.ResetAllCaches();
            }
        }
    }
}
