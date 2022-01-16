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

    public  partial class  KFiltrLRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KGHID, KGHKGGID, KGHFILT, KGHORDR, KGHACTF
, KGHBRA, KGHCIBLE FROM KFILTRL
WHERE KGHID = :id
";
            const string update=@"UPDATE KFILTRL SET 
KGHID = :KGHID, KGHKGGID = :KGHKGGID, KGHFILT = :KGHFILT, KGHORDR = :KGHORDR, KGHACTF = :KGHACTF, KGHBRA = :KGHBRA, KGHCIBLE = :KGHCIBLE
 WHERE 
KGHID = :id";
            const string delete=@"DELETE FROM KFILTRL WHERE KGHID = :id";
            const string insert=@"INSERT INTO  KFILTRL (
KGHID, KGHKGGID, KGHFILT, KGHORDR, KGHACTF
, KGHBRA, KGHCIBLE
) VALUES (
:KGHID, :KGHKGGID, :KGHFILT, :KGHORDR, :KGHACTF
, :KGHBRA, :KGHCIBLE)";
            const string select_GetAll=@"SELECT
KGHID, KGHKGGID, KGHFILT, KGHORDR, KGHACTF
, KGHBRA, KGHCIBLE FROM KFILTRL
";
            #endregion

            public KFiltrLRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KFiltrL Get(Int64 id){
                return connection.Query<KFiltrL>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KGHID") ;
            }

            public void Insert(KFiltrL value){
                    if(value.Kghid == default(Int64)) {
                        value.Kghid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KGHID",value.Kghid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KGHKGGID",value.Kghkggid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KGHFILT",value.Kghfilt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGHORDR",value.Kghordr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KGHACTF",value.Kghactf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGHBRA",value.Kghbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KGHCIBLE",value.Kghcible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KFiltrL value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Kghid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KFiltrL value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KGHID",value.Kghid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KGHKGGID",value.Kghkggid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KGHFILT",value.Kghfilt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGHORDR",value.Kghordr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KGHACTF",value.Kghactf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGHBRA",value.Kghbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KGHCIBLE",value.Kghcible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("id",value.Kghid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KFiltrL> GetAll(){
                    return connection.EnsureOpened().Query<KFiltrL>(select_GetAll).ToList();
            }
    }
}
