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

    public  partial class  KISModlRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKSPP
            const string select=@"SELECT
KGDID, KGDMODID, KGDNMID, KGDLIB, KGDNUMAFF
, KGDSAUTL, KGDMODI, KGDOBLI, KGDSQL, KGDSQLORD
, KGDSCRAFFS, KGDSCRCTRS, KGDPARENID, KGDPARENC, KGDPARENE
, KGDCRU, KGDCRD, KGDMJU, KGDMJD, KGDPRES
, KGDSAID2, KGDSCID2, KGDNREF, KGDVUCON, KGDVUFAM
 FROM KISMODL
WHERE KGDID = :KGDID
";
            const string update=@"UPDATE KISMODL SET 
KGDID = :KGDID, KGDMODID = :KGDMODID, KGDNMID = :KGDNMID, KGDLIB = :KGDLIB, KGDNUMAFF = :KGDNUMAFF, KGDSAUTL = :KGDSAUTL, KGDMODI = :KGDMODI, KGDOBLI = :KGDOBLI, KGDSQL = :KGDSQL, KGDSQLORD = :KGDSQLORD
, KGDSCRAFFS = :KGDSCRAFFS, KGDSCRCTRS = :KGDSCRCTRS, KGDPARENID = :KGDPARENID, KGDPARENC = :KGDPARENC, KGDPARENE = :KGDPARENE, KGDCRU = :KGDCRU, KGDCRD = :KGDCRD, KGDMJU = :KGDMJU, KGDMJD = :KGDMJD, KGDPRES = :KGDPRES
, KGDSAID2 = :KGDSAID2, KGDSCID2 = :KGDSCID2, KGDNREF = :KGDNREF, KGDVUCON = :KGDVUCON, KGDVUFAM = :KGDVUFAM
 WHERE 
KGDID = :KGDID";
            const string delete=@"DELETE FROM KISMODL WHERE KGDID = :KGDID";
            const string insert=@"INSERT INTO  KISMODL (
KGDID, KGDMODID, KGDNMID, KGDLIB, KGDNUMAFF
, KGDSAUTL, KGDMODI, KGDOBLI, KGDSQL, KGDSQLORD
, KGDSCRAFFS, KGDSCRCTRS, KGDPARENID, KGDPARENC, KGDPARENE
, KGDCRU, KGDCRD, KGDMJU, KGDMJD, KGDPRES
, KGDSAID2, KGDSCID2, KGDNREF, KGDVUCON, KGDVUFAM

) VALUES (
:KGDID, :KGDMODID, :KGDNMID, :KGDLIB, :KGDNUMAFF
, :KGDSAUTL, :KGDMODI, :KGDOBLI, :KGDSQL, :KGDSQLORD
, :KGDSCRAFFS, :KGDSCRCTRS, :KGDPARENID, :KGDPARENC, :KGDPARENE
, :KGDCRU, :KGDCRD, :KGDMJU, :KGDMJD, :KGDPRES
, :KGDSAID2, :KGDSCID2, :KGDNREF, :KGDVUCON, :KGDVUFAM
)";
            const string select_SelectByModel=@"SELECT
KGDID, KGDMODID, KGDNMID, KGDLIB, KGDNUMAFF
, KGDSAUTL, KGDMODI, KGDOBLI, KGDSQL, KGDSQLORD
, KGDSCRAFFS, KGDSCRCTRS, KGDPARENID, KGDPARENC, KGDPARENE
, KGDCRU, KGDCRD, KGDMJU, KGDMJD, KGDPRES
, KGDSAID2, KGDSCID2, KGDNREF, KGDVUCON, KGDVUFAM
 FROM KISMODL
WHERE KGDMODID = :parKGDMODID
";

        #endregion

        public KISModlRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KISModl Get(int KGDID){
                return connection.Query<KISModl>(select, new {KGDID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KGDID") ;
            }

            public void Insert(KISModl value){
                    if(value.Kgdid == default(int)) {
                        value.Kgdid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KGDID",value.Kgdid, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGDMODID",value.Kgdmodid??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KGDNMID",value.Kgdnmid??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KGDLIB",value.Kgdlib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:80, scale: 0);
                    parameters.Add("KGDNUMAFF",value.Kgdnumaff, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KGDSAUTL",value.Kgdsautl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGDMODI",value.Kgdmodi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGDOBLI",value.Kgdobli??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGDSQL",value.Kgdsql??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("KGDSQLORD",value.Kgdsqlord, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGDSCRAFFS",value.Kgdscraffs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGDSCRCTRS",value.Kgdscrctrs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGDPARENID",value.Kgdparenid, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGDPARENC",value.Kgdparenc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("KGDPARENE",value.Kgdparene??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("KGDCRU",value.Kgdcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGDCRD",value.Kgdcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGDMJU",value.Kgdmju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGDMJD",value.Kgdmjd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGDPRES",value.Kgdpres, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGDSAID2",value.Kgdsaid2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KGDSCID2",value.Kgdscid2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KGDNREF",value.Kgdnref, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGDVUCON",value.Kgdvucon??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KGDVUFAM",value.Kgdvufam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KISModl value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KGDID",value.Kgdid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KISModl value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KGDID",value.Kgdid, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGDMODID",value.Kgdmodid??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KGDNMID",value.Kgdnmid??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KGDLIB",value.Kgdlib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:80, scale: 0);
                    parameters.Add("KGDNUMAFF",value.Kgdnumaff, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KGDSAUTL",value.Kgdsautl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGDMODI",value.Kgdmodi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGDOBLI",value.Kgdobli??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGDSQL",value.Kgdsql??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("KGDSQLORD",value.Kgdsqlord, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGDSCRAFFS",value.Kgdscraffs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGDSCRCTRS",value.Kgdscrctrs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGDPARENID",value.Kgdparenid, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGDPARENC",value.Kgdparenc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("KGDPARENE",value.Kgdparene??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("KGDCRU",value.Kgdcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGDCRD",value.Kgdcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGDMJU",value.Kgdmju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGDMJD",value.Kgdmjd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGDPRES",value.Kgdpres, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGDSAID2",value.Kgdsaid2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KGDSCID2",value.Kgdscid2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KGDNREF",value.Kgdnref, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGDVUCON",value.Kgdvucon??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KGDVUFAM",value.Kgdvufam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KGDID",value.Kgdid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KISModl> SelectByModel(string parKGDMODID){
                    return connection.EnsureOpened().Query<KISModl>(select_SelectByModel, new {parKGDMODID}).ToList();
            }
    }
}
