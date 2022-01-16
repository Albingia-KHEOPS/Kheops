using System;
using System.Text;
using System.Web.Mvc;
using System.Net;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using System.Text.RegularExpressions;
using ALBINGIA.Framework.Common.AlbingiaExceptions;

namespace ALBINGIA.Framework.Common
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    public class ErrorHandlerBaseAttribute : ActionFilterAttribute, IExceptionFilter
    {
        virtual protected bool HandleStandardRequestError(ExceptionContext context, string fullExp, string expMessage, string erreurParameters)
        {
            if (context.Exception is AlbFoncException foEx) {
                AlbSessionHelper.MessageErreurEcran = foEx.Message;
            }
            else {
                string erreurParametersUser = GetParamValues(context, true);

                AlbSessionHelper.MessageErreurEcran = "<b>Utilisateur :</b> "
                    + AlbSessionHelper.ConnectedUser + "<br/>"
                    + AlbOpConstants.OPENV_GENERAL_ERROR_MESSAGE + "<br/>"
                    + erreurParametersUser + "<br/>"
                    + expMessage;

                var techEx = new AlbTechException(context.Exception, erreurParameters: erreurParameters + fullExp);
            }
            if (context.HttpContext.Request.UrlReferrer != null)
            {
                if (context.HttpContext.Request.UrlReferrer.LocalPath == "/")
                {
                    return false;
                }
                context.Result = new RedirectResult(context.HttpContext.Request.UrlReferrer.AbsoluteUri);
            }
            return true;
        }

        virtual protected bool HandleAjaxRequestError(ExceptionContext context, string erreurParameters) {
            var message = context.Exception.Message;
            if (context.Exception is AlbingiaExceptions.AlbFoncException || context.Exception is BlacklistException) {
                message = context.Exception.Message;
            }
            else {
                switch (AlbOpConstants.ClientWorkEnvironment) {
                    case AlbOpConstants.OPENV_DEV:
                    case AlbOpConstants.OPENV_QUALIF:
                        message = "<b>Url</b> : " + erreurParameters + "<b>Message :</b>" + context.Exception.Message + "<br/><b>Source :</b> " + context.Exception.Source + "<br/><b>Stack Trace :</b> " + context.Exception.StackTrace;
                        new AlbingiaExceptions.AlbTechException(context.Exception, erreurParameters: erreurParameters);
                        break;
                    case AlbOpConstants.OPENV_PROD:
                        message = AlbOpConstants.OPENV_GENERAL_ERROR_MESSAGE;
                        new AlbingiaExceptions.AlbTechException(context.Exception, erreurParameters: erreurParameters);
                        break;
                }
            }

            context.Result = BuildAjaxErrorJson(message, context);
            return true;
        }

        public static string GetParamValues(ControllerContext context, bool replaceId = false)
        {
            if (context.Controller.ControllerContext.RequestContext == null || context.Controller.ControllerContext.RequestContext.RouteData == null)
            {
                return string.Empty;
            }
            var ctxValues = context.Controller.ControllerContext.RequestContext.RouteData.Values;
            var strProvider = new StringBuilder();
            foreach (var value in ctxValues)
            {
                var key = value.Key;
                var val = value.Value.ToString();
                if (replaceId && string.Compare(value.Key, "id", true) == 0)
                {
                    val = Regex.Replace(value.Value.ToString(), @"([A-Z0-9]+)_(\d+)_.*", "$1 - $2");
                    key = "Id Contrat";
                }
                strProvider.Append($"<b>{key}</b> = {val} <br>");
            }
            return strProvider.Length == 0 ? string.Empty : strProvider.ToString();
        }

        protected static JsonResult BuildAjaxErrorJson(string message, ExceptionContext filterContext)
        {
            //Metre le code statut à 500
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //Définir un message générique si le message envoyé est null
            if (string.IsNullOrEmpty(message))
            {
                message = "<b>Utilisateur :</b> " + AlbSessionHelper.ConnectedUser + "Erreur non gérée. Veuillez contactez votre administrateur";
            }
            //Code utilisée pour IIS 7
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            //Formater le retour serveur (message, code erreur) sous format de JsonResult
            return new JsonResult
            {
                Data = new { ErrorMessage = message },
                ContentEncoding = Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            };
        }

        public void OnException(ExceptionContext context)
        {
            try
            {
                string fullExp = Regex.Replace(System.Web.HttpUtility.HtmlEncode(context.Exception.ToString()), "\r?\n", "<br/>");
                string expMessage = Regex.Replace(System.Web.HttpUtility.HtmlEncode(context.Exception.Message), "\r?\n", "<br/>");
                string erreurParameters = GetParamValues(context);

                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    context.ExceptionHandled = HandleAjaxRequestError(context, erreurParameters); ;
                }
                else
                {
                    context.ExceptionHandled = HandleStandardRequestError(context, fullExp, expMessage, erreurParameters);
                }

            }
            catch
            {
                context.ExceptionHandled = false;
                throw new Exception("Unable to handle error. For futher info, look up inner exception", context?.Exception);
            }
        }
    }
}
