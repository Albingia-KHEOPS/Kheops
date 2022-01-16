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

    public  partial class  YSinistRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
SISUA, SINUM, SISBR, SISUM, SISUJ
, SIOUA, SIOUM, SIOUJ, SIIPB, SIALX
, SIAVN, SITCH, SITBR, SITSB, SIGES
, SIRCA, SIRCM, SIRCJ, SINTR, SICAU
, SIJJA, SIJJM, SIJJJ, SIREF, SIAD1
, SIAD2, SIDEP, SICPO, SIVIL, SIPAY
, SIEXP, SIINE, SIREX, SIEXM, SIERP
, SIEJP, SIERD, SIEJD, SINJU, SIAVO
, SIINA, SIRAV, SIICT, SIINC, SIRCO
, SIDCT, SICCT, SIAPR, SIRAP, SIAPP
, SIPCV, SINPL, SINGE, SIACC, SIRFU
, SICRU, SICRA, SICRM, SICRJ, SIMJU
, SIMJA, SIMJM, SIMJJ, SISIT, SISTA
, SISTM, SISTJ, SIETA, SIDRE, SIDRN
, SICVG, SIRPA, SIRPM, SIRPJ, SIRDA
, SIRDM, SIRDJ, SIHIA, SIHIM, SIHIJ
, SIHIH, SIHIU, SIRCD, SIIHB, SIROA
, SIROM, SIROJ, SIROU, SIDRA, SIDRM
, SIDRJ, SIDBA, SIDBM, SIDBJ, SICVR
, SIS36, SIMNG, SICLO, SIEXN, SIRCT
, SIMTB, SIMTS, SIEMI, SIOUP, SICNA
, SISPP, SISPS, SIMFP, SIPF, SICBC
, SIBCR, SIBOU, SIEVN, SIGAR, SINNC
, SIEJR, SIEJS, SIRUA, SIRUM, SIRUJ
, SI15A, SI15M, SI15J, SIADH, SILCO
, SIFIL, SICOK, SINLC, SIE1A, SIE1M
, SIE1J, SIIN5 FROM YSINIST
WHERE SISUA = :SISUA
and SINUM = :SINUM
and SISBR = :SISBR
";
            const string update=@"UPDATE YSINIST SET 
SISUA = :SISUA, SINUM = :SINUM, SISBR = :SISBR, SISUM = :SISUM, SISUJ = :SISUJ, SIOUA = :SIOUA, SIOUM = :SIOUM, SIOUJ = :SIOUJ, SIIPB = :SIIPB, SIALX = :SIALX
, SIAVN = :SIAVN, SITCH = :SITCH, SITBR = :SITBR, SITSB = :SITSB, SIGES = :SIGES, SIRCA = :SIRCA, SIRCM = :SIRCM, SIRCJ = :SIRCJ, SINTR = :SINTR, SICAU = :SICAU
, SIJJA = :SIJJA, SIJJM = :SIJJM, SIJJJ = :SIJJJ, SIREF = :SIREF, SIAD1 = :SIAD1, SIAD2 = :SIAD2, SIDEP = :SIDEP, SICPO = :SICPO, SIVIL = :SIVIL, SIPAY = :SIPAY
, SIEXP = :SIEXP, SIINE = :SIINE, SIREX = :SIREX, SIEXM = :SIEXM, SIERP = :SIERP, SIEJP = :SIEJP, SIERD = :SIERD, SIEJD = :SIEJD, SINJU = :SINJU, SIAVO = :SIAVO
, SIINA = :SIINA, SIRAV = :SIRAV, SIICT = :SIICT, SIINC = :SIINC, SIRCO = :SIRCO, SIDCT = :SIDCT, SICCT = :SICCT, SIAPR = :SIAPR, SIRAP = :SIRAP, SIAPP = :SIAPP
, SIPCV = :SIPCV, SINPL = :SINPL, SINGE = :SINGE, SIACC = :SIACC, SIRFU = :SIRFU, SICRU = :SICRU, SICRA = :SICRA, SICRM = :SICRM, SICRJ = :SICRJ, SIMJU = :SIMJU
, SIMJA = :SIMJA, SIMJM = :SIMJM, SIMJJ = :SIMJJ, SISIT = :SISIT, SISTA = :SISTA, SISTM = :SISTM, SISTJ = :SISTJ, SIETA = :SIETA, SIDRE = :SIDRE, SIDRN = :SIDRN
, SICVG = :SICVG, SIRPA = :SIRPA, SIRPM = :SIRPM, SIRPJ = :SIRPJ, SIRDA = :SIRDA, SIRDM = :SIRDM, SIRDJ = :SIRDJ, SIHIA = :SIHIA, SIHIM = :SIHIM, SIHIJ = :SIHIJ
, SIHIH = :SIHIH, SIHIU = :SIHIU, SIRCD = :SIRCD, SIIHB = :SIIHB, SIROA = :SIROA, SIROM = :SIROM, SIROJ = :SIROJ, SIROU = :SIROU, SIDRA = :SIDRA, SIDRM = :SIDRM
, SIDRJ = :SIDRJ, SIDBA = :SIDBA, SIDBM = :SIDBM, SIDBJ = :SIDBJ, SICVR = :SICVR, SIS36 = :SIS36, SIMNG = :SIMNG, SICLO = :SICLO, SIEXN = :SIEXN, SIRCT = :SIRCT
, SIMTB = :SIMTB, SIMTS = :SIMTS, SIEMI = :SIEMI, SIOUP = :SIOUP, SICNA = :SICNA, SISPP = :SISPP, SISPS = :SISPS, SIMFP = :SIMFP, SIPF = :SIPF, SICBC = :SICBC
, SIBCR = :SIBCR, SIBOU = :SIBOU, SIEVN = :SIEVN, SIGAR = :SIGAR, SINNC = :SINNC, SIEJR = :SIEJR, SIEJS = :SIEJS, SIRUA = :SIRUA, SIRUM = :SIRUM, SIRUJ = :SIRUJ
, SI15A = :SI15A, SI15M = :SI15M, SI15J = :SI15J, SIADH = :SIADH, SILCO = :SILCO, SIFIL = :SIFIL, SICOK = :SICOK, SINLC = :SINLC, SIE1A = :SIE1A, SIE1M = :SIE1M
, SIE1J = :SIE1J, SIIN5 = :SIIN5
 WHERE 
SISUA = :SISUA and SINUM = :SINUM and SISBR = :SISBR";
            const string delete=@"DELETE FROM YSINIST WHERE SISUA = :SISUA AND SINUM = :SINUM AND SISBR = :SISBR";
            const string insert=@"INSERT INTO  YSINIST (
SISUA, SINUM, SISBR, SISUM, SISUJ
, SIOUA, SIOUM, SIOUJ, SIIPB, SIALX
, SIAVN, SITCH, SITBR, SITSB, SIGES
, SIRCA, SIRCM, SIRCJ, SINTR, SICAU
, SIJJA, SIJJM, SIJJJ, SIREF, SIAD1
, SIAD2, SIDEP, SICPO, SIVIL, SIPAY
, SIEXP, SIINE, SIREX, SIEXM, SIERP
, SIEJP, SIERD, SIEJD, SINJU, SIAVO
, SIINA, SIRAV, SIICT, SIINC, SIRCO
, SIDCT, SICCT, SIAPR, SIRAP, SIAPP
, SIPCV, SINPL, SINGE, SIACC, SIRFU
, SICRU, SICRA, SICRM, SICRJ, SIMJU
, SIMJA, SIMJM, SIMJJ, SISIT, SISTA
, SISTM, SISTJ, SIETA, SIDRE, SIDRN
, SICVG, SIRPA, SIRPM, SIRPJ, SIRDA
, SIRDM, SIRDJ, SIHIA, SIHIM, SIHIJ
, SIHIH, SIHIU, SIRCD, SIIHB, SIROA
, SIROM, SIROJ, SIROU, SIDRA, SIDRM
, SIDRJ, SIDBA, SIDBM, SIDBJ, SICVR
, SIS36, SIMNG, SICLO, SIEXN, SIRCT
, SIMTB, SIMTS, SIEMI, SIOUP, SICNA
, SISPP, SISPS, SIMFP, SIPF, SICBC
, SIBCR, SIBOU, SIEVN, SIGAR, SINNC
, SIEJR, SIEJS, SIRUA, SIRUM, SIRUJ
, SI15A, SI15M, SI15J, SIADH, SILCO
, SIFIL, SICOK, SINLC, SIE1A, SIE1M
, SIE1J, SIIN5
) VALUES (
:SISUA, :SINUM, :SISBR, :SISUM, :SISUJ
, :SIOUA, :SIOUM, :SIOUJ, :SIIPB, :SIALX
, :SIAVN, :SITCH, :SITBR, :SITSB, :SIGES
, :SIRCA, :SIRCM, :SIRCJ, :SINTR, :SICAU
, :SIJJA, :SIJJM, :SIJJJ, :SIREF, :SIAD1
, :SIAD2, :SIDEP, :SICPO, :SIVIL, :SIPAY
, :SIEXP, :SIINE, :SIREX, :SIEXM, :SIERP
, :SIEJP, :SIERD, :SIEJD, :SINJU, :SIAVO
, :SIINA, :SIRAV, :SIICT, :SIINC, :SIRCO
, :SIDCT, :SICCT, :SIAPR, :SIRAP, :SIAPP
, :SIPCV, :SINPL, :SINGE, :SIACC, :SIRFU
, :SICRU, :SICRA, :SICRM, :SICRJ, :SIMJU
, :SIMJA, :SIMJM, :SIMJJ, :SISIT, :SISTA
, :SISTM, :SISTJ, :SIETA, :SIDRE, :SIDRN
, :SICVG, :SIRPA, :SIRPM, :SIRPJ, :SIRDA
, :SIRDM, :SIRDJ, :SIHIA, :SIHIM, :SIHIJ
, :SIHIH, :SIHIU, :SIRCD, :SIIHB, :SIROA
, :SIROM, :SIROJ, :SIROU, :SIDRA, :SIDRM
, :SIDRJ, :SIDBA, :SIDBM, :SIDBJ, :SICVR
, :SIS36, :SIMNG, :SICLO, :SIEXN, :SIRCT
, :SIMTB, :SIMTS, :SIEMI, :SIOUP, :SICNA
, :SISPP, :SISPS, :SIMFP, :SIPF, :SICBC
, :SIBCR, :SIBOU, :SIEVN, :SIGAR, :SINNC
, :SIEJR, :SIEJS, :SIRUA, :SIRUM, :SIRUJ
, :SI15A, :SI15M, :SI15J, :SIADH, :SILCO
, :SIFIL, :SICOK, :SINLC, :SIE1A, :SIE1M
, :SIE1J, :SIIN5)";
            const string select_GetByAffaire=@"SELECT
SISUA, SINUM, SISBR, SISUM, SISUJ
, SIOUA, SIOUM, SIOUJ, SIIPB, SIALX
, SIAVN, SITCH, SITBR, SITSB, SIGES
, SIRCA, SIRCM, SIRCJ, SINTR, SICAU
, SIJJA, SIJJM, SIJJJ, SIREF, SIAD1
, SIAD2, SIDEP, SICPO, SIVIL, SIPAY
, SIEXP, SIINE, SIREX, SIEXM, SIERP
, SIEJP, SIERD, SIEJD, SINJU, SIAVO
, SIINA, SIRAV, SIICT, SIINC, SIRCO
, SIDCT, SICCT, SIAPR, SIRAP, SIAPP
, SIPCV, SINPL, SINGE, SIACC, SIRFU
, SICRU, SICRA, SICRM, SICRJ, SIMJU
, SIMJA, SIMJM, SIMJJ, SISIT, SISTA
, SISTM, SISTJ, SIETA, SIDRE, SIDRN
, SICVG, SIRPA, SIRPM, SIRPJ, SIRDA
, SIRDM, SIRDJ, SIHIA, SIHIM, SIHIJ
, SIHIH, SIHIU, SIRCD, SIIHB, SIROA
, SIROM, SIROJ, SIROU, SIDRA, SIDRM
, SIDRJ, SIDBA, SIDBM, SIDBJ, SICVR
, SIS36, SIMNG, SICLO, SIEXN, SIRCT
, SIMTB, SIMTS, SIEMI, SIOUP, SICNA
, SISPP, SISPS, SIMFP, SIPF, SICBC
, SIBCR, SIBOU, SIEVN, SIGAR, SINNC
, SIEJR, SIEJS, SIRUA, SIRUM, SIRUJ
, SI15A, SI15M, SI15J, SIADH, SILCO
, SIFIL, SICOK, SINLC, SIE1A, SIE1M
, SIE1J, SIIN5 FROM YSINIST
WHERE SIIPB = :SIIPB
and SIALX = :SIALX
and SIAVN = :SIAVN
";
            #endregion

            public YSinistRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YSinist Get(int SISUA, int SINUM, string SISBR){
                return connection.Query<YSinist>(select, new {SISUA, SINUM, SISBR}).SingleOrDefault();
            }


            public void Insert(YSinist value){
                    var parameters = new DynamicParameters();
                    parameters.Add("SISUA",value.Sisua, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SINUM",value.Sinum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("SISBR",value.Sisbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SISUM",value.Sisum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SISUJ",value.Sisuj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIOUA",value.Sioua, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIOUM",value.Sioum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIOUJ",value.Siouj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIIPB",value.Siipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("SIALX",value.Sialx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIAVN",value.Siavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SITCH",value.Sitch, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SITBR",value.Sitbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SITSB",value.Sitsb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SIGES",value.Siges??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("SIRCA",value.Sirca, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIRCM",value.Sircm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIRCJ",value.Sircj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SINTR",value.Sintr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("SICAU",value.Sicau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("SIJJA",value.Sijja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIJJM",value.Sijjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIJJJ",value.Sijjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIREF",value.Siref??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("SIAD1",value.Siad1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("SIAD2",value.Siad2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("SIDEP",value.Sidep??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SICPO",value.Sicpo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SIVIL",value.Sivil??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:26, scale: 0);
                    parameters.Add("SIPAY",value.Sipay??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SIEXP",value.Siexp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("SIINE",value.Siine, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIREX",value.Sirex??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:25, scale: 0);
                    parameters.Add("SIEXM",value.Siexm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIERP",value.Sierp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIEJP",value.Siejp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SIERD",value.Sierd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIEJD",value.Siejd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SINJU",value.Sinju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIAVO",value.Siavo, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("SIINA",value.Siina, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIRAV",value.Sirav??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:25, scale: 0);
                    parameters.Add("SIICT",value.Siict, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("SIINC",value.Siinc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIRCO",value.Sirco??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:25, scale: 0);
                    parameters.Add("SIDCT",value.Sidct??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SICCT",value.Sicct??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIAPR",value.Siapr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("SIRAP",value.Sirap??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:25, scale: 0);
                    parameters.Add("SIAPP",value.Siapp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("SIPCV",value.Sipcv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SINPL",value.Sinpl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SINGE",value.Singe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIACC",value.Siacc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIRFU",value.Sirfu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SICRU",value.Sicru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("SICRA",value.Sicra, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SICRM",value.Sicrm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SICRJ",value.Sicrj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIMJU",value.Simju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("SIMJA",value.Simja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIMJM",value.Simjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIMJJ",value.Simjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SISIT",value.Sisit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SISTA",value.Sista, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SISTM",value.Sistm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SISTJ",value.Sistj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIETA",value.Sieta??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIDRE",value.Sidre??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIDRN",value.Sidrn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SICVG",value.Sicvg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIRPA",value.Sirpa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIRPM",value.Sirpm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIRPJ",value.Sirpj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIRDA",value.Sirda, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIRDM",value.Sirdm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIRDJ",value.Sirdj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIHIA",value.Sihia, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIHIM",value.Sihim, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIHIJ",value.Sihij, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIHIH",value.Sihih, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("SIHIU",value.Sihiu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("SIRCD",value.Sircd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIIHB",value.Siihb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIROA",value.Siroa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIROM",value.Sirom, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIROJ",value.Siroj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIROU",value.Sirou??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("SIDRA",value.Sidra, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIDRM",value.Sidrm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIDRJ",value.Sidrj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIDBA",value.Sidba, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIDBM",value.Sidbm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIDBJ",value.Sidbj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SICVR",value.Sicvr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIS36",value.Sis36??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIMNG",value.Simng, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SICLO",value.Siclo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIEXN",value.Siexn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("SIRCT",value.Sirct??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIMTB",value.Simtb, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("SIMTS",value.Simts, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("SIEMI",value.Siemi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIOUP",value.Sioup??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SICNA",value.Sicna??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SISPP",value.Sispp, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("SISPS",value.Sisps, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("SIMFP",value.Simfp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SIPF",value.Sipf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SICBC",value.Sicbc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("SIBCR",value.Sibcr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIBOU",value.Sibou??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIEVN",value.Sievn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("SIGAR",value.Sigar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("SINNC",value.Sinnc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("SIEJR",value.Siejr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SIEJS",value.Siejs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIRUA",value.Sirua, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIRUM",value.Sirum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIRUJ",value.Siruj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SI15A",value.Si15a, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SI15M",value.Si15m, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SI15J",value.Si15j, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIADH",value.Siadh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("SILCO",value.Silco??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("SIFIL",value.Sifil??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SICOK",value.Sicok??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SINLC",value.Sinlc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("SIE1A",value.Sie1a, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIE1M",value.Sie1m, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIE1J",value.Sie1j, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIIN5",value.Siin5, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YSinist value){
                    var parameters = new DynamicParameters();
                    parameters.Add("SISUA",value.Sisua);
                    parameters.Add("SINUM",value.Sinum);
                    parameters.Add("SISBR",value.Sisbr);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YSinist value){
                    var parameters = new DynamicParameters();
                    parameters.Add("SISUA",value.Sisua, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SINUM",value.Sinum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("SISBR",value.Sisbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SISUM",value.Sisum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SISUJ",value.Sisuj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIOUA",value.Sioua, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIOUM",value.Sioum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIOUJ",value.Siouj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIIPB",value.Siipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("SIALX",value.Sialx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIAVN",value.Siavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SITCH",value.Sitch, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SITBR",value.Sitbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SITSB",value.Sitsb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SIGES",value.Siges??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("SIRCA",value.Sirca, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIRCM",value.Sircm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIRCJ",value.Sircj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SINTR",value.Sintr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("SICAU",value.Sicau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("SIJJA",value.Sijja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIJJM",value.Sijjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIJJJ",value.Sijjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIREF",value.Siref??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("SIAD1",value.Siad1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("SIAD2",value.Siad2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("SIDEP",value.Sidep??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SICPO",value.Sicpo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SIVIL",value.Sivil??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:26, scale: 0);
                    parameters.Add("SIPAY",value.Sipay??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SIEXP",value.Siexp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("SIINE",value.Siine, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIREX",value.Sirex??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:25, scale: 0);
                    parameters.Add("SIEXM",value.Siexm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIERP",value.Sierp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIEJP",value.Siejp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SIERD",value.Sierd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIEJD",value.Siejd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SINJU",value.Sinju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIAVO",value.Siavo, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("SIINA",value.Siina, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIRAV",value.Sirav??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:25, scale: 0);
                    parameters.Add("SIICT",value.Siict, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("SIINC",value.Siinc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIRCO",value.Sirco??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:25, scale: 0);
                    parameters.Add("SIDCT",value.Sidct??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SICCT",value.Sicct??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIAPR",value.Siapr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("SIRAP",value.Sirap??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:25, scale: 0);
                    parameters.Add("SIAPP",value.Siapp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("SIPCV",value.Sipcv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SINPL",value.Sinpl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SINGE",value.Singe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIACC",value.Siacc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIRFU",value.Sirfu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SICRU",value.Sicru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("SICRA",value.Sicra, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SICRM",value.Sicrm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SICRJ",value.Sicrj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIMJU",value.Simju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("SIMJA",value.Simja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIMJM",value.Simjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIMJJ",value.Simjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SISIT",value.Sisit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SISTA",value.Sista, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SISTM",value.Sistm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SISTJ",value.Sistj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIETA",value.Sieta??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIDRE",value.Sidre??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIDRN",value.Sidrn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SICVG",value.Sicvg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIRPA",value.Sirpa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIRPM",value.Sirpm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIRPJ",value.Sirpj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIRDA",value.Sirda, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIRDM",value.Sirdm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIRDJ",value.Sirdj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIHIA",value.Sihia, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIHIM",value.Sihim, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIHIJ",value.Sihij, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIHIH",value.Sihih, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("SIHIU",value.Sihiu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("SIRCD",value.Sircd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIIHB",value.Siihb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIROA",value.Siroa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIROM",value.Sirom, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIROJ",value.Siroj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIROU",value.Sirou??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("SIDRA",value.Sidra, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIDRM",value.Sidrm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIDRJ",value.Sidrj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIDBA",value.Sidba, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIDBM",value.Sidbm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIDBJ",value.Sidbj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SICVR",value.Sicvr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIS36",value.Sis36??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIMNG",value.Simng, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SICLO",value.Siclo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIEXN",value.Siexn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("SIRCT",value.Sirct??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIMTB",value.Simtb, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("SIMTS",value.Simts, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("SIEMI",value.Siemi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIOUP",value.Sioup??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SICNA",value.Sicna??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SISPP",value.Sispp, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("SISPS",value.Sisps, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("SIMFP",value.Simfp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SIPF",value.Sipf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SICBC",value.Sicbc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("SIBCR",value.Sibcr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIBOU",value.Sibou??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIEVN",value.Sievn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("SIGAR",value.Sigar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("SINNC",value.Sinnc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("SIEJR",value.Siejr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SIEJS",value.Siejs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SIRUA",value.Sirua, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIRUM",value.Sirum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIRUJ",value.Siruj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SI15A",value.Si15a, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SI15M",value.Si15m, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SI15J",value.Si15j, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIADH",value.Siadh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("SILCO",value.Silco??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("SIFIL",value.Sifil??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SICOK",value.Sicok??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("SINLC",value.Sinlc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("SIE1A",value.Sie1a, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SIE1M",value.Sie1m, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIE1J",value.Sie1j, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SIIN5",value.Siin5, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("SISUA",value.Sisua);
                    parameters.Add("SINUM",value.Sinum);
                    parameters.Add("SISBR",value.Sisbr);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YSinist> GetByAffaire(string SIIPB, int SIALX, int SIAVN){
                    return connection.EnsureOpened().Query<YSinist>(select_GetByAffaire, new {SIIPB, SIALX, SIAVN}).ToList();
            }
    }
}
