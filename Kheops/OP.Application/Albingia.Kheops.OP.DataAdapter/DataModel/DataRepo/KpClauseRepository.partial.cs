using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class KpClauseRepository {

        const string select_AllClausesContrat = @"SELECT 
KCAID AS KCAID, KCATYP AS KCATYP, KCAIPB AS KCAIPB, KCAALX AS KCAALX, KCAETAPE AS KCAETAPE 
, KCAPERI AS KCAPERI, KCARSQ AS KCARSQ, KCAOBJ AS KCAOBJ, KCAINVEN AS KCAINVEN, KCAINLGN AS KCAINLGN 
, KCAFOR AS KCAFOR, KCAOPT AS KCAOPT, KCAGAR AS KCAGAR, KCACTX AS KCACTX, KCAAJT AS KCAAJT 
, KCANTA AS KCANTA, KCAKDUID AS KCAKDUID, KCACLNM1 AS KCACLNM1, KCACLNM2 AS KCACLNM2, KCACLNM3 AS KCACLNM3 
, KCAVER AS KCAVER, KCATXL AS KCATXL, KCAMER AS KCAMER, KCADOC AS KCADOC, KCACHI AS KCACHI 
, KCACHIS AS KCACHIS, KCAIMP AS KCAIMP, KCACXI AS KCACXI, KCAIAN AS KCAIAN, KCAIAC AS KCAIAC 
, KCASIT AS KCASIT, KCASITD AS KCASITD, KCAAVNC AS KCAAVNC, KCACRD AS KCACRD, KCAAVNM AS KCAAVNM 
, KCAMAJD AS KCAMAJD, KCASPA AS KCASPA, KCATYPO AS KCATYPO, KCAAIM AS KCAAIM, KCATAE AS KCATAE 
, KCAELGO AS KCAELGO, KCAELGI AS KCAELGI, KCAXTL AS KCAXTL, KCATYPD AS KCATYPD, KCAETAFF AS KCAETAFF 
, KCAXTLM AS KCAXTLM 
FROM KPCLAUSE 
WHERE ( KCATYP , KCAIPB , KCAALX ) = ( :typeAffaire , :numeroAffaire , :version ) ";

        internal IEnumerable<KpClause> GetAllByAffaire(string typeAffaire, string numeroAffaire, int version) {
            try {
                return connection.EnsureOpened().Query<KpClause>(select_AllClausesContrat, new { typeAffaire, numeroAffaire, version }).ToList();
            }
            finally {
                connection.EnsureClosed();
            }
        }
    }
}
