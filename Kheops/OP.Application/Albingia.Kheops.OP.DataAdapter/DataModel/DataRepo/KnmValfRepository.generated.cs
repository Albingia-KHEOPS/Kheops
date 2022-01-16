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

    public  partial class  KnmValfRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KHKID, KHKNMG, KHKTYPO, KHKORDR, KHKNIV
, KHKMER, KHKKHIID FROM KNMVALF
WHERE KHKID = :id
";
            const string update=@"UPDATE KNMVALF SET 
KHKID = :KHKID, KHKNMG = :KHKNMG, KHKTYPO = :KHKTYPO, KHKORDR = :KHKORDR, KHKNIV = :KHKNIV, KHKMER = :KHKMER, KHKKHIID = :KHKKHIID
 WHERE 
KHKID = :id";
            const string delete=@"DELETE FROM KNMVALF WHERE KHKID = :id";
            const string insert=@"INSERT INTO  KNMVALF (
KHKID, KHKNMG, KHKTYPO, KHKORDR, KHKNIV
, KHKMER, KHKKHIID
) VALUES (
:KHKID, :KHKNMG, :KHKTYPO, :KHKORDR, :KHKNIV
, :KHKMER, :KHKKHIID)";
            const string select_GetAll=@"SELECT
KHKID, KHKNMG, KHKTYPO, KHKORDR, KHKNIV
, KHKMER, KHKKHIID FROM KNMVALF
";
            #endregion

            public KnmValfRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KnmValf Get(Int64 id){
                return connection.Query<KnmValf>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KHKID") ;
            }

            public void Insert(KnmValf value){
                    if(value.Khkid == default(Int64)) {
                        value.Khkid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KHKID",value.Khkid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHKNMG",value.Khknmg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHKTYPO",value.Khktypo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHKORDR",value.Khkordr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KHKNIV",value.Khkniv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHKMER",value.Khkmer, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHKKHIID",value.Khkkhiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KnmValf value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Khkid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KnmValf value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KHKID",value.Khkid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHKNMG",value.Khknmg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHKTYPO",value.Khktypo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHKORDR",value.Khkordr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KHKNIV",value.Khkniv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHKMER",value.Khkmer, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHKKHIID",value.Khkkhiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("id",value.Khkid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KnmValf> GetAll(){
                    return connection.EnsureOpened().Query<KnmValf>(select_GetAll).ToList();
            }
    }
}
