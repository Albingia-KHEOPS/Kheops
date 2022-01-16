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

    public  partial class  HpgaranRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDEID, KDETYP, KDEIPB, KDEALX, KDEAVN
, KDEHIN, KDEFOR, KDEOPT, KDEKDCID, KDEGARAN
, KDESEQ, KDENIVEAU, KDESEM, KDESE1, KDETRI
, KDENUMPRES, KDEAJOUT, KDECAR, KDENAT, KDEGAN
, KDEKDFID, KDEDEFG, KDEKDHID, KDEDATDEB, KDEHEUDEB
, KDEDATFIN, KDEHEUFIN, KDEDUREE, KDEDURUNI, KDEPRP
, KDETYPEMI, KDEALIREF, KDECATNAT, KDEINA, KDETAXCOD
, KDETAXREP, KDECRAVN, KDECRU, KDECRD, KDEMAJAVN
, KDEASVALO, KDEASVALA, KDEASVALW, KDEASUNIT, KDEASBASE
, KDEASMOD, KDEASOBLI, KDEINVSP, KDEINVEN, KDEWDDEB
, KDEWHDEB, KDEWDFIN, KDEWHFIN, KDETCD, KDEMODI
, KDEPIND, KDEPCATN, KDEPREF, KDEPPRP, KDEPEMI
, KDEPTAXC, KDEPNTM, KDEALA, KDEPALA, KDEALO
 FROM HPGARAN
WHERE KDEID = :id
and KDEAVN = :numeroAvenant
";
            const string update=@"UPDATE HPGARAN SET 
KDEID = :KDEID, KDETYP = :KDETYP, KDEIPB = :KDEIPB, KDEALX = :KDEALX, KDEAVN = :KDEAVN, KDEHIN = :KDEHIN, KDEFOR = :KDEFOR, KDEOPT = :KDEOPT, KDEKDCID = :KDEKDCID, KDEGARAN = :KDEGARAN
, KDESEQ = :KDESEQ, KDENIVEAU = :KDENIVEAU, KDESEM = :KDESEM, KDESE1 = :KDESE1, KDETRI = :KDETRI, KDENUMPRES = :KDENUMPRES, KDEAJOUT = :KDEAJOUT, KDECAR = :KDECAR, KDENAT = :KDENAT, KDEGAN = :KDEGAN
, KDEKDFID = :KDEKDFID, KDEDEFG = :KDEDEFG, KDEKDHID = :KDEKDHID, KDEDATDEB = :KDEDATDEB, KDEHEUDEB = :KDEHEUDEB, KDEDATFIN = :KDEDATFIN, KDEHEUFIN = :KDEHEUFIN, KDEDUREE = :KDEDUREE, KDEDURUNI = :KDEDURUNI, KDEPRP = :KDEPRP
, KDETYPEMI = :KDETYPEMI, KDEALIREF = :KDEALIREF, KDECATNAT = :KDECATNAT, KDEINA = :KDEINA, KDETAXCOD = :KDETAXCOD, KDETAXREP = :KDETAXREP, KDECRAVN = :KDECRAVN, KDECRU = :KDECRU, KDECRD = :KDECRD, KDEMAJAVN = :KDEMAJAVN
, KDEASVALO = :KDEASVALO, KDEASVALA = :KDEASVALA, KDEASVALW = :KDEASVALW, KDEASUNIT = :KDEASUNIT, KDEASBASE = :KDEASBASE, KDEASMOD = :KDEASMOD, KDEASOBLI = :KDEASOBLI, KDEINVSP = :KDEINVSP, KDEINVEN = :KDEINVEN, KDEWDDEB = :KDEWDDEB
, KDEWHDEB = :KDEWHDEB, KDEWDFIN = :KDEWDFIN, KDEWHFIN = :KDEWHFIN, KDETCD = :KDETCD, KDEMODI = :KDEMODI, KDEPIND = :KDEPIND, KDEPCATN = :KDEPCATN, KDEPREF = :KDEPREF, KDEPPRP = :KDEPPRP, KDEPEMI = :KDEPEMI
, KDEPTAXC = :KDEPTAXC, KDEPNTM = :KDEPNTM, KDEALA = :KDEALA, KDEPALA = :KDEPALA, KDEALO = :KDEALO
 WHERE 
KDEID = :id and KDEAVN = :numeroAvenant";
            const string delete=@"DELETE FROM HPGARAN WHERE KDEID = :id AND KDEAVN = :numeroAvenant";
            const string insert=@"INSERT INTO  HPGARAN (
KDEID, KDETYP, KDEIPB, KDEALX, KDEAVN
, KDEHIN, KDEFOR, KDEOPT, KDEKDCID, KDEGARAN
, KDESEQ, KDENIVEAU, KDESEM, KDESE1, KDETRI
, KDENUMPRES, KDEAJOUT, KDECAR, KDENAT, KDEGAN
, KDEKDFID, KDEDEFG, KDEKDHID, KDEDATDEB, KDEHEUDEB
, KDEDATFIN, KDEHEUFIN, KDEDUREE, KDEDURUNI, KDEPRP
, KDETYPEMI, KDEALIREF, KDECATNAT, KDEINA, KDETAXCOD
, KDETAXREP, KDECRAVN, KDECRU, KDECRD, KDEMAJAVN
, KDEASVALO, KDEASVALA, KDEASVALW, KDEASUNIT, KDEASBASE
, KDEASMOD, KDEASOBLI, KDEINVSP, KDEINVEN, KDEWDDEB
, KDEWHDEB, KDEWDFIN, KDEWHFIN, KDETCD, KDEMODI
, KDEPIND, KDEPCATN, KDEPREF, KDEPPRP, KDEPEMI
, KDEPTAXC, KDEPNTM, KDEALA, KDEPALA, KDEALO

) VALUES (
:KDEID, :KDETYP, :KDEIPB, :KDEALX, :KDEAVN
, :KDEHIN, :KDEFOR, :KDEOPT, :KDEKDCID, :KDEGARAN
, :KDESEQ, :KDENIVEAU, :KDESEM, :KDESE1, :KDETRI
, :KDENUMPRES, :KDEAJOUT, :KDECAR, :KDENAT, :KDEGAN
, :KDEKDFID, :KDEDEFG, :KDEKDHID, :KDEDATDEB, :KDEHEUDEB
, :KDEDATFIN, :KDEHEUFIN, :KDEDUREE, :KDEDURUNI, :KDEPRP
, :KDETYPEMI, :KDEALIREF, :KDECATNAT, :KDEINA, :KDETAXCOD
, :KDETAXREP, :KDECRAVN, :KDECRU, :KDECRD, :KDEMAJAVN
, :KDEASVALO, :KDEASVALA, :KDEASVALW, :KDEASUNIT, :KDEASBASE
, :KDEASMOD, :KDEASOBLI, :KDEINVSP, :KDEINVEN, :KDEWDDEB
, :KDEWHDEB, :KDEWDFIN, :KDEWHFIN, :KDETCD, :KDEMODI
, :KDEPIND, :KDEPCATN, :KDEPREF, :KDEPPRP, :KDEPEMI
, :KDEPTAXC, :KDEPNTM, :KDEALA, :KDEPALA, :KDEALO
)";
            const string select_GetByAffaire=@"SELECT
KDEID, KDETYP, KDEIPB, KDEALX, KDEAVN
, KDEHIN, KDEFOR, KDEOPT, KDEKDCID, KDEGARAN
, KDESEQ, KDENIVEAU, KDESEM, KDESE1, KDETRI
, KDENUMPRES, KDEAJOUT, KDECAR, KDENAT, KDEGAN
, KDEKDFID, KDEDEFG, KDEKDHID, KDEDATDEB, KDEHEUDEB
, KDEDATFIN, KDEHEUFIN, KDEDUREE, KDEDURUNI, KDEPRP
, KDETYPEMI, KDEALIREF, KDECATNAT, KDEINA, KDETAXCOD
, KDETAXREP, KDECRAVN, KDECRU, KDECRD, KDEMAJAVN
, KDEASVALO, KDEASVALA, KDEASVALW, KDEASUNIT, KDEASBASE
, KDEASMOD, KDEASOBLI, KDEINVSP, KDEINVEN, KDEWDDEB
, KDEWHDEB, KDEWDFIN, KDEWHFIN, KDETCD, KDEMODI
, KDEPIND, KDEPCATN, KDEPREF, KDEPPRP, KDEPEMI
, KDEPTAXC, KDEPNTM, KDEALA, KDEPALA, KDEALO
 FROM HPGARAN
WHERE KDETYP = :type
and KDEIPB = :numeroAffaire
and KDEALX = :numeroAliment
and KDEAVN = :numeroAvenant
";
            const string select_GetByFormule=@"SELECT
KDEID, KDETYP, KDEIPB, KDEALX, KDEAVN
, KDEHIN, KDEFOR, KDEOPT, KDEKDCID, KDEGARAN
, KDESEQ, KDENIVEAU, KDESEM, KDESE1, KDETRI
, KDENUMPRES, KDEAJOUT, KDECAR, KDENAT, KDEGAN
, KDEKDFID, KDEDEFG, KDEKDHID, KDEDATDEB, KDEHEUDEB
, KDEDATFIN, KDEHEUFIN, KDEDUREE, KDEDURUNI, KDEPRP
, KDETYPEMI, KDEALIREF, KDECATNAT, KDEINA, KDETAXCOD
, KDETAXREP, KDECRAVN, KDECRU, KDECRD, KDEMAJAVN
, KDEASVALO, KDEASVALA, KDEASVALW, KDEASUNIT, KDEASBASE
, KDEASMOD, KDEASOBLI, KDEINVSP, KDEINVEN, KDEWDDEB
, KDEWHDEB, KDEWDFIN, KDEWHFIN, KDETCD, KDEMODI
, KDEPIND, KDEPCATN, KDEPREF, KDEPPRP, KDEPEMI
, KDEPTAXC, KDEPNTM, KDEALA, KDEPALA, KDEALO
 FROM HPGARAN
INNER JOIN HPOPTD on KDEKDCID = KDCID and KDEAVN = KDCID
INNER JOIN HPOPT on KDCKDBID = KDBID and KDCAVN = KDBAVN
WHERE KDBKDAID = :idFormule
and KDBAVN = :numeroAvenant
";
            const string select_GetByOption=@"SELECT
KDEID, KDETYP, KDEIPB, KDEALX, KDEAVN
, KDEHIN, KDEFOR, KDEOPT, KDEKDCID, KDEGARAN
, KDESEQ, KDENIVEAU, KDESEM, KDESE1, KDETRI
, KDENUMPRES, KDEAJOUT, KDECAR, KDENAT, KDEGAN
, KDEKDFID, KDEDEFG, KDEKDHID, KDEDATDEB, KDEHEUDEB
, KDEDATFIN, KDEHEUFIN, KDEDUREE, KDEDURUNI, KDEPRP
, KDETYPEMI, KDEALIREF, KDECATNAT, KDEINA, KDETAXCOD
, KDETAXREP, KDECRAVN, KDECRU, KDECRD, KDEMAJAVN
, KDEASVALO, KDEASVALA, KDEASVALW, KDEASUNIT, KDEASBASE
, KDEASMOD, KDEASOBLI, KDEINVSP, KDEINVEN, KDEWDDEB
, KDEWHDEB, KDEWDFIN, KDEWHFIN, KDETCD, KDEMODI
, KDEPIND, KDEPCATN, KDEPREF, KDEPPRP, KDEPEMI
, KDEPTAXC, KDEPNTM, KDEALA, KDEPALA, KDEALO
 FROM HPGARAN
INNER JOIN HPOPT one KDEKDCID = KDCID and KDEAVN = KDCAVN
WHERE KDCKDBID = :idOption
and KDCAVN = :numeroAvenant
";
            const string select_GetByIpbAlx=@"SELECT
KDEID, KDETYP, KDEIPB, KDEALX, KDEAVN
, KDEHIN, KDEFOR, KDEOPT, KDEKDCID, KDEGARAN
, KDESEQ, KDENIVEAU, KDESEM, KDESE1, KDETRI
, KDENUMPRES, KDEAJOUT, KDECAR, KDENAT, KDEGAN
, KDEKDFID, KDEDEFG, KDEKDHID, KDEDATDEB, KDEHEUDEB
, KDEDATFIN, KDEHEUFIN, KDEDUREE, KDEDURUNI, KDEPRP
, KDETYPEMI, KDEALIREF, KDECATNAT, KDEINA, KDETAXCOD
, KDETAXREP, KDECRAVN, KDECRU, KDECRD, KDEMAJAVN
, KDEASVALO, KDEASVALA, KDEASVALW, KDEASUNIT, KDEASBASE
, KDEASMOD, KDEASOBLI, KDEINVSP, KDEINVEN, KDEWDDEB
, KDEWHDEB, KDEWDFIN, KDEWHFIN, KDETCD, KDEMODI
, KDEPIND, KDEPCATN, KDEPREF, KDEPPRP, KDEPEMI
, KDEPTAXC, KDEPNTM, KDEALA, KDEPALA, KDEALO
 FROM HPGARAN
WHERE KDEIPB = :parKDEIPB
and KDEALX = :parKDEALX
";
            const string select_GetBySequence=@"SELECT
KDEID, KDETYP, KDEIPB, KDEALX, KDEAVN
, KDEHIN, KDEFOR, KDEOPT, KDEKDCID, KDEGARAN
, KDESEQ, KDENIVEAU, KDESEM, KDESE1, KDETRI
, KDENUMPRES, KDEAJOUT, KDECAR, KDENAT, KDEGAN
, KDEKDFID, KDEDEFG, KDEKDHID, KDEDATDEB, KDEHEUDEB
, KDEDATFIN, KDEHEUFIN, KDEDUREE, KDEDURUNI, KDEPRP
, KDETYPEMI, KDEALIREF, KDECATNAT, KDEINA, KDETAXCOD
, KDETAXREP, KDECRAVN, KDECRU, KDECRD, KDEMAJAVN
, KDEASVALO, KDEASVALA, KDEASVALW, KDEASUNIT, KDEASBASE
, KDEASMOD, KDEASOBLI, KDEINVSP, KDEINVEN, KDEWDDEB
, KDEWHDEB, KDEWDFIN, KDEWHFIN, KDETCD, KDEMODI
, KDEPIND, KDEPCATN, KDEPREF, KDEPPRP, KDEPEMI
, KDEPTAXC, KDEPNTM, KDEALA, KDEPALA, KDEALO
 FROM HPGARAN
WHERE KDEIPB = :parKDEIPB
and KDEALX = :parKDEALX
and KDETYP = :parKDETYP
and KDEAVN = :parKDEAVN
and KDESEQ = :parKDESEQ
";
            const string select_GetLatestById=@"SELECT
KDEID, KDETYP, KDEIPB, KDEALX, KDEAVN
, KDEHIN, KDEFOR, KDEOPT, KDEKDCID, KDEGARAN
, KDESEQ, KDENIVEAU, KDESEM, KDESE1, KDETRI
, KDENUMPRES, KDEAJOUT, KDECAR, KDENAT, KDEGAN
, KDEKDFID, KDEDEFG, KDEKDHID, KDEDATDEB, KDEHEUDEB
, KDEDATFIN, KDEHEUFIN, KDEDUREE, KDEDURUNI, KDEPRP
, KDETYPEMI, KDEALIREF, KDECATNAT, KDEINA, KDETAXCOD
, KDETAXREP, KDECRAVN, KDECRU, KDECRD, KDEMAJAVN
, KDEASVALO, KDEASVALA, KDEASVALW, KDEASUNIT, KDEASBASE
, KDEASMOD, KDEASOBLI, KDEINVSP, KDEINVEN, KDEWDDEB
, KDEWHDEB, KDEWDFIN, KDEWHFIN, KDETCD, KDEMODI
, KDEPIND, KDEPCATN, KDEPREF, KDEPPRP, KDEPEMI
, KDEPTAXC, KDEPNTM, KDEALA, KDEPALA, KDEALO
 FROM HPGARAN
WHERE KDEID = :parKDEID
ORDER BY KDEAVN DEDSC
FETCH FIRST 1 ROWS ONLY
";

        
 #endregion

        public HpgaranRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpGaran Get(Int64 id, int numeroAvenant){
                return connection.Query<KpGaran>(select, new {id, numeroAvenant}).SingleOrDefault();
            }


            public void Insert(KpGaran value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDEID",value.Kdeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDETYP",value.Kdetyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEIPB",value.Kdeipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDEALX",value.Kdealx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDEAVN",value.Kdeavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDEHIN",value.Kdehin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDEFOR",value.Kdefor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDEOPT",value.Kdeopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDEKDCID",value.Kdekdcid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDEGARAN",value.Kdegaran??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDESEQ",value.Kdeseq, dbType:DbType.Int64, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDENIVEAU",value.Kdeniveau, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDESEM",value.Kdesem, dbType:DbType.Int64, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDESE1",value.Kdese1, dbType:DbType.Int64, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDETRI",value.Kdetri??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:18, scale: 0);
                    parameters.Add("KDENUMPRES",value.Kdenumpres, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDEAJOUT",value.Kdeajout??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDECAR",value.Kdecar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDENAT",value.Kdenat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEGAN",value.Kdegan??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEKDFID",value.Kdekdfid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDEDEFG",value.Kdedefg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDEKDHID",value.Kdekdhid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDEDATDEB",value.Kdedatdeb, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDEHEUDEB",value.Kdeheudeb, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KDEDATFIN",value.Kdedatfin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDEHEUFIN",value.Kdeheufin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KDEDUREE",value.Kdeduree, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDEDURUNI",value.Kdeduruni??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEPRP",value.Kdeprp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDETYPEMI",value.Kdetypemi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEALIREF",value.Kdealiref??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDECATNAT",value.Kdecatnat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEINA",value.Kdeina??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDETAXCOD",value.Kdetaxcod??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDETAXREP",value.Kdetaxrep, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KDECRAVN",value.Kdecravn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDECRU",value.Kdecru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDECRD",value.Kdecrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDEMAJAVN",value.Kdemajavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDEASVALO",value.Kdeasvalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDEASVALA",value.Kdeasvala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDEASVALW",value.Kdeasvalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDEASUNIT",value.Kdeasunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDEASBASE",value.Kdeasbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDEASMOD",value.Kdeasmod??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEASOBLI",value.Kdeasobli??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEINVSP",value.Kdeinvsp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEINVEN",value.Kdeinven, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDEWDDEB",value.Kdewddeb, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDEWHDEB",value.Kdewhdeb, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KDEWDFIN",value.Kdewdfin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDEWHFIN",value.Kdewhfin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KDETCD",value.Kdetcd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDEMODI",value.Kdemodi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEPIND",value.Kdepind??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEPCATN",value.Kdepcatn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEPREF",value.Kdepref??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEPPRP",value.Kdepprp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEPEMI",value.Kdepemi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEPTAXC",value.Kdeptaxc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDEPNTM",value.Kdepntm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEALA",value.Kdeala??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEPALA",value.Kdepala??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEALO",value.Kdealo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpGaran value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Kdeid);
                    parameters.Add("numeroAvenant",value.Kdeavn);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpGaran value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDEID",value.Kdeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDETYP",value.Kdetyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEIPB",value.Kdeipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDEALX",value.Kdealx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDEAVN",value.Kdeavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDEHIN",value.Kdehin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDEFOR",value.Kdefor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDEOPT",value.Kdeopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDEKDCID",value.Kdekdcid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDEGARAN",value.Kdegaran??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDESEQ",value.Kdeseq, dbType:DbType.Int64, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDENIVEAU",value.Kdeniveau, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDESEM",value.Kdesem, dbType:DbType.Int64, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDESE1",value.Kdese1, dbType:DbType.Int64, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDETRI",value.Kdetri??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:18, scale: 0);
                    parameters.Add("KDENUMPRES",value.Kdenumpres, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDEAJOUT",value.Kdeajout??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDECAR",value.Kdecar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDENAT",value.Kdenat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEGAN",value.Kdegan??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEKDFID",value.Kdekdfid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDEDEFG",value.Kdedefg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDEKDHID",value.Kdekdhid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDEDATDEB",value.Kdedatdeb, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDEHEUDEB",value.Kdeheudeb, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KDEDATFIN",value.Kdedatfin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDEHEUFIN",value.Kdeheufin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KDEDUREE",value.Kdeduree, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDEDURUNI",value.Kdeduruni??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEPRP",value.Kdeprp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDETYPEMI",value.Kdetypemi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEALIREF",value.Kdealiref??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDECATNAT",value.Kdecatnat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEINA",value.Kdeina??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDETAXCOD",value.Kdetaxcod??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDETAXREP",value.Kdetaxrep, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KDECRAVN",value.Kdecravn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDECRU",value.Kdecru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDECRD",value.Kdecrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDEMAJAVN",value.Kdemajavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDEASVALO",value.Kdeasvalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDEASVALA",value.Kdeasvala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDEASVALW",value.Kdeasvalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDEASUNIT",value.Kdeasunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDEASBASE",value.Kdeasbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDEASMOD",value.Kdeasmod??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEASOBLI",value.Kdeasobli??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEINVSP",value.Kdeinvsp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEINVEN",value.Kdeinven, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDEWDDEB",value.Kdewddeb, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDEWHDEB",value.Kdewhdeb, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KDEWDFIN",value.Kdewdfin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDEWHFIN",value.Kdewhfin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KDETCD",value.Kdetcd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDEMODI",value.Kdemodi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEPIND",value.Kdepind??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEPCATN",value.Kdepcatn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEPREF",value.Kdepref??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEPPRP",value.Kdepprp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEPEMI",value.Kdepemi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEPTAXC",value.Kdeptaxc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDEPNTM",value.Kdepntm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEALA",value.Kdeala??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEPALA",value.Kdepala??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDEALO",value.Kdealo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("id",value.Kdeid);
                    parameters.Add("numeroAvenant",value.Kdeavn);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpGaran> GetByAffaire(string type, string numeroAffaire, int numeroAliment, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpGaran>(select_GetByAffaire, new {type, numeroAffaire, numeroAliment, numeroAvenant}).ToList();
            }
            public IEnumerable<KpGaran> GetByFormule(Int64 idFormule, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpGaran>(select_GetByFormule, new {idFormule, numeroAvenant}).ToList();
            }
            public IEnumerable<KpGaran> GetByOption(Int64 idOption, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpGaran>(select_GetByOption, new {idOption, numeroAvenant}).ToList();
            }
            public IEnumerable<KpGaran> GetByIpbAlx(string parKDEIPB, int parKDEALX){
                    return connection.EnsureOpened().Query<KpGaran>(select_GetByIpbAlx, new {parKDEIPB, parKDEALX}).ToList();
            }
            public IEnumerable<KpGaran> GetBySequence(string parKDEIPB, int parKDEALX, string parKDETYP, int parKDEAVN, Int64 parKDESEQ){
                    return connection.EnsureOpened().Query<KpGaran>(select_GetBySequence, new {parKDEIPB, parKDEALX, parKDETYP, parKDEAVN, parKDESEQ}).ToList();
            }
            public IEnumerable<KpGaran> GetLatestById(Int64 parKDEID){
                    return connection.EnsureOpened().Query<KpGaran>(select_GetLatestById, new {parKDEID}).ToList();
            }
    }
}
