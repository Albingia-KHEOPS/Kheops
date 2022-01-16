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

    public  partial class  KpEngGarRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDSID, KDSTYP, KDSIPB, KDSALX, KDSRSQ
, KDSFAM, KDSVEN, KDSKDRID, KDSGARAN, KDSENGOK
, KDSLCI, KDSSMP, KDSSMPF, KDSSMPU, KDSSMPR
, KDSCRU, KDSCRD, KDSMAJU, KDSMAJD, KDSKDOID
, KDSCAT FROM KPENGGAR
WHERE KDSID = :KDSID
";
            const string update=@"UPDATE KPENGGAR SET 
KDSID = :KDSID, KDSTYP = :KDSTYP, KDSIPB = :KDSIPB, KDSALX = :KDSALX, KDSRSQ = :KDSRSQ, KDSFAM = :KDSFAM, KDSVEN = :KDSVEN, KDSKDRID = :KDSKDRID, KDSGARAN = :KDSGARAN, KDSENGOK = :KDSENGOK
, KDSLCI = :KDSLCI, KDSSMP = :KDSSMP, KDSSMPF = :KDSSMPF, KDSSMPU = :KDSSMPU, KDSSMPR = :KDSSMPR, KDSCRU = :KDSCRU, KDSCRD = :KDSCRD, KDSMAJU = :KDSMAJU, KDSMAJD = :KDSMAJD, KDSKDOID = :KDSKDOID
, KDSCAT = :KDSCAT
 WHERE 
KDSID = :KDSID";
            const string delete=@"DELETE FROM KPENGGAR WHERE KDSID = :KDSID";
            const string insert=@"INSERT INTO  KPENGGAR (
KDSID, KDSTYP, KDSIPB, KDSALX, KDSRSQ
, KDSFAM, KDSVEN, KDSKDRID, KDSGARAN, KDSENGOK
, KDSLCI, KDSSMP, KDSSMPF, KDSSMPU, KDSSMPR
, KDSCRU, KDSCRD, KDSMAJU, KDSMAJD, KDSKDOID
, KDSCAT
) VALUES (
:KDSID, :KDSTYP, :KDSIPB, :KDSALX, :KDSRSQ
, :KDSFAM, :KDSVEN, :KDSKDRID, :KDSGARAN, :KDSENGOK
, :KDSLCI, :KDSSMP, :KDSSMPF, :KDSSMPU, :KDSSMPR
, :KDSCRU, :KDSCRD, :KDSMAJU, :KDSMAJD, :KDSKDOID
, :KDSCAT)";
            const string select_GetByAffaire=@"SELECT
KDSID, KDSTYP, KDSIPB, KDSALX, KDSRSQ
, KDSFAM, KDSVEN, KDSKDRID, KDSGARAN, KDSENGOK
, KDSLCI, KDSSMP, KDSSMPF, KDSSMPU, KDSSMPR
, KDSCRU, KDSCRD, KDSMAJU, KDSMAJD, KDSKDOID
, KDSCAT FROM KPENGGAR
WHERE KDSTYP = :KDSTYP
and KDSIPB = :KDSIPB
and KDSALX = :KDSALX
";
            #endregion

            public KpEngGarRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpEngGar Get(Int64 KDSID){
                return connection.Query<KpEngGar>(select, new {KDSID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KDSID") ;
            }

            public void Insert(KpEngGar value){
                    if(value.Kdsid == default(Int64)) {
                        value.Kdsid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KDSID",value.Kdsid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDSTYP",value.Kdstyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDSIPB",value.Kdsipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDSALX",value.Kdsalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDSRSQ",value.Kdsrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDSFAM",value.Kdsfam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDSVEN",value.Kdsven, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDSKDRID",value.Kdskdrid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDSGARAN",value.Kdsgaran??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDSENGOK",value.Kdsengok??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDSLCI",value.Kdslci, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDSSMP",value.Kdssmp, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDSSMPF",value.Kdssmpf, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDSSMPU",value.Kdssmpu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDSSMPR",value.Kdssmpr, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDSCRU",value.Kdscru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDSCRD",value.Kdscrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDSMAJU",value.Kdsmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDSMAJD",value.Kdsmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDSKDOID",value.Kdskdoid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDSCAT",value.Kdscat, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpEngGar value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDSID",value.Kdsid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpEngGar value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDSID",value.Kdsid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDSTYP",value.Kdstyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDSIPB",value.Kdsipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDSALX",value.Kdsalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDSRSQ",value.Kdsrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDSFAM",value.Kdsfam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDSVEN",value.Kdsven, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDSKDRID",value.Kdskdrid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDSGARAN",value.Kdsgaran??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDSENGOK",value.Kdsengok??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDSLCI",value.Kdslci, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDSSMP",value.Kdssmp, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDSSMPF",value.Kdssmpf, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDSSMPU",value.Kdssmpu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDSSMPR",value.Kdssmpr, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDSCRU",value.Kdscru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDSCRD",value.Kdscrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDSMAJU",value.Kdsmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDSMAJD",value.Kdsmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDSKDOID",value.Kdskdoid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDSCAT",value.Kdscat, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDSID",value.Kdsid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpEngGar> GetByAffaire(string KDSTYP, string KDSIPB, int KDSALX){
                    return connection.EnsureOpened().Query<KpEngGar>(select_GetByAffaire, new {KDSTYP, KDSIPB, KDSALX}).ToList();
            }
    }
}
