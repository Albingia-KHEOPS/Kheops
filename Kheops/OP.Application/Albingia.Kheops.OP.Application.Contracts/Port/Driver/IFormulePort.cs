using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Inventaire;
using Albingia.Kheops.OP.Domain.Formule;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using OPWebService;
using Albingia.Kheops.Common;

namespace Albingia.Kheops.OP.Application.Port.Driver {

    [ServiceContract]
    public interface IFormulePort
    {
        [OperationContract]
        GarantieDetailsDto GetGarantieDetails(AffaireId id, int numOption, int numFormule, string codeBloc, long sequence, bool isReadonly, DateTime? dateAvenant = null);

        [OperationContract]
        GarantieDto GetGarantie(AffaireId id, int numOption, int numFormule, string codeBloc, long sequence, bool isReadonly);

        [OperationContract]
        FormuleDto GetFormuleAffaire(AffaireId affId, int numFormule, bool isReadOnly = false);

        [OperationContract]
        IEnumerable<FormuleDto> GetAllFormulesOffre(AffaireId affId, IDictionary<int, int[]> newAffairFilter);

        [OperationContract]
        void ResetFormulesOffreSelection(AffaireId affId, string codeContrat, IDictionary<int, int[]> newAffairFilter);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        FormuleDto InitFormuleAffaire(AffaireId affId, int numRisque, IEnumerable<int> numObjet, DateTime? dateAvenant);

        [OperationContract]
        void SetFormuleJson(AffaireId affId, global::OP.WSAS400.DTO.Contrats.Formule formuleDto, int codeFor);

        [OperationContract]
        bool AffaireHasFormules(AffaireId affaireId);

        [OperationContract]
        FormuleDto InitFirstFormuleAffaire(AffaireId affId);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        FormuleDto InitOptionFormuleOffre(AffaireId affId, int numFormule, int numRisque, IEnumerable<int> numObjets, int codeOption);

        [OperationContract]
        AffaireFormuleDto GetFormulesAffaire(AffaireId affaire, bool isReadOnly = false);

        [OperationContract]
        void CancelFormuleAffaireChanges(AffaireId affId);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void ValidateFormuleAffaire(FormuleId formId);

        [OperationContract]
        void SetSelection(FormuleDto formuleInput);

        [OperationContract]
        void SetSelectionVolet(AffaireId affaire, OptionsDetailVoletDto voletDto, int numFormule, int numOption, DateTime? dateAvenant);
        [OperationContract]
        void SetSelectionBloc(AffaireId affaire, OptionsDetailBlocDto blocDto, int numFormule, int numOption, DateTime? dateAvenant);

        [OperationContract]
        void SetSelectionGarantie(AffaireId affaire, GarantieDto garantie, int numFormule, int numOption, DateTime? dateAvenant);

        [OperationContract]
        InventaireDto GetGarantieInventaire(AffaireId id, int numOption, int numFormule, string codeBloc, long sequenceGarantie, bool idReadonly);

        [OperationContract]
        void AddOrUpdateGarantieInventaireItem(AffaireId id, int numOption, int numFormule, string codeBloc, long sequenceGarantie, InventaireItem item);

        [OperationContract]
        void AddOrUpdatePortees(AffaireId id, int numOption, int numFormule, string codeBloc, long sequenceGarantie, ICollection<PorteeGarantieDto> portees, bool reportCalcul);

        [OperationContract]
        void DeleteGarantieInventaireItem(AffaireId id, int numOption, int numFormule, string codeBloc, long sequenceGarantie, int numeroLigne);

        [OperationContract]
        void DeleteWholeGarantieInventaire(AffaireId id, int numOption, int numFormule, string codeBloc, long sequenceGarantie);

        [OperationContract]
        void SaveInventaire(AffaireId id, int numOption, int numFormule, string codeBloc, long sequenceGarantie, InventaireDto inventaire);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void SetGarantieDetails(OptionId id, GarantieDetailsDto garantie);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        (int formule, int option) SetApplication(AffaireId affId, int numFormule, int numRisque, IEnumerable<int> numObjets, DateTime? dateAvenant, int? numOption = null);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void SetDateEffetOption(AffaireId id, int numOption, int numFormule, DateTime dateEffetModif);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void StartNewAvenant(AffaireId id, int numOption, int numFormule, DateTime dateEffetModif);

        [OperationContract]
        IEnumerable<ValidationError> CheckGarantiesDatesInFormule(AffaireId affaireId, int numOption, int numFormule);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void DeleteOption(AffaireId affaireId, int numFormule, int numOption);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        int DuplicateOption(AffaireId affaireId, int numFormule, int numOption);

        [OperationContract]
        IEnumerable<OptionDto> GetOptionsAppsWithHisto(string codeAffaire, int version);

        [OperationContract]
        GarantieDto GetBasicGarantieInfos(AffaireId affaireId, long sequence);

        [OperationContract]
        IEnumerable<RisqueDto> GetApplicationsFormule(AffaireId affaireId, int numOption, int numFormule);

        [OperationContract]
        bool IsAvnDisabled(AffaireId affaireId, int numeroOption, int numeroFormule);

        [OperationContract]
        IEnumerable<ConditionGarantieDto> GetConditionsGaranties(AffaireId affaireId, int option, int formule);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        IEnumerable<string> SaveConditionsGaranties(AffaireId affaireId, bool hasGareat, IEnumerable<ConditionGarantieDto> conditions);

        [OperationContract]
        bool HasGareat(AffaireId affaireId, int risque);

        [OperationContract]
        (bool allowed, IEnumerable<int> numFormules) AllowGareat(AffaireId affaireId, int risque);
    }
}
