using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;


public class Initializer
{
    public static void AppInitialize()
    {
        Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["AppInsightInstrumntationKey"];
        Microsoft.ApplicationInsights.Extensibility.Implementation.TelemetryDebugWriter.IsTracingDisabled = ConfigurationManager.AppSettings["AppInsightsTraceDebug"]?.ToLower() == "false";

    }
}