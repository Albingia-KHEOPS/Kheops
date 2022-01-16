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

    public  partial class  KpEngRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDOID, KDOTYP, KDOIPB, KDOALX, KDOECO
, KDOACT, KDOENGID, KDODATD, KDODATF, KDOCRU
, KDOCRD, KDOMAJU, KDOMAJD, KDONPL, KDOAPP
, KDOENG, KDOENA, KDOOBSV, KDOPCV, KDOLCT
, KDOLCA, KDOCAT, KDOCAA FROM KPENG
WHERE KDOID = :KDOID
";
            const string update=@"UPDATE KPENG SET 
KDOID = :KDOID, KDOTYP = :KDOTYP, KDOIPB = :KDOIPB, KDOALX = :KDOALX, KDOECO = :KDOECO, KDOACT = :KDOACT, KDOENGID = :KDOENGID, KDODATD = :KDODATD, KDODATF = :KDODATF, KDOCRU = :KDOCRU
, KDOCRD = :KDOCRD, KDOMAJU = :KDOMAJU, KDOMAJD = :KDOMAJD, KDONPL = :KDONPL, KDOAPP = :KDOAPP, KDOENG = :KDOENG, KDOENA = :KDOENA, KDOOBSV = :KDOOBSV, KDOPCV = :KDOPCV, KDOLCT = :KDOLCT
, KDOLCA = :KDOLCA, KDOCAT = :KDOCAT, KDOCAA = :KDOCAA
 WHERE 
KDOID = :KDOID";
            const string delete=@"DELETE FROM KPENG WHERE KDOID = :KDOID";
            const string insert=@"INSERT INTO  KPENG (
KDOID, KDOTYP, KDOIPB, KDOALX, KDOECO
, KDOACT, KDOENGID, KDODATD, KDODATF, KDOCRU
, KDOCRD, KDOMAJU, KDOMAJD, KDONPL, KDOAPP
, KDOENG, KDOENA, KDOOBSV, KDOPCV, KDOLCT
, KDOLCA, KDOCAT, KDOCAA
) VALUES (
:KDOID, :KDOTYP, :KDOIPB, :KDOALX, :KDOECO
, :KDOACT, :KDOENGID, :KDODATD, :KDODATF, :KDOCRU
, :KDOCRD, :KDOMAJU, :KDOMAJD, :KDONPL, :KDOAPP
, :KDOENG, :KDOENA, :KDOOBSV, :KDOPCV, :KDOLCT
, :KDOLCA, :KDOCAT, :KDOCAA)";
            const string select_GetByAffaire=@"SELECT
KDOID, KDOTYP, KDOIPB, KDOALX, KDOECO
, KDOACT, KDOENGID, KDODATD, KDODATF, KDOCRU
, KDOCRD, KDOMAJU, KDOMAJD, KDONPL, KDOAPP
, KDOENG, KDOENA, KDOOBSV, KDOPCV, KDOLCT
, KDOLCA, KDOCAT, KDOCAA FROM KPENG
WHERE KDOTYP = :KDOTYP
and KDOIPB = :KDOIPB
and KDOALX = :KDOALX
";
            #endregion

            public KpEngRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpEng Get(Int64 KDOID){
                return connection.Query<KpEng>(select, new {KDOID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KDOID") ;
            }

            public void Insert(KpEng value){
                    if(value.Kdoid == default(Int64)) {
                        value.Kdoid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KDOID",value.Kdoid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDOTYP",value.Kdotyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDOIPB",value.Kdoipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDOALX",value.Kdoalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDOECO",value.Kdoeco??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDOACT",value.Kdoact??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDOENGID",value.Kdoengid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDODATD",value.Kdodatd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDODATF",value.Kdodatf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDOCRU",value.Kdocru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDOCRD",value.Kdocrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDOMAJU",value.Kdomaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDOMAJD",value.Kdomajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDONPL",value.Kdonpl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDOAPP",value.Kdoapp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KDOENG",value.Kdoeng, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDOENA",value.Kdoena, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDOOBSV",value.Kdoobsv, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDOPCV",value.Kdopcv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDOLCT",value.Kdolct, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDOLCA",value.Kdolca, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDOCAT",value.Kdocat, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDOCAA",value.Kdocaa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }

        public void Delete(KpEng value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDOID",value.Kdoid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpEng value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDOID",value.Kdoid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDOTYP",value.Kdotyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDOIPB",value.Kdoipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDOALX",value.Kdoalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDOECO",value.Kdoeco??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDOACT",value.Kdoact??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDOENGID",value.Kdoengid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDODATD",value.Kdodatd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDODATF",value.Kdodatf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDOCRU",value.Kdocru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDOCRD",value.Kdocrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDOMAJU",value.Kdomaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDOMAJD",value.Kdomajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDONPL",value.Kdonpl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDOAPP",value.Kdoapp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KDOENG",value.Kdoeng, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDOENA",value.Kdoena, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDOOBSV",value.Kdoobsv, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDOPCV",value.Kdopcv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDOLCT",value.Kdolct, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDOLCA",value.Kdolca, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDOCAT",value.Kdocat, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDOCAA",value.Kdocaa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KDOID",value.Kdoid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpEng> GetByAffaire(string KDOTYP, string KDOIPB, int KDOALX){
                    return connection.EnsureOpened().Query<KpEng>(select_GetByAffaire, new {KDOTYP, KDOIPB, KDOALX}).ToList();
            }
    }
}
