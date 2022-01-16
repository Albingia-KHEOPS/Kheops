using Albingia.Kheops.OP.Application.Contracts;
using Albingia.Kheops.OP.Application.Infrastructure;
using Albingia.Kheops.OP.Application.Infrastructure.Services;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.DataAdapter;
using Albingia.Kheops.OP.Infrastructure;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Data;
using LightInject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace Albingia.Kheops.OP.Service
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IConfig, Config>();
            serviceRegistry.Register<ObjectCache>(s => MemoryCache.Default);
            serviceRegistry.Register<IGenericCache, Cache>();
            serviceRegistry.Register<IDbConnection>(s => { var c = new ConnectionWrapper(ConfigurationManager.ConnectionStrings["EasyCom"].ConnectionString, s.GetInstance<ISuccessIndicator>()); c.Open(); return c; }, new PerScopeLifetime());
            serviceRegistry.Register<ISuccessIndicator, HttpSuccessIndicator>();
            serviceRegistry.Register<IAffairePort, AffaireService>("Affaire");
         
            Action<Type> register = x=>RegisterRepository(serviceRegistry, x);
            typeof(AffaireRepository).Assembly.GetTypes().Where(IsRepo).ToList().ForEach(register);
        }

        private static bool IsRepo(Type x)
        {
            return x.Name.EndsWith("Repository") || x.Name.EndsWith("Repo");
        }

        private void RegisterRepository(IServiceRegistry serviceRegistry, Type obj)
        {
            var interfaces = obj.GetInterfaces().Where(x => x.FullName.StartsWith("Albingia.Kheops.OP.Applciation.Port")).ToList();
            if (interfaces.Any())
            {
                interfaces.ForEach(x => serviceRegistry.Register(x, obj));
            }
            else
            {
                serviceRegistry.Register(obj);
            }
        }
    }
}