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

    public  partial class  KpSuspRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KICID, KICTYP, KICIPB, KICALX, KICTYE
, KICIPK, KICNUR, KICORI, KICDEBM, KICDEBD
, KICDEBH, KICFINM, KICFIND, KICFINH, KICRSAD
, KICRSAH, KICREVD, KICREVH, KICCRU, KICCRD
, KICCRH, KICAVN, KICMJU, KICMJD, KICMJH
, KICSIT, KICSTU, KICSTD, KICSTH, KICACA
 FROM KPSUSP
WHERE KICID = :id
FETCH FIRST 200 ROWS ONLY
";
            const string update=@"UPDATE KPSUSP SET 
KICID = :KICID, KICTYP = :KICTYP, KICIPB = :KICIPB, KICALX = :KICALX, KICTYE = :KICTYE, KICIPK = :KICIPK, KICNUR = :KICNUR, KICORI = :KICORI, KICDEBM = :KICDEBM, KICDEBD = :KICDEBD
, KICDEBH = :KICDEBH, KICFINM = :KICFINM, KICFIND = :KICFIND, KICFINH = :KICFINH, KICRSAD = :KICRSAD, KICRSAH = :KICRSAH, KICREVD = :KICREVD, KICREVH = :KICREVH, KICCRU = :KICCRU, KICCRD = :KICCRD
, KICCRH = :KICCRH, KICAVN = :KICAVN, KICMJU = :KICMJU, KICMJD = :KICMJD, KICMJH = :KICMJH, KICSIT = :KICSIT, KICSTU = :KICSTU, KICSTD = :KICSTD, KICSTH = :KICSTH, KICACA = :KICACA

 WHERE 
KICID = :id";
            const string delete=@"DELETE FROM KPSUSP WHERE KICID = :id";
            const string insert=@"INSERT INTO  KPSUSP (
KICID, KICTYP, KICIPB, KICALX, KICTYE
, KICIPK, KICNUR, KICORI, KICDEBM, KICDEBD
, KICDEBH, KICFINM, KICFIND, KICFINH, KICRSAD
, KICRSAH, KICREVD, KICREVH, KICCRU, KICCRD
, KICCRH, KICAVN, KICMJU, KICMJD, KICMJH
, KICSIT, KICSTU, KICSTD, KICSTH, KICACA

) VALUES (
:KICID, :KICTYP, :KICIPB, :KICALX, :KICTYE
, :KICIPK, :KICNUR, :KICORI, :KICDEBM, :KICDEBD
, :KICDEBH, :KICFINM, :KICFIND, :KICFINH, :KICRSAD
, :KICRSAH, :KICREVD, :KICREVH, :KICCRU, :KICCRD
, :KICCRH, :KICAVN, :KICMJU, :KICMJD, :KICMJH
, :KICSIT, :KICSTU, :KICSTD, :KICSTH, :KICACA
)";
            const string select_GetByAffaire=@"SELECT
KICID, KICTYP, KICIPB, KICALX, KICTYE
, KICIPK, KICNUR, KICORI, KICDEBM, KICDEBD
, KICDEBH, KICFINM, KICFIND, KICFINH, KICRSAD
, KICRSAH, KICREVD, KICREVH, KICCRU, KICCRD
, KICCRH, KICAVN, KICMJU, KICMJD, KICMJH
, KICSIT, KICSTU, KICSTD, KICSTH, KICACA
 FROM KPSUSP
WHERE KICIPB = :codeContrat
and KICALX = :versionContrat
and KICTYP = :typeContrat
";
            #endregion

            public KpSuspRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpSusp Get(Int64 id){
                return connection.Query<KpSusp>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KICID") ;
            }

            public void Insert(KpSusp value){
                    if(value.Kicid == default(Int64)) {
                        value.Kicid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KICID",value.Kicid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KICTYP",value.Kictyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KICIPB",value.Kicipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KICALX",value.Kicalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KICTYE",value.Kictye??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KICIPK",value.Kicipk, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KICNUR",value.Kicnur, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KICORI",value.Kicori??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KICDEBM",value.Kicdebm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KICDEBD",value.Kicdebd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KICDEBH",value.Kicdebh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KICFINM",value.Kicfinm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KICFIND",value.Kicfind, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KICFINH",value.Kicfinh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KICRSAD",value.Kicrsad, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KICRSAH",value.Kicrsah, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KICREVD",value.Kicrevd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KICREVH",value.Kicrevh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KICCRU",value.Kiccru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KICCRD",value.Kiccrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KICCRH",value.Kiccrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KICAVN",value.Kicavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KICMJU",value.Kicmju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KICMJD",value.Kicmjd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KICMJH",value.Kicmjh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KICSIT",value.Kicsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KICSTU",value.Kicstu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KICSTD",value.Kicstd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KICSTH",value.Kicsth, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KICACA",value.Kicaca??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpSusp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Kicid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpSusp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KICID",value.Kicid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KICTYP",value.Kictyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KICIPB",value.Kicipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KICALX",value.Kicalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KICTYE",value.Kictye??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KICIPK",value.Kicipk, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KICNUR",value.Kicnur, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KICORI",value.Kicori??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KICDEBM",value.Kicdebm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KICDEBD",value.Kicdebd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KICDEBH",value.Kicdebh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KICFINM",value.Kicfinm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KICFIND",value.Kicfind, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KICFINH",value.Kicfinh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KICRSAD",value.Kicrsad, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KICRSAH",value.Kicrsah, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KICREVD",value.Kicrevd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KICREVH",value.Kicrevh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KICCRU",value.Kiccru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KICCRD",value.Kiccrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KICCRH",value.Kiccrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KICAVN",value.Kicavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KICMJU",value.Kicmju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KICMJD",value.Kicmjd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KICMJH",value.Kicmjh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KICSIT",value.Kicsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KICSTU",value.Kicstu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KICSTD",value.Kicstd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KICSTH",value.Kicsth, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KICACA",value.Kicaca??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("id",value.Kicid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpSusp> GetByAffaire(string codeContrat, int versionContrat, string typeContrat){
                    return connection.EnsureOpened().Query<KpSusp>(select_GetByAffaire, new {codeContrat, versionContrat, typeContrat}).ToList();
            }
    }
}
