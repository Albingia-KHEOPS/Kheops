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

    public  partial class  HpexplciRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDIID, KDITYP, KDIIPB, KDIALX, KDIAVN
, KDIHIN, KDILCE, KDIDESC, KDIDESI, KDIORI
, KDIMODI FROM HPEXPLCI
WHERE KDIID = :id
and KDIAVN = :numeroAvenant
";
            const string update=@"UPDATE HPEXPLCI SET 
KDIID = :KDIID, KDITYP = :KDITYP, KDIIPB = :KDIIPB, KDIALX = :KDIALX, KDIAVN = :KDIAVN, KDIHIN = :KDIHIN, KDILCE = :KDILCE, KDIDESC = :KDIDESC, KDIDESI = :KDIDESI, KDIORI = :KDIORI
, KDIMODI = :KDIMODI
 WHERE 
KDIID = :id and KDIAVN = :numeroAvenant";
            const string delete=@"DELETE FROM HPEXPLCI WHERE KDIID = :id AND KDIAVN = :numeroAvenant";
            const string insert=@"INSERT INTO  HPEXPLCI (
KDIID, KDITYP, KDIIPB, KDIALX, KDIAVN
, KDIHIN, KDILCE, KDIDESC, KDIDESI, KDIORI
, KDIMODI
) VALUES (
:KDIID, :KDITYP, :KDIIPB, :KDIALX, :KDIAVN
, :KDIHIN, :KDILCE, :KDIDESC, :KDIDESI, :KDIORI
, :KDIMODI)";
            const string select_GetByAffaire=@"SELECT
KDIID, KDITYP, KDIIPB, KDIALX, KDIAVN
, KDIHIN, KDILCE, KDIDESC, KDIDESI, KDIORI
, KDIMODI FROM HPEXPLCI
WHERE KDITYP = :typeAffaire
and KDIIPB = :numeroAffaire
and KDIALX = :numeroAliment
and KDIAVN = :numeroAvenant
";
            #endregion

            public HpexplciRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpExpLCI Get(Int64 id, int numeroAvenant){
                return connection.Query<KpExpLCI>(select, new {id, numeroAvenant}).SingleOrDefault();
            }


            public void Insert(KpExpLCI value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDIID",value.Kdiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDITYP",value.Kdityp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDIIPB",value.Kdiipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDIALX",value.Kdialx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDIAVN",value.Kdiavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDIHIN",value.Kdihin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
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
                    parameters.Add("numeroAvenant",value.Kdiavn);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpExpLCI value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDIID",value.Kdiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDITYP",value.Kdityp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDIIPB",value.Kdiipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDIALX",value.Kdialx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDIAVN",value.Kdiavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDIHIN",value.Kdihin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDILCE",value.Kdilce??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDIDESC",value.Kdidesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KDIDESI",value.Kdidesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDIORI",value.Kdiori??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDIMODI",value.Kdimodi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("id",value.Kdiid);
                    parameters.Add("numeroAvenant",value.Kdiavn);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpExpLCI> GetByAffaire(string typeAffaire, string numeroAffaire, int numeroAliment, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpExpLCI>(select_GetByAffaire, new {typeAffaire, numeroAffaire, numeroAliment, numeroAvenant}).ToList();
            }
    }
}
