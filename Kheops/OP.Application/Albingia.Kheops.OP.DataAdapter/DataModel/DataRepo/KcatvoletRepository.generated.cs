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

    public  partial class  KcatvoletRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KAPID AS KAPID, KAPBRA AS KAPBRA, KAPCIBLE AS KAPCIBLE, KAPKAIID AS KAPKAIID, KAPVOLET AS KAPVOLET
, KAPKAKID AS KAPKAKID, KAPCAR AS KAPCAR, KAPORDRE AS KAPORDRE, KAPCRU AS KAPCRU, KAPCRD AS KAPCRD
, KAPCRH AS KAPCRH, KAPMAJU AS KAPMAJU, KAPMAJD AS KAPMAJD, KAPMAJH AS KAPMAJH, KAKDESC AS KAKDESC
 FROM KCATVOLET 
left join KVOLET on KAKID = KAPKAKID
WHERE KAPID = :id
FETCH FIRST 200 ROWS ONLY
";
            const string update=@"UPDATE KCATVOLET  SET 
KAPID = :KAPID, KAPBRA = :KAPBRA, KAPCIBLE = :KAPCIBLE, KAPKAIID = :KAPKAIID, KAPVOLET = :KAPVOLET, KAPKAKID = :KAPKAKID, KAPCAR = :KAPCAR, KAPORDRE = :KAPORDRE, KAPCRU = :KAPCRU, KAPCRD = :KAPCRD
, KAPCRH = :KAPCRH, KAPMAJU = :KAPMAJU, KAPMAJD = :KAPMAJD, KAPMAJH = :KAPMAJH
 WHERE 
KAPID = :id";
            const string delete=@"DELETE FROM KCATVOLET  WHERE KAPID = :id";
            const string insert=@"INSERT INTO  KCATVOLET  (
KAPID, KAPBRA, KAPCIBLE, KAPKAIID, KAPVOLET
, KAPKAKID, KAPCAR, KAPORDRE, KAPCRU, KAPCRD
, KAPCRH, KAPMAJU, KAPMAJD, KAPMAJH
) VALUES (
:KAPID, :KAPBRA, :KAPCIBLE, :KAPKAIID, :KAPVOLET
, :KAPKAKID, :KAPCAR, :KAPORDRE, :KAPCRU, :KAPCRD
, :KAPCRH, :KAPMAJU, :KAPMAJD, :KAPMAJH)";
            const string select_ListeBrancheCible=@"SELECT
KAPID AS KAPID, KAPBRA AS KAPBRA, KAPCIBLE AS KAPCIBLE, KAPKAIID AS KAPKAIID, KAPVOLET AS KAPVOLET
, KAPKAKID AS KAPKAKID, KAPCAR AS KAPCAR, KAPORDRE AS KAPORDRE, KAKDESC AS KAKDESC FROM KCATVOLET 
left join KVOLET on KAKID = KAPKAKID
WHERE KAPBRA = :branche
and KAPCIBLE = :cible
FETCH FIRST 200 ROWS ONLY
";
            const string select_GetAll=@"SELECT
KAPID, KAPBRA, KAPCIBLE, KAPKAIID, KAPVOLET
, KAPKAKID, KAPCAR, KAPORDRE, KAPCRU, KAPCRD
, KAPCRH, KAPMAJU, KAPMAJD, KAPMAJH FROM KCATVOLET 
";
            #endregion

            public KcatvoletRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public Kcatvolet Get(Int64 id){
                return connection.Query<Kcatvolet>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KAPID") ;
            }

            public void Insert(Kcatvolet value){
                    if(value.Kapid == default(Int64)) {
                        value.Kapid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KAPID",value.Kapid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAPBRA",value.Kapbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KAPCIBLE",value.Kapcible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAPKAIID",value.Kapkaiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAPVOLET",value.Kapvolet??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAPKAKID",value.Kapkakid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAPCAR",value.Kapcar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAPORDRE",value.Kapordre, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KAPCRU",value.Kapcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAPCRD",value.Kapcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAPCRH",value.Kapcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAPMAJU",value.Kapmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAPMAJD",value.Kapmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAPMAJH",value.Kapmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(Kcatvolet value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Kapid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(Kcatvolet value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAPID",value.Kapid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAPBRA",value.Kapbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KAPCIBLE",value.Kapcible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAPKAIID",value.Kapkaiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAPVOLET",value.Kapvolet??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAPKAKID",value.Kapkakid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAPCAR",value.Kapcar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAPORDRE",value.Kapordre, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KAPCRU",value.Kapcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAPCRD",value.Kapcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAPCRH",value.Kapcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAPMAJU",value.Kapmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAPMAJD",value.Kapmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAPMAJH",value.Kapmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("id",value.Kapid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KcatVolet_BrancheCible> ListeBrancheCible(string branche, string cible){
                    return connection.EnsureOpened().Query<KcatVolet_BrancheCible>(select_ListeBrancheCible, new {branche, cible}).ToList();
            }
            public IEnumerable<Kcatvolet> GetAll(){
                    return connection.EnsureOpened().Query<Kcatvolet>(select_GetAll).ToList();
            }
    }
}
