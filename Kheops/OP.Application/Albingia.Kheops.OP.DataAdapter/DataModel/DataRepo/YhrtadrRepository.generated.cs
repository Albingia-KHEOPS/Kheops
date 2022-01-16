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

    public  partial class  YhrtadrRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
JFIPB, JFALX, JFAVN, JHHIN, JFRSQ
, JFOBJ, JFAD1, JFAD2, JFDEP, JFCPO
, JFVIL, JFPAY, JFADH FROM YHRTADR
WHERE JFIPB = :JFIPB
and JFALX = :JFALX
and JFAVN = :JFAVN
and JHHIN = :JHHIN
and JFRSQ = :JFRSQ
and JFOBJ = :JFOBJ
";
            const string update=@"UPDATE YHRTADR SET 
JFIPB = :JFIPB, JFALX = :JFALX, JFAVN = :JFAVN, JHHIN = :JHHIN, JFRSQ = :JFRSQ, JFOBJ = :JFOBJ, JFAD1 = :JFAD1, JFAD2 = :JFAD2, JFDEP = :JFDEP, JFCPO = :JFCPO
, JFVIL = :JFVIL, JFPAY = :JFPAY, JFADH = :JFADH
 WHERE 
JFIPB = :JFIPB and JFALX = :JFALX and JFAVN = :JFAVN and JHHIN = :JHHIN and JFRSQ = :JFRSQ and JFOBJ = :JFOBJ";
            const string delete=@"DELETE FROM YHRTADR WHERE JFIPB = :JFIPB AND JFALX = :JFALX AND JFAVN = :JFAVN AND JHHIN = :JHHIN AND JFRSQ = :JFRSQ AND JFOBJ = :JFOBJ";
            const string insert=@"INSERT INTO  YHRTADR (
JFIPB, JFALX, JFAVN, JHHIN, JFRSQ
, JFOBJ, JFAD1, JFAD2, JFDEP, JFCPO
, JFVIL, JFPAY, JFADH
) VALUES (
:JFIPB, :JFALX, :JFAVN, :JHHIN, :JFRSQ
, :JFOBJ, :JFAD1, :JFAD2, :JFDEP, :JFCPO
, :JFVIL, :JFPAY, :JFADH)";
            const string select_GetByAffaire=@"SELECT
JFIPB, JFALX, JFAVN, JHHIN, JFRSQ
, JFOBJ, JFAD1, JFAD2, JFDEP, JFCPO
, JFVIL, JFPAY, JFADH FROM YHRTADR
WHERE JFIPB = :JFIPB
and JFALX = :JFALX
and JFAVN = :JFAVN
";
            #endregion

            public YhrtadrRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YprtAdr Get(string JFIPB, int JFALX, int JFAVN, int JHHIN, int JFRSQ, int JFOBJ){
                return connection.Query<YprtAdr>(select, new {JFIPB, JFALX, JFAVN, JHHIN, JFRSQ, JFOBJ}).SingleOrDefault();
            }


            public void Insert(YprtAdr value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JFIPB",value.Jfipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JFALX",value.Jfalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JFAVN",value.Jfavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JHHIN",value.Jhhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JFRSQ",value.Jfrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JFOBJ",value.Jfobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JFAD1",value.Jfad1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("JFAD2",value.Jfad2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("JFDEP",value.Jfdep??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JFCPO",value.Jfcpo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JFVIL",value.Jfvil??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:26, scale: 0);
                    parameters.Add("JFPAY",value.Jfpay??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JFADH",value.Jfadh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YprtAdr value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JFIPB",value.Jfipb);
                    parameters.Add("JFALX",value.Jfalx);
                    parameters.Add("JFAVN",value.Jfavn);
                    parameters.Add("JHHIN",value.Jhhin);
                    parameters.Add("JFRSQ",value.Jfrsq);
                    parameters.Add("JFOBJ",value.Jfobj);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YprtAdr value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JFIPB",value.Jfipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JFALX",value.Jfalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JFAVN",value.Jfavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JHHIN",value.Jhhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JFRSQ",value.Jfrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JFOBJ",value.Jfobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JFAD1",value.Jfad1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("JFAD2",value.Jfad2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("JFDEP",value.Jfdep??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JFCPO",value.Jfcpo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JFVIL",value.Jfvil??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:26, scale: 0);
                    parameters.Add("JFPAY",value.Jfpay??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JFADH",value.Jfadh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("JFIPB",value.Jfipb);
                    parameters.Add("JFALX",value.Jfalx);
                    parameters.Add("JFAVN",value.Jfavn);
                    parameters.Add("JHHIN",value.Jhhin);
                    parameters.Add("JFRSQ",value.Jfrsq);
                    parameters.Add("JFOBJ",value.Jfobj);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YprtAdr> GetByAffaire(string JFIPB, int JFALX, int JFAVN){
                    return connection.EnsureOpened().Query<YprtAdr>(select_GetByAffaire, new {JFIPB, JFALX, JFAVN}).ToList();
            }
    }
}
