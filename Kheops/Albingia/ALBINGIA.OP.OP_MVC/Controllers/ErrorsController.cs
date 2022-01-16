using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.OP.OP_MVC.Models.ModeleTransverse;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ErrorsController : ControllersBase<ErreursPages>
    {
        public ActionResult Index(string id)
        {
          model.ErrorType = id;
          if (id.Contains(Framework.Common.Constants.AlbOpConstants.REDIRECT_ACCESDENIED_ERROR))
          {
            AlbSessionHelper.DelCookie("ASP.NET_SessionId");
          }
          return View(model);
        }
    }
}
