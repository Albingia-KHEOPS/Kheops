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

    public  partial class  KpdocldwRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KEMID, KEMKELID, KEMORD, KEMTYPD, KEMTYPL
, KEMTYENV, KEMTAMP, KEMTYDS, KEMTYI, KEMIDS
, KEMDSTP, KEMINL, KEMNBEX, KEMDOCA, KEMTYDIF
, KEMLMAI, KEMAEMO, KEMAEM, KEMKESID, KEMNTA
, KEMSTU, KEMSIT, KEMSTD, KEMSTH, KEMENVU
, KEMENVD, KEMENVH FROM KPDOCLDW
WHERE KEMID = :KEMID
";
            const string update=@"UPDATE KPDOCLDW SET 
KEMID = :KEMID, KEMKELID = :KEMKELID, KEMORD = :KEMORD, KEMTYPD = :KEMTYPD, KEMTYPL = :KEMTYPL, KEMTYENV = :KEMTYENV, KEMTAMP = :KEMTAMP, KEMTYDS = :KEMTYDS, KEMTYI = :KEMTYI, KEMIDS = :KEMIDS
, KEMDSTP = :KEMDSTP, KEMINL = :KEMINL, KEMNBEX = :KEMNBEX, KEMDOCA = :KEMDOCA, KEMTYDIF = :KEMTYDIF, KEMLMAI = :KEMLMAI, KEMAEMO = :KEMAEMO, KEMAEM = :KEMAEM, KEMKESID = :KEMKESID, KEMNTA = :KEMNTA
, KEMSTU = :KEMSTU, KEMSIT = :KEMSIT, KEMSTD = :KEMSTD, KEMSTH = :KEMSTH, KEMENVU = :KEMENVU, KEMENVD = :KEMENVD, KEMENVH = :KEMENVH
 WHERE 
KEMID = :KEMID";
            const string delete=@"DELETE FROM KPDOCLDW WHERE KEMID = :KEMID";
            const string insert=@"INSERT INTO  KPDOCLDW (
KEMID, KEMKELID, KEMORD, KEMTYPD, KEMTYPL
, KEMTYENV, KEMTAMP, KEMTYDS, KEMTYI, KEMIDS
, KEMDSTP, KEMINL, KEMNBEX, KEMDOCA, KEMTYDIF
, KEMLMAI, KEMAEMO, KEMAEM, KEMKESID, KEMNTA
, KEMSTU, KEMSIT, KEMSTD, KEMSTH, KEMENVU
, KEMENVD, KEMENVH
) VALUES (
:KEMID, :KEMKELID, :KEMORD, :KEMTYPD, :KEMTYPL
, :KEMTYENV, :KEMTAMP, :KEMTYDS, :KEMTYI, :KEMIDS
, :KEMDSTP, :KEMINL, :KEMNBEX, :KEMDOCA, :KEMTYDIF
, :KEMLMAI, :KEMAEMO, :KEMAEM, :KEMKESID, :KEMNTA
, :KEMSTU, :KEMSIT, :KEMSTD, :KEMSTH, :KEMENVU
, :KEMENVD, :KEMENVH)";
            const string select_GetByLot=@"SELECT
KEMID, KEMKELID, KEMORD, KEMTYPD, KEMTYPL
, KEMTYENV, KEMTAMP, KEMTYDS, KEMTYI, KEMIDS
, KEMDSTP, KEMINL, KEMNBEX, KEMDOCA, KEMTYDIF
, KEMLMAI, KEMAEMO, KEMAEM, KEMKESID, KEMNTA
, KEMSTU, KEMSIT, KEMSTD, KEMSTH, KEMENVU
, KEMENVD, KEMENVH FROM KPDOCLDW
WHERE KEMKELID = :lot
";
            const string select_GetByAffaire=@"SELECT
KEMID, KEMKELID, KEMORD, KEMTYPD, KEMTYPL
, KEMTYENV, KEMTAMP, KEMTYDS, KEMTYI, KEMIDS
, KEMDSTP, KEMINL, KEMNBEX, KEMDOCA, KEMTYDIF
, KEMLMAI, KEMAEMO, KEMAEM, KEMKESID, KEMNTA
, KEMSTU, KEMSIT, KEMSTD, KEMSTH, KEMENVU
, KEMENVD, KEMENVH FROM KPDOCLDW
INNER JOIN KPDOCLW ON KEMKELID=KELID
WHERE KELTYP = :typeAffaire
and KELIPB = :codeAffaire
and KELALX = :numeroAliment
and KELAVN = :numeroAvenant
";
            #endregion

            public KpdocldwRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpDocLD Get(Int64 KEMID){
                return connection.Query<KpDocLD>(select, new {KEMID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KEMID") ;
            }

            public void Insert(KpDocLD value){
                    if(value.Kemid == default(Int64)) {
                        value.Kemid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KEMID",value.Kemid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEMKELID",value.Kemkelid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEMORD",value.Kemord, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEMTYPD",value.Kemtypd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEMTYPL",value.Kemtypl, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEMTYENV",value.Kemtyenv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KEMTAMP",value.Kemtamp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEMTYDS",value.Kemtyds??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEMTYI",value.Kemtyi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KEMIDS",value.Kemids, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEMDSTP",value.Kemdstp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEMINL",value.Keminl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEMNBEX",value.Kemnbex, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KEMDOCA",value.Kemdoca, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEMTYDIF",value.Kemtydif??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEMLMAI",value.Kemlmai, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEMAEMO",value.Kemaemo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KEMAEM",value.Kemaem??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KEMKESID",value.Kemkesid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEMNTA",value.Kemnta??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEMSTU",value.Kemstu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEMSIT",value.Kemsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEMSTD",value.Kemstd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KEMSTH",value.Kemsth, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KEMENVU",value.Kemenvu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEMENVD",value.Kemenvd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KEMENVH",value.Kemenvh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpDocLD value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEMID",value.Kemid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpDocLD value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEMID",value.Kemid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEMKELID",value.Kemkelid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEMORD",value.Kemord, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEMTYPD",value.Kemtypd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEMTYPL",value.Kemtypl, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEMTYENV",value.Kemtyenv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KEMTAMP",value.Kemtamp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEMTYDS",value.Kemtyds??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEMTYI",value.Kemtyi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KEMIDS",value.Kemids, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEMDSTP",value.Kemdstp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEMINL",value.Keminl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEMNBEX",value.Kemnbex, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KEMDOCA",value.Kemdoca, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEMTYDIF",value.Kemtydif??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEMLMAI",value.Kemlmai, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEMAEMO",value.Kemaemo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KEMAEM",value.Kemaem??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KEMKESID",value.Kemkesid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEMNTA",value.Kemnta??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEMSTU",value.Kemstu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEMSIT",value.Kemsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEMSTD",value.Kemstd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KEMSTH",value.Kemsth, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KEMENVU",value.Kemenvu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEMENVD",value.Kemenvd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KEMENVH",value.Kemenvh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KEMID",value.Kemid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpDocLD> GetByLot(Int64 lot){
                    return connection.EnsureOpened().Query<KpDocLD>(select_GetByLot, new {lot}).ToList();
            }
            public IEnumerable<KpDocLD> GetByAffaire(string typeAffaire, string codeAffaire, int numeroAliment, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpDocLD>(select_GetByAffaire, new {typeAffaire, codeAffaire, numeroAliment, numeroAvenant}).ToList();
            }
    }
}
