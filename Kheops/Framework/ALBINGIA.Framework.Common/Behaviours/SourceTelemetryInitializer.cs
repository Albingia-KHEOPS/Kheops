using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Tools;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Web;

namespace ALBINGIA.Framework.Common
{
    public class SourceTelemetryInitializer : ITelemetryInitializer
    {
        //static string version = Assembly.GetEntryAssembly().GetName().Version.ToString();
        public void Initialize(Microsoft.ApplicationInsights.Channel.ITelemetry telemetry)
        {
            var env = TelemetryHelper.GetEnv();
            if (!telemetry.Context.Properties.ContainsKey("Source"))
            {
                telemetry.Context.Properties.Add("Source", env);
                telemetry.Context.Cloud.RoleName = TelemetryHelper.GetApp();
            }


            if ( !string.IsNullOrWhiteSpace( AlbSessionHelper.As400User))
            {
                telemetry.Context.User.AuthenticatedUserId = AlbSessionHelper.As400User;
                return;
            }

            int? i = null;
            try
            {
                i=OperationContext.Current?.IncomingMessageHeaders.FindHeader("User", "http://albingia.fr/kheops/2018");
            }
            catch (ObjectDisposedException) { }
            if (i.HasValue && i>= 0)
            {
                var user = OperationContext.Current?.IncomingMessageHeaders.GetHeader<string>(i.Value);
                telemetry.Context.User.AuthenticatedUserId = user;
                return;
            }

            telemetry.Context.User.AuthenticatedUserId = AlbSessionHelper.UserLogin;
            return;
        }
    }
}