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
using Albingia.Kheops.OP.Domain.Affaire;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Transactions;
using FluentAssertions.Equivalency;
using System.Text.RegularExpressions;
using System.IO;
using Albingia.Kheops.OP.Domain.Referentiel;
using System.Data.EasycomClient;
using System.Data;

namespace Test.Albingia.Kheops.OP.DataAdapter
{
    [TestClass]
    public class TestFormuleRepo
    {
        private const string contrat = "RS1803147"; // "RC1800088" ;

        [TestMethod]
        public void GetAffaireShouldNotThrow()
        {
            using (var c = TestSetup.AppDomainSetup()) {
                var rep = c.GetInstance<IFormuleRepository>();
                Step(rep);
            }

        }

        [TestMethod]
        public void SaveAffaireEstStable()
        {

            using (var c = TestSetup.AppDomainSetup()) {
                var rep = c.GetInstance<IFormuleRepository>();
                var sw = new Stopwatch();
                sw.Start();
                var affs = rep.GetFormulesForAffaire(new AffaireId() { TypeAffaire = AffaireType.Contrat, CodeAffaire = contrat, NumeroAliment = 0 });
                var aff = affs.First();
                sw.Stop();
                double t1 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
                sw.Restart();
                var j = JsonConvert.SerializeObject(aff, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, TypeNameHandling = TypeNameHandling.All });
                if (File.Exists(@".\\formule.json")) {
                    File.Delete(@".\\formule.json");
                }
                File.WriteAllText(".\\formule.json", j);
                
                sw.Stop();
                double t2 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;

                sw.Restart();
                rep.SaveFormule(aff, "test");
                sw.Stop();
                double t3 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
                sw.Restart();
                var aff2 = rep.GetFormulesForAffaire(new AffaireId() { TypeAffaire = AffaireType.Contrat, CodeAffaire = contrat, NumeroAliment = 0 }).First();
                sw.Stop();
                double t4 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
                aff2.Should().BeEquivalentTo(aff, cf => cf.IgnoringCyclicReferences().WithTracing().ComparingEnumsByName().ExcludingFields().Excluding((IMemberInfo x) => x.SelectedMemberPath.EndsWith("IsSelected")));
                Console.WriteLine($"{j}");
                Console.WriteLine($"time = {t1:.00} - {t2:.00} - {t3:.00} - {t4:.00}");
            }

        }


        [TestMethod]
        public void SaveAffaire()
        {

            using (var c = TestSetup.AppDomainSetup()) {
                var rep = c.GetInstance<IFormuleRepository>();
                var sw = new Stopwatch();
                sw.Start();
                var formule = rep.GetFormulesForAffaire(new AffaireId() { TypeAffaire = AffaireType.Contrat, CodeAffaire = contrat, NumeroAliment = 0 }).First();
                sw.Stop();
                double t1 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
                sw.Restart();
                var j = JsonConvert.SerializeObject(formule, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                sw.Stop();
                double t2 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;


                formule.Options?.ForEach(x => {
                    x.OptionVolets?.ForEach(ov => {
                        ov.IsChecked = true;
                        ov.Blocs?.ForEach(b => {
                            b.IsChecked = true;
                            b.Garanties?.ForEach(g1 => {
                                g1.SousGaranties?.Concat(g1.SousGaranties?.SelectMany(g2 => g2.SousGaranties?.Concat(g2.SousGaranties?.SelectMany(g3 => g3.SousGaranties))))?.ToList().ForEach(g => { g.NatureRetenue = NatureValue.Accordee; });
                            });
                        });
                    });
                });

                sw.Restart();
                rep.SaveFormule(formule, "test");
                sw.Stop();
                double t3 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
                sw.Restart();
                var aff2 = rep.GetFormulesForAffaire(new AffaireId() { TypeAffaire = AffaireType.Contrat, CodeAffaire = contrat, NumeroAliment = 0 }).First();

                sw.Stop();
                double t4 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
                aff2.Should().BeEquivalentTo(formule, 
                    cf => cf
                    .IgnoringCyclicReferences()
                    .WithTracing()
                    .ComparingEnumsByName()
                    .ExcludingFields()
                    .Excluding((IMemberInfo x)=>x.SelectedMemberPath.EndsWith("IsSelected"))
                );

                Console.WriteLine($"time = {t1:.00} - {t2:.00} - {t3:.00} - {t4:.00}");
            }

        }

        [TestMethod]
        public void SaveAffaireCleanup()
        {

            using (var c = TestSetup.AppDomainSetup()) {
                const int aliment = 0;
                var rep = c.GetInstance<IFormuleRepository>();
                var sw = new Stopwatch();
                sw.Start();
                var formule = rep.GetFormulesForAffaire(new AffaireId() { TypeAffaire = AffaireType.Contrat, CodeAffaire = contrat, NumeroAliment = 0 }).First(x => x.FormuleNumber == 1);
                sw.Stop();
                double t1 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
                sw.Restart();
                var j = JsonConvert.SerializeObject(formule, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                sw.Stop();
                double t2 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;

                //var refCountGaran = GetValue(c, $"select count(1) from KPGARAN where KDEIPB = '{contrat}' and KDEALX={aliment} and KDEFOR=1");

                formule.Options?.ForEach(x => {
                    x.OptionVolets?.ForEach(ov => {
                        ov.IsChecked = false;
                    });
                });

                sw.Restart();
                rep.SaveFormule(formule, "test");
                sw.Stop();
                double t3 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
                sw.Restart();
                var aff2 = rep.GetFormulesForAffaire(new AffaireId() { TypeAffaire = AffaireType.Contrat, CodeAffaire = contrat, NumeroAliment = aliment }).First();

                sw.Stop();
                double t4 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
                aff2.Options.ForEach(x => x.OptionVolets.ForEach(y => y.IsChecked.Should().BeFalse()));

                GetValue(c, $"select count(1) from KPGARTAR where KDGIPB = '{contrat}' and KDGALX={aliment} and KDGFOR=1").Should().Be(0);
                GetValue(c, $"select count(1) from KPGARAN where KDEIPB = '{contrat}' and KDEALX={aliment} and KDEFOR=1").Should().Be(0);
                GetValue(c, $"select count(1) from KPOPTD where KDCIPB = '{contrat}' and KDCALX={aliment} and KDCFOR=1").Should().BeGreaterThan(0);
                GetValue(c, $"select count(1) from KPINVEN left join KPGARAN on KBEIPB = KDEIPB where KBEIPB = '{contrat}' and KDEALX={aliment} and kdeipb is null").Should().Be(0);
                GetValue(c, $"select count(1) from KPINVED left join KPGARAN on KBFIPB = KDEIPB where KBFIPB = '{contrat}' and KDEALX={aliment} and kdeipb is null").Should().Be(0);

                Console.WriteLine($"time = {t1:.00} - {t2:.00} - {t3:.00} - {t4:.00}");
            }

        }

        private static int GetValue(IServiceContainer c, string sql)
        {
            var db = c.GetInstance<IDbConnection>();
            using (var com = db.CreateCommand()) 
            {
                com.CommandText = sql;
                return (int)com.ExecuteScalar();
            }
        }

        [TestMethod]
        public void SaveAffaireEstEffectif()
        {
            using (var c = TestSetup.AppDomainSetup()) {
                var rep = c.GetInstance<IFormuleRepository>();
                var sw = new Stopwatch();
                sw.Start();
                const string numForm = contrat;
                var formule = rep.GetFormulesForAffaire(new AffaireId() { TypeAffaire = AffaireType.Contrat, CodeAffaire = numForm, NumeroAliment = 0 }).OrderBy(x => x.FormuleNumber).First();
                sw.Stop();
                double t1 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
                sw.Restart();
                var j = JsonConvert.SerializeObject(formule, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                sw.Stop();
                double t2 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;

                formule.Id = 0L;
                formule.Options?.ForEach(x => {
                    x.Id = 0;
                    x.OptionVolets?.ForEach(ov => {
                        ov.Id = 0;
                        ov.Blocs?.ForEach(b => {
                            b.Id = 0;
                            b.Garanties?.ForEach(g1 => {
                                if (g1.InventaireSpecifique ?? false) {

                                }
                                g1.Id = 0;
                                g1.SousGaranties?.Concat(g1.SousGaranties?.SelectMany(g2 => g2.SousGaranties?.Concat(g2.SousGaranties?.SelectMany(g3 => g3.SousGaranties))))?.ToList().ForEach(g => { g.Id = 0; });
                            });
                        });
                    });
                    x.OptionNumber++;
                });
                formule.FormuleNumber++;
                formule.Chrono++;


                sw.Restart();
                rep.SaveFormule(formule, "test");
                sw.Stop();
                double t3 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
                sw.Restart();
                var affs = rep.GetFormulesForAffaire(new AffaireId() { TypeAffaire = AffaireType.Contrat, CodeAffaire = numForm, NumeroAliment = 0 }).ToList();
                var aff2 = affs.First(x => x.FormuleNumber == formule.FormuleNumber);
                sw.Stop();
                double t4 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
                aff2.Should().BeEquivalentTo(formule, cf => cf.IgnoringCyclicReferences()/*.WithTracing()*/.ComparingEnumsByName().ExcludingFields().Excluding(
                    (IMemberInfo x) =>
                    x.SelectedMemberPath.EndsWith("Id")
                    || x.SelectedMemberPath.EndsWith(".FormuleNumber")
                    || x.SelectedMemberPath.EndsWith(".Chrono")
                    || x.SelectedMemberPath.EndsWith(".Applications")
                    || x.SelectedMemberPath.EndsWith(".IsSelected")
                    || x.SelectedMemberPath.EndsWith("Calculee")
                    || new Regex(@"Options\[\d+\].OptionVolets\[\d+\].ParamModele").IsMatch(x.SelectedMemberPath)
                    ));
                Console.WriteLine($"{j}");
                Console.WriteLine($"times :\n - Get1 {t1:.00}\n - Ser  {t2:.00}\n - Save {t3:.00}\n - Get2 {t4:.00}");
            }

        }


        private static void Step(IFormuleRepository rep)
        {
            var sw = new Stopwatch();
            sw.Start();
            var aff = rep.GetFormulesForAffaire(new AffaireId() { TypeAffaire = AffaireType.Contrat, CodeAffaire = contrat, NumeroAliment = 0 }).ToArray();
            sw.Stop();
            double t1 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
            sw.Restart();
            var j = JsonConvert.SerializeObject(aff, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            sw.Stop();
            double t2 = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
            Console.WriteLine($"{j}");
            Console.WriteLine($"time = {t1:.00} - {t2:.00}");
        }

    }
}
