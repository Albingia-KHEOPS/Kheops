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

    public  partial class  KpExpLCIRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDIID, KDITYP, KDIIPB, KDIALX, KDILCE
, KDIDESC, KDIDESI, KDIORI, KDIMODI FROM KPEXPLCI
WHERE KDIID = :id
";
            const string update=@"UPDATE KPEXPLCI SET 
KDIID = :KDIID, KDITYP = :KDITYP, KDIIPB = :KDIIPB, KDIALX = :KDIALX, KDILCE = :KDILCE, KDIDESC = :KDIDESC, KDIDESI = :KDIDESI, KDIORI = :KDIORI, KDIMODI = :KDIMODI
 WHERE 
KDIID = :id";
            const string delete=@"DELETE FROM KPEXPLCI WHERE KDIID = :id";
            const string insert=@"INSERT INTO  KPEXPLCI (
KDIID, KDITYP, KDIIPB, KDIALX, KDILCE
, KDIDESC, KDIDESI, KDIORI, KDIMODI
) VALUES (
:KDIID, :KDITYP, :KDIIPB, :KDIALX, :KDILCE
, :KDIDESC, :KDIDESI, :KDIORI, :KDIMODI)";
            const string select_GetByAffaire=@"SELECT
KDIID, KDITYP, KDIIPB, KDIALX, KDILCE
, KDIDESC, KDIDESI, KDIORI, KDIMODI FROM KPEXPLCI
WHERE KDITYP = :typeAffaire
and KDIIPB = :numeroAffaire
and KDIALX = :numeroAliment
";
            #endregion

            public KpExpLCIRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpExpLCI Get(Int64 id){
                return connection.Query<KpExpLCI>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KDIID") ;
            }

            public void Insert(KpExpLCI value){
                    if(value.Kdiid == default(Int64)) {
                        value.Kdiid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KDIID",value.Kdiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDITYP",value.Kdityp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDIIPB",value.Kdiipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDIALX",value.Kdialx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDILCE",value.Kdilce??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDIDESC",value.Kdidesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KDIDESI",value.Kdidesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDIORI",value.Kdiori??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDIMODI",value.Kdimodi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpExpLCI value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Kdiid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpExpLCI value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDIID",value.Kdiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDITYP",value.Kdityp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDIIPB",value.Kdiipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDIALX",value.Kdialx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDILCE",value.Kdilce??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDIDESC",value.Kdidesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KDIDESI",value.Kdidesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDIORI",value.Kdiori??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDIMODI",value.Kdimodi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("id",value.Kdiid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpExpLCI> GetByAffaire(string typeAffaire, string numeroAffaire, int numeroAliment){
                    return connection.EnsureOpened().Query<KpExpLCI>(select_GetByAffaire, new {typeAffaire, numeroAffaire, numeroAliment}).ToList();
            }
    }
}
