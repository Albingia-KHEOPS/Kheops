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

    public  partial class  KpEngRsqRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKSPP
            const string select=@"SELECT
KDRID, KDRTYP, KDRIPB, KDRALX, KDRRSQ
, KDRKDQID, KDRFAM, KDRVEN, KDRLCI, KDRSMP
, KDRENGC, KDRENGF, KDRENGOK, KDRCRU, KDRCRD
, KDRMAJU, KDRMAJD, KDRKDOID, KDRCAT, KDRSMF
 FROM KPENGRSQ
WHERE KDRID = :KDRID
";
            const string update=@"UPDATE KPENGRSQ SET 
KDRID = :KDRID, KDRTYP = :KDRTYP, KDRIPB = :KDRIPB, KDRALX = :KDRALX, KDRRSQ = :KDRRSQ, KDRKDQID = :KDRKDQID, KDRFAM = :KDRFAM, KDRVEN = :KDRVEN, KDRLCI = :KDRLCI, KDRSMP = :KDRSMP
, KDRENGC = :KDRENGC, KDRENGF = :KDRENGF, KDRENGOK = :KDRENGOK, KDRCRU = :KDRCRU, KDRCRD = :KDRCRD, KDRMAJU = :KDRMAJU, KDRMAJD = :KDRMAJD, KDRKDOID = :KDRKDOID, KDRCAT = :KDRCAT, KDRSMF = :KDRSMF

 WHERE 
KDRID = :KDRID";
            const string delete=@"DELETE FROM KPENGRSQ WHERE KDRID = :KDRID";
            const string insert=@"INSERT INTO  KPENGRSQ (
KDRID, KDRTYP, KDRIPB, KDRALX, KDRRSQ
, KDRKDQID, KDRFAM, KDRVEN, KDRLCI, KDRSMP
, KDRENGC, KDRENGF, KDRENGOK, KDRCRU, KDRCRD
, KDRMAJU, KDRMAJD, KDRKDOID, KDRCAT, KDRSMF

) VALUES (
:KDRID, :KDRTYP, :KDRIPB, :KDRALX, :KDRRSQ
, :KDRKDQID, :KDRFAM, :KDRVEN, :KDRLCI, :KDRSMP
, :KDRENGC, :KDRENGF, :KDRENGOK, :KDRCRU, :KDRCRD
, :KDRMAJU, :KDRMAJD, :KDRKDOID, :KDRCAT, :KDRSMF
)";
            const string select_GetByAffaire=@"SELECT
KDRID, KDRTYP, KDRIPB, KDRALX, KDRRSQ
, KDRKDQID, KDRFAM, KDRVEN, KDRLCI, KDRSMP
, KDRENGC, KDRENGF, KDRENGOK, KDRCRU, KDRCRD
, KDRMAJU, KDRMAJD, KDRKDOID, KDRCAT, KDRSMF
 FROM KPENGRSQ
WHERE KDRTYP = :KDRTYP
and KDRIPB = :KDRIPB
and KDRALX = :KDRALX
";
            #endregion

            public KpEngRsqRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpEngRsq Get(Int64 KDRID){
                return connection.Query<KpEngRsq>(select, new {KDRID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KDRID") ;
            }

            public void Insert(KpEngRsq value){
                    if(value.Kdrid == default(Int64)) {
                        value.Kdrid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KDRID",value.Kdrid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDRTYP",value.Kdrtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDRIPB",value.Kdripb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDRALX",value.Kdralx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDRRSQ",value.Kdrrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDRKDQID",value.Kdrkdqid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDRFAM",value.Kdrfam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDRVEN",value.Kdrven, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDRLCI",value.Kdrlci, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDRSMP",value.Kdrsmp, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDRENGC",value.Kdrengc, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDRENGF",value.Kdrengf, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDRENGOK",value.Kdrengok??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDRCRU",value.Kdrcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDRCRD",value.Kdrcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDRMAJU",value.Kdrmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDRMAJD",value.Kdrmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDRKDOID",value.Kdrkdoid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDRCAT",value.Kdrcat, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDRSMF",value.Kdrsmf, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpEngRsq value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDRID",value.Kdrid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpEngRsq value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDRID",value.Kdrid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDRTYP",value.Kdrtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDRIPB",value.Kdripb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDRALX",value.Kdralx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDRRSQ",value.Kdrrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDRKDQID",value.Kdrkdqid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDRFAM",value.Kdrfam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDRVEN",value.Kdrven, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDRLCI",value.Kdrlci, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDRSMP",value.Kdrsmp, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDRENGC",value.Kdrengc, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDRENGF",value.Kdrengf, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDRENGOK",value.Kdrengok??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDRCRU",value.Kdrcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDRCRD",value.Kdrcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDRMAJU",value.Kdrmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDRMAJD",value.Kdrmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDRKDOID",value.Kdrkdoid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDRCAT",value.Kdrcat, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDRSMF",value.Kdrsmf, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDRID",value.Kdrid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpEngRsq> GetByAffaire(string KDRTYP, string KDRIPB, int KDRALX){
                    return connection.EnsureOpened().Query<KpEngRsq>(select_GetByAffaire, new {KDRTYP, KDRIPB, KDRALX}).ToList();
            }
    }
}
