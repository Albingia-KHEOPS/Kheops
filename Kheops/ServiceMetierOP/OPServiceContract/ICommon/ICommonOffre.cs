using System.Collections.Generic;
using System.ServiceModel;
using ALBINGIA.Framework.Common.Constants;
using System;
using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.DTO;
using ALBINGIA.Framework.Common.Tools;

using OPWebService;
using Albingia.Kheops.OP.Domain.Affaire;
using WSDto = OP.WSAS400.DTO;

namespace OPServiceContract.ICommon
{
    [ServiceContract]
    public interface ICommonOffre
    {
        [OperationContract]
        ProfileKheops GetProfileKheops();

        [OperationContract]
        ProfileKheops SetProfileKheops(ProfileKheops profile, IEnumerable<ProfileKheopsData> specificUpdate = null);

        [OperationContract]
        PagingList<ImpayeDto> GetImpayes(int page = 0, int codeAssure = 0);

        [OperationContract]
        PagingList<RetardPaiementDto> GetRetardsPaiement(int page = 0, int codeAssure = 0);

        [OperationContract]
        PagingList<SinistreDto> GetSinistres(int page = 0, int codeAssure = 0);

        [OperationContract]
        PagingList<RelanceDto> GetUserRelances(int page = 0);

        [OperationContract]
        string GetAs400User(string adUser);
        [OperationContract]
        bool GetIsAdmin(string adUser);
        [OperationContract]
        WSDto.NavigationArbre.NavigationArbreDto GetHierarchy(string codeOffre, int version, string type, string codeAvn, ModeConsultation modeNavig, string acteGestionRegule);

        [OperationContract]
        void SetTrace(WSDto.NavigationArbre.TraceDto trace);

        [OperationContract]
        bool ExisteTrace(string codeOffre, int version, string type, string etapeGeneration);

        [OperationContract]
        List<WSDto.Common.ActeGestionDto> GetListActesGestion(string codeOffre, string version, string type, DateTime? dateDeb, DateTime? dateFin, string user, string traitement);

        [OperationContract]
        string GetInfoActeGstion(string codeAffaire, string version, string type);
        [OperationContract]
        WSDto.Common.LCIFranchiseUnitesTypesDto GetLCIFranchiseUnitesTypes(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig);
        [OperationContract]
        WSDto.Common.BandeauDto GetBandeau(string codeOffre, string versionOffre, string typeOffre);
        [OperationContract]
        WSDto.Common.BandeauDto GetBandeauHisto(string codeOffre, string versionOffre, string typeOffre, string numAvenant);
        [OperationContract]
        WSDto.Common.VisuObservationsDto GetVisuObservations(string codeOffre, string version, string type);
        [OperationContract]
        WSDto.Common.VisuSuspensionDto GetVisuSuspension(string codeOffre, string version, string type);
        [OperationContract]
        WSDto.Common.VisuInfosContratDto GetInfosContrat(string codeOffre, string version, string type);
        [OperationContract]
        string OpenWordDocument(string type, string param, string spliChar, bool clauseResolu, bool clauseLibre, bool createClause, string rule, ModeConsultation modeNavig,
            string contexte, string paramGestionDossier, string user, string wrkStation, string ipStation, string userAD,
            int switchModuleGestDoc, int numAvenant);
        [OperationContract]
        string SaveDocMagnetic(string codeAffaire, string version, string type, string codeAvn,
            string codeRsq, string codeObj, string codeFor, string codeOpt,
            string typeDoc, string service, string acteGestion, string ajout, string contexte,
            string idClause, string titleClause, string emplacement, string sousemplacement, string ordre,
            string fullFilePath, string etape);

        [OperationContract]
        string GetClauseFileName(string clauseId);

        #region Gestion des intervenants

        [OperationContract]
        WSDto.GestionIntervenants.IntervenantsInfoDto GetListeIntervenants(string code, string version, string type, string orderby, string ascDesc);

        [OperationContract]
        List<WSDto.GestionIntervenants.IntervenantDto> GetListeIntervenantsAutocomplete(string code, string version, string type, string name, string typeIntervenant, string codeIntervenant, bool fromAffaireOnly);

        [OperationContract]
        WSDto.GestionIntervenants.IntervenantDto GetIntervenantByCodeAutocomplete(string codeOffre, string type, string version, string codeIntervenant, bool fromAffaireOnly);

        [OperationContract]
        List<WSDto.GestionIntervenants.IntervenantDto> GetListeInterlocuteursByIntervenant(Int64 codeIntervenant, string name);

        [OperationContract]
        List<WSDto.Offres.Parametres.ParametreDto> GetListeTypesIntervenant();

        [OperationContract]
        List<WSDto.GestionIntervenants.IntervenantDto> EnregistrerDetailsIntervenant(string code, string version, string type, string codeAvenant, WSDto.GestionIntervenants.IntervenantDto toSave, string user);

        [OperationContract]
        WSDto.GestionIntervenants.IntervenantDto GetDetailsIntervenant(Int64 guidId);

        [OperationContract]
        List<WSDto.GestionIntervenants.IntervenantDto> SupprimerIntervenant(string code, string version, string type, string codeAvenant, Int64 guidId, string user);

        #endregion

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void UpdateRelances(IEnumerable<RelanceDto> relances);

        [OperationContract]
        string GetNumAvn(string codeOffre, string version, string type);

        #region Avenants
        [OperationContract]
        bool ExistTraceAvenant(string codeOffre, string version, string type, string perimetre, string codeRisque, string codeObjet, string codeFormule, string codeOption, string etape);

        [OperationContract]
        void SaveTraceAvenant(string codeOffre, string version, string type, string perimetre, string codeRisque, string codeObjet, string codeFormule, string codeOption, string etape,
                                          string champ, string action, string oldValue, string newValue, string obligatoire, string operation, string user);
        [OperationContract]
        void CheckAndSaveTraceAvenant(string codeOffre, string version, string type, string perimetre, string codeRisque, string codeObjet, string codeFormule, string codeOption, string etape,
                                          string champ, string action, string oldValue, string newValue, string obligatoire, string operation, string user);

        #endregion

        #region Gestion des droits utilisateurs

        [OperationContract]
        List<WSDto.GestUtilisateurs.UtilisateurBrancheDto> GetUserRights();

        #endregion

        #region Gestion des UsersLogin

        [OperationContract]
        WSDto.User.UsersDto GetUsersLogin();

        #endregion

        [OperationContract]
        int SetTraceLog(string codeAffaire, string version, string type, int id, string statut, string methode, string date, string diff);

        [OperationContract]
        WSDto.Common.InfosBaseDto LoadInfosBase(string codeAffaire, string version, string type, string codeAvn, string modeNavig);

        [OperationContract]
        WSDto.Common.InfosBaseDto LoadaffaireNouvelleBase(string codeAffaire, string version, string type, string codeAvn, string modeNavig);

        [OperationContract]
        string UpdateCotisationsAS400(AffaireId affaireId, string field, decimal? oldValue, decimal? value);
    }
}
