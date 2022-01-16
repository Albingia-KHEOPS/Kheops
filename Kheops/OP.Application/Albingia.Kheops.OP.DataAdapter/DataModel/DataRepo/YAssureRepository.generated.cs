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

    public  partial class  YAssureRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
ASIAS, ASAD1, ASAD2, ASDEP, ASCPO
, ASVIL, ASPAY, ASCOM, ASREG, ASFDC
, ASTEL, ASTLC, ASSIR, ASAPE, ASAPG
, ASLIG, ASGRS, ASCRA, ASCRM, ASCRJ
, ASUSR, ASMJA, ASMJM, ASMJJ, ASINS
, ASPUB, ASADH, ASNIC, ASAP5 FROM YASSURE
WHERE ASIAS = :code
";
            const string update=@"UPDATE YASSURE SET 
ASIAS = :ASIAS, ASAD1 = :ASAD1, ASAD2 = :ASAD2, ASDEP = :ASDEP, ASCPO = :ASCPO, ASVIL = :ASVIL, ASPAY = :ASPAY, ASCOM = :ASCOM, ASREG = :ASREG, ASFDC = :ASFDC
, ASTEL = :ASTEL, ASTLC = :ASTLC, ASSIR = :ASSIR, ASAPE = :ASAPE, ASAPG = :ASAPG, ASLIG = :ASLIG, ASGRS = :ASGRS, ASCRA = :ASCRA, ASCRM = :ASCRM, ASCRJ = :ASCRJ
, ASUSR = :ASUSR, ASMJA = :ASMJA, ASMJM = :ASMJM, ASMJJ = :ASMJJ, ASINS = :ASINS, ASPUB = :ASPUB, ASADH = :ASADH, ASNIC = :ASNIC, ASAP5 = :ASAP5
 WHERE 
ASIAS = :code";
            const string delete=@"DELETE FROM YASSURE WHERE ASIAS = :code";
            const string insert=@"INSERT INTO  YASSURE (
ASIAS, ASAD1, ASAD2, ASDEP, ASCPO
, ASVIL, ASPAY, ASCOM, ASREG, ASFDC
, ASTEL, ASTLC, ASSIR, ASAPE, ASAPG
, ASLIG, ASGRS, ASCRA, ASCRM, ASCRJ
, ASUSR, ASMJA, ASMJM, ASMJJ, ASINS
, ASPUB, ASADH, ASNIC, ASAP5
) VALUES (
:ASIAS, :ASAD1, :ASAD2, :ASDEP, :ASCPO
, :ASVIL, :ASPAY, :ASCOM, :ASREG, :ASFDC
, :ASTEL, :ASTLC, :ASSIR, :ASAPE, :ASAPG
, :ASLIG, :ASGRS, :ASCRA, :ASCRM, :ASCRJ
, :ASUSR, :ASMJA, :ASMJM, :ASMJJ, :ASINS
, :ASPUB, :ASADH, :ASNIC, :ASAP5)";
            #endregion

            public YAssureRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YAssure Get(int code){
                return connection.Query<YAssure>(select, new {code}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("ASIAS") ;
            }

            public void Insert(YAssure value){
                    if(value.Asias == default(int)) {
                        value.Asias = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("ASIAS",value.Asias, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("ASAD1",value.Asad1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("ASAD2",value.Asad2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("ASDEP",value.Asdep??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ASCPO",value.Ascpo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("ASVIL",value.Asvil??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:26, scale: 0);
                    parameters.Add("ASPAY",value.Aspay??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("ASCOM",value.Ascom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("ASREG",value.Asreg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ASFDC",value.Asfdc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("ASTEL",value.Astel??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("ASTLC",value.Astlc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("ASSIR",value.Assir, dbType:DbType.Int32, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("ASAPE",value.Asape??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("ASAPG",value.Asapg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ASLIG",value.Aslig??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("ASGRS",value.Asgrs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ASCRA",value.Ascra, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("ASCRM",value.Ascrm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ASCRJ",value.Ascrj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ASUSR",value.Asusr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("ASMJA",value.Asmja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("ASMJM",value.Asmjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ASMJJ",value.Asmjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ASINS",value.Asins??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("ASPUB",value.Aspub??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("ASADH",value.Asadh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("ASNIC",value.Asnic, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("ASAP5",value.Asap5??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YAssure value){
                    var parameters = new DynamicParameters();
                    parameters.Add("code",value.Asias);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YAssure value){
                    var parameters = new DynamicParameters();
                    parameters.Add("ASIAS",value.Asias, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("ASAD1",value.Asad1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("ASAD2",value.Asad2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("ASDEP",value.Asdep??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ASCPO",value.Ascpo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("ASVIL",value.Asvil??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:26, scale: 0);
                    parameters.Add("ASPAY",value.Aspay??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("ASCOM",value.Ascom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("ASREG",value.Asreg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ASFDC",value.Asfdc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("ASTEL",value.Astel??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("ASTLC",value.Astlc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("ASSIR",value.Assir, dbType:DbType.Int32, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("ASAPE",value.Asape??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("ASAPG",value.Asapg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ASLIG",value.Aslig??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("ASGRS",value.Asgrs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ASCRA",value.Ascra, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("ASCRM",value.Ascrm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ASCRJ",value.Ascrj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ASUSR",value.Asusr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("ASMJA",value.Asmja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("ASMJM",value.Asmjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ASMJJ",value.Asmjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("ASINS",value.Asins??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("ASPUB",value.Aspub??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("ASADH",value.Asadh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("ASNIC",value.Asnic, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("ASAP5",value.Asap5??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("code",value.Asias);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
    }
}
