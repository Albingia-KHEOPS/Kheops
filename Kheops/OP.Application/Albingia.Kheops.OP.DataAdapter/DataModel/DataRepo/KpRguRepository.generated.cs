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

    public  partial class  KpRguRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KHWID, KHWTYP, KHWIPB, KHWALX, KHWTTR
, KHWRGAV, KHWAVN, KHWAVND, KHWEXE, KHWDEBP
, KHWFINP, KHWTRGU, KHWIPK, KHWICT, KHWICC
, KHWXCM, KHWCNC, KHWENC, KHWAFC, KHWAFR
, KHWATT, KHWMHT, KHWCNH, KHWGRG, KHWTTT
, KHWMTT, KHWETA, KHWSIT, KHWSTU, KHWSTD
, KHWSTH, KHWCRU, KHWCRD, KHWMAJU, KHWMAJD
, KHWAVP, KHWDESI, KHWOBSV, KHWOBSC, KHWMTF
, KHWMRG, KHWACC, KHWSUAV, KHWNEM, KHWASV
, KHWMIN, KHWBRNC, KHWPBT, KHWPBA, KHWPBS
, KHWPBR, KHWPBP, KHWRIA, KHWRIAF, KHWEMH
, KHWEMHF, KHWCOT, KHWBRNT, KHWSCHG, KHWSIDF
, KHWSREC, KHWSPRO, KHWSPRE, KHWSREP, KHWSRIM
, KHWMHC, KHWPBTR, KHWEMD, KHWSPC, KHWMTX
, KHWCNT, KHWECT, KHWEMT FROM KPRGU
WHERE KHWID = :id
";
            const string update=@"UPDATE KPRGU SET 
KHWID = :KHWID, KHWTYP = :KHWTYP, KHWIPB = :KHWIPB, KHWALX = :KHWALX, KHWTTR = :KHWTTR, KHWRGAV = :KHWRGAV, KHWAVN = :KHWAVN, KHWAVND = :KHWAVND, KHWEXE = :KHWEXE, KHWDEBP = :KHWDEBP
, KHWFINP = :KHWFINP, KHWTRGU = :KHWTRGU, KHWIPK = :KHWIPK, KHWICT = :KHWICT, KHWICC = :KHWICC, KHWXCM = :KHWXCM, KHWCNC = :KHWCNC, KHWENC = :KHWENC, KHWAFC = :KHWAFC, KHWAFR = :KHWAFR
, KHWATT = :KHWATT, KHWMHT = :KHWMHT, KHWCNH = :KHWCNH, KHWGRG = :KHWGRG, KHWTTT = :KHWTTT, KHWMTT = :KHWMTT, KHWETA = :KHWETA, KHWSIT = :KHWSIT, KHWSTU = :KHWSTU, KHWSTD = :KHWSTD
, KHWSTH = :KHWSTH, KHWCRU = :KHWCRU, KHWCRD = :KHWCRD, KHWMAJU = :KHWMAJU, KHWMAJD = :KHWMAJD, KHWAVP = :KHWAVP, KHWDESI = :KHWDESI, KHWOBSV = :KHWOBSV, KHWOBSC = :KHWOBSC, KHWMTF = :KHWMTF
, KHWMRG = :KHWMRG, KHWACC = :KHWACC, KHWSUAV = :KHWSUAV, KHWNEM = :KHWNEM, KHWASV = :KHWASV, KHWMIN = :KHWMIN, KHWBRNC = :KHWBRNC, KHWPBT = :KHWPBT, KHWPBA = :KHWPBA, KHWPBS = :KHWPBS
, KHWPBR = :KHWPBR, KHWPBP = :KHWPBP, KHWRIA = :KHWRIA, KHWRIAF = :KHWRIAF, KHWEMH = :KHWEMH, KHWEMHF = :KHWEMHF, KHWCOT = :KHWCOT, KHWBRNT = :KHWBRNT, KHWSCHG = :KHWSCHG, KHWSIDF = :KHWSIDF
, KHWSREC = :KHWSREC, KHWSPRO = :KHWSPRO, KHWSPRE = :KHWSPRE, KHWSREP = :KHWSREP, KHWSRIM = :KHWSRIM, KHWMHC = :KHWMHC, KHWPBTR = :KHWPBTR, KHWEMD = :KHWEMD, KHWSPC = :KHWSPC, KHWMTX = :KHWMTX
, KHWCNT = :KHWCNT, KHWECT = :KHWECT, KHWEMT = :KHWEMT
 WHERE 
KHWID = :id";
            const string delete=@"DELETE FROM KPRGU WHERE KHWID = :id";
            const string insert=@"INSERT INTO  KPRGU (
KHWID, KHWTYP, KHWIPB, KHWALX, KHWTTR
, KHWRGAV, KHWAVN, KHWAVND, KHWEXE, KHWDEBP
, KHWFINP, KHWTRGU, KHWIPK, KHWICT, KHWICC
, KHWXCM, KHWCNC, KHWENC, KHWAFC, KHWAFR
, KHWATT, KHWMHT, KHWCNH, KHWGRG, KHWTTT
, KHWMTT, KHWETA, KHWSIT, KHWSTU, KHWSTD
, KHWSTH, KHWCRU, KHWCRD, KHWMAJU, KHWMAJD
, KHWAVP, KHWDESI, KHWOBSV, KHWOBSC, KHWMTF
, KHWMRG, KHWACC, KHWSUAV, KHWNEM, KHWASV
, KHWMIN, KHWBRNC, KHWPBT, KHWPBA, KHWPBS
, KHWPBR, KHWPBP, KHWRIA, KHWRIAF, KHWEMH
, KHWEMHF, KHWCOT, KHWBRNT, KHWSCHG, KHWSIDF
, KHWSREC, KHWSPRO, KHWSPRE, KHWSREP, KHWSRIM
, KHWMHC, KHWPBTR, KHWEMD, KHWSPC, KHWMTX
, KHWCNT, KHWECT, KHWEMT
) VALUES (
:KHWID, :KHWTYP, :KHWIPB, :KHWALX, :KHWTTR
, :KHWRGAV, :KHWAVN, :KHWAVND, :KHWEXE, :KHWDEBP
, :KHWFINP, :KHWTRGU, :KHWIPK, :KHWICT, :KHWICC
, :KHWXCM, :KHWCNC, :KHWENC, :KHWAFC, :KHWAFR
, :KHWATT, :KHWMHT, :KHWCNH, :KHWGRG, :KHWTTT
, :KHWMTT, :KHWETA, :KHWSIT, :KHWSTU, :KHWSTD
, :KHWSTH, :KHWCRU, :KHWCRD, :KHWMAJU, :KHWMAJD
, :KHWAVP, :KHWDESI, :KHWOBSV, :KHWOBSC, :KHWMTF
, :KHWMRG, :KHWACC, :KHWSUAV, :KHWNEM, :KHWASV
, :KHWMIN, :KHWBRNC, :KHWPBT, :KHWPBA, :KHWPBS
, :KHWPBR, :KHWPBP, :KHWRIA, :KHWRIAF, :KHWEMH
, :KHWEMHF, :KHWCOT, :KHWBRNT, :KHWSCHG, :KHWSIDF
, :KHWSREC, :KHWSPRO, :KHWSPRE, :KHWSREP, :KHWSRIM
, :KHWMHC, :KHWPBTR, :KHWEMD, :KHWSPC, :KHWMTX
, :KHWCNT, :KHWECT, :KHWEMT)";
            const string select_GetByAffaire=@"SELECT
KHWID, KHWTYP, KHWIPB, KHWALX, KHWTTR
, KHWRGAV, KHWAVN, KHWAVND, KHWEXE, KHWDEBP
, KHWFINP, KHWTRGU, KHWIPK, KHWICT, KHWICC
, KHWXCM, KHWCNC, KHWENC, KHWAFC, KHWAFR
, KHWATT, KHWMHT, KHWCNH, KHWGRG, KHWTTT
, KHWMTT, KHWETA, KHWSIT, KHWSTU, KHWSTD
, KHWSTH, KHWCRU, KHWCRD, KHWMAJU, KHWMAJD
, KHWAVP, KHWDESI, KHWOBSV, KHWOBSC, KHWMTF
, KHWMRG, KHWACC, KHWSUAV, KHWNEM, KHWASV
, KHWMIN, KHWBRNC, KHWPBT, KHWPBA, KHWPBS
, KHWPBR, KHWPBP, KHWRIA, KHWRIAF, KHWEMH
, KHWEMHF, KHWCOT, KHWBRNT, KHWSCHG, KHWSIDF
, KHWSREC, KHWSPRO, KHWSPRE, KHWSREP, KHWSRIM
, KHWMHC, KHWPBTR, KHWEMD, KHWSPC, KHWMTX
, KHWCNT, KHWECT, KHWEMT FROM KPRGU
WHERE KHWTYP = :parKHWTYP
and KHWIPB = :parKHWIPB
and KHWALX = :parKHWALX
and KHWAVN = :parKHWAVN
";
            #endregion

            public KpRguRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpRgu Get(Int64 id){
                return connection.Query<KpRgu>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KHWID") ;
            }

            public void Insert(KpRgu value){
                    if(value.Khwid == default(Int64)) {
                        value.Khwid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KHWID",value.Khwid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHWTYP",value.Khwtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWIPB",value.Khwipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KHWALX",value.Khwalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KHWTTR",value.Khwttr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHWRGAV",value.Khwrgav??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWAVN",value.Khwavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHWAVND",value.Khwavnd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHWEXE",value.Khwexe, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KHWDEBP",value.Khwdebp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHWFINP",value.Khwfinp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHWTRGU",value.Khwtrgu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHWIPK",value.Khwipk, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHWICT",value.Khwict, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHWICC",value.Khwicc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHWXCM",value.Khwxcm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KHWCNC",value.Khwcnc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("KHWENC",value.Khwenc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWAFC",value.Khwafc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWAFR",value.Khwafr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KHWATT",value.Khwatt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWMHT",value.Khwmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWCNH",value.Khwcnh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWGRG",value.Khwgrg, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWTTT",value.Khwttt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWMTT",value.Khwmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWETA",value.Khweta??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWSIT",value.Khwsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWSTU",value.Khwstu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHWSTD",value.Khwstd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHWSTH",value.Khwsth, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KHWCRU",value.Khwcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHWCRD",value.Khwcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHWMAJU",value.Khwmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHWMAJD",value.Khwmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHWAVP",value.Khwavp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHWDESI",value.Khwdesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHWOBSV",value.Khwobsv, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHWOBSC",value.Khwobsc, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHWMTF",value.Khwmtf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHWMRG",value.Khwmrg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHWACC",value.Khwacc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWSUAV",value.Khwsuav??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWNEM",value.Khwnem??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWASV",value.Khwasv, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWMIN",value.Khwmin, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWBRNC",value.Khwbrnc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWPBT",value.Khwpbt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHWPBA",value.Khwpba, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWPBS",value.Khwpbs, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHWPBR",value.Khwpbr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHWPBP",value.Khwpbp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHWRIA",value.Khwria, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHWRIAF",value.Khwriaf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHWEMH",value.Khwemh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWEMHF",value.Khwemhf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWCOT",value.Khwcot, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWBRNT",value.Khwbrnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 4);
                    parameters.Add("KHWSCHG",value.Khwschg, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWSIDF",value.Khwsidf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWSREC",value.Khwsrec, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWSPRO",value.Khwspro, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWSPRE",value.Khwspre, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWSREP",value.Khwsrep, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWSRIM",value.Khwsrim, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWMHC",value.Khwmhc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWPBTR",value.Khwpbtr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHWEMD",value.Khwemd, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWSPC",value.Khwspc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHWMTX",value.Khwmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWCNT",value.Khwcnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWECT",value.Khwect, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWEMT",value.Khwemt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpRgu value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Khwid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpRgu value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KHWID",value.Khwid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHWTYP",value.Khwtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWIPB",value.Khwipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KHWALX",value.Khwalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KHWTTR",value.Khwttr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHWRGAV",value.Khwrgav??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWAVN",value.Khwavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHWAVND",value.Khwavnd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHWEXE",value.Khwexe, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KHWDEBP",value.Khwdebp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHWFINP",value.Khwfinp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHWTRGU",value.Khwtrgu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHWIPK",value.Khwipk, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHWICT",value.Khwict, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHWICC",value.Khwicc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHWXCM",value.Khwxcm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KHWCNC",value.Khwcnc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("KHWENC",value.Khwenc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWAFC",value.Khwafc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWAFR",value.Khwafr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KHWATT",value.Khwatt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWMHT",value.Khwmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWCNH",value.Khwcnh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWGRG",value.Khwgrg, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWTTT",value.Khwttt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWMTT",value.Khwmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWETA",value.Khweta??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWSIT",value.Khwsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWSTU",value.Khwstu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHWSTD",value.Khwstd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHWSTH",value.Khwsth, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KHWCRU",value.Khwcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHWCRD",value.Khwcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHWMAJU",value.Khwmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHWMAJD",value.Khwmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHWAVP",value.Khwavp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHWDESI",value.Khwdesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHWOBSV",value.Khwobsv, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHWOBSC",value.Khwobsc, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHWMTF",value.Khwmtf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHWMRG",value.Khwmrg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHWACC",value.Khwacc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWSUAV",value.Khwsuav??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWNEM",value.Khwnem??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWASV",value.Khwasv, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWMIN",value.Khwmin, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWBRNC",value.Khwbrnc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWPBT",value.Khwpbt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHWPBA",value.Khwpba, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHWPBS",value.Khwpbs, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHWPBR",value.Khwpbr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHWPBP",value.Khwpbp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHWRIA",value.Khwria, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHWRIAF",value.Khwriaf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHWEMH",value.Khwemh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWEMHF",value.Khwemhf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWCOT",value.Khwcot, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWBRNT",value.Khwbrnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 4);
                    parameters.Add("KHWSCHG",value.Khwschg, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWSIDF",value.Khwsidf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWSREC",value.Khwsrec, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWSPRO",value.Khwspro, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWSPRE",value.Khwspre, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWSREP",value.Khwsrep, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWSRIM",value.Khwsrim, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWMHC",value.Khwmhc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWPBTR",value.Khwpbtr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHWEMD",value.Khwemd, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHWSPC",value.Khwspc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHWMTX",value.Khwmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWCNT",value.Khwcnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWECT",value.Khwect, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHWEMT",value.Khwemt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("id",value.Khwid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpRgu> GetByAffaire(string parKHWTYP, string parKHWIPB, int parKHWALX, int parKHWAVN){
                    return connection.EnsureOpened().Query<KpRgu>(select_GetByAffaire, new {parKHWTYP, parKHWIPB, parKHWALX, parKHWAVN}).ToList();
            }
    }
}
