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

    public  partial class  HpentRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KAATYP, KAAIPB, KAAALX, KAAAVN, KAAHIN
, KAABONI, KAABONT, KAAANTI, KAADESI, KAAOBSV
, KAALCIVALO, KAALCIVALA, KAALCIVALW, KAALCIUNIT, KAALCIBASE
, KAAKDIID, KAAFRHVALO, KAAFRHVALA, KAAFRHVALW, KAAFRHUNIT
, KAAFRHBASE, KAAKDKID, KAAATGLCI, KAAATGKLC, KAAATGCAP
, KAAATGKCA, KAAATGSUR, KAAATGBCN, KAAATGKBC, KAAATGCRI
, KAAATGAPT, KAAATGF, KAAATGAPR, KAAATGTRT, KAAATGTRR
, KAAATGTCT, KAAATGTCR, KAAATGTFT, KAAATGTCM, KAAATGTFA
, KAAATGCTX, KAAATGLCF, KAAATGCAF, KAAATGSUF, KAAATGBCF
, KAALCIINA, KAAATGFRR, KAAATGCMT, KAAATGFAT, KAAATGBAS
, KAAATGKBA, KAAFRHINA, KAAAND, KAADPRJ, KAADSTA
, KAAOBSF, KAAOBSA, KAAOBSC, KAAASS, KAAAFS
, KAAXCMS, KAACNCS, KAACIBLE, KAAMAXA, KAAMAXE
, KAAIDE, KAAIMED, KAAIMDA, KAAISIN, KAAAVH
, KAAAVDS, KAARCP, KAAPAQ, KAASCAT, KAALTASP
, KAARELDT, KAAQUACH, KAAQUVEN, KAAAVAO, KAAARIPK
, KAAARTYG, KAAPKRD, KAASUDD, KAASUDH, KAASUFD
, KAASUFH, KAARSDD, KAARSDH FROM HPENT
WHERE KAATYP = :type
and KAAIPB = :numeroAffaire
and KAAALX = :numeroAliment
and KAAAVN = :numeroAvenant
";
            const string update=@"UPDATE HPENT SET 
KAATYP = :KAATYP, KAAIPB = :KAAIPB, KAAALX = :KAAALX, KAAAVN = :KAAAVN, KAAHIN = :KAAHIN, KAABONI = :KAABONI, KAABONT = :KAABONT, KAAANTI = :KAAANTI, KAADESI = :KAADESI, KAAOBSV = :KAAOBSV
, KAALCIVALO = :KAALCIVALO, KAALCIVALA = :KAALCIVALA, KAALCIVALW = :KAALCIVALW, KAALCIUNIT = :KAALCIUNIT, KAALCIBASE = :KAALCIBASE, KAAKDIID = :KAAKDIID, KAAFRHVALO = :KAAFRHVALO, KAAFRHVALA = :KAAFRHVALA, KAAFRHVALW = :KAAFRHVALW, KAAFRHUNIT = :KAAFRHUNIT
, KAAFRHBASE = :KAAFRHBASE, KAAKDKID = :KAAKDKID, KAAATGLCI = :KAAATGLCI, KAAATGKLC = :KAAATGKLC, KAAATGCAP = :KAAATGCAP, KAAATGKCA = :KAAATGKCA, KAAATGSUR = :KAAATGSUR, KAAATGBCN = :KAAATGBCN, KAAATGKBC = :KAAATGKBC, KAAATGCRI = :KAAATGCRI
, KAAATGAPT = :KAAATGAPT, KAAATGF = :KAAATGF, KAAATGAPR = :KAAATGAPR, KAAATGTRT = :KAAATGTRT, KAAATGTRR = :KAAATGTRR, KAAATGTCT = :KAAATGTCT, KAAATGTCR = :KAAATGTCR, KAAATGTFT = :KAAATGTFT, KAAATGTCM = :KAAATGTCM, KAAATGTFA = :KAAATGTFA
, KAAATGCTX = :KAAATGCTX, KAAATGLCF = :KAAATGLCF, KAAATGCAF = :KAAATGCAF, KAAATGSUF = :KAAATGSUF, KAAATGBCF = :KAAATGBCF, KAALCIINA = :KAALCIINA, KAAATGFRR = :KAAATGFRR, KAAATGCMT = :KAAATGCMT, KAAATGFAT = :KAAATGFAT, KAAATGBAS = :KAAATGBAS
, KAAATGKBA = :KAAATGKBA, KAAFRHINA = :KAAFRHINA, KAAAND = :KAAAND, KAADPRJ = :KAADPRJ, KAADSTA = :KAADSTA, KAAOBSF = :KAAOBSF, KAAOBSA = :KAAOBSA, KAAOBSC = :KAAOBSC, KAAASS = :KAAASS, KAAAFS = :KAAAFS
, KAAXCMS = :KAAXCMS, KAACNCS = :KAACNCS, KAACIBLE = :KAACIBLE, KAAMAXA = :KAAMAXA, KAAMAXE = :KAAMAXE, KAAIDE = :KAAIDE, KAAIMED = :KAAIMED, KAAIMDA = :KAAIMDA, KAAISIN = :KAAISIN, KAAAVH = :KAAAVH
, KAAAVDS = :KAAAVDS, KAARCP = :KAARCP, KAAPAQ = :KAAPAQ, KAASCAT = :KAASCAT, KAALTASP = :KAALTASP, KAARELDT = :KAARELDT, KAAQUACH = :KAAQUACH, KAAQUVEN = :KAAQUVEN, KAAAVAO = :KAAAVAO, KAAARIPK = :KAAARIPK
, KAAARTYG = :KAAARTYG, KAAPKRD = :KAAPKRD, KAASUDD = :KAASUDD, KAASUDH = :KAASUDH, KAASUFD = :KAASUFD, KAASUFH = :KAASUFH, KAARSDD = :KAARSDD, KAARSDH = :KAARSDH
 WHERE 
KAATYP = :type and KAAIPB = :numeroAffaire and KAAALX = :numeroAliment and KAAAVN = :numeroAvenant";
            const string delete=@"DELETE FROM HPENT WHERE KAATYP = :type AND KAAIPB = :numeroAffaire AND KAAALX = :numeroAliment AND KAAAVN = :numeroAvenant";
            const string insert=@"INSERT INTO  HPENT (
KAATYP, KAAIPB, KAAALX, KAAAVN, KAAHIN
, KAABONI, KAABONT, KAAANTI, KAADESI, KAAOBSV
, KAALCIVALO, KAALCIVALA, KAALCIVALW, KAALCIUNIT, KAALCIBASE
, KAAKDIID, KAAFRHVALO, KAAFRHVALA, KAAFRHVALW, KAAFRHUNIT
, KAAFRHBASE, KAAKDKID, KAAATGLCI, KAAATGKLC, KAAATGCAP
, KAAATGKCA, KAAATGSUR, KAAATGBCN, KAAATGKBC, KAAATGCRI
, KAAATGAPT, KAAATGF, KAAATGAPR, KAAATGTRT, KAAATGTRR
, KAAATGTCT, KAAATGTCR, KAAATGTFT, KAAATGTCM, KAAATGTFA
, KAAATGCTX, KAAATGLCF, KAAATGCAF, KAAATGSUF, KAAATGBCF
, KAALCIINA, KAAATGFRR, KAAATGCMT, KAAATGFAT, KAAATGBAS
, KAAATGKBA, KAAFRHINA, KAAAND, KAADPRJ, KAADSTA
, KAAOBSF, KAAOBSA, KAAOBSC, KAAASS, KAAAFS
, KAAXCMS, KAACNCS, KAACIBLE, KAAMAXA, KAAMAXE
, KAAIDE, KAAIMED, KAAIMDA, KAAISIN, KAAAVH
, KAAAVDS, KAARCP, KAAPAQ, KAASCAT, KAALTASP
, KAARELDT, KAAQUACH, KAAQUVEN, KAAAVAO, KAAARIPK
, KAAARTYG, KAAPKRD, KAASUDD, KAASUDH, KAASUFD
, KAASUFH, KAARSDD, KAARSDH
) VALUES (
:KAATYP, :KAAIPB, :KAAALX, :KAAAVN, :KAAHIN
, :KAABONI, :KAABONT, :KAAANTI, :KAADESI, :KAAOBSV
, :KAALCIVALO, :KAALCIVALA, :KAALCIVALW, :KAALCIUNIT, :KAALCIBASE
, :KAAKDIID, :KAAFRHVALO, :KAAFRHVALA, :KAAFRHVALW, :KAAFRHUNIT
, :KAAFRHBASE, :KAAKDKID, :KAAATGLCI, :KAAATGKLC, :KAAATGCAP
, :KAAATGKCA, :KAAATGSUR, :KAAATGBCN, :KAAATGKBC, :KAAATGCRI
, :KAAATGAPT, :KAAATGF, :KAAATGAPR, :KAAATGTRT, :KAAATGTRR
, :KAAATGTCT, :KAAATGTCR, :KAAATGTFT, :KAAATGTCM, :KAAATGTFA
, :KAAATGCTX, :KAAATGLCF, :KAAATGCAF, :KAAATGSUF, :KAAATGBCF
, :KAALCIINA, :KAAATGFRR, :KAAATGCMT, :KAAATGFAT, :KAAATGBAS
, :KAAATGKBA, :KAAFRHINA, :KAAAND, :KAADPRJ, :KAADSTA
, :KAAOBSF, :KAAOBSA, :KAAOBSC, :KAAASS, :KAAAFS
, :KAAXCMS, :KAACNCS, :KAACIBLE, :KAAMAXA, :KAAMAXE
, :KAAIDE, :KAAIMED, :KAAIMDA, :KAAISIN, :KAAAVH
, :KAAAVDS, :KAARCP, :KAAPAQ, :KAASCAT, :KAALTASP
, :KAARELDT, :KAAQUACH, :KAAQUVEN, :KAAAVAO, :KAAARIPK
, :KAAARTYG, :KAAPKRD, :KAASUDD, :KAASUDH, :KAASUFD
, :KAASUFH, :KAARSDD, :KAARSDH)";
            #endregion

            public HpentRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpEnt Get(string type, string numeroAffaire, int numeroAliment, int numeroAvenant){
                return connection.Query<KpEnt>(select, new {type, numeroAffaire, numeroAliment, numeroAvenant}).SingleOrDefault();
            }


            public void Insert(KpEnt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAATYP",value.Kaatyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAIPB",value.Kaaipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KAAALX",value.Kaaalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAAAVN",value.Kaaavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAAHIN",value.Kaahin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAABONI",value.Kaaboni??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAABONT",value.Kaabont, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:6, scale: 3);
                    parameters.Add("KAAANTI",value.Kaaanti??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAADESI",value.Kaadesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAAOBSV",value.Kaaobsv, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAALCIVALO",value.Kaalcivalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAALCIVALA",value.Kaalcivala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAALCIVALW",value.Kaalcivalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAALCIUNIT",value.Kaalciunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAALCIBASE",value.Kaalcibase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAAKDIID",value.Kaakdiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAAFRHVALO",value.Kaafrhvalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAFRHVALA",value.Kaafrhvala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAFRHVALW",value.Kaafrhvalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAFRHUNIT",value.Kaafrhunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAAFRHBASE",value.Kaafrhbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAAKDKID",value.Kaakdkid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAAATGLCI",value.Kaaatglci, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAATGKLC",value.Kaaatgklc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAATGCAP",value.Kaaatgcap, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAATGKCA",value.Kaaatgkca, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAATGSUR",value.Kaaatgsur, dbType:DbType.Int32, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KAAATGBCN",value.Kaaatgbcn, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAATGKBC",value.Kaaatgkbc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAATGCRI",value.Kaaatgcri??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KAAATGAPT",value.Kaaatgapt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAATGF",value.Kaaatgf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAATGAPR",value.Kaaatgapr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAATGTRT",value.Kaaatgtrt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAATGTRR",value.Kaaatgtrr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAATGTCT",value.Kaaatgtct, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KAAATGTCR",value.Kaaatgtcr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KAAATGTFT",value.Kaaatgtft, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KAAATGTCM",value.Kaaatgtcm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KAAATGTFA",value.Kaaatgtfa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KAAATGCTX",value.Kaaatgctx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KAAATGLCF",value.Kaaatglcf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAATGCAF",value.Kaaatgcaf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAATGSUF",value.Kaaatgsuf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KAAATGBCF",value.Kaaatgbcf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAALCIINA",value.Kaalciina??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAATGFRR",value.Kaaatgfrr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KAAATGCMT",value.Kaaatgcmt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KAAATGFAT",value.Kaaatgfat, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KAAATGBAS",value.Kaaatgbas, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAATGKBA",value.Kaaatgkba, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAFRHINA",value.Kaafrhina??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAAND",value.Kaaand, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAADPRJ",value.Kaadprj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAADSTA",value.Kaadsta, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAAOBSF",value.Kaaobsf, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAAOBSA",value.Kaaobsa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAAOBSC",value.Kaaobsc, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAAASS",value.Kaaass??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAAFS",value.Kaaafs, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KAAXCMS",value.Kaaxcms??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAACNCS",value.Kaacncs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAACIBLE",value.Kaacible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAAMAXA",value.Kaamaxa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAMAXE",value.Kaamaxe, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAIDE",value.Kaaide, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAAIMED",value.Kaaimed, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KAAIMDA",value.Kaaimda, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KAAISIN",value.Kaaisin??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KAAAVH",value.Kaaavh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAAAVDS",value.Kaaavds, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAARCP",value.Kaarcp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAPAQ",value.Kaapaq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAASCAT",value.Kaascat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAALTASP",value.Kaaltasp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 4);
                    parameters.Add("KAARELDT",value.Kaareldt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAAQUACH",value.Kaaquach??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAAQUVEN",value.Kaaquven??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAAAVAO",value.Kaaavao, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAAARIPK",value.Kaaaripk, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAAARTYG",value.Kaaartyg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAPKRD",value.Kaapkrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAASUDD",value.Kaasudd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAASUDH",value.Kaasudh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAASUFD",value.Kaasufd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAASUFH",value.Kaasufh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAARSDD",value.Kaarsdd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAARSDH",value.Kaarsdh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpEnt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("type",value.Kaatyp);
                    parameters.Add("numeroAffaire",value.Kaaipb);
                    parameters.Add("numeroAliment",value.Kaaalx);
                    parameters.Add("numeroAvenant",value.Kaaavn);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpEnt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAATYP",value.Kaatyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAIPB",value.Kaaipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KAAALX",value.Kaaalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAAAVN",value.Kaaavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAAHIN",value.Kaahin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAABONI",value.Kaaboni??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAABONT",value.Kaabont, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:6, scale: 3);
                    parameters.Add("KAAANTI",value.Kaaanti??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAADESI",value.Kaadesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAAOBSV",value.Kaaobsv, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAALCIVALO",value.Kaalcivalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAALCIVALA",value.Kaalcivala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAALCIVALW",value.Kaalcivalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAALCIUNIT",value.Kaalciunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAALCIBASE",value.Kaalcibase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAAKDIID",value.Kaakdiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAAFRHVALO",value.Kaafrhvalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAFRHVALA",value.Kaafrhvala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAFRHVALW",value.Kaafrhvalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAFRHUNIT",value.Kaafrhunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAAFRHBASE",value.Kaafrhbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAAKDKID",value.Kaakdkid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAAATGLCI",value.Kaaatglci, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAATGKLC",value.Kaaatgklc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAATGCAP",value.Kaaatgcap, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAATGKCA",value.Kaaatgkca, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAATGSUR",value.Kaaatgsur, dbType:DbType.Int32, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KAAATGBCN",value.Kaaatgbcn, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAATGKBC",value.Kaaatgkbc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAATGCRI",value.Kaaatgcri??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KAAATGAPT",value.Kaaatgapt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAATGF",value.Kaaatgf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAATGAPR",value.Kaaatgapr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAATGTRT",value.Kaaatgtrt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAATGTRR",value.Kaaatgtrr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAATGTCT",value.Kaaatgtct, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KAAATGTCR",value.Kaaatgtcr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KAAATGTFT",value.Kaaatgtft, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KAAATGTCM",value.Kaaatgtcm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KAAATGTFA",value.Kaaatgtfa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KAAATGCTX",value.Kaaatgctx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KAAATGLCF",value.Kaaatglcf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAATGCAF",value.Kaaatgcaf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAATGSUF",value.Kaaatgsuf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KAAATGBCF",value.Kaaatgbcf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAALCIINA",value.Kaalciina??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAATGFRR",value.Kaaatgfrr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KAAATGCMT",value.Kaaatgcmt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KAAATGFAT",value.Kaaatgfat, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KAAATGBAS",value.Kaaatgbas, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAATGKBA",value.Kaaatgkba, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAFRHINA",value.Kaafrhina??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAAND",value.Kaaand, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAADPRJ",value.Kaadprj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAADSTA",value.Kaadsta, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAAOBSF",value.Kaaobsf, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAAOBSA",value.Kaaobsa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAAOBSC",value.Kaaobsc, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAAASS",value.Kaaass??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAAFS",value.Kaaafs, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KAAXCMS",value.Kaaxcms??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAACNCS",value.Kaacncs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAACIBLE",value.Kaacible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAAMAXA",value.Kaamaxa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAMAXE",value.Kaamaxe, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KAAIDE",value.Kaaide, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAAIMED",value.Kaaimed, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KAAIMDA",value.Kaaimda, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KAAISIN",value.Kaaisin??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KAAAVH",value.Kaaavh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAAAVDS",value.Kaaavds, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAARCP",value.Kaarcp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAPAQ",value.Kaapaq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAASCAT",value.Kaascat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAALTASP",value.Kaaltasp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 4);
                    parameters.Add("KAARELDT",value.Kaareldt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAAQUACH",value.Kaaquach??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAAQUVEN",value.Kaaquven??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAAAVAO",value.Kaaavao, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAAARIPK",value.Kaaaripk, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAAARTYG",value.Kaaartyg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAAPKRD",value.Kaapkrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAASUDD",value.Kaasudd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAASUDH",value.Kaasudh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAASUFD",value.Kaasufd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAASUFH",value.Kaasufh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAARSDD",value.Kaarsdd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAARSDH",value.Kaarsdh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("type",value.Kaatyp);
                    parameters.Add("numeroAffaire",value.Kaaipb);
                    parameters.Add("numeroAliment",value.Kaaalx);
                    parameters.Add("numeroAvenant",value.Kaaavn);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
    }
}
