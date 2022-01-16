using Albingia.Kheops.OP.Domain.InfosSpecifiques;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class KpIrsObRepository {
        const string select_keys = @"
SELECT DISTINCT IPB , ALX , PBAVN AVN , PBTYP TYP , PBBRA BRANCHE , SECTION  , IFNULL( NULLIF( KFARSQ , 0 ) , KDDRSQ ) RSQ, KFAOBJ OBJ, OPTION, FORMULE 
FROM ( 
	SELECT KFAIPB IPB , KFAALX ALX , KFARSQ, KFAOBJ, 0 OPTION, 0 FORMULE, 'RISQUES' SECTION FROM KPIRSOB WHERE KFAOBJ = 0 UNION 
	SELECT KFAIPB IPB , KFAALX ALX , KFARSQ, KFAOBJ, 0 OPTION, 0 FORMULE, 'OBJETS' SECTION FROM KPIRSOB WHERE KFAOBJ > 0 UNION 
	SELECT KFDIPB IPB , KFDALX ALX , 0 KFARSQ, 0 KFAOBJ, KFDOPT OPTION, KFDFOR FORMULE, 'GARANTIES' SECTION FROM KPIRSGA 
) T 
INNER JOIN ( 
	SELECT PBIPB , PBALX , PBAVN , PBTYP , PBBRA FROM YPOBASE 
	WHERE (PBIPB, PBALX) IN ( SELECT KFAIPB , KFAALX FROM KPIRSOB UNION ALL SELECT KFDIPB , KFDALX FROM KPIRSGA ) 
    AND ( PBIPB , PBALX ) NOT IN ( SELECT KKCIPB , KKCALX FROM KPISVAL ) 
    LIMIT :maxResults 
) PO 
ON ( T.IPB , T.ALX ) = ( PBIPB , PBALX ) 
LEFT JOIN KPOPTAP ON (PBIPB, PBALX, T.FORMULE) = (KDDIPB, KDDALX , KDDFOR) ";

        public IEnumerable<SectionIS> SelectKPIR(int maxResults) {
            if (maxResults < 1) {
                return this.connection.Query<SectionIS>(select_keys.Replace("LIMIT :maxResults", string.Empty)).ToList();
            }
            return this.connection.Query<SectionIS>(select_keys, new { maxResults }).ToList();
        }
    }
}
