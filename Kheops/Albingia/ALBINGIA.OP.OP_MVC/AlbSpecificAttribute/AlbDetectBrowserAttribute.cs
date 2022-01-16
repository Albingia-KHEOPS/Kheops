using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ALBINGIA.OP.OP_MVC.AlbSpecificAttribute
{
    /// <summary>
    /// Cet attribut peut 'applique à uneclasse ou une méthode.
    /// Cette attribut sert a indiquer que la classe ou la méthode peut traiter une redirection cas d'un 
    /// appel Ajax
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AlbDetectBrowserAttribute : ActionFilterAttribute
    {


        /// <summary>
        /// Vérification du navigateur Web 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (AlbOpConstants.ClientWorkEnvironment == AlbOpConstants.OPENV_DEV || AlbOpConstants.ClientWorkEnvironment == AlbOpConstants.OPENV_QUALIF)
            //{
            //  base.OnActionExecuting(filterContext);
            //  return;
            //}
            if (filterContext.HttpContext.Request.Url != null &&
                filterContext.HttpContext.Request.Url.AbsolutePath.ToLower().Contains("errors"))
            {
            //    base.OnActionExecuting(filterContext);
            //    return;
                VerifCache(filterContext);
                return;
            }

            var browser = filterContext.HttpContext.Request.Browser;
            if (Framework.Common.Constants.AlbOpConstants.OtherBrowsers && !(browser.Type.ToLower().Contains("ie") || browser.Type.ToLower().Contains("internetexplorer11")))
            {
                //base.OnActionExecuting(filterContext);
                //return;
                VerifCache(filterContext);
                return;
            }

            var userAgent = filterContext.HttpContext.Request.UserAgent;

            if (userAgent == null || userAgent.ToLower().Contains("trident/5.0") ||
                userAgent.ToLower().Contains("trident/6.0") || userAgent.ToLower().Contains("trident/7.0") || userAgent.ToLower().Contains("edge/13"))
            {
                //base.OnActionExecuting(filterContext);
                //return;
                VerifCache(filterContext);
                return;
            }
            if (browser.Type.ToLower().Contains("ie7") || browser.Type.ToLower().Contains("ie8") ||
                browser.Type.ToLower().Contains("ie9") || browser.Type.ToLower().Contains("ie10") || browser.Type.ToLower().Contains("internetexplorer11"))
            {
                //base.OnActionExecuting(filterContext);
                //return;
                VerifCache(filterContext);
                return;
            }
            if (browser.Type.ToLower().Contains("internetexplorer") || browser.Type.ToLower().Contains("ie") || browser.Type.ToLower().Contains("internetexplorer11"))
            {
                if (
                    Decimal.TryParse(browser.Version, NumberStyles.Number,
                        new NumberFormatInfo { NumberDecimalSeparator = "." }, out decimal version) && version <= 11)
                {
                    //base.OnActionExecuting(filterContext);
                    //return;
                    VerifCache(filterContext);
                    return;
                }

            }


            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JavaScriptResult
                {
                    Script =
                        "window.location='" + "/Errors/Index/" +
                        Framework.Common.Constants.AlbOpConstants.REDIRECT_BROWSER_ERROR +
                        "'"
                };
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    action = "Index" + "/" + Framework.Common.Constants.AlbOpConstants.REDIRECT_BROWSER_ERROR,
                    controller = "Errors",
                }));
            }


        }


         private void VerifCache(ActionExecutingContext filterContext)
        {
            //var request = filterContext.RequestContext.HttpContext.Request;
            //var response = filterContext.RequestContext.HttpContext.Response;
            //if (request.Headers["If-Modified-Since"] != null &&
            //TimeSpan.FromTicks(DateTime.Now.Ticks - DateTime.Parse(request.Headers["If-Modified-Since"]).Ticks).Seconds < 3600)
            //{
            //    response.Write(DateTime.Now);
            //    response.StatusCode = 304;
            //    response.Headers.Add("Content-Encoding", "gzip");
            //    response.StatusDescription = "Not Modified";
            //}
            //else
            //{
                base.OnActionExecuting(filterContext);
            //}
        }

         private void SetCaching(HttpResponseBase response)
         {
            // response.AddFileDependency(fileName);
             response.Cache.SetETagFromFileDependencies();
             response.Cache.SetLastModifiedFromFileDependencies();
             response.Cache.SetCacheability(HttpCacheability.Public);
             response.Cache.SetMaxAge(new TimeSpan(1, 0, 0, 0));
             response.Cache.SetSlidingExpiration(true);
         }

         public override void OnActionExecuted(ActionExecutedContext filterContext)
         {
             //var result = filterContext.Result as view;
             //if (result != null)
             //{
             //    if (!string.IsNullOrEmpty(result.FileName) && (System.IO.File.Exists(result.FileName)))
                    // SetCaching(filterContext.HttpContext.Response);
             //}
           //  base.OnActionExecuted(filterContext);
         }


    }
}


//     protected override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            var request = filterContext.RequestContext.HttpContext.Request;
//            var response = filterContext.RequestContext.HttpContext.Response;
//            if (request.Headers["If-Modified-Since"] != null &&
//            TimeSpan.FromTicks(DateTime.Now.Ticks - DateTime.Parse(request.Headers["If-Modified-Since"]).Ticks).Seconds < 3600)
//            {
//                response.Write(DateTime.Now);
//                response.StatusCode = 304;
//                response.Headers.Add("Content-Encoding", "gzip");
//                response.StatusDescription = "Not Modified";
//            }
//            else
//            {
//                base.OnActionExecuting(filterContext);
//            }
//        }

//        private void SetFileCaching(HttpResponseBase response, string fileName)
//        {
//            response.AddFileDependency(fileName);
//            response.Cache.SetETagFromFileDependencies();
//            response.Cache.SetLastModifiedFromFileDependencies();
//            response.Cache.SetCacheability(HttpCacheability.Public);
//            response.Cache.SetMaxAge(new TimeSpan(7, 0, 0, 0));
//            response.Cache.SetSlidingExpiration(true);
//        }

//        protected override void OnActionExecuted(ActionExecutedContext filterContext)
//        {
//            var result = filterContext.Result as FilePathResult;
//            if (result != null)
//            {
//                if (!string.IsNullOrEmpty(result.FileName) && (System.IO.File.Exists(result.FileName)))
//                    SetFileCaching(filterContext.HttpContext.Response, result.FileName);
//            }
//            base.OnActionExecuted(filterContext);
//        }


//}
