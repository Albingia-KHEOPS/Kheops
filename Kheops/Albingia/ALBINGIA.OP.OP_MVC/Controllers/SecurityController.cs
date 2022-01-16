using ALBINGIA.Framework.Common.CacheTools;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class SecurityController : Controller
    {
        #region Properties
        public string UserId
        {
            get
            {
                return AlbSessionHelper.ConnectedUser;
            }
        }
        #endregion

        public ActionResult Index(string id)
        {
            return base.RedirectToAction("Index", "RechercheSaisie", new { id });
        }
       
    }
}