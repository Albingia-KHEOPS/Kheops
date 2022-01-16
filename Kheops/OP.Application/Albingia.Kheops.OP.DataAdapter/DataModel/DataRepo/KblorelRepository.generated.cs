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

    public  partial class  KblorelRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KGJID, KGJREL, KGJIDBLO1, KGJBLO1, KGJIDBLO2
, KGJBLO2, KGJCRU, KGJCRD, KGJCRH, KGJMAJU
, KGJMAJD, KGJMAJH FROM KBLOREL
WHERE KGJID = :parKGJID
";
            const string update=@"UPDATE KBLOREL SET 
KGJID = :KGJID, KGJREL = :KGJREL, KGJIDBLO1 = :KGJIDBLO1, KGJBLO1 = :KGJBLO1, KGJIDBLO2 = :KGJIDBLO2, KGJBLO2 = :KGJBLO2, KGJCRU = :KGJCRU, KGJCRD = :KGJCRD, KGJCRH = :KGJCRH, KGJMAJU = :KGJMAJU
, KGJMAJD = :KGJMAJD, KGJMAJH = :KGJMAJH
 WHERE 
KGJID = :parKGJID";
            const string delete=@"DELETE FROM KBLOREL WHERE KGJID = :parKGJID";
            const string insert=@"INSERT INTO  KBLOREL (
KGJID, KGJREL, KGJIDBLO1, KGJBLO1, KGJIDBLO2
, KGJBLO2, KGJCRU, KGJCRD, KGJCRH, KGJMAJU
, KGJMAJD, KGJMAJH
) VALUES (
:KGJID, :KGJREL, :KGJIDBLO1, :KGJBLO1, :KGJIDBLO2
, :KGJBLO2, :KGJCRU, :KGJCRD, :KGJCRH, :KGJMAJU
, :KGJMAJD, :KGJMAJH)";
            const string select_GetAll=@"SELECT
KGJID, KGJREL, KGJIDBLO1, KGJBLO1, KGJIDBLO2
, KGJBLO2, KGJCRU, KGJCRD, KGJCRH, KGJMAJU
, KGJMAJD, KGJMAJH FROM KBLOREL
";
            #endregion

            public KblorelRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public Kblorel Get(Int64 parKGJID){
                return connection.Query<Kblorel>(select, new {parKGJID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KGJID") ;
            }

            public void Insert(Kblorel value){
                    if(value.Kgjid == default(Int64)) {
                        value.Kgjid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KGJID",value.Kgjid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KGJREL",value.Kgjrel??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGJIDBLO1",value.Kgjidblo1, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KGJBLO1",value.Kgjblo1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGJIDBLO2",value.Kgjidblo2, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KGJBLO2",value.Kgjblo2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGJCRU",value.Kgjcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGJCRD",value.Kgjcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KGJCRH",value.Kgjcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KGJMAJU",value.Kgjmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGJMAJD",value.Kgjmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KGJMAJH",value.Kgjmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(Kblorel value){
                    var parameters = new DynamicParameters();
                    parameters.Add("parKGJID",value.Kgjid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(Kblorel value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KGJID",value.Kgjid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KGJREL",value.Kgjrel??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGJIDBLO1",value.Kgjidblo1, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KGJBLO1",value.Kgjblo1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGJIDBLO2",value.Kgjidblo2, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KGJBLO2",value.Kgjblo2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGJCRU",value.Kgjcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGJCRD",value.Kgjcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KGJCRH",value.Kgjcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KGJMAJU",value.Kgjmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGJMAJD",value.Kgjmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KGJMAJH",value.Kgjmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("parKGJID",value.Kgjid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<Kblorel> GetAll(){
                    return connection.EnsureOpened().Query<Kblorel>(select_GetAll).ToList();
            }
    }
}
