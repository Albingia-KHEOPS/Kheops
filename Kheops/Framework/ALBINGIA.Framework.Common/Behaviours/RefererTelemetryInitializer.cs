namespace ALBINGIA.Framework.Common
{
    using System.Linq;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;

    public class RefererTelemetryInitializer : ITelemetryInitializer
    {
        private const string HeaderNameDefault = "Referer";


        public RefererTelemetryInitializer()
        {
        }

        public void Initialize(ITelemetry telemetry)
        {
            var context = System.Web.HttpContext.Current;
            if (context == null) { return; }

            if (context.Request.Headers.AllKeys.Any(x=>x == HeaderNameDefault))
            {
                var value = context.Request.Headers.Get(HeaderNameDefault);
                telemetry.Context.Properties["Referer"] = value.ToString();
            }
            if (context.User == null) { return; }
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated) {
                telemetry.Context.User.AuthenticatedUserId = System.Web.HttpContext.Current.User.Identity.Name;
            }
        }
    }
}
