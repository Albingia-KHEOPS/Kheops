using ALBINGIA.Framework.Common.Constants;
using OP.WSAS400.DTO.ChoixClauses;
using OP.WSAS400.DTO.Clause;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Concerts.Clause;
using OP.WSAS400.DTO.Condition;
using OP.WSAS400.DTO.Ecran.ConditionRisqueGarantie;
using OP.WSAS400.DTO.Ecran.DetailsInventaire;
using OP.WSAS400.DTO.Ecran.DetailsObjetRisque;
using OP.WSAS400.DTO.Ecran.DetailsRisque;
using OP.WSAS400.DTO.FormuleGarantie;
using OP.WSAS400.DTO.GarantieModele;
using OP.WSAS400.DTO.MatriceFormule;
using OP.WSAS400.DTO.MatriceGarantie;
using OP.WSAS400.DTO.MatriceRisque;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres.Risque.Inventaire;
using OP.WSAS400.DTO.RefExprComplexe;
using OP.WSAS400.DTO.LibelleClauses;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using Albingia.Kheops.OP.Domain.Affaire;
using OP.WSAS400.DTO.Engagement;

namespace OPServiceContract.IClausesRisquesGaranties
{
    [ServiceContract]
    public interface IRisquesGaranties
    {
        [OperationContract]
        int GetNewInventaireId(string codeOffre, string version, string type, string perimetre, string codeRsq, string codeObj, string codeFor, string codeGaran);
        [OperationContract]
        string GetInfoInventGarantie(string idGarantie, string codeOffre, int version, string type, int codeFormule, int codeOption, ModeConsultation modeNavig, FormuleGarantieSaveDto formulesGarantiesSave, string user, string codeObjetRisque, string codeAvenant);
        //[OperationContract]
        //RisqueDto ObtenirRisqueApplique(string codeOffre, string version, string codeRisque, string codeOption);

        //[OperationContract]
        //string ObtenirLibGarantie(string codeGarantie);
        //[OperationContract]
        //string ObtenirRisqueObjetApplique(string codeOption, string[] perimetres);

        [OperationContract]
        OffreDto OffreGetDto(string id, int version, string type);
        [OperationContract]
        void SupprimerModeleByCategorie(string codeId, string infoUser);
        [OperationContract]
        List<GarantieModeleDto> GarantieModelesGetByBloc(string codeId);
        [OperationContract]
        List<GarantieModeleDto> ModelesGetList();

        //[OperationContract]
        //int GetNextRisque(OffreDto offre);
        [OperationContract]
        string InfoSpecRisqueSet(OffreDto offreDto, string codeAvn, ModeConsultation modeNavig);

        [OperationContract]
        List<RisqueObjetPlatDto> GetEarliestRisqueObjetFormule(string codeOffre, string version, string type);

        #region Conditions

        //[OperationContract]
        //LigneGarantieDto SauvegarderCondition(string ligne, string separateur);

        [OperationContract]
        int SaveCondition(LigneGarantieDto conditionDto, string codeAvn);

        [OperationContract]
        EnsembleGarantieDto CancelGarantie(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string codeGarantie, string codeCondition, string oldFRHExpr, string oldLCIExpr, ModeConsultation modeNavig, bool isReadOnly);

        [OperationContract]
        bool DeleteCondition(string codeCondition);

        [OperationContract]
        void SuppressionExpression(string argType, string argTypeAppel, string argIdExpression, string argIdCondition);

        [OperationContract]
        void AffectExpressionCondition(string argType, string argCodeCondition, string argCodeExpression);

        [OperationContract]
        ConditionRisqueGarantieGetResultDto ObtenirFullConditions(ConditionRisqueGarantieGetQueryDto query, string codeAvn, ModeConsultation modeNavig, bool isReadOnly, bool loadFormule, string user);

        [OperationContract]
        ConditionRisqueGarantieGetResultDto ObtenirConditions(ConditionRisqueGarantieGetQueryDto query, string codeAvn, ModeConsultation modeNavig, bool isReadOnly);

        [OperationContract]
        ConditionComplexeDto RecuperationConditionComplexe(string argCodeOffre, string argVersion, string argTypeOffre, string argType);

        [OperationContract]
        ConditionComplexeDto RecuperationDetailComplexe(string codeOffre, string version, string type, string codeAvn, string codeExpr, string typeExpr, ModeConsultation modeNavig);



        [OperationContract]
        string EnregistrementConditionComplexe(ConditionComplexeDto argExpComp, string argTypeOffre, string argType, string argCodeOffre, string argVersion, int? argIdExpression, string argLibelle, string argDescriptif);

        [OperationContract]
        void SupressionDetail(string argType, string argId);

        [OperationContract]
        List<EngagementPeriodeDto> IsInHpeng(string argCodeOffre);

        [OperationContract]
        List<ParametreDto> GetListeFiltre(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption,
            string typeFiltre, string garantie, string voletbloc, string niveau, ModeConsultation modeNavig);

        [OperationContract]
        string GetFormuleGarantieBranche(string codeOffre, string version, string type, string codeFormule, string codeOption);

        #endregion

        [OperationContract]
        GarantieModeleDto GarantieModeleInfoGet(string code);

        [OperationContract]
        List<GarantieModeleDto> GarantieModeleGet(string code, string description);

        [OperationContract]
        void EnregistrerGarantieModele(string code, string description, bool isNew, out string msgRetour);

        [OperationContract]
        void CopierGarantieModele(string code, string codeCopie);

        [OperationContract]
        void SupprimerGarantieModele(string code, out string msgRetour);

        [OperationContract]
        bool ExistGarantieModeleDansContrat(string code);

        [OperationContract]
        List<GarantieTypeDto> GarantieTypeGet(string codeModele);

        [OperationContract]
        GarantieTypeDto GarantieTypeInfoGet(int seq);
        [OperationContract]
        GarantieTypeDto GarantieTypeLienInfoGet(int seq);

        [OperationContract]
        void EnregistrerGarantieType(GarantieTypeDto garType, bool isNew, out string msgRetour);

        [OperationContract]
        void SupprimerGarantieType(int seq, out string msgRetour);

        [OperationContract]
        void UpdateClauseLibelle(string branche, string cible, string nm1, string nm2, string nm3,string libelle);

        [OperationContract]
        void DeleteClausesLibelle(string branche, string cible, string nm1, string nm2, string nm3, string libelle);

        [OperationContract]
        void SaveClauseLibelle(string branche, string cible, string nm1, string nm2, string nm3, string libelle);

        [OperationContract]
        List<LibelleClauseDto> GetClausesLibelle(string branche, string cible, string nm1, string nm2, string nm3);

        [OperationContract]
        List<ClauseBrancheDto> GetClausesBranches();
        
        //SAB24042016: Pagination clause
        [OperationContract]
        List<ClauseDto> ClausesGet(string type, string numeroOffre, string version, string codeAvn, string etape, string contexte, string rubrique, string sousRubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig);
        //[OperationContract]
        //List<ClauseDto> ClausesGet(string type, string numeroOffre, string version, string codeAvn, string etape, string contexte, string rubrique, string sousRubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig, string coltri, string triimg, int StartLine, int EndLine);
        //[OperationContract]
        //ChoixClausesSetResultDto ClausesSet(ChoixClausesSetQueryDto query);

        //[OperationContract]
        //void EnregistreClauseUnique(string type, string numeroOffre, string version, string natureClause, string codeClause, string versionClause, string actionEnchaine, string contexte, string utilisateur, string etape);
        [OperationContract]
        ClauseDto DetailsClauses(string type, string numeroOffre, string version, string codeAvn, string etape, string contexte, string rubrique, string sousRubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig);

        [OperationContract]
        string EnregistreClauseLibre(string codeOffreContrat, string versionOffreContrat, string typeOffreContrat, string contexte, string etape, string codeRisque, string codeFormule, string codeOption, string codeObj, string libelleClauseLibre, string texteClauseLibre);

        [OperationContract]
        void SupprimeClauseUnique(string id);

        //[OperationContract]
        //List<RisqueDto> ListeRisquesGetByTerm(string codeOffre, string version);

        [OperationContract]
        string UpdateEtatTitre(string clauseId, string etatTitre);

        [OperationContract]
        void UpdateTextClauseLibre(string clauseId, string titreClauseLibre, string texteClauseLibre, string codeObj);
        [OperationContract]
        void UpdateTextClauseLibreOffreSimp(string clauseId, string titreClauseLibre, string texteClauseLibre, string codeRisque, string codeObj, string codeFormule, string codeOption);

        /// <summary>
        /// Detailses the risque get.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        [OperationContract]
        DetailsRisqueGetResultDto DetailsRisqueGet(DetailsRisqueGetQueryDto query, string codeAvn, string branche, string cible, ModeConsultation modeNavig, bool isAdmin, bool isUserHorse);

        /// <summary>
        /// Detailses the risque set.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        [OperationContract]
        DetailsRisqueSetResultDto DetailsRisqueSet(DetailsRisqueSetQueryDto query, string user);

        /// <summary>
        /// Detailses the risque del.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        [OperationContract]
        DetailsRisqueDelResultDto DetailsRisqueDel(DetailsRisqueDelQueryDto query);

        /// <summary>
        /// Detailses the objet risque get.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        [OperationContract]
        DetailsObjetRisqueGetResultDto DetailsObjetRisqueGet(DetailsObjetRisqueGetQueryDto query, string codeAvn, string branche, string cible, ModeConsultation modeNavig, bool isAdmin, bool isUserHorse);

        [OperationContract]
        DetailsObjetRisqueGetResultDto GetListesNomenclatureOnly(string codeOffre, string version, string type, string codeRisque, string codeObjet, string cible);

        [OperationContract]
        List<NomenclatureDto> GetSpecificListeNomenclature(Int64 IdNomenclatureParent, int NumeroCombo, string cible, string idNom1, string idNom2, string idNom3, string idNom4, string idNom5);
        /// <summary>
        /// Detailses the objet risque set.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        [OperationContract]
        string DetailsObjetRisqueSet(DetailsObjetRisqueSetQueryDto query, string user);

        /// <summary>
        /// Detailses the objet risque del.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        [OperationContract]
        DetailsObjetRisqueDelResultDto DetailsObjetRisqueDel(DetailsObjetRisqueDelQueryDto query);

        [OperationContract]
        void SaveValeurModeAvn(string codeOffre, string version, string type, string valeur);

        [OperationContract]
        string GetQuestionMedical(string codeAffaire, string version, string type, string codeRsq, string codeObj, string oldValue, bool controlAssiette, string user);

        /// <summary>
        /// Liste des risques objets
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>

        [OperationContract]
        string GetFirstCodeRsq(string codeOffre, string version, string type);
        [OperationContract]
        string GetFirstCodeObjRsq(string codeOffre, string version, string type, string codeRsq);

        [OperationContract]
        List<RisqueDto> ListRisquesObjet(string codeOffre, string version, string type, string avenant, ModeConsultation modeNavig);

        /// <summary>
        /// Liste des activites
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [OperationContract]
        RechercheActiviteDto GetActivites(string code, string branche, string cible, string nom, int startLigne, int endLigne);

        [OperationContract]
        string LoadCodeClassByCible(string codeCible, string codeActivite);

        #region Détail Inventaire

        [OperationContract]
        void DetailsInventaireDel(DetailsInventaireDelQueryDto query);
        //DetailsInventaireDelResultDto DetailsInventaireDel(DetailsInventaireDelQueryDto query);


        //[OperationContract]
        //DetailsInventaireSetResultDto DetailsInventaireSet(DetailsInventaireSetQueryDto query);







        [OperationContract]
        string DetailsInventaireGetSumBudget(string codeInventaire, string typeInventaire);


        #endregion


        #region Formule

        [OperationContract]
        List<ParamNatGarDto> GetParamNatGar();
        [OperationContract]
        List<ParametreDto> ObtenirTypesInventaire();

        [OperationContract]
        List<RisqueObjetPlatDto> GetRisqueObjetFormule(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, ModeConsultation modeNavig);

        [OperationContract]
        string ObtenirFormuleOptionByOffre(string codeOffre, string version, string codeAvn, ModeConsultation modeNavig);

        [OperationContract]
        FormuleDto InitFormuleGarantie(string codeOffre, string version, string type, string avenant, string codeFormule,
                                       string codeOption, string formGen, ModeConsultation modeNavig, bool readOnly, string user);

        [OperationContract]
        void DeleteFormuleGarantie(string codeOffre, string version, string type, string codeFormule, string typeDel);
        [OperationContract]
        void DeleteFormuleGarantieRsq(string codeOffre, string version, string type, string codeRisque, string typeDel);
        //[OperationContract]
        //string GetNextFormule();
        [OperationContract]
        void DeleteFormule(string codeOffre, string version, string type, string codeFormule, string typeDel);

        [OperationContract]
        void CheckFormule(string codeOffre, string version, string type, string codeFormule, string codeOption);

        [OperationContract]
        string DuplicateFormule(string codeOffre, string version, string type, string codeFormule, string user);
        [OperationContract]
        string GetLibFormule(int codeFormule, string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig);

        //[OperationContract]
        //string FormulesGarantiesSet(string codeOffre, string version, string type, string codeFormule, string codeOption,
        //                     string libelle, string codeObjetRisque, FormuleGarantieDto formuleGarantie, string user);
        [OperationContract]
        string FormulesGarantiesSaveSet(string codeOffre, string version, string type, string codeAvenant, string dateAvenant, string codeFormule, string codeOption, string formGen,
                             string libelle, string codeObjetRisque, FormuleGarantieSaveDto formuleGarantie, string user);

        [OperationContract]
        void RegenerateCanevas(string user, bool totalRegeneration);

        [OperationContract]
        FormGarDto FormulesGarantiesGet(string codeOffre, string version, string type, string codeAvn, string codeFormule,
                                                string codeOption, string formGen, string codeCategorie, string codeAlpha,
                                                string branche, string libFormule, string user, int appliqueA, bool isReadOnly, ModeConsultation modeNavig);


        [OperationContract]
        string EnregistrerFormuleGarantie(string codeOffre, string version, string type, string codeAvt, string codeFormule,
                                          string codeOption, string formGen, string codeAlpha, string branche, string codeCategorie,
                                          string libFormule, string codeObjetRisque, string user);
        [OperationContract]
        void DeleteFormuleGarantieHisto(string codeOffre, string version, string type, string codeFormule, string codeOption);

        [OperationContract]
        FormuleGarantieDetailsDto ObtenirInfosDetailsFormuleGarantie(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string codeGarantie, string codeObjetRisque, FormuleGarantieSaveDto volets, DateTime? dateEffAvnModifLocale, string user, bool isReadonly, ModeConsultation modeNavig);

        [OperationContract]
        FormuleGarantiePorteeDto ObtenirInfosPorteeFormuleGarantie(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string codeGarantie, ModeConsultation modeNavig, string alimAssiette, string branche, string cible, string modifAvn, string codeObjetRisque);

        [OperationContract]
        void SavePorteeGarantie(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string idGarantie, string codeGarantie, string nature, string codeRsq, string codesObj, string codeObjetRisque, FormuleGarantieSaveDto formulesGarantiesSave, string user, RisqueDto rsq, string alimAssiette, bool reportCal);


        [OperationContract]
        string SauverDetailsFormuleGarantie(string codeOffre, string version, string type, string codeFormule, string codeOption, string codeAvenant, string codeObjetRisque, string user, FormuleGarantieDetailsDto garantieDetails, FormuleGarantieSaveDto formulesGarantiesSave);

        [OperationContract]
        void SaveAppliqueA(string codeOffre, string version, string type, string codeFormule, string codeOption, string cible, string formGen, string objetRisqueCode, string user);

        [OperationContract]
        GarantieDetailInfoDto GetInfoDetailsGarantie(string codeOffre, string version, string type, string codeGarantie, string codeFormule, string codeOption);

        [OperationContract]
        string GetLibCible(string codeCible);

        [OperationContract]
        void SaveFormuleFromCondition(string codeOffre, string version, string type, string codeFormule, string codeOption, string libelle);

        [OperationContract]
        FormuleDto GetCibleInfoFormule(string codeOffre, string version, string type, string codeFormule);

        [OperationContract]
        void UpdateDateForcee(string codeOffre, string version, string type, string codeGarantie, string codeFormule, string codeOption, string codeAvn, string codeObjetRisque, string niveauLib, string guidV, string guidB, string guidG, int? dateModifAvt, bool isChecked, string user, FormuleGarantieSaveDto volets);
        [OperationContract]
        void InitParfaitAchevement(string codeOffre, string version, string type, string codeGarantie, string codeFormule, string codeOption, string codeAvenant, string codeObjetRisque, string user, FormuleGarantieSaveDto volets);
        [OperationContract]
        bool IsTraceAvnExist(string codeAffaire, string version, string type, string codeFormule, string codeOption);

        #endregion

        #region Matrice Formule

        [OperationContract]
        MatriceFormuleDto InitMatriceFormule(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string user, string acteGestion, bool isReadonly);

        [OperationContract]
        string GetValidRsq(string codeOffre, string version, string type, string codeRsq);

        #endregion

        #region Matrice Risque

        [OperationContract]
        MatriceRisqueDto InitMatriceRisque(string codeOffre, string version, string type, string codeAvenant, ModeConsultation modeNavig, string user, string acteGestion, bool isReadonly);

        #endregion

        #region Matrice Garantie

        [OperationContract]
        MatriceGarantieDto InitMatriceGarantie(string argCodeOffre, string argVersion, string argType, string argCodeAvn, ModeConsultation modeNavig, string user, string acteGestion, bool isReadonly);

        [OperationContract]
        List<GarantiePeriodeDto> GetDateDebByGaranties(int[] ids, int codeAvn);
        #endregion

        #region Inventaires

        [OperationContract]
        InventaireDto GetInventaire(string codeOffre, int version, string type, string codeAvn, string ecranProvenance, int codeRisque, int codeObjet, int codeFormule, string codeGarantie, string typeInventaire, Int64 codeInven, string branche, string cible, ModeConsultation modeNavig);

        [OperationContract]
        InventaireGridRowDto SaveLineInventaire(string codeOffre, int version, string type, int codeInven, int typeInven, InventaireGridRowDto inventaireLigne);

        [OperationContract]
        void SaveInventaire(string codeOffre, string version, string type, string ecranProvenance, string codeRisque, string codeObjet, string codeInven, string descriptif, string description, string valReport, string unitReport, string typeReport, string taxeReport, bool activeReport, string typeAlim, string garantie, string codeFormule, string codeOption);

        [OperationContract]
        void DeleteLineInventaire(string codeInven);

        #endregion


        #region Test
        [OperationContract]
        bool TestSrtoredProc();

        #endregion

        #region Oppositions

        [OperationContract]
        List<OppositionDto> ObtenirListeOppositions(string idOffre, string versionOffre, string typeOffre, string idRisque, ModeConsultation modeNavig);

        [OperationContract]
        OppositionDto ObtenirDetailOpposition(string idOffre, string versionOffre, string typeOffre, string codeAvn, string idRisque, string idOpposition, string mode, ModeConsultation modeNavig, string typeDest);

        [OperationContract]
        int MiseAJourOpposition(string idOffre, string versionOffre, string typeOffre, string idRisque, OppositionDto opposition, string objets, string user);

        [OperationContract]
        List<OrganismeOppDto> OrganismesGet(string value, string mode, string typeOppBenef);

        #endregion

        #region Clauses
        [OperationContract]
        ClausierPageDto InitClausier();
        [OperationContract]
        List<ClausierDto> SearchClause(string libelle, string motcle1, string motcle2, string motcle3, string sequence, string rubrique, string sousrubrique, string modaliteAffichage, int date);
        [OperationContract]
        List<ClausierDto> GetHistoClause(string rubrique, string sousrubrique, string sequence);
        [OperationContract]
        //void SaveClause(string codeOffre, string version, string type, string etape, string perimetre, string codeRsq, string codeObj,
        //    string codeFor, string codeOpt, string contexte, string codeClause, string versionClause);
        string SaveClause(string codeOffre, string version, string type, string etape, string perimetre, string codeRsq, string codeObj,
           string codeFor, string codeOpt, string contexte, string rubrique, string codeClause, string sousRubrique, string sequence, string versionClause, string numAvenant);
        [OperationContract]
        List<ParametreDto> GetListSousRubriques(string codeRubrique);
        [OperationContract]
        List<ParametreDto> GetListSequences(string codeRubrique, string codeSousRubrique);


        [OperationContract]
        RisqueDto GetRisque(string codeOffre, string version, string type, string codeRsq);


        [OperationContract]
        ClauseLibreViewerDto GetInfoClauseLibreViewer(string codeOffre, string version, string type, string codeRsq, string clauseId, string etape, string contexte);

        [OperationContract]
        string VerifAjout(string etape, string contexte, string typeAjt);

        //[OperationContract]
        //string ExecuteScript(string codeOffre, string version, string type, string etape, string perimetre, string codeRsq, string codeObj, string codeFor, string codeOpt, ParametreGenClauseDto parmClause);
        [OperationContract]
        string SaveClauseLibre(string codeOffre, string version, string type, string codeAvt, string contexte, string etape, string codeRsq, string codeObj, string codeFor, string codeOpt, string emplacement, string sousemplacement, string ordre);
        [OperationContract]
        void UpdateDocumentLibre(string idClause, string idDoc, string codeObj, string codeAvn);
        [OperationContract]
        string CreateDocumentLibre(string codeOffre, string version, string type, string etape, string idClause, string pathDoc, string nameDoc, string createDoc);
        [OperationContract]
        string GetClauseFilePath(string clauseId, ModeConsultation modeNavig);

        [OperationContract]
        ChoixClausePieceJointeDto GetListPiecesJointes(string codeOffre, string version, string type, string codeRisque, string codeObjet, string etape, string contexte);
        [OperationContract]
        void SavePiecesJointes(string codeOffre, string version, string type, string contexte, string etape, string codeRsq, string codeObj,
                string codeFor, string codeOpt, string emplacement, string sousemplacement, string ordre, string piecesjointesid);

        [OperationContract]
        void SaveClauseEntete(string idClause, string emplacement, string sousemplacement, string ordre);

        [OperationContract]
        void SaveClauseMagnetic(string codeAffaire, string version, string type, int idDoc, string acteGes, string etape, string nameClause, string fileName,
            int idClause, string emplacement, string sousemplacement, string ordre, string contexte);
        #endregion

        #region Inventaire de garantie
        [OperationContract]
        string SupprimerGarantieInventaire(string codeOffre, string version, string type, string codeFormule, string codeGarantie, string codeInventaire);
        [OperationContract]
        string SupprimerGarantieListInventaires(string codeOffre, string version, string type, string codeFormule, string codesGaranties, string codesInventaires);
        [OperationContract]
        string SupprimerListInventairesByCodeInventaire(string codesInventaires);
        #endregion

        #region LCIFranchise
        [OperationContract]
        void EnregistrementExpCompGeneraleRisque(string codeOffre, string version, string typeOffre, string codeAvn, string codeFormule, string codeOption, string codeRisque, string codeExpression, string unite, AlbConstantesMetiers.ExpressionComplexe typeVue, AlbConstantesMetiers.TypeAppel typeAppel, ModeConsultation modeNavig);
        [OperationContract]
        LCIFranchiseDto GetLCIFranchise(string codeOffre, string version, string typeOffre, string codeAvn, string codeRisque, AlbConstantesMetiers.ExpressionComplexe typeVue, AlbConstantesMetiers.TypeAppel typeAppel, ModeConsultation modeNavig);
        [OperationContract]
        string InfoSpecRisqueLCIFranchiseSet(OffreDto offreDto, string codeAvn
                                , string argValeurLCIRisque, string argUniteLCIRisque, string argTypeLCIRisque, string argLienCpxLCIRisque
                                , string argValeurFranchiseRisque, string argUniteFranchiseRisque, string argTypeFranchiseRisque, string argLienCpxFranchiseRisque, ModeConsultation modeNavig);
        #endregion
        #region Generation Clauses

        [OperationContract]
        RetGenClauseDto GenerateClause(string type, string codeDossier, int version, ParametreGenClauseDto parmClause);
        [OperationContract]
        string ValiderChoixClause(string type, string codeAffaire, int version, int codeAvn, int idClause, string idLot, List<ClauseOpChoixDto> lstChoixClause, string user);

        //[OperationContract]
        //string ControlesClauses(string type, string codeDossier, int version, ParametreGenClauseDto parmClause);

        [OperationContract]
        string VerifierContraintesClauses(string codeOffre, string version, string type, string perimetre, string acteGestion, string etape, string risque, string objet, string formule, string option, string contexte);


        #endregion
        #region Étapes choix clause
        [OperationContract]
        List<ParametreDto> GetListEtapes(OrigineAppel origine);
        [OperationContract]
        List<ParametreDto> GetListContextes(string codeEtape, string codeOffre, string version, string type, string codeAvn, string contexte, string famille, ModeConsultation modeNavig);
        //SAB24042016: Pagination clause
        [OperationContract]
        ChoixClausesInfoDto GetInfoChoixClause(OrigineAppel origine,
                        string type, string codeOffre, string version, string codeAvn, string etape, string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig,
                        string codeEtape, string contexte, string famille);

        //[OperationContract]
        //ChoixClausesInfoDto GetInfoChoixClause(OrigineAppel origine,
        //                string type, string codeOffre, string version, string codeAvn, string etape, string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig,
        //                string codeEtape, string contexte, string famille, string coltri, string triimg, int StartLine, int EndLine);


        //SAB24042016: Pagination clause
        [OperationContract]
        ChoixClausesInfoDto SupprimeClauseUnique2(string id, OrigineAppel origine,
                        string type, string codeOffre, string version, string codeAvn, string etape, string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig,
                        string codeEtape, string contexte, string famille);

        //[OperationContract]
        //ChoixClausesInfoDto SupprimeClauseUnique2(string id, OrigineAppel origine,
        //                string type, string codeOffre, string version, string codeAvn, string etape, string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig,
        //                string codeEtape, string contexte, string famille, string coltri, string triimg, int StartLine, int EndLine);


        //SAB24042016: Pagination clause
        [OperationContract]
        EnregistrementClauseLibreDto EnregistreClauseLibre2(string codeOffre, string version, string type, string codeAvn, string contexteClause, string etape, string codeRisque, string codeFormule, string codeOption, string codeObj, string libelleClauseLibre, string texteClauseLibre,
                            OrigineAppel origine,
                                string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string filtre, ModeConsultation modeNavig,
                                string codeEtape, string contexte, string famille);

        //[OperationContract]
        //EnregistrementClauseLibreDto EnregistreClauseLibre2(string codeOffre, string version, string type, string codeAvn, string contexteClause, string etape, string codeRisque, string codeFormule, string codeOption, string codeObj, string libelleClauseLibre, string texteClauseLibre,
        //                    OrigineAppel origine,
        //                        string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string filtre, ModeConsultation modeNavig,
        //                        string codeEtape, string contexte, string famille, string coltri, string triimg, int StartLine, int EndLine);

        //SAB24042016: Pagination clause

        [OperationContract]
        EnregistrementClauseLibreDto UpdateTextClauseLibre2(string clauseId, string titreClauseLibre, string texteClauseLibre, string codeObj,
                        string codeOffre, string version, string type, string codeAvn, string etape, string codeRisque, string codeFormule, string codeOption, OrigineAppel origine,
                        string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string filtre, ModeConsultation modeNavig,
                        string codeEtape, string contexte, string famille);

        //[OperationContract]
        //EnregistrementClauseLibreDto UpdateTextClauseLibre2(string clauseId, string titreClauseLibre, string texteClauseLibre, string codeObj,
        //                string codeOffre, string version, string type, string codeAvn, string etape, string codeRisque, string codeFormule, string codeOption, OrigineAppel origine,
        //                string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string filtre, ModeConsultation modeNavig,
        //                string codeEtape, string contexte, string famille, string coltri, string triimg, int StartLine, int EndLine);
        //SAB24042016: Pagination clause
        [OperationContract]
        ClauseVisualisationDto ClausesGet2(string type, string codeOffre, string version, string codeAvn, string etape, string contexte, string rubrique, string sousrubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig);

        //[OperationContract]
        //ClauseVisualisationDto ClausesGet2(string type, string codeOffre, string version, string codeAvn, string etape, string contexte, string rubrique, string sousrubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig, string coltri, string triimg, int StartLine, int EndLine);

        #endregion

        [OperationContract]
        string LoadComplementNum1(string concept, string famille, string code);

        [OperationContract]
        List<ParametreDto> GetListeTypesRegularisation(string branche, string cible);

        [OperationContract]
        bool CheckObjetSorit(string codeOffre, int? version, string type, string codeAvn, string openObj);

        [OperationContract]
        List<RisqueDto> GetFormuleRisquesApplicables(string codeOffre, string version, string type, string avenant, string codeFormule, string codeOption, ModeConsultation modeNavig);

        [OperationContract]
        FormuleDto GetFormuleGarantieInfo(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, ModeConsultation modeNavig);

        [OperationContract]
        List<FormuleDto> GetFormuleIdByRsq(string codeOffre, int numRsq);

        [OperationContract]
        decimal GetSumPrimeGarantie(string codeAff, long codeFromule);

        [OperationContract]
        List<FormuleDto> GetIdGar(string codeAff,long codeFormule);

        [OperationContract]
        void UpdateKpgartar(long id, decimal? PrimeGaranties);
        
        [OperationContract]
        string GetLettreLibFormuleGarantie(IdContratDto contrat);

        #region Expressions Complexes

        [OperationContract]
        ListRefExprComplexeDto LoadListesExprComplexe();
        [OperationContract]
        List<ParametreDto> LoadListExprComplexe(string typeExpr);
        [OperationContract]
        ConditionComplexeDto GetInfoExpComplexe(string typeExpr, string codeExpr);
        [OperationContract]
        Int32 SaveDetailExpr(string idExpr, string typeExpr, string codeExpr, string libExpr, bool modifExpr, string descrExpr);
        [OperationContract]
        void DeleteExprComp(string idExpr, string typeExpr);
        [OperationContract]
        void SaveRowExprComplexe(string idExpr, string typeExpComp, string idRowExpr,
            string valExpr, string unitExpr, string typeExpr, string concuValExpr, string concuUnitExpr, string concuTypeExpr,
            string valMinFrh, string valMaxFrh, string debFrh, string finFrh);
        [OperationContract]
        void DelRowExprComplexe(string idExpr, string typeExpComp, string idRowExpr);
        [OperationContract]
        ConditionComplexeDto LoadRowsExprComplexe(string typeExpr, string idExpr);

        [OperationContract]
        List<ConditionComplexeDto> LoadListExprComplexeReferentiel(string typeExpr, string codeExpr);
        [OperationContract]
        string ValidSelExprReferentiel(string codeOffre, string version, string type, string mode, string typeExpr, string idExpr, string splitCharHtml);

        [OperationContract]
        string DuplicateExpr(string codeOffre, string version, string type, string codeAvn, string typeExpr, string codeExpr);
        [OperationContract]
        List<ParametreDto> SearchExprComp(string typeExpr, string strSearch);

        [OperationContract]
        string CheckSessionClause(int idSessionClause, string ipStation, string user);
        [OperationContract]
        int GetFullPathDocument(string documentFullPath, string ipStation, string userAD);
        #endregion

        #region Trace régularisation
        [OperationContract]
        bool HaveTraceRegularisation(string codeContrat,string codeRisque, string version, string type, string numAvn);
        [OperationContract]
        bool? GetIsRegularisation(string codeContrat, string codeRisque, string version);
        #endregion


    }
}
