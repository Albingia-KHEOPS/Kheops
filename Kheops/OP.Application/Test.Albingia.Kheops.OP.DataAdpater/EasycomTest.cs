using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Data;
using System.Data.EasycomClient;
using System.Diagnostics;
using Test.Albingia.Kheops.OP.DataAdpater;

namespace Test.Albingia.Kheops.OP.DataAdapter
{
    [TestClass]
    public class EasycomTest
    {
        [TestMethod, Priority(99), Ignore]
        public void TestAura()
        {
            TestInternal();
        }
        [TestMethod, Priority(99), Ignore]
        public void TestAura2()
        {
            TestInternal();
        }
        [TestMethod, Priority(99), Ignore]
        public void TestAura3()
        {
            TestInternal();
        }
        [TestMethod, Priority(99), Ignore]
        public void TestAura4()
        {
            TestInternal();
        }
        [TestMethod, Priority(99), Ignore]
        public void TestAura5()
        {
            TestInternal();
        }

        private static void TestInternal()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            int i = 0;
            try {
                using (var c = ConnectionFactory()) {
                    for (i = 0; i < 10000; i++) {
                        using (var com = c.CreateCommand()) {
                            com.CommandText = "Select 1 from sysibm.sysdummy1";
                            using (var dr = com.ExecuteReader()) {
                                dr.Read();
                                dr.GetInt32(0);
                            }
                            //com.Dispose();
                        }
                    }
                }
            } catch (Exception e) {
                Trace.WriteLine($"{i} - {e}");
                throw;
            }
        }

        private static Easycom.EasycomConnection ConnectionFactory()
        {

            var c = new Easycom.EasycomConnection();
            c.ConnectionString = (ConfigurationManager.ConnectionStrings["EasyCom"].ConnectionString);
            if (c.State != ConnectionState.Closed) { c.Close(); }
            c.Open();
            return c;
        }


        [TestMethod, Ignore]
        public void TestInsertOpt()
        {
            const string insert = @"INSERT INTO  KPOPT (
KDBID, KDBTYP, KDBIPB, KDBALX, KDBFOR
, KDBKDAID, KDBOPT, KDBDESC, KDBFORR, KDBKDAIDR
, KDBSPEID, KDBCRU, KDBCRD, KDBCRH, KDBMAJU
, KDBMAJD, KDBMAJH, KDBPAQ, KDBACQ, KDBTMC
, KDBTFF, KDBTFP, KDBPRO, KDBTMI, KDBTFM
, KDBCMC, KDBCFO, KDBCHT, KDBCTT, KDBCCP
, KDBVAL, KDBVAA, KDBVAW, KDBVAT, KDBVAU
, KDBVAH, KDBIVO, KDBIVA, KDBIVW, KDBAVE
, KDBAVG, KDBECO, KDBAVA, KDBAVM, KDBAVJ
, KDBEHH, KDBEHC, KDBEHI, KDBASVALO, KDBASVALA
, KDBASVALW, KDBASUNIT, KDBASBASE, KDBGER
) VALUES (
:KDBID, :KDBTYP, :KDBIPB, :KDBALX, :KDBFOR
, :KDBKDAID, :KDBOPT, :KDBDESC, :KDBFORR, :KDBKDAIDR
, :KDBSPEID, :KDBCRU, :KDBCRD, :KDBCRH, :KDBMAJU
, :KDBMAJD, :KDBMAJH, :KDBPAQ, :KDBACQ, :KDBTMC
, :KDBTFF, :KDBTFP, :KDBPRO, :KDBTMI, :KDBTFM
, :KDBCMC, :KDBCFO, :KDBCHT, :KDBCTT, :KDBCCP
, :KDBVAL, :KDBVAA, :KDBVAW, :KDBVAT, :KDBVAU
, :KDBVAH, :KDBIVO, :KDBIVA, :KDBIVW, :KDBAVE
, :KDBAVG, :KDBECO, :KDBAVA, :KDBAVM, :KDBAVJ
, :KDBEHH, :KDBEHC, :KDBEHI, :KDBASVALO, :KDBASVALA
, :KDBASVALW, :KDBASUNIT, :KDBASBASE, :KDBGER)";

            using (var c = TestSetup.AppDomainSetup()) {
                var con = c.GetInstance<System.Data.IDbConnection>() as EacConnection;
                using (var com = con.CreateCommand()) {
                    com.Parameters.Add(new EacParameter("KDBID", DbType.Int64) { Value = 34527L });
                    com.Parameters.Add(new EacParameter("KDBTYP", DbType.AnsiStringFixedLength) { Value = "P" });
                    com.Parameters.Add(new EacParameter("KDBIPB", DbType.AnsiStringFixedLength) { Value = "RS1803147" });
                    com.Parameters.Add(new EacParameter("KDBALX", DbType.Int32) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBFOR", DbType.Int32) { Value = 6 });
                    com.Parameters.Add(new EacParameter("KDBKDAID", DbType.Int64) { Value = 34575L });
                    com.Parameters.Add(new EacParameter("KDBOPT", DbType.Int32) { Value = 1 });
                    com.Parameters.Add(new EacParameter("KDBDESC", DbType.AnsiStringFixedLength) { Value = "" });
                    com.Parameters.Add(new EacParameter("KDBFORR", DbType.Int32) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBKDAIDR", DbType.Int64) { Value = 0L });
                    com.Parameters.Add(new EacParameter("KDBSPEID", DbType.Int64) { Value = 0L });
                    com.Parameters.Add(new EacParameter("KDBCRU", DbType.AnsiStringFixedLength) { Value = "test" });
                    com.Parameters.Add(new EacParameter("KDBCRD", DbType.Int32) { Value = 20180911 });
                    com.Parameters.Add(new EacParameter("KDBCRH", DbType.Int32) { Value = 1618 });
                    com.Parameters.Add(new EacParameter("KDBMAJU", DbType.AnsiStringFixedLength) { Value = "test" });
                    com.Parameters.Add(new EacParameter("KDBMAJD", DbType.Int32) { Value = 20180911 });
                    com.Parameters.Add(new EacParameter("KDBMAJH", DbType.Int32) { Value = 1618 });
                    com.Parameters.Add(new EacParameter("KDBPAQ", DbType.AnsiStringFixedLength) { Value = "" });
                    com.Parameters.Add(new EacParameter("KDBACQ", DbType.Decimal) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBTMC", DbType.Decimal) { Value = 733.94 });
                    com.Parameters.Add(new EacParameter("KDBTFF", DbType.Decimal) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBTFP", DbType.Decimal) { Value = 1 });
                    com.Parameters.Add(new EacParameter("KDBPRO", DbType.AnsiStringFixedLength) { Value = "" });
                    com.Parameters.Add(new EacParameter("KDBTMI", DbType.AnsiStringFixedLength) { Value = "" });
                    com.Parameters.Add(new EacParameter("KDBTFM", DbType.AnsiStringFixedLength) { Value = "" });
                    com.Parameters.Add(new EacParameter("KDBCMC", DbType.Decimal) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBCFO", DbType.AnsiStringFixedLength) { Value = "N" });
                    com.Parameters.Add(new EacParameter("KDBCHT", DbType.Decimal) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBCTT", DbType.Decimal) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBCCP", DbType.Decimal) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBVAL", DbType.Decimal) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBVAA", DbType.Decimal) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBVAW", DbType.Decimal) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBVAT", DbType.AnsiStringFixedLength) { Value = "" });
                    com.Parameters.Add(new EacParameter("KDBVAU", DbType.AnsiStringFixedLength) { Value = "" });
                    com.Parameters.Add(new EacParameter("KDBVAH", DbType.AnsiStringFixedLength) { Value = "" });
                    com.Parameters.Add(new EacParameter("KDBIVO", DbType.Decimal) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBIVA", DbType.Decimal) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBIVW", DbType.Decimal) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBAVE", DbType.Int32) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBAVG", DbType.Int32) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBECO", DbType.AnsiStringFixedLength) { Value = "" });
                    com.Parameters.Add(new EacParameter("KDBAVA", DbType.Int32) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBAVM", DbType.Int32) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBAVJ", DbType.Int32) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBEHH", DbType.Decimal) { Value = 733.94 });
                    com.Parameters.Add(new EacParameter("KDBEHC", DbType.Decimal) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBEHI", DbType.Decimal) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBASVALO", DbType.Decimal) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBASVALA", DbType.Decimal) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBASVALW", DbType.Decimal) { Value = 0 });
                    com.Parameters.Add(new EacParameter("KDBASUNIT", DbType.AnsiStringFixedLength) { Value = "" });
                    com.Parameters.Add(new EacParameter("KDBASBASE", DbType.AnsiStringFixedLength) { Value = "" });
                    com.Parameters.Add(new EacParameter("KDBGER", DbType.Decimal) { Value = 0 });

                    com.CommandText = insert;

                    com.ExecuteNonQuery();
                }

            }
        }
    }
}
