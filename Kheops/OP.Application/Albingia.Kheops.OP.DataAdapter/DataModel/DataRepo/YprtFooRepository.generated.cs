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

    public  partial class  YprtFooRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
JPIPB, JPALX, JPRSQ, JPFOR, JPOBJ
 FROM YPRTFOO
WHERE JPIPB = :JPIPB
and JPALX = :JPALX
and JPRSQ = :JPRSQ
and JPOBJ = :JPOBJ
";
            const string update=@"UPDATE YPRTFOO SET 
JPIPB = :JPIPB, JPALX = :JPALX, JPRSQ = :JPRSQ, JPFOR = :JPFOR, JPOBJ = :JPOBJ
 WHERE 
JPIPB = :JPIPB and JPALX = :JPALX and JPRSQ = :JPRSQ and JPOBJ = :JPOBJ";
            const string delete=@"DELETE FROM YPRTFOO WHERE JPIPB = :JPIPB AND JPALX = :JPALX AND JPRSQ = :JPRSQ AND JPOBJ = :JPOBJ";
            const string insert=@"INSERT INTO  YPRTFOO (
JPIPB, JPALX, JPRSQ, JPFOR, JPOBJ

) VALUES (
:JPIPB, :JPALX, :JPRSQ, :JPFOR, :JPOBJ
)";
            const string select_GetByAffaire=@"SELECT
JPIPB, JPALX, JPRSQ, JPFOR, JPOBJ
 FROM YPRTFOO
WHERE JPIPB = :JPIPB
and JPALX = :JPALX
";
            #endregion

            public YprtFooRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YprtFoo Get(string JPIPB, int JPALX, int JPRSQ, int JPOBJ){
                return connection.Query<YprtFoo>(select, new {JPIPB, JPALX, JPRSQ, JPOBJ}).SingleOrDefault();
            }


            public void Insert(YprtFoo value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JPIPB",value.Jpipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JPALX",value.Jpalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JPRSQ",value.Jprsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JPFOR",value.Jpfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JPOBJ",value.Jpobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YprtFoo value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JPIPB",value.Jpipb);
                    parameters.Add("JPALX",value.Jpalx);
                    parameters.Add("JPRSQ",value.Jprsq);
                    parameters.Add("JPOBJ",value.Jpobj);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YprtFoo value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JPIPB",value.Jpipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JPALX",value.Jpalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JPRSQ",value.Jprsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JPFOR",value.Jpfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JPOBJ",value.Jpobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JPIPB",value.Jpipb);
                    parameters.Add("JPALX",value.Jpalx);
                    parameters.Add("JPRSQ",value.Jprsq);
                    parameters.Add("JPOBJ",value.Jpobj);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YprtFoo> GetByAffaire(string JPIPB, int JPALX){
                    return connection.EnsureOpened().Query<YprtFoo>(select_GetByAffaire, new {JPIPB, JPALX}).ToList();
            }
    }
}
