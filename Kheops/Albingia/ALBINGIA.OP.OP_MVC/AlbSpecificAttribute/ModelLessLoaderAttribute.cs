using Albingia.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Albingia.Kheops.Mvc {
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ModelLessLoaderAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (filterContext.Controller is IModelLessController controller) {
                if (filterContext.HttpContext.Request.Headers.Get(nameof(controller.TabGuid).ToUpper()) is string guid) {
                    controller.TabGuid = Guid.TryParse(guid, out var g) ? g : default;
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}