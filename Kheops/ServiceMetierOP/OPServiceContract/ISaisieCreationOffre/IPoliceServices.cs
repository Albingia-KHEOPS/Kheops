using System.Collections.Generic;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Aperiteur;
using OP.WSAS400.DTO.Offres.Assures;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OP.WSAS400.DTO.Offres.Indice;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Personnes;
using System.ServiceModel;
using ALBINGIA.Framework.Common.Constants;
using OP.WSAS400.DTO.Historique;
using System;
using OP.WSAS400.DTO.Ecran.DetailsRisque;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.LTA;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.AffaireNouvelle;

namespace OPServiceContract.ISaisieCreationOffre
{
    [ServiceContract]
    public interface IPoliceServices
    {
        [OperationContract]
        LTADto GetInfoLTA(string codeAffaire, int version, string type, int avenant, ModeConsultation modeNavig);
        [OperationContract]
        void SetInfoLTA(string codeAffaire, int version, string type, int avenant, LTADto dto);
        [OperationContract]
        BonificationsDto BonificationGet(string id, int version);
        [OperationContract]
        IndiceGetResultDto IndiceGet(IndiceGetQueryDto query);
        [OperationContract]
        CabinetCourtageGetResultDto CabinetCourtageGet(CabinetCourtageGetQueryDto query, bool modeAutocomplete);
        [OperationContract]
        CabinetCourtageDto ObtenirCabinetCourtageComplet(int code, int codeInterlocuteur);
        [OperationContract]
        InterlocuteurGetResultDto InterlocuteursGet(InterlocuteurGetQueryDto query);
        [OperationContract]
        InterlocuteurGetResultDto InterlocuteursAperiteurGet(string queryNomInterlocuteur, string queryCodeAperiteur);
        [OperationContract]
        AssureGetResultDto AssuresGet(AssureGetQueryDto query, bool modeAutocomplete);
        [OperationContract]
        List<AssurePlatDto> RechercheTransversePreneurAssurance(string codePreneur, string nomPreneur, string cpPreneur);
        [OperationContract]
        AssureDto ObtenirAssureComplet(int code);
        [OperationContract]
        GestionnairesGetResultDto GestionnairesGet(GestionnairesGetQueryDto query);
        [OperationContract]
        SouscripteursGetResultDto SouscripteursGet(SouscripteursGetQueryDto query);
        [OperationContract]
        List<UtilisateurDto> UtilisateursGet(string name);
        [OperationContract]
        AperiteurGetResultDto AperiteursGet(AperiteurGetQueryDto query);
        [OperationContract]
        AperiteurGetResultDto AperiteursGetByCodeNum(Int64 codeNum);
        [OperationContract]
        OffreDto OffreGetDto(string id, int version, string type, ModeConsultation modeNavig);
        [OperationContract]
        bool TestExistanceOffre(string offreId);

        [OperationContract]
        List<ParametreDto> ObtenirParametres(string codeOffre, string version, string type, string codeAvn, string contexte, string famille, ModeConsultation modeNavig);
        [OperationContract]
        List<BrancheDto> BranchesGet();
        [OperationContract]
        List<ParametreDto> RegimeTaxeGet(string branche, string cible);
        [OperationContract]
        List<ParametreDto> ObtenirMotClef(string branche, string cible);
        [OperationContract]
        List<ParametreDto> IndicesReferenceGet();

        [OperationContract]
        string SauvegardeOffre(OffreDto offre, string user);

        [OperationContract]
        List<ParametreDto> GetCibles(string codeBranche, bool loadAllIfNull, bool isAdmin, bool isUserHorse);

        [OperationContract]
        bool VerifyTraceOffre(string codeOffre, string version, string type, string etape, string perimetre);

        [OperationContract]
        void RefusOffre(string codeOffre, string version, string codeMotif);
        [OperationContract]
        string ValiderOffre(string codeOffre, string version, string type, string avenant, string acteGestion, string validable, string complet, string motif, string mode, string lotsId, string user, string reguleId, bool isModifHorsAvn);
        [OperationContract]
        string EditerDocParLot(string codeOffre, string version, string type, string avenant, string mode, string lotsId, string user, bool isReadOnly, string acteGestion, string attesId, string machineName, int switchModuleGestDoc);
        [OperationContract]
        DelegationDto ObtenirDelegation(int codeCabinetCourtage);
        [OperationContract]
        bool OffreEstValide(string numeroOffre, string version, string type, string numAvn);

        [OperationContract]
        List<RisqueDto> ObtenirRisques(ModeConsultation modeNavig, string offreId, int? offreVersion, string type, string codeAvn);

        [OperationContract]
        List<RisqueDto> ObtenirIDRisques(string offreId, int? offreVersion);
        [OperationContract]
        List<RisqueDto> ObtenirInfosRisquesInventaire(ModeConsultation modeNavig, string offreId, int? offreVersion, string type, string codeAvn);

        [OperationContract]
        DetailsRisqueGetResultDto GetInfoDetailRsq(string codeOffre, string version, string type, string numRsq, string numObj, ModeConsultation modeNavig, string codeAvn, bool isAdmin, string codeBranche, string codeCible, bool isUserHorse);

        [OperationContract]
        OffreDto GetInfoCreationSaisie(string codeOffre, string version, string type);

        /// <summary>
        /// Copie des informations de l'offre simplifiée
        /// vers les tables
        /// </summary>
        [OperationContract]
        string ConvertSimpleFolderToStd(string codeOffre, string version, string type, string branche, string cible, string user);

        /// <summary>
        /// Supprime une offre
        /// </summary>
        [OperationContract]
        string DeleteOffre(string codeOffre, string version, string type);

        [OperationContract]
        string GetOffreLastVersion(string codeOffre, string version, string type, string user);

        [OperationContract]
        bool IsEnteteContainAddress(string codeOffre, string version, string type, string codeAvn);

        #region Historique contrat

        [OperationContract]
        HistoriqueDto GetListHistorique(string codeAffaire, string version, string type, bool contractuel);

        #endregion

        #region Modif Hors Avn

        [OperationContract]
        bool GetModifHorsAvnIsRegularisable(string codeOffre, int version, string type, int numAvn, string codeRsq);

        #endregion


        #region Formule de Garantie (refacto)

        //[OperationContract]
        //void InitFormuleGarantie(string codeAffaire, string version, string type, string codeAvn, string codeFor, string codeOpt, string modeNavig);

        #endregion

        #region Gestion Adresse

        [OperationContract]
        AdressePlatDto ObtenirAdresse(string codeOffre, string version, string type);
        #endregion
        #region Gestion risque
        [OperationContract]
        void UpdateRiskTotalValue(string code, int? version, int riskId, long? totalValue, string unite, string riskType, string valueHt);
        [OperationContract]
        RisqueDto ObtenirRisque(ModeConsultation modeNavig, string codeOffre, int numRsq, int? version = null, string type = "O", string codeAvn = "");
        [OperationContract]
        int CopierRisque(string codeOffre, int numRsq,string CBnsPb,  string user, int? version = null, string type = "O");
        #endregion

        [OperationContract]
        void ClasserContratsSansSuite(string codeAffaire, int version, string type, string listeAnnulQuitt, string user);

        [OperationContract]
        List<CreationAffaireNouvelleContratDto> LstContrats(string codeOffre, string version);
        //List<ContratDto> LstContrats(string codeOffre, string version);

        [OperationContract]
        void UpdateOrInsertObservation(string codeOffre, string type, int version, string obsvInfoGen, string obsvCotisation, string obsvEngagement, string obsvMntRef, string obsvRefGest);
    }
}
