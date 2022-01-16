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

    public  partial class  KnmRefRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KHIID, KHITYPO, KHINMC, KHIDESI, KHINORD
 FROM KNMREF
WHERE KHIID = :id
";
            const string update=@"UPDATE KNMREF SET 
KHIID = :KHIID, KHITYPO = :KHITYPO, KHINMC = :KHINMC, KHIDESI = :KHIDESI, KHINORD = :KHINORD
 WHERE 
KHIID = :id";
            const string delete=@"DELETE FROM KNMREF WHERE KHIID = :id";
            const string insert=@"INSERT INTO  KNMREF (
KHIID, KHITYPO, KHINMC, KHIDESI, KHINORD

) VALUES (
:KHIID, :KHITYPO, :KHINMC, :KHIDESI, :KHINORD
)";
            const string select_GetAll=@"SELECT
KHIID, KHITYPO, KHINMC, KHIDESI, KHINORD
 FROM KNMREF
";
            #endregion

            public KnmRefRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KnmRef Get(Int64 id){
                return connection.Query<KnmRef>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KHIID") ;
            }

            public void Insert(KnmRef value){
                    if(value.Khiid == default(Int64)) {
                        value.Khiid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KHIID",value.Khiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHITYPO",value.Khitypo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHINMC",value.Khinmc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHIDESI",value.Khidesi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KHINORD",value.Khinord, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KnmRef value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Khiid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KnmRef value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KHIID",value.Khiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHITYPO",value.Khitypo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHINMC",value.Khinmc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHIDESI",value.Khidesi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KHINORD",value.Khinord, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("id",value.Khiid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KnmRef> GetAll(){
                    return connection.EnsureOpened().Query<KnmRef>(select_GetAll).ToList();
            }
    }
}
