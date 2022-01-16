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

    public  partial class  KpdoclwRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KELID, KELTYP, KELIPB, KELALX, KELSUA
, KELNUM, KELSBR, KELSERV, KELACTG, KELACTN
, KELAVN, KELLIB, KELSTU, KELSIT, KELSTD
, KELSTH, KELCRU, KELCRD, KELCRH, KELMAJU
, KELMAJD, KELMAJH, KELEMI, KELIPK FROM KPDOCLW
WHERE KELID = :lotId
";
            const string update=@"UPDATE KPDOCLW SET 
KELID = :KELID, KELTYP = :KELTYP, KELIPB = :KELIPB, KELALX = :KELALX, KELSUA = :KELSUA, KELNUM = :KELNUM, KELSBR = :KELSBR, KELSERV = :KELSERV, KELACTG = :KELACTG, KELACTN = :KELACTN
, KELAVN = :KELAVN, KELLIB = :KELLIB, KELSTU = :KELSTU, KELSIT = :KELSIT, KELSTD = :KELSTD, KELSTH = :KELSTH, KELCRU = :KELCRU, KELCRD = :KELCRD, KELCRH = :KELCRH, KELMAJU = :KELMAJU
, KELMAJD = :KELMAJD, KELMAJH = :KELMAJH, KELEMI = :KELEMI, KELIPK = :KELIPK
 WHERE 
KELID = :lotId";
            const string delete=@"DELETE FROM KPDOCLW WHERE KELID = :lotId";
            const string insert=@"INSERT INTO  KPDOCLW (
KELID, KELTYP, KELIPB, KELALX, KELSUA
, KELNUM, KELSBR, KELSERV, KELACTG, KELACTN
, KELAVN, KELLIB, KELSTU, KELSIT, KELSTD
, KELSTH, KELCRU, KELCRD, KELCRH, KELMAJU
, KELMAJD, KELMAJH, KELEMI, KELIPK
) VALUES (
:KELID, :KELTYP, :KELIPB, :KELALX, :KELSUA
, :KELNUM, :KELSBR, :KELSERV, :KELACTG, :KELACTN
, :KELAVN, :KELLIB, :KELSTU, :KELSIT, :KELSTD
, :KELSTH, :KELCRU, :KELCRD, :KELCRH, :KELMAJU
, :KELMAJD, :KELMAJH, :KELEMI, :KELIPK)";
            const string select_GetByAffaire=@"SELECT
KELID, KELTYP, KELIPB, KELALX, KELSUA
, KELNUM, KELSBR, KELSERV, KELACTG, KELACTN
, KELAVN, KELLIB, KELSTU, KELSIT, KELSTD
, KELSTH, KELCRU, KELCRD, KELCRH, KELMAJU
, KELMAJD, KELMAJH, KELEMI, KELIPK FROM KPDOCLW
WHERE KELTYP = :typeAffaire
and KELIPB = :numeroAffaire
and KELALX = :numeroAliment
and KELAVN = :numeroAvenant
";
            #endregion

            public KpdoclwRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpDocL Get(Int64 lotId){
                return connection.Query<KpDocL>(select, new {lotId}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KELID") ;
            }

            public void Insert(KpDocL value){
                    if(value.Kelid == default(Int64)) {
                        value.Kelid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KELID",value.Kelid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KELTYP",value.Keltyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KELIPB",value.Kelipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KELALX",value.Kelalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KELSUA",value.Kelsua, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KELNUM",value.Kelnum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KELSBR",value.Kelsbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KELSERV",value.Kelserv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KELACTG",value.Kelactg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KELACTN",value.Kelactn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KELAVN",value.Kelavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KELLIB",value.Kellib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KELSTU",value.Kelstu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KELSIT",value.Kelsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KELSTD",value.Kelstd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KELSTH",value.Kelsth, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KELCRU",value.Kelcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KELCRD",value.Kelcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KELCRH",value.Kelcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KELMAJU",value.Kelmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KELMAJD",value.Kelmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KELMAJH",value.Kelmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KELEMI",value.Kelemi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KELIPK",value.Kelipk, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpDocL value){
                    var parameters = new DynamicParameters();
                    parameters.Add("lotId",value.Kelid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpDocL value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KELID",value.Kelid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KELTYP",value.Keltyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KELIPB",value.Kelipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KELALX",value.Kelalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KELSUA",value.Kelsua, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KELNUM",value.Kelnum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KELSBR",value.Kelsbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KELSERV",value.Kelserv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KELACTG",value.Kelactg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KELACTN",value.Kelactn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KELAVN",value.Kelavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KELLIB",value.Kellib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KELSTU",value.Kelstu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KELSIT",value.Kelsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KELSTD",value.Kelstd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KELSTH",value.Kelsth, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KELCRU",value.Kelcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KELCRD",value.Kelcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KELCRH",value.Kelcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KELMAJU",value.Kelmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KELMAJD",value.Kelmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KELMAJH",value.Kelmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KELEMI",value.Kelemi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KELIPK",value.Kelipk, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("lotId",value.Kelid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpDocL> GetByAffaire(string typeAffaire, string numeroAffaire, int numeroAliment, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpDocL>(select_GetByAffaire, new {typeAffaire, numeroAffaire, numeroAliment, numeroAvenant}).ToList();
            }
    }
}
