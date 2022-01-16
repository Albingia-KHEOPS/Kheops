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

    public  partial class  KGarFamRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
GVGAR, GVBRA, GVSBR, GVCAT, GVFAM
 FROM KGARFAM
WHERE GVGAR = :parGVGAR
and GVBRA = :parGVBRA
and GVSBR = :parGVSBR
and GVCAT = :parGVCAT
";
            const string update=@"UPDATE KGARFAM SET 
GVGAR = :GVGAR, GVBRA = :GVBRA, GVSBR = :GVSBR, GVCAT = :GVCAT, GVFAM = :GVFAM
 WHERE 
GVGAR = :parGVGAR and GVBRA = :parGVBRA and GVSBR = :parGVSBR and GVCAT = :parGVCAT";
            const string delete=@"DELETE FROM KGARFAM WHERE GVGAR = :parGVGAR AND GVBRA = :parGVBRA AND GVSBR = :parGVSBR AND GVCAT = :parGVCAT";
            const string insert=@"INSERT INTO  KGARFAM (
GVGAR, GVBRA, GVSBR, GVCAT, GVFAM

) VALUES (
:GVGAR, :GVBRA, :GVSBR, :GVCAT, :GVFAM
)";
            const string select_GetAll=@"SELECT
GVGAR, GVBRA, GVSBR, GVCAT, GVFAM
 FROM KGARFAM
";
            #endregion

            public KGarFamRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KGarFam Get(string parGVGAR, string parGVBRA, string parGVSBR, string parGVCAT){
                return connection.Query<KGarFam>(select, new {parGVGAR, parGVBRA, parGVSBR, parGVCAT}).SingleOrDefault();
            }


            public void Insert(KGarFam value){
                    var parameters = new DynamicParameters();
                    parameters.Add("GVGAR",value.Gvgar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("GVBRA",value.Gvbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("GVSBR",value.Gvsbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("GVCAT",value.Gvcat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("GVFAM",value.Gvfam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KGarFam value){
                    var parameters = new DynamicParameters();
                    parameters.Add("parGVGAR",value.Gvgar);
                    parameters.Add("parGVBRA",value.Gvbra);
                    parameters.Add("parGVSBR",value.Gvsbr);
                    parameters.Add("parGVCAT",value.Gvcat);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KGarFam value){
                    var parameters = new DynamicParameters();
                    parameters.Add("GVGAR",value.Gvgar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("GVBRA",value.Gvbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("GVSBR",value.Gvsbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("GVCAT",value.Gvcat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("GVFAM",value.Gvfam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("parGVGAR",value.Gvgar);
                    parameters.Add("parGVBRA",value.Gvbra);
                    parameters.Add("parGVSBR",value.Gvsbr);
                    parameters.Add("parGVCAT",value.Gvcat);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KGarFam> GetAll(){
                    return connection.EnsureOpened().Query<KGarFam>(select_GetAll).ToList();
            }
    }
}
