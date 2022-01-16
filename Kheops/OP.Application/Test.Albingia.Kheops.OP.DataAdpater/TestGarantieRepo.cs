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
using Newtonsoft.Json.Linq;
using Albingia.Kheops.OP.Domain.Affaire;
using Newtonsoft.Json;
using System.IO;

namespace Test.Albingia.Kheops.OP.DataAdapter
{
    [TestClass]
    public class TestGarantiesRepo
    {

        private const string contrat = "RS1803147"; // "RC1800088" ;

        [TestMethod]
        public void GetAffaireShouldNotThrow()
        {
            var c = TestSetup.AppDomainSetup();
            var rep = c.GetInstance<IAffaireRepository>();
            Step(rep);
            Step(rep);

        }
        [TestMethod]
        public void GetRisques()
        {
            var c = TestSetup.AppDomainSetup();
            var risqueRepository = c.GetInstance<IRisqueRepository>();
            var rsq = risqueRepository.GetRisquesByAffaire((new AffaireId() { TypeAffaire = AffaireType.Contrat, CodeAffaire = contrat, NumeroAliment = 0 }));
            var j = JsonConvert.SerializeObject(rsq, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, TypeNameHandling = TypeNameHandling.All });
            if (!File.Exists(@".\\risques.json")) {
                File.Delete(@".\\risques.json");
            }
            File.WriteAllText(".\\risques.json", j);
        }

        [TestMethod]
        public void GetAffaireHistoShouldNotThrow()
        {
            var c = TestSetup.AppDomainSetup();
            var rep = c.GetInstance<IAffaireRepository>();
            StepHisto(rep);

        }
        private static void StepHisto(IAffaireRepository rep)
        {
            var sw = new Stopwatch();
            sw.Start();
            var aff = rep.GetById(AffaireType.Contrat, "RS1603015", 0, 0);
            sw.Stop();
            double t1 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
            sw.Restart();
            var j = JObject.FromObject(aff).ToString(Newtonsoft.Json.Formatting.Indented);
            sw.Stop();
            double t2 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
            Console.WriteLine($"{j}");
            Console.WriteLine($"time = {t1:.00} - {t2:.00}");
        }

        private static void Step(IAffaireRepository rep)
        {
            var sw = new Stopwatch();
            sw.Start();
            var aff = rep.GetById(AffaireType.Contrat, "RS1504215", 0);
            sw.Stop();
            double t1 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
            sw.Restart();
            var j = JObject.FromObject(aff).ToString(Newtonsoft.Json.Formatting.Indented);
            sw.Stop();
            double t2 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
            Console.WriteLine($"time = {t1:.00} - {t2:.00}");
        }
    }
}
