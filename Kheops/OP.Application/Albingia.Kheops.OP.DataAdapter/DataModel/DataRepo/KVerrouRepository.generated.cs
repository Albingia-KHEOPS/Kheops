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

    public  partial class  KVerrouRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KAVID, KAVSERV, KAVTYP, KAVIPB, KAVALX
, KAVAVN, KAVSUA, KAVNUM, KAVSBR, KAVACTG
, KAVACT, KAVVERR, KAVCRU, KAVCRD, KAVCRH
, KAVLIB FROM KVERROU
WHERE KAVID = :KAVID
";
            const string update=@"UPDATE KVERROU SET 
KAVID = :KAVID, KAVSERV = :KAVSERV, KAVTYP = :KAVTYP, KAVIPB = :KAVIPB, KAVALX = :KAVALX, KAVAVN = :KAVAVN, KAVSUA = :KAVSUA, KAVNUM = :KAVNUM, KAVSBR = :KAVSBR, KAVACTG = :KAVACTG
, KAVACT = :KAVACT, KAVVERR = :KAVVERR, KAVCRU = :KAVCRU, KAVCRD = :KAVCRD, KAVCRH = :KAVCRH, KAVLIB = :KAVLIB
 WHERE 
KAVID = :KAVID";
            const string delete=@"DELETE FROM KVERROU WHERE KAVID = :KAVID";
            const string insert=@"INSERT INTO  KVERROU (
KAVID, KAVSERV, KAVTYP, KAVIPB, KAVALX
, KAVAVN, KAVSUA, KAVNUM, KAVSBR, KAVACTG
, KAVACT, KAVVERR, KAVCRU, KAVCRD, KAVCRH
, KAVLIB
) VALUES (
:KAVID, :KAVSERV, :KAVTYP, :KAVIPB, :KAVALX
, :KAVAVN, :KAVSUA, :KAVNUM, :KAVSBR, :KAVACTG
, :KAVACT, :KAVVERR, :KAVCRU, :KAVCRD, :KAVCRH
, :KAVLIB)";
            const string select_GetByAffaire=@"SELECT
KAVID, KAVSERV, KAVTYP, KAVIPB, KAVALX
, KAVAVN, KAVSUA, KAVNUM, KAVSBR, KAVACTG
, KAVACT, KAVVERR, KAVCRU, KAVCRD, KAVCRH
, KAVLIB FROM KVERROU
WHERE KAVTYP = :KAVTYP
and KAVIPB = :KAVIPB
and KAVALX = :KAVALX
and KAVAVN = :KAVAVN
";
            const string select_GetByLockKey=@"SELECT
KAVID, KAVSERV, KAVTYP, KAVIPB, KAVALX
, KAVAVN, KAVSUA, KAVNUM, KAVSBR, KAVACTG
, KAVACT, KAVVERR, KAVCRU, KAVCRD, KAVCRH
, KAVLIB FROM KVERROU
WHERE KAVIPB = :parKAVIPB
and KAVALX = :parKAVALX
and KAVTYP = :parKAVTYP
";
            const string select_GetByUser=@"SELECT
KAVID, KAVSERV, KAVTYP, KAVIPB, KAVALX
, KAVAVN, KAVSUA, KAVNUM, KAVSBR, KAVACTG
, KAVACT, KAVVERR, KAVCRU, KAVCRD, KAVCRH
, KAVLIB FROM KVERROU
WHERE KAVCRU = :parKAVCRU
";
            #endregion

            public KVerrouRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KVerrou Get(Int64 KAVID){
                return connection.Query<KVerrou>(select, new {KAVID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KAVID") ;
            }

            public void Insert(KVerrou value){
                    if(value.Kavid == default(Int64)) {
                        value.Kavid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KAVID",value.Kavid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAVSERV",value.Kavserv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAVTYP",value.Kavtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAVIPB",value.Kavipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KAVALX",value.Kavalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAVAVN",value.Kavavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAVSUA",value.Kavsua, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAVNUM",value.Kavnum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAVSBR",value.Kavsbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAVACTG",value.Kavactg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAVACT",value.Kavact??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("KAVVERR",value.Kavverr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAVCRU",value.Kavcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAVCRD",value.Kavcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAVCRH",value.Kavcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAVLIB",value.Kavlib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KVerrou value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAVID",value.Kavid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KVerrou value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAVID",value.Kavid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAVSERV",value.Kavserv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAVTYP",value.Kavtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAVIPB",value.Kavipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KAVALX",value.Kavalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAVAVN",value.Kavavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAVSUA",value.Kavsua, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAVNUM",value.Kavnum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAVSBR",value.Kavsbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAVACTG",value.Kavactg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAVACT",value.Kavact??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("KAVVERR",value.Kavverr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAVCRU",value.Kavcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAVCRD",value.Kavcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAVCRH",value.Kavcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAVLIB",value.Kavlib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KAVID",value.Kavid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KVerrou> GetByAffaire(string KAVTYP, string KAVIPB, int KAVALX, int KAVAVN){
                    return connection.EnsureOpened().Query<KVerrou>(select_GetByAffaire, new {KAVTYP, KAVIPB, KAVALX, KAVAVN}).ToList();
            }
            public IEnumerable<KVerrou> GetByLockKey(string parKAVIPB, int parKAVALX, string parKAVTYP){
                    return connection.EnsureOpened().Query<KVerrou>(select_GetByLockKey, new {parKAVIPB, parKAVALX, parKAVTYP}).ToList();
            }
            public IEnumerable<KVerrou> GetByUser(string parKAVCRU){
                    return connection.EnsureOpened().Query<KVerrou>(select_GetByUser, new {parKAVCRU}).ToList();
            }
    }
}
