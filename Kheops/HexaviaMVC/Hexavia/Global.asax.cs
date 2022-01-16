using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Hexavia.Business;
using log4net;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using System.Web.Routing;
using Hexavia.App_Start;

namespace Hexavia
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MvcApplication));

        private static IContainer Container { get; set; }

        protected void Application_Start()
        {
            Logger.InfoFormat(CultureInfo.InvariantCulture, "Application starting...");

            GlobalConfiguration.Configure(WebApiConfig.Register);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AppInsightConfig.Configure();


            var builder = new ContainerBuilder();

            // Register your MVC controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);

            // Register modules
            builder.RegisterModule(new BusinessModule());

            Container = builder.Build();
            var autofacDependencyResolver = new AutofacDependencyResolver(Container);
            var autofacWebApiDependencyResolver = new AutofacWebApiDependencyResolver(Container);
            DependencyResolver.SetResolver(autofacDependencyResolver);
            GlobalConfiguration.Configuration.DependencyResolver = autofacWebApiDependencyResolver;

            Logger.InfoFormat(CultureInfo.InvariantCulture, "Application started");
        }
    }
}
