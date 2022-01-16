using System;
using System.Web.Mvc;
using System.IO;

namespace ALBINGIA.OP.OP_MVC.Helpers
{

    public class AutoReadOnlyViewResult : ViewResult
    {
        private bool isReadOnly;
        private bool isModifHorsAvenant;

        public AutoReadOnlyViewResult(bool isReadOnly, bool isModifHorsAvenant) {
            this.isReadOnly = isReadOnly;
            this.isModifHorsAvenant = isModifHorsAvenant;
        }

        public override void ExecuteResult(ControllerContext context) {
            if (context == null) {
                throw new ArgumentNullException("context");
            }
            if (string.IsNullOrEmpty(ViewName)) {
                ViewName = context.RouteData.GetRequiredString("action");
            }
            ViewEngineResult viewEngineResult = null;
            if (View == null) {
                viewEngineResult = FindView(context);
                View = viewEngineResult.View;
            }
            TextWriter output = context.HttpContext.Response.Output;

            using (var sw = new StringWriter()) {

                ViewContext viewContext = new ViewContext(context, View, ViewData, TempData, sw);
                View.Render(viewContext, sw);
                output.Write(ReadOnlyRewiter.FormatHtmlToReadonly(sw.ToString(), isReadOnly, isModifHorsAvenant));
            }
            viewEngineResult?.ViewEngine.ReleaseView(context, View);
        }

    }
}


