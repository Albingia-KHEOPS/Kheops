using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ALBINGIA.Framework.Common.Tools {
    public static class MvcHelper {
        /// <summary>
        /// Decode les paramètres de l'url
        /// </summary>
        /// <param name="filterContext">Contexte controller</param>
        public static void DecodeUrlParams(this ActionExecutingContext filterContext) {
            var keyList = filterContext.RouteData.Values.Keys.Where(k => !k.IsIn("controller", "action")).ToArray();
            foreach (var key in keyList) {
                filterContext.RouteData.Values[key] = filterContext.HttpContext.Server.UrlDecode(filterContext.RouteData.Values[key] as string);
                if (filterContext.ActionParameters.ContainsKey(key)) {
                    filterContext.ActionParameters[key] = filterContext.RouteData.Values[key];
                }
            }
        }

        public static string ControllerName(this Type controllerType) {
            if (controllerType is null) {
                return string.Empty;
            }
            if (!controllerType.IsSubclassOf(typeof(Controller))) {
                throw new ArgumentException($"Must be a sub-class of {typeof(Controller).FullName}", nameof(controllerType));
            }
            if (controllerType.Name == nameof(Controller)) {
                return nameof(Controller);
            }
            var a = controllerType.Name.Split(new[] { nameof(Controller) }, StringSplitOptions.None);
            return a.Length == 1
                ? controllerType.Name
                : string.Join(nameof(Controller), a.TakeWhile((x, i) => (i + 1) < a.Length || x.Length > 0));
        }
    }
}
