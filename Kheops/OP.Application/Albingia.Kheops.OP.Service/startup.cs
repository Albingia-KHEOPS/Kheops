using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using LightInject;
using LightInject.Wcf;


[assembly: System.Web.PreApplicationStartMethod(typeof(Albingia.Kheops.OP.Service.Startup), "Initialize")]
namespace Albingia.Kheops.OP.Service
{
    public class Startup
    {
        public static void Initialize()
        {
            SqlMapperConfig.Init();
            MapperConfig.Init();
            var container = new ServiceContainer();
            container.EnableWcf();
            container.RegisterFrom<CompositionRoot>();
            LightInjectServiceHostFactory.Container = container;

        }
    }
}