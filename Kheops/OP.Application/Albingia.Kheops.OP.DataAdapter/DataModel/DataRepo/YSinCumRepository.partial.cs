using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class YSinCumRepository {
        static readonly string select_CumulsPreneur = $@"
SELECT YSINCUM.*
FROM YSINIST INNER JOIN ( 
    SELECT PBIPB , PBALX , PBIAS 
    FROM YPOBASE  WHERE PBETA = 'V' AND PBTYP = 'P' AND PBSTA > 0 
) AS PO ON ( SIIPB , SIALX , :IAS ) = ( PBIPB , PBALX , PBIAS ) 
INNER JOIN YSINCUM ON ( SISUA , SINUM , SISBR ) = ( SUSUA , SUNUM , SUSBR ) ";

        internal IEnumerable<YSinCum> SelectCumulsPreneur(int codeAssure) {
            var c = this.connection.EnsureOpened();
            return c.Query<YSinCum>(select_CumulsPreneur, new { IAS = codeAssure }).ToList();
        }
    }
}
