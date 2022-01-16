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

    public  partial class  HpmatflRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KECTYP, KECIPB, KECALX, KECAVN, KECHIN
, KECKEACHR, KECKEBCHR, KECICO FROM HPMATFL
WHERE KECTYP = :KECTYP
and KECIPB = :KECIPB
and KECALX = :KECALX
and KECAVN = :KECAVN
and KECHIN = :KECHIN
and KECKEACHR = :KECKEACHR
and KECKEBCHR = :KECKEBCHR
";
            const string update=@"UPDATE HPMATFL SET 
KECTYP = :KECTYP, KECIPB = :KECIPB, KECALX = :KECALX, KECAVN = :KECAVN, KECHIN = :KECHIN, KECKEACHR = :KECKEACHR, KECKEBCHR = :KECKEBCHR, KECICO = :KECICO
 WHERE 
KECTYP = :KECTYP and KECIPB = :KECIPB and KECALX = :KECALX and KECAVN = :KECAVN and KECHIN = :KECHIN and KECKEACHR = :KECKEACHR and KECKEBCHR = :KECKEBCHR";
            const string delete=@"DELETE FROM HPMATFL WHERE KECTYP = :KECTYP AND KECIPB = :KECIPB AND KECALX = :KECALX AND KECAVN = :KECAVN AND KECHIN = :KECHIN AND KECKEACHR = :KECKEACHR AND KECKEBCHR = :KECKEBCHR";
            const string insert=@"INSERT INTO  HPMATFL (
KECTYP, KECIPB, KECALX, KECAVN, KECHIN
, KECKEACHR, KECKEBCHR, KECICO
) VALUES (
:KECTYP, :KECIPB, :KECALX, :KECAVN, :KECHIN
, :KECKEACHR, :KECKEBCHR, :KECICO)";
            const string select_GetByAffaire=@"SELECT
KECTYP, KECIPB, KECALX, KECAVN, KECHIN
, KECKEACHR, KECKEBCHR, KECICO FROM HPMATFL
WHERE KECTYP = :KECTYP
and KECIPB = :KECIPB
and KECALX = :KECALX
and KECAVN = :KECAVN
";
            #endregion

            public HpmatflRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpMatFl Get(string KECTYP, string KECIPB, int KECALX, int KECAVN, int KECHIN, int KECKEACHR, int KECKEBCHR){
                return connection.Query<KpMatFl>(select, new {KECTYP, KECIPB, KECALX, KECAVN, KECHIN, KECKEACHR, KECKEBCHR}).SingleOrDefault();
            }


            public void Insert(KpMatFl value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KECTYP",value.Kectyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KECIPB",value.Kecipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KECALX",value.Kecalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KECAVN",value.Kecavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KECHIN",value.Kechin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
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
                    parameters.Add("KECAVN",value.Kecavn);
                    parameters.Add("KECHIN",value.Kechin);
                    parameters.Add("KECKEACHR",value.Keckeachr);
                    parameters.Add("KECKEBCHR",value.Keckebchr);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpMatFl value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KECTYP",value.Kectyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KECIPB",value.Kecipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KECALX",value.Kecalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KECAVN",value.Kecavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KECHIN",value.Kechin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KECKEACHR",value.Keckeachr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KECKEBCHR",value.Keckebchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KECICO",value.Kecico??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KECTYP",value.Kectyp);
                    parameters.Add("KECIPB",value.Kecipb);
                    parameters.Add("KECALX",value.Kecalx);
                    parameters.Add("KECAVN",value.Kecavn);
                    parameters.Add("KECHIN",value.Kechin);
                    parameters.Add("KECKEACHR",value.Keckeachr);
                    parameters.Add("KECKEBCHR",value.Keckebchr);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpMatFl> GetByAffaire(string KECTYP, string KECIPB, int KECALX, int KECAVN){
                    return connection.EnsureOpened().Query<KpMatFl>(select_GetByAffaire, new {KECTYP, KECIPB, KECALX, KECAVN}).ToList();
            }
    }
}
