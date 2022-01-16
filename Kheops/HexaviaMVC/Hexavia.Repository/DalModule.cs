using Autofac;
using Hexavia.Repository.Interfaces;

namespace Hexavia.Repository
{
    public class DalModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AdresseRepository>().As<IAddressRepository>();
            builder.RegisterType<LatitudeLogitudeRepository>().As<ILatitudeLogitudeRepository>();
            builder.RegisterType<LayerRepository>().As<ILayerRepository>();
            builder.RegisterType<ReferentielRepository>().As<IReferentielRepository>();
            builder.RegisterType<DataAccessManager>().As<DataAccessManager>();
            builder.RegisterType<EasyCom>().As<EasyCom>();
            
            base.Load(builder);
        }
    }
}
