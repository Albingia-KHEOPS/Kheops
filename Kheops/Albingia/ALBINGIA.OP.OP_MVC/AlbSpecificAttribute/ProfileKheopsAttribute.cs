using ALBINGIA.Framework.Common.ServiceFactory;
using ALBINGIA.OP.OP_MVC.Common;
using OPServiceContract.ICommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.AlbSpecificAttribute {
    public class ProfileKheopsAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (filterContext?.Controller is IMetaModelsController controller && controller.Model != null) {
                using (var client = ServiceClientFactory.GetClient<ICommonOffre>()) {
                    controller.Model.ProfileKheops = client.Channel.GetProfileKheops();
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}