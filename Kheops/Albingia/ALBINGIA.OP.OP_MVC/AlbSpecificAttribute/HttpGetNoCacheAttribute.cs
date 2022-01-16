using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.AlbSpecificAttribute {
    public class HttpGetNoCacheAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (filterContext.Controller != null && filterContext.RequestContext.HttpContext?.Request.HttpMethod.ToUpper() == "GET") {
                var response = filterContext.RequestContext.HttpContext.Response;
                response.Cache.SetCacheability(HttpCacheability.NoCache);
                response.Cache.AppendCacheExtension("no-store, must-revalidate");
                response.AppendHeader("Pragma", "no-cache");
                response.AppendHeader("Expires", "0");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}