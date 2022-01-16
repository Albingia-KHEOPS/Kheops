using System.Collections.Generic;
using OP.WSAS400.DTO.Bloc;
using OP.WSAS400.DTO.Categorie;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Volet;
using System.ServiceModel;
using OP.WSAS400.DTO.GarantieModele;
using OP.WSAS400.DTO.ParametreCibles;
using System;
using OP.WSAS400.DTO.VerouillageOffres;
using OP.WSAS400.DTO.ParametreEngagement;
using OP.WSAS400.DTO.ParamIS;
using OP.WSAS400.DTO.ParametreFamilles;
using OP.WSAS400.DTO.ParametreFiltre;
using OP.WSAS400.DTO.ParametreGaranties;
using OP.WSAS400.DTO.ParametreTypeValeur;
using OP.WSAS400.DTO.ParamTemplates;
using ALBINGIA.Framework.Common.Constants;
using OP.WSAS400.DTO.MenuContextuel;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Ecran.Rercherchesaisie;
using OP.WSAS400.DTO.GestionNomenclature;
using OP.WSAS400.DTO.GestUtilisateurs;
using OP.WSAS400.DTO.Stat;
using OP.WSAS400.DTO.Logging;
using OP.WSAS400.DTO.FraisAccessoires;
using OP.WSAS400.DTO.OffresRapide;
using OP.WSAS400.DTO.ParametreInventaires;

namespace OPServiceContract.IAdministration
{
    [ServiceContract]
    public interface IVoletsBlocsCategories
    {
        #region Transverses

        [OperationContract]
        string SupprimerModeleByCategorie(string codeId, string infoUser);

        //[OperationContract]
        //string EnregistrerModeleByCategorie(string codeId, string codeIdBloc, string dateApp, string codeTypo,
        //                                  string codeModele, string user);

        [OperationContract]
        string EnregistrerModeleByCible(string codeId, string codeIdBloc, string dateApp, string codeTypo, string codeModele, string user);

        [OperationContract]
        List<GarantieModeleDto> GarantieModelesGetByBloc(string codeId);

        [OperationContract]
        List<ParametreDto> ObtenirParametres(string contexte, string famille);

        [OperationContract]
        List<BrancheDto> BranchesGet();

        [OperationContract]
        List<GarantieModeleDto> ModelesGetList();

        #endregion

        #region Volet

        [OperationContract]
        List<DtoVolet> VoletsGetByCible(string codeId, string codeBranche);

        [OperationContract]
        void EnregistrerVoletByCible(string codeId, string codeBranche, string codeCible, string codeIdCible, string codeVolet, string codeIdVolet, string codeCaractere, double ordreVolet, string user);

        //[OperationContract]
        //List<DtoVolet> VoletsGetByCategorie(string codeId);

        [OperationContract]
        List<DtoVolet> VoletsGet(string code, string description);

        [OperationContract]
        List<DtoVolet> VoletsGetList();

        [OperationContract]
        DtoVolet VoletInfoGet(string code);

        [OperationContract]
        void VoletInfoSet(string codeId, string code, string description, string branche, string isVoletGeneral, string isVoletCollapse, string update = "0", string user = "");

        [OperationContract]
        string SupprimerVolet(string code, string infoUser);
        [OperationContract]
        void EnregistrerVoletByCategorie(string codeId, string codeBranche, string codeCategorie, string codeIdCategorie,
                                         string codeVolet, string codeIdVolet, string codeCaractere, string user);
        [OperationContract]
        string SupprimerVoletByCategorie(string codeId, string codeBranche, string codeIdCible, string infoUser);

        #endregion

        #region Bloc

        [OperationContract]
        void EnregistrerBlocByCible(string codeId, string codeBranche, string codeCible, string codeVolet, string codeIdVolet, string codeBloc, string codeIdBloc, string codeCaractere, double ordreBloc, string user);

        [OperationContract]
        List<BlocDto> BlocsGetByVolet(string codeId);

        [OperationContract]
        List<BlocDto> BlocsGet(string code, string description);

        [OperationContract]
        List<BlocDto> BlocsGetList();

        [OperationContract]
        BlocDto BlocInfoGet(string code);

        [OperationContract]
        void BlocInfoSet(string codeId, string code, string description, string update = "0", string user = "");

        [OperationContract]
        string SupprimerBloc(string code, string infoUser);

        [OperationContract]
        void EnregistrerBlocByCategorie(string codeId, string codeBranche, string codeCategorie, string codeVolet,
                                        string codeIdVolet, string codeBloc, string codeIdBloc, string codeCaractere,
                                        string user);

        [OperationContract]
        string SupprimerBlocByCategorie(string codeId, string codeIdVolet, string infoUser);

        [OperationContract]
        List<BlocDto> GetListeBlocsIncompatiblesAssocies(string codeIdBloc, string typeBloc);

        [OperationContract]
        List<BlocDto> GetListeBlocsReferentielIncompatiblesAssocies(string codeIdBloc, string typeBloc);

        [OperationContract]
        string EnregistrerBlocIncompatibleAssocie(string idAssociation, string mode, string idBloctraite, string idBlocIncompatible, string typeBloc, string user);

        #endregion

        #region Categorie
        [OperationContract]
        CategorieDto ObtenirCategorieByCode(string code);

        #endregion

        #region Cible

        [OperationContract]
        List<CibleDto> CiblesGet(string codeBranche);

        [OperationContract]
        List<ParamCiblesDto> LoadListCible(string codeCiblePattern, string libelleCiblePattern);

        [OperationContract]
        List<ParamListCibleBranchesDto> GetCibleBranches(Int64 codeCibleId);

        [OperationContract]
        List<ParametreDto> GetBranches(int codeCibleId);
        [OperationContract]
        List<ParametreDto> GetBranchesEdit(int codeCibleId, string codeBranche);
        [OperationContract]
        List<ParametreDto> GetSousBranches(string codeBranche);
        [OperationContract]
        List<ParametreDto> GetListeBranches();

        [OperationContract]
        List<ParametreDto> GetCategories(string codeBranche, string codeSousBranche);

        [OperationContract]
        int AjouterBSCCible(string guidIdCible, string codeCible, string codeBranche, string codeSousBranche, string codeCategorie);
        [OperationContract]
        int AjouterCible(string codeCible, string descriptionCible, string grille, string famille, string concept, string user);
        [OperationContract]
        void ModifierCible(string guidIdCible, string descriptionCible, string grille, string famille, string concept, string user);
        [OperationContract]
        string SupprimerCible(int guidIdCible, string infoUser);
        [OperationContract]
        string SupprimerCibleBSCByGuidId(string guidId, string infoUser);
        [OperationContract]
        int ModifierCibleBSCByGuidId(string guidIdCible, string codeCible, string codeBranche, string codeSousBranche, string codeCategorie);
        [OperationContract]
        ParamCiblesDto GetCible(string guidId);
        [OperationContract]
        bool ExisteCiblePortefeuille(int guidId);
        [OperationContract]
        void AjouterOffreVerouille(string sevice, string typ, string ipb, int alx, int avn, int kavsua, int kavnum, string kavsbr, string kavactg, string kavact, string kavverr, string user, string kavlib);
        [OperationContract]
        List<OffreVerouilleeDto> GetOffresVerouillees(bool TypeOffre_O, bool TypeOffre_P, string NumOffre, string Version, string numAvn, string Utilisateur, string DateVerouillageDebut, string DateVerouillageFin);
        [OperationContract]
        void SupprimerOffreVerouillee(string NumOffre, string Version, string type, string numAvn, string user, string acteGestion, bool isAlimStat, bool isModifHorsAvn, bool isAnnul);
        [OperationContract]
        string GetUserVerrou(string codeOffre, string version, string type, string numAvn);
        [OperationContract]
        bool IsOffreVeruouille(string codeOffre, string version, string type, string numAvn);
        [OperationContract]
        List<ParametreDto> GetGrilles();
        #endregion

        #region Inventaire
        [OperationContract]
        List<ParametreDto> GetFiltres();
        [OperationContract]
        ParamInventairesListDto ModifierInventaire(string guidIdCible, string codeInventaire, string descriptionCible, int kagtmap, string codeFiltre);
        [OperationContract]
        ParamInventairesListDto AjouterInventaire(string codeInventaire, string descriptionInventaire, int kagtmap, string codeFiltre);
        [OperationContract]
        ParamInventairesListDto SupprimerInventaire(int guidIdInventaire);
        [OperationContract]
        List<ParamInventairesDto> LoadInventaire(string codeInventaire, string descriptionInventaire, bool isAdmin = true);

        #endregion

        #region Param Engagement
        [OperationContract]
        ParamEngagementDto InitParamEngment();
        [OperationContract]
        ParamEngagementDto GetListColonne(string codeTraite);
        [OperationContract]
        ParamEngmentColonneDto LoadColonne(string codeTraite, string code);
        [OperationContract]
        void SaveColonne(string codeTraite, string code, string libelle, string separation, string mode);
        [OperationContract]
        string DeleteColonne(string codeTraite, string code, string infoUser);
        #endregion

        #region Paramétrage IS

        [OperationContract]
        List<LigneModeleISDto> GetISReferenciel(string referentielId = "", bool modeAutocomplete = false);

        [OperationContract]
        string SaveISReferenciel(LigneModeleISDto reference);

        [OperationContract]
        void SupprimerISReferenciel(string code);

        [OperationContract]
        string SaveISModeleDetails(ModeleISDto modele, string user);

        [OperationContract]
        string SaveISModeleLigne(ParamISLigneModeleDto ligne, string user);

        [OperationContract]
        void SupprimerModeleEtDependances(string modeleName);

        [OperationContract]
        List<ModeleISDto> GetISModeles(string modeleId = "");

        [OperationContract]
        List<ParamISLigneModeleDto> GetISModeleLignes(string modeleId, string dbMapCol, string ligneId);

        [OperationContract]
        string GetDbMapColFromLigneModele(string ligneModeleId);



        #endregion

        #region Impression

        [OperationContract]
        string GenererCP(string codeOffreContrat, int version, string type, string acteGestion, string chemin,
            string storagePrefixDirectory, int storageMaxFiles, int storageNumPosDirectory, string user, string wrkStation, string ipStation, string userAD,
            int switchModuleGestDoc);

        #endregion

        #region Familles
        [OperationContract]
        List<ParamFamilleDto> GetFamilles(string codeConcept, string codeFamille, string descriptionFamille, string additionalParam, string restriction);
        [OperationContract]
        ParamFamilleDto GetFamille(string codeConcept, string codeFamille, string additionalParam, string restriction);
        [OperationContract]
        List<ParametreDto> GetFamillesByConcept(string codeConcept, string additionalParam, string restriction);
        [OperationContract]
        List<ParametreDto> FamillesGet(string value, string mode, string additionalParam, string restriction);
        [OperationContract]
        List<ParamValeurDto> GetValeursByFamille(string codeConcept, string codeFamille, string additionalParam, string restriction);
        [OperationContract]
        string EnregistrerFamille(string mode, ParamFamilleDto famille, string additionalParam);
        [OperationContract]
        string SupprimerFamille(ParamFamilleDto famille);
        [OperationContract]
        ParamValeurDto GetValeur(string codeConcept, string codeFamille, string codeValeur, string additionalParam, string restriction);
        [OperationContract]
        string EnregistrerValeur(string mode, ParamValeurDto valeur, string additionalParam);
        [OperationContract]
        string SupprimerValeur(ParamValeurDto valeur);

        [OperationContract]
        string CheckConceptFamille(string codeConcept, string codeFamille);
        #endregion

        #region Concepts
        [OperationContract]
        List<ParametreDto> LoadListConcepts(string codeConcept, string descriptionConcept, bool modeAutocomplete, bool isAdmin);

        [OperationContract]
        List<ParametreDto> EnregistrerConcept(string mode, ParametreDto concept);

        [OperationContract]
        string SupprimerConcept(ParametreDto concept);

        #endregion

        #region Filtres

        [OperationContract]
        List<ModeleFiltreLigneDto> LoadListFiltres(string codeFiltre, string descriptionFiltre, string IdFiltre);

        [OperationContract]
        ModeleDetailsFiltreDto GetFiltreDetails(Int64 idFiltre);

        [OperationContract]
        List<ParametreDto> GetBranchesFiltre();

        [OperationContract]
        List<ParametreDto> GetCiblesFiltre(string codeBranche);

        [OperationContract]
        List<ModeleFiltreLigneDto> EnregistrerFiltre(string mode, ModeleFiltreLigneDto filtre, string user);

        [OperationContract]
        List<ModeleLigneBrancheCibleDto> EnregistrerPaireBrancheCible(string mode, Int64 idFiltre, ModeleLigneBrancheCibleDto paire, string user);

        [OperationContract]
        string SupprimerFiltre(ModeleFiltreLigneDto filtre);

        [OperationContract]
        string SupprimerPaireBrancheCible(ModeleLigneBrancheCibleDto paire);

        [OperationContract]
        List<ModeleLigneBrancheCibleDto> GetListeBrancheCible(Int64 idFiltre, Int64? idPaire, string branche, string cible);

        #endregion

        #region Type Valeur (code)

        [OperationContract]
        List<ModeleLigneTypeValeurDto> LoadListTypesValeur(string codeTypeValeur, string descriptionTypeValeur);

        [OperationContract]
        List<ModeleLigneTypeValeurDto> EnregistrerTypeValeur(string mode, ModeleLigneTypeValeurDto typeValeur, string user);

        [OperationContract]
        void SupprimerTypeValeur(string mode, string code);

        [OperationContract]
        ModeleDetailsTypeValeurDto GetTypeValeurDetails(string codeTypeValeur);

        [OperationContract]
        List<ModeleLigneTypeValeurCompatibleDto> GetListeReferentielTypeValeurComp(string typeValeur);

        [OperationContract]
        void EnregistrerTypeValeurComp(string mode, string typeValeurId, string typeValeurCompId, string typeValeurCompCode, string user);

        [OperationContract]
        List<ModeleLigneTypeValeurCompatibleDto> GetListeTypeValeurComp(string codeTypeValeur, string codeTypeValeurComp, string idTypeValeurComp);

        [OperationContract]
        bool IsValueFree(string codeValeur);

        #endregion

        #region Garanties
        [OperationContract]
        List<ParamGarantieDto> GetGaranties(string code, string designation, string additionalParam, bool modeAutocomplete);
        [OperationContract]
        ParamGarantieDto GetGarantie(string code, string additionalParam);
        [OperationContract]
        ParamGarantieListesDto GetParamGarantieListes();
        [OperationContract]
        string EnregistrerGarantie(ParamGarantieDto garantie, string mode, string additionalParam);
        [OperationContract]
        string SupprimerGarantie(string codeGarantie, string additionalParam);
        [OperationContract]
        List<TypeValeurDto> GetGarTypesValeur(string codeGarantie, string additionalParam);
        [OperationContract]
        TypeValeurDto GetGarTypeValeurById(int id, string codeGarantie, string additionalParam);
        [OperationContract]
        List<ParametreDto> LoadGarListTypesValeur(string codeGarantie, string id);
        [OperationContract]
        string EnregistrerGarTypeValeur(TypeValeurDto typeValeur, string mode, string additionalParam);
        [OperationContract]
        List<FamilleReassuranceDto> GetGarFamillesReassurance(string codeGarantie, string additionalParam);
        [OperationContract]
        List<ParametreDto> LoadListBranches();


        /// <summary>
        /// Call the Business Tier to  Load the list of Famille as specified in the spec using for tCon = "REASS" and tFam = "GARAN"
        /// </summary>
        /// <param name="branche"></param>
        /// <param name="cible"></param>
        /// <param name="tCon"></param>
        /// <param name="tFam"></param>
        /// <param name="tPca1"></param>
        /// <param name="tCod"></param>
        /// <param name="notIn"></param>
        /// <param name="isBO"></param>
        /// <param name="tPcn2"></param>
        /// <returns>List of Param dto but just two fields are really need{CODE;LIBELLE}</returns>
        [OperationContract]
        List<ParametreDto> LoadListFamilles(string branche, string cible, string tCon, string tFam, string tPca1 = null, List<String> tCod = null, bool notIn = false, bool isBO = false, string tPcn2 = null);

        [OperationContract]
        List<ParametreDto> LoadListSousBranches(string codeBranche);
        [OperationContract]
        List<ParametreDto> LoadListCategories(string codeBranche, string codeSousBranche);
        [OperationContract]
        string GetGarFamille(string codeCategorie);
        [OperationContract]
        string EnregistrerFamilleReassurance(FamilleReassuranceDto familleReassurance, FamilleReassuranceDto familleReassuranceAncienne, string mode, string additionalParam);
        [OperationContract]
        string SupprimerGarTypeValeur(string codeGarantie, int id, string additionalParam);
        [OperationContract]
        string SupprimerFamilleReassurance(string codeGarantie, string codeBranche, string codeSousBranche, string codeCategorie, string codeFamille, string additionalParam);
        [OperationContract]
        List<OffreInventaireGarantieDto> GetListOffreInventaireByGarantie(string codeGarantie);//permet de récuperer les offres/contrats contenant des inventaires de la garantie ayant le code : codeGarantie
        [OperationContract]
        List<GarTypeRegulDto> AddTypeRegul(string codeGarantie, string codeTypeRegul);
        [OperationContract]
        List<GarTypeRegulDto> DeleteTypeRegul(string codeGarantie, string codeTypeRegul);
        [OperationContract]
        bool IsTypeRegulAssociated(string codeGarantie, string codeTypeRegul);
        #endregion

        #region Templates

        [OperationContract]
        List<ModeleLigneTemplateDto> LoadListTemplates(Int64 idTemplate, string codeTemplate, string descriptionTemplate, string typeTemplate, string lienCible, string branche, bool modeAutocomplete, bool existOnly);

        [OperationContract]
        List<ModeleLigneTemplateDto> LoadListTemplatesCNVA(string codeTemplate, string type, bool modeAutocomplete, bool existOnly);

        [OperationContract]
        string EnregistrerTemplate(string mode, ModeleLigneTemplateDto template, string user);

        [OperationContract]
        ModeleLigneTemplateDto GetDetailsTemplate(Int64 idTemplate);


        [OperationContract]
        EditTemplateDto GetCibleTemplate(string idCible, string cible);

        [OperationContract]
        void UpdateCibleTemplate(string idTemp, bool isChecked);

        [OperationContract]
        EditTemplateDto DelCibleTemplate(string idCible, string idTemp);

        [OperationContract]
        EditTemplateDto AssociateCibleTemplate(string idCible, string idTemp);

        [OperationContract]
        bool ExistCanevas(string idTemplate);

        [OperationContract]
        string SupprimerTemplate(Int64 idTemplate);

        [OperationContract]
        string GetParamTemplate(string idTemp, string tempFlag);

        [OperationContract]
        OffreDto GetDefaultTemplateOffre(string type, string codeCible, string codeBranche, ModeConsultation modeNavig);

        [OperationContract]
        ContratInfoBaseDto GetDefaultTemplateContrat(string type, string codeAvn, string codeCible, string codeBranche, ModeConsultation modeNavig);

        [OperationContract]
        void RegenerateCanevas(string user, bool totalRegeneration, string canevas = "%CV%");

        [OperationContract]
        void CopyCanevas(String source, String cible, String user);

        #endregion

        #region Menus Contextuels

        [OperationContract]
        List<UsersContextMenuDto> GetAllUsersContextMenu(string appl);

        [OperationContract]
        List<ContextMenuPlatDto> GetAllUsersFlatContextMenu(string appl);

        #endregion

        #region Gestion Nomenclature

        [OperationContract]
        GestionNomenclatureDto LoadInfoNomenclature();
        [OperationContract]
        GestionNomenclatureDto LoadListNomenclature(string typologie, string branche, string cible);
        [OperationContract]
        NomenclatureDto OpenNomenclature(string idNomenclature, string typologie);
        [OperationContract]
        string SaveNomenclature(string idNomenclature, string codeNomenclature, string ordreNomenclature, string libelleNomenclature, string typologie);
        [OperationContract]
        void DeleteNomenclature(string idNomenclature);

        #endregion

        #region Gestion grille nomenclature

        [OperationContract]
        List<GrilleDto> LoadInfoGestionGrille(string searchGrille);
        [OperationContract]
        GrilleDto OpenGrille(string idGrille);
        [OperationContract]
        string SaveGrille(string codeGrille, string libelleGrille, int newGrille);
        [OperationContract]
        void DeleteGrille(string codeGrille);
        [OperationContract]
        string SaveLineGrille(string codeGrille, string libelleGrille, string newGrille,
            string typologie, string libTypologie, string lienTypologie, string ordreTypologie);
        [OperationContract]
        void DeleteLineGrille(string codeGrille, string ordreTypologie);
        [OperationContract]
        GrilleDto OpenSelectionValeur(string codeGrille, string typoGrille, string niveau);
        [OperationContract]
        void SaveValeurs(string codeGrille, string typologie, string niveau, string niveauMere,
            string selVal1, string selVal2, string selVal3, string selVal4, string selVal5,
            string selVal6, string selVal7, string selVal8, string selVal9, string selVal10);
        [OperationContract]
        GrilleDto SearchValeurNomenclature(string codeGrille, string typologie, string idMere, string searchTerm);
        [OperationContract]
        GrilleDto LoadValeurs(string codeGrille, string idMere, string niveau);
        [OperationContract]
        TypologieDto LoadListValeurs(string codeGrille, string idMere, string niveau);
        [OperationContract]
        TypologieDto ReloadListValeurs(string codeGrille, string idMere, string niveau);

        #endregion

        #region Gest Utilisateurs

        [OperationContract]
        List<UtilisateurBrancheDto> GetUtilisateurBranchesByCriteria(KUSRDRT_COl criteriaCol, string criteriaValue, WHERE_OPER comapreOpertor);

        [OperationContract]
        UtilisateurBrancheDto GetUtilisateurBrancheDto(string utilisateur, string branche);

        [OperationContract]
        List<UtilisateurBrancheDto> LoadUtilisateurBrancheDtos();

        [OperationContract]
        List<UtilisateurBrancheDto> GetUtilisateurBrancheDtos(string utilisateur, string branche, string typeDroit);

        [OperationContract]
        void AjouterUtilisateurBranche(UtilisateurBrancheDto utilisateurBranche);

        [OperationContract]
        void ModifierUtilisateurBranche(UtilisateurBrancheDto utilisateurBranche, string newTypeDroit);

        [OperationContract]
        void SupprimerUtilisateurBranche(UtilisateurBrancheDto utilisateurBranche);

        #endregion

        #region Stat offre/Contrat
        [OperationContract]
        List<StatOffreContratDto> GetStatOffreContrat(StatOffreContratFiltreDto filtre);
        #endregion

        #region Stat Clauses libres
        [OperationContract]
        List<StatClausesLibresDto> GetStatClausesLibres(ParamRecherClausLibDto paramrecherche);

        #endregion

        #region Log ParamCibleRecup
        [OperationContract]
        List<LogParamCibleRecupDto> GetLogParamCibleRecup(LogParamCibleRecupDto filtre);
        #endregion

        #region Frais Accessoires
        [OperationContract]
        List<FraisAccessoiresEngDto> GetFraisAccessoires(FraisAccessoiresEngDto filtre, bool likeCategorie);

        [OperationContract]
        void InsertFraisAccessoires(FraisAccessoiresEngDto toSave);

        [OperationContract]
        void UpdateFraisAccessoires(FraisAccessoiresEngDto filtre, FraisAccessoiresEngDto toSave);

        [OperationContract]
        void DeleteFraisAccessoires(FraisAccessoiresEngDto filtre);
        #endregion

        #region Informations Base de données

        [OperationContract]
        IList<ColumnInfoDto> GetTableDescription(string env, string tableName);

        #endregion

        #region Recherche Offre Rapide

        [OperationContract]
        OffreRapideResultDto GetOffresRapides(OffreRapideFiltreDto filtre);

        [OperationContract]
        RechercheOffresRapideSaisieDto GetRechercheOffresRapideReferentiel();


        #endregion

        #region LogPerf

        [OperationContract]
        List<LogPerf> GetLogPerfs(DateTime? startDate, DateTime? endDate);

        #endregion

        //[OperationContract]
        //void RestartBridge();
        //[OperationContract]
        //void UpdateScriptsForBridge();

    }
}
