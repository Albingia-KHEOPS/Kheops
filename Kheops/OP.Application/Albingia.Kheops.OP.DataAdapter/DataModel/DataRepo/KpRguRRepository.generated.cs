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

    public  partial class  KpRguRRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KILID, KILKHWID, KILTYP, KILIPB, KILALX
, KILRSQ, KILDEBP, KILFINP, KILSIT, KILMHC
, KILFRC, KILFR0, KILMHT, KILMTX, KILTTT
, KILMTT, KILCNH, KILCNT, KILGRM, KILPRO
, KILECH, KILECT, KILKEA, KILKTD, KILASV
, KILPBT, KILMIN, KILBRNC, KILPBA, KILPBS
, KILPBR, KILPBP, KILRIA, KILSPRE, KILRIAF
, KILEMH, KILEMT, KILEMHF, KILEMTF, KILCOT
, KILBRNT, KILSCHG, KILSIDF, KILSREC, KILSPRO
, KILSREP, KILSRIM, KILPBTR, KILEMD, KILSPC
 FROM KPRGUR
WHERE KILID = :KILID
";
            const string update=@"UPDATE KPRGUR SET 
KILID = :KILID, KILKHWID = :KILKHWID, KILTYP = :KILTYP, KILIPB = :KILIPB, KILALX = :KILALX, KILRSQ = :KILRSQ, KILDEBP = :KILDEBP, KILFINP = :KILFINP, KILSIT = :KILSIT, KILMHC = :KILMHC
, KILFRC = :KILFRC, KILFR0 = :KILFR0, KILMHT = :KILMHT, KILMTX = :KILMTX, KILTTT = :KILTTT, KILMTT = :KILMTT, KILCNH = :KILCNH, KILCNT = :KILCNT, KILGRM = :KILGRM, KILPRO = :KILPRO
, KILECH = :KILECH, KILECT = :KILECT, KILKEA = :KILKEA, KILKTD = :KILKTD, KILASV = :KILASV, KILPBT = :KILPBT, KILMIN = :KILMIN, KILBRNC = :KILBRNC, KILPBA = :KILPBA, KILPBS = :KILPBS
, KILPBR = :KILPBR, KILPBP = :KILPBP, KILRIA = :KILRIA, KILSPRE = :KILSPRE, KILRIAF = :KILRIAF, KILEMH = :KILEMH, KILEMT = :KILEMT, KILEMHF = :KILEMHF, KILEMTF = :KILEMTF, KILCOT = :KILCOT
, KILBRNT = :KILBRNT, KILSCHG = :KILSCHG, KILSIDF = :KILSIDF, KILSREC = :KILSREC, KILSPRO = :KILSPRO, KILSREP = :KILSREP, KILSRIM = :KILSRIM, KILPBTR = :KILPBTR, KILEMD = :KILEMD, KILSPC = :KILSPC

 WHERE 
KILID = :KILID";
            const string delete=@"DELETE FROM KPRGUR WHERE KILID = :KILID";
            const string insert=@"INSERT INTO  KPRGUR (
KILID, KILKHWID, KILTYP, KILIPB, KILALX
, KILRSQ, KILDEBP, KILFINP, KILSIT, KILMHC
, KILFRC, KILFR0, KILMHT, KILMTX, KILTTT
, KILMTT, KILCNH, KILCNT, KILGRM, KILPRO
, KILECH, KILECT, KILKEA, KILKTD, KILASV
, KILPBT, KILMIN, KILBRNC, KILPBA, KILPBS
, KILPBR, KILPBP, KILRIA, KILSPRE, KILRIAF
, KILEMH, KILEMT, KILEMHF, KILEMTF, KILCOT
, KILBRNT, KILSCHG, KILSIDF, KILSREC, KILSPRO
, KILSREP, KILSRIM, KILPBTR, KILEMD, KILSPC

) VALUES (
:KILID, :KILKHWID, :KILTYP, :KILIPB, :KILALX
, :KILRSQ, :KILDEBP, :KILFINP, :KILSIT, :KILMHC
, :KILFRC, :KILFR0, :KILMHT, :KILMTX, :KILTTT
, :KILMTT, :KILCNH, :KILCNT, :KILGRM, :KILPRO
, :KILECH, :KILECT, :KILKEA, :KILKTD, :KILASV
, :KILPBT, :KILMIN, :KILBRNC, :KILPBA, :KILPBS
, :KILPBR, :KILPBP, :KILRIA, :KILSPRE, :KILRIAF
, :KILEMH, :KILEMT, :KILEMHF, :KILEMTF, :KILCOT
, :KILBRNT, :KILSCHG, :KILSIDF, :KILSREC, :KILSPRO
, :KILSREP, :KILSRIM, :KILPBTR, :KILEMD, :KILSPC
)";
            const string select_GetByAffaire=@"SELECT
KILID, KILKHWID, KILTYP, KILIPB, KILALX
, KILRSQ, KILDEBP, KILFINP, KILSIT, KILMHC
, KILFRC, KILFR0, KILMHT, KILMTX, KILTTT
, KILMTT, KILCNH, KILCNT, KILGRM, KILPRO
, KILECH, KILECT, KILKEA, KILKTD, KILASV
, KILPBT, KILMIN, KILBRNC, KILPBA, KILPBS
, KILPBR, KILPBP, KILRIA, KILSPRE, KILRIAF
, KILEMH, KILEMT, KILEMHF, KILEMTF, KILCOT
, KILBRNT, KILSCHG, KILSIDF, KILSREC, KILSPRO
, KILSREP, KILSRIM, KILPBTR, KILEMD, KILSPC
 FROM KPRGUR
WHERE KILTYP = :KILTYP
and KILIPB = :KILIPB
and KILALX = :KILALX
";
            const string select_GetByRegul=@"SELECT
KILID, KILKHWID, KILTYP, KILIPB, KILALX
, KILRSQ, KILDEBP, KILFINP, KILSIT, KILMHC
, KILFRC, KILFR0, KILMHT, KILMTX, KILTTT
, KILMTT, KILCNH, KILCNT, KILGRM, KILPRO
, KILECH, KILECT, KILKEA, KILKTD, KILASV
, KILPBT, KILMIN, KILBRNC, KILPBA, KILPBS
, KILPBR, KILPBP, KILRIA, KILSPRE, KILRIAF
, KILEMH, KILEMT, KILEMHF, KILEMTF, KILCOT
, KILBRNT, KILSCHG, KILSIDF, KILSREC, KILSPRO
, KILSREP, KILSRIM, KILPBTR, KILEMD, KILSPC
 FROM KPRGUR
WHERE KILKHWID = :parKILKHWID
";
            #endregion

            public KpRguRRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpRguR Get(Int64 KILID){
                return connection.Query<KpRguR>(select, new {KILID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KILID") ;
            }

            public void Insert(KpRguR value){
                    if(value.Kilid == default(Int64)) {
                        value.Kilid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KILID",value.Kilid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KILKHWID",value.Kilkhwid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KILTYP",value.Kiltyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KILIPB",value.Kilipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KILALX",value.Kilalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KILRSQ",value.Kilrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KILDEBP",value.Kildebp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KILFINP",value.Kilfinp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KILSIT",value.Kilsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KILMHC",value.Kilmhc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILFRC",value.Kilfrc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KILFR0",value.Kilfr0??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KILMHT",value.Kilmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILMTX",value.Kilmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILTTT",value.Kilttt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILMTT",value.Kilmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILCNH",value.Kilcnh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILCNT",value.Kilcnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILGRM",value.Kilgrm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILPRO",value.Kilpro, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILECH",value.Kilech, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILECT",value.Kilect, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILKEA",value.Kilkea, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILKTD",value.Kilktd, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILASV",value.Kilasv, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILPBT",value.Kilpbt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("KILMIN",value.Kilmin, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KILBRNC",value.Kilbrnc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILPBA",value.Kilpba, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KILPBS",value.Kilpbs, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KILPBR",value.Kilpbr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KILPBP",value.Kilpbp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KILRIA",value.Kilria, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KILSPRE",value.Kilspre, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KILRIAF",value.Kilriaf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KILEMH",value.Kilemh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILEMT",value.Kilemt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILEMHF",value.Kilemhf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILEMTF",value.Kilemtf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILCOT",value.Kilcot, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILBRNT",value.Kilbrnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 4);
                    parameters.Add("KILSCHG",value.Kilschg, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KILSIDF",value.Kilsidf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KILSREC",value.Kilsrec, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KILSPRO",value.Kilspro, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KILSREP",value.Kilsrep, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KILSRIM",value.Kilsrim, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KILPBTR",value.Kilpbtr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KILEMD",value.Kilemd, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILSPC",value.Kilspc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpRguR value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KILID",value.Kilid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpRguR value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KILID",value.Kilid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KILKHWID",value.Kilkhwid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KILTYP",value.Kiltyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KILIPB",value.Kilipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KILALX",value.Kilalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KILRSQ",value.Kilrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KILDEBP",value.Kildebp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KILFINP",value.Kilfinp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KILSIT",value.Kilsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KILMHC",value.Kilmhc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILFRC",value.Kilfrc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KILFR0",value.Kilfr0??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KILMHT",value.Kilmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILMTX",value.Kilmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILTTT",value.Kilttt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILMTT",value.Kilmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILCNH",value.Kilcnh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILCNT",value.Kilcnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILGRM",value.Kilgrm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILPRO",value.Kilpro, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILECH",value.Kilech, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILECT",value.Kilect, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILKEA",value.Kilkea, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILKTD",value.Kilktd, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILASV",value.Kilasv, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILPBT",value.Kilpbt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("KILMIN",value.Kilmin, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KILBRNC",value.Kilbrnc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILPBA",value.Kilpba, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KILPBS",value.Kilpbs, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KILPBR",value.Kilpbr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KILPBP",value.Kilpbp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KILRIA",value.Kilria, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KILSPRE",value.Kilspre, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KILRIAF",value.Kilriaf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KILEMH",value.Kilemh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILEMT",value.Kilemt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILEMHF",value.Kilemhf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILEMTF",value.Kilemtf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILCOT",value.Kilcot, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILBRNT",value.Kilbrnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 4);
                    parameters.Add("KILSCHG",value.Kilschg, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KILSIDF",value.Kilsidf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KILSREC",value.Kilsrec, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KILSPRO",value.Kilspro, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KILSREP",value.Kilsrep, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KILSRIM",value.Kilsrim, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KILPBTR",value.Kilpbtr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KILEMD",value.Kilemd, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KILSPC",value.Kilspc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KILID",value.Kilid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpRguR> GetByAffaire(string KILTYP, string KILIPB, int KILALX){
                    return connection.EnsureOpened().Query<KpRguR>(select_GetByAffaire, new {KILTYP, KILIPB, KILALX}).ToList();
            }
            public IEnumerable<KpRguR> GetByRegul(Int64 parKILKHWID){
                    return connection.EnsureOpened().Query<KpRguR>(select_GetByRegul, new {parKILKHWID}).ToList();
            }
    }
}
