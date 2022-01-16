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

    public  partial class  YPrimGaRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
PLIPB, PLALX, PLIPK, PLTYE, PLGAR
, PLMHT, PLMTX, PLTAX, PLTXV, PLTXU
, PLMTT, PLXF1, PLMX1, PLXF2, PLMX2
, PLXF3, PLMX3, PLSUP, PLGAP, PLKHT
, PLKHX, PLKTT, PLKX1, PLKX2, PLKX3
, PLGRH, PLKRH FROM YPRIMGA
WHERE PLIPB = :PLIPB
and PLALX = :PLALX
and PLIPK = :PLIPK
and PLTYE = :PLTYE
and PLGAR = :PLGAR
";
            const string update=@"UPDATE YPRIMGA SET 
PLIPB = :PLIPB, PLALX = :PLALX, PLIPK = :PLIPK, PLTYE = :PLTYE, PLGAR = :PLGAR, PLMHT = :PLMHT, PLMTX = :PLMTX, PLTAX = :PLTAX, PLTXV = :PLTXV, PLTXU = :PLTXU
, PLMTT = :PLMTT, PLXF1 = :PLXF1, PLMX1 = :PLMX1, PLXF2 = :PLXF2, PLMX2 = :PLMX2, PLXF3 = :PLXF3, PLMX3 = :PLMX3, PLSUP = :PLSUP, PLGAP = :PLGAP, PLKHT = :PLKHT
, PLKHX = :PLKHX, PLKTT = :PLKTT, PLKX1 = :PLKX1, PLKX2 = :PLKX2, PLKX3 = :PLKX3, PLGRH = :PLGRH, PLKRH = :PLKRH
 WHERE 
PLIPB = :PLIPB and PLALX = :PLALX and PLIPK = :PLIPK and PLTYE = :PLTYE and PLGAR = :PLGAR";
            const string delete=@"DELETE FROM YPRIMGA WHERE PLIPB = :PLIPB AND PLALX = :PLALX AND PLIPK = :PLIPK AND PLTYE = :PLTYE AND PLGAR = :PLGAR";
            const string insert=@"INSERT INTO  YPRIMGA (
PLIPB, PLALX, PLIPK, PLTYE, PLGAR
, PLMHT, PLMTX, PLTAX, PLTXV, PLTXU
, PLMTT, PLXF1, PLMX1, PLXF2, PLMX2
, PLXF3, PLMX3, PLSUP, PLGAP, PLKHT
, PLKHX, PLKTT, PLKX1, PLKX2, PLKX3
, PLGRH, PLKRH
) VALUES (
:PLIPB, :PLALX, :PLIPK, :PLTYE, :PLGAR
, :PLMHT, :PLMTX, :PLTAX, :PLTXV, :PLTXU
, :PLMTT, :PLXF1, :PLMX1, :PLXF2, :PLMX2
, :PLXF3, :PLMX3, :PLSUP, :PLGAP, :PLKHT
, :PLKHX, :PLKTT, :PLKX1, :PLKX2, :PLKX3
, :PLGRH, :PLKRH)";
            const string select_GetByAffaire=@"SELECT
PLIPB, PLALX, PLIPK, PLTYE, PLGAR
, PLMHT, PLMTX, PLTAX, PLTXV, PLTXU
, PLMTT, PLXF1, PLMX1, PLXF2, PLMX2
, PLXF3, PLMX3, PLSUP, PLGAP, PLKHT
, PLKHX, PLKTT, PLKX1, PLKX2, PLKX3
, PLGRH, PLKRH FROM YPRIMGA
WHERE PLIPB = :PLIPB
and PLALX = :PLALX
";
            #endregion

            public YPrimGaRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YPrimGa Get(string PLIPB, int PLALX, int PLIPK, string PLTYE, string PLGAR){
                return connection.Query<YPrimGa>(select, new {PLIPB, PLALX, PLIPK, PLTYE, PLGAR}).SingleOrDefault();
            }


            public void Insert(YPrimGa value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PLIPB",value.Plipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PLALX",value.Plalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PLIPK",value.Plipk, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
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
                    parameters.Add("PLGRH",value.Plgrh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLKRH",value.Plkrh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YPrimGa value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PLIPB",value.Plipb);
                    parameters.Add("PLALX",value.Plalx);
                    parameters.Add("PLIPK",value.Plipk);
                    parameters.Add("PLTYE",value.Pltye);
                    parameters.Add("PLGAR",value.Plgar);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YPrimGa value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PLIPB",value.Plipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PLALX",value.Plalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PLIPK",value.Plipk, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
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
                    parameters.Add("PLGRH",value.Plgrh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLKRH",value.Plkrh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PLIPB",value.Plipb);
                    parameters.Add("PLALX",value.Plalx);
                    parameters.Add("PLIPK",value.Plipk);
                    parameters.Add("PLTYE",value.Pltye);
                    parameters.Add("PLGAR",value.Plgar);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YPrimGa> GetByAffaire(string PLIPB, int PLALX){
                    return connection.EnsureOpened().Query<YPrimGa>(select_GetByAffaire, new {PLIPB, PLALX}).ToList();
            }
    }
}
