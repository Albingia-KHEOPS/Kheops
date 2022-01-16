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

    public  partial class  YhrtobtRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KFIPB, KFALX, KFAVN, KFHIN, KFRSQ
, KFOBJ, KFDUV, KFDUU, KFDDA, KFDDM
, KFDDJ, KFDFA, KFDFM, KFDFJ, KFESV
, KFESU, KFEDA, KFEDM, KFEDJ, KFEFA
, KFEFM, KFEFJ, KFTDF FROM YHRTOBT
WHERE KFIPB = :KFIPB
and KFALX = :KFALX
and KFAVN = :KFAVN
and KFHIN = :KFHIN
and KFRSQ = :KFRSQ
and KFOBJ = :KFOBJ
";
            const string update=@"UPDATE YHRTOBT SET 
KFIPB = :KFIPB, KFALX = :KFALX, KFAVN = :KFAVN, KFHIN = :KFHIN, KFRSQ = :KFRSQ, KFOBJ = :KFOBJ, KFDUV = :KFDUV, KFDUU = :KFDUU, KFDDA = :KFDDA, KFDDM = :KFDDM
, KFDDJ = :KFDDJ, KFDFA = :KFDFA, KFDFM = :KFDFM, KFDFJ = :KFDFJ, KFESV = :KFESV, KFESU = :KFESU, KFEDA = :KFEDA, KFEDM = :KFEDM, KFEDJ = :KFEDJ, KFEFA = :KFEFA
, KFEFM = :KFEFM, KFEFJ = :KFEFJ, KFTDF = :KFTDF
 WHERE 
KFIPB = :KFIPB and KFALX = :KFALX and KFAVN = :KFAVN and KFHIN = :KFHIN and KFRSQ = :KFRSQ and KFOBJ = :KFOBJ";
            const string delete=@"DELETE FROM YHRTOBT WHERE KFIPB = :KFIPB AND KFALX = :KFALX AND KFAVN = :KFAVN AND KFHIN = :KFHIN AND KFRSQ = :KFRSQ AND KFOBJ = :KFOBJ";
            const string insert=@"INSERT INTO  YHRTOBT (
KFIPB, KFALX, KFAVN, KFHIN, KFRSQ
, KFOBJ, KFDUV, KFDUU, KFDDA, KFDDM
, KFDDJ, KFDFA, KFDFM, KFDFJ, KFESV
, KFESU, KFEDA, KFEDM, KFEDJ, KFEFA
, KFEFM, KFEFJ, KFTDF
) VALUES (
:KFIPB, :KFALX, :KFAVN, :KFHIN, :KFRSQ
, :KFOBJ, :KFDUV, :KFDUU, :KFDDA, :KFDDM
, :KFDDJ, :KFDFA, :KFDFM, :KFDFJ, :KFESV
, :KFESU, :KFEDA, :KFEDM, :KFEDJ, :KFEFA
, :KFEFM, :KFEFJ, :KFTDF)";
            const string select_GetByAffaire=@"SELECT
KFIPB, KFALX, KFAVN, KFHIN, KFRSQ
, KFOBJ, KFDUV, KFDUU, KFDDA, KFDDM
, KFDDJ, KFDFA, KFDFM, KFDFJ, KFESV
, KFESU, KFEDA, KFEDM, KFEDJ, KFEFA
, KFEFM, KFEFJ, KFTDF FROM YHRTOBT
WHERE KFIPB = :KFIPB
and KFALX = :KFALX
and KFAVN = :KFAVN
";
            #endregion

            public YhrtobtRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YprtObt Get(string KFIPB, int KFALX, int KFAVN, int KFHIN, int KFRSQ, int KFOBJ){
                return connection.Query<YprtObt>(select, new {KFIPB, KFALX, KFAVN, KFHIN, KFRSQ, KFOBJ}).SingleOrDefault();
            }


            public void Insert(YprtObt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFIPB",value.Kfipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFALX",value.Kfalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFAVN",value.Kfavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFHIN",value.Kfhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFRSQ",value.Kfrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFOBJ",value.Kfobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDUV",value.Kfduv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFDUU",value.Kfduu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFDDA",value.Kfdda, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFDDM",value.Kfddm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFDDJ",value.Kfddj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFDFA",value.Kfdfa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFDFM",value.Kfdfm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFDFJ",value.Kfdfj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFESV",value.Kfesv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFESU",value.Kfesu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFEDA",value.Kfeda, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFEDM",value.Kfedm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFEDJ",value.Kfedj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFEFA",value.Kfefa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFEFM",value.Kfefm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFEFJ",value.Kfefj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFTDF",value.Kftdf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YprtObt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFIPB",value.Kfipb);
                    parameters.Add("KFALX",value.Kfalx);
                    parameters.Add("KFAVN",value.Kfavn);
                    parameters.Add("KFHIN",value.Kfhin);
                    parameters.Add("KFRSQ",value.Kfrsq);
                    parameters.Add("KFOBJ",value.Kfobj);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YprtObt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFIPB",value.Kfipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFALX",value.Kfalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFAVN",value.Kfavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFHIN",value.Kfhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFRSQ",value.Kfrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFOBJ",value.Kfobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDUV",value.Kfduv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFDUU",value.Kfduu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFDDA",value.Kfdda, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFDDM",value.Kfddm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFDDJ",value.Kfddj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFDFA",value.Kfdfa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFDFM",value.Kfdfm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFDFJ",value.Kfdfj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFESV",value.Kfesv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFESU",value.Kfesu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFEDA",value.Kfeda, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFEDM",value.Kfedm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFEDJ",value.Kfedj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFEFA",value.Kfefa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFEFM",value.Kfefm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFEFJ",value.Kfefj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFTDF",value.Kftdf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFIPB",value.Kfipb);
                    parameters.Add("KFALX",value.Kfalx);
                    parameters.Add("KFAVN",value.Kfavn);
                    parameters.Add("KFHIN",value.Kfhin);
                    parameters.Add("KFRSQ",value.Kfrsq);
                    parameters.Add("KFOBJ",value.Kfobj);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YprtObt> GetByAffaire(string KFIPB, int KFALX, int KFAVN){
                    return connection.EnsureOpened().Query<YprtObt>(select_GetByAffaire, new {KFIPB, KFALX, KFAVN}).ToList();
            }
    }
}
