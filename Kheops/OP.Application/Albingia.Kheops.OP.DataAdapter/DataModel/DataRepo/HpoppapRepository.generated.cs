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

    public  partial class  HpoppapRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KFQID, KFQTYP, KFQIPB, KFQALX, KFQAVN
, KFQHIN, KFQKFPID, KFQPERI, KFQRSQ, KFQOBJ
, KFQCRU, KFQCRD, KFQMAJU, KFQMAJD FROM HPOPPAP
WHERE KFQID = :KFQID
and KFQAVN = :KFQAVN
and KFQHIN = :KFQHIN
";
            const string update=@"UPDATE HPOPPAP SET 
KFQID = :KFQID, KFQTYP = :KFQTYP, KFQIPB = :KFQIPB, KFQALX = :KFQALX, KFQAVN = :KFQAVN, KFQHIN = :KFQHIN, KFQKFPID = :KFQKFPID, KFQPERI = :KFQPERI, KFQRSQ = :KFQRSQ, KFQOBJ = :KFQOBJ
, KFQCRU = :KFQCRU, KFQCRD = :KFQCRD, KFQMAJU = :KFQMAJU, KFQMAJD = :KFQMAJD
 WHERE 
KFQID = :KFQID and KFQAVN = :KFQAVN and KFQHIN = :KFQHIN";
            const string delete=@"DELETE FROM HPOPPAP WHERE KFQID = :KFQID AND KFQAVN = :KFQAVN AND KFQHIN = :KFQHIN";
            const string insert=@"INSERT INTO  HPOPPAP (
KFQID, KFQTYP, KFQIPB, KFQALX, KFQAVN
, KFQHIN, KFQKFPID, KFQPERI, KFQRSQ, KFQOBJ
, KFQCRU, KFQCRD, KFQMAJU, KFQMAJD
) VALUES (
:KFQID, :KFQTYP, :KFQIPB, :KFQALX, :KFQAVN
, :KFQHIN, :KFQKFPID, :KFQPERI, :KFQRSQ, :KFQOBJ
, :KFQCRU, :KFQCRD, :KFQMAJU, :KFQMAJD)";
            const string select_GetByAffaire=@"SELECT
KFQID, KFQTYP, KFQIPB, KFQALX, KFQAVN
, KFQHIN, KFQKFPID, KFQPERI, KFQRSQ, KFQOBJ
, KFQCRU, KFQCRD, KFQMAJU, KFQMAJD FROM HPOPPAP
WHERE KFQTYP = :KFQTYP
and KFQIPB = :KFQIPB
and KFQALX = :KFQALX
and KFQAVN = :KFQAVN
";
            #endregion

            public HpoppapRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpOppAp Get(Int64 KFQID, int KFQAVN, int KFQHIN){
                return connection.Query<KpOppAp>(select, new {KFQID, KFQAVN, KFQHIN}).SingleOrDefault();
            }


            public void Insert(KpOppAp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFQID",value.Kfqid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFQTYP",value.Kfqtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFQIPB",value.Kfqipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFQALX",value.Kfqalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFQAVN",value.Kfqavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFQHIN",value.Kfqhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFQKFPID",value.Kfqkfpid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFQPERI",value.Kfqperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFQRSQ",value.Kfqrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFQOBJ",value.Kfqobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFQCRU",value.Kfqcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFQCRD",value.Kfqcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFQMAJU",value.Kfqmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFQMAJD",value.Kfqmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpOppAp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFQID",value.Kfqid);
                    parameters.Add("KFQAVN",value.Kfqavn);
                    parameters.Add("KFQHIN",value.Kfqhin);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpOppAp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFQID",value.Kfqid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFQTYP",value.Kfqtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFQIPB",value.Kfqipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFQALX",value.Kfqalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFQAVN",value.Kfqavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFQHIN",value.Kfqhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFQKFPID",value.Kfqkfpid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFQPERI",value.Kfqperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFQRSQ",value.Kfqrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFQOBJ",value.Kfqobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFQCRU",value.Kfqcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFQCRD",value.Kfqcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFQMAJU",value.Kfqmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFQMAJD",value.Kfqmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFQID",value.Kfqid);
                    parameters.Add("KFQAVN",value.Kfqavn);
                    parameters.Add("KFQHIN",value.Kfqhin);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpOppAp> GetByAffaire(string KFQTYP, string KFQIPB, int KFQALX, int KFQAVN){
                    return connection.EnsureOpened().Query<KpOppAp>(select_GetByAffaire, new {KFQTYP, KFQIPB, KFQALX, KFQAVN}).ToList();
            }
    }
}
