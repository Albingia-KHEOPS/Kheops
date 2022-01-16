using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Application.Infrastructure.Services;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Extension;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Model;
using Albingia.Kheops.OP.Domain.Parametrage.Formule;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Kheops.OP.Domain.Risque;
using Albingia.Kheops.OP.Service;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Test.Albingia.Kheops.OP.Domain {
    [TestClass]
    public class TestAffairePort
    {

        [TestMethod]
        public void GetAffaireShouldReturn()
        {
            IFixture fixture = InitFixture();

            Mock<IAffaireRepository> r = fixture.Freeze<Mock<IAffaireRepository>>();
            Affaire a = fixture.Freeze<Affaire>();

            IAffairePort affaire = fixture.Create<AffaireService>();
            var aff = affaire.GetAffaire(new AffaireId { TypeAffaire = AffaireType.Contrat, CodeAffaire = "TEST", NumeroAliment = 0 });
            r.Verify(x => x.GetById(It.IsAny<AffaireType>(), It.IsAny<String>(), It.IsAny<int>(), It.IsAny<int?>(), false));
            aff.Should().BeEquivalentTo(a, c => c.IgnoringCyclicReferences().WithTracing().ComparingEnumsByName().ExcludingFields().Excluding(p => p.CibleCategorie).ExcludingMissingMembers());//.IncludingNestedObjects());
            Console.WriteLine(JsonConvert.SerializeObject(aff));
        }

        [TestMethod]
        public void GetAffaireHistoShouldMap()
        {
            IFixture fixture = InitFixture();

            Mock<IAffaireRepository> r = fixture.Freeze<Mock<IAffaireRepository>>();
            Affaire a = fixture.Freeze<Affaire>();

            IAffairePort affaire = fixture.Create<AffaireService>();
            var aff = affaire.GetAffaire(new AffaireId { TypeAffaire = AffaireType.Contrat, CodeAffaire = "TEST", NumeroAliment = 0, NumeroAvenant = 12, IsHisto = true });
            r.Verify(x => x.GetById(It.IsAny<AffaireType>(), It.IsAny<String>(), It.IsAny<int>(), It.IsAny<int?>(), true));
            aff.Should().BeEquivalentTo(a, c => c.IgnoringCyclicReferences().WithTracing().ComparingEnumsByName().ExcludingFields().Excluding(p => p.CibleCategorie).ExcludingMissingMembers());//.IncludingNestedObjects());
            Console.WriteLine(JsonConvert.SerializeObject(aff));
        }
        [TestMethod]
        public void ValidationOK()
        {
            MapperConfig.Init();
            IFixture fixture = InitFixture();

            Mock<IAffaireRepository> r = fixture.Freeze<Mock<IAffaireRepository>>();
            Mock<IFormuleRepository> f = fixture.Freeze<Mock<IFormuleRepository>>();

            var content = File.ReadAllText(".\\formule.json");
            Formule formule = JsonConvert.DeserializeObject<Formule>(content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            var v = formule.Validate();
            v.Should().BeEmpty();
            Console.WriteLine(JsonConvert.SerializeObject(v));
        }
        [TestMethod]
        public void ValidationBlocs()
        {
            MapperConfig.Init();
            IFixture fixture = InitFixture();

            Mock<IAffaireRepository> r = fixture.Freeze<Mock<IAffaireRepository>>();
            Mock<IFormuleRepository> f = fixture.Freeze<Mock<IFormuleRepository>>();
            SetupData(fixture);
            var affaire = fixture.Create<Affaire>();
            var formule = fixture.Create<Formule>();
            var paramVol = fixture.Create<IEnumerable<ParamVolet>>();
            var truthTable = fixture.Create<IDictionary<(CaractereSelection, NatureValue), NatureSelection>>();
            formule.ApplyParameters(affaire, paramVol, truthTable, false, TypologieModele.Standard, new DateTime(2018, 3, 12));
            formule.Options.First().OptionVolets.First().Blocs.ForEach(x => x.IsChecked = true);
            var v = formule.Validate();
            v.Should().NotBeEmpty();
            Console.WriteLine(JsonConvert.SerializeObject(v));
        }

        [TestMethod]
        public void ValidationAll()
        {
            MapperConfig.Init();
            IFixture fixture = InitFixture();

            Mock<IAffaireRepository> r = fixture.Freeze<Mock<IAffaireRepository>>();
            Mock<IFormuleRepository> f = fixture.Freeze<Mock<IFormuleRepository>>();
            SetupData(fixture);
            var truthTable = fixture.Create<IDictionary<(CaractereSelection, NatureValue), NatureSelection>>();

            Formule formule;
            List<OptionBloc> blocs;
            (formule, blocs) = SetupFormule(fixture, truthTable);

            var g1 = blocs.SelectMany(x => x.Garanties);
            var g2 = g1.SelectMany(x => x.SousGaranties);
            var g3 = g2.SelectMany(x => x.SousGaranties);
            var g4 = g3.SelectMany(x => x.SousGaranties);
            var gall = g1.Union(g2).Union(g3).Union(g4).ToList();
            gall[4].ParamRelations.Add(new GarantieRelation { EsclaveId = gall[0].ParamGarantie.Sequence, MaitreId = gall[4].ParamGarantie.Sequence, Relation = TypeRelation.Incompatible });
            gall.ForEach(x => x.UpdateCheck(truthTable, NatureValue.Accordee, true));

            var v = formule.Validate();
            v.Should().NotBeEmpty();
            Console.WriteLine(JsonConvert.SerializeObject(v));
        }

        private static (Formule formule, List<OptionBloc> blocs) SetupFormule(IFixture fixture, IDictionary<(CaractereSelection, NatureValue), NatureSelection> truthTable)
        {
            var formule = fixture.Create<Formule>();
            var paramVol = fixture.Create<IEnumerable<ParamVolet>>();
            var affaire = fixture.Create<Affaire>();

            formule.ApplyParameters(affaire, paramVol, truthTable, false, TypologieModele.Standard, new DateTime(2018, 3, 12));
            var blocs = formule.Options.First().OptionVolets.First().Blocs;
            blocs.ForEach(x => x.IsChecked = true);
            return (formule, blocs);
        }

        [TestMethod]
        public void GetFormulesShouldMap()
        {
            MapperConfig.Init();
            IFixture fixture = InitFixture();

            Mock<IAffaireRepository> r = fixture.Freeze<Mock<IAffaireRepository>>();
            Mock<IFormuleRepository> f = fixture.Freeze<Mock<IFormuleRepository>>();
            SetupData(fixture);

            var source = fixture.Create<IEnumerable<Formule>>().ToList();

            f.Setup(x => x.GetFormulesForAffaire(It.IsAny<AffaireId>())).Returns(source);

            IFormulePort affaireServ = fixture.Create<FormuleService>();
            var b = affaireServ.GetFormulesAffaire(new AffaireId { TypeAffaire = AffaireType.Contrat, CodeAffaire = "TEST", NumeroAliment = 0 });
            f.Verify(x => x.GetFormulesForAffaire(It.IsAny<AffaireId>()));

            b.Formules.Should().BeEquivalentTo(source,
                 c => c.AllowingInfiniteRecursion().ExcludingMissingMembers().IncludingNestedObjects().WithStrictOrdering().IgnoringCyclicReferences().ExcludingMissingMembers()
                 .Excluding(predicate: x => x.SelectedMemberPath.EndsWith("NumeroAvenant"))
                 /*.IgnoringCyclicReferences().WithTracing().ComparingEnumsByName().ExcludingFields()*/);//.IncludingNestedObjects());
            Console.WriteLine(JsonConvert.SerializeObject(b));
        }

        private static void SetupData(IFixture fixture)
        {
            fixture.Inject<ILiveDataCache>(new NoCache());


            var content = File.ReadAllText(".\\formule.json");
            Formule formule = JsonConvert.DeserializeObject<Formule>(content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });
            content = File.ReadAllText(".\\paramvolets.json");
            var pvolets = JsonConvert.DeserializeObject<List<ParamVolet>>(content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });
            content = File.ReadAllText(".\\risques.json");
            var risques = JsonConvert.DeserializeObject<List<Risque>>(content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });
            var affaire = new Affaire {
                CodeAffaire = formule.AffaireId.CodeAffaire,
                TypeAffaire = formule.AffaireId.TypeAffaire,
                NumeroAliment = formule.AffaireId.NumeroAliment,
                NumeroAvenant = formule.AffaireId.NumeroAvenant ?? 0,
                Etat = EtatAffaire.NonValidee,
                Categorie = "TRIO",
                Branche = new Branche { Code = "RS" },
                Cible = new Cible { Code = "SPECTACLE" },
                DateEffet = new DateTime(2018, 3, 12)

            };
            var table = new string[][] {
                new []{"B","A","O","N","A","A"},
                new []{"B","C","O","O","C","E"},
                new []{"B","E","N","O","C","E"},
                new []{"F","A","N","O","A"," "},
                new []{"F","C","N","O","C"," "},
                new []{"F","E","O","O","A","E"},
                new []{"O","A","O","N","A","A"},
                new []{"O","C","O","N","C","C"},
                new []{"O","E","N","N","E","E"},
                new []{"P","A","O","O","A"," "},
                new []{"P","C","O","O","C"," "},
                new []{"P","E","N","O","A","E"}
                }.Select(v =>
                new KeyValuePair<(CaractereSelection, NatureValue), NatureSelection>(
                       (v[0].AsEnum<CaractereSelection>(), v[1].AsEnum<NatureValue>()),
                       new NatureSelection { IsAffiche = v[2].AsBool(), IsModifiable = v[3].AsBool(), Checked = v[4].AsEnum<NatureValue>(), Unchecked = v[5].AsEnum<NatureValue>() }
               )
            );
            var truthTable = table.ToDictionary(x => x.Key, y => y.Value);
            fixture.Inject<IDictionary<(CaractereSelection, NatureValue), NatureSelection>>(truthTable);
            fixture.Inject(affaire);
            fixture.Inject<IEnumerable<Risque>>(risques);
            fixture.Inject<IEnumerable<ParamVolet>>(pvolets);
            fixture.Inject<IEnumerable<Formule>>(new Formule[] { formule });
            fixture.Inject<Formule>(formule);
        }

        [TestMethod]
        public void SetFormuleGarantieSelected()
        {
            MapperConfig.Init();
            IFixture fixture = InitFixture();
            Mock<IAffaireRepository> r = fixture.Freeze<Mock<IAffaireRepository>>();
            Mock<IFormuleRepository> f = fixture.Freeze<Mock<IFormuleRepository>>();

            SetupData(fixture);
            var source = fixture.Create<IEnumerable<Formule>>().ToList();

            f.Setup(x => x.GetFormulesForAffaire(It.IsAny<AffaireId>())).Returns(source);

            IFormulePort affaireService = fixture.Create<FormuleService>();
            var b = affaireService.GetFormulesAffaire(new AffaireId { TypeAffaire = AffaireType.Contrat, CodeAffaire = "TEST", NumeroAliment = 0 });
            f.Verify(x => x.GetFormulesForAffaire(It.IsAny<AffaireId>()));

            //formule.SetGaratieStatus(idGaran);
            b.Formules.Should().BeEquivalentTo(source,
                 c => c.AllowingInfiniteRecursion().ExcludingMissingMembers().IncludingNestedObjects().WithStrictOrdering().IgnoringCyclicReferences().ExcludingMissingMembers() /*.IgnoringCyclicReferences().WithTracing().ComparingEnumsByName().ExcludingFields()*/);//.IncludingNestedObjects());
            Console.WriteLine(JsonConvert.SerializeObject(b));
        }

        private static IFixture InitFixture()
        {
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization() { ConfigureMembers = true });
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(beh => fixture.Behaviors.Remove(beh));
            int recursionDepth = 2;
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(recursionDepth));
            return fixture;
        }

    }
}
