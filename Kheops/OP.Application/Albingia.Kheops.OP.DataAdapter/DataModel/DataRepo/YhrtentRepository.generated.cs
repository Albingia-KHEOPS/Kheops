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

    public  partial class  YhrtentRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
JDIPB, JDALX, JDAVN, JDHIN, JDSHT
, JDENC, JDITC, JDVAL, JDVAA, JDVAW
, JDVAT, JDVAU, JDVAH, JDDRQ, JDNBR
, JDTXL, JDTRR, JDXCM, JDNEX, JDNPA
, JDAFC, JDAFR, JDATT, JDCNA, JDCNC
, JDINA, JDIND, JDIXC, JDIXF, JDIXL
, JDIXP, JDIVO, JDIVA, JDIVW, JDMHT
, JDREA, JDREB, JDREH, JDDPV, JDGAU
, JDGVL, JDGUN, JDPBN, JDPBS, JDPBR
, JDPBT, JDPBC, JDPBP, JDPBA, JDRCG
, JDCCG, JDRCS, JDCCS, JDCLV, JDAGM
, JDLCV, JDLCA, JDLCW, JDLCU, JDLCE
, JDDPA, JDDPM, JDDPJ, JDFPA, JDFPM
, JDFPJ, JDPEA, JDPEM, JDPEJ, JDACQ
, JDTMC, JDTFO, JDTFT, JDTFF, JDTFP
, JDPRO, JDTMI, JDTFM, JDTMA, JDTMG
, JDCMC, JDCFO, JDCFT, JDCFH, JDCHT
, JDCTT, JDCCP, JDEHH, JDEHC, JDSMP
, JDIVX, JDTCR, JDNPG, JDEDI, JDEDN
, JDEDA, JDEDM, JDEDJ, JDEHI, JDIAX
, JDTED, JDDOO, JDRUA, JDRUM, JDRUJ
, JDECG, JDECP, JDAPT, JDAPR, JDAAT
, JDAAR, JDACR, JDACV, JDAXT, JDAXC
, JDAXF, JDAXM, JDAXG, JDATA, JDATX
, JDAUT, JDAUF, JDLTA, JDLTASP, JDLDEB
, JDLDEH, JDLFIN, JDLFIH, JDLDUR, JDLDUU
 FROM YHRTENT
WHERE JDIPB = :numeroAffaire
and JDALX = :numeroAliment
and JDAVN = :numeroAvenant
";
            const string update=@"UPDATE YHRTENT SET 
JDIPB = :JDIPB, JDALX = :JDALX, JDAVN = :JDAVN, JDHIN = :JDHIN, JDSHT = :JDSHT, JDENC = :JDENC, JDITC = :JDITC, JDVAL = :JDVAL, JDVAA = :JDVAA, JDVAW = :JDVAW
, JDVAT = :JDVAT, JDVAU = :JDVAU, JDVAH = :JDVAH, JDDRQ = :JDDRQ, JDNBR = :JDNBR, JDTXL = :JDTXL, JDTRR = :JDTRR, JDXCM = :JDXCM, JDNEX = :JDNEX, JDNPA = :JDNPA
, JDAFC = :JDAFC, JDAFR = :JDAFR, JDATT = :JDATT, JDCNA = :JDCNA, JDCNC = :JDCNC, JDINA = :JDINA, JDIND = :JDIND, JDIXC = :JDIXC, JDIXF = :JDIXF, JDIXL = :JDIXL
, JDIXP = :JDIXP, JDIVO = :JDIVO, JDIVA = :JDIVA, JDIVW = :JDIVW, JDMHT = :JDMHT, JDREA = :JDREA, JDREB = :JDREB, JDREH = :JDREH, JDDPV = :JDDPV, JDGAU = :JDGAU
, JDGVL = :JDGVL, JDGUN = :JDGUN, JDPBN = :JDPBN, JDPBS = :JDPBS, JDPBR = :JDPBR, JDPBT = :JDPBT, JDPBC = :JDPBC, JDPBP = :JDPBP, JDPBA = :JDPBA, JDRCG = :JDRCG
, JDCCG = :JDCCG, JDRCS = :JDRCS, JDCCS = :JDCCS, JDCLV = :JDCLV, JDAGM = :JDAGM, JDLCV = :JDLCV, JDLCA = :JDLCA, JDLCW = :JDLCW, JDLCU = :JDLCU, JDLCE = :JDLCE
, JDDPA = :JDDPA, JDDPM = :JDDPM, JDDPJ = :JDDPJ, JDFPA = :JDFPA, JDFPM = :JDFPM, JDFPJ = :JDFPJ, JDPEA = :JDPEA, JDPEM = :JDPEM, JDPEJ = :JDPEJ, JDACQ = :JDACQ
, JDTMC = :JDTMC, JDTFO = :JDTFO, JDTFT = :JDTFT, JDTFF = :JDTFF, JDTFP = :JDTFP, JDPRO = :JDPRO, JDTMI = :JDTMI, JDTFM = :JDTFM, JDTMA = :JDTMA, JDTMG = :JDTMG
, JDCMC = :JDCMC, JDCFO = :JDCFO, JDCFT = :JDCFT, JDCFH = :JDCFH, JDCHT = :JDCHT, JDCTT = :JDCTT, JDCCP = :JDCCP, JDEHH = :JDEHH, JDEHC = :JDEHC, JDSMP = :JDSMP
, JDIVX = :JDIVX, JDTCR = :JDTCR, JDNPG = :JDNPG, JDEDI = :JDEDI, JDEDN = :JDEDN, JDEDA = :JDEDA, JDEDM = :JDEDM, JDEDJ = :JDEDJ, JDEHI = :JDEHI, JDIAX = :JDIAX
, JDTED = :JDTED, JDDOO = :JDDOO, JDRUA = :JDRUA, JDRUM = :JDRUM, JDRUJ = :JDRUJ, JDECG = :JDECG, JDECP = :JDECP, JDAPT = :JDAPT, JDAPR = :JDAPR, JDAAT = :JDAAT
, JDAAR = :JDAAR, JDACR = :JDACR, JDACV = :JDACV, JDAXT = :JDAXT, JDAXC = :JDAXC, JDAXF = :JDAXF, JDAXM = :JDAXM, JDAXG = :JDAXG, JDATA = :JDATA, JDATX = :JDATX
, JDAUT = :JDAUT, JDAUF = :JDAUF, JDLTA = :JDLTA, JDLTASP = :JDLTASP, JDLDEB = :JDLDEB, JDLDEH = :JDLDEH, JDLFIN = :JDLFIN, JDLFIH = :JDLFIH, JDLDUR = :JDLDUR, JDLDUU = :JDLDUU

 WHERE 
JDIPB = :numeroAffaire and JDALX = :numeroAliment and JDAVN = :numeroAvenant";
            const string delete=@"DELETE FROM YHRTENT WHERE JDIPB = :numeroAffaire AND JDALX = :numeroAliment AND JDAVN = :numeroAvenant";
            const string insert=@"INSERT INTO  YHRTENT (
JDIPB, JDALX, JDAVN, JDHIN, JDSHT
, JDENC, JDITC, JDVAL, JDVAA, JDVAW
, JDVAT, JDVAU, JDVAH, JDDRQ, JDNBR
, JDTXL, JDTRR, JDXCM, JDNEX, JDNPA
, JDAFC, JDAFR, JDATT, JDCNA, JDCNC
, JDINA, JDIND, JDIXC, JDIXF, JDIXL
, JDIXP, JDIVO, JDIVA, JDIVW, JDMHT
, JDREA, JDREB, JDREH, JDDPV, JDGAU
, JDGVL, JDGUN, JDPBN, JDPBS, JDPBR
, JDPBT, JDPBC, JDPBP, JDPBA, JDRCG
, JDCCG, JDRCS, JDCCS, JDCLV, JDAGM
, JDLCV, JDLCA, JDLCW, JDLCU, JDLCE
, JDDPA, JDDPM, JDDPJ, JDFPA, JDFPM
, JDFPJ, JDPEA, JDPEM, JDPEJ, JDACQ
, JDTMC, JDTFO, JDTFT, JDTFF, JDTFP
, JDPRO, JDTMI, JDTFM, JDTMA, JDTMG
, JDCMC, JDCFO, JDCFT, JDCFH, JDCHT
, JDCTT, JDCCP, JDEHH, JDEHC, JDSMP
, JDIVX, JDTCR, JDNPG, JDEDI, JDEDN
, JDEDA, JDEDM, JDEDJ, JDEHI, JDIAX
, JDTED, JDDOO, JDRUA, JDRUM, JDRUJ
, JDECG, JDECP, JDAPT, JDAPR, JDAAT
, JDAAR, JDACR, JDACV, JDAXT, JDAXC
, JDAXF, JDAXM, JDAXG, JDATA, JDATX
, JDAUT, JDAUF, JDLTA, JDLTASP, JDLDEB
, JDLDEH, JDLFIN, JDLFIH, JDLDUR, JDLDUU

) VALUES (
:JDIPB, :JDALX, :JDAVN, :JDHIN, :JDSHT
, :JDENC, :JDITC, :JDVAL, :JDVAA, :JDVAW
, :JDVAT, :JDVAU, :JDVAH, :JDDRQ, :JDNBR
, :JDTXL, :JDTRR, :JDXCM, :JDNEX, :JDNPA
, :JDAFC, :JDAFR, :JDATT, :JDCNA, :JDCNC
, :JDINA, :JDIND, :JDIXC, :JDIXF, :JDIXL
, :JDIXP, :JDIVO, :JDIVA, :JDIVW, :JDMHT
, :JDREA, :JDREB, :JDREH, :JDDPV, :JDGAU
, :JDGVL, :JDGUN, :JDPBN, :JDPBS, :JDPBR
, :JDPBT, :JDPBC, :JDPBP, :JDPBA, :JDRCG
, :JDCCG, :JDRCS, :JDCCS, :JDCLV, :JDAGM
, :JDLCV, :JDLCA, :JDLCW, :JDLCU, :JDLCE
, :JDDPA, :JDDPM, :JDDPJ, :JDFPA, :JDFPM
, :JDFPJ, :JDPEA, :JDPEM, :JDPEJ, :JDACQ
, :JDTMC, :JDTFO, :JDTFT, :JDTFF, :JDTFP
, :JDPRO, :JDTMI, :JDTFM, :JDTMA, :JDTMG
, :JDCMC, :JDCFO, :JDCFT, :JDCFH, :JDCHT
, :JDCTT, :JDCCP, :JDEHH, :JDEHC, :JDSMP
, :JDIVX, :JDTCR, :JDNPG, :JDEDI, :JDEDN
, :JDEDA, :JDEDM, :JDEDJ, :JDEHI, :JDIAX
, :JDTED, :JDDOO, :JDRUA, :JDRUM, :JDRUJ
, :JDECG, :JDECP, :JDAPT, :JDAPR, :JDAAT
, :JDAAR, :JDACR, :JDACV, :JDAXT, :JDAXC
, :JDAXF, :JDAXM, :JDAXG, :JDATA, :JDATX
, :JDAUT, :JDAUF, :JDLTA, :JDLTASP, :JDLDEB
, :JDLDEH, :JDLFIN, :JDLFIH, :JDLDUR, :JDLDUU
)";
            #endregion

            public YhrtentRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YprtEnt Get(string numeroAffaire, int numeroAliment, int numeroAvenant){
                return connection.Query<YprtEnt>(select, new {numeroAffaire, numeroAliment, numeroAvenant}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("JDIPB");
            }

            public void Insert(YprtEnt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JDIPB",value.Jdipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JDALX",value.Jdalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JDAVN",value.Jdavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JDHIN",value.Jdhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JDSHT",value.Jdsht??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDENC",value.Jdenc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDITC",value.Jditc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDVAL",value.Jdval, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDVAA",value.Jdvaa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDVAW",value.Jdvaw, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDVAT",value.Jdvat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JDVAU",value.Jdvau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDVAH",value.Jdvah??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDDRQ",value.Jddrq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JDNBR",value.Jdnbr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JDTXL",value.Jdtxl, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDTRR",value.Jdtrr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JDXCM",value.Jdxcm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("JDNEX",value.Jdnex, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDNPA",value.Jdnpa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDAFC",value.Jdafc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDAFR",value.Jdafr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("JDATT",value.Jdatt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDCNA",value.Jdcna??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDCNC",value.Jdcnc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("JDINA",value.Jdina??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDIND",value.Jdind??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JDIXC",value.Jdixc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDIXF",value.Jdixf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDIXL",value.Jdixl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDIXP",value.Jdixp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDIVO",value.Jdivo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("JDIVA",value.Jdiva, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("JDIVW",value.Jdivw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("JDMHT",value.Jdmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDREA",value.Jdrea??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDREB",value.Jdreb, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDREH",value.Jdreh??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDDPV",value.Jddpv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDGAU",value.Jdgau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDGVL",value.Jdgvl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JDGUN",value.Jdgun??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDPBN",value.Jdpbn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDPBS",value.Jdpbs, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JDPBR",value.Jdpbr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JDPBT",value.Jdpbt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDPBC",value.Jdpbc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDPBP",value.Jdpbp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JDPBA",value.Jdpba, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDRCG",value.Jdrcg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("JDCCG",value.Jdccg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("JDRCS",value.Jdrcs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("JDCCS",value.Jdccs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("JDCLV",value.Jdclv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDAGM",value.Jdagm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDLCV",value.Jdlcv, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDLCA",value.Jdlca, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDLCW",value.Jdlcw, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDLCU",value.Jdlcu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDLCE",value.Jdlce??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDDPA",value.Jddpa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JDDPM",value.Jddpm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDDPJ",value.Jddpj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDFPA",value.Jdfpa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JDFPM",value.Jdfpm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDFPJ",value.Jdfpj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDPEA",value.Jdpea, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JDPEM",value.Jdpem, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDPEJ",value.Jdpej, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDACQ",value.Jdacq, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDTMC",value.Jdtmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDTFO",value.Jdtfo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDTFT",value.Jdtft??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDTFF",value.Jdtff, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDTFP",value.Jdtfp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 9);
                    parameters.Add("JDPRO",value.Jdpro??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDTMI",value.Jdtmi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDTFM",value.Jdtfm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDTMA",value.Jdtma, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDTMG",value.Jdtmg, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDCMC",value.Jdcmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDCFO",value.Jdcfo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDCFT",value.Jdcft??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDCFH",value.Jdcfh??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDCHT",value.Jdcht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDCTT",value.Jdctt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDCCP",value.Jdccp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 9);
                    parameters.Add("JDEHH",value.Jdehh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDEHC",value.Jdehc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDSMP",value.Jdsmp, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDIVX",value.Jdivx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDTCR",value.Jdtcr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDNPG",value.Jdnpg, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDEDI",value.Jdedi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDEDN",value.Jdedn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JDEDA",value.Jdeda, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JDEDM",value.Jdedm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDEDJ",value.Jdedj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDEHI",value.Jdehi, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDIAX",value.Jdiax??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDTED",value.Jdted??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDDOO",value.Jddoo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("JDRUA",value.Jdrua, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JDRUM",value.Jdrum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDRUJ",value.Jdruj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDECG",value.Jdecg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDECP",value.Jdecp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDAPT",value.Jdapt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDAPR",value.Jdapr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDAAT",value.Jdaat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDAAR",value.Jdaar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDACR",value.Jdacr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDACV",value.Jdacv, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("JDAXT",value.Jdaxt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("JDAXC",value.Jdaxc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("JDAXF",value.Jdaxf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("JDAXM",value.Jdaxm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("JDAXG",value.Jdaxg, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("JDATA",value.Jdata??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDATX",value.Jdatx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 3);
                    parameters.Add("JDAUT",value.Jdaut, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("JDAUF",value.Jdauf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("JDLTA",value.Jdlta??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDLTASP",value.Jdltasp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 4);
                    parameters.Add("JDLDEB",value.Jdldeb, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("JDLDEH",value.Jdldeh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JDLFIN",value.Jdlfin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("JDLFIH",value.Jdlfih, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JDLDUR",value.Jdldur, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JDLDUU",value.Jdlduu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YprtEnt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("numeroAffaire",value.Jdipb);
                    parameters.Add("numeroAliment",value.Jdalx);
                    parameters.Add("numeroAvenant",value.Jdavn);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YprtEnt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JDIPB",value.Jdipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JDALX",value.Jdalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JDAVN",value.Jdavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JDHIN",value.Jdhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JDSHT",value.Jdsht??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDENC",value.Jdenc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDITC",value.Jditc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDVAL",value.Jdval, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDVAA",value.Jdvaa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDVAW",value.Jdvaw, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDVAT",value.Jdvat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JDVAU",value.Jdvau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDVAH",value.Jdvah??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDDRQ",value.Jddrq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JDNBR",value.Jdnbr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JDTXL",value.Jdtxl, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDTRR",value.Jdtrr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JDXCM",value.Jdxcm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("JDNEX",value.Jdnex, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDNPA",value.Jdnpa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDAFC",value.Jdafc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDAFR",value.Jdafr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("JDATT",value.Jdatt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDCNA",value.Jdcna??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDCNC",value.Jdcnc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("JDINA",value.Jdina??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDIND",value.Jdind??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JDIXC",value.Jdixc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDIXF",value.Jdixf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDIXL",value.Jdixl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDIXP",value.Jdixp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDIVO",value.Jdivo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("JDIVA",value.Jdiva, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("JDIVW",value.Jdivw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("JDMHT",value.Jdmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDREA",value.Jdrea??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDREB",value.Jdreb, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDREH",value.Jdreh??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDDPV",value.Jddpv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDGAU",value.Jdgau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDGVL",value.Jdgvl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JDGUN",value.Jdgun??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDPBN",value.Jdpbn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDPBS",value.Jdpbs, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JDPBR",value.Jdpbr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JDPBT",value.Jdpbt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDPBC",value.Jdpbc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDPBP",value.Jdpbp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JDPBA",value.Jdpba, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDRCG",value.Jdrcg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("JDCCG",value.Jdccg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("JDRCS",value.Jdrcs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("JDCCS",value.Jdccs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("JDCLV",value.Jdclv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDAGM",value.Jdagm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDLCV",value.Jdlcv, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDLCA",value.Jdlca, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDLCW",value.Jdlcw, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDLCU",value.Jdlcu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDLCE",value.Jdlce??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDDPA",value.Jddpa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JDDPM",value.Jddpm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDDPJ",value.Jddpj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDFPA",value.Jdfpa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JDFPM",value.Jdfpm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDFPJ",value.Jdfpj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDPEA",value.Jdpea, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JDPEM",value.Jdpem, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDPEJ",value.Jdpej, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDACQ",value.Jdacq, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDTMC",value.Jdtmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDTFO",value.Jdtfo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDTFT",value.Jdtft??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDTFF",value.Jdtff, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDTFP",value.Jdtfp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 9);
                    parameters.Add("JDPRO",value.Jdpro??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDTMI",value.Jdtmi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDTFM",value.Jdtfm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDTMA",value.Jdtma, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDTMG",value.Jdtmg, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDCMC",value.Jdcmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDCFO",value.Jdcfo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDCFT",value.Jdcft??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDCFH",value.Jdcfh??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDCHT",value.Jdcht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDCTT",value.Jdctt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDCCP",value.Jdccp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 9);
                    parameters.Add("JDEHH",value.Jdehh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDEHC",value.Jdehc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDSMP",value.Jdsmp, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JDIVX",value.Jdivx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDTCR",value.Jdtcr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDNPG",value.Jdnpg, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDEDI",value.Jdedi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDEDN",value.Jdedn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JDEDA",value.Jdeda, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JDEDM",value.Jdedm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDEDJ",value.Jdedj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDEHI",value.Jdehi, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JDIAX",value.Jdiax??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDTED",value.Jdted??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDDOO",value.Jddoo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("JDRUA",value.Jdrua, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JDRUM",value.Jdrum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDRUJ",value.Jdruj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDECG",value.Jdecg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDECP",value.Jdecp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDAPT",value.Jdapt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDAPR",value.Jdapr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDAAT",value.Jdaat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDAAR",value.Jdaar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDACR",value.Jdacr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDACV",value.Jdacv, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("JDAXT",value.Jdaxt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("JDAXC",value.Jdaxc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("JDAXF",value.Jdaxf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("JDAXM",value.Jdaxm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("JDAXG",value.Jdaxg, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("JDATA",value.Jdata??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JDATX",value.Jdatx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 3);
                    parameters.Add("JDAUT",value.Jdaut, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("JDAUF",value.Jdauf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("JDLTA",value.Jdlta??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JDLTASP",value.Jdltasp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 4);
                    parameters.Add("JDLDEB",value.Jdldeb, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("JDLDEH",value.Jdldeh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JDLFIN",value.Jdlfin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("JDLFIH",value.Jdlfih, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JDLDUR",value.Jdldur, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JDLDUU",value.Jdlduu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("numeroAffaire",value.Jdipb);
                    parameters.Add("numeroAliment",value.Jdalx);
                    parameters.Add("numeroAvenant",value.Jdavn);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
    }
}
