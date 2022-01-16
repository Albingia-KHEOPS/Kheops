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

    public  partial class  HpmatglRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KEFTYP, KEFIPB, KEFALX, KEFAVN, KEFHIN
, KEFKEDCHR, KEFKEECHR, KEFICO FROM HPMATGL
WHERE KEFTYP = :KEFTYP
and KEFIPB = :KEFIPB
and KEFALX = :KEFALX
and KEFAVN = :KEFAVN
and KEFHIN = :KEFHIN
and KEFKEDCHR = :KEFKEDCHR
and KEFKEECHR = :KEFKEECHR
";
            const string update=@"UPDATE HPMATGL SET 
KEFTYP = :KEFTYP, KEFIPB = :KEFIPB, KEFALX = :KEFALX, KEFAVN = :KEFAVN, KEFHIN = :KEFHIN, KEFKEDCHR = :KEFKEDCHR, KEFKEECHR = :KEFKEECHR, KEFICO = :KEFICO
 WHERE 
KEFTYP = :KEFTYP and KEFIPB = :KEFIPB and KEFALX = :KEFALX and KEFAVN = :KEFAVN and KEFHIN = :KEFHIN and KEFKEDCHR = :KEFKEDCHR and KEFKEECHR = :KEFKEECHR";
            const string delete=@"DELETE FROM HPMATGL WHERE KEFTYP = :KEFTYP AND KEFIPB = :KEFIPB AND KEFALX = :KEFALX AND KEFAVN = :KEFAVN AND KEFHIN = :KEFHIN AND KEFKEDCHR = :KEFKEDCHR AND KEFKEECHR = :KEFKEECHR";
            const string insert=@"INSERT INTO  HPMATGL (
KEFTYP, KEFIPB, KEFALX, KEFAVN, KEFHIN
, KEFKEDCHR, KEFKEECHR, KEFICO
) VALUES (
:KEFTYP, :KEFIPB, :KEFALX, :KEFAVN, :KEFHIN
, :KEFKEDCHR, :KEFKEECHR, :KEFICO)";
            const string select_GetByAffaire=@"SELECT
KEFTYP, KEFIPB, KEFALX, KEFAVN, KEFHIN
, KEFKEDCHR, KEFKEECHR, KEFICO FROM HPMATGL
WHERE KEFTYP = :KEFTYP
and KEFIPB = :KEFIPB
and KEFALX = :KEFALX
and KEFAVN = :KEFAVN
";
            #endregion

            public HpmatglRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpMatGl Get(string KEFTYP, string KEFIPB, int KEFALX, int KEFAVN, int KEFHIN, int KEFKEDCHR, int KEFKEECHR){
                return connection.Query<KpMatGl>(select, new {KEFTYP, KEFIPB, KEFALX, KEFAVN, KEFHIN, KEFKEDCHR, KEFKEECHR}).SingleOrDefault();
            }


            public void Insert(KpMatGl value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEFTYP",value.Keftyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEFIPB",value.Kefipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KEFALX",value.Kefalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEFAVN",value.Kefavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEFHIN",value.Kefhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
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
                    parameters.Add("KEFAVN",value.Kefavn);
                    parameters.Add("KEFHIN",value.Kefhin);
                    parameters.Add("KEFKEDCHR",value.Kefkedchr);
                    parameters.Add("KEFKEECHR",value.Kefkeechr);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpMatGl value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEFTYP",value.Keftyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEFIPB",value.Kefipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KEFALX",value.Kefalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEFAVN",value.Kefavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEFHIN",value.Kefhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEFKEDCHR",value.Kefkedchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEFKEECHR",value.Kefkeechr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEFICO",value.Kefico??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEFTYP",value.Keftyp);
                    parameters.Add("KEFIPB",value.Kefipb);
                    parameters.Add("KEFALX",value.Kefalx);
                    parameters.Add("KEFAVN",value.Kefavn);
                    parameters.Add("KEFHIN",value.Kefhin);
                    parameters.Add("KEFKEDCHR",value.Kefkedchr);
                    parameters.Add("KEFKEECHR",value.Kefkeechr);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpMatGl> GetByAffaire(string KEFTYP, string KEFIPB, int KEFALX, int KEFAVN){
                    return connection.EnsureOpened().Query<KpMatGl>(select_GetByAffaire, new {KEFTYP, KEFIPB, KEFALX, KEFAVN}).ToList();
            }
    }
}
