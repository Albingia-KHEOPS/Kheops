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

    public  partial class  KDesiRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDWID AS KDWID FROM KDESI
";
            const string update=@"UPDATE KDESI SET 
KDWID = :KDWID
 WHERE 
";
            const string delete=@"DELETE FROM KDESI WHERE ";
            const string insert=@"INSERT INTO  KDESI (
KDWID
) VALUES (
:KDWID)";
            const string select_GetAll=@"SELECT
KDWID, KDWDESI FROM KDESI
";
            #endregion

            public KDesiRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KDesi Get(){
                return connection.Query<KDesi>(select, new {}).SingleOrDefault();
            }


            public void Insert(KDesi value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDWID",value.Kdwid, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDWDESI",value.Kdwdesi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KDesi value){
                    var parameters = new DynamicParameters();
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KDesi value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDWID",value.Kdwid, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDWDESI",value.Kdwdesi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KDesi> GetAll(){
                    return connection.EnsureOpened().Query<KDesi>(select_GetAll).ToList();
            }
    }
}
