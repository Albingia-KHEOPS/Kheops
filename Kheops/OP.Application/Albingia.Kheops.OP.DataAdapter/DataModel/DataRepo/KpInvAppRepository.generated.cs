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

    public  partial class  KpInvAppRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KBGTYP, KBGIPB, KBGALX, KBGNUM, KBGKBEID
, KBGPERI, KBGRSQ, KBGOBJ, KBGFOR, KBGGAR
 FROM KPINVAPP
WHERE KBGTYP = :typeAffaire
and KBGIPB = :codeAffaire
and KBGALX = :numeroAliment
and KBGNUM = :numeroInventaire
";
            const string update=@"UPDATE KPINVAPP SET 
KBGTYP = :KBGTYP, KBGIPB = :KBGIPB, KBGALX = :KBGALX, KBGNUM = :KBGNUM, KBGKBEID = :KBGKBEID, KBGPERI = :KBGPERI, KBGRSQ = :KBGRSQ, KBGOBJ = :KBGOBJ, KBGFOR = :KBGFOR, KBGGAR = :KBGGAR

 WHERE 
KBGTYP = :typeAffaire and KBGIPB = :codeAffaire and KBGALX = :numeroAliment and KBGNUM = :numeroInventaire";
            const string delete=@"DELETE FROM KPINVAPP WHERE KBGTYP = :typeAffaire AND KBGIPB = :codeAffaire AND KBGALX = :numeroAliment AND KBGNUM = :numeroInventaire";
            const string insert=@"INSERT INTO  KPINVAPP (
KBGTYP, KBGIPB, KBGALX, KBGNUM, KBGKBEID
, KBGPERI, KBGRSQ, KBGOBJ, KBGFOR, KBGGAR

) VALUES (
:KBGTYP, :KBGIPB, :KBGALX, :KBGNUM, :KBGKBEID
, :KBGPERI, :KBGRSQ, :KBGOBJ, :KBGFOR, :KBGGAR
)";
            const string select_GetByAffaire=@"SELECT
KBGTYP, KBGIPB, KBGALX, KBGNUM, KBGKBEID
, KBGPERI, KBGRSQ, KBGOBJ, KBGFOR, KBGGAR
 FROM KPINVAPP
WHERE KBGTYP = :parKBGTYP
and KBGIPB = :parKBGIPB
and KBGALX = :parKBGALX
";
            const string select_GetByInven=@"SELECT
KBGTYP, KBGIPB, KBGALX, KBGNUM, KBGKBEID
, KBGPERI, KBGRSQ, KBGOBJ, KBGFOR, KBGGAR
 FROM KPINVAPP
WHERE KBGKBEID = :invenId
";
            #endregion

            public KpInvAppRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpInvApp Get(string typeAffaire, string codeAffaire, int numeroAliment, int numeroInventaire){
                return connection.Query<KpInvApp>(select, new {typeAffaire, codeAffaire, numeroAliment, numeroInventaire}).SingleOrDefault();
            }


            public void Insert(KpInvApp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KBGTYP",value.Kbgtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBGIPB",value.Kbgipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KBGALX",value.Kbgalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KBGNUM",value.Kbgnum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBGKBEID",value.Kbgkbeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBGPERI",value.Kbgperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KBGRSQ",value.Kbgrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBGOBJ",value.Kbgobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBGFOR",value.Kbgfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBGGAR",value.Kbggar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpInvApp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("typeAffaire",value.Kbgtyp);
                    parameters.Add("codeAffaire",value.Kbgipb);
                    parameters.Add("numeroAliment",value.Kbgalx);
                    parameters.Add("numeroInventaire",value.Kbgnum);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpInvApp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KBGTYP",value.Kbgtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBGIPB",value.Kbgipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KBGALX",value.Kbgalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KBGNUM",value.Kbgnum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBGKBEID",value.Kbgkbeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBGPERI",value.Kbgperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KBGRSQ",value.Kbgrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBGOBJ",value.Kbgobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBGFOR",value.Kbgfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBGGAR",value.Kbggar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("typeAffaire",value.Kbgtyp);
                    parameters.Add("codeAffaire",value.Kbgipb);
                    parameters.Add("numeroAliment",value.Kbgalx);
                    parameters.Add("numeroInventaire",value.Kbgnum);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpInvApp> GetByAffaire(string parKBGTYP, string parKBGIPB, int parKBGALX){
                    return connection.EnsureOpened().Query<KpInvApp>(select_GetByAffaire, new {parKBGTYP, parKBGIPB, parKBGALX}).ToList();
            }
            public IEnumerable<KpInvApp> GetByInven(Int64 invenId){
                    return connection.EnsureOpened().Query<KpInvApp>(select_GetByInven, new {invenId}).ToList();
            }
    }
}
