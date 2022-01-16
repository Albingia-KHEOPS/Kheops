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

    public  partial class  KFiltreRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KGGID, KGGFILT, KGGDESC, KGGCRU, KGGCRD
, KGGCRH, KGGMAJU, KGGMAJD, KGGMAJH FROM KFILTRE
WHERE KGGID = :parKGGID
";
            const string update=@"UPDATE KFILTRE SET 
KGGID = :KGGID, KGGFILT = :KGGFILT, KGGDESC = :KGGDESC, KGGCRU = :KGGCRU, KGGCRD = :KGGCRD, KGGCRH = :KGGCRH, KGGMAJU = :KGGMAJU, KGGMAJD = :KGGMAJD, KGGMAJH = :KGGMAJH
 WHERE 
KGGID = :parKGGID";
            const string delete=@"DELETE FROM KFILTRE WHERE KGGID = :parKGGID";
            const string insert=@"INSERT INTO  KFILTRE (
KGGID, KGGFILT, KGGDESC, KGGCRU, KGGCRD
, KGGCRH, KGGMAJU, KGGMAJD, KGGMAJH
) VALUES (
:KGGID, :KGGFILT, :KGGDESC, :KGGCRU, :KGGCRD
, :KGGCRH, :KGGMAJU, :KGGMAJD, :KGGMAJH)";
            const string select_GetAll=@"SELECT
KGGID, KGGFILT, KGGDESC, KGGCRU, KGGCRD
, KGGCRH, KGGMAJU, KGGMAJD, KGGMAJH FROM KFILTRE
";
            #endregion

            public KFiltreRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KFiltre Get(Int64 parKGGID){
                return connection.Query<KFiltre>(select, new {parKGGID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KGGID") ;
            }

            public void Insert(KFiltre value){
                    if(value.Kggid == default(Int64)) {
                        value.Kggid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KGGID",value.Kggid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KGGFILT",value.Kggfilt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGGDESC",value.Kggdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KGGCRU",value.Kggcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGGCRD",value.Kggcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KGGCRH",value.Kggcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KGGMAJU",value.Kggmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGGMAJD",value.Kggmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KGGMAJH",value.Kggmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KFiltre value){
                    var parameters = new DynamicParameters();
                    parameters.Add("parKGGID",value.Kggid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KFiltre value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KGGID",value.Kggid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KGGFILT",value.Kggfilt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGGDESC",value.Kggdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KGGCRU",value.Kggcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGGCRD",value.Kggcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KGGCRH",value.Kggcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KGGMAJU",value.Kggmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGGMAJD",value.Kggmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KGGMAJH",value.Kggmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("parKGGID",value.Kggid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KFiltre> GetAll(){
                    return connection.EnsureOpened().Query<KFiltre>(select_GetAll).ToList();
            }
    }
}
