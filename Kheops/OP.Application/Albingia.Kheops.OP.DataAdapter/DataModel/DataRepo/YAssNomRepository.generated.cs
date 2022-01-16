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

    public  partial class  YAssNomRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
ANIAS AS CODE, ANTNM AS TYPE, ANINL AS CODEINTERLOCUTEUR FROM YASSNOM
WHERE ANIAS = :code
and ANINL = :codeInterloc
and ANTNM = :typeNom
";
            const string update=@"UPDATE YASSNOM SET 
ANIAS = :ANIAS, ANINL = :ANINL, ANTNM = :ANTNM
 WHERE 
ANIAS = :code and ANINL = :codeInterloc and ANTNM = :typeNom";
            const string delete=@"DELETE FROM YASSNOM WHERE ANIAS = :code AND ANINL = :codeInterloc AND ANTNM = :typeNom";
            const string insert=@"INSERT INTO  YASSNOM (
ANIAS, ANTNM, ANINL
) VALUES (
:ANIAS, :ANTNM, :ANINL)";
            const string select_GetByAssure=@"SELECT
ANIAS, ANINL, ANTNM, ANORD, ANNOM
, ANTIT FROM YASSNOM
WHERE ANIAS = :codeAssure
";
            #endregion

            public YAssNomRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YAssNom Get(int code, int codeInterloc, string typeNom){
                return connection.Query<YAssNom>(select, new {code, codeInterloc, typeNom}).SingleOrDefault();
            }


            public void Insert(YAssNom value){
                    var parameters = new DynamicParameters();
                    parameters.Add("ANIAS",value.Anias, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("ANINL",value.Aninl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ANTNM",value.Antnm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("ANORD",value.Anord, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ANNOM",value.Annom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("ANTIT",value.Antit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YAssNom value){
                    var parameters = new DynamicParameters();
                    parameters.Add("code",value.Anias);
                    parameters.Add("codeInterloc",value.Aninl);
                    parameters.Add("typeNom",value.Antnm);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YAssNom value){
                    var parameters = new DynamicParameters();
                    parameters.Add("ANIAS",value.Anias, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("ANINL",value.Aninl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ANTNM",value.Antnm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("ANORD",value.Anord, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ANNOM",value.Annom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("ANTIT",value.Antit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("code",value.Anias);
                    parameters.Add("codeInterloc",value.Aninl);
                    parameters.Add("typeNom",value.Antnm);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YAssNom> GetByAssure(int codeAssure){
                    return connection.EnsureOpened().Query<YAssNom>(select_GetByAssure, new {codeAssure}).ToList();
            }
    }
}
