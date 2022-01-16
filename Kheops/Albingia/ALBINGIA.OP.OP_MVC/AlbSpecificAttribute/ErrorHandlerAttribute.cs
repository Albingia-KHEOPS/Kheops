using System.Linq;
using System.Web.Mvc;
using Albingia.Kheops.Common;
using Albingia.Kheops.Mvc;
using ALBINGIA.Framework.Common;

namespace ALBINGIA.OP.OP_MVC {
    public class ErrorHandlerAttribute : ErrorHandlerBaseAttribute {
        protected override bool HandleAjaxRequestError(ExceptionContext context, string erreurParameters) {
            if (context.Exception is BusinessValidationException bex && (bex.Errors?.Any(e => e.Error.StartsWith(CheckVerrouAttribute.SessionLost)) ?? false)) {
                context.Result = BuildAjaxErrorJson(bex.Errors.First().Error, context);
                return true;
            }
            return base.HandleAjaxRequestError(context, erreurParameters);
        }
    }
}