using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ALBINGIA.Framework.Common.Extensions
{
    

    public class AlbAjaxRedirect : RedirectResult
    {
        #region variables membres

        private string _url;

        #endregion

        public AlbAjaxRedirect(string url)
            : base(url)
        {
            _url = url;
        }

        public AlbAjaxRedirect(string url, bool permanent)
            : base(url, permanent)
        {
            _url = url;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (!context.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                base.ExecuteResult(context);
                return;
            }
            var urlCible = UrlHelper.GenerateContentUrl(_url, context.HttpContext);
            var result = new JavaScriptResult { Script = "window.location = '" + urlCible + "';" };
            result.ExecuteResult(context);
        }
    }
}
