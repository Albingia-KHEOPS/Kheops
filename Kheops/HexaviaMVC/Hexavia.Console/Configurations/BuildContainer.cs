using Autofac;
using Hexavia.Business;


namespace Hexavia.Console.Configurations
{
    /// <summary>
    /// Autofac DI builder
    /// </summary>
    public static class BuildContainer
    {
        
        public static IContainer CompositionRoot()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<GeoCoding>();
            builder.RegisterModule(new BusinessModule());
            return builder.Build();
        }
    }
}
