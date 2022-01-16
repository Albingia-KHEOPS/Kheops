using Albingia.Kheops.OP.Application.Infrastructure;
using Albingia.Kheops.OP.Application.Infrastructure.Services;
using Albingia.Kheops.OP.Application.Infrastructure.Tools;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.DataAdapter;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Data;
using LightInject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.EasycomClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Web;

namespace Albingia.Kheops.OP.Test
{
    public class CompositionRoot : ICompositionRoot
    {
        internal class FalseSuccesIndicator : ISuccessIndicator
        {
            public bool ShouldCommit { get { return false; } set { } }
        }

        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IConfig, Config>();
            serviceRegistry.Register<ILiveDataCache, LiveCache>(new PerContainerLifetime());
            serviceRegistry.Register<ISharedDataCache>((serv) => new SharedCache(serv.GetInstance<ObjectCache>(), serv.GetInstance<ISessionContext>()));
            serviceRegistry.Register<IGenericCache, Cache>(new PerContainerLifetime());
            serviceRegistry.Register<ObjectCache>(s=> new MemoryCache(new Random().Next().ToString()), new PerContainerLifetime());
            serviceRegistry.Register<ISuccessIndicator, FalseSuccesIndicator>();
            serviceRegistry.Register<IDbConnection>(s=>  {
                var c = new ConnectionWrapper(ConfigurationManager.ConnectionStrings["EasyCom"].ConnectionString, s.GetInstance<ISuccessIndicator>());
                c.Open();
                c.AddTransaction(c.BeginTransaction());
                return c;
            }, new PerScopeLifetime());
            Action<Type> register = x=>RegisterRepository(serviceRegistry, x);
            typeof(AffaireRepository).Assembly.GetTypes().Where(IsRepo).ToList().ForEach(register);
            serviceRegistry.Register<IReferentielPort, ReferentielService>();
            serviceRegistry.Register<IAffairePort, AffaireService>();
            serviceRegistry.Register<IFormulePort, FormuleService>();
            serviceRegistry.Register<IRegularisationPort, RegularisationService>();
            serviceRegistry.Register<ISessionContext, SessionContext>();

        }

        private static bool IsRepo(Type x)
        {
            return x.Name.EndsWith("Repository") || x.Name.EndsWith("Repo");
        }

        private void RegisterRepository(IServiceRegistry serviceRegistry, Type obj)
        {
            var interfaces = obj.GetInterfaces().Where(x => x.FullName.StartsWith("Albingia.Kheops.OP.Application.Port")).ToList();
            if (interfaces.Any())
            {
                interfaces.ForEach(x=> serviceRegistry.Register(x,obj, new PerScopeLifetime()));
            }
            else
            {
                serviceRegistry.Register(obj);
            }
        }


        private class SessionContext : ISessionContext
        {
            private static Guid guid = Guid.NewGuid();

            public string SessionId => guid.ToString();

            public int Timeout => 10;

            public string UserId => "ConsoleUser";
        }

    }
}