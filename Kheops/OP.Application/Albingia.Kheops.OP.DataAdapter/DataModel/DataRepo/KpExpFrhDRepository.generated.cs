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

    public  partial class  KpExpFrhDRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDLID, KDLKDKID, KDLORDRE, KDLFHVAL, KDLFHVAU
, KDLFHBASE, KDLIND, KDLIVO, KDLFHMINI, KDLFHMAXI
, KDLLIMDEB, KDLLIMFIN FROM KPEXPFRHD
WHERE KDLID = :id
";
            const string update=@"UPDATE KPEXPFRHD SET 
KDLID = :KDLID, KDLKDKID = :KDLKDKID, KDLORDRE = :KDLORDRE, KDLFHVAL = :KDLFHVAL, KDLFHVAU = :KDLFHVAU, KDLFHBASE = :KDLFHBASE, KDLIND = :KDLIND, KDLIVO = :KDLIVO, KDLFHMINI = :KDLFHMINI, KDLFHMAXI = :KDLFHMAXI
, KDLLIMDEB = :KDLLIMDEB, KDLLIMFIN = :KDLLIMFIN
 WHERE 
KDLID = :id";
            const string delete=@"DELETE FROM KPEXPFRHD WHERE KDLID = :id";
            const string insert=@"INSERT INTO  KPEXPFRHD (
KDLID, KDLKDKID, KDLORDRE, KDLFHVAL, KDLFHVAU
, KDLFHBASE, KDLIND, KDLIVO, KDLFHMINI, KDLFHMAXI
, KDLLIMDEB, KDLLIMFIN
) VALUES (
:KDLID, :KDLKDKID, :KDLORDRE, :KDLFHVAL, :KDLFHVAU
, :KDLFHBASE, :KDLIND, :KDLIVO, :KDLFHMINI, :KDLFHMAXI
, :KDLLIMDEB, :KDLLIMFIN)";
            const string select_GetByAffaire=@"SELECT
KDLID, KDLKDKID, KDLORDRE, KDLFHVAL, KDLFHVAU
, KDLFHBASE, KDLIND, KDLIVO, KDLFHMINI, KDLFHMAXI
, KDLLIMDEB, KDLLIMFIN FROM KPEXPFRHD
inner join KPEXPFRH ON KDLKDKID = KDKID
WHERE KDKTYP = :typeAffaire
and KDKIPB = :numeroAffaire
and KDKALX = :numeroAliment
";
            #endregion

            public KpExpFrhDRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpExpFrhD Get(Int64 id){
                return connection.Query<KpExpFrhD>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KDLID") ;
            }

            public void Insert(KpExpFrhD value){
                    if(value.Kdlid == default(Int64)) {
                        value.Kdlid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KDLID",value.Kdlid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDLKDKID",value.Kdlkdkid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDLORDRE",value.Kdlordre, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDLFHVAL",value.Kdlfhval, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDLFHVAU",value.Kdlfhvau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDLFHBASE",value.Kdlfhbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDLIND",value.Kdlind??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDLIVO",value.Kdlivo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDLFHMINI",value.Kdlfhmini, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDLFHMAXI",value.Kdlfhmaxi, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDLLIMDEB",value.Kdllimdeb, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDLLIMFIN",value.Kdllimfin, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpExpFrhD value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Kdlid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpExpFrhD value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDLID",value.Kdlid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDLKDKID",value.Kdlkdkid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDLORDRE",value.Kdlordre, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDLFHVAL",value.Kdlfhval, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDLFHVAU",value.Kdlfhvau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDLFHBASE",value.Kdlfhbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDLIND",value.Kdlind??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDLIVO",value.Kdlivo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDLFHMINI",value.Kdlfhmini, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDLFHMAXI",value.Kdlfhmaxi, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDLLIMDEB",value.Kdllimdeb, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDLLIMFIN",value.Kdllimfin, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("id",value.Kdlid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpExpFrhD> GetByAffaire(string typeAffaire, string numeroAffaire, int numeroAliment){
                    return connection.EnsureOpened().Query<KpExpFrhD>(select_GetByAffaire, new {typeAffaire, numeroAffaire, numeroAliment}).ToList();
            }
    }
}
