using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;

namespace Albingia.Kheops.OP.Application.Port.Driver {
    public interface IGarantiePort {
        string GetLibelleGarantie(string code);
        /// <summary>
        /// Gets the Garantie by its id. Suppling the Avenant brings about searching in history
        /// </summary>
        /// <param name="id">The Garantie identifier</param>
        /// <param name="numeroAvenant">Avenant number</param>
        /// <returns></returns>
        GarantieDto GetGarantieLight(long id, int? numeroAvenant = null);

        /// <summary>
        /// Tries to find the current Garantie. If not exists, get the latest version in history
        /// </summary>
        /// <param name="id">The Garantie identifier</param>
        /// <returns></returns>
        GarantieDto GetLatestGarantieLight(long id);

        GareatStateDto ComputeGareat(AffaireId affaireId, GareatStateDto gareatStateDto, decimal? tauxCommissionsBase = null);

       
     
    }
}
