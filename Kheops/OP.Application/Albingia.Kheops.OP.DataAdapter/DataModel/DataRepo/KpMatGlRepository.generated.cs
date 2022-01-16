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

    public  partial class  KpMatGlRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KEFTYP, KEFIPB, KEFALX, KEFKEDCHR, KEFKEECHR
, KEFICO FROM KPMATGL
WHERE KEFTYP = :KEFTYP
and KEFIPB = :KEFIPB
and KEFALX = :KEFALX
and KEFKEDCHR = :KEFKEDCHR
and KEFKEECHR = :KEFKEECHR
";
            const string update=@"UPDATE KPMATGL SET 
KEFTYP = :KEFTYP, KEFIPB = :KEFIPB, KEFALX = :KEFALX, KEFKEDCHR = :KEFKEDCHR, KEFKEECHR = :KEFKEECHR, KEFICO = :KEFICO
 WHERE 
KEFTYP = :KEFTYP and KEFIPB = :KEFIPB and KEFALX = :KEFALX and KEFKEDCHR = :KEFKEDCHR and KEFKEECHR = :KEFKEECHR";
            const string delete=@"DELETE FROM KPMATGL WHERE KEFTYP = :KEFTYP AND KEFIPB = :KEFIPB AND KEFALX = :KEFALX AND KEFKEDCHR = :KEFKEDCHR AND KEFKEECHR = :KEFKEECHR";
            const string insert=@"INSERT INTO  KPMATGL (
KEFTYP, KEFIPB, KEFALX, KEFKEDCHR, KEFKEECHR
, KEFICO
) VALUES (
:KEFTYP, :KEFIPB, :KEFALX, :KEFKEDCHR, :KEFKEECHR
, :KEFICO)";
            const string select_GetByAffaire=@"SELECT
KEFTYP, KEFIPB, KEFALX, KEFKEDCHR, KEFKEECHR
, KEFICO FROM KPMATGL
WHERE KEFTYP = :KEFTYP
and KEFIPB = :KEFIPB
and KEFALX = :KEFALX
";
            #endregion

            public KpMatGlRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpMatGl Get(string KEFTYP, string KEFIPB, int KEFALX, int KEFKEDCHR, int KEFKEECHR){
                return connection.Query<KpMatGl>(select, new {KEFTYP, KEFIPB, KEFALX, KEFKEDCHR, KEFKEECHR}).SingleOrDefault();
            }


            public void Insert(KpMatGl value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEFTYP",value.Keftyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEFIPB",value.Kefipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KEFALX",value.Kefalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEFKEDCHR",value.Kefkedchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEFKEECHR",value.Kefkeechr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEFICO",value.Kefico??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpMatGl value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEFTYP",value.Keftyp);
                    parameters.Add("KEFIPB",value.Kefipb);
                    parameters.Add("KEFALX",value.Kefalx);
                    parameters.Add("KEFKEDCHR",value.Kefkedchr);
                    parameters.Add("KEFKEECHR",value.Kefkeechr);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpMatGl value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEFTYP",value.Keftyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEFIPB",value.Kefipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KEFALX",value.Kefalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEFKEDCHR",value.Kefkedchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEFKEECHR",value.Kefkeechr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEFICO",value.Kefico??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEFTYP",value.Keftyp);
                    parameters.Add("KEFIPB",value.Kefipb);
                    parameters.Add("KEFALX",value.Kefalx);
                    parameters.Add("KEFKEDCHR",value.Kefkedchr);
                    parameters.Add("KEFKEECHR",value.Kefkeechr);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpMatGl> GetByAffaire(string KEFTYP, string KEFIPB, int KEFALX){
                    return connection.EnsureOpened().Query<KpMatGl>(select_GetByAffaire, new {KEFTYP, KEFIPB, KEFALX}).ToList();
            }
    }
}
