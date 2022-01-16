
using LightInject;
using OP.IOWebService.LightInject;

[assembly: System.Web.PreApplicationStartMethod(typeof(OP.IOWebService.Startup), "Initialize")]

namespace OP.IOWebService {
    using Albingia.Kheops.DTO;
    using Albingia.Kheops.OP.Application;
    using Albingia.Kheops.OP.Application.Contracts;
    using Albingia.Kheops.OP.Application.Infrastructure.Services;
    using Albingia.Kheops.OP.Application.Infrastructure.Tools;
    using Albingia.Kheops.OP.Application.Port.Driver;
    using Albingia.Kheops.OP.DataAdapter;
    using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
    using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
    using Albingia.Kheops.OP.Domain.Affaire;
    using Albingia.Kheops.OP.Domain.Referentiel;
    using ALBINGIA.Framework.Common;
    using ALBINGIA.Framework.Common.Data;
    using ALBINGIA.Framework.Common.Extensions;
    using Dapper;
    using Mapster;
    using OP.DataAccess;
    using OP.Services;
    using OP.Services.Administration;
    using OP.Services.BLServices;
    using OP.Services.BLServices.Regularisations;
    using OP.Services.Common;
    using OP.Services.Connexites;
    using OP.Services.Hexavia;
    using OP.Services.REST.wsadel;
    using OP.Services.TraitementAffNouv;
    using OP.Services.TraitementsFinOffre;
    using OP.Services.Web;
    using OP.WSAS400.DTO.Offres.Parametres;
    using OPServiceContract;
    using OPServiceContract.IAdministration;
    using OPServiceContract.IAffaireNouvelle;
    using OPServiceContract.ICommon;
    using OPServiceContract.IHexavia;
    using OPServiceContract.ITraitementsFinOffre;
    using RestContracts;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Runtime.Caching;

    public class Startup
    {
        public static void Initialize()
        {
            var container = new ServiceContainer();
            typeof(RepositoryBase).Assembly.GetTypes()
                .Where(t => t.BaseType == typeof(RepositoryBase))
                .ToList()
                .ForEach(type => container.Register(type));

            container.Register<CibleService, CibleService>();
            container.Register<Services.DocumentService, Services.DocumentService>();
            container.Register<TraceService, TraceService>();
            container.Register<EngagementService, EngagementService>();
            container.Register<IFormule, Services.BLServices.FormuleService>();
            container.Register<FolderService, FolderService>();
            container.Register<IAvenant, AvenantService>();
            container.Register<IWebAvenant, AvenantRestService>();
            container.Register<ConnexiteService, ConnexiteService>();
            container.Register<RegularisationManager, RegularisationManager>();
            container.Register<SinistresRestClient, SinistresRestClient>();
            container.Register<IAffaireNouvelle, AffaireNouvelle>();
            container.Register<ICommonAffaire, CommonAffaireService>();
            container.Register<IFinOffre, FinOffre>();
            container.Register<IRisque, Services.ClausesRisquesGaranties.RisqueService>();
            container.Register<ICommonOffre, CommonOffre>();
            container.Register<ICommonAffaire, CommonAffaireService>();
            container.Register<IRegularisation, Regularisation>();
            container.Register<IStepFinder, StepFinder>();
            container.Register<INavigationService, NavigationService>();
            container.Register<IAdresseHexavia, AdresseHexavia>();
            container.Register<ISinistres, SinistresService>();
            container.Register<IVoletsBlocsCategories, Administration>();
            container.Register<ISuccessIndicator, WcfSuccessIndicator>(new PerScopeLifetime());

            container.Register<IDbConnection>(s =>
            {
                var c = new ConnectionWrapper(
                    ConfigurationManager.ConnectionStrings["EasyCom"].ConnectionString,
                    s.GetInstance<ISuccessIndicator>());

                c.IsTransactionShared = false;
                c.Open();
                try
                {
                    c.AddTransaction(c.BeginTransaction());
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Trace.TraceError("Failed BeginTransaction: " + exception.Message + ", Connection Handle: " + c.GetCnxHandle().ToString());
                }
                return c;
            }, new PerScopeLifetime());

            InitOPApplication(container);

            DapperConfig();

            LightInjectServiceHostFactory.Container = container;
        }

        private static void InitOPApplication(ServiceContainer container)
        {
            container.Register<IConfig, Config>();
            container.Register<ILiveDataCache, LiveCache>(new PerContainerLifetime());
            container.Register<ISharedDataCache>((serv) => new SharedCache(serv.GetInstance<ObjectCache>(), serv.GetInstance<SharedContext>()));
            container.Register<IGenericCache, Cache>(new PerContainerLifetime());
            container.Register<ObjectCache>(s => new MemoryCache(new Random().Next().ToString()), new PerContainerLifetime());
            container.Register<IdentifierGenerator, IdentifierGenerator>();
            container.Register<IReferentielPort, ReferentielService>();
            container.Register<IDocumentPort, Albingia.Kheops.OP.Application.Infrastructure.Services.DocumentService>();
            container.Register<IAffairePort, AffaireService>();
            container.Register<IEtapePort, EtapeService>();
            container.Register<IRegularisationPort, RegularisationService>();
            container.Register<IUserPort, UserService>();
            container.Register<IRisquePort, RisqueService>();
            container.Register<ISinistrePort, SinistreService>();
            container.Register<IGarantiePort, GarantieService>();
            container.Register<IFormulePort, Albingia.Kheops.OP.Application.Infrastructure.Services.FormuleService>();
            container.Register<ISessionContext, SessionContext>();
            container.Register<IInfosSpecifiquesPort, Albingia.Kheops.OP.Application.Infrastructure.Services.InfosSpecifiquesService>();
            container.Register<SharedContext, SharedContext>();
            container.Register<IParametrageModelesPort, ParametrageModelesService>();

            Action<Type> register = x => RegisterRepository(container, x);
            typeof(RefRepository).Assembly.GetTypes().Where(IsRepo).ToList().ForEach(register);

            ServiceMapper.Init();
            TypeAdapterConfig<FolderLock, FolderLockDto>
                .NewConfig()
                .Map(target => target.CodeAffaire, source => source.CodeOffre)
                .Map(target => target.NumeroAliment, source => source.Version)
                .Map(target => target.TypeAffaire, source => source.Type.ParseCode<AffaireType>())
                .Map(target => target.IsHisto, source => false);

            TypeAdapterConfig<FolderLockDto, FolderLock>
                .NewConfig()
                .Map(target => target.CodeOffre, source => source.CodeAffaire)
                .Map(target => target.Version, source => source.NumeroAliment)
                .Map(target => target.Type, source => source.TypeAffaire.AsCode(null, true));

            TypeAdapterConfig<RefParamValue, ParametreDto>
                .NewConfig()
                .Include<Quittancement, ParametreDto>()
                .Include<TypeRelance, ParametreDto>()
                .Include<TypeTraitement, ParametreDto>()
                .Map(target => target.CodeTpcn1, source => source.ParamNum1)
                .Map(target => target.CodeTpcn2, source => source.ParamNum2)
                .Map(target => target.CodeTpca1, source => source.ParamText1)
                .Map(target => target.CodeTpca2, source => source.ParamText2)
                .Map(target => target.Libelle, source => source.Libelle)
                .Map(target => target.Descriptif, source => source.LibelleLong);

            TypeAdapterConfig<AffaireId, PGMFolder>
                .NewConfig()
                .Map(target => target.CodeOffre, source => source.CodeAffaire)
                .Map(target => target.Version, source => source.NumeroAliment)
                .Map(target => target.Type, source => source.TypeAffaire.AsCode(null, true))
                .Map(target => target.NumeroAvenant, source => source.NumeroAvenant.GetValueOrDefault());

#if DEBUG
                Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.DisableTelemetry = true;
#endif
            //System.Threading.Tasks.Task.Run(() => Warmup.Load(container));
        }

        public static void DapperConfig() {
            SqlMapperConfig.Init();
            SqlMapper.AddTypeHandler(new ALBINGIA.Framework.Common.Data.Dapper.EacNullableDatetimeTypeHandler());
        }

        private static void RegisterRepository(IServiceRegistry serviceRegistry, Type type)
        {
            var interfaces = type.GetInterfaces().Where(x => x.FullName.StartsWith("Albingia.Kheops.OP.Application.Port")).ToList();
            if (interfaces.Any())
            {
                interfaces.ForEach(x =>
                {
                    //Console.WriteLine($"Registering {x.Name} of {type.FullName}...");
                    serviceRegistry.Register(x, type);
                });

            }
            else
            {
                //Console.WriteLine($"Registering {type.FullName}...");
                serviceRegistry.Register(type);
            }
        }

        private static bool IsRepo(Type x)
        {
            return x.Name.EndsWith("Repository") || x.Name.EndsWith("Repo");
        }
    }

}