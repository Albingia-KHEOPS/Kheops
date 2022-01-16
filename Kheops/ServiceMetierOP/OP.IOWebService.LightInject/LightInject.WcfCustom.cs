/*********************************************************************************
    The MIT License (MIT)
    Copyright (c) 2017 bernhard.richter@gmail.com
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:
    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
******************************************************************************
   LightInject.Wcf version 2.0.0 modified
   http://www.lightinject.net/
   http://twitter.com/bernhardrichter
******************************************************************************/
using LightInject;
using LightInject.Interception;

namespace LightInject {
    using System;
    using System.ServiceModel;
    using OP.IOWebService.LightInject;

    /// <summary>
    /// Extends the <see cref="IServiceContainer"/> interface with a method
    /// to enable services that are scoped per <see cref="OperationContext"/>.
    /// </summary>
    public static class WcfContainerExtensions {
        /// <summary>
        /// Ensures that services registered with the <see cref="PerScopeLifetime"/> or <see cref="PerRequestLifeTime"/>
        /// is properly disposed at the end of an <see cref="OperationContext"/>.
        /// </summary>
        /// <param name="serviceContainer">The target <see cref="IServiceContainer"/>.</param>
        public static void EnableWcf(this IServiceContainer serviceContainer) {
            LightInjectServiceHostFactory.Container = serviceContainer;
            ((ServiceContainer)serviceContainer).ScopeManagerProvider = new PerLogicalCallContextScopeManagerProvider();
        }
    }

    /// <summary>
    /// Extends the <see cref="Type"/> class.
    /// </summary>
    public static class TypeExtensions {
        /// <summary>
        /// Determines if the <paramref name="type"/> represents a service contract.
        /// </summary>
        /// <param name="type">The target <see cref="Type"/>.</param>
        /// <returns><b>true</b> if the <paramref name="type"/> represents a service type, otherwise <b>false</b>.</returns>
        public static bool IsServiceContract(this Type type) {
            return type.IsDefined(typeof(ServiceContractAttribute), true);
        }
    }
}

namespace OP.IOWebService.LightInject {
    using Albingia.Kheops.Common;
    using ALBINGIA.Framework.Common;
    using OPWebService;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.ServiceModel.Description;


    /// <summary>
    /// A subclass of the <see cref="ServiceHost"/> class that allows
    /// xml configuration to be applied.
    /// </summary>
    public class LightInjectServiceHost: ServiceHost {
        /// <summary>
        /// Initializes a new instance of the <see cref="LightInjectServiceHost"/> class with the type of service and its base addresses specified.
        /// </summary>
        /// <param name="serviceType">The type of hosted service.</param>
        /// <param name="baseAddresses">An array of type <see cref="Uri"/> that contains the base addresses for the hosted service.</param>
        public LightInjectServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses) {
        }

        protected override void ApplyConfiguration()
        {
            base.ApplyConfiguration();
        }

        /// <summary>
        /// Loads the service description from the configuration file and applies it to the runtime being constructed.
        /// </summary>
        public void LoadConfiguration() {
            ApplyConfiguration();
        }
    }

    /// <summary>
    /// A <see cref="ServiceHostFactory"/> that uses the LightInject <see cref="ServiceContainer"/>
    /// to create WCF services.
    /// </summary>
    public class LightInjectServiceHostFactory: ServiceHostFactory {
        private static IServiceContainer container;
        //private static Dictionary<Type, ServiceHost> allHosts = new Dictionary<Type, ServiceHost>();

        /// <summary>
        /// Gets or sets the <see cref="IServiceContainer"/> instance that is used to resolve services.
        /// </summary>
        public static IServiceContainer Container {
            set => container = value;
        }

        public static Scope CallBeginScope() {
            return container.BeginScope();
        }

        /// <summary>
        /// Creates a <see cref="T:System.ServiceModel.ServiceHost"/> with specific base addresses and initializes it with specified data.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.ServiceHost"/> with specific base addresses.
        /// </returns>
        /// <param name="constructorString">The initialization data passed to the <see cref="T:System.ServiceModel.ServiceHostBase"/> instance being constructed by the factory. </param><param name="baseAddresses">The <see cref="T:System.Array"/> of type <see cref="T:System.Uri"/> that contains the base addresses for the service hosted.</param><exception cref="T:System.ArgumentNullException"><paramref name="baseAddresses"/> is null.</exception><exception cref="T:System.InvalidOperationException">There is no hosting context provided or <paramref name="constructorString"/> is null or empty.</exception>
        public override ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses) {
            EnsureValidServiceContainer();

            ServiceRegistration registration = GetServiceRegistrationByName(constructorString);
            if (registration != null) {
                return CreateServiceHost(registration.ServiceType, registration.ServiceName, baseAddresses);
            }

            return base.CreateServiceHost(constructorString, baseAddresses);
        }

        /// <summary>
        /// Creates a <see cref="ServiceHost"/> with the specified <paramref name="baseAddresses"/>.
        /// </summary>
        /// <typeparam name="TService">The type of service to be hosted by the <see cref="ServiceHost"/>.</typeparam>
        /// <param name="baseAddresses">The base addresses for the hosted service.</param>
        /// <returns>A <see cref="ServiceHost"/> for the specified <typeparamref name="TService"/>.</returns>
        public ServiceHost CreateServiceHost<TService>(params string[] baseAddresses) {
            var uriBaseAddresses = baseAddresses.Select(s => new Uri(s)).ToArray();
            return CreateServiceHost(typeof(TService), uriBaseAddresses);
        }

        /// <summary>
        /// Creates a <see cref="T:System.ServiceModel.ServiceHost"/> for a specified type of service with a specific base address.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.ServiceHost"/> for the type of service specified with a specific base address.
        /// </returns>
        /// <param name="serviceType">Specifies the type of service to host. </param><param name="baseAddresses">The <see cref="T:System.Array"/> of type <see cref="T:System.Uri"/> that contains the base addresses for the service hosted.</param>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses) {
            return CreateServiceHost(serviceType, serviceType.FullName, baseAddresses);
        }

        private static void EnsureValidServiceContainer() {
            if (container == null) {
                container = new ServiceContainer();
                container.ScopeManagerProvider = new PerLogicalCallContextScopeManagerProvider();
            }
        }

        private static ServiceHost CreateServiceHost(Type serviceType, string constructorString, Uri[] baseAddresses) {
            ValidateServiceType(serviceType);

            var proxyType = CreateServiceProxyType(serviceType);

            var serviceHost = new LightInjectServiceHost(proxyType, baseAddresses);
            serviceHost.Description.ConfigurationName = constructorString;
            serviceHost.Description.Name = constructorString;
            serviceHost.LoadConfiguration();
            if (!serviceHost.Description.Endpoints.Any()) {
                serviceHost.AddDefaultEndpoints();
            }
            ApplyServiceBehaviors(serviceHost);
            ApplyEndpointBehaviors(serviceHost);

            return serviceHost;
        }

        private static ServiceRegistration GetServiceRegistrationByName(string constructorString) {
            var registrations =
                container.AvailableServices.Where(
                    sr => sr.ServiceName.Equals(constructorString, StringComparison.InvariantCultureIgnoreCase))
                    .ToArray();

            if (registrations.Length == 0) {
                return null;
            }

            if (registrations.Length > 1) {
                throw new InvalidOperationException(
                    string.Format("Multiple services found under the same name '{0}'", constructorString));
            }

            return registrations[0];
        }

        private static void ApplyEndpointBehaviors(ServiceHost serviceHost) {
            IEnumerable<IEndpointBehavior> endpointBehaviors = container.GetAllInstances<IEndpointBehavior>().ToArray();
            foreach (var endpoint in serviceHost.Description.Endpoints) {
                foreach (var endpointBehavior in endpointBehaviors) {
                    if (!endpoint.Behaviors.Contains(endpointBehavior.GetType())) {
                        endpoint.Behaviors.Add(endpointBehavior);
                    }
                }
            }
        }

        private static void ApplyServiceBehaviors(ServiceHostBase serviceHost) {
            var serviceBehaviors = container.GetAllInstances<IServiceBehavior>();
            var description = serviceHost.Description;
            foreach (var serviceBehavior in serviceBehaviors) {
                if (!description.Behaviors.Contains(serviceBehavior.GetType())) {
                    description.Behaviors.Add(serviceBehavior);
                }
            }
        }

        private static Type CreateServiceProxyType(Type serviceType) {
            var proxyBuilder = new ProxyBuilder();
            var proxyDefinition = CreateProxyDefinition(serviceType);
            ImplementServiceInterface(serviceType, proxyDefinition);
            return proxyBuilder.GetProxyType(proxyDefinition);
        }

        private static ProxyDefinition CreateProxyDefinition(Type serviceType) {
            var proxyDefinition = new ProxyDefinition(serviceType, () => {

                return container.GetInstance(serviceType);
            });
            if (container.CanGetInstance(serviceType, string.Empty)) {
                ServiceRegistration serviceRegistration = container.AvailableServices.FirstOrDefault(sr => sr.ServiceType == serviceType);
                if (serviceRegistration != null && serviceRegistration.ImplementingType != null) {
                    proxyDefinition.AddCustomAttributes(
                        serviceRegistration.ImplementingType.GetCustomAttributesData().ToArray());
                }
            }

            return proxyDefinition;
        }

        private static void ImplementServiceInterface(
           Type serviceType, ProxyDefinition proxyDefinition) {
            proxyDefinition.Implement(() => new ServiceInterceptor(container), m => m.IsDeclaredBy(serviceType));
        }

        private static void ValidateServiceType(Type serviceType) {
            if (serviceType == null) {
                throw new ArgumentNullException("serviceType");
            }

            if (!IsInterfaceWithServiceContractAttribute(serviceType)) {
                throw new NotSupportedException(
                    "Only interfaces with [ServiceContract] attribute are supported with LightInjectServiceHostFactory.");
            }
        }

        private static bool IsInterfaceWithServiceContractAttribute(Type serviceType) {
            return serviceType.IsInterface && serviceType.IsDefined(typeof(ServiceContractAttribute), true);
        }
    }

    /// <summary>
    /// An <see cref="IInterceptor"/> that ensures that a service operation is
    /// executed within a <see cref="Scope"/>.
    /// </summary>
    public class ServiceInterceptor: IInterceptor {
        private readonly IServiceContainer serviceContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceInterceptor"/> class.
        /// </summary>
        /// <param name="serviceContainer">The <see cref="IServiceContainer"/> that is used to create the <see cref="Scope"/>.</param>
        internal ServiceInterceptor(IServiceContainer serviceContainer) {
            this.serviceContainer = serviceContainer;
        }

        /// <summary>
        /// Wraps the execution of a service operation inside a <see cref="Scope"/>.
        /// </summary>
        /// <param name="invocationInfo">The <see cref="IInvocationInfo"/> instance that
        /// contains information about the current method call.</param>
        /// <returns>The return value from the method.</returns>
        public object Invoke(IInvocationInfo invocationInfo) {
            var scope = this.serviceContainer.BeginScope();
            try {
                //if (invocationInfo.Proxy.Target is OP.Services.TraitementAffNouv.AffaireNouvelle an && invocationInfo.Method.Name == nameof(an.SaveNewAffair)) {
                //}
                return invocationInfo.Proceed();
            }
            catch (Exception ex) {
                var indicator = scope.GetInstance<ISuccessIndicator>();
                if (indicator != null) {
                    indicator.ShouldCommit = false;
                }
                if (ex is BusinessValidationException bex) {
                    // List<ValidationError>
                    if (invocationInfo.Method.CustomAttributes.Any(a => a.AttributeType == typeof(FaultContractAttribute) && a.ConstructorArguments.Any(arg => arg.ArgumentType == typeof(Type) && Equals(arg.Value, typeof(List<ValidationError>))))) {
                        throw new FaultException<List<ValidationError>>(bex.Errors.ToList(), bex.Message);
                    }
                    // BusinessError
                    if (invocationInfo.Method.CustomAttributes.Any(a => a.AttributeType == typeof(FaultContractAttribute) && a.ConstructorArguments.Any(arg => arg.ArgumentType == typeof(Type) && Equals(arg.Value, typeof(BusinessError))))) {
                        throw new FaultException<BusinessError>(new BusinessError(bex), bex.Message);
                    }
                }

                throw;
            }
            finally {
                scope.Dispose();
            }
        }
    }
}