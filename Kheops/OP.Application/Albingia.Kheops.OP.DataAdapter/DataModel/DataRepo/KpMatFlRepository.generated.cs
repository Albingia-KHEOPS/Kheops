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

    public  partial class  KpMatFlRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KECTYP, KECIPB, KECALX, KECKEACHR, KECKEBCHR
, KECICO FROM KPMATFL
WHERE KECTYP = :KECTYP
and KECIPB = :KECIPB
and KECALX = :KECALX
and KECKEACHR = :KECKEACHR
and KECKEBCHR = :KECKEBCHR
";
            const string update=@"UPDATE KPMATFL SET 
KECTYP = :KECTYP, KECIPB = :KECIPB, KECALX = :KECALX, KECKEACHR = :KECKEACHR, KECKEBCHR = :KECKEBCHR, KECICO = :KECICO
 WHERE 
KECTYP = :KECTYP and KECIPB = :KECIPB and KECALX = :KECALX and KECKEACHR = :KECKEACHR and KECKEBCHR = :KECKEBCHR";
            const string delete=@"DELETE FROM KPMATFL WHERE KECTYP = :KECTYP AND KECIPB = :KECIPB AND KECALX = :KECALX AND KECKEACHR = :KECKEACHR AND KECKEBCHR = :KECKEBCHR";
            const string insert=@"INSERT INTO  KPMATFL (
KECTYP, KECIPB, KECALX, KECKEACHR, KECKEBCHR
, KECICO
) VALUES (
:KECTYP, :KECIPB, :KECALX, :KECKEACHR, :KECKEBCHR
, :KECICO)";
            const string select_GetByAffaire=@"SELECT
KECTYP, KECIPB, KECALX, KECKEACHR, KECKEBCHR
, KECICO FROM KPMATFL
WHERE KECTYP = :KECTYP
and KECIPB = :KECIPB
and KECALX = :KECALX
";
            #endregion

            public KpMatFlRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpMatFl Get(string KECTYP, string KECIPB, int KECALX, int KECKEACHR, int KECKEBCHR){
                return connection.Query<KpMatFl>(select, new {KECTYP, KECIPB, KECALX, KECKEACHR, KECKEBCHR}).SingleOrDefault();
            }


            public void Insert(KpMatFl value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KECTYP",value.Kectyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KECIPB",value.Kecipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KECALX",value.Kecalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KECKEACHR",value.Keckeachr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KECKEBCHR",value.Keckebchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KECICO",value.Kecico??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpMatFl value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KECTYP",value.Kectyp);
                    parameters.Add("KECIPB",value.Kecipb);
                    parameters.Add("KECALX",value.Kecalx);
                    parameters.Add("KECKEACHR",value.Keckeachr);
                    parameters.Add("KECKEBCHR",value.Keckebchr);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpMatFl value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KECTYP",value.Kectyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KECIPB",value.Kecipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KECALX",value.Kecalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KECKEACHR",value.Keckeachr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KECKEBCHR",value.Keckebchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KECICO",value.Kecico??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KECTYP",value.Kectyp);
                    parameters.Add("KECIPB",value.Kecipb);
                    parameters.Add("KECALX",value.Kecalx);
                    parameters.Add("KECKEACHR",value.Keckeachr);
                    parameters.Add("KECKEBCHR",value.Keckebchr);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpMatFl> GetByAffaire(string KECTYP, string KECIPB, int KECALX){
                    return connection.EnsureOpened().Query<KpMatFl>(select_GetByAffaire, new {KECTYP, KECIPB, KECALX}).ToList();
            }
    }
}
