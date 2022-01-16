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

    public  partial class  KpEngVenRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDQID, KDQTYP, KDQIPB, KDQALX, KDQKDPID
, KDQFAM, KDQVEN, KDQENGC, KDQENGF, KDQENGOK
, KDQCRU, KDQCRD, KDQMAJU, KDQMAJD, KDQLCT
, KDQCAT, KDQKDOID FROM KPENGVEN
WHERE KDQID = :KDQID
";
            const string update=@"UPDATE KPENGVEN SET 
KDQID = :KDQID, KDQTYP = :KDQTYP, KDQIPB = :KDQIPB, KDQALX = :KDQALX, KDQKDPID = :KDQKDPID, KDQFAM = :KDQFAM, KDQVEN = :KDQVEN, KDQENGC = :KDQENGC, KDQENGF = :KDQENGF, KDQENGOK = :KDQENGOK
, KDQCRU = :KDQCRU, KDQCRD = :KDQCRD, KDQMAJU = :KDQMAJU, KDQMAJD = :KDQMAJD, KDQLCT = :KDQLCT, KDQCAT = :KDQCAT, KDQKDOID = :KDQKDOID
 WHERE 
KDQID = :KDQID";
            const string delete=@"DELETE FROM KPENGVEN WHERE KDQID = :KDQID";
            const string insert=@"INSERT INTO  KPENGVEN (
KDQID, KDQTYP, KDQIPB, KDQALX, KDQKDPID
, KDQFAM, KDQVEN, KDQENGC, KDQENGF, KDQENGOK
, KDQCRU, KDQCRD, KDQMAJU, KDQMAJD, KDQLCT
, KDQCAT, KDQKDOID
) VALUES (
:KDQID, :KDQTYP, :KDQIPB, :KDQALX, :KDQKDPID
, :KDQFAM, :KDQVEN, :KDQENGC, :KDQENGF, :KDQENGOK
, :KDQCRU, :KDQCRD, :KDQMAJU, :KDQMAJD, :KDQLCT
, :KDQCAT, :KDQKDOID)";
            const string select_GetByAffaire=@"SELECT
KDQID, KDQTYP, KDQIPB, KDQALX, KDQKDPID
, KDQFAM, KDQVEN, KDQENGC, KDQENGF, KDQENGOK
, KDQCRU, KDQCRD, KDQMAJU, KDQMAJD, KDQLCT
, KDQCAT, KDQKDOID FROM KPENGVEN
WHERE KDQTYP = :KDQTYP
and KDQIPB = :KDQIPB
and KDQALX = :KDQALX
";
            #endregion

            public KpEngVenRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpEngVen Get(Int64 KDQID){
                return connection.Query<KpEngVen>(select, new {KDQID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KDQID") ;
            }

            public void Insert(KpEngVen value){
                    if(value.Kdqid == default(Int64)) {
                        value.Kdqid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KDQID",value.Kdqid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDQTYP",value.Kdqtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDQIPB",value.Kdqipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDQALX",value.Kdqalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDQKDPID",value.Kdqkdpid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDQFAM",value.Kdqfam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDQVEN",value.Kdqven, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDQENGC",value.Kdqengc, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDQENGF",value.Kdqengf, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDQENGOK",value.Kdqengok??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDQCRU",value.Kdqcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDQCRD",value.Kdqcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDQMAJU",value.Kdqmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDQMAJD",value.Kdqmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDQLCT",value.Kdqlct, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDQCAT",value.Kdqcat, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDQKDOID",value.Kdqkdoid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpEngVen value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDQID",value.Kdqid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpEngVen value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDQID",value.Kdqid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDQTYP",value.Kdqtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDQIPB",value.Kdqipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDQALX",value.Kdqalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDQKDPID",value.Kdqkdpid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDQFAM",value.Kdqfam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDQVEN",value.Kdqven, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDQENGC",value.Kdqengc, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDQENGF",value.Kdqengf, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDQENGOK",value.Kdqengok??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDQCRU",value.Kdqcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDQCRD",value.Kdqcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDQMAJU",value.Kdqmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDQMAJD",value.Kdqmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDQLCT",value.Kdqlct, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDQCAT",value.Kdqcat, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDQKDOID",value.Kdqkdoid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDQID",value.Kdqid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpEngVen> GetByAffaire(string KDQTYP, string KDQIPB, int KDQALX){
                    return connection.EnsureOpened().Query<KpEngVen>(select_GetByAffaire, new {KDQTYP, KDQIPB, KDQALX}).ToList();
            }
    }
}
