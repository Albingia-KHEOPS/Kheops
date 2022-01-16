using Albingia.Kheops.OP.Application.Contracts;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Common;
using LightInject;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Caching;

namespace ALBINGIA.OP.OP_MVC {
    public class CompositionRoot : ICompositionRoot {
        public void Compose(IServiceRegistry serviceRegistry) {
            serviceRegistry.Register<IConfig, Config>();
            serviceRegistry.Register<IGenericCache, Cache>(new PerContainerLifetime());
            serviceRegistry.Register<ObjectCache>(s => new MemoryCache(new Random().Next().ToString()), new PerContainerLifetime());
            serviceRegistry.Register<ISuccessIndicator, HttpSuccessIndicator>(new PerScopeLifetime());
            serviceRegistry.Register<CacheConditions, CacheConditions>(new PerScopeLifetime());

            serviceRegistry.Register<IDbConnection>(s => {
                var c = new ConnectionWrapper(ConfigurationManager.ConnectionStrings["EasyCom"].ConnectionString, s.GetInstance<ISuccessIndicator>());
                c.IsTransactionShared = false;
                c.Open();
                c.AddTransaction(c.BeginTransaction());
                return c;
            }, new PerScopeLifetime());

            serviceRegistry.Register<ISessionContext, SessionContext>();
            serviceRegistry.Register<SharedContext, SharedContext>();
        }


        private void RegisterRepository(IServiceRegistry serviceRegistry, Type type) {
            var interfaces = type.GetInterfaces().Where(x => x.FullName.StartsWith("Albingia.Kheops.OP.Application.Port")).ToList();
            if (interfaces.Any()) {
                interfaces.ForEach(x => {
                    Console.WriteLine($"Registering {x.Name} of {type.FullName}...");
                    AlbLog.Log($"Registering {x.Name} of {type.FullName}...", AlbLog.LogTraceLevel.Information);
                    serviceRegistry.Register(x, type);
                });

            }
            else {
                Console.WriteLine($"Registering {type.FullName}...");
                AlbLog.Log($"Registering {type.FullName}...", AlbLog.LogTraceLevel.Information);
                serviceRegistry.Register(type);
            }
        }

        private static bool IsRepo(Type x) {
            return x.Name.EndsWith("Repository") || x.Name.EndsWith("Repo");
        }
    }
}
