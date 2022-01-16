using Albingia.Kheops.OP.Domain.Engagements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Port.Driven {
    public interface IEngagementRepository : IRepriseAvenantRepository {
        Engagement GetEngagement(int id);
        Engagement GetEngagement(string ipb, int alx, string type);
        void UpdateTraiteSMP(string codeAffaire, int version, string type, IEnumerable<(long idVentilation, int codeRisque, long smp)> valeurs);
        void UpdateTraiteSMPTotal(int id, int  total);
    }
}
