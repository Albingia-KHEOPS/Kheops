using ALBINGIA.Framework.Common.Business;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace ALBINGIA.OP.OP_MVC
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
            if (System.Web.HttpContext.Current.Session.IsNewSession) {
                filterContext.Result = new JavaScriptResult {
                    Script = "window.location.replace(\"/RechercheSaisie/Index\");"
                };
                return;

            }

            if (!(filterContext.Result is RedirectToRouteResult result) || !filterContext.HttpContext.Request.IsAjaxRequest()) {
                base.OnActionExecuted(filterContext);
                return;
            }

            IDictionary<string, object> values = (((RedirectToRouteResult)(filterContext.Result))).RouteValues;
            var redirectObject = new AlbRedirectObject();

            foreach (var value in values)
            {
                if (value.Key == "returnHome" && value.Value.ToString().ToLower() == "1")
                {
                    redirectObject.ToHome = true;
                }

                switch (value.Key)
                {
                    case "action":
                        redirectObject.Action = value.Value.ToString();
                        break;
                    case "controller":
                        redirectObject.Controller = value.Value.ToString();
                        break;
                    default:
                        SetParams(filterContext, redirectObject, value);
                        break;
                }
            }

            if (redirectObject.ToHome)
            {
                SetHomeAction(redirectObject);
                FolderController.DeverrouillerAffaire(redirectObject.GuidTab);
            }

            // Redirection Information génèrale avenant
            var screenType = InformationGeneraleTransverse.GetScreenType(redirectObject.QuerystringId);
            if ((AlbConstantesMetiers.SCREEN_TYPE_AVNMD == screenType
                  || AlbConstantesMetiers.SCREEN_TYPE_AVNRS == screenType 
                  || AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR == screenType)
                && redirectObject.Controller.ToLower() == "aninformationsgenerales")
            {
                redirectObject.Controller = "AvenantInfoGenerales";
            }

            var url = new UrlHelper(filterContext.RequestContext);
            string querystring = redirectObject.BuildQuerystring().NullIfEmpty();

            var redirectTargetDictionary = new RouteValueDictionary { {"action", redirectObject.Action + (querystring is null ? string.Empty : ("/" + querystring)) }, {"controller", redirectObject.Controller } };

            string javascript = null;
            if (redirectObject.CreateSubmitter)
            {
                javascript = $"$(\"#redirectForm\").attr('action', '/{redirectObject.Controller}/{redirectObject.Action}'); $(\"#redirectForm\").submit();";
            }
            else if (redirectObject.IsNewWindow) {
                javascript = $"OpenWindowWithPost('http://{filterContext.HttpContext.Request.Url.Host} ', '__newtab', 'NewWINFRMGen','newWindow{url.RouteUrl(redirectTargetDictionary)}')";
            }
            else {
                javascript = $"window.location.replace(\"/{redirectObject.Controller}/{redirectObject.Action}{(querystring is null ? string.Empty : ("/" + querystring))}\");";
            }

            filterContext.Result = new JavaScriptResult {
                Script = javascript
            };
        }

        private static void SetParams(ActionExecutedContext filterContext, AlbRedirectObject redirectObject, KeyValuePair<string, object> value)
        {
            switch (value.Key.ToUpper()) {
                case "ACTIONENGAGEMENT":
                    redirectObject.AccessMode = Enum.TryParse(value.Value?.ToString() ?? string.Empty, true, out AccessMode mode) ? mode : 0;
                    redirectObject.Querystring.Add($"{nameof(AccessMode)}={redirectObject.AccessMode}");
                    break;
                case "GUIDTAB":
                    redirectObject.GuidTab = (string)value.Value;
                    break;
                case "SUBMIT":
                    redirectObject.CreateSubmitter = true;
                    break;
                case "NEWWINDOW":
                    if ((string)value.Value == "newWindow") {
                        redirectObject.IsNewWindow = true;
                    }
                    break;
                case "ID":
                    if (value.Value.ToString().Contains("newWindow")) {
                        redirectObject.IsNewWindow = true;
                    }
                    redirectObject.QuerystringId = filterContext.HttpContext.Server.UrlEncode(value.Value.ToString().Replace("_newWindow", string.Empty));
                    break;
            }
        }

        private static void SetHomeAction(AlbRedirectObject redirectObject)
        {
            redirectObject.Action = "Index";
            redirectObject.Controller = "RechercheSaisie";
            redirectObject.QuerystringId = string.Empty;
            redirectObject.Querystring.Clear();
        }
    }
}
