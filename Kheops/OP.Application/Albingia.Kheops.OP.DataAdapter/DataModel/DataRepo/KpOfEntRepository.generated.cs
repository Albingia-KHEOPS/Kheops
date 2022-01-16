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

    public  partial class  KpOfEntRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KFHPOG, KFHALG, KFHIPB, KFHALX, KFHNPO
, KFHEFD, KFHSAD, KFHBRA, KFHCIBLE, KFHIPR
, KFHALR, KFHTYPO, KFHIPM, KHFSIT, KFHSTU
, KFHSTD FROM KPOFENT
WHERE KFHPOG = :parKFHPOG
and KFHALG = :parKFHALG
";
            const string update=@"UPDATE KPOFENT SET 
KFHPOG = :KFHPOG, KFHALG = :KFHALG, KFHIPB = :KFHIPB, KFHALX = :KFHALX, KFHNPO = :KFHNPO, KFHEFD = :KFHEFD, KFHSAD = :KFHSAD, KFHBRA = :KFHBRA, KFHCIBLE = :KFHCIBLE, KFHIPR = :KFHIPR
, KFHALR = :KFHALR, KFHTYPO = :KFHTYPO, KFHIPM = :KFHIPM, KHFSIT = :KHFSIT, KFHSTU = :KFHSTU, KFHSTD = :KFHSTD
 WHERE 
KFHPOG = :parKFHPOG and KFHALG = :parKFHALG";
            const string delete=@"DELETE FROM KPOFENT WHERE KFHPOG = :parKFHPOG AND KFHALG = :parKFHALG";
            const string insert=@"INSERT INTO  KPOFENT (
KFHPOG, KFHALG, KFHIPB, KFHALX, KFHNPO
, KFHEFD, KFHSAD, KFHBRA, KFHCIBLE, KFHIPR
, KFHALR, KFHTYPO, KFHIPM, KHFSIT, KFHSTU
, KFHSTD
) VALUES (
:KFHPOG, :KFHALG, :KFHIPB, :KFHALX, :KFHNPO
, :KFHEFD, :KFHSAD, :KFHBRA, :KFHCIBLE, :KFHIPR
, :KFHALR, :KFHTYPO, :KFHIPM, :KHFSIT, :KFHSTU
, :KFHSTD)";
            const string select_GetByOffre=@"SELECT
KFHPOG, KFHALG, KFHIPB, KFHALX, KFHNPO
, KFHEFD, KFHSAD, KFHBRA, KFHCIBLE, KFHIPR
, KFHALR, KFHTYPO, KFHIPM, KHFSIT, KFHSTU
, KFHSTD FROM KPOFENT
WHERE KFHIPB = :parKFHIPB
and KFHALX = :parKFHALX
";
            #endregion

            public KpOfEntRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpOfEnt Get(string parKFHPOG, int parKFHALG){
                return connection.Query<KpOfEnt>(select, new {parKFHPOG, parKFHALG}).SingleOrDefault();
            }


            public void Insert(KpOfEnt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFHPOG",value.Kfhpog??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFHALG",value.Kfhalg, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFHIPB",value.Kfhipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFHALX",value.Kfhalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFHNPO",value.Kfhnpo, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFHEFD",value.Kfhefd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFHSAD",value.Kfhsad, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFHBRA",value.Kfhbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFHCIBLE",value.Kfhcible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFHIPR",value.Kfhipr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFHALR",value.Kfhalr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFHTYPO",value.Kfhtypo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFHIPM",value.Kfhipm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KHFSIT",value.Khfsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFHSTU",value.Kfhstu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFHSTD",value.Kfhstd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpOfEnt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("parKFHPOG",value.Kfhpog);
                    parameters.Add("parKFHALG",value.Kfhalg);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpOfEnt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFHPOG",value.Kfhpog??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFHALG",value.Kfhalg, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFHIPB",value.Kfhipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFHALX",value.Kfhalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFHNPO",value.Kfhnpo, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFHEFD",value.Kfhefd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFHSAD",value.Kfhsad, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFHBRA",value.Kfhbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFHCIBLE",value.Kfhcible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFHIPR",value.Kfhipr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFHALR",value.Kfhalr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFHTYPO",value.Kfhtypo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFHIPM",value.Kfhipm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KHFSIT",value.Khfsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFHSTU",value.Kfhstu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFHSTD",value.Kfhstd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("parKFHPOG",value.Kfhpog);
                    parameters.Add("parKFHALG",value.Kfhalg);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpOfEnt> GetByOffre(string parKFHIPB, int parKFHALX){
                    return connection.EnsureOpened().Query<KpOfEnt>(select_GetByOffre, new {parKFHIPB, parKFHALX}).ToList();
            }
    }
}
