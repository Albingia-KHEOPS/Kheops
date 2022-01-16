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

    public  partial class  YPriPGaRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
PLIPB, PLALX, PLTYE, PLGAR, PLMHT
, PLMTX, PLTAX, PLTXV, PLTXU, PLMTT
, PLXF1, PLMX1, PLXF2, PLMX2, PLXF3
, PLMX3, PLSUP, PLGAP, PLKHT, PLKHX
, PLKTT, PLKX1, PLKX2, PLKX3 FROM YPRIPGA
WHERE PLIPB = :PLIPB
and PLALX = :PLALX
and PLTYE = :PLTYE
and PLGAR = :PLGAR
";
            const string update=@"UPDATE YPRIPGA SET 
PLIPB = :PLIPB, PLALX = :PLALX, PLTYE = :PLTYE, PLGAR = :PLGAR, PLMHT = :PLMHT, PLMTX = :PLMTX, PLTAX = :PLTAX, PLTXV = :PLTXV, PLTXU = :PLTXU, PLMTT = :PLMTT
, PLXF1 = :PLXF1, PLMX1 = :PLMX1, PLXF2 = :PLXF2, PLMX2 = :PLMX2, PLXF3 = :PLXF3, PLMX3 = :PLMX3, PLSUP = :PLSUP, PLGAP = :PLGAP, PLKHT = :PLKHT, PLKHX = :PLKHX
, PLKTT = :PLKTT, PLKX1 = :PLKX1, PLKX2 = :PLKX2, PLKX3 = :PLKX3
 WHERE 
PLIPB = :PLIPB and PLALX = :PLALX and PLTYE = :PLTYE and PLGAR = :PLGAR";
            const string delete=@"DELETE FROM YPRIPGA WHERE PLIPB = :PLIPB AND PLALX = :PLALX AND PLTYE = :PLTYE AND PLGAR = :PLGAR";
            const string insert=@"INSERT INTO  YPRIPGA (
PLIPB, PLALX, PLTYE, PLGAR, PLMHT
, PLMTX, PLTAX, PLTXV, PLTXU, PLMTT
, PLXF1, PLMX1, PLXF2, PLMX2, PLXF3
, PLMX3, PLSUP, PLGAP, PLKHT, PLKHX
, PLKTT, PLKX1, PLKX2, PLKX3
) VALUES (
:PLIPB, :PLALX, :PLTYE, :PLGAR, :PLMHT
, :PLMTX, :PLTAX, :PLTXV, :PLTXU, :PLMTT
, :PLXF1, :PLMX1, :PLXF2, :PLMX2, :PLXF3
, :PLMX3, :PLSUP, :PLGAP, :PLKHT, :PLKHX
, :PLKTT, :PLKX1, :PLKX2, :PLKX3)";
            const string select_GetByAffaire=@"SELECT
PLIPB, PLALX, PLTYE, PLGAR, PLMHT
, PLMTX, PLTAX, PLTXV, PLTXU, PLMTT
, PLXF1, PLMX1, PLXF2, PLMX2, PLXF3
, PLMX3, PLSUP, PLGAP, PLKHT, PLKHX
, PLKTT, PLKX1, PLKX2, PLKX3 FROM YPRIPGA
WHERE PLIPB = :PLIPB
and PLALX = :PLALX
";
            #endregion

            public YPriPGaRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YPriPGa Get(string PLIPB, int PLALX, string PLTYE, string PLGAR){
                return connection.Query<YPriPGa>(select, new {PLIPB, PLALX, PLTYE, PLGAR}).SingleOrDefault();
            }


            public void Insert(YPriPGa value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PLIPB",value.Plipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PLALX",value.Plalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PLTYE",value.Pltye??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PLGAR",value.Plgar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("PLMHT",value.Plmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLMTX",value.Plmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLTAX",value.Pltax??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PLTXV",value.Pltxv, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 3);
                    parameters.Add("PLTXU",value.Pltxu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PLMTT",value.Plmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLXF1",value.Plxf1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PLMX1",value.Plmx1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLXF2",value.Plxf2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PLMX2",value.Plmx2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLXF3",value.Plxf3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PLMX3",value.Plmx3, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLSUP",value.Plsup??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PLGAP",value.Plgap, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PLKHT",value.Plkht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLKHX",value.Plkhx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLKTT",value.Plktt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLKX1",value.Plkx1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLKX2",value.Plkx2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLKX3",value.Plkx3, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YPriPGa value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PLIPB",value.Plipb);
                    parameters.Add("PLALX",value.Plalx);
                    parameters.Add("PLTYE",value.Pltye);
                    parameters.Add("PLGAR",value.Plgar);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YPriPGa value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PLIPB",value.Plipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PLALX",value.Plalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PLTYE",value.Pltye??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PLGAR",value.Plgar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("PLMHT",value.Plmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLMTX",value.Plmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLTAX",value.Pltax??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PLTXV",value.Pltxv, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 3);
                    parameters.Add("PLTXU",value.Pltxu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PLMTT",value.Plmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLXF1",value.Plxf1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PLMX1",value.Plmx1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLXF2",value.Plxf2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PLMX2",value.Plmx2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLXF3",value.Plxf3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PLMX3",value.Plmx3, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLSUP",value.Plsup??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PLGAP",value.Plgap, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PLKHT",value.Plkht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLKHX",value.Plkhx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLKTT",value.Plktt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLKX1",value.Plkx1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLKX2",value.Plkx2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLKX3",value.Plkx3, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLIPB",value.Plipb);
                    parameters.Add("PLALX",value.Plalx);
                    parameters.Add("PLTYE",value.Pltye);
                    parameters.Add("PLGAR",value.Plgar);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YPriPGa> GetByAffaire(string PLIPB, int PLALX){
                    return connection.EnsureOpened().Query<YPriPGa>(select_GetByAffaire, new {PLIPB, PLALX}).ToList();
            }
    }
}
