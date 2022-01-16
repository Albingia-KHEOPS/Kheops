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

    public  partial class  KcibleRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KAHID, KAHCIBLE, KAHDESC, KAHCRU, KAHCRD
, KAHCRH, KAHMAJU, KAHMAJD, KAHMAJH, KAHNMG
, KAHCON, KAHFAM, KAHAUT FROM KCIBLE
WHERE KAHID = :id
";
            const string update=@"UPDATE KCIBLE SET 
KAHID = :KAHID, KAHCIBLE = :KAHCIBLE, KAHDESC = :KAHDESC, KAHCRU = :KAHCRU, KAHCRD = :KAHCRD, KAHCRH = :KAHCRH, KAHMAJU = :KAHMAJU, KAHMAJD = :KAHMAJD, KAHMAJH = :KAHMAJH, KAHNMG = :KAHNMG
, KAHCON = :KAHCON, KAHFAM = :KAHFAM, KAHAUT = :KAHAUT
 WHERE 
KAHID = :id";
            const string delete=@"DELETE FROM KCIBLE WHERE KAHID = :id";
            const string insert=@"INSERT INTO  KCIBLE (
KAHID, KAHCIBLE, KAHDESC, KAHCRU, KAHCRD
, KAHCRH, KAHMAJU, KAHMAJD, KAHMAJH, KAHNMG
, KAHCON, KAHFAM, KAHAUT
) VALUES (
:KAHID, :KAHCIBLE, :KAHDESC, :KAHCRU, :KAHCRD
, :KAHCRH, :KAHMAJU, :KAHMAJD, :KAHMAJH, :KAHNMG
, :KAHCON, :KAHFAM, :KAHAUT)";
            const string select_ListeCibleBrche=@"SELECT
KAHCIBLE AS KAHCIBLE, KAHDESC AS KAHDESC FROM KCIBLE
INNER JOIN KCIBLEF ON KAICIBLE = KAHCIBLE
WHERE KAIBRA = :branche
ORDER BY KAHCIBLE
FETCH FIRST 200 ROWS ONLY
";
            const string select_GetAll=@"SELECT
KAHID, KAHCIBLE, KAHDESC, KAHCRU, KAHCRD
, KAHCRH, KAHMAJU, KAHMAJD, KAHMAJH, KAHNMG
, KAHCON, KAHFAM, KAHAUT FROM KCIBLE
";
            const string select_GetByCible=@"SELECT
KAHID, KAHCIBLE, KAHDESC, KAHCRU, KAHCRD
, KAHCRH, KAHMAJU, KAHMAJD, KAHMAJH, KAHNMG
, KAHCON, KAHFAM, KAHAUT FROM KCIBLE
WHERE KAHCIBLE = :cible
";
            #endregion

            public KcibleRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public Kcible Get(Int64 id){
                return connection.Query<Kcible>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KAHID") ;
            }

            public void Insert(Kcible value){
                    if(value.Kahid == default(Int64)) {
                        value.Kahid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KAHID",value.Kahid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAHCIBLE",value.Kahcible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAHDESC",value.Kahdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KAHCRU",value.Kahcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAHCRD",value.Kahcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAHCRH",value.Kahcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAHMAJU",value.Kahmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAHMAJD",value.Kahmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAHMAJH",value.Kahmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAHNMG",value.Kahnmg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAHCON",value.Kahcon??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAHFAM",value.Kahfam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAHAUT",value.Kahaut??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(Kcible value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Kahid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(Kcible value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAHID",value.Kahid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAHCIBLE",value.Kahcible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAHDESC",value.Kahdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KAHCRU",value.Kahcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAHCRD",value.Kahcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAHCRH",value.Kahcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAHMAJU",value.Kahmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAHMAJD",value.Kahmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAHMAJH",value.Kahmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAHNMG",value.Kahnmg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAHCON",value.Kahcon??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAHFAM",value.Kahfam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAHAUT",value.Kahaut??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("id",value.Kahid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<Kcible> ListeCibleBrche(string branche){
                    return connection.EnsureOpened().Query<Kcible>(select_ListeCibleBrche, new {branche}).ToList();
            }
            public IEnumerable<Kcible> GetAll(){
                    return connection.EnsureOpened().Query<Kcible>(select_GetAll).ToList();
            }
            public IEnumerable<Kcible> GetByCible(string cible){
                    return connection.EnsureOpened().Query<Kcible>(select_GetByCible, new {cible}).ToList();
            }
    }
}
