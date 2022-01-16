using Albingia.Kheops.OP.Domain.Affaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Port.Driven {
    public interface IRelancesRepository {
        void UpdateSituationsRelances(IEnumerable<(AffaireId id, string motif, string code)> valeurs, string user);
    }
}
