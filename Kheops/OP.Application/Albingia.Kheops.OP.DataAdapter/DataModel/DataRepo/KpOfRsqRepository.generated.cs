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

    public  partial class  KpOfRsqRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KFIPOG, KFIALG, KFIIPB, KFIALX, KFICHR
, KFITYE, KFIRSQ, KFIOBJ, KFIINV, KFISEL
 FROM KPOFRSQ
WHERE KFIPOG = :parKFIPOG
and KFIALG = :parKFIALG
and KFICHR = :parKFICHR
";
            const string update=@"UPDATE KPOFRSQ SET 
KFIPOG = :KFIPOG, KFIALG = :KFIALG, KFIIPB = :KFIIPB, KFIALX = :KFIALX, KFICHR = :KFICHR, KFITYE = :KFITYE, KFIRSQ = :KFIRSQ, KFIOBJ = :KFIOBJ, KFIINV = :KFIINV, KFISEL = :KFISEL

 WHERE 
KFIPOG = :parKFIPOG and KFIALG = :parKFIALG and KFICHR = :parKFICHR";
            const string delete=@"DELETE FROM KPOFRSQ WHERE KFIPOG = :parKFIPOG AND KFIALG = :parKFIALG AND KFICHR = :parKFICHR";
            const string insert=@"INSERT INTO  KPOFRSQ (
KFIPOG, KFIALG, KFIIPB, KFIALX, KFICHR
, KFITYE, KFIRSQ, KFIOBJ, KFIINV, KFISEL

) VALUES (
:KFIPOG, :KFIALG, :KFIIPB, :KFIALX, :KFICHR
, :KFITYE, :KFIRSQ, :KFIOBJ, :KFIINV, :KFISEL
)";
            const string select_GetByContrat=@"SELECT
KFIPOG, KFIALG, KFIIPB, KFIALX, KFICHR
, KFITYE, KFIRSQ, KFIOBJ, KFIINV, KFISEL
 FROM KPOFRSQ
WHERE KFIPOG = :parKFIPOG
and KFIALG = :parKFIALG
";
            const string select_GetByOffre=@"SELECT
KFIPOG, KFIALG, KFIIPB, KFIALX, KFICHR
, KFITYE, KFIRSQ, KFIOBJ, KFIINV, KFISEL
 FROM KPOFRSQ
WHERE KFIIPB = :parKFIIPB
and KFIALX = :parKFIALX
";
            #endregion

            public KpOfRsqRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpOfRsq Get(string parKFIPOG, int parKFIALG, int parKFICHR){
                return connection.Query<KpOfRsq>(select, new {parKFIPOG, parKFIALG, parKFICHR}).SingleOrDefault();
            }


            public void Insert(KpOfRsq value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFIPOG",value.Kfipog??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFIALG",value.Kfialg, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFIIPB",value.Kfiipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFIALX",value.Kfialx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFICHR",value.Kfichr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KFITYE",value.Kfitye??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFIRSQ",value.Kfirsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFIOBJ",value.Kfiobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFIINV",value.Kfiinv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFISEL",value.Kfisel??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpOfRsq value){
                    var parameters = new DynamicParameters();
                    parameters.Add("parKFIPOG",value.Kfipog);
                    parameters.Add("parKFIALG",value.Kfialg);
                    parameters.Add("parKFICHR",value.Kfichr);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpOfRsq value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFIPOG",value.Kfipog??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFIALG",value.Kfialg, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFIIPB",value.Kfiipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFIALX",value.Kfialx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFICHR",value.Kfichr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KFITYE",value.Kfitye??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFIRSQ",value.Kfirsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFIOBJ",value.Kfiobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFIINV",value.Kfiinv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFISEL",value.Kfisel??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("parKFIPOG",value.Kfipog);
                    parameters.Add("parKFIALG",value.Kfialg);
                    parameters.Add("parKFICHR",value.Kfichr);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpOfRsq> GetByContrat(string parKFIPOG, int parKFIALG){
                    return connection.EnsureOpened().Query<KpOfRsq>(select_GetByContrat, new {parKFIPOG, parKFIALG}).ToList();
            }
            public IEnumerable<KpOfRsq> GetByOffre(string parKFIIPB, int parKFIALX){
                    return connection.EnsureOpened().Query<KpOfRsq>(select_GetByOffre, new {parKFIIPB, parKFIALX}).ToList();
            }
    }
}
