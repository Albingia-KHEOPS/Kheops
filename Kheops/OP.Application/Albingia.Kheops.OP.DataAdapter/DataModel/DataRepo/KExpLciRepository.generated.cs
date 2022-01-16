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

    public  partial class  KExpLciRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KHGID, KHGLCE, KHGDESC, KHGDESI, KHGMODI
 FROM KEXPLCI
WHERE KHGID = :id
";
            const string update=@"UPDATE KEXPLCI SET 
KHGID = :KHGID, KHGLCE = :KHGLCE, KHGDESC = :KHGDESC, KHGDESI = :KHGDESI, KHGMODI = :KHGMODI
 WHERE 
KHGID = :id";
            const string delete=@"DELETE FROM KEXPLCI WHERE KHGID = :id";
            const string insert=@"INSERT INTO  KEXPLCI (
KHGID, KHGLCE, KHGDESC, KHGDESI, KHGMODI

) VALUES (
:KHGID, :KHGLCE, :KHGDESC, :KHGDESI, :KHGMODI
)";
            const string select_GetAll=@"SELECT
KHGID, KHGLCE, KHGDESC, KHGDESI, KHGMODI
 FROM KEXPLCI
";
            const string select_GetByBase=@"SELECT
KHGID, KHGLCE, KHGDESC, KHGDESI, KHGMODI
 FROM KEXPLCI
WHERE KHGLCE = :KHGLCE
";
            #endregion

            public KExpLciRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KExpLci Get(Int64 id){
                return connection.Query<KExpLci>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KHGID") ;
            }

            public void Insert(KExpLci value){
                    if(value.Khgid == default(Int64)) {
                        value.Khgid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KHGID",value.Khgid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHGLCE",value.Khglce??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHGDESC",value.Khgdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KHGDESI",value.Khgdesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHGMODI",value.Khgmodi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KExpLci value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Khgid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KExpLci value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KHGID",value.Khgid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHGLCE",value.Khglce??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHGDESC",value.Khgdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KHGDESI",value.Khgdesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHGMODI",value.Khgmodi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("id",value.Khgid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KExpLci> GetAll(){
                    return connection.EnsureOpened().Query<KExpLci>(select_GetAll).ToList();
            }
            public IEnumerable<KExpLci> GetByBase(string KHGLCE){
                    return connection.EnsureOpened().Query<KExpLci>(select_GetByBase, new {KHGLCE}).ToList();
            }
    }
}
