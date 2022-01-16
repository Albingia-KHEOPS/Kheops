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

    public  partial class  YpoConxRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
PJTYP, PJCCX, PJCNX, PJIPB, PJALX
, PJBRA, PJSBR, PJCAT, PJOBS, PJIDE
 FROM YPOCONX
WHERE PJTYP = :PJTYP
and PJIPB = :PJIPB
and PJALX = :PJALX
and PJCCX = :PJCCX
and PJCNX = :PJCNX
";
            const string update=@"UPDATE YPOCONX SET 
PJTYP = :PJTYP, PJCCX = :PJCCX, PJCNX = :PJCNX, PJIPB = :PJIPB, PJALX = :PJALX, PJBRA = :PJBRA, PJSBR = :PJSBR, PJCAT = :PJCAT, PJOBS = :PJOBS, PJIDE = :PJIDE

 WHERE 
PJTYP = :PJTYP and PJIPB = :PJIPB and PJALX = :PJALX and PJCCX = :PJCCX and PJCNX = :PJCNX";
            const string delete=@"DELETE FROM YPOCONX WHERE PJTYP = :PJTYP AND PJIPB = :PJIPB AND PJALX = :PJALX AND PJCCX = :PJCCX AND PJCNX = :PJCNX";
            const string insert=@"INSERT INTO  YPOCONX (
PJTYP, PJCCX, PJCNX, PJIPB, PJALX
, PJBRA, PJSBR, PJCAT, PJOBS, PJIDE

) VALUES (
:PJTYP, :PJCCX, :PJCNX, :PJIPB, :PJALX
, :PJBRA, :PJSBR, :PJCAT, :PJOBS, :PJIDE
)";
            const string select_GetByAffaire=@"SELECT
PJTYP, PJCCX, PJCNX, PJIPB, PJALX
, PJBRA, PJSBR, PJCAT, PJOBS, PJIDE
 FROM YPOCONX
WHERE PJTYP = :PJTYP
and PJIPB = :PJIPB
and PJALX = :PJALX
";
            #endregion

            public YpoConxRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YpoConx Get(string PJTYP, string PJIPB, int PJALX, string PJCCX, int PJCNX){
                return connection.Query<YpoConx>(select, new {PJTYP, PJIPB, PJALX, PJCCX, PJCNX}).SingleOrDefault();
            }


            public void Insert(YpoConx value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PJTYP",value.Pjtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PJCCX",value.Pjccx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PJCNX",value.Pjcnx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PJIPB",value.Pjipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PJALX",value.Pjalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PJBRA",value.Pjbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PJSBR",value.Pjsbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PJCAT",value.Pjcat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PJOBS",value.Pjobs, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("PJIDE",value.Pjide, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YpoConx value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PJTYP",value.Pjtyp);
                    parameters.Add("PJIPB",value.Pjipb);
                    parameters.Add("PJALX",value.Pjalx);
                    parameters.Add("PJCCX",value.Pjccx);
                    parameters.Add("PJCNX",value.Pjcnx);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YpoConx value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PJTYP",value.Pjtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PJCCX",value.Pjccx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PJCNX",value.Pjcnx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PJIPB",value.Pjipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PJALX",value.Pjalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PJBRA",value.Pjbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PJSBR",value.Pjsbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PJCAT",value.Pjcat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PJOBS",value.Pjobs, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("PJIDE",value.Pjide, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("PJTYP",value.Pjtyp);
                    parameters.Add("PJIPB",value.Pjipb);
                    parameters.Add("PJALX",value.Pjalx);
                    parameters.Add("PJCCX",value.Pjccx);
                    parameters.Add("PJCNX",value.Pjcnx);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YpoConx> GetByAffaire(string PJTYP, string PJIPB, int PJALX){
                    return connection.EnsureOpened().Query<YpoConx>(select_GetByAffaire, new {PJTYP, PJIPB, PJALX}).ToList();
            }
    }
}
