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

    public  partial class  KcliblefRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KAIID, KAICIBLE, KAIKAHID, KAIBRA, KAISBR
, KAICAT, KAICRU, KAICRD, KAICRH, KAIMAJU
, KAIMAJD, KAIMAJH FROM KCIBLEF
WHERE KAIID = :id
";
            const string update=@"UPDATE KCIBLEF SET 
KAIID = :KAIID, KAICIBLE = :KAICIBLE, KAIKAHID = :KAIKAHID, KAIBRA = :KAIBRA, KAISBR = :KAISBR, KAICAT = :KAICAT, KAICRU = :KAICRU, KAICRD = :KAICRD, KAICRH = :KAICRH, KAIMAJU = :KAIMAJU
, KAIMAJD = :KAIMAJD, KAIMAJH = :KAIMAJH
 WHERE 
KAIID = :id";
            const string delete=@"DELETE FROM KCIBLEF WHERE KAIID = :id";
            const string insert=@"INSERT INTO  KCIBLEF (
KAIID, KAICIBLE, KAIKAHID, KAIBRA, KAISBR
, KAICAT, KAICRU, KAICRD, KAICRH, KAIMAJU
, KAIMAJD, KAIMAJH
) VALUES (
:KAIID, :KAICIBLE, :KAIKAHID, :KAIBRA, :KAISBR
, :KAICAT, :KAICRU, :KAICRD, :KAICRH, :KAIMAJU
, :KAIMAJD, :KAIMAJH)";
            const string select_GetAll=@"SELECT
KAIID, KAICIBLE, KAIKAHID, KAIBRA, KAISBR
, KAICAT, KAICRU, KAICRD, KAICRH, KAIMAJU
, KAIMAJD, KAIMAJH FROM KCIBLEF
";
            #endregion

            public KcliblefRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public Kcliblef Get(Int64 id){
                return connection.Query<Kcliblef>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KAIID") ;
            }

            public void Insert(Kcliblef value){
                    if(value.Kaiid == default(Int64)) {
                        value.Kaiid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KAIID",value.Kaiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAICIBLE",value.Kaicible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAIKAHID",value.Kaikahid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAIBRA",value.Kaibra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KAISBR",value.Kaisbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAICAT",value.Kaicat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAICRU",value.Kaicru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAICRD",value.Kaicrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAICRH",value.Kaicrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAIMAJU",value.Kaimaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAIMAJD",value.Kaimajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAIMAJH",value.Kaimajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(Kcliblef value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Kaiid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(Kcliblef value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAIID",value.Kaiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAICIBLE",value.Kaicible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAIKAHID",value.Kaikahid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAIBRA",value.Kaibra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KAISBR",value.Kaisbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAICAT",value.Kaicat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAICRU",value.Kaicru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAICRD",value.Kaicrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAICRH",value.Kaicrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAIMAJU",value.Kaimaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAIMAJD",value.Kaimajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAIMAJH",value.Kaimajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("id",value.Kaiid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<Kcliblef> GetAll(){
                    return connection.EnsureOpened().Query<Kcliblef>(select_GetAll).ToList();
            }
    }
}
