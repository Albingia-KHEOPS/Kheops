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

    public  partial class  KpForRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDAID, KDATYP, KDAIPB, KDAALX, KDAFOR
, KDACCH, KDAALPHA, KDABRA, KDACIBLE, KDAKAIID
, KDADESC, KDACRU, KDACRD, KDAMAJU, KDAMAJD
, KDAFGEN FROM KPFOR
WHERE KDAID = :formuleId
FETCH FIRST 200 ROWS ONLY
";
            const string update=@"UPDATE KPFOR SET 
KDAID = :KDAID, KDATYP = :KDATYP, KDAIPB = :KDAIPB, KDAALX = :KDAALX, KDAFOR = :KDAFOR, KDACCH = :KDACCH, KDAALPHA = :KDAALPHA, KDABRA = :KDABRA, KDACIBLE = :KDACIBLE, KDAKAIID = :KDAKAIID
, KDADESC = :KDADESC, KDACRU = :KDACRU, KDACRD = :KDACRD, KDAMAJU = :KDAMAJU, KDAMAJD = :KDAMAJD, KDAFGEN = :KDAFGEN
 WHERE 
KDAID = :formuleId";
            const string delete=@"DELETE FROM KPFOR WHERE KDAID = :formuleId";
            const string insert=@"INSERT INTO  KPFOR (
KDAID, KDATYP, KDAIPB, KDAALX, KDAFOR
, KDACCH, KDAALPHA, KDABRA, KDACIBLE, KDAKAIID
, KDADESC, KDACRU, KDACRD, KDAMAJU, KDAMAJD
, KDAFGEN
) VALUES (
:KDAID, :KDATYP, :KDAIPB, :KDAALX, :KDAFOR
, :KDACCH, :KDAALPHA, :KDABRA, :KDACIBLE, :KDAKAIID
, :KDADESC, :KDACRU, :KDACRD, :KDAMAJU, :KDAMAJD
, :KDAFGEN)";
            const string select_Liste=@"SELECT
KDAID, KDATYP, KDAIPB, KDAALX, KDAFOR
, KDACCH, KDAALPHA, KDABRA, KDACIBLE, KDAKAIID
, KDADESC, KDACRU, KDACRD, KDAMAJU, KDAMAJD
, KDAFGEN FROM KPFOR
WHERE KDATYP = :typeAffaire
and KDAIPB = :numeroAffaire
and KDAALX = :version
FETCH FIRST 200 ROWS ONLY
";
            #endregion

            public KpForRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpFor Get(Int64 formuleId){
                return connection.Query<KpFor>(select, new {formuleId}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KDAID") ;
            }

            public void Insert(KpFor value){
                    if(value.Kdaid == default(Int64)) {
                        value.Kdaid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KDAID",value.Kdaid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDATYP",value.Kdatyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDAIPB",value.Kdaipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDAALX",value.Kdaalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDAFOR",value.Kdafor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDACCH",value.Kdacch, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDAALPHA",value.Kdaalpha??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDABRA",value.Kdabra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDACIBLE",value.Kdacible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDAKAIID",value.Kdakaiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDADESC",value.Kdadesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KDACRU",value.Kdacru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDACRD",value.Kdacrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDAMAJU",value.Kdamaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDAMAJD",value.Kdamajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDAFGEN",value.Kdafgen??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpFor value){
                    var parameters = new DynamicParameters();
                    parameters.Add("formuleId",value.Kdaid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpFor value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDAID",value.Kdaid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDATYP",value.Kdatyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDAIPB",value.Kdaipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDAALX",value.Kdaalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDAFOR",value.Kdafor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDACCH",value.Kdacch, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDAALPHA",value.Kdaalpha??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDABRA",value.Kdabra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDACIBLE",value.Kdacible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDAKAIID",value.Kdakaiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDADESC",value.Kdadesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KDACRU",value.Kdacru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDACRD",value.Kdacrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDAMAJU",value.Kdamaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDAMAJD",value.Kdamajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KDAFGEN",value.Kdafgen??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("formuleId",value.Kdaid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpFor> Liste(string typeAffaire, string numeroAffaire, int version){
                    return connection.EnsureOpened().Query<KpFor>(select_Liste, new {typeAffaire, numeroAffaire, version}).ToList();
            }
    }
}
