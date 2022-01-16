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

    public  partial class  KInvTypRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KAGID, KAGTYINV, KAGDESC, KAGTMAP, KAGTABLE
, KAGKGGID FROM KINVTYP
WHERE KAGID = :parKAGID
";
            const string update=@"UPDATE KINVTYP SET 
KAGID = :KAGID, KAGTYINV = :KAGTYINV, KAGDESC = :KAGDESC, KAGTMAP = :KAGTMAP, KAGTABLE = :KAGTABLE, KAGKGGID = :KAGKGGID
 WHERE 
KAGID = :parKAGID";
            const string delete=@"DELETE FROM KINVTYP WHERE KAGID = :parKAGID";
            const string insert=@"INSERT INTO  KINVTYP (
KAGID, KAGTYINV, KAGDESC, KAGTMAP, KAGTABLE
, KAGKGGID
) VALUES (
:KAGID, :KAGTYINV, :KAGDESC, :KAGTMAP, :KAGTABLE
, :KAGKGGID)";
            const string select_GetAll=@"SELECT
KAGID, KAGTYINV, KAGDESC, KAGTMAP, KAGTABLE
, KAGKGGID FROM KINVTYP
";
            #endregion

            public KInvTypRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KInvTyp Get(Int64 parKAGID){
                return connection.Query<KInvTyp>(select, new {parKAGID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KAGID") ;
            }

            public void Insert(KInvTyp value){
                    if(value.Kagid == default(Int64)) {
                        value.Kagid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KAGID",value.Kagid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAGTYINV",value.Kagtyinv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("KAGDESC",value.Kagdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KAGTMAP",value.Kagtmap, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KAGTABLE",value.Kagtable??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAGKGGID",value.Kagkggid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KInvTyp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("parKAGID",value.Kagid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KInvTyp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAGID",value.Kagid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAGTYINV",value.Kagtyinv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("KAGDESC",value.Kagdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KAGTMAP",value.Kagtmap, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KAGTABLE",value.Kagtable??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAGKGGID",value.Kagkggid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("parKAGID",value.Kagid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KInvTyp> GetAll(){
                    return connection.EnsureOpened().Query<KInvTyp>(select_GetAll).ToList();
            }
    }
}
