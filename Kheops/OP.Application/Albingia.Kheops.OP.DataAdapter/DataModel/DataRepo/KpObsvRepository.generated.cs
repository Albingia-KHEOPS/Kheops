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

    public  partial class  KpObsvRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KAJCHR, KAJTYP, KAJIPB, KAJALX, KAJTYPOBS
, KAJOBSV FROM KPOBSV
WHERE KAJCHR = :KAJCHR
";
            const string update=@"UPDATE KPOBSV SET 
KAJCHR = :KAJCHR, KAJTYP = :KAJTYP, KAJIPB = :KAJIPB, KAJALX = :KAJALX, KAJTYPOBS = :KAJTYPOBS, KAJOBSV = :KAJOBSV
 WHERE 
KAJCHR = :KAJCHR";
            const string delete=@"DELETE FROM KPOBSV WHERE KAJCHR = :KAJCHR";
            const string insert=@"INSERT INTO  KPOBSV (
KAJCHR, KAJTYP, KAJIPB, KAJALX, KAJTYPOBS
, KAJOBSV
) VALUES (
:KAJCHR, :KAJTYP, :KAJIPB, :KAJALX, :KAJTYPOBS
, :KAJOBSV)";
            const string select_GetByAffaire=@"SELECT
KAJCHR, KAJTYP, KAJIPB, KAJALX, KAJTYPOBS
, KAJOBSV FROM KPOBSV
WHERE KAJTYP = :KAJTYP
and KAJIPB = :KAJIPB
and KAJALX = :KAJALX
";
            #endregion

            public KpObsvRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpObsv Get(int KAJCHR){
                return connection.Query<KpObsv>(select, new {KAJCHR}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KAJCHR") ;
            }

            public void Insert(KpObsv value){
                    if(value.Kajchr == default(int)) {
                        value.Kajchr = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KAJCHR",value.Kajchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAJTYP",value.Kajtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAJIPB",value.Kajipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KAJALX",value.Kajalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAJTYPOBS",value.Kajtypobs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAJOBSV",value.Kajobsv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpObsv value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAJCHR",value.Kajchr);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpObsv value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAJCHR",value.Kajchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAJTYP",value.Kajtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAJIPB",value.Kajipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KAJALX",value.Kajalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAJTYPOBS",value.Kajtypobs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAJOBSV",value.Kajobsv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("KAJCHR",value.Kajchr);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpObsv> GetByAffaire(string KAJTYP, string KAJIPB, int KAJALX){
                    return connection.EnsureOpened().Query<KpObsv>(select_GetByAffaire, new {KAJTYP, KAJIPB, KAJALX}).ToList();
            }
    }
}
