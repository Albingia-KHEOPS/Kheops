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

    public  partial class  YhpassuRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
PCIPB, PCALX, PCAVN, PCHIN, PCIAS
, PCPRI, PCQL1, PCQL2, PCQL3, PCQLD
, PCCNR, PCASS, PCSCP, PCTYP, PCDESI
 FROM YHPASSU
WHERE PCIPB = :PCIPB
and PCALX = :PCALX
and PCAVN = :PCAVN
and PCHIN = :PCHIN
and PCIAS = :PCIAS
";
            const string update=@"UPDATE YHPASSU SET 
PCIPB = :PCIPB, PCALX = :PCALX, PCAVN = :PCAVN, PCHIN = :PCHIN, PCIAS = :PCIAS, PCPRI = :PCPRI, PCQL1 = :PCQL1, PCQL2 = :PCQL2, PCQL3 = :PCQL3, PCQLD = :PCQLD
, PCCNR = :PCCNR, PCASS = :PCASS, PCSCP = :PCSCP, PCTYP = :PCTYP, PCDESI = :PCDESI
 WHERE 
PCIPB = :PCIPB and PCALX = :PCALX and PCAVN = :PCAVN and PCHIN = :PCHIN and PCIAS = :PCIAS";
            const string delete=@"DELETE FROM YHPASSU WHERE PCIPB = :PCIPB AND PCALX = :PCALX AND PCAVN = :PCAVN AND PCHIN = :PCHIN AND PCIAS = :PCIAS";
            const string insert=@"INSERT INTO  YHPASSU (
PCIPB, PCALX, PCAVN, PCHIN, PCIAS
, PCPRI, PCQL1, PCQL2, PCQL3, PCQLD
, PCCNR, PCASS, PCSCP, PCTYP, PCDESI

) VALUES (
:PCIPB, :PCALX, :PCAVN, :PCHIN, :PCIAS
, :PCPRI, :PCQL1, :PCQL2, :PCQL3, :PCQLD
, :PCCNR, :PCASS, :PCSCP, :PCTYP, :PCDESI
)";
            const string select_GetByAffaire=@"SELECT
PCIPB, PCALX, PCAVN, PCHIN, PCIAS
, PCPRI, PCQL1, PCQL2, PCQL3, PCQLD
, PCCNR, PCASS, PCSCP, PCTYP, PCDESI
 FROM YHPASSU
WHERE PCIPB = :PCIPB
and PCALX = :PCALX
and PCAVN = :PCAVN
and PCTYP = :PCTYP
";
            #endregion

            public YhpassuRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YpoAssu Get(string PCIPB, int PCALX, int PCAVN, int PCHIN, int PCIAS){
                return connection.Query<YpoAssu>(select, new {PCIPB, PCALX, PCAVN, PCHIN, PCIAS}).SingleOrDefault();
            }


            public void Insert(YpoAssu value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PCIPB",value.Pcipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PCALX",value.Pcalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PCAVN",value.Pcavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PCHIN",value.Pchin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PCIAS",value.Pcias, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PCPRI",value.Pcpri??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCQL1",value.Pcql1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PCQL2",value.Pcql2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PCQL3",value.Pcql3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PCQLD",value.Pcqld??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("PCCNR",value.Pccnr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCASS",value.Pcass??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCSCP",value.Pcscp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCTYP",value.Pctyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCDESI",value.Pcdesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YpoAssu value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PCIPB",value.Pcipb);
                    parameters.Add("PCALX",value.Pcalx);
                    parameters.Add("PCAVN",value.Pcavn);
                    parameters.Add("PCHIN",value.Pchin);
                    parameters.Add("PCIAS",value.Pcias);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YpoAssu value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PCIPB",value.Pcipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PCALX",value.Pcalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PCAVN",value.Pcavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PCHIN",value.Pchin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PCIAS",value.Pcias, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PCPRI",value.Pcpri??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCQL1",value.Pcql1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PCQL2",value.Pcql2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PCQL3",value.Pcql3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PCQLD",value.Pcqld??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("PCCNR",value.Pccnr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCASS",value.Pcass??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCSCP",value.Pcscp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCTYP",value.Pctyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCDESI",value.Pcdesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("PCIPB",value.Pcipb);
                    parameters.Add("PCALX",value.Pcalx);
                    parameters.Add("PCAVN",value.Pcavn);
                    parameters.Add("PCHIN",value.Pchin);
                    parameters.Add("PCIAS",value.Pcias);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YpoAssu> GetByAffaire(string PCIPB, int PCALX, int PCAVN, string PCTYP){
                    return connection.EnsureOpened().Query<YpoAssu>(select_GetByAffaire, new {PCIPB, PCALX, PCAVN, PCTYP}).ToList();
            }
    }
}
