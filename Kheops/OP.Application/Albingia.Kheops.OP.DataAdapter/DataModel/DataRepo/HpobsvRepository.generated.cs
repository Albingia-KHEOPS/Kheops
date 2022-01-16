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

    public partial class HpobsvRepository : BaseTableRepository {

        private IdentifierGenerator idGenerator;

        #region Query texts        
        //ZALBINKHEO
        const string select = @"SELECT
KAJCHR, KAJTYP, KAJIPB, KAJALX, KAJAVN
, KAJHIN, KAJTYPOBS, KAJOBSV FROM HPOBSV
WHERE KAJCHR = :KAJCHR
and KAJAVN = :KAJAVN
and KAJHIN = :KAJHIN
";
        const string update = @"UPDATE HPOBSV SET 
KAJCHR = :KAJCHR, KAJTYP = :KAJTYP, KAJIPB = :KAJIPB, KAJALX = :KAJALX, KAJAVN = :KAJAVN, KAJHIN = :KAJHIN, KAJTYPOBS = :KAJTYPOBS, KAJOBSV = :KAJOBSV
 WHERE 
KAJCHR = :KAJCHR and KAJAVN = :KAJAVN and KAJHIN = :KAJHIN";
        const string delete = @"DELETE FROM HPOBSV WHERE KAJCHR = :KAJCHR AND KAJAVN = :KAJAVN AND KAJHIN = :KAJHIN";
        const string insert = @"INSERT INTO  HPOBSV (
KAJCHR, KAJTYP, KAJIPB, KAJALX, KAJAVN
, KAJHIN, KAJTYPOBS, KAJOBSV
) VALUES (
:KAJCHR, :KAJTYP, :KAJIPB, :KAJALX, :KAJAVN
, :KAJHIN, :KAJTYPOBS, :KAJOBSV)";
        const string select_GetByAffaire = @"SELECT
KAJCHR, KAJTYP, KAJIPB, KAJALX, KAJAVN
, KAJHIN, KAJTYPOBS, KAJOBSV FROM HPOBSV
WHERE KAJTYP = :KAJTYP
and KAJIPB = :KAJIPB
and KAJALX = :KAJALX
and KAJAVN = :KAJAVN
";
        #endregion

            public HpobsvRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpObsv Get(int KAJCHR, int KAJAVN, int KAJHIN){
                return connection.Query<KpObsv>(select, new {KAJCHR, KAJAVN, KAJHIN}).SingleOrDefault();
            }


            public void Insert(KpObsv value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAJCHR",value.Kajchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAJTYP",value.Kajtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAJIPB",value.Kajipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KAJALX",value.Kajalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAJAVN",value.Kajavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAJHIN",value.Kajhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAJTYPOBS",value.Kajtypobs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAJOBSV",value.Kajobsv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpObsv value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAJCHR",value.Kajchr);
                    parameters.Add("KAJAVN",value.Kajavn);
                    parameters.Add("KAJHIN",value.Kajhin);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpObsv value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAJCHR",value.Kajchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAJTYP",value.Kajtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAJIPB",value.Kajipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KAJALX",value.Kajalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAJAVN",value.Kajavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAJHIN",value.Kajhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAJTYPOBS",value.Kajtypobs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAJOBSV",value.Kajobsv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("KAJCHR",value.Kajchr);
                    parameters.Add("KAJAVN",value.Kajavn);
                    parameters.Add("KAJHIN",value.Kajhin);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpObsv> GetByAffaire(string KAJTYP, string KAJIPB, int KAJALX, int KAJAVN){
                    return connection.EnsureOpened().Query<KpObsv>(select_GetByAffaire, new {KAJTYP, KAJIPB, KAJALX, KAJAVN}).ToList();
            }
    }
}
