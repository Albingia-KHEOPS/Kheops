using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using System.Collections.Generic;

namespace Albingia.Kheops.OP.Application.Port.Driven {
    public interface IGarantieRepository {
        Garantie GetById(long id);
        /// <summary>
        /// Search in both current an history
        /// </summary>
        /// <param name="id">The Garantie identifier</param>
        /// <returns></returns>
        Garantie GetLatestById(long id);
        Garantie GetHistoById(long id, int numeroAvenant);
        Garantie GetBySequence(GarantieUniqueId uniqueId);
        string GetRefLibelle(string code);

        
        IEnumerable<Garantie> FindRCGaranties(AffaireId affaireId);
        IEnumerable<Garantie> GetGaranties(AffaireId affaireId);
        IEnumerable<Garantie> GetGarantiesWithTarifs(AffaireId affaireId);

        IDictionary<(int idGar, string codeGar), (ValeursOptionsTarif assiete, TarifGarantie tarif)> GetConditionsGaranties(AffaireId affaireId, int option, int formule);

        /// <summary>
        /// Tries updating KPGARTAR and Assiette KPGARAN
        /// </summary>
        /// <param name="affaireId"></param>
        /// <param name="tarifs"></param>
        /// <returns>The Garantie codes updated</returns>
        IEnumerable<string> UpdateConditions(AffaireId affaireId, IDictionary<int, (ValeursOptionsTarif assiete, TarifGarantie tarif)> tarifs);

        /// <summary>
        /// Tries to update a single Tarif (kpgartar)
        /// </summary>
        /// <param name="tarif"></param>
        /// <returns></returns>
        bool TryUpdateTarif(TarifGarantie tarif);

        ParamGarantie GetParamGarantie(string codeGarantie);
    }
}
