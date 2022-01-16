using System;
using System.Text;
using System.Web.Mvc;
using System.Net;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using System.Text.RegularExpressions;
using LightInject;
using LightInject.Web;

namespace ALBINGIA.Framework.Common
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class RollbackAttribute : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var indicator = PerWebRequestScopeManager.GetCurrentScope()?.GetInstance<ISuccessIndicator>();
            if (indicator != null) {
                indicator.ShouldCommit = false;
            }
        }

    }


}
