using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Risque;
using System.Collections.Generic;

namespace Albingia.Kheops.OP.Application.Port.Driven {
    public interface IRisqueRepository : IRepriseAvenantRepository {
        IEnumerable<Risque> GetRisquesByAffaire(AffaireId affaire);

        /// <summary>
        /// Retrieves all risques from the beginning AFFNOUV till now
        /// </summary>
        /// <param name="codeOffre">IPB code</param>
        /// <param name="version">ALX code</param>
        /// <returns></returns>
        IEnumerable<Risque> GetAllRisquesByAffaire(string codeOffre, int version);

        void SaveConditions(Risque risque, string user);

        void ToggleCanatFlag(Risque risque);
    }
}
