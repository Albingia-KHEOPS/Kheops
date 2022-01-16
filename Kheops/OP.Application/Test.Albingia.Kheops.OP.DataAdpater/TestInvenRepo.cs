using System;
using FluentAssertions;
using Albingia.Kheops.OP.Application.Port.Driven;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Albingia.Kheops.OP.DataAdpater;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Albingia.Kheops.OP.Domain.Referentiel;
using System.Data;
using Albingia.Kheops.OP.Domain.Inventaire;

namespace Test.Albingia.Kheops.OP.DataAdapter
{
    [TestClass]
    public class TestInvenRepo
    {
        [TestMethod]
        public void CantCommandRunOnDisposedConnection()
        {

            using (var c = TestSetup.AppDomainSetup()) {
                var db = c.GetInstance<IDbConnection>();
                    using (var com = db.CreateCommand()) {
                        com.CommandText = "select * from kpoptd where kdcid = 504695 ";
                        //com.CommandText = "update kpoptd set KDCMAJU='KO1' where kdcid = 504695 ";
                        com.ExecuteNonQuery();
                    }
                db.Dispose();
                db = c.GetInstance<IDbConnection>();
                    using (var com = db.CreateCommand()) {
                        com.CommandText = "select * from kpoptd where kdcid = 504695 ";
                        //com.CommandText = "update kpoptd set KDCMAJU='KO1' where kdcid = 504695 ";
                        Assert.ThrowsException<InvalidOperationException>(()=>com.ExecuteNonQuery());
                    }
                
            }
        }

        [TestMethod]
        public void GetInvenShouldNotThrow()
        {

            var ids = new List<Int64>();
            using (var c = TestSetup.AppDomainSetup()) {
                var con = c.GetInstance<IDbConnection>();
                using (var com = con.CreateCommand()) {
                    com.CommandText = "SELECT KBEID as ID from KPINVEN ORDER BY KBEID DESC FETCH FIRST 100 ROWS ONLY";
                    using (var dr = com.ExecuteReader()) {
                        while (dr.Read()) {
                            ids.Add(dr.GetInt64(0));
                        }
                        dr.Close();
                    }
                    (com as System.Data.EasycomClient.EacCommand)?.Dispose();
                }
            }
            var i = 0;
            using (var c = TestSetup.AppDomainSetup()) {
                var rep = c.GetInstance<IInventaireRepository>();
                foreach (var id in ids) {
                    i++;
                    Debug.WriteLineIf(i % 100 == 0, $"{i}");
                    if (i % 100 == 0) {
                        Console.WriteLine($"{i}");
                    }
                    try {
                        rep.GetInventaireById(id, null);
                    } catch (Exception e) {
                        Debug.WriteLine($"i={i},id={id},Exception: {e}");
                        Console.Error.WriteLine($"i={i},id={id},Exception: {e}");
                        throw;
                    }
                }
            }
        }

        [TestMethod]
        public void SaveInvenGarantieShouldNotThrow()
        {

            using (var c = TestSetup.AppDomainSetup()) {
                var rep = c.GetInstance<IInventaireRepository>();
                var id = 14871;
                var i = 1;
                try {
                    var inven = rep.GetInventaireById(id, null);
                    inven.Items.Cast<PersonneDesigneeIndispo>().ToList().ForEach(x => {
                        x.DateNaissance = new DateTime(2017, 01, 01);
                        x.Nom = "Toto_" + (i);
                        x.Prenom = "Titi_" + (i++);
                    });
                    inven.NumChrono++;
                    inven.Descriptif = "Designation";
                    inven.Items.Remove(inven.Items.Last());
                    inven.Items.Remove(inven.Items.Last());
                    inven.Items.Remove(inven.Items.Last());
                    inven.Items.Remove(inven.Items.Last());
                    rep.SaveInventaire(inven);
                    var inven2 = rep.GetInventaireById(id, null);
                    inven.Should().BeEquivalentTo(inven2);
                } catch (Exception e) {
                    Debug.WriteLine($"id={id},Exception: {e}");
                    Console.Error.WriteLine($"id={id},Exception: {e}");
                    throw;
                }

            }
        }

        [TestMethod]
        public void SaveInvenManifRemoveItemShouldNotThrow()
        {

            using (var c = TestSetup.AppDomainSetup()) {
                var rep = c.GetInstance<IInventaireRepository>();
                var id = 43768;
                var i = 1;
                try {
                    var inven = rep.GetInventaireById(id, null);
                    inven.Items.Cast<ManifestationAssure>().ToList().ForEach(x => {
                        x.DateDebut = new DateTime(2017, 01, 01);
                        x.DateFin = new DateTime(2017, 01, 05);
                        x.Designation = "Toto_" + (i);
                    });
                    inven.NumChrono++;
                    inven.Descriptif = "Designation";
                    inven.Items.Remove(inven.Items.Last());
                    inven.Items.Remove(inven.Items.Last());
                    inven.Items.Remove(inven.Items.Last());
                    inven.Items.Remove(inven.Items.Last());
                    rep.SaveInventaire(inven);
                    var inven2 = rep.GetInventaireById(id, null);
                    inven.Should().BeEquivalentTo(inven2);
                } catch (Exception e) {
                    Debug.WriteLine($"id={id},Exception: {e}");
                    Console.Error.WriteLine($"id={id},Exception: {e}");
                    throw;
                }

            }
        }
        [TestMethod]
        public void SaveInvenManifAddItemShouldNotThrow()
        {

            using (var c = TestSetup.AppDomainSetup()) {
                var rep = c.GetInstance<IInventaireRepository>();
                var id = 43768;
                var i = 1;
                try {
                    var inven = rep.GetInventaireById(id, null);
                    inven.Items.Cast<ManifestationAssure>().ToList().ForEach(x => {
                        x.DateDebut = new DateTime(2017, 01, 01);
                        x.DateFin = new DateTime(2017, 01, 05);
                        x.Designation = "Toto_" + (i);
                    });
                    inven.NumChrono++;
                    inven.Descriptif = "Designation";
                    inven.Items.Add(new ManifestationAssure {
                        DateDebut = new DateTime(2017, 01, 01),
                        DateFin = new DateTime(2017, 01, 05),
                        Designation = "Toto_200",
                        CodePostal = "75000",
                        Lieu = "Lieu à Paris",
                        Montant = 112,
                        NatureLieu = new NatureLieu { Code = "LPLA" },
                        Ville = "Paris 00"
                    });
                    rep.SaveInventaire(inven);
                    var inven2 = rep.GetInventaireById(id, null);
                    inven.Should().BeEquivalentTo(inven2);
                } catch (Exception e) {
                    Debug.WriteLine($"id={id},Exception: {e}");
                    Console.Error.WriteLine($"id={id},Exception: {e}");
                    throw;
                }

            }
        }

        [TestMethod]
        public void SaveInvenPersDesAddItemShouldNotThrow()
        {

            using (var c = TestSetup.AppDomainSetup()) {
                var rep = c.GetInstance<IInventaireRepository>();
                var id = 43918;
                var i = 1;
                try {
                    var inven = rep.GetInventaireById(id, null);
                    inven.Items.Cast<PersonneDesigneeIndispo>().ToList().ForEach(x => {
                        x.DateNaissance = new DateTime(2017, 01, i % 30);
                        x.Fonction = $"Fonc_{i}";
                        x.Nom = $"Toto_{i}";
                        x.Prenom = $"Titi_{i++}";
                        x.Montant = 112;
                        x.Franchise = "1234";
                    });
                    inven.NumChrono++;
                    inven.Descriptif = "Designation";
                    inven.Items.Add(new PersonneDesigneeIndispo {
                        DateNaissance = new DateTime(2017, 01, 05),
                        Fonction = "Toto_200",
                        Nom = "Toto_200",
                        Prenom = "Titi_200",
                        Montant = 112,
                        Franchise = "1234",
                        Extention = new Indisponibilite { Code = "ACCTO" }
                    });
                    rep.SaveInventaire(inven);
                    var inven2 = rep.GetInventaireById(id, null);
                    inven.Should().BeEquivalentTo(inven2);
                } catch (Exception e) {
                    Debug.WriteLine($"id={id},Exception: {e}");
                    Console.Error.WriteLine($"id={id},Exception: {e}");
                    throw;
                }

            }
        }
    }
}
