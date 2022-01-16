using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using OPWebService;
using System.Collections.Generic;
using System.ServiceModel;

namespace OPServiceContract {
    [ServiceContract]
    public interface IRisque {
        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void SaveInfosComplementairesRisque(OP.WSAS400.DTO.Offres.OffreDto offreDto, ValeursUniteDto lci, ValeursUniteDto franchise, bool isModifHorsAvn = false, IEnumerable<string> ignoredCheckingFields = null);

        [OperationContract]
        RisqueDto GetRisque(AffaireId affaireId, int numero);
    }
}
