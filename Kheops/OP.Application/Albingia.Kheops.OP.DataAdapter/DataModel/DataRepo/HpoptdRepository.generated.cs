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

    public  partial class  HpoptdRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDCID, KDCTYP, KDCIPB, KDCALX, KDCAVN
, KDCHIN, KDCFOR, KDCOPT, KDCKDBID, KDCTENG
, KDCKAKID, KDCKAEID, KDCKAQID, KDCMODELE, KDCKARID
, KDCCRU, KDCCRD, KDCMAJU, KDCMAJD, KDCFLAG
, KDCORDRE FROM HPOPTD
WHERE KDCID = :id
and KDCAVN = :numeroAvenant
";
            const string update=@"UPDATE HPOPTD SET 
KDCID = :KDCID, KDCTYP = :KDCTYP, KDCIPB = :KDCIPB, KDCALX = :KDCALX, KDCAVN = :KDCAVN, KDCHIN = :KDCHIN, KDCFOR = :KDCFOR, KDCOPT = :KDCOPT, KDCKDBID = :KDCKDBID, KDCTENG = :KDCTENG
, KDCKAKID = :KDCKAKID, KDCKAEID = :KDCKAEID, KDCKAQID = :KDCKAQID, KDCMODELE = :KDCMODELE, KDCKARID = :KDCKARID, KDCCRU = :KDCCRU, KDCCRD = :KDCCRD, KDCMAJU = :KDCMAJU, KDCMAJD = :KDCMAJD, KDCFLAG = :KDCFLAG
, KDCORDRE = :KDCORDRE
 WHERE 
KDCID = :id and KDCAVN = :numeroAvenant";
            const string delete=@"DELETE FROM HPOPTD WHERE KDCID = :id AND KDCAVN = :numeroAvenant";
            const string insert=@"INSERT INTO  HPOPTD (
KDCID, KDCTYP, KDCIPB, KDCALX, KDCAVN
, KDCHIN, KDCFOR, KDCOPT, KDCKDBID, KDCTENG
, KDCKAKID, KDCKAEID, KDCKAQID, KDCMODELE, KDCKARID
, KDCCRU, KDCCRD, KDCMAJU, KDCMAJD, KDCFLAG
, KDCORDRE
) VALUES (
:KDCID, :KDCTYP, :KDCIPB, :KDCALX, :KDCAVN
, :KDCHIN, :KDCFOR, :KDCOPT, :KDCKDBID, :KDCTENG
, :KDCKAKID, :KDCKAEID, :KDCKAQID, :KDCMODELE, :KDCKARID
, :KDCCRU, :KDCCRD, :KDCMAJU, :KDCMAJD, :KDCFLAG
, :KDCORDRE)";
            const string select_GetByAffaire=@"SELECT
KDCID, KDCTYP, KDCIPB, KDCALX, KDCAVN
, KDCHIN, KDCFOR, KDCOPT, KDCKDBID, KDCTENG
, KDCKAKID, KDCKAEID, KDCKAQID, KDCMODELE, KDCKARID
, KDCCRU, KDCCRD, KDCMAJU, KDCMAJD, KDCFLAG
, KDCORDRE FROM HPOPTD
WHERE KDCTYP = :type
and KDCIPB = :numeroAffaire
and KDCALX = :numeroAliment
and KDCAVN = :numeroAvenant
";
            const string select_GetByFormule=@"SELECT
KDCID, KDCTYP, KDCIPB, KDCALX, KDCAVN
, KDCHIN, KDCFOR, KDCOPT, KDCKDBID, KDCTENG
, KDCKAKID, KDCKAEID, KDCKAQID, KDCMODELE, KDCKARID
, KDCCRU, KDCCRD, KDCMAJU, KDCMAJD, KDCFLAG
, KDCORDRE FROM HPOPTD
inner join HPOPT on KDCKDBID = KDBID
WHERE KDBKDAID = :idFormule
and KDBAVN = :numeroAvenant
";
            const string select_GetByOption=@"SELECT
KDCID, KDCTYP, KDCIPB, KDCALX, KDCAVN
, KDCHIN, KDCFOR, KDCOPT, KDCKDBID, KDCTENG
, KDCKAKID, KDCKAEID, KDCKAQID, KDCMODELE, KDCKARID
, KDCCRU, KDCCRD, KDCMAJU, KDCMAJD, KDCFLAG
, KDCORDRE FROM HPOPTD
WHERE KDCKDBID = :idOption
and KDCAVN = :numeroAvenant
";
            #endregion

            public HpoptdRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpOptD Get(Int64 id, int numeroAvenant){
                return connection.Query<KpOptD>(select, new {id, numeroAvenant}).SingleOrDefault();
            }


            public void Insert(KpOptD value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDCID",value.Kdcid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDCTYP",value.Kdctyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDCIPB",value.Kdcipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDCALX",value.Kdcalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDCAVN",value.Kdcavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDCHIN",value.Kdchin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
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
                    parameters.Add("id",value.Kdcid);
                    parameters.Add("numeroAvenant",value.Kdcavn);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpOptD value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDCID",value.Kdcid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDCTYP",value.Kdctyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDCIPB",value.Kdcipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDCALX",value.Kdcalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDCAVN",value.Kdcavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDCHIN",value.Kdchin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
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
                    parameters.Add("id",value.Kdcid);
                    parameters.Add("numeroAvenant",value.Kdcavn);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpOptD> GetByAffaire(string type, string numeroAffaire, int numeroAliment, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpOptD>(select_GetByAffaire, new {type, numeroAffaire, numeroAliment, numeroAvenant}).ToList();
            }
            public IEnumerable<KpOptD> GetByFormule(Int64 idFormule, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpOptD>(select_GetByFormule, new {idFormule, numeroAvenant}).ToList();
            }
            public IEnumerable<KpOptD> GetByOption(Int64 idOption, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpOptD>(select_GetByOption, new {idOption, numeroAvenant}).ToList();
            }
    }
}
