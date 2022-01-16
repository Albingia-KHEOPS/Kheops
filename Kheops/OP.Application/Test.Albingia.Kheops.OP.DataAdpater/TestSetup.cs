using Albingia.Kheops.OP.Application.Infrastructure;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.DataAdapter;
using Albingia.Kheops.OP.Infrastructure;
using Albingia.Kheops.OP.Service;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Data;
using LightInject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Test.Albingia.Kheops.OP.DataAdapter;

namespace Test.Albingia.Kheops.OP.DataAdpater
{
    class TestSetup
    {
        public static IServiceContainer AppDomainSetup()
        {
            var serviceRegistry = new LightInject.ServiceContainer();
            serviceRegistry.Register<IConfig, Config>();
            serviceRegistry.Register<ObjectCache>(s => new MemoryCache(new Random().Next().ToString(), null));
            serviceRegistry.Register<IGenericCache, Cache>(new PerContainerLifetime());
            serviceRegistry.Register<ISessionContext, SessionContext>(new PerContainerLifetime());
            serviceRegistry.Register<IDbConnection>(s => ConnectionFactory(s), new PerContainerLifetime());
            serviceRegistry.Register<ISuccessIndicator, FalseSuccesIndicator>();


            Action<Type> register = x => RegisterRepository(serviceRegistry, x);
            typeof(AffaireRepository).Assembly.GetTypes().Where(IsRepo).ToList().ForEach(register);

            return serviceRegistry;
        }

        private static IDbConnection ConnectionFactory(IServiceFactory s)
        {

            var c = new ConnectionWrapper(ConfigurationManager.ConnectionStrings["EasyCom"].ConnectionString, s.GetInstance<ISuccessIndicator>());
            if (c.State != ConnectionState.Closed) { c.Close(); }
            c.Open();
            c.AddTransaction(c.BeginTransaction());
            return (IDbConnection)c;
        }

        private static bool IsRepo(Type x)
        {
            return x.Name.EndsWith("Repository") || x.Name.EndsWith("Repo");
        }

        private static void RegisterRepository(IServiceRegistry serviceRegistry, Type obj)
        {
            var interfaces = obj.GetInterfaces().Where(x => x.FullName.StartsWith("Albingia.Kheops.OP.Application.Port")).ToList();
            if (interfaces.Any()) {
                interfaces.ForEach(x => serviceRegistry.Register(x, obj));
            } else {
                serviceRegistry.Register(obj);
            }
        }

        private class SessionContext : ISessionContext
        {
            private static Guid guid = Guid.NewGuid();

            public string SessionId => guid.ToString();

            public int Timeout => 10;

            public string UserId => "TestUser";

        }
    }

}
