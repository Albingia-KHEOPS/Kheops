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

    public  partial class  YcategoRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
CABRA, CASBR, CACAT, CADES, CADEA
, CARCG, CARCS, CAAFR, CATAX, CARGC
, CAPMI, CAATT, CACNP, CACNC, CASMP
, CAUSR, CAMJA, CAMJM, CAMJJ, CACNX
, CAATX, CAVAT, CASAI, CAINA, CAIND
, CAIXC, CAIXF, CAIXL, CAIXP, CAIPM
, CAAUT, CALIB, CARST, CAAPR, CAGRI
, CAEDI FROM YCATEGO
WHERE CABRA = :branche
and CASBR = :sousBranche
and CACAT = :categorie
";
            const string update=@"UPDATE YCATEGO SET 
CABRA = :CABRA, CASBR = :CASBR, CACAT = :CACAT, CADES = :CADES, CADEA = :CADEA, CARCG = :CARCG, CARCS = :CARCS, CAAFR = :CAAFR, CATAX = :CATAX, CARGC = :CARGC
, CAPMI = :CAPMI, CAATT = :CAATT, CACNP = :CACNP, CACNC = :CACNC, CASMP = :CASMP, CAUSR = :CAUSR, CAMJA = :CAMJA, CAMJM = :CAMJM, CAMJJ = :CAMJJ, CACNX = :CACNX
, CAATX = :CAATX, CAVAT = :CAVAT, CASAI = :CASAI, CAINA = :CAINA, CAIND = :CAIND, CAIXC = :CAIXC, CAIXF = :CAIXF, CAIXL = :CAIXL, CAIXP = :CAIXP, CAIPM = :CAIPM
, CAAUT = :CAAUT, CALIB = :CALIB, CARST = :CARST, CAAPR = :CAAPR, CAGRI = :CAGRI, CAEDI = :CAEDI
 WHERE 
CABRA = :branche and CASBR = :sousBranche and CACAT = :categorie";
            const string delete=@"DELETE FROM YCATEGO WHERE CABRA = :branche AND CASBR = :sousBranche AND CACAT = :categorie";
            const string insert=@"INSERT INTO  YCATEGO (
CABRA, CASBR, CACAT, CADES, CADEA
, CARCG, CARCS, CAAFR, CATAX, CARGC
, CAPMI, CAATT, CACNP, CACNC, CASMP
, CAUSR, CAMJA, CAMJM, CAMJJ, CACNX
, CAATX, CAVAT, CASAI, CAINA, CAIND
, CAIXC, CAIXF, CAIXL, CAIXP, CAIPM
, CAAUT, CALIB, CARST, CAAPR, CAGRI
, CAEDI
) VALUES (
:CABRA, :CASBR, :CACAT, :CADES, :CADEA
, :CARCG, :CARCS, :CAAFR, :CATAX, :CARGC
, :CAPMI, :CAATT, :CACNP, :CACNC, :CASMP
, :CAUSR, :CAMJA, :CAMJM, :CAMJJ, :CACNX
, :CAATX, :CAVAT, :CASAI, :CAINA, :CAIND
, :CAIXC, :CAIXF, :CAIXL, :CAIXP, :CAIPM
, :CAAUT, :CALIB, :CARST, :CAAPR, :CAGRI
, :CAEDI)";
            const string select_GetAll=@"SELECT
CABRA, CASBR, CACAT, CADES, CADEA
, CARCG, CARCS, CAAFR, CATAX, CARGC
, CAPMI, CAATT, CACNP, CACNC, CASMP
, CAUSR, CAMJA, CAMJM, CAMJJ, CACNX
, CAATX, CAVAT, CASAI, CAINA, CAIND
, CAIXC, CAIXF, CAIXL, CAIXP, CAIPM
, CAAUT, CALIB, CARST, CAAPR, CAGRI
, CAEDI FROM YCATEGO
";
            #endregion

            public YcategoRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public Ycatego Get(string branche, string sousBranche, string categorie){
                return connection.Query<Ycatego>(select, new {branche, sousBranche, categorie}).SingleOrDefault();
            }


            public void Insert(Ycatego value){
                    var parameters = new DynamicParameters();
                    parameters.Add("CABRA",value.Cabra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("CASBR",value.Casbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("CACAT",value.Cacat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("CADES",value.Cades??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("CADEA",value.Cadea??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("CARCG",value.Carcg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("CARCS",value.Carcs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("CAAFR",value.Caafr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("CATAX",value.Catax??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("CARGC",value.Cargc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("CAPMI",value.Capmi, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("CAATT",value.Caatt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CACNP",value.Cacnp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("CACNC",value.Cacnc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("CASMP",value.Casmp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("CAUSR",value.Causr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("CAMJA",value.Camja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("CAMJM",value.Camjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("CAMJJ",value.Camjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("CACNX",value.Cacnx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("CAATX",value.Caatx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("CAVAT",value.Cavat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("CASAI",value.Casai??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CAINA",value.Caina??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CAIND",value.Caind??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("CAIXC",value.Caixc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CAIXF",value.Caixf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CAIXL",value.Caixl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CAIXP",value.Caixp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CAIPM",value.Caipm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CAAUT",value.Caaut??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("CALIB",value.Calib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:38, scale: 0);
                    parameters.Add("CARST",value.Carst??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CAAPR",value.Caapr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CAGRI",value.Cagri??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("CAEDI",value.Caedi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(Ycatego value){
                    var parameters = new DynamicParameters();
                    parameters.Add("branche",value.Cabra);
                    parameters.Add("sousBranche",value.Casbr);
                    parameters.Add("categorie",value.Cacat);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(Ycatego value){
                    var parameters = new DynamicParameters();
                    parameters.Add("CABRA",value.Cabra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("CASBR",value.Casbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("CACAT",value.Cacat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("CADES",value.Cades??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("CADEA",value.Cadea??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("CARCG",value.Carcg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("CARCS",value.Carcs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("CAAFR",value.Caafr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("CATAX",value.Catax??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("CARGC",value.Cargc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("CAPMI",value.Capmi, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("CAATT",value.Caatt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CACNP",value.Cacnp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("CACNC",value.Cacnc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("CASMP",value.Casmp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("CAUSR",value.Causr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("CAMJA",value.Camja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("CAMJM",value.Camjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("CAMJJ",value.Camjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("CACNX",value.Cacnx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("CAATX",value.Caatx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("CAVAT",value.Cavat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("CASAI",value.Casai??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CAINA",value.Caina??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CAIND",value.Caind??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("CAIXC",value.Caixc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CAIXF",value.Caixf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CAIXL",value.Caixl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CAIXP",value.Caixp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CAIPM",value.Caipm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CAAUT",value.Caaut??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("CALIB",value.Calib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:38, scale: 0);
                    parameters.Add("CARST",value.Carst??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CAAPR",value.Caapr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("CAGRI",value.Cagri??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("CAEDI",value.Caedi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("branche",value.Cabra);
                    parameters.Add("sousBranche",value.Casbr);
                    parameters.Add("categorie",value.Cacat);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<Ycatego> GetAll(){
                    return connection.EnsureOpened().Query<Ycatego>(select_GetAll).ToList();
            }
    }
}
