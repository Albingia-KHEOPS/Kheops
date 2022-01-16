using System;
using LightInject;
using FluentAssertions;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Albingia.Kheops.OP.DataAdpater;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Collections.Generic;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using Albingia.Kheops.OP.Domain.Referentiel;
using Newtonsoft.Json;
using System.IO;

namespace Test.Albingia.Kheops.OP.DataAdapter
{
    [TestClass]
    public class TestParamRepo
    {
        [TestMethod, Priority(0)]
        public void GetParamsVoletShouldReturn()
        {
            var c = TestSetup.AppDomainSetup();
            List<ParamVolet> volets = null;
            var sw = new Stopwatch();
            sw.Start();
            var rep = c.GetInstance<IParamRepository>();
            volets = rep.GetParamVolets().ToList();
            sw.Stop();
            double t1 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
            Console.WriteLine($"time = {t1:.00}");
            Console.WriteLine($"volets.Count = {volets.Count} ");
            volets.Count.Should().BeGreaterThan(2);
            var j = JsonConvert.SerializeObject(volets, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, TypeNameHandling = TypeNameHandling.All });
            if (!File.Exists(@".\\paramvolets.json")) {
                File.Delete(@".\\paramvolets.json");
            }
                File.WriteAllText(".\\paramvolets.json", j);
            
        }
        [TestMethod, Priority(10)]
        public void GetParamsVoletShouldbeFaster2dnTime()
        {
            var c = TestSetup.AppDomainSetup();
            var rep = c.GetInstance<IParamRepository>();
            var sw = new Stopwatch();
            sw.Start();
            var volets = rep.GetParamVolets().ToList();
            sw.Stop();

            double t1 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
            sw.Restart();
            volets = rep.GetParamVolets().ToList();
            sw.Stop();
            double t2 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;

            t2.Should().BeLessThan(t1);

            Console.WriteLine($"1st attempt = {t1:.00}");
            Console.WriteLine($"2nd attempt = {t2:.00}");

        }

        //[TestMethod]
        public void GetParamsVoletModelesGarantiesShouldBeResolved()
        {
            var c = TestSetup.AppDomainSetup();
            var rep = c.GetInstance<IParamRepository>();
            var sw = new Stopwatch();
            sw.Start();
            sw.Stop();

            double t1 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
            sw.Restart();
            var volets = rep.GetParamVolets().ToList();
            var blocs = volets.SelectMany(x => x.Blocs).ToList();
            var modeles = blocs.SelectMany(x => x.Modeles).ToList();
            var count = 0;
            //modeles.ForEach(x => x.Garanties.Should().HaveCountGreaterThan(0));
            var test = modeles.All(x => TestGaranties(x.Garanties, ref count));
            sw.Stop();

            test.Should().BeTrue();
            Console.WriteLine($"Volets = {volets.Count}");
            Console.WriteLine($"Blocs = {blocs.Count}");
            Console.WriteLine($"Modeles = {modeles.Count}");
            Console.WriteLine($"Garanties = {count}");
            Console.WriteLine($"Ellapsed = {t1:.00}");

        }

        [TestMethod]
        public void GetExpressionComplexeLci()
        {
            var c = TestSetup.AppDomainSetup();
            var rep = c.GetInstance<IParamRepository>();
            var sw = new Stopwatch();
            sw.Start();

            var exps = rep.GetExpressionLCIs();


            sw.Stop();
            double t1 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;

            exps.Should().HaveCountGreaterThan(0);
            Console.WriteLine($"Expr. LCI = {exps.Count()}");
            Console.WriteLine($"Ellapsed  = {t1:.00}");

        }
        [TestMethod]
        public void GetExpressionComplexeLFrh()
        {
            var c = TestSetup.AppDomainSetup();
            var rep = c.GetInstance<IParamRepository>();
            var sw = new Stopwatch();
            sw.Start();

            var exps = rep.GetExpressionFranchises();


            sw.Stop();
            double t1 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;

            exps.Should().HaveCountGreaterThan(0);
            Console.WriteLine($"Expr. Count = {exps.Count()}");
            Console.WriteLine($"Ellapsed  = {t1:.00}");

        }

        private static bool TestGaranties(IEnumerable<ParamGarantieHierarchie> gars, ref int i)
        {
            var t = i;
            var ret = gars.All(x => { t++; return x.ParamGarantie != null && TestGaranties(x.GarantiesChildren, ref t); });
            i = t;
            return ret;
        }

        [TestMethod]
        public void TestUsers()
        {
            DoWithRefRepo(refRepo => refRepo.GetUtilisateurs());
        }
        [TestMethod]
        public void TestTraitements()
        {
            DoWithRefRepo(refRepo =>refRepo.GetValues<Traitement>());
        }



        [TestMethod]
        public IEnumerable<Branche> GetBranches()
        {
            using (var c = TestSetup.AppDomainSetup()) {
                var refRepo = c.GetInstance<IReferentialRepository>();
                return refRepo.GetValues<Branche>();
            }
        }

        public R DoWithRepo<T, R>(Func<T, R> func)
        {
            using (var c = TestSetup.AppDomainSetup()) {
                var refRepo = c.GetInstance<T>();
                return func(refRepo);
            }
        }
        private R DoWithRefRepo<R>(Func<IReferentialRepository, R> func)
        {
            return DoWithRepo<IReferentialRepository, R>(func);
        }

        [TestMethod]
        public IEnumerable<Cible> GetCibles()
        {
            return DoWithRefRepo(r => r.GetCibles());
        }
        [TestMethod]
        public IEnumerable<Devise> GetDevises(CibleFiltre cible)
        {
            return DoWithRefRepo(r => r.GetValues<Devise>());
        }
        [TestMethod]
        public IEnumerable<Encaissement> GetEncaissements()
        {
            return DoWithRefRepo(r => r.GetValues<Encaissement>());
        }
        [TestMethod]
        public IEnumerable<Etat> GetEtats()
        {
            return DoWithRefRepo(r => r.GetValues<Etat>());
        }
        [TestMethod]
        public IEnumerable<Indice> GetIndices()
        {
            return DoWithRefRepo(r => r.GetValues<Indice>());
        }
        [TestMethod]
        public IEnumerable<MotifAvenant> GetMotifs()
        {
            return DoWithRefRepo(r => r.GetValues<MotifAvenant>());
        }
        [TestMethod]
        public IEnumerable<NatureAffaire> GetNatures()
        {
            return DoWithRefRepo(r => r.GetValues<NatureAffaire>());
        }
        [TestMethod]
        public IEnumerable<NatureGarantie> GetNaturesGarantie()
        {
            return DoWithRefRepo(r => r.GetValues<NatureGarantie>());
        }
        [TestMethod]
        public IEnumerable<Periodicite> GetPeriodicites()
        {
            return DoWithRefRepo(r => r.GetValues<Periodicite>());
        }
        [TestMethod]
        public IEnumerable<TypeAccord> GetRetours()
        {
            return DoWithRefRepo(r => r.GetValues<TypeAccord>());
        }
        [TestMethod]
        public IEnumerable<Situation> GetSituations()
        {
            return DoWithRefRepo(r => r.GetValues<Situation>());
        }
        [TestMethod]
        public IEnumerable<UniteFranchise> GetUniteFranchise(CibleFiltre cible)
        {
            return DoWithRefRepo(r => r.GetValues<UniteFranchise>());
        }
    }
}
