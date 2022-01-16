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

    public  partial class  YpoEcheRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
PITYP, PIIPB, PIALX, PIEHA, PIEHM
, PIEHJ, PIEHE, PIPCR, PIPCC, PIPMR
, PIPMC, PIAFR, PIIPK, PIATT FROM YPOECHE
WHERE PITYP = :PITYP
and PIIPB = :PIIPB
and PIALX = :PIALX
and PIEHE = :PIEHE
and PIEHA = :PIEHA
and PIEHM = :PIEHM
and PIEHJ = :PIEHJ
";
            const string update=@"UPDATE YPOECHE SET 
PITYP = :PITYP, PIIPB = :PIIPB, PIALX = :PIALX, PIEHA = :PIEHA, PIEHM = :PIEHM, PIEHJ = :PIEHJ, PIEHE = :PIEHE, PIPCR = :PIPCR, PIPCC = :PIPCC, PIPMR = :PIPMR
, PIPMC = :PIPMC, PIAFR = :PIAFR, PIIPK = :PIIPK, PIATT = :PIATT
 WHERE 
PITYP = :PITYP and PIIPB = :PIIPB and PIALX = :PIALX and PIEHE = :PIEHE and PIEHA = :PIEHA and PIEHM = :PIEHM and PIEHJ = :PIEHJ";
            const string delete=@"DELETE FROM YPOECHE WHERE PITYP = :PITYP AND PIIPB = :PIIPB AND PIALX = :PIALX AND PIEHE = :PIEHE AND PIEHA = :PIEHA AND PIEHM = :PIEHM AND PIEHJ = :PIEHJ";
            const string insert=@"INSERT INTO  YPOECHE (
PITYP, PIIPB, PIALX, PIEHA, PIEHM
, PIEHJ, PIEHE, PIPCR, PIPCC, PIPMR
, PIPMC, PIAFR, PIIPK, PIATT
) VALUES (
:PITYP, :PIIPB, :PIALX, :PIEHA, :PIEHM
, :PIEHJ, :PIEHE, :PIPCR, :PIPCC, :PIPMR
, :PIPMC, :PIAFR, :PIIPK, :PIATT)";
            const string select_GetByAffaire=@"SELECT
PITYP, PIIPB, PIALX, PIEHA, PIEHM
, PIEHJ, PIEHE, PIPCR, PIPCC, PIPMR
, PIPMC, PIAFR, PIIPK, PIATT FROM YPOECHE
WHERE PITYP = :PITYP
and PIIPB = :PIIPB
and PIALX = :PIALX
";
            #endregion

            public YpoEcheRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YpoEche Get(string PITYP, string PIIPB, int PIALX, string PIEHE, int PIEHA, int PIEHM, int PIEHJ){
                return connection.Query<YpoEche>(select, new {PITYP, PIIPB, PIALX, PIEHE, PIEHA, PIEHM, PIEHJ}).SingleOrDefault();
            }


            public void Insert(YpoEche value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PITYP",value.Pityp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PIIPB",value.Piipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PIALX",value.Pialx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PIEHA",value.Pieha, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PIEHM",value.Piehm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PIEHJ",value.Piehj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PIEHE",value.Piehe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PIPCR",value.Pipcr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PIPCC",value.Pipcc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:9, scale: 6);
                    parameters.Add("PIPMR",value.Pipmr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PIPMC",value.Pipmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PIAFR",value.Piafr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PIIPK",value.Piipk, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PIATT",value.Piatt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YpoEche value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PITYP",value.Pityp);
                    parameters.Add("PIIPB",value.Piipb);
                    parameters.Add("PIALX",value.Pialx);
                    parameters.Add("PIEHE",value.Piehe);
                    parameters.Add("PIEHA",value.Pieha);
                    parameters.Add("PIEHM",value.Piehm);
                    parameters.Add("PIEHJ",value.Piehj);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YpoEche value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PITYP",value.Pityp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PIIPB",value.Piipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PIALX",value.Pialx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PIEHA",value.Pieha, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PIEHM",value.Piehm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PIEHJ",value.Piehj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PIEHE",value.Piehe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PIPCR",value.Pipcr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PIPCC",value.Pipcc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:9, scale: 6);
                    parameters.Add("PIPMR",value.Pipmr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PIPMC",value.Pipmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PIAFR",value.Piafr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PIIPK",value.Piipk, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PIATT",value.Piatt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PITYP",value.Pityp);
                    parameters.Add("PIIPB",value.Piipb);
                    parameters.Add("PIALX",value.Pialx);
                    parameters.Add("PIEHE",value.Piehe);
                    parameters.Add("PIEHA",value.Pieha);
                    parameters.Add("PIEHM",value.Piehm);
                    parameters.Add("PIEHJ",value.Piehj);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YpoEche> GetByAffaire(string PITYP, string PIIPB, int PIALX){
                    return connection.EnsureOpened().Query<YpoEche>(select_GetByAffaire, new {PITYP, PIIPB, PIALX}).ToList();
            }
    }
}
