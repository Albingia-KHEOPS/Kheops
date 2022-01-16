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

    public  partial class  KISModRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KGCMODID, KGCDESC, KGCDATD, KGCDATF, KGCSELECT
, KGCINSERT, KGCUPDATE, KGCEXIST, KGCCRU, KGCCRD
, KGCMJU, KGCMJD, KGCSAID2, KGCSCID2 FROM KISMOD
WHERE KGCMODID = :KGCMODID
";
            const string update=@"UPDATE KISMOD SET 
KGCMODID = :KGCMODID, KGCDESC = :KGCDESC, KGCDATD = :KGCDATD, KGCDATF = :KGCDATF, KGCSELECT = :KGCSELECT, KGCINSERT = :KGCINSERT, KGCUPDATE = :KGCUPDATE, KGCEXIST = :KGCEXIST, KGCCRU = :KGCCRU, KGCCRD = :KGCCRD
, KGCMJU = :KGCMJU, KGCMJD = :KGCMJD, KGCSAID2 = :KGCSAID2, KGCSCID2 = :KGCSCID2
 WHERE 
KGCMODID = :KGCMODID";
            const string delete=@"DELETE FROM KISMOD WHERE KGCMODID = :KGCMODID";
            const string insert=@"INSERT INTO  KISMOD (
KGCMODID, KGCDESC, KGCDATD, KGCDATF, KGCSELECT
, KGCINSERT, KGCUPDATE, KGCEXIST, KGCCRU, KGCCRD
, KGCMJU, KGCMJD, KGCSAID2, KGCSCID2
) VALUES (
:KGCMODID, :KGCDESC, :KGCDATD, :KGCDATF, :KGCSELECT
, :KGCINSERT, :KGCUPDATE, :KGCEXIST, :KGCCRU, :KGCCRD
, :KGCMJU, :KGCMJD, :KGCSAID2, :KGCSCID2)";
            const string select_GetAll=@"SELECT
KGCMODID, KGCDESC, KGCDATD, KGCDATF, KGCSELECT
, KGCINSERT, KGCUPDATE, KGCEXIST, KGCCRU, KGCCRD
, KGCMJU, KGCMJD, KGCSAID2, KGCSCID2 FROM KISMOD
";
            #endregion

            public KISModRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KISMod Get(string KGCMODID){
                return connection.Query<KISMod>(select, new {KGCMODID}).SingleOrDefault();
            }

            public string NewId () {
                return idGenerator.NewId("KGCMODID").ToString() ;
            }

            public void Insert(KISMod value){
                    if(value.Kgcmodid == default(string)) {
                        value.Kgcmodid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KGCMODID",value.Kgcmodid??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KGCDESC",value.Kgcdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KGCDATD",value.Kgcdatd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGCDATF",value.Kgcdatf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGCSELECT",value.Kgcselect??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("KGCINSERT",value.Kgcinsert??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("KGCUPDATE",value.Kgcupdate??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("KGCEXIST",value.Kgcexist??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("KGCCRU",value.Kgccru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGCCRD",value.Kgccrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGCMJU",value.Kgcmju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGCMJD",value.Kgcmjd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGCSAID2",value.Kgcsaid2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KGCSCID2",value.Kgcscid2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KISMod value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KGCMODID",value.Kgcmodid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KISMod value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KGCMODID",value.Kgcmodid??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KGCDESC",value.Kgcdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KGCDATD",value.Kgcdatd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGCDATF",value.Kgcdatf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGCSELECT",value.Kgcselect??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("KGCINSERT",value.Kgcinsert??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("KGCUPDATE",value.Kgcupdate??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("KGCEXIST",value.Kgcexist??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("KGCCRU",value.Kgccru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGCCRD",value.Kgccrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGCMJU",value.Kgcmju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGCMJD",value.Kgcmjd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGCSAID2",value.Kgcsaid2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KGCSCID2",value.Kgcscid2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KGCMODID",value.Kgcmodid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KISMod> GetAll(){
                    return connection.EnsureOpened().Query<KISMod>(select_GetAll).ToList();
            }
    }
}
