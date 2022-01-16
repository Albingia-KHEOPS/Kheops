using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using Dapper;
using Dapper.FluentMap.Mapping;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {

    public  partial class  KpOptRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
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
, KDBASVALW, KDBASUNIT, KDBASBASE, KDBGER FROM KPOPT
WHERE KDBID = :id
";
            const string update=@"UPDATE KPOPT SET 
KDBID = :KDBID, KDBTYP = :KDBTYP, KDBIPB = :KDBIPB, KDBALX = :KDBALX, KDBFOR = :KDBFOR, KDBKDAID = :KDBKDAID, KDBOPT = :KDBOPT, KDBDESC = :KDBDESC, KDBFORR = :KDBFORR, KDBKDAIDR = :KDBKDAIDR
, KDBSPEID = :KDBSPEID, KDBCRU = :KDBCRU, KDBCRD = :KDBCRD, KDBCRH = :KDBCRH, KDBMAJU = :KDBMAJU, KDBMAJD = :KDBMAJD, KDBMAJH = :KDBMAJH, KDBPAQ = :KDBPAQ, KDBACQ = :KDBACQ, KDBTMC = :KDBTMC
, KDBTFF = :KDBTFF, KDBTFP = :KDBTFP, KDBPRO = :KDBPRO, KDBTMI = :KDBTMI, KDBTFM = :KDBTFM, KDBCMC = :KDBCMC, KDBCFO = :KDBCFO, KDBCHT = :KDBCHT, KDBCTT = :KDBCTT, KDBCCP = :KDBCCP
, KDBVAL = :KDBVAL, KDBVAA = :KDBVAA, KDBVAW = :KDBVAW, KDBVAT = :KDBVAT, KDBVAU = :KDBVAU, KDBVAH = :KDBVAH, KDBIVO = :KDBIVO, KDBIVA = :KDBIVA, KDBIVW = :KDBIVW, KDBAVE = :KDBAVE
, KDBAVG = :KDBAVG, KDBECO = :KDBECO, KDBAVA = :KDBAVA, KDBAVM = :KDBAVM, KDBAVJ = :KDBAVJ, KDBEHH = :KDBEHH, KDBEHC = :KDBEHC, KDBEHI = :KDBEHI, KDBASVALO = :KDBASVALO, KDBASVALA = :KDBASVALA
, KDBASVALW = :KDBASVALW, KDBASUNIT = :KDBASUNIT, KDBASBASE = :KDBASBASE, KDBGER = :KDBGER
 WHERE 
KDBID = :id";
            const string delete=@"DELETE FROM KPOPT WHERE KDBID = :id";
            const string insert=@"INSERT INTO  KPOPT (
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
            const string select_GetByAffaire=@"SELECT
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
, KDBASVALW, KDBASUNIT, KDBASBASE, KDBGER FROM KPOPT
WHERE KDBTYP = :typeAffaire
and KDBIPB = :codeAffaire
and KDBALX = :version
";
            const string select_GetByFormule=@"SELECT
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
, KDBASVALW, KDBASUNIT, KDBASBASE, KDBGER FROM KPOPT
WHERE KDBKDAID = :formuleId
";
            #endregion

            public KpOptRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpOpt Get(Int64 id){
                return connection.Query<KpOpt>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KDBID") ;
            }

            public void Insert(KpOpt value){
                    if(value.Kdbid == default(Int64)) {
                        value.Kdbid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KDBID",value.Kdbid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDBTYP",value.Kdbtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDBIPB",value.Kdbipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDBALX",value.Kdbalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDBFOR",value.Kdbfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDBKDAID",value.Kdbkdaid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDBOPT",value.Kdbopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDBDESC",value.Kdbdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KDBFORR",value.Kdbforr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDBKDAIDR",value.Kdbkdaidr, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDBSPEID",value.Kdbspeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDBCRU",value.Kdbcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDBCRD",value.Kdbcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDBCRH",value.Kdbcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KDBMAJU",value.Kdbmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDBMAJD",value.Kdbmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDBMAJH",value.Kdbmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KDBPAQ",value.Kdbpaq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDBACQ",value.Kdbacq, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBTMC",value.Kdbtmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBTFF",value.Kdbtff, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBTFP",value.Kdbtfp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 9);
                    parameters.Add("KDBPRO",value.Kdbpro??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDBTMI",value.Kdbtmi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDBTFM",value.Kdbtfm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDBCMC",value.Kdbcmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBCFO",value.Kdbcfo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDBCHT",value.Kdbcht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBCTT",value.Kdbctt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBCCP",value.Kdbccp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 9);
                    parameters.Add("KDBVAL",value.Kdbval, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBVAA",value.Kdbvaa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBVAW",value.Kdbvaw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBVAT",value.Kdbvat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDBVAU",value.Kdbvau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDBVAH",value.Kdbvah??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDBIVO",value.Kdbivo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDBIVA",value.Kdbiva, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDBIVW",value.Kdbivw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDBAVE",value.Kdbave, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDBAVG",value.Kdbavg, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDBECO",value.Kdbeco??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDBAVA",value.Kdbava, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDBAVM",value.Kdbavm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDBAVJ",value.Kdbavj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDBEHH",value.Kdbehh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBEHC",value.Kdbehc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBEHI",value.Kdbehi, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBASVALO",value.Kdbasvalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBASVALA",value.Kdbasvala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBASVALW",value.Kdbasvalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBASUNIT",value.Kdbasunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDBASBASE",value.Kdbasbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDBGER",value.Kdbger, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpOpt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Kdbid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpOpt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDBID",value.Kdbid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDBTYP",value.Kdbtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDBIPB",value.Kdbipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDBALX",value.Kdbalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDBFOR",value.Kdbfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDBKDAID",value.Kdbkdaid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDBOPT",value.Kdbopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDBDESC",value.Kdbdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KDBFORR",value.Kdbforr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDBKDAIDR",value.Kdbkdaidr, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDBSPEID",value.Kdbspeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDBCRU",value.Kdbcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDBCRD",value.Kdbcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDBCRH",value.Kdbcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KDBMAJU",value.Kdbmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDBMAJD",value.Kdbmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDBMAJH",value.Kdbmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KDBPAQ",value.Kdbpaq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDBACQ",value.Kdbacq, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBTMC",value.Kdbtmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBTFF",value.Kdbtff, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBTFP",value.Kdbtfp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 9);
                    parameters.Add("KDBPRO",value.Kdbpro??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDBTMI",value.Kdbtmi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDBTFM",value.Kdbtfm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDBCMC",value.Kdbcmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBCFO",value.Kdbcfo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDBCHT",value.Kdbcht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBCTT",value.Kdbctt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBCCP",value.Kdbccp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 9);
                    parameters.Add("KDBVAL",value.Kdbval, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBVAA",value.Kdbvaa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBVAW",value.Kdbvaw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBVAT",value.Kdbvat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDBVAU",value.Kdbvau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDBVAH",value.Kdbvah??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDBIVO",value.Kdbivo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDBIVA",value.Kdbiva, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDBIVW",value.Kdbivw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDBAVE",value.Kdbave, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDBAVG",value.Kdbavg, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDBECO",value.Kdbeco??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDBAVA",value.Kdbava, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDBAVM",value.Kdbavm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDBAVJ",value.Kdbavj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDBEHH",value.Kdbehh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBEHC",value.Kdbehc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBEHI",value.Kdbehi, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBASVALO",value.Kdbasvalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBASVALA",value.Kdbasvala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBASVALW",value.Kdbasvalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDBASUNIT",value.Kdbasunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDBASBASE",value.Kdbasbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDBGER",value.Kdbger, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("id",value.Kdbid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpOpt> GetByAffaire(string typeAffaire, string codeAffaire, int version){
                    return connection.EnsureOpened().Query<KpOpt>(select_GetByAffaire, new {typeAffaire, codeAffaire, version}).ToList();
            }
            public IEnumerable<KpOpt> GetByFormule(Int64 formuleId){
                    return connection.EnsureOpened().Query<KpOpt>(select_GetByFormule, new {formuleId}).ToList();
            }
    }
}
