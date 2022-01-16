using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Inventaire;
using System.Collections.Generic;

namespace Albingia.Kheops.OP.Application.Port.Driven
{
    public interface IInventaireRepository : IRepriseAvenantRepository {
        void SaveInventaire(Inventaire inventaire);
        void SaveInventaireApplication(Inventaire inventaire, Garantie gar, int formuleNumber);
        void DeleteInventaire(long inventaireId);
        Inventaire GetInventaireById(long id, int? numAvenant);
        Inventaire GetInventaireByGarantieId(long id, int? numAvenant);
        IEnumerable<Inventaire> GetInventaireByFormule(FormuleId id);
    }
}