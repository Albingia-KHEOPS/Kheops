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

    public  partial class  KnmGriRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KHJNMG, KHJDESI, KHJTYPO1, KHJLIB1, KHJLIEN1
, KHJVALF1, KHJTYPO2, KHJLIB2, KHJLIEN2, KHJVALF2
, KHJTYPO3, KHJLIB3, KHJLIEN3, KHJVALF3, KHJTYPO4
, KHJLIB4, KHJLIEN4, KHJVALF4, KHJTYPO5, KHJLIB5
, KHJLIEN5, KHJVALF5 FROM KNMGRI
WHERE KHJNMG = :codeGrille
";
            const string update=@"UPDATE KNMGRI SET 
KHJNMG = :KHJNMG, KHJDESI = :KHJDESI, KHJTYPO1 = :KHJTYPO1, KHJLIB1 = :KHJLIB1, KHJLIEN1 = :KHJLIEN1, KHJVALF1 = :KHJVALF1, KHJTYPO2 = :KHJTYPO2, KHJLIB2 = :KHJLIB2, KHJLIEN2 = :KHJLIEN2, KHJVALF2 = :KHJVALF2
, KHJTYPO3 = :KHJTYPO3, KHJLIB3 = :KHJLIB3, KHJLIEN3 = :KHJLIEN3, KHJVALF3 = :KHJVALF3, KHJTYPO4 = :KHJTYPO4, KHJLIB4 = :KHJLIB4, KHJLIEN4 = :KHJLIEN4, KHJVALF4 = :KHJVALF4, KHJTYPO5 = :KHJTYPO5, KHJLIB5 = :KHJLIB5
, KHJLIEN5 = :KHJLIEN5, KHJVALF5 = :KHJVALF5
 WHERE 
KHJNMG = :codeGrille";
            const string delete=@"DELETE FROM KNMGRI WHERE KHJNMG = :codeGrille";
            const string insert=@"INSERT INTO  KNMGRI (
KHJNMG, KHJDESI, KHJTYPO1, KHJLIB1, KHJLIEN1
, KHJVALF1, KHJTYPO2, KHJLIB2, KHJLIEN2, KHJVALF2
, KHJTYPO3, KHJLIB3, KHJLIEN3, KHJVALF3, KHJTYPO4
, KHJLIB4, KHJLIEN4, KHJVALF4, KHJTYPO5, KHJLIB5
, KHJLIEN5, KHJVALF5
) VALUES (
:KHJNMG, :KHJDESI, :KHJTYPO1, :KHJLIB1, :KHJLIEN1
, :KHJVALF1, :KHJTYPO2, :KHJLIB2, :KHJLIEN2, :KHJVALF2
, :KHJTYPO3, :KHJLIB3, :KHJLIEN3, :KHJVALF3, :KHJTYPO4
, :KHJLIB4, :KHJLIEN4, :KHJVALF4, :KHJTYPO5, :KHJLIB5
, :KHJLIEN5, :KHJVALF5)";
            const string select_GetAll=@"SELECT
KHJNMG, KHJDESI, KHJTYPO1, KHJLIB1, KHJLIEN1
, KHJVALF1, KHJTYPO2, KHJLIB2, KHJLIEN2, KHJVALF2
, KHJTYPO3, KHJLIB3, KHJLIEN3, KHJVALF3, KHJTYPO4
, KHJLIB4, KHJLIEN4, KHJVALF4, KHJTYPO5, KHJLIB5
, KHJLIEN5, KHJVALF5 FROM KNMGRI
";
            #endregion

            public KnmGriRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KnmGri Get(string codeGrille){
                return connection.Query<KnmGri>(select, new {codeGrille}).SingleOrDefault();
            }

            public string NewId () {
                return idGenerator.NewId("KHJNMG").ToString() ;
            }

            public void Insert(KnmGri value){
                    if(value.Khjnmg == default(string)) {
                        value.Khjnmg = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KHJNMG",value.Khjnmg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJDESI",value.Khjdesi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KHJTYPO1",value.Khjtypo1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIB1",value.Khjlib1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIEN1",value.Khjlien1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHJVALF1",value.Khjvalf1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHJTYPO2",value.Khjtypo2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIB2",value.Khjlib2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIEN2",value.Khjlien2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHJVALF2",value.Khjvalf2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHJTYPO3",value.Khjtypo3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIB3",value.Khjlib3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIEN3",value.Khjlien3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHJVALF3",value.Khjvalf3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHJTYPO4",value.Khjtypo4??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIB4",value.Khjlib4??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIEN4",value.Khjlien4??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHJVALF4",value.Khjvalf4??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHJTYPO5",value.Khjtypo5??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIB5",value.Khjlib5??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIEN5",value.Khjlien5??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHJVALF5",value.Khjvalf5??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KnmGri value){
                    var parameters = new DynamicParameters();
                    parameters.Add("codeGrille",value.Khjnmg);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KnmGri value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KHJNMG",value.Khjnmg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJDESI",value.Khjdesi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KHJTYPO1",value.Khjtypo1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIB1",value.Khjlib1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIEN1",value.Khjlien1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHJVALF1",value.Khjvalf1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHJTYPO2",value.Khjtypo2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIB2",value.Khjlib2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIEN2",value.Khjlien2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHJVALF2",value.Khjvalf2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHJTYPO3",value.Khjtypo3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIB3",value.Khjlib3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIEN3",value.Khjlien3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHJVALF3",value.Khjvalf3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHJTYPO4",value.Khjtypo4??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIB4",value.Khjlib4??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIEN4",value.Khjlien4??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHJVALF4",value.Khjvalf4??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHJTYPO5",value.Khjtypo5??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIB5",value.Khjlib5??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHJLIEN5",value.Khjlien5??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHJVALF5",value.Khjvalf5??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("codeGrille",value.Khjnmg);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KnmGri> GetAll(){
                    return connection.EnsureOpened().Query<KnmGri>(select_GetAll).ToList();
            }
    }
}
