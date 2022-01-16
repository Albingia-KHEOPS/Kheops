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

    public  partial class  HpinvenRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KBEID, KBETYP, KBEIPB, KBEALX, KBEAVN
, KBEHIN, KBECHR, KBEDESC, KBEKAGID, KBEKADID
, KBEREPVAL, KBEVAL, KBEVAA, KBEVAW, KBEVAT
, KBEVAU, KBEVAH, KBEIVO, KBEIVA, KBEIVW
 FROM HPINVEN
WHERE KBEID = :parKBEID
and KBEAVN = :parKBEAVN
";
            const string update=@"UPDATE HPINVEN SET 
KBEID = :KBEID, KBETYP = :KBETYP, KBEIPB = :KBEIPB, KBEALX = :KBEALX, KBEAVN = :KBEAVN, KBEHIN = :KBEHIN, KBECHR = :KBECHR, KBEDESC = :KBEDESC, KBEKAGID = :KBEKAGID, KBEKADID = :KBEKADID
, KBEREPVAL = :KBEREPVAL, KBEVAL = :KBEVAL, KBEVAA = :KBEVAA, KBEVAW = :KBEVAW, KBEVAT = :KBEVAT, KBEVAU = :KBEVAU, KBEVAH = :KBEVAH, KBEIVO = :KBEIVO, KBEIVA = :KBEIVA, KBEIVW = :KBEIVW

 WHERE 
KBEID = :parKBEID and KBEAVN = :parKBEAVN";
            const string delete=@"DELETE FROM HPINVEN WHERE KBEID = :parKBEID AND KBEAVN = :parKBEAVN";
            const string insert=@"INSERT INTO  HPINVEN (
KBEID, KBETYP, KBEIPB, KBEALX, KBEAVN
, KBEHIN, KBECHR, KBEDESC, KBEKAGID, KBEKADID
, KBEREPVAL, KBEVAL, KBEVAA, KBEVAW, KBEVAT
, KBEVAU, KBEVAH, KBEIVO, KBEIVA, KBEIVW

) VALUES (
:KBEID, :KBETYP, :KBEIPB, :KBEALX, :KBEAVN
, :KBEHIN, :KBECHR, :KBEDESC, :KBEKAGID, :KBEKADID
, :KBEREPVAL, :KBEVAL, :KBEVAA, :KBEVAW, :KBEVAT
, :KBEVAU, :KBEVAH, :KBEIVO, :KBEIVA, :KBEIVW
)";
            const string select_GetByAffaire=@"SELECT
KBEID, KBETYP, KBEIPB, KBEALX, KBEAVN
, KBEHIN, KBECHR, KBEDESC, KBEKAGID, KBEKADID
, KBEREPVAL, KBEVAL, KBEVAA, KBEVAW, KBEVAT
, KBEVAU, KBEVAH, KBEIVO, KBEIVA, KBEIVW
 FROM HPINVEN
WHERE KBETYP = :type
and KBEIPB = :code
and KBEALX = :numeroAliment
and KBEAVN = :numeroAvenant
";
            const string select_GetByGarantie=@"SELECT
KBEID, KBETYP, KBEIPB, KBEALX, KBEAVN
, KBEHIN, KBECHR, KBEDESC, KBEKAGID, KBEKADID
, KBEREPVAL, KBEVAL, KBEVAA, KBEVAW, KBEVAT
, KBEVAU, KBEVAH, KBEIVO, KBEIVA, KBEIVW
 FROM HPINVEN
inner join KPGARAN on KBEID=KDEINVEN and KDEAVN=KBEAVN
WHERE KDEID = :idGaran
and KDEAVN = :numeroAvenant
";
            #endregion

            public HpinvenRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpInven Get(Int64 parKBEID, int parKBEAVN){
                return connection.Query<KpInven>(select, new {parKBEID, parKBEAVN}).SingleOrDefault();
            }


            public void Insert(KpInven value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KBEID",value.Kbeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBETYP",value.Kbetyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBEIPB",value.Kbeipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KBEALX",value.Kbealx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KBEAVN",value.Kbeavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KBEHIN",value.Kbehin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KBECHR",value.Kbechr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KBEDESC",value.Kbedesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KBEKAGID",value.Kbekagid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBEKADID",value.Kbekadid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBEREPVAL",value.Kberepval??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBEVAL",value.Kbeval, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KBEVAA",value.Kbevaa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KBEVAW",value.Kbevaw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KBEVAT",value.Kbevat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBEVAU",value.Kbevau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBEVAH",value.Kbevah??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBEIVO",value.Kbeivo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KBEIVA",value.Kbeiva, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KBEIVW",value.Kbeivw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpInven value){
                    var parameters = new DynamicParameters();
                    parameters.Add("parKBEID",value.Kbeid);
                    parameters.Add("parKBEAVN",value.Kbeavn);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpInven value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KBEID",value.Kbeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBETYP",value.Kbetyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBEIPB",value.Kbeipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KBEALX",value.Kbealx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KBEAVN",value.Kbeavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KBEHIN",value.Kbehin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KBECHR",value.Kbechr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KBEDESC",value.Kbedesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KBEKAGID",value.Kbekagid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBEKADID",value.Kbekadid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBEREPVAL",value.Kberepval??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBEVAL",value.Kbeval, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KBEVAA",value.Kbevaa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KBEVAW",value.Kbevaw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KBEVAT",value.Kbevat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBEVAU",value.Kbevau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBEVAH",value.Kbevah??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBEIVO",value.Kbeivo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KBEIVA",value.Kbeiva, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KBEIVW",value.Kbeivw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("parKBEID",value.Kbeid);
                    parameters.Add("parKBEAVN",value.Kbeavn);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpInven> GetByAffaire(string type, string code, int numeroAliment, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpInven>(select_GetByAffaire, new {type, code, numeroAliment, numeroAvenant}).ToList();
            }
            public IEnumerable<KpInven> GetByGarantie(Int64 idGaran, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpInven>(select_GetByGarantie, new {idGaran, numeroAvenant}).ToList();
            }
    }
}
