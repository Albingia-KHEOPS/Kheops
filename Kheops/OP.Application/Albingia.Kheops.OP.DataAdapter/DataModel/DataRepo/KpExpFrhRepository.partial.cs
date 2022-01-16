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

    public  partial class  KpExpFrhRepository {


        public void DeleteWithCascade(long expressionId)
        {
            const string deleteSubSql = "DELETE FROM KPEXPFRHD WHERE KDLKDKID=:expID";
            connection.Execute(deleteSubSql, new { expId = expressionId });
            this.Delete(new KpExpFrh { Kdkid = expressionId });
        }

        public IEnumerable<KpExpFrh> GetByAffaireWithGaranties(string typeAffaire, string numeroAffaire, int numeroAliment)
        {
            const string getByAffaireHavingGaranties = select_GetByAffaire +
@" and exists (
    SELECT 1 
    from KPGARTAR sub
    WHERE 
        sub.KDKTYP = KPEXPFRH.KDKTYP
        and sub.KDKIPB = KPEXPFRH.KDKIPB
        and sub.KDKALX = KPEXKPEXPFRHFRH.KDKALX
        AND sub.KPGARTAR . KDGKDKID = KPEXPFRH . KDKID
)";
            return connection.Query<KpExpFrh>(getByAffaireHavingGaranties, new { typeAffaire = typeAffaire, numeroAffaire = numeroAffaire, numeroAliment = numeroAliment });
        }
    }
}
