using Albingia.Kheops.OP.Application.Contracts;
using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.InfosSpecifiques;
using Albingia.Kheops.OP.Domain.Risque;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Port.Driven {
    public interface IInfosSpecifiquesRepository : IRepositoryCache, IRepriseAvenantRepository {
        IEnumerable<ModeleIS> GetModeles(bool resetCache = false);
        (IEnumerable<ModeleIS> modeles, Affaire affaire, IEnumerable<Risque> risques) GetAllAffaireModeles(AffaireId affaireId);
        IEnumerable<LigneModeleIS> GetModeleLignes(ModeleIS modele, Affaire affaire, Risque risque = null, Objet objet = null, int numOption = 0, int numFormule = 0);
        IEnumerable<InformationSpecifique> GetModeleInfos(ModeleIS modele, Affaire affaire, Risque risque = null, Objet objet = null, int option = 0, int formule = 0);
        void SaveISVals(AffaireId affaireId, IEnumerable<InformationSpecifique> infos, string codeModele, string user);
        IEnumerable<SectionIS> GetKPIRSKeys(int maxResults, bool fromHisto = false);
    }
}
