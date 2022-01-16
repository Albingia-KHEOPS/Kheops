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

    public  partial class  KpMatGrRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KEDTYP, KEDIPB, KEDALX, KEDCHR, KEDRSQ
 FROM KPMATGR
WHERE KEDTYP = :KEDTYP
and KEDIPB = :KEDIPB
and KEDALX = :KEDALX
and KEDCHR = :KEDCHR
";
            const string update=@"UPDATE KPMATGR SET 
KEDTYP = :KEDTYP, KEDIPB = :KEDIPB, KEDALX = :KEDALX, KEDCHR = :KEDCHR, KEDRSQ = :KEDRSQ
 WHERE 
KEDTYP = :KEDTYP and KEDIPB = :KEDIPB and KEDALX = :KEDALX and KEDCHR = :KEDCHR";
            const string delete=@"DELETE FROM KPMATGR WHERE KEDTYP = :KEDTYP AND KEDIPB = :KEDIPB AND KEDALX = :KEDALX AND KEDCHR = :KEDCHR";
            const string insert=@"INSERT INTO  KPMATGR (
KEDTYP, KEDIPB, KEDALX, KEDCHR, KEDRSQ

) VALUES (
:KEDTYP, :KEDIPB, :KEDALX, :KEDCHR, :KEDRSQ
)";
            const string select_GetByAffaire=@"SELECT
KEDTYP, KEDIPB, KEDALX, KEDCHR, KEDRSQ
 FROM KPMATGR
WHERE KEDTYP = :KEDTYP
and KEDIPB = :KEDIPB
and KEDALX = :KEDALX
";
            #endregion

            public KpMatGrRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpMatGr Get(string KEDTYP, string KEDIPB, int KEDALX, int KEDCHR){
                return connection.Query<KpMatGr>(select, new {KEDTYP, KEDIPB, KEDALX, KEDCHR}).SingleOrDefault();
            }


            public void Insert(KpMatGr value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEDTYP",value.Kedtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEDIPB",value.Kedipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KEDALX",value.Kedalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEDCHR",value.Kedchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEDRSQ",value.Kedrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpMatGr value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEDTYP",value.Kedtyp);
                    parameters.Add("KEDIPB",value.Kedipb);
                    parameters.Add("KEDALX",value.Kedalx);
                    parameters.Add("KEDCHR",value.Kedchr);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpMatGr value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEDTYP",value.Kedtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEDIPB",value.Kedipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KEDALX",value.Kedalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEDCHR",value.Kedchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEDRSQ",value.Kedrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEDTYP",value.Kedtyp);
                    parameters.Add("KEDIPB",value.Kedipb);
                    parameters.Add("KEDALX",value.Kedalx);
                    parameters.Add("KEDCHR",value.Kedchr);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpMatGr> GetByAffaire(string KEDTYP, string KEDIPB, int KEDALX){
                    return connection.EnsureOpened().Query<KpMatGr>(select_GetByAffaire, new {KEDTYP, KEDIPB, KEDALX}).ToList();
            }
    }
}
