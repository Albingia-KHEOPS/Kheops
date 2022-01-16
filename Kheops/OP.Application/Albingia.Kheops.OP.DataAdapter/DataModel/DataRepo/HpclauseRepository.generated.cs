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

    public  partial class  HpclauseRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KCAID, KCATYP, KCAIPB, KCAALX, KCAAVN
, KCAHIN, KCAETAPE, KCAPERI, KCARSQ, KCAOBJ
, KCAINVEN, KCAINLGN, KCAFOR, KCAOPT, KCAGAR
, KCACTX, KCAAJT, KCANTA, KCAKDUID, KCACLNM1
, KCACLNM2, KCACLNM3, KCAVER, KCATXL, KCAMER
, KCADOC, KCACHI, KCACHIS, KCAIMP, KCACXI
, KCAIAN, KCAIAC, KCASIT, KCASITD, KCAAVNC
, KCACRD, KCAAVNM, KCAMAJD, KCASPA, KCATYPO
, KCAAIM, KCATAE, KCAELGO, KCAELGI, KCAXTL
, KCATYPD, KCAETAFF, KCAXTLM FROM HPCLAUSE
WHERE KCAID = :KCAID
";
            const string update=@"UPDATE HPCLAUSE SET 
KCAID = :KCAID, KCATYP = :KCATYP, KCAIPB = :KCAIPB, KCAALX = :KCAALX, KCAAVN = :KCAAVN, KCAHIN = :KCAHIN, KCAETAPE = :KCAETAPE, KCAPERI = :KCAPERI, KCARSQ = :KCARSQ, KCAOBJ = :KCAOBJ
, KCAINVEN = :KCAINVEN, KCAINLGN = :KCAINLGN, KCAFOR = :KCAFOR, KCAOPT = :KCAOPT, KCAGAR = :KCAGAR, KCACTX = :KCACTX, KCAAJT = :KCAAJT, KCANTA = :KCANTA, KCAKDUID = :KCAKDUID, KCACLNM1 = :KCACLNM1
, KCACLNM2 = :KCACLNM2, KCACLNM3 = :KCACLNM3, KCAVER = :KCAVER, KCATXL = :KCATXL, KCAMER = :KCAMER, KCADOC = :KCADOC, KCACHI = :KCACHI, KCACHIS = :KCACHIS, KCAIMP = :KCAIMP, KCACXI = :KCACXI
, KCAIAN = :KCAIAN, KCAIAC = :KCAIAC, KCASIT = :KCASIT, KCASITD = :KCASITD, KCAAVNC = :KCAAVNC, KCACRD = :KCACRD, KCAAVNM = :KCAAVNM, KCAMAJD = :KCAMAJD, KCASPA = :KCASPA, KCATYPO = :KCATYPO
, KCAAIM = :KCAAIM, KCATAE = :KCATAE, KCAELGO = :KCAELGO, KCAELGI = :KCAELGI, KCAXTL = :KCAXTL, KCATYPD = :KCATYPD, KCAETAFF = :KCAETAFF, KCAXTLM = :KCAXTLM
 WHERE 
KCAID = :KCAID";
            const string delete=@"DELETE FROM HPCLAUSE WHERE KCAID = :KCAID";
            const string insert=@"INSERT INTO  HPCLAUSE (
KCAID, KCATYP, KCAIPB, KCAALX, KCAAVN
, KCAHIN, KCAETAPE, KCAPERI, KCARSQ, KCAOBJ
, KCAINVEN, KCAINLGN, KCAFOR, KCAOPT, KCAGAR
, KCACTX, KCAAJT, KCANTA, KCAKDUID, KCACLNM1
, KCACLNM2, KCACLNM3, KCAVER, KCATXL, KCAMER
, KCADOC, KCACHI, KCACHIS, KCAIMP, KCACXI
, KCAIAN, KCAIAC, KCASIT, KCASITD, KCAAVNC
, KCACRD, KCAAVNM, KCAMAJD, KCASPA, KCATYPO
, KCAAIM, KCATAE, KCAELGO, KCAELGI, KCAXTL
, KCATYPD, KCAETAFF, KCAXTLM
) VALUES (
:KCAID, :KCATYP, :KCAIPB, :KCAALX, :KCAAVN
, :KCAHIN, :KCAETAPE, :KCAPERI, :KCARSQ, :KCAOBJ
, :KCAINVEN, :KCAINLGN, :KCAFOR, :KCAOPT, :KCAGAR
, :KCACTX, :KCAAJT, :KCANTA, :KCAKDUID, :KCACLNM1
, :KCACLNM2, :KCACLNM3, :KCAVER, :KCATXL, :KCAMER
, :KCADOC, :KCACHI, :KCACHIS, :KCAIMP, :KCACXI
, :KCAIAN, :KCAIAC, :KCASIT, :KCASITD, :KCAAVNC
, :KCACRD, :KCAAVNM, :KCAMAJD, :KCASPA, :KCATYPO
, :KCAAIM, :KCATAE, :KCAELGO, :KCAELGI, :KCAXTL
, :KCATYPD, :KCAETAFF, :KCAXTLM)";
            const string select_GetByAffaire=@"SELECT
KCAID, KCATYP, KCAIPB, KCAALX, KCAAVN
, KCAHIN, KCAETAPE, KCAPERI, KCARSQ, KCAOBJ
, KCAINVEN, KCAINLGN, KCAFOR, KCAOPT, KCAGAR
, KCACTX, KCAAJT, KCANTA, KCAKDUID, KCACLNM1
, KCACLNM2, KCACLNM3, KCAVER, KCATXL, KCAMER
, KCADOC, KCACHI, KCACHIS, KCAIMP, KCACXI
, KCAIAN, KCAIAC, KCASIT, KCASITD, KCAAVNC
, KCACRD, KCAAVNM, KCAMAJD, KCASPA, KCATYPO
, KCAAIM, KCATAE, KCAELGO, KCAELGI, KCAXTL
, KCATYPD, KCAETAFF, KCAXTLM FROM HPCLAUSE
WHERE KCATYP = :KCATYP
and KCAIPB = :KCAIPB
and KCAALX = :KCAALX
and KCAAVN = :KCAAVN
";
            #endregion

            public HpclauseRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpClause Get(Int64 KCAID){
                return connection.Query<KpClause>(select, new {KCAID}).SingleOrDefault();
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
                    parameters.Add("KCAAVN",value.Kcaavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KCAHIN",value.Kcahin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
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
                    parameters.Add("KCAID",value.Kcaid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpClause value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KCAID",value.Kcaid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KCATYP",value.Kcatyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KCAIPB",value.Kcaipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KCAALX",value.Kcaalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KCAAVN",value.Kcaavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KCAHIN",value.Kcahin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
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
                    parameters.Add("KCAID",value.Kcaid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpClause> GetByAffaire(string KCATYP, string KCAIPB, int KCAALX, int KCAAVN){
                    return connection.EnsureOpened().Query<KpClause>(select_GetByAffaire, new {KCATYP, KCAIPB, KCAALX, KCAAVN}).ToList();
            }
    }
}
