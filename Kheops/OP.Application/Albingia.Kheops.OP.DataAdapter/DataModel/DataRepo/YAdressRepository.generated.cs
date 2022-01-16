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

    public  partial class  YAdressRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
ABPCHR, ABPLG3, ABPNUM, ABPEXT, ABPLBN
, ABPLG4, ABPL4F, ABPLG5, ABPDP6, ABPCP6
, ABPVI6, ABPCDX, ABPCEX, ABPL6F, ABPPAY
, ABPLOC, ABPMAT, ABPRET, ABPERR, ABPMJU
, ABPMJA, ABPMJM, ABPMJJ, ABPVIX FROM YADRESS
WHERE ABPCHR = :ABPCHR
";
            const string update=@"UPDATE YADRESS SET 
ABPCHR = :ABPCHR, ABPLG3 = :ABPLG3, ABPNUM = :ABPNUM, ABPEXT = :ABPEXT, ABPLBN = :ABPLBN, ABPLG4 = :ABPLG4, ABPL4F = :ABPL4F, ABPLG5 = :ABPLG5, ABPDP6 = :ABPDP6, ABPCP6 = :ABPCP6
, ABPVI6 = :ABPVI6, ABPCDX = :ABPCDX, ABPCEX = :ABPCEX, ABPL6F = :ABPL6F, ABPPAY = :ABPPAY, ABPLOC = :ABPLOC, ABPMAT = :ABPMAT, ABPRET = :ABPRET, ABPERR = :ABPERR, ABPMJU = :ABPMJU
, ABPMJA = :ABPMJA, ABPMJM = :ABPMJM, ABPMJJ = :ABPMJJ, ABPVIX = :ABPVIX
 WHERE 
ABPCHR = :ABPCHR";
            const string delete=@"DELETE FROM YADRESS WHERE ABPCHR = :ABPCHR";
            const string insert=@"INSERT INTO  YADRESS (
ABPCHR, ABPLG3, ABPNUM, ABPEXT, ABPLBN
, ABPLG4, ABPL4F, ABPLG5, ABPDP6, ABPCP6
, ABPVI6, ABPCDX, ABPCEX, ABPL6F, ABPPAY
, ABPLOC, ABPMAT, ABPRET, ABPERR, ABPMJU
, ABPMJA, ABPMJM, ABPMJJ, ABPVIX
) VALUES (
:ABPCHR, :ABPLG3, :ABPNUM, :ABPEXT, :ABPLBN
, :ABPLG4, :ABPL4F, :ABPLG5, :ABPDP6, :ABPCP6
, :ABPVI6, :ABPCDX, :ABPCEX, :ABPL6F, :ABPPAY
, :ABPLOC, :ABPMAT, :ABPRET, :ABPERR, :ABPMJU
, :ABPMJA, :ABPMJM, :ABPMJJ, :ABPVIX)";
            #endregion

            public YAdressRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YAdress Get(int ABPCHR){
                return connection.Query<YAdress>(select, new {ABPCHR}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("ABPCHR") ;
            }

            public void Insert(YAdress value){
                    if(value.Abpchr == default(int)) {
                        value.Abpchr = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("ABPCHR",value.Abpchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("ABPLG3",value.Abplg3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("ABPNUM",value.Abpnum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("ABPEXT",value.Abpext??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("ABPLBN",value.Abplbn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("ABPLG4",value.Abplg4??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("ABPL4F",value.Abpl4f??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("ABPLG5",value.Abplg5??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("ABPDP6",value.Abpdp6??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ABPCP6",value.Abpcp6, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("ABPVI6",value.Abpvi6??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("ABPCDX",value.Abpcdx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ABPCEX",value.Abpcex, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("ABPL6F",value.Abpl6f??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("ABPPAY",value.Abppay??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("ABPLOC",value.Abploc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("ABPMAT",value.Abpmat, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("ABPRET",value.Abpret??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("ABPERR",value.Abperr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("ABPMJU",value.Abpmju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("ABPMJA",value.Abpmja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("ABPMJM",value.Abpmjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ABPMJJ",value.Abpmjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ABPVIX",value.Abpvix??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YAdress value){
                    var parameters = new DynamicParameters();
                    parameters.Add("ABPCHR",value.Abpchr);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YAdress value){
                    var parameters = new DynamicParameters();
                    parameters.Add("ABPCHR",value.Abpchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("ABPLG3",value.Abplg3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("ABPNUM",value.Abpnum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("ABPEXT",value.Abpext??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("ABPLBN",value.Abplbn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("ABPLG4",value.Abplg4??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("ABPL4F",value.Abpl4f??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("ABPLG5",value.Abplg5??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("ABPDP6",value.Abpdp6??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ABPCP6",value.Abpcp6, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("ABPVI6",value.Abpvi6??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("ABPCDX",value.Abpcdx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ABPCEX",value.Abpcex, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("ABPL6F",value.Abpl6f??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("ABPPAY",value.Abppay??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("ABPLOC",value.Abploc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("ABPMAT",value.Abpmat, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("ABPRET",value.Abpret??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("ABPERR",value.Abperr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("ABPMJU",value.Abpmju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("ABPMJA",value.Abpmja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("ABPMJM",value.Abpmjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ABPMJJ",value.Abpmjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ABPVIX",value.Abpvix??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("ABPCHR",value.Abpchr);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
    }
}
