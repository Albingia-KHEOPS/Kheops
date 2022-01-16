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

    public  partial class  YcourtiRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
TCICT, TCTYP, TCICI, TCAD1, TCAD2
, TCDEP, TCCPO, TCVIL, TCPAY, TCCOM
, TCREG, TCBUR, TCFDC, TCTEL, TCTLC
, TCBQE, TCGUI, TCCPT, TCRIB, TCICP
, TCORI, TCTIN, TCCII, TCAPG, TCLIG
, TCRGC, TCENC, TCMAN, TCDCP, TCRAF
, TCCHA, TCGEP, TCPRD, TCCRA, TCCRM
, TCCRJ, TCFVA, TCFVM, TCFVJ, TCRPL
, TCUSR, TCMJA, TCMJM, TCMJJ, TCOLD
, TCAPE, TCINS, TCSPE, TCYEN, TCAEM
, TCIOO, TCIOC, TCIOJ, TCIOM, TCIOA
, TCIAG, TCIAJ, TCIAM, TCIAA, TCIII
, TCIIJ, TCIIM, TCIIA, TCIMD, TCIMJ
, TCIMM, TCIMA, TCADH, TCRCV, TCRCS
, TCINM, TCSEC, TCIRN, TCIRJ, TCIRM
, TCIRA, TCRCN, TCAP5, TCEDI, TCIDN
, TCIST, TCGEM FROM YCOURTI
WHERE TCICT = :id
";
            const string update=@"UPDATE YCOURTI SET 
TCICT = :TCICT, TCTYP = :TCTYP, TCICI = :TCICI, TCAD1 = :TCAD1, TCAD2 = :TCAD2, TCDEP = :TCDEP, TCCPO = :TCCPO, TCVIL = :TCVIL, TCPAY = :TCPAY, TCCOM = :TCCOM
, TCREG = :TCREG, TCBUR = :TCBUR, TCFDC = :TCFDC, TCTEL = :TCTEL, TCTLC = :TCTLC, TCBQE = :TCBQE, TCGUI = :TCGUI, TCCPT = :TCCPT, TCRIB = :TCRIB, TCICP = :TCICP
, TCORI = :TCORI, TCTIN = :TCTIN, TCCII = :TCCII, TCAPG = :TCAPG, TCLIG = :TCLIG, TCRGC = :TCRGC, TCENC = :TCENC, TCMAN = :TCMAN, TCDCP = :TCDCP, TCRAF = :TCRAF
, TCCHA = :TCCHA, TCGEP = :TCGEP, TCPRD = :TCPRD, TCCRA = :TCCRA, TCCRM = :TCCRM, TCCRJ = :TCCRJ, TCFVA = :TCFVA, TCFVM = :TCFVM, TCFVJ = :TCFVJ, TCRPL = :TCRPL
, TCUSR = :TCUSR, TCMJA = :TCMJA, TCMJM = :TCMJM, TCMJJ = :TCMJJ, TCOLD = :TCOLD, TCAPE = :TCAPE, TCINS = :TCINS, TCSPE = :TCSPE, TCYEN = :TCYEN, TCAEM = :TCAEM
, TCIOO = :TCIOO, TCIOC = :TCIOC, TCIOJ = :TCIOJ, TCIOM = :TCIOM, TCIOA = :TCIOA, TCIAG = :TCIAG, TCIAJ = :TCIAJ, TCIAM = :TCIAM, TCIAA = :TCIAA, TCIII = :TCIII
, TCIIJ = :TCIIJ, TCIIM = :TCIIM, TCIIA = :TCIIA, TCIMD = :TCIMD, TCIMJ = :TCIMJ, TCIMM = :TCIMM, TCIMA = :TCIMA, TCADH = :TCADH, TCRCV = :TCRCV, TCRCS = :TCRCS
, TCINM = :TCINM, TCSEC = :TCSEC, TCIRN = :TCIRN, TCIRJ = :TCIRJ, TCIRM = :TCIRM, TCIRA = :TCIRA, TCRCN = :TCRCN, TCAP5 = :TCAP5, TCEDI = :TCEDI, TCIDN = :TCIDN
, TCIST = :TCIST, TCGEM = :TCGEM
 WHERE 
TCICT = :id";
            const string delete=@"DELETE FROM YCOURTI WHERE TCICT = :id";
            const string insert=@"INSERT INTO  YCOURTI (
TCICT, TCTYP, TCICI, TCAD1, TCAD2
, TCDEP, TCCPO, TCVIL, TCPAY, TCCOM
, TCREG, TCBUR, TCFDC, TCTEL, TCTLC
, TCBQE, TCGUI, TCCPT, TCRIB, TCICP
, TCORI, TCTIN, TCCII, TCAPG, TCLIG
, TCRGC, TCENC, TCMAN, TCDCP, TCRAF
, TCCHA, TCGEP, TCPRD, TCCRA, TCCRM
, TCCRJ, TCFVA, TCFVM, TCFVJ, TCRPL
, TCUSR, TCMJA, TCMJM, TCMJJ, TCOLD
, TCAPE, TCINS, TCSPE, TCYEN, TCAEM
, TCIOO, TCIOC, TCIOJ, TCIOM, TCIOA
, TCIAG, TCIAJ, TCIAM, TCIAA, TCIII
, TCIIJ, TCIIM, TCIIA, TCIMD, TCIMJ
, TCIMM, TCIMA, TCADH, TCRCV, TCRCS
, TCINM, TCSEC, TCIRN, TCIRJ, TCIRM
, TCIRA, TCRCN, TCAP5, TCEDI, TCIDN
, TCIST, TCGEM
) VALUES (
:TCICT, :TCTYP, :TCICI, :TCAD1, :TCAD2
, :TCDEP, :TCCPO, :TCVIL, :TCPAY, :TCCOM
, :TCREG, :TCBUR, :TCFDC, :TCTEL, :TCTLC
, :TCBQE, :TCGUI, :TCCPT, :TCRIB, :TCICP
, :TCORI, :TCTIN, :TCCII, :TCAPG, :TCLIG
, :TCRGC, :TCENC, :TCMAN, :TCDCP, :TCRAF
, :TCCHA, :TCGEP, :TCPRD, :TCCRA, :TCCRM
, :TCCRJ, :TCFVA, :TCFVM, :TCFVJ, :TCRPL
, :TCUSR, :TCMJA, :TCMJM, :TCMJJ, :TCOLD
, :TCAPE, :TCINS, :TCSPE, :TCYEN, :TCAEM
, :TCIOO, :TCIOC, :TCIOJ, :TCIOM, :TCIOA
, :TCIAG, :TCIAJ, :TCIAM, :TCIAA, :TCIII
, :TCIIJ, :TCIIM, :TCIIA, :TCIMD, :TCIMJ
, :TCIMM, :TCIMA, :TCADH, :TCRCV, :TCRCS
, :TCINM, :TCSEC, :TCIRN, :TCIRJ, :TCIRM
, :TCIRA, :TCRCN, :TCAP5, :TCEDI, :TCIDN
, :TCIST, :TCGEM)";
            const string select_FullCourtier=@"SELECT
TCICT AS TCICT, TCTYP AS TCTYP, TCICI AS TCICI, TCAD1 AS TCAD1, TCAD2 AS TCAD2
, TCDEP AS TCDEP, TCCPO AS TCCPO, TCVIL AS TCVIL, TCPAY AS TCPAY, TCCOM AS TCCOM
, TCREG AS TCREG, TCBUR AS TCBUR, TCFDC AS TCFDC, TCTEL AS TCTEL, TCTLC AS TCTLC
, TCBQE AS TCBQE, TCGUI AS TCGUI, TCCPT AS TCCPT, TCRIB AS TCRIB, TCICP AS TCICP
, TCORI AS TCORI, TCTIN AS TCTIN, TCCII AS TCCII, TCAPG AS TCAPG, TCLIG AS TCLIG
, TCRGC AS TCRGC, TCENC AS TCENC, TCMAN AS TCMAN, TCDCP AS TCDCP, TCRAF AS TCRAF
, TCCHA AS TCCHA, TCGEP AS TCGEP, TCPRD AS TCPRD, TCCRA AS TCCRA, TCCRM AS TCCRM
, TCCRJ AS TCCRJ, TCFVA AS TCFVA, TCFVM AS TCFVM, TCFVJ AS TCFVJ, TCRPL AS TCRPL
, TCUSR AS TCUSR, TCMJA AS TCMJA, TCMJM AS TCMJM, TCMJJ AS TCMJJ, TCOLD AS TCOLD
, TCAPE AS TCAPE, TCINS AS TCINS, TCSPE AS TCSPE, TCYEN AS TCYEN, TCAEM AS TCAEM
, TCIOO AS TCIOO, TCIOC AS TCIOC, TCIOJ AS TCIOJ, TCIOM AS TCIOM, TCIOA AS TCIOA
, TCIAG AS TCIAG, TCIAJ AS TCIAJ, TCIAM AS TCIAM, TCIAA AS TCIAA, TCIII AS TCIII
, TCIIJ AS TCIIJ, TCIIM AS TCIIM, TCIIA AS TCIIA, TCIMD AS TCIMD, TCIMJ AS TCIMJ
, TCIMM AS TCIMM, TCIMA AS TCIMA, TCADH AS TCADH, TCRCV AS TCRCV, TCRCS AS TCRCS
, TCINM AS TCINM, TCSEC AS TCSEC, TCIRN AS TCIRN, TCIRJ AS TCIRJ, TCIRM AS TCIRM
, TCIRA AS TCIRA, TCRCN AS TCRCN, TCAP5 AS TCAP5, TCEDI AS TCEDI, TCIDN AS TCIDN
, TCIST AS TCIST, TCGEM AS TCGEM, TNICT AS TNICT, TNINL AS TNINL, TNTNM AS TNTNM
, TNORD AS TNORD, TNTYP AS TNTYP, TNNOM AS TNNOM, TNTIT AS TNTIT, TNXN5 AS TNXN5
, ABPCP6 AS ABPCP6, ABPDP6 AS ABPDP6, ABPVI6 AS ABPVI6, ABPPAY AS ABPPAY, ABHSEC AS ABHSEC
, ABHDES AS ABHDES, ABHLIB AS ABHLIB, ABHORD AS ABHORD, ABHINS AS ABHINS, ACLINS AS ACLINS
, ACLDES AS ACLDES, ACLLIB AS ACLLIB, ACLORD AS ACLORD, ACLUIN AS ACLUIN FROM YCOURTI
LEFT JOIN YCOURTN N ON TCICT =  N.TNICT  AND  N .TNTNM ='A' AND N.TNXN5 = 0
inner join YADRESS ON TCADH = ABPCHR
LEFT JOIN YSECTEU ON ABHSEC=TCSEC
LEFT JOIN YINSPEC ON ACLINS = ABHINS
WHERE TNICT = :id
and TNINL = :code
";
            #endregion

            public YcourtiRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public Ycourti Get(int id){
                return connection.Query<Ycourti>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("TCICT") ;
            }

            public void Insert(Ycourti value){
                    if(value.Tcict == default(int)) {
                        value.Tcict = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("TCICT",value.Tcict, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCTYP",value.Tctyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCICI",value.Tcici??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("TCAD1",value.Tcad1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("TCAD2",value.Tcad2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("TCDEP",value.Tcdep??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCCPO",value.Tccpo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("TCVIL",value.Tcvil??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:26, scale: 0);
                    parameters.Add("TCPAY",value.Tcpay??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("TCCOM",value.Tccom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCREG",value.Tcreg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCBUR",value.Tcbur??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCFDC",value.Tcfdc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCTEL",value.Tctel??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("TCTLC",value.Tctlc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("TCBQE",value.Tcbqe, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCGUI",value.Tcgui, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCCPT",value.Tccpt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("TCRIB",value.Tcrib, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCICP",value.Tcicp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("TCORI",value.Tcori??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCTIN",value.Tctin??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCCII",value.Tccii??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("TCAPG",value.Tcapg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCLIG",value.Tclig??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCRGC",value.Tcrgc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCENC",value.Tcenc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCMAN",value.Tcman??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCDCP",value.Tcdcp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCRAF",value.Tcraf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCCHA",value.Tccha??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("TCGEP",value.Tcgep??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCPRD",value.Tcprd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("TCCRA",value.Tccra, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("TCCRM",value.Tccrm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCCRJ",value.Tccrj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCFVA",value.Tcfva, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("TCFVM",value.Tcfvm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCFVJ",value.Tcfvj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCRPL",value.Tcrpl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCUSR",value.Tcusr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("TCMJA",value.Tcmja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("TCMJM",value.Tcmjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCMJJ",value.Tcmjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCOLD",value.Tcold??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("TCAPE",value.Tcape??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("TCINS",value.Tcins??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("TCSPE",value.Tcspe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("TCYEN",value.Tcyen??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCAEM",value.Tcaem??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("TCIOO",value.Tcioo, dbType:DbType.Int64, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("TCIOC",value.Tcioc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCIOJ",value.Tcioj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIOM",value.Tciom, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIOA",value.Tcioa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("TCIAG",value.Tciag??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCIAJ",value.Tciaj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIAM",value.Tciam, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIAA",value.Tciaa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("TCIII",value.Tciii??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCIIJ",value.Tciij, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIIM",value.Tciim, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIIA",value.Tciia, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("TCIMD",value.Tcimd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCIMJ",value.Tcimj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIMM",value.Tcimm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIMA",value.Tcima, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("TCADH",value.Tcadh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("TCRCV",value.Tcrcv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("TCRCS",value.Tcrcs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("TCINM",value.Tcinm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCSEC",value.Tcsec??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("TCIRN",value.Tcirn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCIRJ",value.Tcirj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIRM",value.Tcirm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIRA",value.Tcira, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("TCRCN",value.Tcrcn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCAP5",value.Tcap5??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCEDI",value.Tcedi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("TCIDN",value.Tcidn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("TCIST",value.Tcist??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("TCGEM",value.Tcgem??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(Ycourti value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Tcict);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(Ycourti value){
                    var parameters = new DynamicParameters();
                    parameters.Add("TCICT",value.Tcict, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCTYP",value.Tctyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCICI",value.Tcici??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("TCAD1",value.Tcad1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("TCAD2",value.Tcad2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("TCDEP",value.Tcdep??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCCPO",value.Tccpo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("TCVIL",value.Tcvil??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:26, scale: 0);
                    parameters.Add("TCPAY",value.Tcpay??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("TCCOM",value.Tccom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCREG",value.Tcreg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCBUR",value.Tcbur??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCFDC",value.Tcfdc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCTEL",value.Tctel??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("TCTLC",value.Tctlc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("TCBQE",value.Tcbqe, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCGUI",value.Tcgui, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCCPT",value.Tccpt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("TCRIB",value.Tcrib, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCICP",value.Tcicp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("TCORI",value.Tcori??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCTIN",value.Tctin??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCCII",value.Tccii??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("TCAPG",value.Tcapg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCLIG",value.Tclig??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCRGC",value.Tcrgc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCENC",value.Tcenc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCMAN",value.Tcman??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCDCP",value.Tcdcp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCRAF",value.Tcraf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCCHA",value.Tccha??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("TCGEP",value.Tcgep??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCPRD",value.Tcprd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("TCCRA",value.Tccra, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("TCCRM",value.Tccrm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCCRJ",value.Tccrj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCFVA",value.Tcfva, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("TCFVM",value.Tcfvm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCFVJ",value.Tcfvj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCRPL",value.Tcrpl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCUSR",value.Tcusr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("TCMJA",value.Tcmja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("TCMJM",value.Tcmjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCMJJ",value.Tcmjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCOLD",value.Tcold??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("TCAPE",value.Tcape??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("TCINS",value.Tcins??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("TCSPE",value.Tcspe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("TCYEN",value.Tcyen??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCAEM",value.Tcaem??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("TCIOO",value.Tcioo, dbType:DbType.Int64, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("TCIOC",value.Tcioc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCIOJ",value.Tcioj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIOM",value.Tciom, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIOA",value.Tcioa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("TCIAG",value.Tciag??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCIAJ",value.Tciaj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIAM",value.Tciam, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIAA",value.Tciaa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("TCIII",value.Tciii??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCIIJ",value.Tciij, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIIM",value.Tciim, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIIA",value.Tciia, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("TCIMD",value.Tcimd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCIMJ",value.Tcimj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIMM",value.Tcimm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIMA",value.Tcima, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("TCADH",value.Tcadh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("TCRCV",value.Tcrcv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("TCRCS",value.Tcrcs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("TCINM",value.Tcinm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCSEC",value.Tcsec??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("TCIRN",value.Tcirn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TCIRJ",value.Tcirj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIRM",value.Tcirm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TCIRA",value.Tcira, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("TCRCN",value.Tcrcn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCAP5",value.Tcap5??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCEDI",value.Tcedi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("TCIDN",value.Tcidn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("TCIST",value.Tcist??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("TCGEM",value.Tcgem??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("id",value.Tcict);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<Courtier> FullCourtier(int id, int code){
                    return connection.EnsureOpened().Query<Courtier>(select_FullCourtier, new {id, code}).ToList();
            }
    }
}
