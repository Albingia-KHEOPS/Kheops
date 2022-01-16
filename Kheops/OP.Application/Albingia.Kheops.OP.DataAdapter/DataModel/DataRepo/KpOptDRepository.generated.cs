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

    public  partial class  KpOptDRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDCID, KDCTYP, KDCIPB, KDCALX, KDCFOR
, KDCOPT, KDCKDBID, KDCTENG, KDCKAKID, KDCKAEID
, KDCKAQID, KDCMODELE, KDCKARID, KDCCRU, KDCCRD
, KDCMAJU, KDCMAJD, KDCFLAG, KDCORDRE FROM KPOPTD
WHERE KDCID = :parKDCID
";
            const string update=@"UPDATE KPOPTD SET 
KDCID = :KDCID, KDCTYP = :KDCTYP, KDCIPB = :KDCIPB, KDCALX = :KDCALX, KDCFOR = :KDCFOR, KDCOPT = :KDCOPT, KDCKDBID = :KDCKDBID, KDCTENG = :KDCTENG, KDCKAKID = :KDCKAKID, KDCKAEID = :KDCKAEID
, KDCKAQID = :KDCKAQID, KDCMODELE = :KDCMODELE, KDCKARID = :KDCKARID, KDCCRU = :KDCCRU, KDCCRD = :KDCCRD, KDCMAJU = :KDCMAJU, KDCMAJD = :KDCMAJD, KDCFLAG = :KDCFLAG, KDCORDRE = :KDCORDRE
 WHERE 
KDCID = :parKDCID";
            const string delete=@"DELETE FROM KPOPTD WHERE KDCID = :parKDCID";
            const string insert=@"INSERT INTO  KPOPTD (
KDCID, KDCTYP, KDCIPB, KDCALX, KDCFOR
, KDCOPT, KDCKDBID, KDCTENG, KDCKAKID, KDCKAEID
, KDCKAQID, KDCMODELE, KDCKARID, KDCCRU, KDCCRD
, KDCMAJU, KDCMAJD, KDCFLAG, KDCORDRE
) VALUES (
:KDCID, :KDCTYP, :KDCIPB, :KDCALX, :KDCFOR
, :KDCOPT, :KDCKDBID, :KDCTENG, :KDCKAKID, :KDCKAEID
, :KDCKAQID, :KDCMODELE, :KDCKARID, :KDCCRU, :KDCCRD
, :KDCMAJU, :KDCMAJD, :KDCFLAG, :KDCORDRE)";
            const string select_GetByAffaire=@"SELECT
KDCID, KDCTYP, KDCIPB, KDCALX, KDCFOR
, KDCOPT, KDCKDBID, KDCTENG, KDCKAKID, KDCKAEID
, KDCKAQID, KDCMODELE, KDCKARID, KDCCRU, KDCCRD
, KDCMAJU, KDCMAJD, KDCFLAG, KDCORDRE FROM KPOPTD
WHERE KDCTYP = :typeAffaire
and KDCIPB = :codeAffaire
and KDCALX = :version
";
            const string select_GetByFormule=@"SELECT
KDCID, KDCTYP, KDCIPB, KDCALX, KDCFOR
, KDCOPT, KDCKDBID, KDCTENG, KDCKAKID, KDCKAEID
, KDCKAQID, KDCMODELE, KDCKARID, KDCCRU, KDCCRD
, KDCMAJU, KDCMAJD, KDCFLAG, KDCORDRE FROM KPOPTD
inner join KPOPT ON KDBID = KDCKDBID
WHERE KDBKDAID = :formuleId
";
            const string select_GetByOption=@"SELECT
KDCID, KDCTYP, KDCIPB, KDCALX, KDCFOR
, KDCOPT, KDCKDBID, KDCTENG, KDCKAKID, KDCKAEID
, KDCKAQID, KDCMODELE, KDCKARID, KDCCRU, KDCCRD
, KDCMAJU, KDCMAJD, KDCFLAG, KDCORDRE FROM KPOPTD
WHERE KDCKDBID = :optionId
";
            #endregion

            public KpOptDRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpOptD Get(Int64 parKDCID){
                return connection.Query<KpOptD>(select, new {parKDCID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KDCID") ;
            }

            public void Insert(KpOptD value){
                    if(value.Kdcid == default(Int64)) {
                        value.Kdcid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KDCID",value.Kdcid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDCTYP",value.Kdctyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDCIPB",value.Kdcipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDCALX",value.Kdcalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDCFOR",value.Kdcfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDCOPT",value.Kdcopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDCKDBID",value.Kdckdbid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDCTENG",value.Kdcteng??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDCKAKID",value.Kdckakid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDCKAEID",value.Kdckaeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDCKAQID",value.Kdckaqid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDCMODELE",value.Kdcmodele??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDCKARID",value.Kdckarid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDCCRU",value.Kdccru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDCCRD",value.Kdccrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDCMAJU",value.Kdcmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDCMAJD",value.Kdcmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDCFLAG",value.Kdcflag, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDCORDRE",value.Kdcordre, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpOptD value){
                    var parameters = new DynamicParameters();
                    parameters.Add("parKDCID",value.Kdcid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpOptD value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDCID",value.Kdcid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDCTYP",value.Kdctyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDCIPB",value.Kdcipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDCALX",value.Kdcalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDCFOR",value.Kdcfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDCOPT",value.Kdcopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDCKDBID",value.Kdckdbid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDCTENG",value.Kdcteng??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDCKAKID",value.Kdckakid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDCKAEID",value.Kdckaeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDCKAQID",value.Kdckaqid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDCMODELE",value.Kdcmodele??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDCKARID",value.Kdckarid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDCCRU",value.Kdccru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDCCRD",value.Kdccrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDCMAJU",value.Kdcmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDCMAJD",value.Kdcmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDCFLAG",value.Kdcflag, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDCORDRE",value.Kdcordre, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("parKDCID",value.Kdcid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpOptD> GetByAffaire(string typeAffaire, string codeAffaire, int version){
                    return connection.EnsureOpened().Query<KpOptD>(select_GetByAffaire, new {typeAffaire, codeAffaire, version}).ToList();
            }
            public IEnumerable<KpOptD> GetByFormule(Int64 formuleId){
                    return connection.EnsureOpened().Query<KpOptD>(select_GetByFormule, new {formuleId}).ToList();
            }
            public IEnumerable<KpOptD> GetByOption(Int64 optionId){
                    return connection.EnsureOpened().Query<KpOptD>(select_GetByOption, new {optionId}).ToList();
            }
    }
}
