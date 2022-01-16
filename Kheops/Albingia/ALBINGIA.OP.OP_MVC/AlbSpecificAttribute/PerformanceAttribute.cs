using ALBINGIA.Framework.Common.CacheTools;
using OPServiceContract.ICommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.AlbSpecificAttribute
{


    public class PerformanceAttribute : ActionFilterAttribute
    {


        private readonly Stopwatch _stopwatch;
        private  DateTime _startDate ;

        public PerformanceAttribute()
        {
            _stopwatch = new Stopwatch();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _stopwatch.Reset();
            _stopwatch.Start();
            _startDate = DateTime.Now;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
       {
            _stopwatch.Stop();

            try
            {
                TraceAction(filterContext);
            }
            catch (Exception)
            {
            }
        }


        public void TraceAction(ActionExecutedContext filterContext) {

            var httpContext = filterContext.HttpContext;
            var request = httpContext.Request;
            var response = httpContext.Response;
            var elapsed = _stopwatch.Elapsed.ToString();


            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var commonOffreClient=client.Channel;
                int? idSessionLog = (int?)httpContext.Session["ID_SESSION_LOG"];



                var codeOffre = string.Empty;
                var type = string.Empty;
                var version = string.Empty;
                var userName = string.Empty;
                var tabGuid = string.Empty;


                if (request.Headers.AllKeys.Any(i => i == "x-tab-guid"))
                    tabGuid = request.Headers.GetValues("x-tab-guid")[0];

                else if(filterContext.RouteData.Values["id"] != null)
                    tabGuid = ExtractTabGuid(filterContext.RouteData.Values["id"].ToString());

                if (!string.IsNullOrEmpty(tabGuid))
                {
                    var info = AlbSessionHelper.GetFolderByTabGuid(tabGuid.Split(new string[] { "tabGuid" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                    if (info != null)
                    {
                        var splits = info.Id.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                        codeOffre = info.Folder;
                        type = splits[4];
                        version = splits[3];
                    }
                }

                userName = AlbSessionHelper.ConnectedUser;

                if (!filterContext.ActionDescriptor.ActionName.Contains("DeverouillerUserOffre"))
                {
                    idSessionLog = commonOffreClient.SetTraceLog(codeOffre,
                                                                 string.IsNullOrEmpty(version) ? "0" : version.Trim(),
                                                                 type,
                                                                 idSessionLog ?? 0,
                                                                 "KHEPER",
                                                                 userName,
                                                                 _startDate.ToString(CultureInfo.InvariantCulture),
                                                                 string.Format("{0}_{1}_{2} seconds", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName, _stopwatch.Elapsed.ToString(@"ss\.ff")));
                }
                httpContext.Session["ID_SESSION_LOG"] = idSessionLog;

            }
        }

        private string ExtractTabGuid(string id)
        {
            var tabGuid = string.Empty;

            var regex = new Regex(@"tabGuid([A-Za-z0-9\-]+)tabGuid");
            var match = regex.Match(id);
            var tabGuids = new List<string>();

            if (match.Success)
            {
                tabGuid = match.Groups[1].Value.Trim();
            }

            return tabGuid;
        }

    }
}