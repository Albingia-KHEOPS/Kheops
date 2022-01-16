using LightInject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ALBINGIA.Framework.Common.Mvc
{
    public class LightInjectDependencyResolver : ServiceContainer, IDependencyResolver
    {
        public LightInjectDependencyResolver() : base()
        {

        }
        public LightInjectDependencyResolver(ContainerOptions options) : base(options)
        {

        }
        public object GetService(Type serviceType)
        {
            var i = TryGetInstance(serviceType);
            return i;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return GetAllInstances(serviceType);
        }
    }
}
