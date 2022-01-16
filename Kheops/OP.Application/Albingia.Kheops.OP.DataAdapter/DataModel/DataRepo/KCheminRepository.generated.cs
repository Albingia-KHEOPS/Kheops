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

    public  partial class  KCheminRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KHMCLE, KHMSRV, KHMRAC, KHMENV, KHMDES
, KHMTCH, KHMCHM, KHMCRU, KHMCRD, KHMMJU
, KHMMJD FROM KCHEMIN
WHERE KHMCLE = :KHMCLE
";
            const string update=@"UPDATE KCHEMIN SET 
KHMCLE = :KHMCLE, KHMSRV = :KHMSRV, KHMRAC = :KHMRAC, KHMENV = :KHMENV, KHMDES = :KHMDES, KHMTCH = :KHMTCH, KHMCHM = :KHMCHM, KHMCRU = :KHMCRU, KHMCRD = :KHMCRD, KHMMJU = :KHMMJU
, KHMMJD = :KHMMJD
 WHERE 
KHMCLE = :KHMCLE";
            const string delete=@"DELETE FROM KCHEMIN WHERE KHMCLE = :KHMCLE";
            const string insert=@"INSERT INTO  KCHEMIN (
KHMCLE, KHMSRV, KHMRAC, KHMENV, KHMDES
, KHMTCH, KHMCHM, KHMCRU, KHMCRD, KHMMJU
, KHMMJD
) VALUES (
:KHMCLE, :KHMSRV, :KHMRAC, :KHMENV, :KHMDES
, :KHMTCH, :KHMCHM, :KHMCRU, :KHMCRD, :KHMMJU
, :KHMMJD)";
            const string select_GetAll=@"SELECT
KHMCLE, KHMSRV, KHMRAC, KHMENV, KHMDES
, KHMTCH, KHMCHM, KHMCRU, KHMCRD, KHMMJU
, KHMMJD FROM KCHEMIN
";
            #endregion

            public KCheminRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KChemin Get(string KHMCLE){
                return connection.Query<KChemin>(select, new {KHMCLE}).SingleOrDefault();
            }

            public string NewId () {
                return idGenerator.NewId("KHMCLE").ToString() ;
            }

            public void Insert(KChemin value){
                    if(value.Khmcle == default(string)) {
                        value.Khmcle = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KHMCLE",value.Khmcle??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("KHMSRV",value.Khmsrv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KHMRAC",value.Khmrac??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KHMENV",value.Khmenv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KHMDES",value.Khmdes??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KHMTCH",value.Khmtch??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHMCHM",value.Khmchm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:150, scale: 0);
                    parameters.Add("KHMCRU",value.Khmcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHMCRD",value.Khmcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHMMJU",value.Khmmju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHMMJD",value.Khmmjd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KChemin value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KHMCLE",value.Khmcle);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KChemin value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KHMCLE",value.Khmcle??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("KHMSRV",value.Khmsrv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KHMRAC",value.Khmrac??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KHMENV",value.Khmenv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KHMDES",value.Khmdes??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KHMTCH",value.Khmtch??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHMCHM",value.Khmchm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:150, scale: 0);
                    parameters.Add("KHMCRU",value.Khmcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHMCRD",value.Khmcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHMMJU",value.Khmmju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHMMJD",value.Khmmjd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHMCLE",value.Khmcle);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KChemin> GetAll(){
                    return connection.EnsureOpened().Query<KChemin>(select_GetAll).ToList();
            }
    }
}
