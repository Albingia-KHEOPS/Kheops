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

    public  partial class  KpCtrlARepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KGTTYP, KGTIPB, KGTALX, KGTETAPE, KGTLIB
, KGTCRU, KGTCRD, KGTCRH, KGTMAJU, KGTMAJD
, KGTMAJH FROM KPCTRLA
WHERE KGTTYP = :parKGTTYP
and KGTIPB = :parKGTIPB
and KGTALX = :parKGTALX
";
            const string update=@"UPDATE KPCTRLA SET 
KGTTYP = :KGTTYP, KGTIPB = :KGTIPB, KGTALX = :KGTALX, KGTETAPE = :KGTETAPE, KGTLIB = :KGTLIB, KGTCRU = :KGTCRU, KGTCRD = :KGTCRD, KGTCRH = :KGTCRH, KGTMAJU = :KGTMAJU, KGTMAJD = :KGTMAJD
, KGTMAJH = :KGTMAJH
 WHERE 
KGTTYP = :parKGTTYP and KGTIPB = :parKGTIPB and KGTALX = :parKGTALX";
            const string delete=@"DELETE FROM KPCTRLA WHERE KGTTYP = :parKGTTYP AND KGTIPB = :parKGTIPB AND KGTALX = :parKGTALX";
            const string insert=@"INSERT INTO  KPCTRLA (
KGTTYP, KGTIPB, KGTALX, KGTETAPE, KGTLIB
, KGTCRU, KGTCRD, KGTCRH, KGTMAJU, KGTMAJD
, KGTMAJH
) VALUES (
:KGTTYP, :KGTIPB, :KGTALX, :KGTETAPE, :KGTLIB
, :KGTCRU, :KGTCRD, :KGTCRH, :KGTMAJU, :KGTMAJD
, :KGTMAJH)";
            #endregion

            public KpCtrlARepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpCtrlA Get(string parKGTTYP, string parKGTIPB, int parKGTALX){
                return connection.Query<KpCtrlA>(select, new {parKGTTYP, parKGTIPB, parKGTALX}).SingleOrDefault();
            }


            public void Insert(KpCtrlA value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KGTTYP",value.Kgttyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGTIPB",value.Kgtipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KGTALX",value.Kgtalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGTETAPE",value.Kgtetape??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGTLIB",value.Kgtlib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("KGTCRU",value.Kgtcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGTCRD",value.Kgtcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KGTCRH",value.Kgtcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KGTMAJU",value.Kgtmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGTMAJD",value.Kgtmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KGTMAJH",value.Kgtmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpCtrlA value){
                    var parameters = new DynamicParameters();
                    parameters.Add("parKGTTYP",value.Kgttyp);
                    parameters.Add("parKGTIPB",value.Kgtipb);
                    parameters.Add("parKGTALX",value.Kgtalx);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpCtrlA value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KGTTYP",value.Kgttyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGTIPB",value.Kgtipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KGTALX",value.Kgtalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGTETAPE",value.Kgtetape??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGTLIB",value.Kgtlib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("KGTCRU",value.Kgtcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGTCRD",value.Kgtcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KGTCRH",value.Kgtcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KGTMAJU",value.Kgtmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGTMAJD",value.Kgtmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KGTMAJH",value.Kgtmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("parKGTTYP",value.Kgttyp);
                    parameters.Add("parKGTIPB",value.Kgtipb);
                    parameters.Add("parKGTALX",value.Kgtalx);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
    }
}
