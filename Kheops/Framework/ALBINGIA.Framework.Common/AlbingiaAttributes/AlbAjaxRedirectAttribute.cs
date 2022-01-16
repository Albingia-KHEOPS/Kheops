using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using ALBINGIA.OP.OP_MVC.Common;

namespace ALBINGIA.Framework.Common.AlbingiaAttributes
{
    /// <summary>
    /// Cet attribut peut 'applique à uneclasse ou une méthode.
    /// Cette attribut sert a indiquer que la classe ou la méthode peut traiter une redirection cas d'un 
    /// appel Ajax
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AlbAjaxRedirectAttribute : ActionFilterAttribute
    {

        /// <summary>
        /// Traite la redirection Ajax aprés que l'action soit executé
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as RedirectToRouteResult;
            if (result == null || !filterContext.HttpContext.Request.IsAjaxRequest())
            {
                base.OnActionExecuted(filterContext);
                return;
            }
            var values = (((RedirectToRouteResult)(filterContext.Result))).RouteValues.ToList();
            var controller = string.Empty;
            var action = string.Empty;
            var strProvider = new StringBuilder();
            var guidTab = string.Empty;
            bool toHome = false;
            
            foreach (var value in values)
            {
                if (value.Key == "returnHome" && value.Value.ToString().ToLower() == "1")
                {
                    //action = "Index";
                    //controller = "RechercheSaisie";
                    toHome = true;
                    //break;
                }
                switch (value.Key)
                {
                    case "action":
                        action = value.Value.ToString();
                        break;
                    case "controller":
                        controller = value.Value.ToString();
                        break;
                    default:

                        if (value.Key == "guidTab")
                            guidTab = (string)value.Value;
                        else
                        {
                            strProvider.Append("/" + filterContext.HttpContext.Server.UrlEncode(value.Value.ToString()));
                            //strProvider.Append("/" + value.Value.ToString().Replace("'", "\'"));
                        }

                        break;
                }
            }
            if (toHome)
            {
                if (InformationGeneraleTransverse.GetTraceAssiette())
                {
                    action = "Index";
                    controller = "Cotisation";
                }
                else
                {
                    action = "Index";
                    controller = "RechercheSaisie";
                    strProvider.Clear();
                }
            }
            var url = new UrlHelper(filterContext.RequestContext);

            var redirectTargetDictionary = new RouteValueDictionary
             {
               {"action", action + strProvider+guidTab},
               {"controller", controller}
             };


            filterContext.Result = new JavaScriptResult
             {
                 //Script = "window.location='" + filterContext.HttpContext.Server.UrlEncode(url.RouteUrl(redirectTargetDictionary)) + "'"//.Replace("Metier.html?", string.Empty) + "'"
                 Script = "window.location='" + url.RouteUrl(redirectTargetDictionary) + "'"//.Replace("Metier.html?", string.Empty) + "'"

             };
        }
    }


}
