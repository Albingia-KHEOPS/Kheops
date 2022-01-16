using Autofac;
using Hexavia.Business.Interfaces;
using Hexavia.Repository;

namespace Hexavia.Business
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AdresseBusiness>().As<IAdresseBusiness>();
            builder.RegisterType<LatitudeLogitudeBusiness>().As<ILatitudeLogitudeBusiness>();
            builder.RegisterType<KheopsUrlBusiness>().As<IKheopsUrlBusiness>();         
            builder.RegisterType<OfferContractBusiness>().As<IOfferContractBusiness>();
            builder.RegisterType<PartnerBusiness>().As<IPartnerBusiness>();
            builder.RegisterType<GrpUrlBusiness>().As<IGrpUrlBusiness>();
            builder.RegisterType<LayerBusiness>().As<ILayerBusiness>();
            builder.RegisterType<ReferentielBusiness>().As<IReferentielBusiness>();
            builder.RegisterModule(new DalModule());
        }
    }
}
