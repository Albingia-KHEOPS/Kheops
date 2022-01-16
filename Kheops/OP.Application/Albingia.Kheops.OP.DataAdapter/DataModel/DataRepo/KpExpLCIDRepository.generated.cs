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

    public  partial class  KpExpLCIDRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDJID, KDJKDIID, KDJORDRE, KDJLCVAL, KDJLCVAU
, KDJLCBASE, KDJLOVAL, KDJLOVAU, KDJLOBASE FROM KPEXPLCID
WHERE KDJID = :id
";
            const string update=@"UPDATE KPEXPLCID SET 
KDJID = :KDJID, KDJKDIID = :KDJKDIID, KDJORDRE = :KDJORDRE, KDJLCVAL = :KDJLCVAL, KDJLCVAU = :KDJLCVAU, KDJLCBASE = :KDJLCBASE, KDJLOVAL = :KDJLOVAL, KDJLOVAU = :KDJLOVAU, KDJLOBASE = :KDJLOBASE
 WHERE 
KDJID = :id";
            const string delete=@"DELETE FROM KPEXPLCID WHERE KDJID = :id";
            const string insert=@"INSERT INTO  KPEXPLCID (
KDJID, KDJKDIID, KDJORDRE, KDJLCVAL, KDJLCVAU
, KDJLCBASE, KDJLOVAL, KDJLOVAU, KDJLOBASE
) VALUES (
:KDJID, :KDJKDIID, :KDJORDRE, :KDJLCVAL, :KDJLCVAU
, :KDJLCBASE, :KDJLOVAL, :KDJLOVAU, :KDJLOBASE)";
            const string select_GetByAffaire=@"SELECT
KDJID, KDJKDIID, KDJORDRE, KDJLCVAL, KDJLCVAU
, KDJLCBASE, KDJLOVAL, KDJLOVAU, KDJLOBASE FROM KPEXPLCID
INNER JOIN KPEXPLCI ON KDJKDIID = KDIID
WHERE KDITYP = :typeAffaire
and KDIIPB = :numeroAffaire
and KDIALX = :numeroAliment
";
            #endregion

            public KpExpLCIDRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpExpLCID Get(Int64 id){
                return connection.Query<KpExpLCID>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KDJID") ;
            }

            public void Insert(KpExpLCID value){
                    if(value.Kdjid == default(Int64)) {
                        value.Kdjid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KDJID",value.Kdjid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDJKDIID",value.Kdjkdiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDJORDRE",value.Kdjordre, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDJLCVAL",value.Kdjlcval, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDJLCVAU",value.Kdjlcvau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDJLCBASE",value.Kdjlcbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDJLOVAL",value.Kdjloval, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDJLOVAU",value.Kdjlovau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDJLOBASE",value.Kdjlobase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpExpLCID value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Kdjid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpExpLCID value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDJID",value.Kdjid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDJKDIID",value.Kdjkdiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDJORDRE",value.Kdjordre, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDJLCVAL",value.Kdjlcval, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDJLCVAU",value.Kdjlcvau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDJLCBASE",value.Kdjlcbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDJLOVAL",value.Kdjloval, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDJLOVAU",value.Kdjlovau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDJLOBASE",value.Kdjlobase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("id",value.Kdjid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpExpLCID> GetByAffaire(string typeAffaire, string numeroAffaire, int numeroAliment){
                    return connection.EnsureOpened().Query<KpExpLCID>(select_GetByAffaire, new {typeAffaire, numeroAffaire, numeroAliment}).ToList();
            }
    }
}
