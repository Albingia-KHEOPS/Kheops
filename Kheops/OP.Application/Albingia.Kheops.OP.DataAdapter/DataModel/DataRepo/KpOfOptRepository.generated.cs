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

    public  partial class  KpOfOptRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KFJPOG, KFJALG, KFJIPB, KFJALX, KFJCHR
, KFJTENG, KFJFOR, KFJOPT, KFJKDAID, KFJKDBID
, KFJKAKID, KFJSEL FROM KPOFOPT
WHERE KFJPOG = :parKFJPOG
and KFJALG = :parKFJALG
and KFJCHR = :parKFJCHR
";
            const string update=@"UPDATE KPOFOPT SET 
KFJPOG = :KFJPOG, KFJALG = :KFJALG, KFJIPB = :KFJIPB, KFJALX = :KFJALX, KFJCHR = :KFJCHR, KFJTENG = :KFJTENG, KFJFOR = :KFJFOR, KFJOPT = :KFJOPT, KFJKDAID = :KFJKDAID, KFJKDBID = :KFJKDBID
, KFJKAKID = :KFJKAKID, KFJSEL = :KFJSEL
 WHERE 
KFJPOG = :parKFJPOG and KFJALG = :parKFJALG and KFJCHR = :parKFJCHR";
            const string delete=@"DELETE FROM KPOFOPT WHERE KFJPOG = :parKFJPOG AND KFJALG = :parKFJALG AND KFJCHR = :parKFJCHR";
            const string insert=@"INSERT INTO  KPOFOPT (
KFJPOG, KFJALG, KFJIPB, KFJALX, KFJCHR
, KFJTENG, KFJFOR, KFJOPT, KFJKDAID, KFJKDBID
, KFJKAKID, KFJSEL
) VALUES (
:KFJPOG, :KFJALG, :KFJIPB, :KFJALX, :KFJCHR
, :KFJTENG, :KFJFOR, :KFJOPT, :KFJKDAID, :KFJKDBID
, :KFJKAKID, :KFJSEL)";
            const string select_GetByOffre=@"SELECT
KFJPOG, KFJALG, KFJIPB, KFJALX, KFJCHR
, KFJTENG, KFJFOR, KFJOPT, KFJKDAID, KFJKDBID
, KFJKAKID, KFJSEL FROM KPOFOPT
WHERE KFJIPB = :parKFJIPB
and KFJALX = :parKFJALX
";
            const string select_GetByContrat=@"SELECT
KFJPOG, KFJALG, KFJIPB, KFJALX, KFJCHR
, KFJTENG, KFJFOR, KFJOPT, KFJKDAID, KFJKDBID
, KFJKAKID, KFJSEL FROM KPOFOPT
WHERE KFJPOG = :parKFJPOG
and KFJALG = :parKFJALG
";
            #endregion

            public KpOfOptRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpOfOpt Get(string parKFJPOG, int parKFJALG, int parKFJCHR){
                return connection.Query<KpOfOpt>(select, new {parKFJPOG, parKFJALG, parKFJCHR}).SingleOrDefault();
            }


            public void Insert(KpOfOpt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFJPOG",value.Kfjpog??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFJALG",value.Kfjalg, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFJIPB",value.Kfjipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFJALX",value.Kfjalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFJCHR",value.Kfjchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KFJTENG",value.Kfjteng??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFJFOR",value.Kfjfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFJOPT",value.Kfjopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFJKDAID",value.Kfjkdaid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFJKDBID",value.Kfjkdbid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFJKAKID",value.Kfjkakid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFJSEL",value.Kfjsel??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpOfOpt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("parKFJPOG",value.Kfjpog);
                    parameters.Add("parKFJALG",value.Kfjalg);
                    parameters.Add("parKFJCHR",value.Kfjchr);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpOfOpt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFJPOG",value.Kfjpog??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFJALG",value.Kfjalg, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFJIPB",value.Kfjipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFJALX",value.Kfjalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFJCHR",value.Kfjchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KFJTENG",value.Kfjteng??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFJFOR",value.Kfjfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFJOPT",value.Kfjopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFJKDAID",value.Kfjkdaid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFJKDBID",value.Kfjkdbid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFJKAKID",value.Kfjkakid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFJSEL",value.Kfjsel??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("parKFJPOG",value.Kfjpog);
                    parameters.Add("parKFJALG",value.Kfjalg);
                    parameters.Add("parKFJCHR",value.Kfjchr);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpOfOpt> GetByOffre(string parKFJIPB, int parKFJALX){
                    return connection.EnsureOpened().Query<KpOfOpt>(select_GetByOffre, new {parKFJIPB, parKFJALX}).ToList();
            }
            public IEnumerable<KpOfOpt> GetByContrat(string parKFJPOG, int parKFJALG){
                    return connection.EnsureOpened().Query<KpOfOpt>(select_GetByContrat, new {parKFJPOG, parKFJALG}).ToList();
            }
    }
}
