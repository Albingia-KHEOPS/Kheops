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

    public  partial class  KExpFrhDRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KHFID, KHFKHEID, KHFORDRE, KHFFHVAL, KHFFHVAU
, KHFBASE, KHFIND, KHFIVO, KHFFHMINI, KHFFHMAXI
, KHFLIMDEB, KHFLIMFIN FROM KEXPFRHD
WHERE KHFID = :id
";
            const string update=@"UPDATE KEXPFRHD SET 
KHFID = :KHFID, KHFKHEID = :KHFKHEID, KHFORDRE = :KHFORDRE, KHFFHVAL = :KHFFHVAL, KHFFHVAU = :KHFFHVAU, KHFBASE = :KHFBASE, KHFIND = :KHFIND, KHFIVO = :KHFIVO, KHFFHMINI = :KHFFHMINI, KHFFHMAXI = :KHFFHMAXI
, KHFLIMDEB = :KHFLIMDEB, KHFLIMFIN = :KHFLIMFIN
 WHERE 
KHFID = :id";
            const string delete=@"DELETE FROM KEXPFRHD WHERE KHFID = :id";
            const string insert=@"INSERT INTO  KEXPFRHD (
KHFID, KHFKHEID, KHFORDRE, KHFFHVAL, KHFFHVAU
, KHFBASE, KHFIND, KHFIVO, KHFFHMINI, KHFFHMAXI
, KHFLIMDEB, KHFLIMFIN
) VALUES (
:KHFID, :KHFKHEID, :KHFORDRE, :KHFFHVAL, :KHFFHVAU
, :KHFBASE, :KHFIND, :KHFIVO, :KHFFHMINI, :KHFFHMAXI
, :KHFLIMDEB, :KHFLIMFIN)";
            const string select_GetAll=@"SELECT
KHFID, KHFKHEID, KHFORDRE, KHFFHVAL, KHFFHVAU
, KHFBASE, KHFIND, KHFIVO, KHFFHMINI, KHFFHMAXI
, KHFLIMDEB, KHFLIMFIN FROM KEXPFRHD
";
            const string select_GetByExpFranchise=@"SELECT
KHFID, KHFKHEID, KHFORDRE, KHFFHVAL, KHFFHVAU
, KHFBASE, KHFIND, KHFIVO, KHFFHMINI, KHFFHMAXI
, KHFLIMDEB, KHFLIMFIN FROM KEXPFRHD
WHERE KHFKHEID = :parKHFKHEID
";
            #endregion

            public KExpFrhDRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KExpFrhD Get(Int64 id){
                return connection.Query<KExpFrhD>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KHFID") ;
            }

            public void Insert(KExpFrhD value){
                    if(value.Khfid == default(Int64)) {
                        value.Khfid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KHFID",value.Khfid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHFKHEID",value.Khfkheid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHFORDRE",value.Khfordre, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHFFHVAL",value.Khffhval, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHFFHVAU",value.Khffhvau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHFBASE",value.Khfbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHFIND",value.Khfind??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHFIVO",value.Khfivo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KHFFHMINI",value.Khffhmini, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHFFHMAXI",value.Khffhmaxi, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHFLIMDEB",value.Khflimdeb, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHFLIMFIN",value.Khflimfin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KExpFrhD value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Khfid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KExpFrhD value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KHFID",value.Khfid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHFKHEID",value.Khfkheid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHFORDRE",value.Khfordre, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHFFHVAL",value.Khffhval, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHFFHVAU",value.Khffhvau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHFBASE",value.Khfbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHFIND",value.Khfind??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHFIVO",value.Khfivo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KHFFHMINI",value.Khffhmini, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHFFHMAXI",value.Khffhmaxi, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHFLIMDEB",value.Khflimdeb, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHFLIMFIN",value.Khflimfin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("id",value.Khfid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KExpFrhD> GetAll(){
                    return connection.EnsureOpened().Query<KExpFrhD>(select_GetAll).ToList();
            }
            public IEnumerable<KExpFrhD> GetByExpFranchise(Int64 parKHFKHEID){
                    return connection.EnsureOpened().Query<KExpFrhD>(select_GetByExpFranchise, new {parKHFKHEID}).ToList();
            }
    }
}
