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

    public  partial class  KedilogRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
IDSESSION, TYP, IPB, ALX, STATUT
, METHODE, DATEHEURE, INFO, SEQ FROM KEDILOG
WHERE SEQ = :sequence
FETCH FIRST 200 ROWS ONLY
";
            const string update=@"UPDATE KEDILOG SET 
IDSESSION = :IDSESSION, TYP = :TYP, IPB = :IPB, ALX = :ALX, STATUT = :STATUT, METHODE = :METHODE, DATEHEURE = :DATEHEURE, INFO = :INFO, SEQ = :SEQ
 WHERE 
SEQ = :sequence";
            const string delete=@"DELETE FROM KEDILOG WHERE SEQ = :sequence";
            const string insert=@"INSERT INTO  KEDILOG (
IDSESSION, TYP, IPB, ALX, STATUT
, METHODE, DATEHEURE, INFO, SEQ
) VALUES (
:IDSESSION, :TYP, :IPB, :ALX, :STATUT
, :METHODE, :DATEHEURE, :INFO, :SEQ)";
            #endregion

            public KedilogRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public Kedilog Get(string sequence){
                return connection.Query<Kedilog>(select, new {sequence}).SingleOrDefault();
            }

            public string NewId () {
                return idGenerator.NewId("SEQ").ToString() ;
            }

            public void Insert(Kedilog value){
                    if(value.Seq == default(string)) {
                        value.Seq = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("IDSESSION",value.Idsession, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("TYP",value.Typ??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("IPB",value.Ipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("ALX",value.Alx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("STATUT",value.Statut??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("METHODE",value.Methode??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("DATEHEURE",value.Dateheure??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("INFO",value.Info??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:500, scale: 0);
                    parameters.Add("SEQ",value.Seq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:8, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(Kedilog value){
                    var parameters = new DynamicParameters();
                    parameters.Add("sequence",value.Seq);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(Kedilog value){
                    var parameters = new DynamicParameters();
                    parameters.Add("IDSESSION",value.Idsession, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("TYP",value.Typ??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("IPB",value.Ipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("ALX",value.Alx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("STATUT",value.Statut??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("METHODE",value.Methode??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("DATEHEURE",value.Dateheure??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("INFO",value.Info??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:500, scale: 0);
                    parameters.Add("SEQ",value.Seq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("sequence",value.Seq);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
    }
}
