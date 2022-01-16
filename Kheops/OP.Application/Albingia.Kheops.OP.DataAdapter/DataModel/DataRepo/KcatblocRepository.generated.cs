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

    public  partial class  KcatblocRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KAQID, KAQBRA, KAQCIBLE, KAQVOLET, KAQKAPID
, KAQBLOC, KAQKAEID, KAQCAR, KAQORDRE, KAQCRU
, KAQCRD, KAQCRH, KAQMAJU, KAQMAJD, KAQMAJH
 FROM KCATBLOC
WHERE KAQID = :IdUnique
FETCH FIRST 200 ROWS ONLY
";
            const string update=@"UPDATE KCATBLOC SET 
KAQID = :KAQID, KAQBRA = :KAQBRA, KAQCIBLE = :KAQCIBLE, KAQVOLET = :KAQVOLET, KAQKAPID = :KAQKAPID, KAQBLOC = :KAQBLOC, KAQKAEID = :KAQKAEID, KAQCAR = :KAQCAR, KAQORDRE = :KAQORDRE, KAQCRU = :KAQCRU
, KAQCRD = :KAQCRD, KAQCRH = :KAQCRH, KAQMAJU = :KAQMAJU, KAQMAJD = :KAQMAJD, KAQMAJH = :KAQMAJH
 WHERE 
KAQID = :IdUnique";
            const string delete=@"DELETE FROM KCATBLOC WHERE KAQID = :IdUnique";
            const string insert=@"INSERT INTO  KCATBLOC (
KAQID, KAQBRA, KAQCIBLE, KAQVOLET, KAQKAPID
, KAQBLOC, KAQKAEID, KAQCAR, KAQORDRE, KAQCRU
, KAQCRD, KAQCRH, KAQMAJU, KAQMAJD, KAQMAJH

) VALUES (
:KAQID, :KAQBRA, :KAQCIBLE, :KAQVOLET, :KAQKAPID
, :KAQBLOC, :KAQKAEID, :KAQCAR, :KAQORDRE, :KAQCRU
, :KAQCRD, :KAQCRH, :KAQMAJU, :KAQMAJD, :KAQMAJH
)";
            const string select_ListeBrancheCible=@"SELECT
KAQID AS KAQID, KAQBRA AS KAQBRA, KAQCIBLE AS KAQCIBLE, KAQBLOC AS KAQBLOC, KAQKAEID AS KAQKAEID
, KAQCAR AS KAQCAR, KAQORDRE AS KAQORDRE, KAEDESC AS KAEDESC, KAQKAPID AS KAQKAPID FROM KCATBLOC
Left join KBLOC on KAEID = KAQKAEID
where KAQKAPID
in
(Select KAPID from KCATVOLET where KAPBRA = :KAQBRA  and KAPCIBLE = :KAQCIBLE)
FETCH FIRST 200 ROWS ONLY
";
            const string select_GetAll=@"SELECT
KAQID, KAQBRA, KAQCIBLE, KAQVOLET, KAQKAPID
, KAQBLOC, KAQKAEID, KAQCAR, KAQORDRE, KAQCRU
, KAQCRD, KAQCRH, KAQMAJU, KAQMAJD, KAQMAJH
 FROM KCATBLOC
";
            #endregion

            public KcatblocRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public Kcatbloc Get(Int64 IdUnique){
                return connection.Query<Kcatbloc>(select, new {IdUnique}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KAQID") ;
            }

            public void Insert(Kcatbloc value){
                    if(value.Kaqid == default(Int64)) {
                        value.Kaqid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KAQID",value.Kaqid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAQBRA",value.Kaqbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KAQCIBLE",value.Kaqcible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAQVOLET",value.Kaqvolet??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAQKAPID",value.Kaqkapid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAQBLOC",value.Kaqbloc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAQKAEID",value.Kaqkaeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAQCAR",value.Kaqcar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAQORDRE",value.Kaqordre, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KAQCRU",value.Kaqcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAQCRD",value.Kaqcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAQCRH",value.Kaqcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAQMAJU",value.Kaqmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAQMAJD",value.Kaqmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAQMAJH",value.Kaqmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(Kcatbloc value){
                    var parameters = new DynamicParameters();
                    parameters.Add("IdUnique",value.Kaqid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(Kcatbloc value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAQID",value.Kaqid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAQBRA",value.Kaqbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KAQCIBLE",value.Kaqcible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAQVOLET",value.Kaqvolet??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAQKAPID",value.Kaqkapid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAQBLOC",value.Kaqbloc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAQKAEID",value.Kaqkaeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAQCAR",value.Kaqcar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAQORDRE",value.Kaqordre, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KAQCRU",value.Kaqcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAQCRD",value.Kaqcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAQCRH",value.Kaqcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAQMAJU",value.Kaqmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAQMAJD",value.Kaqmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAQMAJH",value.Kaqmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("IdUnique",value.Kaqid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KCatBloc_BrancheCible> ListeBrancheCible(string KAQBRA, string KAQCIBLE){
                    return connection.EnsureOpened().Query<KCatBloc_BrancheCible>(select_ListeBrancheCible, new {KAQBRA, KAQCIBLE}).ToList();
            }
            public IEnumerable<Kcatbloc> GetAll(){
                    return connection.EnsureOpened().Query<Kcatbloc>(select_GetAll).ToList();
            }
    }
}
