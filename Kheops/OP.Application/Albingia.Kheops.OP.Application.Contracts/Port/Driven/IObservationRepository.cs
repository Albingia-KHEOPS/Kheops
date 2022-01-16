
using ALBINGIA.Framework.Business;

namespace Albingia.Kheops.OP.Application.Port.Driven {
    public interface IObservationRepository : IRepriseAvenantRepository {
        Observation GetObservation(int id, int? avenant = null);
        void AddOrUpdate(Observation observation);
    }
}
