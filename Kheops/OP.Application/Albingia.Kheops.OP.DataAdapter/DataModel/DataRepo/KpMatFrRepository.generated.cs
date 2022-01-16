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

    public  partial class  KpMatFrRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KEBTYP, KEBIPB, KEBALX, KEBCHR, KEBTYE
, KEBRSQ, KEBOBJ, KEBINV, KEBVID FROM KPMATFR
WHERE KEBTYP = :KEBTYP
and KEBIPB = :KEBIPB
and KEBALX = :KEBALX
and KEBCHR = :KEBCHR
";
            const string update=@"UPDATE KPMATFR SET 
KEBTYP = :KEBTYP, KEBIPB = :KEBIPB, KEBALX = :KEBALX, KEBCHR = :KEBCHR, KEBTYE = :KEBTYE, KEBRSQ = :KEBRSQ, KEBOBJ = :KEBOBJ, KEBINV = :KEBINV, KEBVID = :KEBVID
 WHERE 
KEBTYP = :KEBTYP and KEBIPB = :KEBIPB and KEBALX = :KEBALX and KEBCHR = :KEBCHR";
            const string delete=@"DELETE FROM KPMATFR WHERE KEBTYP = :KEBTYP AND KEBIPB = :KEBIPB AND KEBALX = :KEBALX AND KEBCHR = :KEBCHR";
            const string insert=@"INSERT INTO  KPMATFR (
KEBTYP, KEBIPB, KEBALX, KEBCHR, KEBTYE
, KEBRSQ, KEBOBJ, KEBINV, KEBVID
) VALUES (
:KEBTYP, :KEBIPB, :KEBALX, :KEBCHR, :KEBTYE
, :KEBRSQ, :KEBOBJ, :KEBINV, :KEBVID)";
            const string select_GetByAffaire=@"SELECT
KEBTYP, KEBIPB, KEBALX, KEBCHR, KEBTYE
, KEBRSQ, KEBOBJ, KEBINV, KEBVID FROM KPMATFR
WHERE KEBTYP = :KEBTYP
and KEBIPB = :KEBIPB
and KEBALX = :KEBALX
";
            #endregion

            public KpMatFrRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpMatFr Get(string KEBTYP, string KEBIPB, int KEBALX, int KEBCHR){
                return connection.Query<KpMatFr>(select, new {KEBTYP, KEBIPB, KEBALX, KEBCHR}).SingleOrDefault();
            }


            public void Insert(KpMatFr value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEBTYP",value.Kebtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEBIPB",value.Kebipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KEBALX",value.Kebalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEBCHR",value.Kebchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEBTYE",value.Kebtye??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEBRSQ",value.Kebrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEBOBJ",value.Kebobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEBINV",value.Kebinv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEBVID",value.Kebvid??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpMatFr value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEBTYP",value.Kebtyp);
                    parameters.Add("KEBIPB",value.Kebipb);
                    parameters.Add("KEBALX",value.Kebalx);
                    parameters.Add("KEBCHR",value.Kebchr);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpMatFr value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEBTYP",value.Kebtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEBIPB",value.Kebipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KEBALX",value.Kebalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEBCHR",value.Kebchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEBTYE",value.Kebtye??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEBRSQ",value.Kebrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEBOBJ",value.Kebobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEBINV",value.Kebinv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEBVID",value.Kebvid??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEBTYP",value.Kebtyp);
                    parameters.Add("KEBIPB",value.Kebipb);
                    parameters.Add("KEBALX",value.Kebalx);
                    parameters.Add("KEBCHR",value.Kebchr);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpMatFr> GetByAffaire(string KEBTYP, string KEBIPB, int KEBALX){
                    return connection.EnsureOpened().Query<KpMatFr>(select_GetByAffaire, new {KEBTYP, KEBIPB, KEBALX}).ToList();
            }
    }
}
