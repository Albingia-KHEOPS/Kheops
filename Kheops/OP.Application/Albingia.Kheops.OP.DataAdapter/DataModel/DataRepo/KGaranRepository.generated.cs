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

    public  partial class  KGaranRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
GAGAR, GADES, GADEA, GATAX, GACNX
, GACOM, GACAR, GADFG, GAIFC, GAFAM
, GARGE, GATRG, GAINV, GATYI, GARUT
, GASTA, GAATG FROM KGARAN
WHERE GAGAR = :codeGarantie
";
            const string update=@"UPDATE KGARAN SET 
GAGAR = :GAGAR, GADES = :GADES, GADEA = :GADEA, GATAX = :GATAX, GACNX = :GACNX, GACOM = :GACOM, GACAR = :GACAR, GADFG = :GADFG, GAIFC = :GAIFC, GAFAM = :GAFAM
, GARGE = :GARGE, GATRG = :GATRG, GAINV = :GAINV, GATYI = :GATYI, GARUT = :GARUT, GASTA = :GASTA, GAATG = :GAATG
 WHERE 
GAGAR = :codeGarantie";
            const string delete=@"DELETE FROM KGARAN WHERE GAGAR = :codeGarantie";
            const string insert=@"INSERT INTO  KGARAN (
GAGAR, GADES, GADEA, GATAX, GACNX
, GACOM, GACAR, GADFG, GAIFC, GAFAM
, GARGE, GATRG, GAINV, GATYI, GARUT
, GASTA, GAATG
) VALUES (
:GAGAR, :GADES, :GADEA, :GATAX, :GACNX
, :GACOM, :GACAR, :GADFG, :GAIFC, :GAFAM
, :GARGE, :GATRG, :GAINV, :GATYI, :GARUT
, :GASTA, :GAATG)";
            const string select_GetAll=@"SELECT
GAGAR, GADES, GADEA, GATAX, GACNX
, GACOM, GACAR, GADFG, GAIFC, GAFAM
, GARGE, GATRG, GAINV, GATYI, GARUT
, GASTA, GAATG FROM KGARAN
";
            #endregion

            public KGaranRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KGaran Get(string codeGarantie){
                return connection.Query<KGaran>(select, new {codeGarantie}).SingleOrDefault();
            }

            public string NewId () {
                return idGenerator.NewId("GAGAR").ToString() ;
            }

            public void Insert(KGaran value){
                    if(value.Gagar == default(string)) {
                        value.Gagar = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("GAGAR",value.Gagar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("GADES",value.Gades??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:120, scale: 0);
                    parameters.Add("GADEA",value.Gadea??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("GATAX",value.Gatax??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("GACNX",value.Gacnx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("GACOM",value.Gacom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("GACAR",value.Gacar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("GADFG",value.Gadfg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("GAIFC",value.Gaifc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("GAFAM",value.Gafam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("GARGE",value.Garge??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("GATRG",value.Gatrg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("GAINV",value.Gainv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("GATYI",value.Gatyi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("GARUT",value.Garut??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("GASTA",value.Gasta??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("GAATG",value.Gaatg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KGaran value){
                    var parameters = new DynamicParameters();
                    parameters.Add("codeGarantie",value.Gagar);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KGaran value){
                    var parameters = new DynamicParameters();
                    parameters.Add("GAGAR",value.Gagar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("GADES",value.Gades??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:120, scale: 0);
                    parameters.Add("GADEA",value.Gadea??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("GATAX",value.Gatax??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("GACNX",value.Gacnx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("GACOM",value.Gacom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("GACAR",value.Gacar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("GADFG",value.Gadfg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("GAIFC",value.Gaifc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("GAFAM",value.Gafam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("GARGE",value.Garge??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("GATRG",value.Gatrg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("GAINV",value.Gainv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("GATYI",value.Gatyi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("GARUT",value.Garut??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("GASTA",value.Gasta??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("GAATG",value.Gaatg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("codeGarantie",value.Gagar);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KGaran> GetAll(){
                    return connection.EnsureOpened().Query<KGaran>(select_GetAll).ToList();
            }
    }
}
