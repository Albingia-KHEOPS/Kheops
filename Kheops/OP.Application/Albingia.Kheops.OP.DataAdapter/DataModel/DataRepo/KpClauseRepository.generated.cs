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

    public  partial class  KpClauseRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KCAID AS KCAID, KCATYP AS KCATYP, KCAIPB AS KCAIPB, KCAALX AS KCAALX, KCAETAPE AS KCAETAPE
, KCAPERI AS KCAPERI, KCARSQ AS KCARSQ, KCAOBJ AS KCAOBJ, KCAINVEN AS KCAINVEN, KCAINLGN AS KCAINLGN
, KCAFOR AS KCAFOR, KCAOPT AS KCAOPT, KCAGAR AS KCAGAR, KCACTX AS KCACTX, KCAAJT AS KCAAJT
, KCANTA AS KCANTA, KCAKDUID AS KCAKDUID, KCACLNM1 AS KCACLNM1, KCACLNM2 AS KCACLNM2, KCACLNM3 AS KCACLNM3
, KCAVER AS KCAVER, KCATXL AS KCATXL, KCAMER AS KCAMER, KCADOC AS KCADOC, KCACHI AS KCACHI
, KCACHIS AS KCACHIS, KCAIMP AS KCAIMP, KCACXI AS KCACXI, KCAIAN AS KCAIAN, KCAIAC AS KCAIAC
, KCASIT AS KCASIT, KCASITD AS KCASITD, KCAAVNC AS KCAAVNC, KCACRD AS KCACRD, KCAAVNM AS KCAAVNM
, KCAMAJD AS KCAMAJD, KCASPA AS KCASPA, KCATYPO AS KCATYPO, KCAAIM AS KCAAIM, KCATAE AS KCATAE
, KCAELGO AS KCAELGO, KCAELGI AS KCAELGI, KCAXTL AS KCAXTL, KCATYPD AS KCATYPD, KCAETAFF AS KCAETAFF
, KCAXTLM AS KCAXTLM, KDULIB AS KDULIB, KDULIR AS KDULIR FROM KPCLAUSE
Left join KCLAUSE on KDUID = KCAKDUID
WHERE KCAID = :id
FETCH FIRST 200 ROWS ONLY
";
            const string update=@"UPDATE KPCLAUSE SET 
KCAID = :KCAID, KCATYP = :KCATYP, KCAIPB = :KCAIPB, KCAALX = :KCAALX, KCAETAPE = :KCAETAPE, KCAPERI = :KCAPERI, KCARSQ = :KCARSQ, KCAOBJ = :KCAOBJ, KCAINVEN = :KCAINVEN, KCAINLGN = :KCAINLGN
, KCAFOR = :KCAFOR, KCAOPT = :KCAOPT, KCAGAR = :KCAGAR, KCACTX = :KCACTX, KCAAJT = :KCAAJT, KCANTA = :KCANTA, KCAKDUID = :KCAKDUID, KCACLNM1 = :KCACLNM1, KCACLNM2 = :KCACLNM2, KCACLNM3 = :KCACLNM3
, KCAVER = :KCAVER, KCATXL = :KCATXL, KCAMER = :KCAMER, KCADOC = :KCADOC, KCACHI = :KCACHI, KCACHIS = :KCACHIS, KCAIMP = :KCAIMP, KCACXI = :KCACXI, KCAIAN = :KCAIAN, KCAIAC = :KCAIAC
, KCASIT = :KCASIT, KCASITD = :KCASITD, KCAAVNC = :KCAAVNC, KCACRD = :KCACRD, KCAAVNM = :KCAAVNM, KCAMAJD = :KCAMAJD, KCASPA = :KCASPA, KCATYPO = :KCATYPO, KCAAIM = :KCAAIM, KCATAE = :KCATAE
, KCAELGO = :KCAELGO, KCAELGI = :KCAELGI, KCAXTL = :KCAXTL, KCATYPD = :KCATYPD, KCAETAFF = :KCAETAFF, KCAXTLM = :KCAXTLM
 WHERE 
KCAID = :id";
            const string delete=@"DELETE FROM KPCLAUSE WHERE KCAID = :id";
            const string insert=@"INSERT INTO  KPCLAUSE (
KCAID, KCATYP, KCAIPB, KCAALX, KCAETAPE
, KCAPERI, KCARSQ, KCAOBJ, KCAINVEN, KCAINLGN
, KCAFOR, KCAOPT, KCAGAR, KCACTX, KCAAJT
, KCANTA, KCAKDUID, KCACLNM1, KCACLNM2, KCACLNM3
, KCAVER, KCATXL, KCAMER, KCADOC, KCACHI
, KCACHIS, KCAIMP, KCACXI, KCAIAN, KCAIAC
, KCASIT, KCASITD, KCAAVNC, KCACRD, KCAAVNM
, KCAMAJD, KCASPA, KCATYPO, KCAAIM, KCATAE
, KCAELGO, KCAELGI, KCAXTL, KCATYPD, KCAETAFF
, KCAXTLM
) VALUES (
:KCAID, :KCATYP, :KCAIPB, :KCAALX, :KCAETAPE
, :KCAPERI, :KCARSQ, :KCAOBJ, :KCAINVEN, :KCAINLGN
, :KCAFOR, :KCAOPT, :KCAGAR, :KCACTX, :KCAAJT
, :KCANTA, :KCAKDUID, :KCACLNM1, :KCACLNM2, :KCACLNM3
, :KCAVER, :KCATXL, :KCAMER, :KCADOC, :KCACHI
, :KCACHIS, :KCAIMP, :KCACXI, :KCAIAN, :KCAIAC
, :KCASIT, :KCASITD, :KCAAVNC, :KCACRD, :KCAAVNM
, :KCAMAJD, :KCASPA, :KCATYPO, :KCAAIM, :KCATAE
, :KCAELGO, :KCAELGI, :KCAXTL, :KCATYPD, :KCAETAFF
, :KCAXTLM)";
            const string select_ClausesContrat=@"SELECT
KCAID AS KCAID, KCATYP AS KCATYP, KCAIPB AS KCAIPB, KCAALX AS KCAALX, KCAETAPE AS KCAETAPE
, KCAPERI AS KCAPERI, KCARSQ AS KCARSQ, KCAOBJ AS KCAOBJ, KCAINVEN AS KCAINVEN, KCAINLGN AS KCAINLGN
, KCAFOR AS KCAFOR, KCAOPT AS KCAOPT, KCAGAR AS KCAGAR, KCACTX AS KCACTX, KCAAJT AS KCAAJT
, KCANTA AS KCANTA, KCAKDUID AS KCAKDUID, KCACLNM1 AS KCACLNM1, KCACLNM2 AS KCACLNM2, KCACLNM3 AS KCACLNM3
, KCAVER AS KCAVER, KCATXL AS KCATXL, KCAMER AS KCAMER, KCADOC AS KCADOC, KCACHI AS KCACHI
, KCACHIS AS KCACHIS, KCAIMP AS KCAIMP, KCACXI AS KCACXI, KCAIAN AS KCAIAN, KCAIAC AS KCAIAC
, KCASIT AS KCASIT, KCASITD AS KCASITD, KCAAVNC AS KCAAVNC, KCACRD AS KCACRD, KCAAVNM AS KCAAVNM
, KCAMAJD AS KCAMAJD, KCASPA AS KCASPA, KCATYPO AS KCATYPO, KCAAIM AS KCAAIM, KCATAE AS KCATAE
, KCAELGO AS KCAELGO, KCAELGI AS KCAELGI, KCAXTL AS KCAXTL, KCATYPD AS KCATYPD, KCAETAFF AS KCAETAFF
, KCAXTLM AS KCAXTLM, KDULIB AS KDULIB, KDULIR AS KDULIR FROM KPCLAUSE
Left join KCLAUSE on KDUID = KCAKDUID
WHERE KCATYP = :typeAffaire
and KCAIPB = :numeroAffaire
and KCAALX = :version
and KCAETAPE = :etape
FETCH FIRST 200 ROWS ONLY
";
            const string select_GetByAffaire=@"SELECT
KCAID, KCATYP, KCAIPB, KCAALX, KCAETAPE
, KCAPERI, KCARSQ, KCAOBJ, KCAINVEN, KCAINLGN
, KCAFOR, KCAOPT, KCAGAR, KCACTX, KCAAJT
, KCANTA, KCAKDUID, KCACLNM1, KCACLNM2, KCACLNM3
, KCAVER, KCATXL, KCAMER, KCADOC, KCACHI
, KCACHIS, KCAIMP, KCACXI, KCAIAN, KCAIAC
, KCASIT, KCASITD, KCAAVNC, KCACRD, KCAAVNM
, KCAMAJD, KCASPA, KCATYPO, KCAAIM, KCATAE
, KCAELGO, KCAELGI, KCAXTL, KCATYPD, KCAETAFF
, KCAXTLM FROM KPCLAUSE
WHERE KCATYP = :typeAffaire
and KCAIPB = :codeAffaire
and KCAALX = :numeroAliment
";
            #endregion

            public KpClauseRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpClause Get(Int64 id){
                return connection.Query<KpClause>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KCAID") ;
            }

            public void Insert(KpClause value){
                    if(value.Kcaid == default(Int64)) {
                        value.Kcaid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KCAID",value.Kcaid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KCATYP",value.Kcatyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KCAIPB",value.Kcaipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KCAALX",value.Kcaalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KCAETAPE",value.Kcaetape??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCAPERI",value.Kcaperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCARSQ",value.Kcarsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KCAOBJ",value.Kcaobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KCAINVEN",value.Kcainven, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KCAINLGN",value.Kcainlgn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KCAFOR",value.Kcafor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KCAOPT",value.Kcaopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KCAGAR",value.Kcagar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCACTX",value.Kcactx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCAAJT",value.Kcaajt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KCANTA",value.Kcanta??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KCAKDUID",value.Kcakduid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KCACLNM1",value.Kcaclnm1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KCACLNM2",value.Kcaclnm2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KCACLNM3",value.Kcaclnm3, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KCAVER",value.Kcaver, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KCATXL",value.Kcatxl, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KCAMER",value.Kcamer, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KCADOC",value.Kcadoc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCACHI",value.Kcachi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCACHIS",value.Kcachis??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCAIMP",value.Kcaimp, dbType:DbType.Int64, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCACXI",value.Kcacxi, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 3);
                    parameters.Add("KCAIAN",value.Kcaian??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KCAIAC",value.Kcaiac??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCASIT",value.Kcasit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KCASITD",value.Kcasitd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KCAAVNC",value.Kcaavnc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KCACRD",value.Kcacrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KCAAVNM",value.Kcaavnm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KCAMAJD",value.Kcamajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KCASPA",value.Kcaspa??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KCATYPO",value.Kcatypo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCAAIM",value.Kcaaim??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("KCATAE",value.Kcatae??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KCAELGO",value.Kcaelgo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCAELGI",value.Kcaelgi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KCAXTL",value.Kcaxtl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KCATYPD",value.Kcatypd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KCAETAFF",value.Kcaetaff??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCAXTLM",value.Kcaxtlm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpClause value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Kcaid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpClause value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KCAID",value.Kcaid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KCATYP",value.Kcatyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KCAIPB",value.Kcaipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KCAALX",value.Kcaalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KCAETAPE",value.Kcaetape??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCAPERI",value.Kcaperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCARSQ",value.Kcarsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KCAOBJ",value.Kcaobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KCAINVEN",value.Kcainven, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KCAINLGN",value.Kcainlgn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KCAFOR",value.Kcafor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KCAOPT",value.Kcaopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KCAGAR",value.Kcagar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCACTX",value.Kcactx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCAAJT",value.Kcaajt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KCANTA",value.Kcanta??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KCAKDUID",value.Kcakduid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KCACLNM1",value.Kcaclnm1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KCACLNM2",value.Kcaclnm2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KCACLNM3",value.Kcaclnm3, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KCAVER",value.Kcaver, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KCATXL",value.Kcatxl, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KCAMER",value.Kcamer, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KCADOC",value.Kcadoc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCACHI",value.Kcachi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCACHIS",value.Kcachis??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCAIMP",value.Kcaimp, dbType:DbType.Int64, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCACXI",value.Kcacxi, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 3);
                    parameters.Add("KCAIAN",value.Kcaian??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KCAIAC",value.Kcaiac??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCASIT",value.Kcasit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KCASITD",value.Kcasitd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KCAAVNC",value.Kcaavnc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KCACRD",value.Kcacrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KCAAVNM",value.Kcaavnm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KCAMAJD",value.Kcamajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KCASPA",value.Kcaspa??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KCATYPO",value.Kcatypo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCAAIM",value.Kcaaim??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("KCATAE",value.Kcatae??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KCAELGO",value.Kcaelgo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCAELGI",value.Kcaelgi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KCAXTL",value.Kcaxtl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KCATYPD",value.Kcatypd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KCAETAFF",value.Kcaetaff??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KCAXTLM",value.Kcaxtlm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("id",value.Kcaid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpClause> ClausesContrat(string typeAffaire, string numeroAffaire, int version, string etape){
                    return connection.EnsureOpened().Query<KpClause>(select_ClausesContrat, new {typeAffaire, numeroAffaire, version, etape}).ToList();
            }
            public IEnumerable<KpClause> GetByAffaire(string typeAffaire, string codeAffaire, int numeroAliment){
                    return connection.EnsureOpened().Query<KpClause>(select_GetByAffaire, new {typeAffaire, codeAffaire, numeroAliment}).ToList();
            }
    }
}
