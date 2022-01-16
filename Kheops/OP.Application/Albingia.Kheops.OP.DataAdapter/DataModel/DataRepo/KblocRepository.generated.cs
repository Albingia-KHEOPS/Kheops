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

    public  partial class  KblocRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KAEID, KAEBLOC, KAEDESC, KAECRU, KAECRD
, KAECRH, KAEMAJU, KAEMAJD, KAEMAJH FROM KBLOC
WHERE KAEID = :parKAEID
";
            const string update=@"UPDATE KBLOC SET 
KAEID = :KAEID, KAEBLOC = :KAEBLOC, KAEDESC = :KAEDESC, KAECRU = :KAECRU, KAECRD = :KAECRD, KAECRH = :KAECRH, KAEMAJU = :KAEMAJU, KAEMAJD = :KAEMAJD, KAEMAJH = :KAEMAJH
 WHERE 
KAEID = :parKAEID";
            const string delete=@"DELETE FROM KBLOC WHERE KAEID = :parKAEID";
            const string insert=@"INSERT INTO  KBLOC (
KAEID, KAEBLOC, KAEDESC, KAECRU, KAECRD
, KAECRH, KAEMAJU, KAEMAJD, KAEMAJH
) VALUES (
:KAEID, :KAEBLOC, :KAEDESC, :KAECRU, :KAECRD
, :KAECRH, :KAEMAJU, :KAEMAJD, :KAEMAJH)";
            const string select_GetAll=@"SELECT
KAEID, KAEBLOC, KAEDESC, KAECRU, KAECRD
, KAECRH, KAEMAJU, KAEMAJD, KAEMAJH FROM KBLOC
";
            #endregion

            public KblocRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public Kbloc Get(Int64 parKAEID){
                return connection.Query<Kbloc>(select, new {parKAEID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KAEID") ;
            }

            public void Insert(Kbloc value){
                    if(value.Kaeid == default(Int64)) {
                        value.Kaeid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KAEID",value.Kaeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAEBLOC",value.Kaebloc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAEDESC",value.Kaedesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KAECRU",value.Kaecru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAECRD",value.Kaecrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAECRH",value.Kaecrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAEMAJU",value.Kaemaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAEMAJD",value.Kaemajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAEMAJH",value.Kaemajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(Kbloc value){
                    var parameters = new DynamicParameters();
                    parameters.Add("parKAEID",value.Kaeid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(Kbloc value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAEID",value.Kaeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAEBLOC",value.Kaebloc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAEDESC",value.Kaedesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KAECRU",value.Kaecru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAECRD",value.Kaecrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAECRH",value.Kaecrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAEMAJU",value.Kaemaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAEMAJD",value.Kaemajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAEMAJH",value.Kaemajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("parKAEID",value.Kaeid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<Kbloc> GetAll(){
                    return connection.EnsureOpened().Query<Kbloc>(select_GetAll).ToList();
            }
    }
}
