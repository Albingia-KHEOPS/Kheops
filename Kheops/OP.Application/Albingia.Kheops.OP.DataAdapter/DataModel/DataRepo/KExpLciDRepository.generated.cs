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

    public  partial class  KExpLciDRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KHHID, KHHKHGID, KHHORDRE, KHHLCVAL, KHHLCVAU
, KHHLCBASE, KHHLOVAL, KHHLOVAU, KHHLOBASE FROM KEXPLCID
";
            const string update=@"UPDATE KEXPLCID SET 
KHHID = :KHHID, KHHKHGID = :KHHKHGID, KHHORDRE = :KHHORDRE, KHHLCVAL = :KHHLCVAL, KHHLCVAU = :KHHLCVAU, KHHLCBASE = :KHHLCBASE, KHHLOVAL = :KHHLOVAL, KHHLOVAU = :KHHLOVAU, KHHLOBASE = :KHHLOBASE
 WHERE 
";
            const string delete=@"DELETE FROM KEXPLCID WHERE ";
            const string insert=@"INSERT INTO  KEXPLCID (
KHHID, KHHKHGID, KHHORDRE, KHHLCVAL, KHHLCVAU
, KHHLCBASE, KHHLOVAL, KHHLOVAU, KHHLOBASE
) VALUES (
:KHHID, :KHHKHGID, :KHHORDRE, :KHHLCVAL, :KHHLCVAU
, :KHHLCBASE, :KHHLOVAL, :KHHLOVAU, :KHHLOBASE)";
            const string select_GetAll=@"SELECT
KHHID, KHHKHGID, KHHORDRE, KHHLCVAL, KHHLCVAU
, KHHLCBASE, KHHLOVAL, KHHLOVAU, KHHLOBASE FROM KEXPLCID
";
            const string select_GetByExpLci=@"SELECT
KHHID, KHHKHGID, KHHORDRE, KHHLCVAL, KHHLCVAU
, KHHLCBASE, KHHLOVAL, KHHLOVAU, KHHLOBASE FROM KEXPLCID
WHERE KHHKHGID = :KHHKHGID
";
            #endregion

            public KExpLciDRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KExpLciD Get(){
                return connection.Query<KExpLciD>(select, new {}).SingleOrDefault();
            }


            public void Insert(KExpLciD value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KHHID",value.Khhid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHHKHGID",value.Khhkhgid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHHORDRE",value.Khhordre, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHHLCVAL",value.Khhlcval, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHHLCVAU",value.Khhlcvau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHHLCBASE",value.Khhlcbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHHLOVAL",value.Khhloval, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHHLOVAU",value.Khhlovau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHHLOBASE",value.Khhlobase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KExpLciD value){
                    var parameters = new DynamicParameters();
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KExpLciD value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KHHID",value.Khhid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHHKHGID",value.Khhkhgid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHHORDRE",value.Khhordre, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHHLCVAL",value.Khhlcval, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHHLCVAU",value.Khhlcvau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHHLCBASE",value.Khhlcbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHHLOVAL",value.Khhloval, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHHLOVAU",value.Khhlovau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHHLOBASE",value.Khhlobase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KExpLciD> GetAll(){
                    return connection.EnsureOpened().Query<KExpLciD>(select_GetAll).ToList();
            }
            public IEnumerable<KExpLciD> GetByExpLci(Int64 KHHKHGID){
                    return connection.EnsureOpened().Query<KExpLciD>(select_GetByExpLci, new {KHHKHGID}).ToList();
            }
    }
}
