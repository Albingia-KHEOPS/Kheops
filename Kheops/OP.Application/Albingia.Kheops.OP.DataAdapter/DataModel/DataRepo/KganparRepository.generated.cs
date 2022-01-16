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

    public  partial class  KganparRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KAUCAR, KAUNAT, KAUAFFI, KAUMODI, KAUGANC
, KAUGANNC FROM KGANPAR
WHERE KAUNAT = :nature
and KAUCAR = :caratere
";
            const string update=@"UPDATE KGANPAR SET 
KAUCAR = :KAUCAR, KAUNAT = :KAUNAT, KAUAFFI = :KAUAFFI, KAUMODI = :KAUMODI, KAUGANC = :KAUGANC, KAUGANNC = :KAUGANNC
 WHERE 
KAUNAT = :nature and KAUCAR = :caratere";
            const string delete=@"DELETE FROM KGANPAR WHERE KAUNAT = :nature AND KAUCAR = :caratere";
            const string insert=@"INSERT INTO  KGANPAR (
KAUCAR, KAUNAT, KAUAFFI, KAUMODI, KAUGANC
, KAUGANNC
) VALUES (
:KAUCAR, :KAUNAT, :KAUAFFI, :KAUMODI, :KAUGANC
, :KAUGANNC)";
            const string select_GetAll=@"SELECT
KAUCAR, KAUNAT, KAUAFFI, KAUMODI, KAUGANC
, KAUGANNC FROM KGANPAR
";
            #endregion

            public KganparRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public Kganpar Get(string nature, string caratere){
                return connection.Query<Kganpar>(select, new {nature, caratere}).SingleOrDefault();
            }


            public void Insert(Kganpar value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAUCAR",value.Kaucar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAUNAT",value.Kaunat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAUAFFI",value.Kauaffi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAUMODI",value.Kaumodi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAUGANC",value.Kauganc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAUGANNC",value.Kaugannc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(Kganpar value){
                    var parameters = new DynamicParameters();
                    parameters.Add("nature",value.Kaunat);
                    parameters.Add("caratere",value.Kaucar);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(Kganpar value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAUCAR",value.Kaucar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAUNAT",value.Kaunat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAUAFFI",value.Kauaffi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAUMODI",value.Kaumodi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAUGANC",value.Kauganc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAUGANNC",value.Kaugannc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("nature",value.Kaunat);
                    parameters.Add("caratere",value.Kaucar);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<Kganpar> GetAll(){
                    return connection.EnsureOpened().Query<Kganpar>(select_GetAll).ToList();
            }
    }
}
