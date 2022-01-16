using Albingia.Kheops.OP.Application;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.DataAdapter;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using LightInject;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Test
{
    class Program
    {
        static void Main()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            try {
                var sw = new Stopwatch();
                var cont = new ServiceContainer();
                cont.ScopeManagerProvider = new PerLogicalCallContextScopeManagerProvider();
                ServiceMapper.Init();
                SqlMapperConfig.Init();
                new CompositionRoot().Compose(cont);
                Warmup.Load(cont).Wait();
                Console.WriteLine($"Elapsed {sw.ElapsedMilliseconds }");
            } catch (Exception e) {
                Console.Error.WriteLine(e.ToString());
                Console.In.ReadLine();
            }
        }

        static void Main5()
        {
            var sw = new Stopwatch();
            var cont = new ServiceContainer();
            cont.ScopeManagerProvider = new PerLogicalCallContextScopeManagerProvider();
            ServiceMapper.Init();
            SqlMapperConfig.Init();
            new CompositionRoot().Compose(cont);
            var mgr = cont.ScopeManagerProvider.GetScopeManager(cont);
            var scope = mgr.BeginScope();
            try {
                sw.Start();
                var serv = scope.GetInstance<IFormulePort>();
                var d = serv.GetFormulesAffaire(new AffaireId {
                    TypeAffaire = AffaireType.Contrat,
                    CodeAffaire = "RS1800153",
                    IsHisto = false,
                    NumeroAliment = 0
                }, false);
            } finally {
                mgr.EndScope(scope);
            }
            Console.WriteLine($"Elapsed {sw.ElapsedMilliseconds }");

        }

        public class Warmup
        {
            internal static Task Load(ServiceContainer ctx)
            {

                List<Action<IParamRepository>> fetchParams = new List<Action<IParamRepository>> {
                (r => r.GetDesignations()),
                (r => r.GetTypeInventaires()),
                (r => r.GetExpressionFranchises()),
                (r => r.GetExpressionLCIs()),
                (r => r.GetParamVolets()),
            };
                List<Action<IReferentialRepository>> fetch = new List<Action<IReferentialRepository>> {
                (r => r.GetCibleCategos()),
                (r => r.GetCibles()),
                (r => r.GetUtilisateurs())
            };
                var refTypes = typeof(RefValue).Assembly.GetExportedTypes().Where(x => typeof(RefValue).IsAssignableFrom(x) && !x.IsAbstract);

                var refFetchers = refTypes.Select<Type, Action<IReferentialRepository>>(x =>
                     ((IReferentialRepository rf) => {
                         rf.GetType().GetMethod("GetValues").MakeGenericMethod(x).Invoke(rf, new object[0]);
                     })
                );

                fetch.AddRange(refFetchers);
                List<Action> actions =
                        fetch.Select<Action<IReferentialRepository>, Action>(action => (() => { RunInScope(ctx, action); }))
                    .Concat(
                        fetchParams.Select<Action<IParamRepository>, Action>(action => (() => { RunInScope(ctx, action); }))
                    ).ToList();
                return Task.Run(() => {
                    actions.ForEach(x => x());
                });
            }

            static void RunInScope<T>(ServiceContainer ctx, Action<T> action)
            {
                using (var scope = ctx.BeginScope()) {
                    var inst = scope.GetInstance<T>();
                    try {
                        action(inst);
                    } catch {
                        //Ignore Errors 
                    }
                }
            }
        }

        static void Main4()
        {
            var sw = new Stopwatch();
            var cont = new ServiceContainer();
            cont.ScopeManagerProvider = new PerLogicalCallContextScopeManagerProvider();
            SqlMapperConfig.Init();
            new CompositionRoot().Compose(cont);
            var mgr = cont.ScopeManagerProvider.GetScopeManager(cont);
            var scope = mgr.BeginScope();
            try {
                sw.Start();
                var serv = scope.GetInstance<IReferentielPort>();
                var d = serv.GetDevises(new Domain.Referentiel.CibleFiltre("", ""));
                Console.WriteLine(d.Count());
                d = serv.GetDevises(null);
                Console.WriteLine(d.Count());
                d = serv.GetDevises(new Domain.Referentiel.CibleFiltre("RS", "CEREMONIE"));
                Console.WriteLine(d.Count());

                var e = serv.GetUnitesFranchise(new Domain.Referentiel.CibleFiltre("", ""));
                Console.WriteLine($"UNFRA: {String.Join(", ", e.Select(x => $"{x.Code} / {x.CodeFiltre}"))}");
                Console.WriteLine($"UNFRA: {e.Count()}");
                e = serv.GetUnitesFranchise(null);
                Console.WriteLine($"UNFRA: {String.Join(", ", e.Select(x => $"{x.Code} / {x.CodeFiltre}"))}");
                Console.WriteLine($"UNFRA: {e.Count()}");
                e = serv.GetUnitesFranchise(new Domain.Referentiel.CibleFiltre("RS", "CEREMONIE"));
                Console.WriteLine($"UNFRA: {String.Join(", ", e.Select(x => $"{x.Code} / {x.CodeFiltre}"))}");
                Console.WriteLine($"UNFRA: {e.Count()}");
                e = serv.GetUnitesFranchise(new Domain.Referentiel.CibleFiltre("IA", "REVENTE"));
                Console.WriteLine($"UNFRA: {String.Join(", ", e.Select(x => $"{x.Code} / {x.CodeFiltre}"))}");
                Console.WriteLine($"UNFRA: {e.Count()}");
                e = serv.GetUnitesFranchise(new Domain.Referentiel.CibleFiltre("IA", "TEST"));
                Console.WriteLine($"UNFRA: {String.Join(", ", e.Select(x => $"{x.Code} / {x.CodeFiltre}"))}");
                Console.WriteLine($"UNFRA: {e.Count()}");
            } finally {
                mgr.EndScope(scope);
            }
            Console.WriteLine($"Elapsed {sw.ElapsedMilliseconds }");

        }
        static void Main3(string[] args)
        {
            var sw1 = new Stopwatch();
            var cont = new ServiceContainer();
            cont.ScopeManagerProvider = new PerLogicalCallContextScopeManagerProvider();
            sw1.Start();
            SqlMapperConfig.Init();
            sw1.Stop();
            Console.WriteLine($"Sql Init Elapsed {sw1.ElapsedMilliseconds }");
            sw1.Restart();
            new CompositionRoot().Compose(cont);
            sw1.Stop();
            Console.WriteLine($"Compose Elapsed {sw1.ElapsedMilliseconds }");

            var sw = new Stopwatch();
            var mgr = cont.ScopeManagerProvider.GetScopeManager(cont);
            var scope = mgr.BeginScope();
            try {
                sw.Start();
                var serv = scope.GetInstance<IParamRepository>();
                IEnumerable<Domain.Parametrage.Formules.ParamVolet> volets = serv.GetParamVolets();
                var fo = JsonConvert.SerializeObject(volets, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                sw.Stop();
                IEnumerable<Domain.Parametrage.Formules.ParamBloc> blocs = volets.SelectMany(x => x.Blocs);
                IEnumerable<Domain.Parametrage.Formules.ParamModeleGarantie> modeles = blocs.SelectMany(x => x.Modeles);
                IEnumerable<Domain.Parametrage.Formules.ParamGarantieHierarchie> g1 = modeles.SelectMany(x => x.Garanties).Distinct();
                IEnumerable<Domain.Parametrage.Formules.ParamGarantieHierarchie> g2 = g1.SelectMany(x => x.GarantiesChildren).Distinct();
                IEnumerable<Domain.Parametrage.Formules.ParamGarantieHierarchie> g3 = g2.SelectMany(x => x.GarantiesChildren).Distinct();
                IEnumerable<Domain.Parametrage.Formules.ParamGarantieHierarchie> g4 = g3.SelectMany(x => x.GarantiesChildren).Distinct();
                IEnumerable<Domain.Parametrage.Formules.ParamGarantieHierarchie> g5 = g4.SelectMany(x => x.GarantiesChildren).Distinct();
                Console.WriteLine($"Stats : V { volets.Count()}, B { blocs.Count() }, M : {modeles.Count()}, G1 {g1.Count()}, g2 {g2.Count()}, g3:{g3.Count()}, g4 {g4.Count()}, g5 {g5.Count()} ");
            } finally {
                mgr.EndScope(scope);
            }
            Console.WriteLine($"Elapsed {sw.ElapsedMilliseconds }");

        }


        static void Main2(string[] args)
        {
            var cont = new ServiceContainer();
            cont.ScopeManagerProvider = new PerLogicalCallContextScopeManagerProvider();
            var mgr = cont.ScopeManagerProvider.GetScopeManager(cont);
            mgr.BeginScope();
            try {
                new CompositionRoot().Compose(cont);

                var serv = cont.GetInstance<IAffaireRepository>();

                SqlMapperConfig.Init();

                var sw = new Stopwatch();
                sw.Start();
                //System.Console.WriteLine(
                var fo = JsonConvert.SerializeObject(serv.GetById(AffaireType.Contrat, "RS1504768", 0), Formatting.Indented);
                sw.Stop();
                Console.WriteLine($"Elapsed {sw.ElapsedMilliseconds }");
                sw.Restart();
                fo = JsonConvert.SerializeObject(serv.GetById(AffaireType.Contrat, "RS1504768", 0), Formatting.Indented);
                //, Formatting.Indented));
                sw.Stop();
                Console.WriteLine($"Elapsed {sw.ElapsedMilliseconds }");

                String[] contracts = new[] { "RS1405492", "RS1409122", "RS1409126", "RS1409127", "RS1503589", "RS1503602", "RS1503638", "RS1503721", "RS1503796", "RS1503816", "RS1503843", "RS1503844", "RS1503985", "RS1503986", "RS1503987", "RS1503988", "RS1504048", "RS1504053", "RS1504071", "RS1504075", "RS1504174", "RS1504187", "RS1504192", "RS1504198", "RS1504202", "RS1504215", "RS1504237", "RS1504253", "RS1504258", "RS1504265", "RS1504272", "RS1504285", "RS1504288", "RS1504308", "RS1504311", "RS1504314", "RS1504319", "RS1504327", "RS1504328", "RS1504330", "RS1504362", "RS1504370", "RS1504374", "RS1504381", "RS1504400", "RS1504401", "RS1504403", "RS1504407", "RS1504412", "RS1504415", "RS1504418", "RS1504423", "RS1504426", "RS1504434", "RS1504435", "RS1504454", "RS1504456", "RS1504459", "RS1504463", "RS1504469", "RS1504470", "RS1504484", "RS1504487", "RS1504488", "RS1504489", "RS1504503", "RS1504505", "RS1504507", "RS1504513", "RS1504516", "RS1504518", "RS1504519", "RS1504521", "RS1504522", "RS1504524", "RS1504526", "RS1504535", "RS1504540", "RS1504570", "RS1504579", "RS1504582", "RS1504596", "RS1504602", "RS1504606", "RS1504613", "RS1504627", "RS1504639", "RS1504642", "RS1504643", "RS1504644", "RS1504651", "RS1504652", "RS1504653", "RS1504660", "RS1504662", "RS1504665", "RS1504667", "RS1504695", "RS1504708", "RS1504709", "RS1504712", "RS1504714", "RS1504727", "RS1504730", "RS1504732", "RS1504737", "RS1504739", "RS1504740", "RS1504750", "RS1504754", "RS1504757", "RS1504761", "RS1504762", "RS1504768", "RS1504775", "RS1504777", "RS1504789", "RS1504797", "RS1504809", "RS1504811", "RS1504818", "RS1504820", "RS1504821", "RS1504822", "RS1504823", "RS1504833", "RS1504837", "RS1504839", "RS1504841", "RS1504843", "RS1504844", "RS1504845", "RS1504846", "RS1504847", "RS1504850", "RS1504851", "RS1504857", "RS1504860", "RS1504863", "RS1504864", "RS1504865", "RS1504868", "RS1504878", "RS1504881", "RS1504882", "RS1504883", "RS1504887", "RS1504888", "RS1504890", "RS1504897", "RS1504908", "RS1504921", "RS1504923", "RS1504927", "RS1504931", "RS1504932", "RS1504943", "RS1504952", "RS1504960", "RS1504969", "RS1504970", "RS1504975", "RS1504978", "RS1504981", "RS1504984", "RS1504986", "RS1505004", "RS1505012", "RS1505014", "RS1505018", "RS1505019", "RS1505023", "RS1505025", "RS1505035", "RS1505047", "RS1505050", "RS1505059", "RS1505075", "RS1505076", "RS1505079", "RS1505085", "RS1505087", "RS1505089", "RS1505091", "RS1505094", "RS1505112", "RS1505142", "RS1505143", "RS1505147", "RS1505155", "RS1505160", "RS1505161", "RS1505162", "RS1505174", "RS1505184", "RS1505193", "RS1505196", "RS1505238", "RS1505246", "RS1505255", "RS1505257", "RS1505263", "RS1505274", "RS1505275", "RS1505276", "RS1505293", "RS1505294", "RS1505302", "RS1505315", "RS1505316", "RS1505329", "RS1505330", "RS1505341", "RS1505344", "RS1505347", "RS1505352", "RS1505357", "RS1505360", "RS1505362", "RS1505379", "RS1505380", "RS1505384", "RS1505385", "RS1505389", "RS1505392", "RS1505401", "RS1505403", "RS1505409", "RS1505410", "RS1505412", "RS1505416", "RS1505421", "RS1505425", "RS1505432", "RS1505442", "RS1505450", "RS1505452", "RS1505454", "RS1505455", "RS1505464", "RS1505467", "RS1505475", "RS1505482", "RS1505496", "RS1505497", "RS1505508", "RS1505510", "RS1505511", "RS1505512", "RS1505519", "RS1505523", "RS1505534", "RS1505544", "RS1505545", "RS1505551", "RS1505553", "RS1505560", "RS1505561", "RS1505562", "RS1505563", "RS1505566", "RS1505577", "RS1505583", "RS1505585", "RS1505606", "RS1505615", "RS1505616", "RS1505620", "RS1505621", "RS1505622", "RS1505623", "RS1505624", "RS1505628", "RS1505635", "RS1505636", "RS1505637", "RS1505638", "RS1505641", "RS1505645", "RS1505655", "RS1505668", "RS1505669", "RS1505674", "RS1505675", "RS1505677", "RS1505678", "RS1505688", "RS1505692", "RS1505695", "RS1505700", "RS1505702", "RS1505710", "RS1505711", "RS1505713", "RS1505728", "RS1505729", "RS1505734", "RS1505735", "RS1505741", "RS1505742", "RS1505743", "RS1505747", "RS1505754", "RS1505756", "RS1505762", "RS1505767", "RS1505776", "RS1505778", "RS1505784", "RS1505785", "RS1505788", "RS1505791", "RS1505794", "RS1505799", "RS1505802", "RS1505810", "RS1505811", "RS1505816", "RS1505817", "RS1505827", "RS1505829", "RS1505838", "RS1505839", "RS1505848", "RS1505852", "RS1505853", "RS1505857", "RS1505860", "RS1505865", "RS1505869", "RS1505877", "RS1505885", "RS1505888", "RS1505891", "RS1505892", "RS1505910", "RS1505914", "RS1505915", "RS1505925", "RS1505926", "RS1505928", "RS1505929", "RS1505932", "RS1505935", "RS1505936", "RS1505938", "RS1505959", "RS1505960", "RS1505963", "RS1505964", "RS1505969", "RS1505981", "RS1505985", "RS1505989", "RS1505991", "RS1505992", "RS1505993", "RS1505996", "RS1506001", "RS1506003", "RS1506006", "RS1506010", "RS1506014", "RS1506018", "RS1506022", "RS1506026", "RS1506034", "RS1506046", "RS1506047", "RS1506049", "RS1506055", "RS1506061", "RS1506066", "RS1506068", "RS1506071", "RS1506077", "RS1506088", "RS1506090", "RS1506092", "RS1506094", "RS1506128", "RS1506138", "RS1506144", "RS1506147", "RS1506154", "RS1506155", "RS1506185", "RS1506188", "RS1506215", "RS1506219", "RS1506221", "RS1506222", "RS1506225", "RS1506232", "RS1506241", "RS1506242", "RS1506245", "RS1506246", "RS1506249", "RS1506259" };


                sw.Restart();
                foreach (var c in contracts) {
                    fo = JsonConvert.SerializeObject(serv.GetById(AffaireType.Contrat, c, 0), Formatting.Indented);
                    Console.WriteLine(fo);
                }
                sw.Stop();
                Console.WriteLine($"Elapsed {sw.ElapsedMilliseconds } / {contracts.Length} => {1.0 * sw.ElapsedMilliseconds / contracts.Length}  ");
            } finally {
                mgr.EndScope(mgr.CurrentScope);
            }
        }
    }
}
