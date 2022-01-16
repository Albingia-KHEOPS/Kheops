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

    public  partial class  KVoletRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KAKID, KAKVOLET, KAKDESC, KAKCRU, KAKCRD
, KAKCRH, KAKMAJU, KAKMAJD, KAKMAJH, KAKBRA
, KAKFGEN, KAKPRES FROM KVOLET
WHERE KAKID = :Id
";
            const string update=@"UPDATE KVOLET SET 
KAKID = :KAKID, KAKVOLET = :KAKVOLET, KAKDESC = :KAKDESC, KAKCRU = :KAKCRU, KAKCRD = :KAKCRD, KAKCRH = :KAKCRH, KAKMAJU = :KAKMAJU, KAKMAJD = :KAKMAJD, KAKMAJH = :KAKMAJH, KAKBRA = :KAKBRA
, KAKFGEN = :KAKFGEN, KAKPRES = :KAKPRES
 WHERE 
KAKID = :Id";
            const string delete=@"DELETE FROM KVOLET WHERE KAKID = :Id";
            const string insert=@"INSERT INTO  KVOLET (
KAKID, KAKVOLET, KAKDESC, KAKCRU, KAKCRD
, KAKCRH, KAKMAJU, KAKMAJD, KAKMAJH, KAKBRA
, KAKFGEN, KAKPRES
) VALUES (
:KAKID, :KAKVOLET, :KAKDESC, :KAKCRU, :KAKCRD
, :KAKCRH, :KAKMAJU, :KAKMAJD, :KAKMAJH, :KAKBRA
, :KAKFGEN, :KAKPRES)";
            const string select_GetAll=@"SELECT
KAKID, KAKVOLET, KAKDESC, KAKCRU, KAKCRD
, KAKCRH, KAKMAJU, KAKMAJD, KAKMAJH, KAKBRA
, KAKFGEN, KAKPRES FROM KVOLET
";
            #endregion

            public KVoletRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KVolet Get(Int64 Id){
                return connection.Query<KVolet>(select, new {Id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KAKID") ;
            }

            public void Insert(KVolet value){
                    if(value.Kakid == default(Int64)) {
                        value.Kakid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KAKID",value.Kakid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAKVOLET",value.Kakvolet??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAKDESC",value.Kakdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KAKCRU",value.Kakcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAKCRD",value.Kakcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAKCRH",value.Kakcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAKMAJU",value.Kakmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAKMAJD",value.Kakmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAKMAJH",value.Kakmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAKBRA",value.Kakbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KAKFGEN",value.Kakfgen??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAKPRES",value.Kakpres??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KVolet value){
                    var parameters = new DynamicParameters();
                    parameters.Add("Id",value.Kakid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KVolet value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAKID",value.Kakid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAKVOLET",value.Kakvolet??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAKDESC",value.Kakdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KAKCRU",value.Kakcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAKCRD",value.Kakcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAKCRH",value.Kakcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAKMAJU",value.Kakmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAKMAJD",value.Kakmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAKMAJH",value.Kakmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAKBRA",value.Kakbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KAKFGEN",value.Kakfgen??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAKPRES",value.Kakpres??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("Id",value.Kakid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KVolet> GetAll(){
                    return connection.EnsureOpened().Query<KVolet>(select_GetAll).ToList();
            }
    }
}
