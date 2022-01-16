using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class NavigationController : ControllersBase<Navigation_MetaModel>
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(NavigationController));
        public ActionResult Index()
        {
            model.PageTitle = "Navigation";
            return View("Navigation",model);
        }
    }
}
