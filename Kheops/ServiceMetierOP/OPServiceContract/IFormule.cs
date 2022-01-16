using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using OP.WSAS400.DTO.Ecran.ConditionRisqueGarantie;
using OPWebService;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace OPServiceContract {
    [ServiceContract]
    public interface IFormule {
        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void ValiderConditions(AffaireId affaireId, int option, int formule, ConditionRisqueGarantieGetResultDto conditions);
        [OperationContract]
        IEnumerable<ConditionGarantieDto> GetConditionsGaranties(AffaireId affaireId, int option, int formule);
        [OperationContract]
        bool IsSortie(string codeAffaire, int numRisque, int numFormule, DateTime dateDebutAvn);
        [OperationContract]
        bool HasGareat(AffaireId affaireId, int risque);

        [OperationContract]
        GareatStateDto ComputeGareat(AffaireId affaireId, GareatStateDto gareatStateDto);

        [OperationContract]
        IDictionary<long, GareatStateDto> ComputeEachGareat(AffaireId affaireId, bool computeOnly = false);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void SaveInfosComplementairesRisque(OP.WSAS400.DTO.Offres.OffreDto offreDto, ValeursUniteDto lci, ValeursUniteDto franchise, bool isModifHorsAvn = false, IEnumerable<string> ignoredCheckingFields = null);

        [OperationContract]
        RisqueDto GetRisque(AffaireId affaireId, int numero);

    
    }
}
