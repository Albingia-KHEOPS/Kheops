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

    public  partial class  YhpinteRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
PPIPB, PPALX, PPAVN, PPHIN, PPIIN
, PPTYI, PPINL, PPPOL, PPSYM FROM YHPINTE
WHERE PPIPB = :PPIPB
and PPALX = :PPALX
and PPAVN = :PPAVN
and PPHIN = :PPHIN
and PPIIN = :PPIIN
";
            const string update=@"UPDATE YHPINTE SET 
PPIPB = :PPIPB, PPALX = :PPALX, PPAVN = :PPAVN, PPHIN = :PPHIN, PPIIN = :PPIIN, PPTYI = :PPTYI, PPINL = :PPINL, PPPOL = :PPPOL, PPSYM = :PPSYM
 WHERE 
PPIPB = :PPIPB and PPALX = :PPALX and PPAVN = :PPAVN and PPHIN = :PPHIN and PPIIN = :PPIIN";
            const string delete=@"DELETE FROM YHPINTE WHERE PPIPB = :PPIPB AND PPALX = :PPALX AND PPAVN = :PPAVN AND PPHIN = :PPHIN AND PPIIN = :PPIIN";
            const string insert=@"INSERT INTO  YHPINTE (
PPIPB, PPALX, PPAVN, PPHIN, PPIIN
, PPTYI, PPINL, PPPOL, PPSYM
) VALUES (
:PPIPB, :PPALX, :PPAVN, :PPHIN, :PPIIN
, :PPTYI, :PPINL, :PPPOL, :PPSYM)";
            const string select_GetByAffaire=@"SELECT
PPIPB, PPALX, PPAVN, PPHIN, PPIIN
, PPTYI, PPINL, PPPOL, PPSYM FROM YHPINTE
WHERE PPIPB = :PPIPB
and PPALX = :PPALX
and PPAVN = :PPAVN
";
            #endregion

            public YhpinteRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YpoInte Get(string PPIPB, int PPALX, int PPAVN, int PPHIN, int PPIIN){
                return connection.Query<YpoInte>(select, new {PPIPB, PPALX, PPAVN, PPHIN, PPIIN}).SingleOrDefault();
            }


            public void Insert(YpoInte value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PPIPB",value.Ppipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PPALX",value.Ppalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PPAVN",value.Ppavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PPHIN",value.Pphin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
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
                    parameters.Add("PPAVN",value.Ppavn);
                    parameters.Add("PPHIN",value.Pphin);
                    parameters.Add("PPIIN",value.Ppiin);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YpoInte value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PPIPB",value.Ppipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PPALX",value.Ppalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PPAVN",value.Ppavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PPHIN",value.Pphin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PPIIN",value.Ppiin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PPTYI",value.Pptyi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PPINL",value.Ppinl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PPPOL",value.Pppol??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:25, scale: 0);
                    parameters.Add("PPSYM",value.Ppsym??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PPIPB",value.Ppipb);
                    parameters.Add("PPALX",value.Ppalx);
                    parameters.Add("PPAVN",value.Ppavn);
                    parameters.Add("PPHIN",value.Pphin);
                    parameters.Add("PPIIN",value.Ppiin);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YpoInte> GetByAffaire(string PPIPB, int PPALX, int PPAVN){
                    return connection.EnsureOpened().Query<YpoInte>(select_GetByAffaire, new {PPIPB, PPALX, PPAVN}).ToList();
            }
    }
}
