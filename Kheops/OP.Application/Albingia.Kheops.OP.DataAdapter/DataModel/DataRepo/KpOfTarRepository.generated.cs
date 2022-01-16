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

    public  partial class  KpOfTarRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KFKPOG, KFKALG, KFKIPB, KFKALX, KFKFOR
, KFKOPT, KFKGARAN, KFKNUMTAR, KFKKDGID, KFKSEL
 FROM KPOFTAR
WHERE KFKPOG = :parKFKPOG
and KFKALG = :parKFKALG
and KFKFOR = :parKFKFOR
and KFKOPT = :parKFKOPT
and KFKGARAN = :parKFKGARAN
and KFKNUMTAR = :parKFKNUMTAR
";
            const string update=@"UPDATE KPOFTAR SET 
KFKPOG = :KFKPOG, KFKALG = :KFKALG, KFKIPB = :KFKIPB, KFKALX = :KFKALX, KFKFOR = :KFKFOR, KFKOPT = :KFKOPT, KFKGARAN = :KFKGARAN, KFKNUMTAR = :KFKNUMTAR, KFKKDGID = :KFKKDGID, KFKSEL = :KFKSEL

 WHERE 
KFKPOG = :parKFKPOG and KFKALG = :parKFKALG and KFKFOR = :parKFKFOR and KFKOPT = :parKFKOPT and KFKGARAN = :parKFKGARAN and KFKNUMTAR = :parKFKNUMTAR";
            const string delete=@"DELETE FROM KPOFTAR WHERE KFKPOG = :parKFKPOG AND KFKALG = :parKFKALG AND KFKFOR = :parKFKFOR AND KFKOPT = :parKFKOPT AND KFKGARAN = :parKFKGARAN AND KFKNUMTAR = :parKFKNUMTAR";
            const string insert=@"INSERT INTO  KPOFTAR (
KFKPOG, KFKALG, KFKIPB, KFKALX, KFKFOR
, KFKOPT, KFKGARAN, KFKNUMTAR, KFKKDGID, KFKSEL

) VALUES (
:KFKPOG, :KFKALG, :KFKIPB, :KFKALX, :KFKFOR
, :KFKOPT, :KFKGARAN, :KFKNUMTAR, :KFKKDGID, :KFKSEL
)";
            const string select_GetByOffre=@"SELECT
KFKPOG, KFKALG, KFKIPB, KFKALX, KFKFOR
, KFKOPT, KFKGARAN, KFKNUMTAR, KFKKDGID, KFKSEL
 FROM KPOFTAR
WHERE KFKIPB = :parKFKIPB
and KFKALX = :parKFKALX
";
            #endregion

            public KpOfTarRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpOfTar Get(string parKFKPOG, int parKFKALG, int parKFKFOR, int parKFKOPT, string parKFKGARAN, int parKFKNUMTAR){
                return connection.Query<KpOfTar>(select, new {parKFKPOG, parKFKALG, parKFKFOR, parKFKOPT, parKFKGARAN, parKFKNUMTAR}).SingleOrDefault();
            }


            public void Insert(KpOfTar value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFKPOG",value.Kfkpog??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFKALG",value.Kfkalg, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFKIPB",value.Kfkipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFKALX",value.Kfkalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFKFOR",value.Kfkfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFKOPT",value.Kfkopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFKGARAN",value.Kfkgaran??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFKNUMTAR",value.Kfknumtar, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFKKDGID",value.Kfkkdgid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFKSEL",value.Kfksel??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpOfTar value){
                    var parameters = new DynamicParameters();
                    parameters.Add("parKFKPOG",value.Kfkpog);
                    parameters.Add("parKFKALG",value.Kfkalg);
                    parameters.Add("parKFKFOR",value.Kfkfor);
                    parameters.Add("parKFKOPT",value.Kfkopt);
                    parameters.Add("parKFKGARAN",value.Kfkgaran);
                    parameters.Add("parKFKNUMTAR",value.Kfknumtar);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpOfTar value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFKPOG",value.Kfkpog??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFKALG",value.Kfkalg, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFKIPB",value.Kfkipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFKALX",value.Kfkalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFKFOR",value.Kfkfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFKOPT",value.Kfkopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFKGARAN",value.Kfkgaran??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFKNUMTAR",value.Kfknumtar, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFKKDGID",value.Kfkkdgid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFKSEL",value.Kfksel??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("parKFKPOG",value.Kfkpog);
                    parameters.Add("parKFKALG",value.Kfkalg);
                    parameters.Add("parKFKFOR",value.Kfkfor);
                    parameters.Add("parKFKOPT",value.Kfkopt);
                    parameters.Add("parKFKGARAN",value.Kfkgaran);
                    parameters.Add("parKFKNUMTAR",value.Kfknumtar);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpOfTar> GetByOffre(string parKFKIPB, int parKFKALX){
                    return connection.EnsureOpened().Query<KpOfTar>(select_GetByOffre, new {parKFKIPB, parKFKALX}).ToList();
            }
    }
}
