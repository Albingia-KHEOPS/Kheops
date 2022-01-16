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

    public  partial class  KExpFrhRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KHEID, KHEFHE, KHEDESC, KHEDESI, KHEMODI
 FROM KEXPFRH
WHERE KHEID = :id
";
            const string update=@"UPDATE KEXPFRH SET 
KHEID = :KHEID, KHEFHE = :KHEFHE, KHEDESC = :KHEDESC, KHEDESI = :KHEDESI, KHEMODI = :KHEMODI
 WHERE 
KHEID = :id";
            const string delete=@"DELETE FROM KEXPFRH WHERE KHEID = :id";
            const string insert=@"INSERT INTO  KEXPFRH (
KHEID, KHEFHE, KHEDESC, KHEDESI, KHEMODI

) VALUES (
:KHEID, :KHEFHE, :KHEDESC, :KHEDESI, :KHEMODI
)";
            const string select_GetAll=@"SELECT
KHEID, KHEFHE, KHEDESC, KHEDESI, KHEMODI
 FROM KEXPFRH
";
            const string select_GetByBase=@"SELECT
KHEID, KHEFHE, KHEDESC, KHEDESI, KHEMODI
 FROM KEXPFRH
WHERE KHEFHE = :parKHEFHE
";
            #endregion

            public KExpFrhRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KExpFrh Get(Int64 id){
                return connection.Query<KExpFrh>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KHEID") ;
            }

            public void Insert(KExpFrh value){
                    if(value.Kheid == default(Int64)) {
                        value.Kheid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KHEID",value.Kheid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHEFHE",value.Khefhe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHEDESC",value.Khedesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KHEDESI",value.Khedesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHEMODI",value.Khemodi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KExpFrh value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Kheid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KExpFrh value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KHEID",value.Kheid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHEFHE",value.Khefhe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHEDESC",value.Khedesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KHEDESI",value.Khedesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHEMODI",value.Khemodi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("id",value.Kheid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KExpFrh> GetAll(){
                    return connection.EnsureOpened().Query<KExpFrh>(select_GetAll).ToList();
            }
            public IEnumerable<KExpFrh> GetByBase(string parKHEFHE){
                    return connection.EnsureOpened().Query<KExpFrh>(select_GetByBase, new {parKHEFHE}).ToList();
            }
    }
}
