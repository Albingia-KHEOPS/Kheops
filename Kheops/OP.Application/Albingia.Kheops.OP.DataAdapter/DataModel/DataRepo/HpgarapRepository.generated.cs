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

    public  partial class  HpgarapRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDFID, KDFTYP, KDFIPB, KDFALX, KDFAVN
, KDFHIN, KDFFOR, KDFOPT, KDFGARAN, KDFKDEID
, KDFGAN, KDFPERI, KDFRSQ, KDFOBJ, KDFINVEN
, KDFINVEP, KDFCRU, KDFCRD, KDFMAJU, KDFMAJD
, KDFPRV, KDFPRA, KDFPRW, KDFPRU, KDFTYC
, KDFMNT FROM HPGARAP
WHERE KDFID = :parKDFID
";
            const string update=@"UPDATE HPGARAP SET 
KDFID = :KDFID, KDFTYP = :KDFTYP, KDFIPB = :KDFIPB, KDFALX = :KDFALX, KDFAVN = :KDFAVN, KDFHIN = :KDFHIN, KDFFOR = :KDFFOR, KDFOPT = :KDFOPT, KDFGARAN = :KDFGARAN, KDFKDEID = :KDFKDEID
, KDFGAN = :KDFGAN, KDFPERI = :KDFPERI, KDFRSQ = :KDFRSQ, KDFOBJ = :KDFOBJ, KDFINVEN = :KDFINVEN, KDFINVEP = :KDFINVEP, KDFCRU = :KDFCRU, KDFCRD = :KDFCRD, KDFMAJU = :KDFMAJU, KDFMAJD = :KDFMAJD
, KDFPRV = :KDFPRV, KDFPRA = :KDFPRA, KDFPRW = :KDFPRW, KDFPRU = :KDFPRU, KDFTYC = :KDFTYC, KDFMNT = :KDFMNT
 WHERE 
KDFID = :parKDFID";
            const string delete=@"DELETE FROM HPGARAP WHERE KDFID = :parKDFID";
            const string insert=@"INSERT INTO  HPGARAP (
KDFID, KDFTYP, KDFIPB, KDFALX, KDFAVN
, KDFHIN, KDFFOR, KDFOPT, KDFGARAN, KDFKDEID
, KDFGAN, KDFPERI, KDFRSQ, KDFOBJ, KDFINVEN
, KDFINVEP, KDFCRU, KDFCRD, KDFMAJU, KDFMAJD
, KDFPRV, KDFPRA, KDFPRW, KDFPRU, KDFTYC
, KDFMNT
) VALUES (
:KDFID, :KDFTYP, :KDFIPB, :KDFALX, :KDFAVN
, :KDFHIN, :KDFFOR, :KDFOPT, :KDFGARAN, :KDFKDEID
, :KDFGAN, :KDFPERI, :KDFRSQ, :KDFOBJ, :KDFINVEN
, :KDFINVEP, :KDFCRU, :KDFCRD, :KDFMAJU, :KDFMAJD
, :KDFPRV, :KDFPRA, :KDFPRW, :KDFPRU, :KDFTYC
, :KDFMNT)";
            const string select_GetByFormule=@"SELECT
KDFID, KDFTYP, KDFIPB, KDFALX, KDFAVN
, KDFHIN, KDFFOR, KDFOPT, KDFGARAN, KDFKDEID
, KDFGAN, KDFPERI, KDFRSQ, KDFOBJ, KDFINVEN
, KDFINVEP, KDFCRU, KDFCRD, KDFMAJU, KDFMAJD
, KDFPRV, KDFPRA, KDFPRW, KDFPRU, KDFTYC
, KDFMNT FROM HPGARAP
inner join HPFOR on
KDATYP = KDFTYP
AND
KDAIPB=KDFIPB
AND
KDAALX=KDFALX
AND
KDAFOR=KDFFOR
AND
KDAAVN=KDFAVN
WHERE KDAID = :idFormule
and KDAAVN = :numAvenant
";
            const string select_GetByAffaire=@"SELECT
KDFID, KDFTYP, KDFIPB, KDFALX, KDFAVN
, KDFHIN, KDFFOR, KDFOPT, KDFGARAN, KDFKDEID
, KDFGAN, KDFPERI, KDFRSQ, KDFOBJ, KDFINVEN
, KDFINVEP, KDFCRU, KDFCRD, KDFMAJU, KDFMAJD
, KDFPRV, KDFPRA, KDFPRW, KDFPRU, KDFTYC
, KDFMNT FROM HPGARAP
WHERE KDFTYP = :tupeAffaire
and KDFIPB = :codeAffaire
and KDFALX = :numeroAliment
and KDFAVN = :numeroAvenant
";
            const string select_GetByIpbAlx=@"SELECT
KDFID, KDFTYP, KDFIPB, KDFALX, KDFAVN
, KDFHIN, KDFFOR, KDFOPT, KDFGARAN, KDFKDEID
, KDFGAN, KDFPERI, KDFRSQ, KDFOBJ, KDFINVEN
, KDFINVEP, KDFCRU, KDFCRD, KDFMAJU, KDFMAJD
, KDFPRV, KDFPRA, KDFPRW, KDFPRU, KDFTYC
, KDFMNT FROM HPGARAP
WHERE KDFIPB = :parKDFIPB
and KDFALX = :parKDFALX
";
            #endregion

            public HpgarapRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpGarAp Get(Int64 parKDFID){
                return connection.Query<KpGarAp>(select, new {parKDFID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KDFID") ;
            }

            public void Insert(KpGarAp value){
                    if(value.Kdfid == default(Int64)) {
                        value.Kdfid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KDFID",value.Kdfid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDFTYP",value.Kdftyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDFIPB",value.Kdfipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDFALX",value.Kdfalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDFAVN",value.Kdfavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDFHIN",value.Kdfhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDFFOR",value.Kdffor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDFOPT",value.Kdfopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDFGARAN",value.Kdfgaran??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDFKDEID",value.Kdfkdeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDFGAN",value.Kdfgan??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDFPERI",value.Kdfperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDFRSQ",value.Kdfrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDFOBJ",value.Kdfobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDFINVEN",value.Kdfinven, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDFINVEP",value.Kdfinvep, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDFCRU",value.Kdfcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDFCRD",value.Kdfcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDFMAJU",value.Kdfmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDFMAJD",value.Kdfmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDFPRV",value.Kdfprv, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:16, scale: 4);
                    parameters.Add("KDFPRA",value.Kdfpra, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:16, scale: 4);
                    parameters.Add("KDFPRW",value.Kdfprw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:16, scale: 4);
                    parameters.Add("KDFPRU",value.Kdfpru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDFTYC",value.Kdftyc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDFMNT",value.Kdfmnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:15, scale: 2);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpGarAp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("parKDFID",value.Kdfid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpGarAp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDFID",value.Kdfid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDFTYP",value.Kdftyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDFIPB",value.Kdfipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDFALX",value.Kdfalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDFAVN",value.Kdfavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDFHIN",value.Kdfhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDFFOR",value.Kdffor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDFOPT",value.Kdfopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDFGARAN",value.Kdfgaran??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDFKDEID",value.Kdfkdeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDFGAN",value.Kdfgan??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDFPERI",value.Kdfperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDFRSQ",value.Kdfrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDFOBJ",value.Kdfobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDFINVEN",value.Kdfinven, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDFINVEP",value.Kdfinvep, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDFCRU",value.Kdfcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDFCRD",value.Kdfcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDFMAJU",value.Kdfmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDFMAJD",value.Kdfmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDFPRV",value.Kdfprv, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:16, scale: 4);
                    parameters.Add("KDFPRA",value.Kdfpra, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:16, scale: 4);
                    parameters.Add("KDFPRW",value.Kdfprw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:16, scale: 4);
                    parameters.Add("KDFPRU",value.Kdfpru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDFTYC",value.Kdftyc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDFMNT",value.Kdfmnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:15, scale: 2);
                    parameters.Add("parKDFID",value.Kdfid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpGarAp> GetByFormule(Int64 idFormule, int numAvenant){
                    return connection.EnsureOpened().Query<KpGarAp>(select_GetByFormule, new {idFormule, numAvenant}).ToList();
            }
            public IEnumerable<KpGarAp> GetByAffaire(string tupeAffaire, string codeAffaire, int numeroAliment, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpGarAp>(select_GetByAffaire, new {tupeAffaire, codeAffaire, numeroAliment, numeroAvenant}).ToList();
            }
            public IEnumerable<KpGarAp> GetByIpbAlx(string parKDFIPB, int parKDFALX){
                    return connection.EnsureOpened().Query<KpGarAp>(select_GetByIpbAlx, new {parKDFIPB, parKDFALX}).ToList();
            }
    }
}
