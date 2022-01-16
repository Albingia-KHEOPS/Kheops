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

    public  partial class  KClauseRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDUID, KDUNM1, KDUNM2, KDUNM3, KDUVER
, KDULIB, KDULIR, KDUKDWID, KDUKDVID, KDUKDXID
, KDUDATD, KDUDATF, KDUDOC, KDUTDOC, KDUSERV
, KDUACTG, KDUCRU, KDUCRD, KDUCRH, KDUMAJU
, KDUMAJD, KDUMAJH, KDURGP, KDUORD, KDUANX
, KDUORA FROM KCLAUSE
WHERE KDUID = :KDUID
";
            const string update=@"UPDATE KCLAUSE SET 
KDUID = :KDUID, KDUNM1 = :KDUNM1, KDUNM2 = :KDUNM2, KDUNM3 = :KDUNM3, KDUVER = :KDUVER, KDULIB = :KDULIB, KDULIR = :KDULIR, KDUKDWID = :KDUKDWID, KDUKDVID = :KDUKDVID, KDUKDXID = :KDUKDXID
, KDUDATD = :KDUDATD, KDUDATF = :KDUDATF, KDUDOC = :KDUDOC, KDUTDOC = :KDUTDOC, KDUSERV = :KDUSERV, KDUACTG = :KDUACTG, KDUCRU = :KDUCRU, KDUCRD = :KDUCRD, KDUCRH = :KDUCRH, KDUMAJU = :KDUMAJU
, KDUMAJD = :KDUMAJD, KDUMAJH = :KDUMAJH, KDURGP = :KDURGP, KDUORD = :KDUORD, KDUANX = :KDUANX, KDUORA = :KDUORA
 WHERE 
KDUID = :KDUID";
            const string delete=@"DELETE FROM KCLAUSE WHERE KDUID = :KDUID";
            const string insert=@"INSERT INTO  KCLAUSE (
KDUID, KDUNM1, KDUNM2, KDUNM3, KDUVER
, KDULIB, KDULIR, KDUKDWID, KDUKDVID, KDUKDXID
, KDUDATD, KDUDATF, KDUDOC, KDUTDOC, KDUSERV
, KDUACTG, KDUCRU, KDUCRD, KDUCRH, KDUMAJU
, KDUMAJD, KDUMAJH, KDURGP, KDUORD, KDUANX
, KDUORA
) VALUES (
:KDUID, :KDUNM1, :KDUNM2, :KDUNM3, :KDUVER
, :KDULIB, :KDULIR, :KDUKDWID, :KDUKDVID, :KDUKDXID
, :KDUDATD, :KDUDATF, :KDUDOC, :KDUTDOC, :KDUSERV
, :KDUACTG, :KDUCRU, :KDUCRD, :KDUCRH, :KDUMAJU
, :KDUMAJD, :KDUMAJH, :KDURGP, :KDUORD, :KDUANX
, :KDUORA)";
            const string select_GetAll=@"SELECT
KDUID, KDUNM1, KDUNM2, KDUNM3, KDUVER
, KDULIB, KDULIR, KDUKDWID, KDUKDVID, KDUKDXID
, KDUDATD, KDUDATF, KDUDOC, KDUTDOC, KDUSERV
, KDUACTG, KDUCRU, KDUCRD, KDUCRH, KDUMAJU
, KDUMAJD, KDUMAJH, KDURGP, KDUORD, KDUANX
, KDUORA FROM KCLAUSE
";
            #endregion

            public KClauseRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KClause Get(Int64 KDUID){
                return connection.Query<KClause>(select, new {KDUID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KDUID") ;
            }

            public void Insert(KClause value){
                    if(value.Kduid == default(Int64)) {
                        value.Kduid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KDUID",value.Kduid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDUNM1",value.Kdunm1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDUNM2",value.Kdunm2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDUNM3",value.Kdunm3, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDUVER",value.Kduver, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDULIB",value.Kdulib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KDULIR",value.Kdulir??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("KDUKDWID",value.Kdukdwid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDUKDVID",value.Kdukdvid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDUKDXID",value.Kdukdxid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDUDATD",value.Kdudatd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDUDATF",value.Kdudatf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDUDOC",value.Kdudoc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:100, scale: 0);
                    parameters.Add("KDUTDOC",value.Kdutdoc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDUSERV",value.Kduserv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDUACTG",value.Kduactg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDUCRU",value.Kducru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDUCRD",value.Kducrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDUCRH",value.Kducrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KDUMAJU",value.Kdumaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDUMAJD",value.Kdumajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDUMAJH",value.Kdumajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KDURGP",value.Kdurgp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDUORD",value.Kduord, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDUANX",value.Kduanx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDUORA",value.Kduora, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KClause value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDUID",value.Kduid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KClause value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDUID",value.Kduid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDUNM1",value.Kdunm1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDUNM2",value.Kdunm2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDUNM3",value.Kdunm3, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDUVER",value.Kduver, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDULIB",value.Kdulib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KDULIR",value.Kdulir??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("KDUKDWID",value.Kdukdwid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDUKDVID",value.Kdukdvid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDUKDXID",value.Kdukdxid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDUDATD",value.Kdudatd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDUDATF",value.Kdudatf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDUDOC",value.Kdudoc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:100, scale: 0);
                    parameters.Add("KDUTDOC",value.Kdutdoc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDUSERV",value.Kduserv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDUACTG",value.Kduactg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDUCRU",value.Kducru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDUCRD",value.Kducrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDUCRH",value.Kducrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KDUMAJU",value.Kdumaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDUMAJD",value.Kdumajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDUMAJH",value.Kdumajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KDURGP",value.Kdurgp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDUORD",value.Kduord, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDUANX",value.Kduanx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDUORA",value.Kduora, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDUID",value.Kduid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KClause> GetAll(){
                    return connection.EnsureOpened().Query<KClause>(select_GetAll).ToList();
            }
    }
}
