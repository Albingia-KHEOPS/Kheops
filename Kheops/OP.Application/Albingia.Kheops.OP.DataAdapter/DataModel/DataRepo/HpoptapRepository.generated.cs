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

    public  partial class  HpoptapRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDDID, KDDTYP, KDDIPB, KDDALX, KDDAVN
, KDDHIN, KDDFOR, KDDOPT, KDDKDBID, KDDPERI
, KDDRSQ, KDDOBJ, KDDINVEN, KDDINVEP, KDDCRU
, KDDCRD, KDDMAJU, KDDMAJD FROM HPOPTAP
WHERE KDDID = :id
and KDDAVN = :numeroAvenant
";
            const string update=@"UPDATE HPOPTAP SET 
KDDID = :KDDID, KDDTYP = :KDDTYP, KDDIPB = :KDDIPB, KDDALX = :KDDALX, KDDAVN = :KDDAVN, KDDHIN = :KDDHIN, KDDFOR = :KDDFOR, KDDOPT = :KDDOPT, KDDKDBID = :KDDKDBID, KDDPERI = :KDDPERI
, KDDRSQ = :KDDRSQ, KDDOBJ = :KDDOBJ, KDDINVEN = :KDDINVEN, KDDINVEP = :KDDINVEP, KDDCRU = :KDDCRU, KDDCRD = :KDDCRD, KDDMAJU = :KDDMAJU, KDDMAJD = :KDDMAJD
 WHERE 
KDDID = :id and KDDAVN = :numeroAvenant";
            const string delete=@"DELETE FROM HPOPTAP WHERE KDDID = :id AND KDDAVN = :numeroAvenant";
            const string insert=@"INSERT INTO  HPOPTAP (
KDDID, KDDTYP, KDDIPB, KDDALX, KDDAVN
, KDDHIN, KDDFOR, KDDOPT, KDDKDBID, KDDPERI
, KDDRSQ, KDDOBJ, KDDINVEN, KDDINVEP, KDDCRU
, KDDCRD, KDDMAJU, KDDMAJD
) VALUES (
:KDDID, :KDDTYP, :KDDIPB, :KDDALX, :KDDAVN
, :KDDHIN, :KDDFOR, :KDDOPT, :KDDKDBID, :KDDPERI
, :KDDRSQ, :KDDOBJ, :KDDINVEN, :KDDINVEP, :KDDCRU
, :KDDCRD, :KDDMAJU, :KDDMAJD)";
            const string select_GetByAffaire=@"SELECT
KDDID, KDDTYP, KDDIPB, KDDALX, KDDAVN
, KDDHIN, KDDFOR, KDDOPT, KDDKDBID, KDDPERI
, KDDRSQ, KDDOBJ, KDDINVEN, KDDINVEP, KDDCRU
, KDDCRD, KDDMAJU, KDDMAJD FROM HPOPTAP
WHERE KDDTYP = :type
and KDDIPB = :numeroAffaire
and KDDALX = :numeroAliment
and KDDAVN = :numeroAvenant
";
            const string select_GetByFormule=@"SELECT
KDDID, KDDTYP, KDDIPB, KDDALX, KDDAVN
, KDDHIN, KDDFOR, KDDOPT, KDDKDBID, KDDPERI
, KDDRSQ, KDDOBJ, KDDINVEN, KDDINVEP, KDDCRU
, KDDCRD, KDDMAJU, KDDMAJD FROM HPOPTAP
INNER JOIN HPTOPT on KDBID = KDDKDBID and KDDAVN = KDBAVN
WHERE KDBKDAID = :formuleId
and KDBAVN = :numeroAvenant
";
            const string select_GetByIpbAlx=@"SELECT
KDDID, KDDTYP, KDDIPB, KDDALX, KDDAVN
, KDDHIN, KDDFOR, KDDOPT, KDDKDBID, KDDPERI
, KDDRSQ, KDDOBJ, KDDINVEN, KDDINVEP, KDDCRU
, KDDCRD, KDDMAJU, KDDMAJD FROM HPOPTAP
WHERE KDDIPB = :parKDDIPB
and KDDALX = :parKDDALX
";
            #endregion

            public HpoptapRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpOptAp Get(Int64 id, int numeroAvenant){
                return connection.Query<KpOptAp>(select, new {id, numeroAvenant}).SingleOrDefault();
            }


            public void Insert(KpOptAp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDDID",value.Kddid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDDTYP",value.Kddtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDDIPB",value.Kddipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDDALX",value.Kddalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDDAVN",value.Kddavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDDHIN",value.Kddhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDDFOR",value.Kddfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDDOPT",value.Kddopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDDKDBID",value.Kddkdbid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDDPERI",value.Kddperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDDRSQ",value.Kddrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDDOBJ",value.Kddobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDDINVEN",value.Kddinven, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDDINVEP",value.Kddinvep, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDDCRU",value.Kddcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDDCRD",value.Kddcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDDMAJU",value.Kddmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDDMAJD",value.Kddmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpOptAp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Kddid);
                    parameters.Add("numeroAvenant",value.Kddavn);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpOptAp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDDID",value.Kddid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDDTYP",value.Kddtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDDIPB",value.Kddipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDDALX",value.Kddalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDDAVN",value.Kddavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDDHIN",value.Kddhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDDFOR",value.Kddfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDDOPT",value.Kddopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDDKDBID",value.Kddkdbid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDDPERI",value.Kddperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDDRSQ",value.Kddrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDDOBJ",value.Kddobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDDINVEN",value.Kddinven, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDDINVEP",value.Kddinvep, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDDCRU",value.Kddcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDDCRD",value.Kddcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDDMAJU",value.Kddmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDDMAJD",value.Kddmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("id",value.Kddid);
                    parameters.Add("numeroAvenant",value.Kddavn);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpOptAp> GetByAffaire(string type, string numeroAffaire, int numeroAliment, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpOptAp>(select_GetByAffaire, new {type, numeroAffaire, numeroAliment, numeroAvenant}).ToList();
            }
            public IEnumerable<KpOptAp> GetByFormule(Int64 formuleId, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpOptAp>(select_GetByFormule, new {formuleId, numeroAvenant}).ToList();
            }
            public IEnumerable<KpOptAp> GetByIpbAlx(string parKDDIPB, int parKDDALX){
                    return connection.EnsureOpened().Query<KpOptAp>(select_GetByIpbAlx, new {parKDDIPB, parKDDALX}).ToList();
            }
    }
}
