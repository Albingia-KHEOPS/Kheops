using Albingia.Kheops.OP.Domain.Affaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Port.Driven
{
    public interface IDesignationRepository : IRepriseAvenantRepository {
        string GetDesignation(long id);
        string GetDesignation(long id, int avenant);
        IDictionary<int, string> GetDesignationsByAffaire(AffaireId id);

        int UpdateDesi(string value, AffaireId affId, long id, string type);
        void Delete(long id);
        void DeleteForInventaire(long inventaireId);
    }
}
