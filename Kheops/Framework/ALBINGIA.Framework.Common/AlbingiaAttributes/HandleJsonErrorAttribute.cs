using LightInject;
using LightInject.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.Web.Mvc;

namespace ALBINGIA.Framework.Common {
    public class HandleJsonErrorAttribute: HandleErrorAttribute {
        public override void OnException(ExceptionContext filterContext) {
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            filterContext.Result = new ContentResult {
                ContentType = "application/json",
                Content = BuildJsonError(filterContext)
            };
            filterContext.ExceptionHandled = true;
        }

        public static JObject BuildJObjectError(Exception exception) {
            if (exception is null) {
                return null;
            }
            var exType = exception.GetType();
            Type errorType = null;
            IDictionary<string, object> values = new Dictionary<string, object> {
                { nameof(exception.Message), exception.Message },
                { nameof(exception.StackTrace), exception.StackTrace }
            };
            if (exception is FaultException && exType.GetGenericArguments().Any()) {
                errorType = exType.GetGenericArguments().First();
                var handledProperties = errorType
                    .GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(HandledByMvcErrorAttribute)));
                var detail = exType.InvokeMember("Detail", BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance, null, exception, null);
                handledProperties.ToDictionary(p => p.Name, p => p.GetValue(detail)).ToList().ForEach(kv => values.Add(kv));
            }
            else {
                errorType = exType;
                var handledProperties = errorType
                    .GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(HandledByMvcErrorAttribute)));
                handledProperties.ToDictionary(p => p.Name, p => p.GetValue(exception)).ToList().ForEach(kv => values.Add(kv));
            }
            values.Add("$type", errorType.FullName);
            JsonSerializer j = new JsonSerializer();
            j.Converters.Add(new StringEnumConverter());
            return JObject.FromObject(values, j);
        }

        protected virtual string BuildJsonError(ExceptionContext filterContext) {
            return BuildError(filterContext).ToString();
        }

        private JObject BuildError(ExceptionContext filterContext) {
            return BuildJObjectError(filterContext.Exception);
        }
    }
}
