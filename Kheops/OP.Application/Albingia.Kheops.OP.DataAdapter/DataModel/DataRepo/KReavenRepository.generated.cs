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

    public  partial class  KReavenRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KGAFAM, KGAVEN, KGALIBV, KGASEPA FROM KREAVEN
WHERE KGAFAM = :famille
and KGAVEN = :numColVentil
";
            const string update=@"UPDATE KREAVEN SET 
KGAFAM = :KGAFAM, KGAVEN = :KGAVEN, KGALIBV = :KGALIBV, KGASEPA = :KGASEPA
 WHERE 
KGAFAM = :famille and KGAVEN = :numColVentil";
            const string delete=@"DELETE FROM KREAVEN WHERE KGAFAM = :famille AND KGAVEN = :numColVentil";
            const string insert=@"INSERT INTO  KREAVEN (
KGAFAM, KGAVEN, KGALIBV, KGASEPA
) VALUES (
:KGAFAM, :KGAVEN, :KGALIBV, :KGASEPA)";
            const string select_GetAll=@"SELECT
KGAFAM, KGAVEN, KGALIBV, KGASEPA FROM KREAVEN
";
            #endregion

            public KReavenRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KReaven Get(string famille, int numColVentil){
                return connection.Query<KReaven>(select, new {famille, numColVentil}).SingleOrDefault();
            }


            public void Insert(KReaven value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KGAFAM",value.Kgafam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGAVEN",value.Kgaven, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KGALIBV",value.Kgalibv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("KGASEPA",value.Kgasepa??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KReaven value){
                    var parameters = new DynamicParameters();
                    parameters.Add("famille",value.Kgafam);
                    parameters.Add("numColVentil",value.Kgaven);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KReaven value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KGAFAM",value.Kgafam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGAVEN",value.Kgaven, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KGALIBV",value.Kgalibv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("KGASEPA",value.Kgasepa??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("famille",value.Kgafam);
                    parameters.Add("numColVentil",value.Kgaven);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KReaven> GetAll(){
                    return connection.EnsureOpened().Query<KReaven>(select_GetAll).ToList();
            }
    }
}
