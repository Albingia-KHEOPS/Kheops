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

    public  partial class  YpoAssuRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
PCTYP, PCIPB, PCALX, PCIAS, PCPRI
, PCQL1, PCQL2, PCQL3, PCQLD, PCCNR
, PCASS, PCSCP, PCDESI FROM YPOASSU
WHERE PCTYP = :PCTYP
and PCIPB = :PCIPB
and PCALX = :PCALX
and PCIAS = :PCIAS
";
            const string update=@"UPDATE YPOASSU SET 
PCTYP = :PCTYP, PCIPB = :PCIPB, PCALX = :PCALX, PCIAS = :PCIAS, PCPRI = :PCPRI, PCQL1 = :PCQL1, PCQL2 = :PCQL2, PCQL3 = :PCQL3, PCQLD = :PCQLD, PCCNR = :PCCNR
, PCASS = :PCASS, PCSCP = :PCSCP, PCDESI = :PCDESI
 WHERE 
PCTYP = :PCTYP and PCIPB = :PCIPB and PCALX = :PCALX and PCIAS = :PCIAS";
            const string delete=@"DELETE FROM YPOASSU WHERE PCTYP = :PCTYP AND PCIPB = :PCIPB AND PCALX = :PCALX AND PCIAS = :PCIAS";
            const string insert=@"INSERT INTO  YPOASSU (
PCTYP, PCIPB, PCALX, PCIAS, PCPRI
, PCQL1, PCQL2, PCQL3, PCQLD, PCCNR
, PCASS, PCSCP, PCDESI
) VALUES (
:PCTYP, :PCIPB, :PCALX, :PCIAS, :PCPRI
, :PCQL1, :PCQL2, :PCQL3, :PCQLD, :PCCNR
, :PCASS, :PCSCP, :PCDESI)";
            const string select_GetByAffaire=@"SELECT
PCTYP, PCIPB, PCALX, PCIAS, PCPRI
, PCQL1, PCQL2, PCQL3, PCQLD, PCCNR
, PCASS, PCSCP, PCDESI FROM YPOASSU
WHERE PCTYP = :PCTYP
and PCIPB = :PCIPB
and PCALX = :PCALX
";
            #endregion

            public YpoAssuRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YpoAssu Get(string PCTYP, string PCIPB, int PCALX, int PCIAS){
                return connection.Query<YpoAssu>(select, new {PCTYP, PCIPB, PCALX, PCIAS}).SingleOrDefault();
            }


            public void Insert(YpoAssu value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PCTYP",value.Pctyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCIPB",value.Pcipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PCALX",value.Pcalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PCIAS",value.Pcias, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PCPRI",value.Pcpri??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCQL1",value.Pcql1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PCQL2",value.Pcql2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PCQL3",value.Pcql3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PCQLD",value.Pcqld??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("PCCNR",value.Pccnr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCASS",value.Pcass??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCSCP",value.Pcscp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCDESI",value.Pcdesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YpoAssu value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PCTYP",value.Pctyp);
                    parameters.Add("PCIPB",value.Pcipb);
                    parameters.Add("PCALX",value.Pcalx);
                    parameters.Add("PCIAS",value.Pcias);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YpoAssu value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PCTYP",value.Pctyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCIPB",value.Pcipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PCALX",value.Pcalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PCIAS",value.Pcias, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PCPRI",value.Pcpri??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCQL1",value.Pcql1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PCQL2",value.Pcql2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PCQL3",value.Pcql3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PCQLD",value.Pcqld??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("PCCNR",value.Pccnr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCASS",value.Pcass??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCSCP",value.Pcscp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PCDESI",value.Pcdesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("PCTYP",value.Pctyp);
                    parameters.Add("PCIPB",value.Pcipb);
                    parameters.Add("PCALX",value.Pcalx);
                    parameters.Add("PCIAS",value.Pcias);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YpoAssu> GetByAffaire(string PCTYP, string PCIPB, int PCALX){
                    return connection.EnsureOpened().Query<YpoAssu>(select_GetByAffaire, new {PCTYP, PCIPB, PCALX}).ToList();
            }
    }
}
