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

    public  partial class  YCourtNRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
TNICT, TNINL, TNTNM, TNORD, TNTYP
, TNNOM, TNTIT, TNXN5 FROM YCOURTN
WHERE TNICT = :TNICT
and TNXN5 = :TNXN5
and TNTNM = :TNTNM
and TNORD = :TNORD
";
            const string update=@"UPDATE YCOURTN SET 
TNICT = :TNICT, TNINL = :TNINL, TNTNM = :TNTNM, TNORD = :TNORD, TNTYP = :TNTYP, TNNOM = :TNNOM, TNTIT = :TNTIT, TNXN5 = :TNXN5
 WHERE 
TNICT = :TNICT and TNXN5 = :TNXN5 and TNTNM = :TNTNM and TNORD = :TNORD";
            const string delete=@"DELETE FROM YCOURTN WHERE TNICT = :TNICT AND TNXN5 = :TNXN5 AND TNTNM = :TNTNM AND TNORD = :TNORD";
            const string insert=@"INSERT INTO  YCOURTN (
TNICT, TNINL, TNTNM, TNORD, TNTYP
, TNNOM, TNTIT, TNXN5
) VALUES (
:TNICT, :TNINL, :TNTNM, :TNORD, :TNTYP
, :TNNOM, :TNTIT, :TNXN5)";
            #endregion

            public YCourtNRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YCourtN Get(int TNICT, int TNXN5, string TNTNM, int TNORD){
                return connection.Query<YCourtN>(select, new {TNICT, TNXN5, TNTNM, TNORD}).SingleOrDefault();
            }


            public void Insert(YCourtN value){
                    var parameters = new DynamicParameters();
                    parameters.Add("TNICT",value.Tnict, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TNINL",value.Tninl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TNTNM",value.Tntnm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TNORD",value.Tnord, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TNTYP",value.Tntyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TNNOM",value.Tnnom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("TNTIT",value.Tntit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TNXN5",value.Tnxn5, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YCourtN value){
                    var parameters = new DynamicParameters();
                    parameters.Add("TNICT",value.Tnict);
                    parameters.Add("TNXN5",value.Tnxn5);
                    parameters.Add("TNTNM",value.Tntnm);
                    parameters.Add("TNORD",value.Tnord);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YCourtN value){
                    var parameters = new DynamicParameters();
                    parameters.Add("TNICT",value.Tnict, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TNINL",value.Tninl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TNTNM",value.Tntnm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TNORD",value.Tnord, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("TNTYP",value.Tntyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TNNOM",value.Tnnom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("TNTIT",value.Tntit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TNXN5",value.Tnxn5, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TNICT",value.Tnict);
                    parameters.Add("TNXN5",value.Tnxn5);
                    parameters.Add("TNTNM",value.Tntnm);
                    parameters.Add("TNORD",value.Tnord);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
    }
}
