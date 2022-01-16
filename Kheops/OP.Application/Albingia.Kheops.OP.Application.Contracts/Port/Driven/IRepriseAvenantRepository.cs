using Albingia.Kheops.OP.Domain.Affaire;

namespace Albingia.Kheops.OP.Application.Port.Driven {
    public interface IRepriseAvenantRepository {
        void Reprise(AffaireId id);
    }
}
