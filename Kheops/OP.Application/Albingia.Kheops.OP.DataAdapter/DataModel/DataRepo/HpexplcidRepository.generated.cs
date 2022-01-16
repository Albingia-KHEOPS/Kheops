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

    public  partial class  HpexplcidRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDJID, KDJAVN, KDJHIN, KDJKDIID, KDJORDRE
, KDJLCVAL, KDJLCVAU, KDJLCBASE, KDJLOVAL, KDJLOVAU
, KDJLOBASE FROM HPEXPLCID
WHERE KDJID = :id
and KDJAVN = :numeroAvenant
";
            const string update=@"UPDATE HPEXPLCID SET 
KDJID = :KDJID, KDJAVN = :KDJAVN, KDJHIN = :KDJHIN, KDJKDIID = :KDJKDIID, KDJORDRE = :KDJORDRE, KDJLCVAL = :KDJLCVAL, KDJLCVAU = :KDJLCVAU, KDJLCBASE = :KDJLCBASE, KDJLOVAL = :KDJLOVAL, KDJLOVAU = :KDJLOVAU
, KDJLOBASE = :KDJLOBASE
 WHERE 
KDJID = :id and KDJAVN = :numeroAvenant";
            const string delete=@"DELETE FROM HPEXPLCID WHERE KDJID = :id AND KDJAVN = :numeroAvenant";
            const string insert=@"INSERT INTO  HPEXPLCID (
KDJID, KDJAVN, KDJHIN, KDJKDIID, KDJORDRE
, KDJLCVAL, KDJLCVAU, KDJLCBASE, KDJLOVAL, KDJLOVAU
, KDJLOBASE
) VALUES (
:KDJID, :KDJAVN, :KDJHIN, :KDJKDIID, :KDJORDRE
, :KDJLCVAL, :KDJLCVAU, :KDJLCBASE, :KDJLOVAL, :KDJLOVAU
, :KDJLOBASE)";
            const string select_GetByAffaire=@"SELECT
KDJID, KDJAVN, KDJHIN, KDJKDIID, KDJORDRE
, KDJLCVAL, KDJLCVAU, KDJLCBASE, KDJLOVAL, KDJLOVAU
, KDJLOBASE FROM HPEXPLCID
INNER JOIN HPEXPLCI ON KDJKDIID = KDIID AND KDJAVN = KDIAVN
WHERE KDITYP = :typeAffaire
and KDIIPB = :numeroAffaire
and KDIALX = :numeroAliment
and KDIAVN = :numeroAvenant
";
            #endregion

            public HpexplcidRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpExpLCID Get(Int64 id, int numeroAvenant){
                return connection.Query<KpExpLCID>(select, new {id, numeroAvenant}).SingleOrDefault();
            }


            public void Insert(KpExpLCID value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDJID",value.Kdjid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDJAVN",value.Kdjavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDJHIN",value.Kdjhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
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
                    parameters.Add("numeroAvenant",value.Kdjavn);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpExpLCID value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDJID",value.Kdjid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDJAVN",value.Kdjavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDJHIN",value.Kdjhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDJKDIID",value.Kdjkdiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDJORDRE",value.Kdjordre, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDJLCVAL",value.Kdjlcval, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDJLCVAU",value.Kdjlcvau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDJLCBASE",value.Kdjlcbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDJLOVAL",value.Kdjloval, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDJLOVAU",value.Kdjlovau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDJLOBASE",value.Kdjlobase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("id",value.Kdjid);
                    parameters.Add("numeroAvenant",value.Kdjavn);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpExpLCID> GetByAffaire(string typeAffaire, string numeroAffaire, int numeroAliment, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpExpLCID>(select_GetByAffaire, new {typeAffaire, numeroAffaire, numeroAliment, numeroAvenant}).ToList();
            }
    }
}
