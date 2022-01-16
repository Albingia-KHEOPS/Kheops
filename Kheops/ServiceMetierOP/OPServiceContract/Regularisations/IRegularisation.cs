
using ALBINGIA.Framework.Common;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Regularisation;
using OPWebService;
using System.Collections.Generic;
using System.ServiceModel;

namespace OPServiceContract
{
    [ServiceContract]
    public interface IRegularisation
    {
        [OperationContract]
        RegularisationContext BuildContext(RegularisationContext initialContext);

        [OperationContract]
        RegularisationContext EnsureContext(RegularisationContext context);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        RegularisationInfoDto Init(Folder folder, RegularisationContext context, CauseResetRegularisation? causeReset = null);

        [OperationContract]
        List<ParametreDto> GetModeleTypeRegul(string codeOffre, string version, string type, string codeAvn);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        RegularisationContext GetPreviousStep(RegularisationContext context);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        RegularisationContext ValidateStepAndGetNext(RegularisationContext context, bool cancelMode = false);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        RegularisationContext ValidateStepAndRichOther(RegularisationContext context, RegularisationStep stepToRich);

        [OperationContract]
        RegularisationComputeData GetInfosRegularisationContrat(RegularisationMode mode, long regularisationId, long numAvt);

        [OperationContract]
        IEnumerable<RegularisationStateDto> GetCurrentStates(long rgId);

        [OperationContract]
        string ComputeRegularisation(RegularisationContext context, RegularisationComputeData figures);

        [OperationContract]
        RegularisationComputeData ComputeCotisations(RegularisationComputeData computeData);

        [OperationContract]
        List<SaisieRisqueInfoDto> GetListSaisieRisqueRegulatisation(RegularisationContext context);

        [OperationContract]
        void CancelRegularisationRisque(long rgId, int rsqId);

        [OperationContract]
        RegularisationComputeData GetInfosRegularisationRisque(RegularisationMode mode, long regularisationId, long codeRsq, long numAvt);

        [OperationContract]
        IdContratDto GetContratInfo(string codeOffre, string version, string type);

        [OperationContract]
        RegularisationBandeauContratDto GetBandeauContratInfo(string codeOffre, string version, string type, bool lightVersion = false);

        [OperationContract]
        List<ParametreDto> GetAvailableUnites();

        [OperationContract]
        List<ParametreDto> GetAvailableCodeTaxes();

        [OperationContract]
        SaisieGarantieInfosDto GetGarantiesRCFRHeader(RegularisationContext context);

        [OperationContract]
        ListeGarantiesRCDto GetGarantiesRCGroup(RegularisationContext context);

        [OperationContract]
        ListeGarantiesRCDto ComputeGarantiesRC(RegularisationContext context, ListeGarantiesRCDto garanties);

        [OperationContract]
        string ValidateGarantiesRC(RegularisationContext context, ListeGarantiesRCDto garanties);

        [OperationContract]
        List<LigneMouvtGarantieDto> SupprimerMouvtPeriod(RegularisationContext context);

        [OperationContract]
        bool IsSimplifiedReguleFlow(RegularisationContext context);

        [OperationContract]
        LigneRegularisationDto GetLastRegularisation(string codeContrat, string version, string type, string codeAvn);

        [OperationContract]
        LigneRegularisationDto GetRegularisationByID(string codeContrat, string version, string type, long rgId);

        [OperationContract]
        void ReportDataRegulFromRsqToEnt(string codeContrat, string version, string codeAvn);

        [OperationContract]
        RegularisationComputeData GetInfosRegularisationContratTR(long rgId, long numAvt);

        [OperationContract]
        RegularisationComputeData GetInfosRegularisationRisqueTR(long rgId, long rsqId);
        
        [OperationContract]
        string ComputeRegularisationTR(RegularisationContext context, RegularisationComputeData figures);

        [OperationContract]
        string GetMotifRegularisation(long reguleId);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        RegularisationGarInfoDto GetInfoRegularisationGarantie(RegularisationContext context);
    }
}
