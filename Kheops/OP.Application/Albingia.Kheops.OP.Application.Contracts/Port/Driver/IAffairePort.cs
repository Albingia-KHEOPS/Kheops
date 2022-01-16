using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Ecran.ConditionRisqueGarantie;
using OPWebService;
using System.Collections.Generic;
using System.ServiceModel;

namespace Albingia.Kheops.OP.Application.Port.Driver {
    [ServiceContract]
    public interface IAffairePort {
        [OperationContract]
        AffaireDto GetAffaire(AffaireId affaire, bool includeRegul = false);

        [OperationContract]
        AffaireDto GetAffaire1(AffaireId affaireId);

        [OperationContract]
        AffaireId GetCurrentAffaireId(string codeAffaire, int version);

        [OperationContract]
        IEnumerable<AffaireDto> GetFullAffaire(string codeContrat, int version);

        [OperationContract]
        NouvelleAffaireDto GetNouvelleAffaire(AffaireId offre, string codeContrat, int versionContrat);

        [OperationContract]
        void SetSelectionRisqueNouvelleAffaire(NouvelleAffaireDto dto, int numRisque);

        [OperationContract]
        void SetSelectionFormuleNouvelleAffaire(NouvelleAffaireDto dto, int numFormule);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void SaveSelectionFormulesNouvelleAffaire(AffaireId offre, string codeContrat, int version);

        [OperationContract]
        void ValidateNewAffair(AffaireId offre, string code, int version = 0);

        [OperationContract]
        void CancelNewAffairChanges(AffaireId id);

        [OperationContract]
        PagingList<ImpayeDto> GetImpayes(int page = 0, int codeAssure = 0);

        [OperationContract]
        PagingList<RelanceDto> GetUserRelances(int page = 0);

        [OperationContract]
        PagingList<RetardPaiementDto> GetRetardsPaiement(int page, int codeAssure);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void RepriseAvenant(AffaireId affaireId);

        [OperationContract]
        (decimal tauxCom, decimal tauxComCATNAT) GetTauxCommissions(AffaireId affaireId);

        [OperationContract]
        ConnexiteDto GetAffaireConnexite(AffaireId affaireId, int numero, int type);

        /// <summary>
        /// Deverrouille les affaires de l'utilisateur courant
        /// </summary>
        [OperationContract]
        void UnockAffaires();

        [OperationContract]
        VerrouAffaire TryLockAffaire(AffaireId affaireId, string action, string acteGestion = null);

        [OperationContract]
        IEnumerable<VerrouAffaire> TryLockAffaireList(IEnumerable<AffaireId> affaireIds, string action, string acteGestion = null);

        [OperationContract]
        void UnockAffaireList(IEnumerable<AffaireId> affaireIds);

        [OperationContract]
        VerrouAffaire GetCurrentLock(AffaireId affaireId);

        [OperationContract]
        IEnumerable<VerrouAffaire> GetUserLocks();

        /// <summary>
        /// Builds the Affaire identifier, defining either the IsHisto flag if the avenant is supplied, or the avenant
        /// </summary>
        /// <param name="code"></param>
        /// <param name="version"></param>
        /// <param name="avenant"></param>
        /// <returns></returns>
        [OperationContract]
        AffaireId GetAffaireId(string code, int version, int? avenant = null);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void ClasserOffresSansSuite(IEnumerable<(AffaireId id, string motif)> affaireIds);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void DiffererRelances(IEnumerable<(AffaireId id, int delaisJours)> affaireIds);

        [OperationContract]
        void UpdateFlagAttenteCourtier(IEnumerable<(AffaireId id, bool expecting)> flags);

        [OperationContract]
        AffaireDto GetAffaireCanevas(int templateId);

        [OperationContract]
        AffaireDto GetSingleRelance(AffaireId affaireId);

        [OperationContract]
        decimal? GetTauxCommission(AffaireId affaireId, decimal? tauxCommDef = null, AffaireDto affaire = null);

        /// <summary>
        /// Gets Garanties which either code is GAREAT or is flagged Gareat
        /// </summary>
        /// <param name="affaireId"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<ConditionGarantieDto> GetGarantiesGareat(AffaireId affaireId);

        [OperationContract]
        PrimesGareatDto GetPrimesGareat(AffaireId affaireId, bool isReadonly = false);

        [OperationContract]
        void SaveConditions(AffaireId affaireId, ConditionRisqueGarantieGetResultDto conditions);

        [OperationContract]
        void SetGareat(AffaireId affaireId, string trancheMax);

        [OperationContract]
        void SaveGareat(AffaireId affaireId, IEnumerable<(long idTarif, decimal valeur)> valeursGareat);

        [OperationContract]
        void ResetGareat(AffaireId affaireId, int numeroRisque);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void SaveEngagementsTraites(AffaireId affaireId, EngagementTraiteDto traiteDto);
    }
}
