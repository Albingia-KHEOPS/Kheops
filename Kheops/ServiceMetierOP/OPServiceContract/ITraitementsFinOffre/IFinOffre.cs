using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Models.FileModel;
using OP.WSAS400.DTO.AttentatGareat;
using OP.WSAS400.DTO.ControleFin;
using OP.WSAS400.DTO.Cotisations;
using OP.WSAS400.DTO.DocumentGestion;
using OP.WSAS400.DTO.DocumentsJoints;
using OP.WSAS400.DTO.Engagement;
using OP.WSAS400.DTO.FinOffre;
using OP.WSAS400.DTO.MontantReference;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Quittance;
using OP.WSAS400.DTO.SMP;
using OP.WSAS400.DTO.SuiviDocuments;
using OP.WSAS400.DTO.SyntheseDocuments;
using OP.WSAS400.DTO.Traite;
using OP.WSAS400.DTO.Validation;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace OPServiceContract.ITraitementsFinOffre
{
    [ServiceContract]
    public interface IFinOffre
    {
        #region Engagement

        [OperationContract]
        EngagementDto InitEngagement(string codeOffre, string version, string type, string codeAvn, string codePeriode, ModeConsultation modeNavig, bool isReadonly, bool enregistrementEncoursOnly, string user, string acteGestion, string accessMode, string screen);

        [OperationContract]
        void UpdateEngagement(string codeOffre, string version, string type, EngagementDto engagement, string field, string user, string acteGestion, string codePeriode);

        [OperationContract]
        void SaveEngagementCommentaire(string codeOffre, string version, string type, string commentaire, string codePeriode);

        [OperationContract]
        void SavePeriodeCnx(PeriodeConnexiteDto dto, string mode);

        #endregion

        #region Traite

        [OperationContract]
        TraiteDto InitTraite(string codeOffre, string version, string type, string codeAvn, string traite, ModeConsultation modeNavig,
            bool isReadonly, string user, string acteGestion, string codePeriode, string accesMode);

        [OperationContract]
        void UpdateTraite(string codeOffre, string version, string type, TraiteDto traiteDto, string user, string codePeriode);


        [OperationContract]
        List<SMPTaiteDto> GetSmpT(int Id);

        [OperationContract]
        void SaveSmpT(int SmpCptF, int id);
        

        #endregion

        #region Attentat/Gareat

        [OperationContract]
        AttentatGareatDto InitAttentat(string codeOffre, string version, string type, string codeAvn, bool isReadonly, ModeConsultation modeNavig, string user, string acteGestion);

        [OperationContract]
        void UpdateAttentat(string codeOffre, string version, string type, string field, string value, string commentForce, string user, ModeConsultation modeNavig, string acteGestion);

        [OperationContract]
        void UpdateAttentatComment(string codeOffre, string version, string type, string commentForce);

        #endregion

        #region SMP

        [OperationContract]
        SMPdto ObtenirDetailSMP(string argCodeOffre, string argVersion, string argType, string codeAvn, string risque, string ventilation, ModeConsultation modeNavig);

        [OperationContract]
        SMPdto RecalculSMP(SMPdto argQuery, string argCodeOffre, string argVersion, string argType, string codeAvn, string risque, string ventilation, ModeConsultation modeNavig);

        #endregion

        #region Cotisations

        [OperationContract]
        CotisationsDto InitCotisations(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, bool isChecked, bool reload, string user, string acteGestion, bool isreadonly);

        [OperationContract]
        void UpdateCotisations(string codeOffre, string version, string type, CotisationsDto cotisationsDto, string field, string value, string oldvalue, string user, ModeConsultation modeNavig, string codeAvn, string acteGestion);

        [OperationContract]
        void SaveCommentaireCotisation(string codeOffre, string version, string type, string commentaire);

        [OperationContract]
        CotisationsInfoTarifDto LoadCotisationsTarif(string lienKpgaran);

        //[OperationContract]
        //List<CotisationGarantieDto> GetCotisationGaranties(string codeOffre, string version, string type, bool onlyGarPorteuse);

        [OperationContract]
        CotisationsDto GetCotisationsGaranties(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, bool onlyGarPorteuse, string typePart);

        #endregion

        #region Fin Offre

        [OperationContract]
        FinOffreDto InitFinOffre(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig);

        [OperationContract]
        void UpdateFinOffre(string codeOffre, string version, string type, FinOffreDto finOffreDto, string user);

        #endregion

        #region Controle Fin

        [OperationContract]
        ControleFinDto InitControleFin(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig);

        [OperationContract]
        void Alimentation(string codeOffre, string version, string type, string user, ModeConsultation modeNavig, bool isModifHorsAvn, bool isAvenant, string regulId, int codeAssure);
        [OperationContract]
        void UpdateEtatRegul(string codeOffre, string version, string type, string regulId);

        #endregion

        #region Validation Offre

        [OperationContract]
        ValidationDto InitValidationOffre(string codeOffre, string version, string type, string codeAvn, bool isReadonly, ModeConsultation modeNavig, string user, string acteGestion, string reguleId, string reguleMode);

        [OperationContract]
        void SaveEtatMotif(string codeOffre, string version, string type, string etat, string motif, string acteGestion, string regulId);

        [OperationContract]
        ValidationEditionDto GetValidationEdition(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string acteGestion, string modeEcran, bool isBNS);

        [OperationContract]
        string CheckIsDocEdit(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string acteGestion);

        [OperationContract]
        bool CheckValidateOffre(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string acteGestion);

        [OperationContract]
        string VerifCourtier(string codeOffre, string version, string type);

        #endregion

        #region Gestion Document

        [OperationContract]
        List<DocumentGestionDocDto> InitDocumentsGestion(string codeOffre, string version, string type, string codeAvt, string acteGestion, string typeAvenant, string user, ModeConsultation modeNavig, bool isReadOnly, bool init, bool isValidation, long[] docsId, bool firstLoad, string attesId, string regulId);
        [OperationContract]
        List<DocumentGestionDocDto> GetListeDocumentsEditables(string codeOffre, string version, string type, ModeConsultation modeNavig);
        [OperationContract]
        DocumentGestionInfoDestDto ShowInfoDest(string idDest, string typeDest);
        [OperationContract]
        void ValidSupprDoc(string selectDoc, string unselectDoc);
        [OperationContract]
        List<SyntheseDocumentsDocDto> InitSyntheseDocument(string codeOffre, string version, string type);
        [OperationContract]
        void SaveTraceArbreFinAffnouv(string codeOffre, string version, string type, string user);

        [OperationContract]
        void ChangeSituationDoc(string idDoc, string situation);

        [OperationContract]
        void RegenerateDocLibre(string codeOffre, string version, string type, string idsDoc, string user);

        [OperationContract]
        void OuvrirGestionDocument(string codeAffaire, int version, string type, string user, string wrkStation, string ipAdress);
        [OperationContract]
        string OpenGED(string codeAffaire, int version, string type, string userName, string ip, string machineName);
        #endregion

        #region Documents Gestion Détails

        [OperationContract]
        List<ParametreDto> GetListeDocumentsDispo();

        [OperationContract]
        List<ParametreDto> GetListeTypesDestinataire();

        [OperationContract]
        List<ParametreDto> GetListeTypesEnvoi();

        [OperationContract]
        List<ParametreDto> GetListeTampons();

        [OperationContract]
        List<CourrierTypeDto> GetListeCourriersType(string filtre, string typeDoc);

        [OperationContract]
        List<DestinataireDto> GetListeDestinatairesDetails(string codeDocument);

        [OperationContract]
        List<DestinataireDto> GetListeCourtiers(string code, string type, string version, string codeDocument);

        [OperationContract]
        List<DestinataireDto> GetListeAssures(string code, string type, string version, string codeDocument);

        [OperationContract]
        List<DestinataireDto> GetListeCompagnies(string code, string type, string version, string codeDocument);

        [OperationContract]
        List<DestinataireDto> GetListeIntervenants(string code, string type, string version, string codeDocument, string typeIntervenant);

        [OperationContract]
        DocumentGestionDetailsInfoGen GetInfoComplementairesDetailsDocumentGestion(string codeDocument);

        [OperationContract]
        List<DestinataireDto> SaveLigneDestinataireDetails(string code, string version, string type, string user, string lotId, string typeDoc, string courrierType, string codeDocument, DestinataireDto destinataire, string acteGestion);

        [OperationContract]
        List<DestinataireDto> DeleteLigneDestinataireDetails(string codeDocument, string guidIdLigne);

        [OperationContract]
        void SaveInformationsComplementairesDetailsDocument(string code, string type, string version, Int64 codeDocument, string document, Int64 courrierType, int nbExSupp, string user);

        [OperationContract]
        void DeleteLotDocumentsGestion(Int64 codeLot);

        #endregion

        #region Quittance
        [OperationContract]
        List<QuittanceDto> GetQuittances(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string modeAffichage, string numQuittanceVisu, bool launchPGM, bool isModeCalculForce, string user, string acteGestion, string idRegule, bool isreadonly, string isFGACocheIHM);
        [OperationContract]
        QuittanceDetailDto GetQuittanceDetail(string codeOffre, string version, string codeAvn, ModeConsultation modeNavig, string acteGestion, string modeAffichage, string numQuittanceVisu);
        [OperationContract]
        InfoCompQuittanceDto GetInfoComplementairesQuittance(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string acteGestion, bool isReadonly, string user, bool isValidQuitt);
        [OperationContract]
        void GestionTraceAvt(string codeOffre, string version, string type, bool isChecked, string user, string acteGestion);
        [OperationContract]
        string GetCodeAvnQuittance(string codeOffre, string version, string numQuittance);
        [OperationContract]
        void CalculAvenant(string codeContrat, string version, string codeAvn, string user, string acteGestion, string isFGACocheIHM, decimal fraisAccessoire = 0, bool updateaccavn = false);
        #endregion
        #region Quittance - Frais accessoires
        [OperationContract]
        FraisAccessoiresDto InitFraisAccessoire(string codeOffre, string versionOffre, string typeOffre, string codeAvn, int anneeEffet, bool isReadonly, ModeConsultation modeNavig, string user, string acteGestion, string reguleId,bool isModifHorsAvn);
        [OperationContract]
        List<ParametreDto> GetListeTypesAccessoire();
        [OperationContract]
        string UpdateFraisAccessoires(string codeOffre, string version, string type, int effetAnnee, string typeFrais, int fraisRetenus,
            bool taxeAttentat/*, int fraisSpecifiques*/, long codeCommentaires, string commentaires, string codeAvn, string user, string acteGestion,bool isModifHorsAvn);
        [OperationContract]
        void SaveCommentQuittance(string codeOffre, string version, string type, string codeAvn, string comment, string acteGestion, string reguleId, string modifPeriod, string dateDeb, string dateFin);
        [OperationContract]
        string GetCommentQuittance(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig);
        [OperationContract]
        void UpdateFraisAccessoiresAvn(string codeOffre, string version, string type, string codeAvn, string acteGestion, string reguleId, string user, int fraisforce, bool taxeAttentat);
        #endregion

        #region Quittance - Ventilation détaillée

        [OperationContract]
        List<QuittanceVentilationDetailleeGarantieDto> GetQuittanceVentilationDetailleeGaranties(string codeOffre, string version, string type, ModeConsultation modeNavig, string modeAffichage, string numQuittanceVisu, string acteGestion);

        [OperationContract]
        List<QuittanceVentilationDetailleeTaxeDto> GetQuittanceVentilationDetailleeTaxes(string codeOffre, string version, string type, ModeConsultation modeNavig, string modeAffichage, string numQuittanceVisu, string acteGestion);

        #endregion

        #region Quittance - Ventilation des commissions

        [OperationContract]
        List<QuittanceVentilationCommissionCourtierDto> GetQuittanceVentilationCommissionCourtiers(string codeOffre, string version, string type, ModeConsultation modeNavig, string modeAffichage, string numQuittanceVisu, string acteGestion);

        #endregion

        #region Quittance - Tab Part Albingia

        [OperationContract]
        QuittanceVentilationAperitionDto GetQuittancePartAlbingia(string codeOffre, string version, string type, string codeAvn, string modeNavig, string acteGestion, string modeAffichage, string numQuittanceVisu);

        [OperationContract]
        List<QuittanceTabAperitionLigneDto> GetQuittanceVentilationCoassureurs(string codeOffre, string version, string type, string modeAffichage, string numQuittanceVisu, string acteGestion);

        [OperationContract]
        List<QuittanceTabAperitionLigneDto> GetQuittanceVentilationCoassureursParGarantie(string codeOffre, string version, string type, string codeCoass, string modeAffichage, string numQuittanceVisu, string acteGestion);

        #endregion

        #region Quittance - Visualisation

        [OperationContract]
        List<QuittanceVisualisationLigneDto> GetListeVisualisationQuittances(string codeOffre, string version, DateTime? dateEmission, string typeOperation, string situation, DateTime? datePeriodeDebut, DateTime? datePeriodeFin, AlbConstantesMetiers.TypeQuittances typeQuittances, string colTri);
        [OperationContract]
        List<ParametreDto> GetListeTypesOperation();

        [OperationContract]
        string LancerBulletinAvis(string codeOffre, string version, string type, string avenant, string numQuittanceVisu, string nbExemplaire, string typeCopie, bool isAvisEcheance, string user);

        #endregion
        #region Quittance - Annulation quittances

        [OperationContract]
        QuittanceVisualisationDto GetListeQuittancesAnnulation(bool init, string codeOffre, string version, int avenant, DateTime dateEffetAvenant, DateTime? dateEmission, string typeOperation, string situation, DateTime? datePeriodeDebut, DateTime? datePeriodeFin, AlbConstantesMetiers.TypeQuittances typeQuittances, string user, string acteGestion, bool isreadonly, ModeConsultation modeNavig, string colTri);
        //List<QuittanceVisualisationLigneDto> GetListeQuittancesAnnulation(bool init, string codeOffre, string version, int avenant, DateTime dateEffetAvenant, DateTime? dateEmission, string typeOperation, string situation, DateTime? datePeriodeDebut, DateTime? datePeriodeFin, AlbConstantesMetiers.TypeQuittances typeQuittances, string user, string acteGestion, bool isreadonly, ModeConsultation modeNavig);

        [OperationContract]
        void EnregistrerQuittancesAnnulees(string codeOffre, string version, string listeQuittancesAnnulees);

        #endregion

        #region Quittance - Calcul Forcé

        [OperationContract]
        QuittanceForceDto LoadCalculWindow(string codeOffre, string version, string avenant, string typeVal, string typeHTTTC, ModeConsultation modeNavig, string acteGestion);
        [OperationContract]
        string ExistMntCalcul(string codeOffre, string version, string avenant, ModeConsultation modeNavig, string acteGestion);
        [OperationContract]
        string UpdateCalculForce(string codeOffre, string type, string version, string avenant, string typeVal, string typeHTTTC,
            string codeRsq, string codeFor, string montantForce, string user, string acteGestion, string reguleId);

        [OperationContract]
        QuittanceForceGarantieDto LoadGaranInfo(string codeOffre, string version, string avenant, string codeFor, ModeConsultation modeNavig, string acteGestion);

        [OperationContract]
        QuittanceForceGarantieDto UpdateGaranForce(string codeOffre, string version, string avenant, string formId, string codeFor, string codeRsq, string codeGaran, string montantForce, string catnatForce, string codeTaxeForce, ModeConsultation modeNavig, string acteGestion);

        [OperationContract]
        string ValidFormGaranForce(string codeOffre, string type, string version, string avenant, string codeFor, string codeRsq, string user, string acteGestion);

        #endregion

        #region Périodes d'engagement
        [OperationContract]
        List<EngagementPeriodeDto> GetEngagementPeriodes(string codeOffre, string version, string type, string codeAvn, string user, ModeConsultation modeNavig);

        [OperationContract]
        List<ModeleDetailsConnexitesEngPeriodeDto> GetEngagementPeriodesDetails(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig);
        [OperationContract]
        string SaveEngagementPeriode(string codeOffre, string version, string type, string codeAvn, string user, EngagementPeriodeDto engagementPeriodeDto, string typeOperation, ModeConsultation modeNavig, bool updateTable);
       
        [OperationContract]
        string DeleteEngagementPeriode(string code);
        [OperationContract]
        void SaveArbreEngagementPeriode(string codeOffre, string version, string type, string user);
        [OperationContract]
        string GetDateControle(string branche, string cible);
        #endregion

        #region connexité
        [OperationContract]
        void SaveObsvConnexite(string codeAffaire, string version, string type, int codeObservation, string observation, string acteGestion, string reguleId);
        [OperationContract]
        string GetNumeroConnexite(string codeOffre, string version, string type, string codeTypeConnexite);
        [OperationContract]
        InfoContratConnexeDto GetInfoContratsConnexes(string codeOffre, string version, string type, string codeTypeConnexite);
        [OperationContract]
        InfoConfirmationConnexeDto GetInfoConfirmationAction(string codeOffreActuel, string versionActuel, string typeActuel, string codeTypeConnexite, string numConnexite,
                                    string codeOffreAjoute, string versionAjoute, string typeAjoute,
                                    ModeConsultation modeNavig);
        [OperationContract]
        List<ContratConnexeDto> GetContratsConnexes(string typeOffre, string codeTypeConnexite, string numeroConnexite);
        [OperationContract]
        List<ContratConnexeDto> GetContratsConnexesEngagement(string typeOffre, string codeTypeConnexite, string numeroConnexite);

        [OperationContract]
        List<ModeleDetailsConnexitesEngPeriodeDto> GetPeriodesConnexitesEngagement(string codeOffre, string version,
                                                                                   string type, string codeAvn,
                                                                                   ModeConsultation
                                                                                       modeNavig);

        [OperationContract]
        bool IsContratInConnexite(string codeOffre, string version, string type, string codeTypeConnexite, string numConnexite);
        [OperationContract]
        string AddConnexite(string codeOffre_connexe, string version_connexe, string type_connexe, string branche_connexe, string sousBranche_connexe, string categorie_connexe,
                                          string codeOffre_courant, string version_courant, string type_courant, string branche_courant, string sousBranche_courant, string categorie_courant,
                                          string codeObservation, string observation, string codeTypeConnexite, string numConnexite, string mode);

        [OperationContract]
        string AddConnexiteEngagement(string codeOffre_connexe, string version_connexe, string type_connexe, string branche_connexe, string sousBranche_connexe, string categorie_connexe,
                                          string codeOffre_courant, string version_courant, string type_courant, string branche_courant, string sousBranche_courant, string categorie_courant,
                                          string codeObservation, string observation, string codeTypeConnexite, string numConnexite, string mode);
        [OperationContract]
        string DeleteConnexite(string codeOffre_connexe, string version_connexe, string typeOffre_connexe, string numConnexite, string type_connexe, string ideConnexite, string user);
        [OperationContract]
        void DeleteConnexiteEng(string codeOffre,int version , string type );
        [OperationContract]
        bool IsContratConnexe(string codeOffre, string version, string type, string codeTypeConnexite);
        [OperationContract]
        string FusionDetachConnexite(
          string numOffreOrigine, string typeOffreOrigine, string versionOffreOrigine, string brancheOrigine, string sousBrancheOrigine, string catOrigine,
          string numConnexiteOrigine, long codeObsvOrigine, string obsvOrigine, string ideConnexiteOrigine,
          string numOffreActuelle, string typeOffreActuelle, string versionOffreActuelle, string brancheActuelle, string sousBrancheActuelle, string catActuelle,
          string numConnexiteActuelle, long codeObsvActuelle, string obsvActuelle, string codeTypeConnexite,
          string user, string modeAction);
        [OperationContract]
        List<EngagementConnexiteTraiteDto> GetEngagementsTraites(string idEngagement);
        [OperationContract]
        List<ContratConnexeTraiteDto> GetContratsConnexesTraite(string typeOffre, string codeTypeConnexite, string numeroConnexite);
        [OperationContract]
        List<EngagementConnexiteDto> GetEngagementsConnexite(string IdeConnexiteEngagement);
        //[OperationContract]
        //List<EngagementConnexiteDto> GetEngagementsConnexiteTraite(string typeOffre, string codeTypeConnexite, string numeroConnexite);
        [OperationContract]
        long UpdateEngagementTraite(string codeOffre, string versionOffre, string typeOffre, int dateDeb, int dateFin, int idEng, int idTraite, string codeFamille, long engTotal, long engAlbin, string user, string modeMaj);
        [OperationContract]
        string AddEngagementFamille(long idEng, int dateDeb, int dateFin, string codeFamille, long engTotal, long engAlbin, string user);
        #endregion
        #region Echeancier
        [OperationContract]
        EcheancierDto InitEcheancier(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string modeSaisieEcheancierParam);
        [OperationContract]
        List<EcheanceDto> GetAllEcheances(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig);

        [OperationContract]
        bool PossedeEcheances(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig);

        [OperationContract]
        List<EcheanceDto> GetEcheances(string codeOffre, string version, string type, string codeAvn, int typeEcheance, ModeConsultation modeNavig);
        [OperationContract]
        string UpdateEcheance(string codeOffre, string version, string type, DateTime? dateEcheance, decimal PrimePourcent, decimal montantEcheance, decimal montantCalcule, decimal fraisAccessoires, bool taxeAttentat, string typeOperation, int typeEcheance);
        [OperationContract]
        string UpdatePourcentCalcule(string codeOffre, string version, string type, string codeAvn, double comptantHT, double primeHT, ModeConsultation modeNavig);
        [OperationContract]
        string UpdateMontantCalcule(string codeOffre, string version, string type, string codeAvn, string primePourcent, double comptantHT, double primeHT, ModeConsultation modeNavig);
        [OperationContract]
        void SupprimerEcheance(string codeOffre, string version, string type, DateTime dateEcheance);
        [OperationContract]
        void SupprimerEcheances(string codeOffre, string version, string type);
        [OperationContract]
        void SupprimerEcheancier(string codeOffre, string version, string type, string codeAvn);
        #endregion

        #region Montant Référence
        [OperationContract]
        MontantReferenceDto InitInfoMontantReference(string codeOffre, string version, string type, string codeAvn, bool isReadonly, ModeConsultation modeNavig, string user, string acteGestion);
        [OperationContract]
        MontantReferenceDto ReloadMontantReference(string codeOffre, string version, string type, string codeAvn, string mode, bool isReadonly, ModeConsultation modeNavig, string user);
        [OperationContract]
        MontantReferenceInfoDto GetMontantFormule(string codeOffre, string version, string type, string codeAvn, string codeRsq, string codeForm, ModeConsultation modeNavig);
        [OperationContract]
        MontantReferenceDto GetMontantTotal(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig);
        [OperationContract]
        void ValidMontantFormule(string codeOffre, string version, string type, string codeRsq, string codeForm, decimal mntForce, bool mntProvi, decimal mntAcquis, bool chkMntAcquis);
        [OperationContract]
        void ValidMontantTotal(string codeOffre, string version, string type, decimal mntForce, decimal mntAcquis, bool checkedA, bool checkedP);
        [OperationContract]
        MontantReferenceDto UpdateMontantRef(string codeOffre, string version, string type, string codeAvn, bool topForce, bool topAcquis, bool topForceTotal, string commentForce, Int64 codeCommentForce, string user, ModeConsultation modeNavig, string acteGestion, bool reset);
        [OperationContract]
        void SaveCommentairesMontantRef(string codeOffre, string version, string type, Int64 codeCommentForce, string commentForce);
        #endregion

        #region Documents Joints

        [OperationContract]
        DocumentsJointsDto GetListDocumentsJoints(string codeOffre, string version, string type, string modeNavig, bool isReadOnly, string orderField, string orderType);

        [OperationContract]
        DocumentsAddDto OpenEditionDocsJoints(string idDoc);

        [OperationContract]
        DocumentsJointsDto SaveDocsJoints(string codeOffre, string version, string type, string idDoc, string typeDoc, string titleDoc, string fileDoc, string pathDoc, string refDoc, string user, string modeNavig, bool isReadOnly, string acteGestion);

        [OperationContract]
        DocumentsJointsDto DeleteDocsJoints(string idDoc, string codeOffre, string version, string type, string modeNavig, bool isReadOnly);

        [OperationContract]
        string ReloadPathFile(string typologie);
        #endregion

        #region Suivi Documents

        [OperationContract]
        SuiviDocumentsListeDto GetListSuiviDocuments(SuiviDocFiltreDto filtreDto, ModeConsultation modeNavig, bool readOnly);

        [OperationContract]
        bool GenerateDocuments(string numAffaire, string version, string type, string avenant, string lotId);

        [OperationContract]
        bool PrintDocuments(string lotId, string user);

        [OperationContract]
        bool ReeditDocument(string idDoc, string user);

        [OperationContract]
        List<string> EditerDocuments(string listeDocIdLogo, string listeDocIdNOLogo);

        [OperationContract]
        string UpdDocCPCS(string docId);

        [OperationContract]
        void SaveBloc(string fullPathDoc);

        [OperationContract]
        FileDescription OpenPJ(string docId);

        [OperationContract]
        string RefreshDocuments(string docId);

        #endregion
    }
}
