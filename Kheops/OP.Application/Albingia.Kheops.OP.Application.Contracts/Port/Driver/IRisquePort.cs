using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using OP.WSAS400.DTO.Ecran.ConditionRisqueGarantie;
using System.Collections.Generic;
using System.ServiceModel;

namespace Albingia.Kheops.OP.Application.Port.Driver {
    [ServiceContract]
    public interface IRisquePort
    {
        [OperationContract]
        IEnumerable<RisqueDto> GetRisques(AffaireId affaire);

        /// <summary>
        /// Retrieves all risques-objets from the beginning AFFNOUV till now
        /// </summary>
        /// <param name="codeOffre">IPB code</param>
        /// <param name="version">ALX code</param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<RisqueDto> GetAllRisquesByAffaire(string codeOffre, int version);

        [OperationContract]
        bool IsAvnDisabled(AffaireId affaireId, int numeroRisque);

        [OperationContract]
        void SaveConditions(AffaireId affaireId, RisqueDto risque, ConditionRisqueGarantieGetResultDto conditions);

        [OperationContract]
        void ToggleCanatFlag(AffaireId affaireId, int numeroRisque, bool allowCanat);
    }
}
