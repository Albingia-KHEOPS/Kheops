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

    public  partial class  KpCopidRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KFLTYP, KFLIPB, KFLALX, KFLTAB, KFLIDO
, KFLIDC FROM KPCOPID
WHERE KFLTYP = :KFLTYP
and KFLIPB = :KFLIPB
and KFLALX = :KFLALX
and KFLTAB = :KFLTAB
and KFLIDO = :KFLIDO
";
            const string update=@"UPDATE KPCOPID SET 
KFLTYP = :KFLTYP, KFLIPB = :KFLIPB, KFLALX = :KFLALX, KFLTAB = :KFLTAB, KFLIDO = :KFLIDO, KFLIDC = :KFLIDC
 WHERE 
KFLTYP = :KFLTYP and KFLIPB = :KFLIPB and KFLALX = :KFLALX and KFLTAB = :KFLTAB and KFLIDO = :KFLIDO";
            const string delete=@"DELETE FROM KPCOPID WHERE KFLTYP = :KFLTYP AND KFLIPB = :KFLIPB AND KFLALX = :KFLALX AND KFLTAB = :KFLTAB AND KFLIDO = :KFLIDO";
            const string insert=@"INSERT INTO  KPCOPID (
KFLTYP, KFLIPB, KFLALX, KFLTAB, KFLIDO
, KFLIDC
) VALUES (
:KFLTYP, :KFLIPB, :KFLALX, :KFLTAB, :KFLIDO
, :KFLIDC)";
            const string select_GetByAffaire=@"SELECT
KFLTYP, KFLIPB, KFLALX, KFLTAB, KFLIDO
, KFLIDC FROM KPCOPID
WHERE KFLTYP = :KFLTYP
and KFLIPB = :KFLIPB
and KFLALX = :KFLALX
";
            #endregion

            public KpCopidRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpCopid Get(string KFLTYP, string KFLIPB, int KFLALX, string KFLTAB, Int64 KFLIDO){
                return connection.Query<KpCopid>(select, new {KFLTYP, KFLIPB, KFLALX, KFLTAB, KFLIDO}).SingleOrDefault();
            }


            public void Insert(KpCopid value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFLTYP",value.Kfltyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFLIPB",value.Kflipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFLALX",value.Kflalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFLTAB",value.Kfltab??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFLIDO",value.Kflido, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFLIDC",value.Kflidc, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpCopid value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFLTYP",value.Kfltyp);
                    parameters.Add("KFLIPB",value.Kflipb);
                    parameters.Add("KFLALX",value.Kflalx);
                    parameters.Add("KFLTAB",value.Kfltab);
                    parameters.Add("KFLIDO",value.Kflido);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpCopid value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFLTYP",value.Kfltyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFLIPB",value.Kflipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFLALX",value.Kflalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFLTAB",value.Kfltab??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFLIDO",value.Kflido, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFLIDC",value.Kflidc, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFLTYP",value.Kfltyp);
                    parameters.Add("KFLIPB",value.Kflipb);
                    parameters.Add("KFLALX",value.Kflalx);
                    parameters.Add("KFLTAB",value.Kfltab);
                    parameters.Add("KFLIDO",value.Kflido);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpCopid> GetByAffaire(string KFLTYP, string KFLIPB, int KFLALX){
                    return connection.EnsureOpened().Query<KpCopid>(select_GetByAffaire, new {KFLTYP, KFLIPB, KFLALX}).ToList();
            }
    }
}
