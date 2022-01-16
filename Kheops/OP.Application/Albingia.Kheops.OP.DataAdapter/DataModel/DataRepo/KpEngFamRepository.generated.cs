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

    public  partial class  KpEngFamRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDPID, KDPTYP, KDPIPB, KDPALX, KDPKDOID
, KDPFAM, KDPENG, KDPENA, KDPCRU, KDPCRD
, KDPMAJU, KDPMAJD, KDPLCT, KDPLCA, KDPCAT
, KDPCAA FROM KPENGFAM
WHERE KDPID = :KDPID
";
            const string update=@"UPDATE KPENGFAM SET 
KDPID = :KDPID, KDPTYP = :KDPTYP, KDPIPB = :KDPIPB, KDPALX = :KDPALX, KDPKDOID = :KDPKDOID, KDPFAM = :KDPFAM, KDPENG = :KDPENG, KDPENA = :KDPENA, KDPCRU = :KDPCRU, KDPCRD = :KDPCRD
, KDPMAJU = :KDPMAJU, KDPMAJD = :KDPMAJD, KDPLCT = :KDPLCT, KDPLCA = :KDPLCA, KDPCAT = :KDPCAT, KDPCAA = :KDPCAA
 WHERE 
KDPID = :KDPID";
            const string delete=@"DELETE FROM KPENGFAM WHERE KDPID = :KDPID";
            const string insert=@"INSERT INTO  KPENGFAM (
KDPID, KDPTYP, KDPIPB, KDPALX, KDPKDOID
, KDPFAM, KDPENG, KDPENA, KDPCRU, KDPCRD
, KDPMAJU, KDPMAJD, KDPLCT, KDPLCA, KDPCAT
, KDPCAA
) VALUES (
:KDPID, :KDPTYP, :KDPIPB, :KDPALX, :KDPKDOID
, :KDPFAM, :KDPENG, :KDPENA, :KDPCRU, :KDPCRD
, :KDPMAJU, :KDPMAJD, :KDPLCT, :KDPLCA, :KDPCAT
, :KDPCAA)";
            const string select_GetByAffaire=@"SELECT
KDPID, KDPTYP, KDPIPB, KDPALX, KDPKDOID
, KDPFAM, KDPENG, KDPENA, KDPCRU, KDPCRD
, KDPMAJU, KDPMAJD, KDPLCT, KDPLCA, KDPCAT
, KDPCAA FROM KPENGFAM
WHERE KDPTYP = :KDPTYP
and KDPIPB = :KDPIPB
and KDPALX = :KDPALX
";
            #endregion

            public KpEngFamRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpEngFam Get(Int64 KDPID){
                return connection.Query<KpEngFam>(select, new {KDPID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KDPID") ;
            }

            public void Insert(KpEngFam value){
                    if(value.Kdpid == default(Int64)) {
                        value.Kdpid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KDPID",value.Kdpid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDPTYP",value.Kdptyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDPIPB",value.Kdpipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDPALX",value.Kdpalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDPKDOID",value.Kdpkdoid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDPFAM",value.Kdpfam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDPENG",value.Kdpeng, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDPENA",value.Kdpena, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDPCRU",value.Kdpcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDPCRD",value.Kdpcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDPMAJU",value.Kdpmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDPMAJD",value.Kdpmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDPLCT",value.Kdplct, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDPLCA",value.Kdplca, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDPCAT",value.Kdpcat, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDPCAA",value.Kdpcaa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpEngFam value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDPID",value.Kdpid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpEngFam value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDPID",value.Kdpid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDPTYP",value.Kdptyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDPIPB",value.Kdpipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDPALX",value.Kdpalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDPKDOID",value.Kdpkdoid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDPFAM",value.Kdpfam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDPENG",value.Kdpeng, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDPENA",value.Kdpena, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDPCRU",value.Kdpcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDPCRD",value.Kdpcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDPMAJU",value.Kdpmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDPMAJD",value.Kdpmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDPLCT",value.Kdplct, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDPLCA",value.Kdplca, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDPCAT",value.Kdpcat, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDPCAA",value.Kdpcaa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDPID",value.Kdpid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpEngFam> GetByAffaire(string KDPTYP, string KDPIPB, int KDPALX){
                    return connection.EnsureOpened().Query<KpEngFam>(select_GetByAffaire, new {KDPTYP, KDPIPB, KDPALX}).ToList();
            }
    }
}
