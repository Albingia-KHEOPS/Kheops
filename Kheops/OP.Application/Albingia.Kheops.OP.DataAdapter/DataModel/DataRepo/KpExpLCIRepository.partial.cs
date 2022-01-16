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

    public  partial class  KpExpLCIRepository {

        public void DeleteWithCascade(long expressionId)
        {
            const string delete1Sql = "DELETE FROM KPEXPLCID WHERE KDJKDIID=:expID";
            connection.Execute(delete1Sql, new { expId = expressionId });
            this.Delete(new KpExpLCI { Kdiid = expressionId });
        }


        public IEnumerable<KpExpLCI> GetByAffaireWithGaranties(string typeAffaire, string numeroAffaire, int numeroAliment)
        {
            const string getByAffaireHavingGaranties = select_GetByAffaire +
@" and exists (
    SELECT 1 
    from KPGARTAR sub
    WHERE 
        sub.KDKTYP = KPEXPLCI.KDITYP
        and sub.KDKIPB = KPEXPLCI.KDIIPB
        and sub.KDKALX = KPEXPLCI.KDIALX
        AND sub.KPGARTAR . KDGKDIID = KPEXPLCI . KDIID
)";
            return connection.Query<KpExpLCI>(getByAffaireHavingGaranties, new { typeAffaire = typeAffaire, numeroAffaire = numeroAffaire, numeroAliment = numeroAliment });
        }
    }
}
