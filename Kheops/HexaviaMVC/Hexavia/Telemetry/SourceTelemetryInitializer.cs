using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using System.Web;

namespace Hexavia.Telemetry
{


    public class SourceTelemetryInitializer : ITelemetryInitializer
    {
        //static string version = Assembly.GetEntryAssembly().GetName().Version.ToString();
        public void Initialize(Microsoft.ApplicationInsights.Channel.ITelemetry telemetry)
        {

            telemetry.Context.User.AuthenticatedUserId = HttpContext.Current.User.Identity.Name;
            return;
        }
    }
}
