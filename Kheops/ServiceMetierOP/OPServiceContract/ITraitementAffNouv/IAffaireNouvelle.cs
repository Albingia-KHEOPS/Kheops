using Albingia.Kheops.Common;
using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using OP.WSAS400.DTO;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Attestation;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.Contrats;
using OP.WSAS400.DTO.Engagement;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Partenaire;
using OP.WSAS400.DTO.Regularisation;
using OPWebService;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace OPServiceContract.IAffaireNouvelle
{
    [ServiceContract]
    public interface IAffaireNouvelle
    {
        #region Ecran Creation Affaire Nouvelle

        [OperationContract]
        CreationAffaireNouvelleDto InitCreateAffaireNouvelle(string codeOffre, string version, string type);

        [OperationContract]
        CreationAffaireNouvelleContratDto InitAffaireNouvelleContrat(string codeOffre, string version, string type, string codeAvn, string user, ModeConsultation modeNavig);
        [OperationContract]
        ContratInfoBaseDto InitContratInfoBase(string id, string version, string type, string codeAvn, string user, ModeConsultation modeNavig);

        [OperationContract]
        ContratDto GetContrat(string contratId, string version, string type, string codeAvn, ModeConsultation modeNavig);

        [OperationContract]
        ContratDto GetBasicFolder(Folder folder);

        [OperationContract]
        ContratDto GetContratSansRisques(string contratId, string version, string type, string codeAvn, ModeConsultation modeNavig);

        [OperationContract]
        void UpdatePeriodicite(string codeOffre, string version, string type, string periodicite);
        [OperationContract]
        ModeleInfoPageCommissionDto LoadInfoPageCourtier(string codeContrat, string versionContrat, string type, string codeAvn, bool isReadonly, ModeConsultation modeNavig, string user, string acteGestion);
        [OperationContract]
        List<CourtierDto> GetListCourtiers(string contratId, string version, string type, string codeAvn, ModeConsultation modeNavig);
        [OperationContract]
        CourtierDto GetCourtier(int CodeCourtier, string contratId, string version, string type, ModeConsultation modeNavig);
        [OperationContract]
        void InfoGeneralesContratModifier(ContratDto contrat, string utilisateur, bool isModifHorsAvn);
        [OperationContract]
        string UpdateContrat(ContratInfoBaseDto contrat, string utilisateur, string acteGestion, string user);
        [OperationContract]
        string UpdateCourtier(string codeContrat, string versionContrat, string type, string typeCourtier, int identifiantCourtier, Single partCommission, string typeOperation, string user);
        [OperationContract]
        void SupprimerCourtier(string codeContrat, string versionContrat, int identifiantCourtier);
        [OperationContract]
        void ModifierCommissionCourtier(string codeContrat, string versionContrat, int identifiantCourtier, Single commission);
        [OperationContract]
        CommissionCourtierDto GetCommissionsStandardCourtier(string codeContrat, string versionContrat, string type, string codeAvn, bool isReadonly, ModeConsultation modeNavig, string user, string acteGestion);
        [OperationContract]
        void UpdateCommissionsStandardCourtier(string codeContrat, string versionContrat, string type, CommissionCourtierDto commissionStandard);
        [OperationContract]
        string CreateContrat(string codeOffre, string version, string type, string codeContrat, string versionContrat, string typeContrat, DateTime? dateAccord,
                        DateTime? dateEffet, int heureEffet, string contratRemp, string versionRemp, string souscripteur, string gestionnaire, string branche, string cible, string observation, string user,
                    string acteGestion);
        [OperationContract]
        string VerifContratMere(string codeOffre, int version, string branche, string cible);

        [OperationContract]
        string ControleSousGest(string souscripteur, string gestionnaire);

        [OperationContract]
        string ControleValidateOffer(string codeOffre, string version, string type, string dateAccord, string dateEffet);

        [OperationContract]
        string VerifContratRemp(string codeOffre, int version);
        [OperationContract]
        double GetMontantStatistique(string codeContrat, string version);
        [OperationContract]
        string GetNumeroAliment(string contratMere);
        [OperationContract]
        void UpdateEtatContrat(string codeContrat, long version, string type, string etat);
        [OperationContract]
        void CopyAllInfo(string codeOffre, string version, string type, string codeContrat, string versionContrat, string user, string splitChar, string acteGestion);//, bool isModifHorsAvn);
        [OperationContract]
        IDictionary<(int, string), List<(int, string)>> FindAvailableOptions(SelectionRisquesObjets rsqObjAffNouv, bool exactMatch = false);
        [OperationContract]
        void UpdateTypeRegul(string codeOffre, string version, string type, string newType, string dateDebAvn, string codeAvn, string codeReg);
        #endregion
        #region Ecran Choix Risque/Objet Affaire Nouvelle

        [OperationContract]
        RsqObjAffNouvDto InitRsqObjAffNouv(string codeOffre, string version, string type, string codeContrat, string versionContrat);

        [OperationContract]
        void UpdateRsqObj(string codeContrat, string versionContrat, string type, string codeRsq, string codeObj, string isChecked);

        [OperationContract]
        SelectionRisquesObjets GetOffreSelections(Folder offre, Folder contrat);

        #endregion
        #region Ecran Choix Formule/Volet Affaire Nouvelle

        [OperationContract]
        FormVolAffNouvDto InitFormVolAffNouv(string codeOffre, string version, string type, string codeContrat, string versionContrat);

        [OperationContract]
        void UpdateFormVol(string codeContrat, string versionContrat, string codeOffre, string version, string typeOffre, string codeForm, string guidForm, string codeOpt,
                string guidOpt, string guidVol, string type, string isChecked);

        [OperationContract]
        FormVolAffNouvRecapDto GetListRsqForm(string codeOffre, string version, string type, string codeContrat, string versionContrat, string user, string splitChar, string acteGestion, bool isModifHorsAvn);

        #endregion
        #region Ecran Choix Options tarif

        [OperationContract]
        OptTarAffNouvDto InitOptTarifAffNouv(string codeContrat, string versionContrat);

        [OperationContract]
        void UpdateOptTarif(string codeContrat, string versionContrat, string guidTarif);

        [OperationContract]
        void ValidContrat(string codeOffre, string version, string type, string codeContrat, string versionContrat, string user, string splitChar, bool isModifHorsAvn, string acteGestion);

        #endregion
        #region CoAssureurs

        [OperationContract]
        FormCoAssureurDto InitCoAssureurs(string type, string idOffre, string idAliment, string codeAvn, ModeConsultation modeNavig);

        [OperationContract]
        bool ExistCoAs(string idContrat, string version, string type, ModeConsultation modeNavig);

        [OperationContract]
        CoAssureurDto GetCoAssureurDetail(string type, string idOffre, string idAliment, string idCoAssureur, ModeConsultation modeNavig, bool modeCoAss);

        [OperationContract]
        string EnregistrerListeCoAssureurs(string code, string version, string type, string typeAvenant, string avenant, List<CoAssureurDto> listeCoass, string user);

        [OperationContract]
        string EnregistrerCoAssureur(string type, string idOffre, string idAliment, CoAssureurDto coAssureur, string typeOperation, string user);

        [OperationContract]
        string SupprimerCoAssureur(string type, string idOffre, string idAliment, string guidId);

        #endregion
        #region retours signatures

        [OperationContract]
        List<ParametreDto> GetListeTypesAccord();

        //[OperationContract]
        //RetourPreneurDto GetRetourPreneur(string codeContrat, string versionContrat, string typeContrat, ModeConsultation modeNavig);

        [OperationContract]
        List<RetourCoassureurDto> GetRetoursCoassureurs(string codeContrat, string versionContrat, string typeContrat);

        [OperationContract]
        RetourPreneurDto GetRetourPreneur2(string codeContrat, string version, string type, string codeAvt, ModeConsultation modeNavig);

        [OperationContract]
        List<RetourCoassureurDto> GetRetoursCoassureurs2(string codeContrat, string version, string type, string codeAvt, ModeConsultation modeNavig);

        [OperationContract]
        void EnregistrerRetours(string codeContrat, string versionContrat, string typeContrat, string codeAvt, RetourPreneurDto retourPreneur, List<RetourCoassureurDto> retoursCoAssureurs, ModeConsultation modeNavig, string user, bool isModifHorsAvn);

        #endregion

        #region Template
        [OperationContract]
        ContratInfoBaseDto GetInfoTemplate(string idTemp);
        #endregion

        #region Blocage termes
        [OperationContract]
        List<ParametreDto> GetListeZonesStop();

        [OperationContract]
        void SaveZoneStop(string codeContrat, string versionContrat, string typeContrat, string zoneStop, string user);

        [OperationContract]
        DeblocageTermeDto GetEcheanceEmission(string codeContrat, string versionContrat, string typeContrat, string mode, string user, string acteGestion, AlbConstantesMetiers.DroitBlocageTerme niveauDroit, bool emission);
        #endregion
        [OperationContract]
        void RemoveControlAssiette(string codeContrat, string versionContrat, string typeContrat);
        [OperationContract]
        bool CheckControlAssiette(string codeContrat, string versionContrat, string typeContrat);
        [OperationContract]
        string ChangePreavisResil(string codeContrat, string version, string codeAvn, string dateEffet, string dateFinEffet, string dateAvenant, string periodicite, string echeancePrincipale, string splitCharHtml, string user, string acteGestion);
        [OperationContract]
        string ControleEcheance(string prochaineEcheance, string periodicite, string echeancePrincipale);
        [OperationContract]
        bool ContratHasQuittances(string codeOffre, string version, string type);

        #region Ecran Création Avenant
        [OperationContract]
        ContratDto GetInfoRegulPage(string codeOffre, string version, string type, string codeAvn);
        [OperationContract]
        AvenantInfoPageDto GetInfoAvenantPage(string codeOffre, string version, string type, string codeAvn, string typeAvt, string modeAvt, string user, string modeNavig);
        [OperationContract]
        AvenantDto GetInfoAvenant(string codeOffre, string version, string type, short numAvn, string codeAvn, string typeAvt, string modeAvt, string user, string modeNavig);
        [OperationContract]
        AvenantModificationDto GetInfoAvenantModification(string codeOffre, string version, string type, short numAvn, string typeAvt, string modeAvt, string modeNavig);
        [OperationContract]
        List<ParametreDto> ReloadAvenantResilMotif();

        #endregion

        #region Ecran Création Attestation

        [OperationContract]
        AttestationDto GetInfoAttestation(string codeContrat, string version, string type, string user);

        [OperationContract]
        string ChangeExercicePeriode(string codeContrat, string version, string type, int exercice, DateTime? periodeDeb, DateTime? periodeFin);

        [OperationContract]
        AttestationSelRsqDto OpenTabRsqObj(string codeContrat, string version, string type, string codeAvn,
            string lotId, string exercice, DateTime? periodeDeb, DateTime? periodeFin, string typeAttes, bool integralite, string user);

        [OperationContract]
        AttestationSelGarDto OpenTabGarantie(string codeContrat, string version, string type, string codeAvn,
            string lotId, string exercice, DateTime? periodeDeb, DateTime? periodeFin, string typeAttes, bool integralite, string user);

        [OperationContract]
        AttestationSelGarDto LoadAttestationGarantie(string codeContrat, string version, string type, string lotId);

        [OperationContract]
        string ValidSelectRsqObj(string codeContrat, string version, string type, string lotId, string selRsqObj, string user);

        [OperationContract]
        string ValidSelectionGar(string codeContrat, string version, string type, string lotId, string selGarantie, string user);

        [OperationContract]
        void SaveNewAffair(Folder offre, Folder contrat, string user);

        [OperationContract]
        string ValidPeriodeAttestation(string codeContrat, string version, string type, string lotId, string exercice, DateTime? periodeDeb, DateTime? periodeFin,
            string typeAttes, bool integralite, string user);

        #endregion

        #region Ecran Création Régularisation

        [OperationContract]
        RegularisationDto GetInfoRegularisation(string codeContrat, string version, string type, string codeAvn, string user);

        [OperationContract]
        RegularisationInfoDto GetInfoAvenantRegule(string codeContrat, string version, string type, string typeAvt, string modeAvt, string user, string lotId, string reguleId, string regulMode);

        [OperationContract]
        RegularisationInfoDto GetInfoAvnRegule(string codeContrat, string version, string type, string codeAvn, string modeAvt, int exercice, DateTime? periodeDeb, DateTime? periodeFin);

        [OperationContract]
        RegularisationInfoDto GetAvnRegule(string codeContrat, string version, string type, string codeAvn, string typeAvt, int exercice, DateTime? periodeDeb, DateTime? periodeFin, string user, string lotId, string reguleId, string regulMode, string deleteMod, string cancelMod, bool resetLot = false);
        [OperationContract]
        void DeleteReguleP(string reguleId);

        [OperationContract]
        RegularisationRsqDto GetListRsqRegule(string lotId, string reguleId, string mode);

        //[OperationContract]
        //RegularisationRsqDto CreateAvenantRegul(string lotId, string reguleId, string codeContrat, string version, string type, string typeAvt, string codeAvn, string exercice, string dateDeb, string dateFin,
        //    string codeICT, string codeICC, string tauxCom, string tauxComCATNAT, string codeEnc, string user, string mode, string gestionnaire, string souscripteur, AvenantRegularisationDto modeleAvtRegulstring ,string regulMode, string regulType, string regulNiveau, string regulAvn);

        [OperationContract]
        List<RisqueDto> ReloadListRsqRegule(string lotId, string reguleId, bool isReadonly);

        [OperationContract]
        RegularisationGarDto GetListGarRegule(string lotId, string reguleId, string codeRsq, bool isReadonly);

        [OperationContract]
        RisqueDto GetAppliqueRegule(string codeContrat, string version, string type, string codeAvn, string codeFor);
        [OperationContract]
        string MouvementsGarPeriodeAS400(string codeContrat, string version, string type, string rsq, string codfor, string garan, string idlot, string idregul);

        [OperationContract]
        RegularisationGarInfoDto GetInfoRegularisationGarantie(RegularisationContext context);

        [OperationContract]
        List<RegulMatriceDto> GetRegulMatrice(string codeAffaire, int version, string type, string lotId, long rgId = 0);

        [OperationContract]
        bool IsValidRegul(string reguleId);



        [OperationContract]
        AjoutMouvtGarantieDto AjouterMouvtPeriod(string codeContrat, string version, string type, string codersq, string codfor, string codegar, string idregul, string idlot, int datedeb, int datefin);
        [OperationContract]
        List<LigneRegularisationDto> GetListeRegularisation(string codeContrat, string version, string type);
        [OperationContract]
        List<LigneRegularisationDto> DeleteRegule(string codeContrat, string version, string type, string codeAvn, string reguleId);

        [OperationContract]
        List<LigneMouvtGarantieDto> ReloadMouvtPeriod(string codeAffaire, string version, string type, string codeRsq, string codeFor, string codeGar, string codeRegul);
        [OperationContract]
        List<LigneMouvtGarantieDto> GetListDatesPeriod(string codeContrat, string version, string type, string reguleId, string codersq, string codfor, string codegar);
        [OperationContract]
        string CheckDatesPeriodAllRsqIntegrity(string codeContrat, string version, string type, string idLot, string typeAvt, string dateDebReg, string dateFinReg, string reguleId);
        [OperationContract]
        SaisieInfoRegulPeriodDto InitSaisieGarRegul(string idRegulGar, string codeAvenant);

        [OperationContract]
        SaisieInfoRegulPeriodDto ReloadSaisieGarRegul(string codeContrat, string version, string type, string codeAvenant, string idregulgar, string codeRsq, string assiettePrev, string valeurPrev, string unitePrev, string codetaxePrev, string assiettedef, string valeurdef, string uniteDef, string codetaxeDef, string cotisForceHT, string cotisForceTaxe, string coeff);

        [OperationContract]
        string ValidSaisiePeriodRegule(string codeContrat, string version, string type, string codeAvn, string codeRsq, string reguleGarId, string typeRegule, SaisieGarInfoDto modelDto);
        [OperationContract]
        ConfirmSaisieReguleDto GetPopupConfirm(string reguleGarId);

        [OperationContract]
        LigneRegularisationDto GetLastValidInfoAvnRegul(string codeContrat, string version, string type);


        #endregion

        #region Ecran Infos Générales Avenant

        [OperationContract]
        AvenantInfoDto GetAvenant(string codeOffre, string version, string type, string codeAvenant, ModeConsultation modeNavig);
        [OperationContract]
        void SupprimerEcheances(string codeOffre, string version, string type);
        [OperationContract]
        void EnregistrerAvenant(ContratDto avenant, string user, bool isModifHorsAvn);

        #endregion

        #region ParamCibleRecup
        [OperationContract]
        OffreRecupDto RecupOffre(string codeOffre, string version);

        [OperationContract]
        bool MigrationOffre(string codeOffre, string version, string type, string fromCible, string toCible);

        #endregion

        [OperationContract]
        IDictionary<Folder, (DateTime? debut, DateTime? fin)> GetDatesEffets(IEnumerable<Folder> folders);

        [OperationContract]
        void CorrectionECM(string codeContrat, string versionContrat, string splitChar, string user, string acteGestion, bool isModifHorsAvn);

        [OperationContract]
        IEnumerable<ContratConnexeDto> GetAllConnexites(Folder folder);

        [OperationContract]
        IEnumerable<ContratConnexeDto> GetConnexites(Folder folder, TypeConnexite type);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void AddPeriodeEngagement(PeriodeConnexiteDto periode);

        [OperationContract]
        IEnumerable<PeriodeConnexiteDto> GetPeriodesEngagements(int numeroConnexite);

        [OperationContract]
        (string pbbra, string pbsbr, string pbcat, string kaacible) GetCiblage(Folder folder, ModeConsultation modeConsultation);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void AddConnexite(Folder folder, ContratConnexeDto contratConnexe, string user);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void RemoveConnexite(Folder folder, ContratConnexeDto contratConnexe);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void ModifyPeriodesEngagements(Folder folder, IEnumerable<PeriodeConnexiteDto> periodes);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void MergeConnexites(FusionConnexitesDto fusionDto);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void PickTargetToConnexites(FusionConnexitesDto fusionDto);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void MoveSourceInConnexites(FusionConnexitesDto fusionDto);

        [OperationContract]
        ContractJsonDto CreationContractsKheops(ContractJsonDto contract, string user);

        [OperationContract]
        ContractJsonDto CreationOffersKheops(ContractJsonDto offer, string user);


        #region Trace contrat
        [OperationContract]
        bool HaveTraceOfEndEffectDate(string contratId, string version, string type, string numAvn);
        [OperationContract]
        void TraceContratInfoModifHorsAvn(string contratId, string version, string type, string numAvn, string user, string codeRisque = null, bool regulTrace = false);

        [OperationContract]
        ContratDto GetEndEffectDate(string contratId, string version, string type);

        [OperationContract]
        void TraceResiliation(string contratId, string version, string type, string numAvn, string user, ResiliationTraceType TraceType);
        #endregion
        #region Gestion partenaires
        /// <summary>
        /// Obtenir la liste des partenaires de base :
        /// Courtier gestionnaire
        /// Interlocuteur
        /// Courtier apporteur
        /// Courtier payeur
        /// Preneur d'assurance
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="version">Version</param>
        /// <param name="type">Type</param>
        /// <param name="codeAvn">Code AVN</param>
        /// <returns></returns>
        [OperationContract]
        PartenairesBaseDto GetListPartenairesInfosBase(string code, string version, string type, string codeAvn);
        /// <summary>
        /// Obtenir tous les partenaires :
        /// Partenaires de base (courtiers ,interlocuteur et preneur d'assurance)
        /// Assurés additionnels
        /// Co-assureurs
        /// Intervenants
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="version">Version</param>
        /// <param name="type">Type</param>
        /// <param name="codeAvn">Code AVN</param>
        /// <param name="modeNavig">mode Navig</param>
        /// <returns></returns>
        [OperationContract]
        PartenairesDto GetListPartenaires(string code, string version, string type, string codeAvn, ModeConsultation modeNavig);
        /// <summary>
        /// Obtenir la liste des assurés additionnels
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="version">Version</param>
        /// <param name="type">Type</param>
        /// <param name="codeAvn">Code AVN</param>
        /// <param name="modeNavig">mode Navig</param>
        /// <returns></returns>
        [OperationContract]
        List<PartenaireDto> GetListAssuresAdditionnelsInfosBase(string code, string version, string type, string codeAvn, ModeConsultation modeNavig);
        #endregion
    }
}