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

    public  partial class  KpMatFfRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KEATYP, KEAIPB, KEAALX, KEACHR, KEAFOR
, KEAOPT, KEAKDBID, KEAKDAID FROM KPMATFF
WHERE KEATYP = :KEATYP
and KEAIPB = :KEAIPB
and KEAALX = :KEAALX
and KEACHR = :KEACHR
";
            const string update=@"UPDATE KPMATFF SET 
KEATYP = :KEATYP, KEAIPB = :KEAIPB, KEAALX = :KEAALX, KEACHR = :KEACHR, KEAFOR = :KEAFOR, KEAOPT = :KEAOPT, KEAKDBID = :KEAKDBID, KEAKDAID = :KEAKDAID
 WHERE 
KEATYP = :KEATYP and KEAIPB = :KEAIPB and KEAALX = :KEAALX and KEACHR = :KEACHR";
            const string delete=@"DELETE FROM KPMATFF WHERE KEATYP = :KEATYP AND KEAIPB = :KEAIPB AND KEAALX = :KEAALX AND KEACHR = :KEACHR";
            const string insert=@"INSERT INTO  KPMATFF (
KEATYP, KEAIPB, KEAALX, KEACHR, KEAFOR
, KEAOPT, KEAKDBID, KEAKDAID
) VALUES (
:KEATYP, :KEAIPB, :KEAALX, :KEACHR, :KEAFOR
, :KEAOPT, :KEAKDBID, :KEAKDAID)";
            const string select_GetByAffaire=@"SELECT
KEATYP, KEAIPB, KEAALX, KEACHR, KEAFOR
, KEAOPT, KEAKDBID, KEAKDAID FROM KPMATFF
WHERE KEATYP = :KEATYP
and KEAIPB = :KEAIPB
and KEAALX = :KEAALX
";
            #endregion

            public KpMatFfRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpMatFf Get(string KEATYP, string KEAIPB, int KEAALX, int KEACHR){
                return connection.Query<KpMatFf>(select, new {KEATYP, KEAIPB, KEAALX, KEACHR}).SingleOrDefault();
            }


            public void Insert(KpMatFf value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEATYP",value.Keatyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEAIPB",value.Keaipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KEAALX",value.Keaalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEACHR",value.Keachr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEAFOR",value.Keafor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEAOPT",value.Keaopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEAKDBID",value.Keakdbid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEAKDAID",value.Keakdaid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpMatFf value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEATYP",value.Keatyp);
                    parameters.Add("KEAIPB",value.Keaipb);
                    parameters.Add("KEAALX",value.Keaalx);
                    parameters.Add("KEACHR",value.Keachr);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpMatFf value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEATYP",value.Keatyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEAIPB",value.Keaipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KEAALX",value.Keaalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEACHR",value.Keachr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEAFOR",value.Keafor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEAOPT",value.Keaopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEAKDBID",value.Keakdbid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEAKDAID",value.Keakdaid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEATYP",value.Keatyp);
                    parameters.Add("KEAIPB",value.Keaipb);
                    parameters.Add("KEAALX",value.Keaalx);
                    parameters.Add("KEACHR",value.Keachr);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpMatFf> GetByAffaire(string KEATYP, string KEAIPB, int KEAALX){
                    return connection.EnsureOpened().Query<KpMatFf>(select_GetByAffaire, new {KEATYP, KEAIPB, KEAALX}).ToList();
            }
    }
}
