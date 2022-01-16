using System;
using System.Text;
using System.Web.Mvc;
using System.Net;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using System.Text.RegularExpressions;
using ALBINGIA.Framework.Common.AlbingiaExceptions;

namespace ALBINGIA.Framework.Common {
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    public class ErrorMessageHandlerAttribute : ActionFilterAttribute, IExceptionFilter {
        private static string HandleStandardRequestError(ExceptionContext context) {
            string fullExp = Regex.Replace(System.Web.HttpUtility.HtmlEncode(context.Exception.ToString()), "\r?\n", "<br/>");
            string expMessage = Regex.Replace(System.Web.HttpUtility.HtmlEncode(context.Exception.Message), "\r?\n", "<br/>");
            string erreurParameters = GetParamValues(context);
            if (context.Exception is AlbFoncException) {
                return $"<p>{ expMessage }</p>";
            }
            else
            {

                string erreurParametersUser = GetParamValues(context, true);

                return "<b>Utilisateur :</b> "
                    + AlbSessionHelper.ConnectedUser + "<br/>"
                    + AlbOpConstants.OPENV_GENERAL_ERROR_MESSAGE + "<br/>"
                    + erreurParametersUser + "<br/>"
                    + expMessage;
            }
        }
        

        private static string GetParamValues(ControllerContext context, bool replaceId = false) {
            if (context.Controller.ControllerContext.RequestContext == null || context.Controller.ControllerContext.RequestContext.RouteData == null) {
                return string.Empty;
            }
            var ctxValues = context.Controller.ControllerContext.RequestContext.RouteData.Values;
            var strProvider = new StringBuilder();
            foreach (var value in ctxValues) {
                var key = value.Key;
                var val = value.Value.ToString();
                if (replaceId && string.Compare(value.Key, "id", true) == 0) {
                    val = Regex.Replace(value.Value.ToString(), @"([A-Z0-9]+)_(\d+)_.*", "$1 - $2");
                    key = "Id Contrat";
                }
                strProvider.Append($"<b>{key}</b> = {val} <br>");
            }
            return strProvider.Length == 0 ? string.Empty : strProvider.ToString();
        }


        public void OnException(ExceptionContext context) {
            context.ExceptionHandled = false;

            try
            {
                var message = HandleStandardRequestError(context);
                if (message != null)
                {
                    context.Result = new PartialViewResult
                    {
                        ViewName = "ErrorMessage",
                        ViewData = new ViewDataDictionary { ["message"] = message, ["error"] = context.Exception },
                        TempData = null
                    };
                }

                context.ExceptionHandled = true;
                context.HttpContext.Response.StatusCode = 500;
            }
            catch {
            }
        }
    }
}
