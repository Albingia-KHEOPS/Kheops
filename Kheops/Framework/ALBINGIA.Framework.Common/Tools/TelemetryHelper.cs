using ALBINGIA.Framework.Common.IOFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Tools
{
    public class TelemetryHelper
    {
        public static string GetApp()
        {
            var req = "Unknown";

            var config = FileContentManager.GetConfigValue("InternalApplicationId");
            if (!String.IsNullOrWhiteSpace(config))
            {
                return config;
            }
            return req;
        }

        public static string GetEnv()
        {
            var req = "Unknown";

            var config = FileContentManager.GetConfigValue("EnvironnementDeploiment");
            if (!String.IsNullOrWhiteSpace(config)) {
                return config;
            }

            if (System.Web.HttpContext.Current != null)
            {
                return GetEnvFromContext();
            }

            if (System.ServiceModel.OperationContext.Current != null)
            {
                return GetEnvFromOpContext();
            }
            return req;
        }

        private static string GetEnvFromContext()
        {
            var req = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            req = Trunk(req);
            return req;
        }

        private static string GetEnvFromOpContext()
        {
            var req = System.ServiceModel.OperationContext.Current.InstanceContext.Host.BaseAddresses.First().ToString();
            if (req.StartsWith("http://") || req.StartsWith("https://"))
            {
                req = req.Substring(0, req.IndexOf('/', req.IndexOf('/', 9)+1));
            }
            return req;
        }
        private static string Trunk(string req)
        {
            if (req.StartsWith("http://") || req.StartsWith("https://"))
            {
                req = req.Substring(0, req.IndexOf('/', 9, req.IndexOf('/', 9) + 1));
            }

            return req;
        }
    }
}
