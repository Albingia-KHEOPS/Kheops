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

    public  partial class  KCanevRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KGOID, KGOTYP, KGOCNVA, KGODESC, KGOKAIID
, KGOCDEF, KGOCRU, KGOCRD, KGOCRH, KGOMAJU
, KGOMAJD, KGOMAJH, KGOSIT FROM KCANEV
WHERE KGOID = :KGOID
";
            const string update=@"UPDATE KCANEV SET 
KGOID = :KGOID, KGOTYP = :KGOTYP, KGOCNVA = :KGOCNVA, KGODESC = :KGODESC, KGOKAIID = :KGOKAIID, KGOCDEF = :KGOCDEF, KGOCRU = :KGOCRU, KGOCRD = :KGOCRD, KGOCRH = :KGOCRH, KGOMAJU = :KGOMAJU
, KGOMAJD = :KGOMAJD, KGOMAJH = :KGOMAJH, KGOSIT = :KGOSIT
 WHERE 
KGOID = :KGOID";
            const string delete=@"DELETE FROM KCANEV WHERE KGOID = :KGOID";
            const string insert=@"INSERT INTO  KCANEV (
KGOID, KGOTYP, KGOCNVA, KGODESC, KGOKAIID
, KGOCDEF, KGOCRU, KGOCRD, KGOCRH, KGOMAJU
, KGOMAJD, KGOMAJH, KGOSIT
) VALUES (
:KGOID, :KGOTYP, :KGOCNVA, :KGODESC, :KGOKAIID
, :KGOCDEF, :KGOCRU, :KGOCRD, :KGOCRH, :KGOMAJU
, :KGOMAJD, :KGOMAJH, :KGOSIT)";
            #endregion

            public KCanevRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KCanev Get(Int64 KGOID){
                return connection.Query<KCanev>(select, new {KGOID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KGOID") ;
            }

            public void Insert(KCanev value){
                    if(value.Kgoid == default(Int64)) {
                        value.Kgoid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KGOID",value.Kgoid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KGOTYP",value.Kgotyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGOCNVA",value.Kgocnva??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KGODESC",value.Kgodesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KGOKAIID",value.Kgokaiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KGOCDEF",value.Kgocdef??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGOCRU",value.Kgocru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGOCRD",value.Kgocrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KGOCRH",value.Kgocrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KGOMAJU",value.Kgomaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGOMAJD",value.Kgomajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KGOMAJH",value.Kgomajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KGOSIT",value.Kgosit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KCanev value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KGOID",value.Kgoid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KCanev value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KGOID",value.Kgoid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KGOTYP",value.Kgotyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGOCNVA",value.Kgocnva??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KGODESC",value.Kgodesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KGOKAIID",value.Kgokaiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KGOCDEF",value.Kgocdef??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGOCRU",value.Kgocru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGOCRD",value.Kgocrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KGOCRH",value.Kgocrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KGOMAJU",value.Kgomaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGOMAJD",value.Kgomajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KGOMAJH",value.Kgomajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KGOSIT",value.Kgosit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGOID",value.Kgoid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
    }
}
