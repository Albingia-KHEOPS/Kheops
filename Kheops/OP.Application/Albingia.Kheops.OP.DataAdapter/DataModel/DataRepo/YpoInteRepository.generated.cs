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

    public  partial class  YpoInteRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
PPIPB, PPALX, PPIIN, PPTYI, PPINL
, PPPOL, PPSYM FROM YPOINTE
WHERE PPIPB = :PPIPB
and PPALX = :PPALX
and PPIIN = :PPIIN
";
            const string update=@"UPDATE YPOINTE SET 
PPIPB = :PPIPB, PPALX = :PPALX, PPIIN = :PPIIN, PPTYI = :PPTYI, PPINL = :PPINL, PPPOL = :PPPOL, PPSYM = :PPSYM
 WHERE 
PPIPB = :PPIPB and PPALX = :PPALX and PPIIN = :PPIIN";
            const string delete=@"DELETE FROM YPOINTE WHERE PPIPB = :PPIPB AND PPALX = :PPALX AND PPIIN = :PPIIN";
            const string insert=@"INSERT INTO  YPOINTE (
PPIPB, PPALX, PPIIN, PPTYI, PPINL
, PPPOL, PPSYM
) VALUES (
:PPIPB, :PPALX, :PPIIN, :PPTYI, :PPINL
, :PPPOL, :PPSYM)";
            const string select_GetByAffaire=@"SELECT
PPIPB, PPALX, PPIIN, PPTYI, PPINL
, PPPOL, PPSYM FROM YPOINTE
WHERE PPIPB = :PPIPB
and PPALX = :PPALX
";
            #endregion

            public YpoInteRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YpoInte Get(string PPIPB, int PPALX, int PPIIN){
                return connection.Query<YpoInte>(select, new {PPIPB, PPALX, PPIIN}).SingleOrDefault();
            }


            public void Insert(YpoInte value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PPIPB",value.Ppipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PPALX",value.Ppalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PPIIN",value.Ppiin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PPTYI",value.Pptyi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PPINL",value.Ppinl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PPPOL",value.Pppol??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:25, scale: 0);
                    parameters.Add("PPSYM",value.Ppsym??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YpoInte value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PPIPB",value.Ppipb);
                    parameters.Add("PPALX",value.Ppalx);
                    parameters.Add("PPIIN",value.Ppiin);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YpoInte value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PPIPB",value.Ppipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PPALX",value.Ppalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PPIIN",value.Ppiin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PPTYI",value.Pptyi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PPINL",value.Ppinl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PPPOL",value.Pppol??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:25, scale: 0);
                    parameters.Add("PPSYM",value.Ppsym??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PPIPB",value.Ppipb);
                    parameters.Add("PPALX",value.Ppalx);
                    parameters.Add("PPIIN",value.Ppiin);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YpoInte> GetByAffaire(string PPIPB, int PPALX){
                    return connection.EnsureOpened().Query<YpoInte>(select_GetByAffaire, new {PPIPB, PPALX}).ToList();
            }
    }
}
