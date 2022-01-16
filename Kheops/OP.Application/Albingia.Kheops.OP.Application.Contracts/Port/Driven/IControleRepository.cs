using Albingia.Kheops.OP.Domain.Affaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Port.Driven {
    public interface IControleRepository : IRepriseAvenantRepository {
        void InsertControlAssiette(AffaireId affaireId, string user, string etape, string libelle);
        void AnnulerEtapes(string codeAffaire, int version, IEnumerable<string> etapes, IEnumerable<int> risques = null);
    }
}
