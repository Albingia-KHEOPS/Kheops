using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Hexavia.App_Start
{
    public static class AppInsightConfig
    {
        public static void Configure() {
            Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["AppInsightInstrumentationKey"];
        }
    }
}