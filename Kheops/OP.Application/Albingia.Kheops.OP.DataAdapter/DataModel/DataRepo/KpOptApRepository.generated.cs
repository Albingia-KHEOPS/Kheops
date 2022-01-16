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

    public  partial class  KpOptApRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDDID, KDDTYP, KDDIPB, KDDALX, KDDFOR
, KDDOPT, KDDKDBID, KDDPERI, KDDRSQ, KDDOBJ
, KDDINVEN, KDDINVEP, KDDCRU, KDDCRD, KDDMAJU
, KDDMAJD FROM KPOPTAP
WHERE KDDID = :id
";
            const string update=@"UPDATE KPOPTAP SET 
KDDID = :KDDID, KDDTYP = :KDDTYP, KDDIPB = :KDDIPB, KDDALX = :KDDALX, KDDFOR = :KDDFOR, KDDOPT = :KDDOPT, KDDKDBID = :KDDKDBID, KDDPERI = :KDDPERI, KDDRSQ = :KDDRSQ, KDDOBJ = :KDDOBJ
, KDDINVEN = :KDDINVEN, KDDINVEP = :KDDINVEP, KDDCRU = :KDDCRU, KDDCRD = :KDDCRD, KDDMAJU = :KDDMAJU, KDDMAJD = :KDDMAJD
 WHERE 
KDDID = :id";
            const string delete=@"DELETE FROM KPOPTAP WHERE KDDID = :id";
            const string insert=@"INSERT INTO  KPOPTAP (
KDDID, KDDTYP, KDDIPB, KDDALX, KDDFOR
, KDDOPT, KDDKDBID, KDDPERI, KDDRSQ, KDDOBJ
, KDDINVEN, KDDINVEP, KDDCRU, KDDCRD, KDDMAJU
, KDDMAJD
) VALUES (
:KDDID, :KDDTYP, :KDDIPB, :KDDALX, :KDDFOR
, :KDDOPT, :KDDKDBID, :KDDPERI, :KDDRSQ, :KDDOBJ
, :KDDINVEN, :KDDINVEP, :KDDCRU, :KDDCRD, :KDDMAJU
, :KDDMAJD)";
            const string select_GetByAffaire=@"SELECT
KDDID, KDDTYP, KDDIPB, KDDALX, KDDFOR
, KDDOPT, KDDKDBID, KDDPERI, KDDRSQ, KDDOBJ
, KDDINVEN, KDDINVEP, KDDCRU, KDDCRD, KDDMAJU
, KDDMAJD FROM KPOPTAP
WHERE KDDTYP = :typeAffaire
and KDDIPB = :codeAffaire
and KDDALX = :version
";
            const string select_GetByFormule=@"SELECT
KDDID, KDDTYP, KDDIPB, KDDALX, KDDFOR
, KDDOPT, KDDKDBID, KDDPERI, KDDRSQ, KDDOBJ
, KDDINVEN, KDDINVEP, KDDCRU, KDDCRD, KDDMAJU
, KDDMAJD FROM KPOPTAP
INNER JOIN KPTOPT on KDBID = KDDKDBID
WHERE KDBKDAID = :formuleId
";
            #endregion

            public KpOptApRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpOptAp Get(Int64 id){
                return connection.Query<KpOptAp>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KDDID") ;
            }

            public void Insert(KpOptAp value){
                    if(value.Kddid == default(Int64)) {
                        value.Kddid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KDDID",value.Kddid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDDTYP",value.Kddtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDDIPB",value.Kddipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDDALX",value.Kddalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
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
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpOptAp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDDID",value.Kddid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDDTYP",value.Kddtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDDIPB",value.Kddipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDDALX",value.Kddalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
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
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpOptAp> GetByAffaire(string typeAffaire, string codeAffaire, int version){
                    return connection.EnsureOpened().Query<KpOptAp>(select_GetByAffaire, new {typeAffaire, codeAffaire, version}).ToList();
            }
            public IEnumerable<KpOptAp> GetByFormule(Int64 formuleId){
                    return connection.EnsureOpened().Query<KpOptAp>(select_GetByFormule, new {formuleId}).ToList();
            }
    }
}
