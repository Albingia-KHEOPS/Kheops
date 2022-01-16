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

    public  partial class  KpAvTrcRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KHOID, KHOTYP, KHOIPB, KHOALX, KHOPERI
, KHORSQ, KHOOBJ, KHOFOR, KHOOPT, KHOETAPE
, KHOCHAM, KHOACT, KHOANV, KHONVV, KHOAVO
, KHOOEF, KHOCRU, KHOCRD, KHOCRH FROM KPAVTRC
WHERE KHOID = :parKHOID
";
            const string update=@"UPDATE KPAVTRC SET 
KHOID = :KHOID, KHOTYP = :KHOTYP, KHOIPB = :KHOIPB, KHOALX = :KHOALX, KHOPERI = :KHOPERI, KHORSQ = :KHORSQ, KHOOBJ = :KHOOBJ, KHOFOR = :KHOFOR, KHOOPT = :KHOOPT, KHOETAPE = :KHOETAPE
, KHOCHAM = :KHOCHAM, KHOACT = :KHOACT, KHOANV = :KHOANV, KHONVV = :KHONVV, KHOAVO = :KHOAVO, KHOOEF = :KHOOEF, KHOCRU = :KHOCRU, KHOCRD = :KHOCRD, KHOCRH = :KHOCRH
 WHERE 
KHOID = :parKHOID";
            const string delete=@"DELETE FROM KPAVTRC WHERE KHOID = :parKHOID";
            const string insert=@"INSERT INTO  KPAVTRC (
KHOID, KHOTYP, KHOIPB, KHOALX, KHOPERI
, KHORSQ, KHOOBJ, KHOFOR, KHOOPT, KHOETAPE
, KHOCHAM, KHOACT, KHOANV, KHONVV, KHOAVO
, KHOOEF, KHOCRU, KHOCRD, KHOCRH
) VALUES (
:KHOID, :KHOTYP, :KHOIPB, :KHOALX, :KHOPERI
, :KHORSQ, :KHOOBJ, :KHOFOR, :KHOOPT, :KHOETAPE
, :KHOCHAM, :KHOACT, :KHOANV, :KHONVV, :KHOAVO
, :KHOOEF, :KHOCRU, :KHOCRD, :KHOCRH)";
            const string select_GetByAffaire=@"SELECT
KHOID, KHOTYP, KHOIPB, KHOALX, KHOPERI
, KHORSQ, KHOOBJ, KHOFOR, KHOOPT, KHOETAPE
, KHOCHAM, KHOACT, KHOANV, KHONVV, KHOAVO
, KHOOEF, KHOCRU, KHOCRD, KHOCRH FROM KPAVTRC
WHERE KHOTYP = :parKHOTYP
and KHOIPB = :parKHOIPB
and KHOALX = :parKHOALX
";
            #endregion

            public KpAvTrcRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpAvTrc Get(Int64 parKHOID){
                return connection.Query<KpAvTrc>(select, new {parKHOID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KHOID") ;
            }

            public void Insert(KpAvTrc value){
                    if(value.Khoid == default(Int64)) {
                        value.Khoid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KHOID",value.Khoid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHOTYP",value.Khotyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHOIPB",value.Khoipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KHOALX",value.Khoalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KHOPERI",value.Khoperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHORSQ",value.Khorsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHOOBJ",value.Khoobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHOFOR",value.Khofor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHOOPT",value.Khoopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHOETAPE",value.Khoetape??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHOCHAM",value.Khocham??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("KHOACT",value.Khoact??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHOANV",value.Khoanv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:100, scale: 0);
                    parameters.Add("KHONVV",value.Khonvv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:100, scale: 0);
                    parameters.Add("KHOAVO",value.Khoavo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHOOEF",value.Khooef??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("KHOCRU",value.Khocru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHOCRD",value.Khocrd, dbType:DbType.Int64, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHOCRH",value.Khocrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpAvTrc value){
                    var parameters = new DynamicParameters();
                    parameters.Add("parKHOID",value.Khoid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpAvTrc value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KHOID",value.Khoid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHOTYP",value.Khotyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHOIPB",value.Khoipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KHOALX",value.Khoalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KHOPERI",value.Khoperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHORSQ",value.Khorsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHOOBJ",value.Khoobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHOFOR",value.Khofor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHOOPT",value.Khoopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHOETAPE",value.Khoetape??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHOCHAM",value.Khocham??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("KHOACT",value.Khoact??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHOANV",value.Khoanv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:100, scale: 0);
                    parameters.Add("KHONVV",value.Khonvv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:100, scale: 0);
                    parameters.Add("KHOAVO",value.Khoavo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHOOEF",value.Khooef??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("KHOCRU",value.Khocru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHOCRD",value.Khocrd, dbType:DbType.Int64, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHOCRH",value.Khocrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("parKHOID",value.Khoid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpAvTrc> GetByAffaire(string parKHOTYP, string parKHOIPB, int parKHOALX){
                    return connection.EnsureOpened().Query<KpAvTrc>(select_GetByAffaire, new {parKHOTYP, parKHOIPB, parKHOALX}).ToList();
            }
    }
}
