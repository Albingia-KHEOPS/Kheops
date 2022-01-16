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

    public  partial class  HpexpfrhdRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDLID, KDLAVN, KDLHIN, KDLKDKID, KDLORDRE
, KDLFHVAL, KDLFHVAU, KDLFHBASE, KDLIND, KDLIVO
, KDLFHMINI, KDLFHMAXI, KDLLIMDEB, KDLLIMFIN FROM HPEXPFRHD
WHERE KDLID = :id
and KDLAVN = :numeroAvenant
";
            const string update=@"UPDATE HPEXPFRHD SET 
KDLID = :KDLID, KDLAVN = :KDLAVN, KDLHIN = :KDLHIN, KDLKDKID = :KDLKDKID, KDLORDRE = :KDLORDRE, KDLFHVAL = :KDLFHVAL, KDLFHVAU = :KDLFHVAU, KDLFHBASE = :KDLFHBASE, KDLIND = :KDLIND, KDLIVO = :KDLIVO
, KDLFHMINI = :KDLFHMINI, KDLFHMAXI = :KDLFHMAXI, KDLLIMDEB = :KDLLIMDEB, KDLLIMFIN = :KDLLIMFIN
 WHERE 
KDLID = :id and KDLAVN = :numeroAvenant";
            const string delete=@"DELETE FROM HPEXPFRHD WHERE KDLID = :id AND KDLAVN = :numeroAvenant";
            const string insert=@"INSERT INTO  HPEXPFRHD (
KDLID, KDLAVN, KDLHIN, KDLKDKID, KDLORDRE
, KDLFHVAL, KDLFHVAU, KDLFHBASE, KDLIND, KDLIVO
, KDLFHMINI, KDLFHMAXI, KDLLIMDEB, KDLLIMFIN
) VALUES (
:KDLID, :KDLAVN, :KDLHIN, :KDLKDKID, :KDLORDRE
, :KDLFHVAL, :KDLFHVAU, :KDLFHBASE, :KDLIND, :KDLIVO
, :KDLFHMINI, :KDLFHMAXI, :KDLLIMDEB, :KDLLIMFIN)";
            const string select_GetByAffaire=@"SELECT
KDLID, KDLAVN, KDLHIN, KDLKDKID, KDLORDRE
, KDLFHVAL, KDLFHVAU, KDLFHBASE, KDLIND, KDLIVO
, KDLFHMINI, KDLFHMAXI, KDLLIMDEB, KDLLIMFIN FROM HPEXPFRHD
inner join HPEXPFRH ON KDLKDKID = KDKID and KDLAVN = KDKAVN
WHERE KDKTYP = :typeAffaire
and KDKIPB = :numeroAffaire
and KDKALX = :numeroAliment
and KDKAVN = :numeroAvenant
";
            #endregion

            public HpexpfrhdRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpExpFrhD Get(Int64 id, int numeroAvenant){
                return connection.Query<KpExpFrhD>(select, new {id, numeroAvenant}).SingleOrDefault();
            }


            public void Insert(KpExpFrhD value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDLID",value.Kdlid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDLAVN",value.Kdlavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDLHIN",value.Kdlhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
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
                    parameters.Add("numeroAvenant",value.Kdlavn);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpExpFrhD value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDLID",value.Kdlid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDLAVN",value.Kdlavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDLHIN",value.Kdlhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
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
                    parameters.Add("numeroAvenant",value.Kdlavn);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpExpFrhD> GetByAffaire(string typeAffaire, string numeroAffaire, int numeroAliment, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpExpFrhD>(select_GetByAffaire, new {typeAffaire, numeroAffaire, numeroAliment, numeroAvenant}).ToList();
            }
    }
}
