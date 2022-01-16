using Albingia.Kheops.OP.Application.Port.Driver;
//using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.IOFile;
using OP.DataAccess;
using OP.Services.BLServices;
using OP.Services.SaisieCreationOffre;
using OP.Services.WSKheoBridge;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Bloc;
using OP.WSAS400.DTO.Categorie;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Ecran.Rercherchesaisie;
using OP.WSAS400.DTO.FraisAccessoires;
using OP.WSAS400.DTO.GarantieModele;
using OP.WSAS400.DTO.GestionNomenclature;
using OP.WSAS400.DTO.GestUtilisateurs;
using OP.WSAS400.DTO.Logging;
using OP.WSAS400.DTO.MenuContextuel;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.OffresRapide;
using OP.WSAS400.DTO.ParametreCibles;
using OP.WSAS400.DTO.ParametreEngagement;
using OP.WSAS400.DTO.ParametreFamilles;
using OP.WSAS400.DTO.ParametreFiltre;
using OP.WSAS400.DTO.ParametreGaranties;
using OP.WSAS400.DTO.ParametreInventaires;
using OP.WSAS400.DTO.ParametreTypeValeur;
using OP.WSAS400.DTO.ParamIS;
using OP.WSAS400.DTO.ParamTemplates;
using OP.WSAS400.DTO.Stat;
using OP.WSAS400.DTO.VerouillageOffres;
using OP.WSAS400.DTO.Volet;
using OPServiceContract.IAdministration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel.Activation;

namespace OP.Services.Administration
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Administration : IVoletsBlocsCategories
    {
        private readonly IAffairePort affaireService;
        public Administration(IAffairePort affaireService) {
            this.affaireService = affaireService;
        }

        #region Méthodes Transverses


        public string EnregistrerModeleByCible(string codeId, string codeIdBloc, string dateApp, string codeTypo, string codeModele, string user)
        {
            return GarantieModeleRepository.EnregistrerModeleByCible(codeId, codeIdBloc, dateApp, codeTypo, codeModele, user);
        }
        public List<ParametreDto> ObtenirParametres(string contexte, string famille)
        {
            return CommonRepository.GetParametres(string.Empty, string.Empty, contexte, famille);
            //return ObjectMapperManager.DefaultInstance.GetMapper<List<Parametre>, List<ParametreDto>>().Map(ReferenceRepository.RechercherParametres(contexte, famille));

        }



        /// <summary>
        /// Brancheses the get.
        /// </summary>
        /// <returns></returns>
        public List<BrancheDto> BranchesGet()
        {
            return new PoliceServices().BranchesGet();
        }

        /// <summary>
        /// Modeleses the get list.
        /// </summary>
        /// <returns></returns>
        public List<GarantieModeleDto> ModelesGetList()
        {


            //using (var easyComConnectionHelper = new EasyComConnectionHelper())
            //{
            //AccesDataManager._connectionHelper = easyComConnectionHelper;

            return GarantieModeleRepository.ObtenirModeles();
            //}


        }

        /// <summary>
        /// Garanties the modeles get by bloc.
        /// </summary>
        /// <param name="codeId"></param>
        /// <returns></returns>
        public List<GarantieModeleDto> GarantieModelesGetByBloc(string codeId)
        {
            return GarantieModeleRepository.RechercherGarantieModelesByBloc(codeId);
        }

        /// <summary>
        /// Enregistrers the modele by categorie.
        /// </summary>
        /// <param name="codeId">The code id.</param>
        /// <param name="codeIdBloc">The code id bloc.</param>
        /// <param name="dateApp">The date app.</param>
        /// <param name="codeTypo">The code typo.</param>
        /// <param name="codeModele">The code modele.</param>
        /// <param name="user">The user.</param>
        //public void EnregistrerModeleByCategorie(string codeId, string codeIdBloc, string dateApp, string codeTypo, string codeModele, string user)
        //{

        //  GarantieModeleRepository.EnregistrerModeleByCategorie(codeId, codeIdBloc, dateApp, codeTypo, codeModele, user);

        //}

        /// <summary>
        /// Supprimers the modele by categorie.
        /// </summary>
        /// <param name="codeId">The code id.</param>
        public string SupprimerModeleByCategorie(string codeId, string infoUser)
        {
            return GarantieModeleRepository.SupprimerModeleByCategorie(codeId, infoUser);
        }

        #endregion

        #region Volet
        public List<DtoVolet> VoletsGetByCible(string codeId, string codeBranche)
        {
            return VoletRepository.RechercherVoletsByCible(codeId, codeBranche);
        }


        public void EnregistrerVoletByCible(string codeId, string codeBranche, string codeCible, string codeIdCible, string codeVolet, string codeIdVolet, string codeCaractere, double ordreVolet, string user)
        {
            VoletRepository.EnregistrerVoletByCible(codeId, codeBranche, codeCible, codeIdCible, codeVolet, codeIdVolet, codeCaractere, ordreVolet, user);
        }

        //public List<DtoVolet> VoletsGetByCategorie(string codeId)
        //{
        //    return VoletRepository.RechercherVoletsByCategorie(codeId);
        //}

        public List<DtoVolet> VoletsGet(string code, string description)
        {
            List<DtoVolet> voletsDto;
            voletsDto = VoletRepository.GetVolets(code, description);
            return voletsDto;
        }

        public List<DtoVolet> VoletsGetList()
        {
            return VoletRepository.ObtenirVolets();
        }

        public DtoVolet VoletInfoGet(string code)
        {
            DtoVolet voletDto;
            voletDto = VoletRepository.ObtenirVolet(code);

            voletDto.CategoriesVolet = CategorieRepository.ObtenirBrancheParVolet(code);
            voletDto.Categories = CategorieRepository.ObtenirCategories();
            voletDto.Branches = CommonRepository.GetParametres(string.Empty, string.Empty, "GENER", "BRCHE");

            return voletDto;
        }


        public void VoletInfoSet(string codeId, string code, string description, string branche, string isVoletGeneral, string isVoletCollapse, string update = "0", string user = "")
        {
            VoletRepository.EnregistrerVolet(codeId, code, description, branche, isVoletGeneral, isVoletCollapse, update, user);
        }

        public string SupprimerVolet(string code, string infoUser)
        {
            return VoletRepository.SupprimerVolet(code, infoUser);
        }

        public void EnregistrerVoletByCategorie(string codeId, string codeBranche, string codeCategorie, string codeIdCategorie, string codeVolet, string codeIdVolet, string codeCaractere, string user)
        {
            VoletRepository.EnregistrerVoletByCategorie(codeId, codeBranche, codeCategorie, codeIdCategorie, codeVolet, codeIdVolet, codeCaractere, user);
        }

        public string SupprimerVoletByCategorie(string codeId, string codeBranche, string codeIdCible, string infoUser)
        {
            return VoletRepository.SupprimerVoletByCategorie(codeId, codeBranche, codeIdCible, infoUser);
        }

        #endregion

        #region Bloc
        public void EnregistrerBlocByCible(string codeId, string codeBranche, string codeCible, string codeVolet, string codeIdVolet, string codeBloc, string codeIdBloc, string codeCaractere, double ordreBloc, string user)
        {
            BlocRepository.EnregistrerBlocByCible(codeId, codeBranche, codeCible, codeVolet, codeIdVolet, codeBloc, codeIdBloc, codeCaractere, ordreBloc, user);
        }

        public List<BlocDto> BlocsGetByVolet(string codeId)
        {
            return BlocRepository.RechercherBlocsByVolet(codeId);
        }

        public List<BlocDto> BlocsGet(string code, string description)
        {
            return BlocRepository.GetBlocs(code, description);
        }

        public List<BlocDto> BlocsGetList()
        {
            return BlocRepository.ObtenirBlocs();
        }

        public BlocDto BlocInfoGet(string code)
        {
            return BlocRepository.ObtenirBloc(code);
        }

        public void BlocInfoSet(string codeId, string code, string description, string update = "0", string user = "")
        {
            BlocRepository.EnregistrerBloc(codeId, code, description, update, user);
        }

        public string SupprimerBloc(string code, string infoUser)
        {
            return BlocRepository.SupprimerBloc(code, infoUser);
        }

        public void EnregistrerBlocByCategorie(string codeId, string codeBranche, string codeCategorie, string codeVolet, string codeIdVolet, string codeBloc, string codeIdBloc, string codeCaractere, string user)
        {

            //AccesDataManager._connectionHelper = easyComConnectionHelper;
            BlocRepository.EnregistrerBlocByCategorie(codeId, codeBranche, codeCategorie, codeVolet, codeIdVolet, codeBloc, codeIdBloc, codeCaractere, user);

        }

        public string SupprimerBlocByCategorie(string codeId, string codeIdVolet, string infoUser)
        {
            return BlocRepository.SupprimerBlocByCategorie(codeId, codeIdVolet, infoUser);
        }

        public List<BlocDto> GetListeBlocsIncompatiblesAssocies(string codeIdBloc, string typeBloc)
        {
            return BlocRepository.GetListeBlocsIncompatiblesAssocies(codeIdBloc, typeBloc);
        }

        public List<BlocDto> GetListeBlocsReferentielIncompatiblesAssocies(string codeIdBloc, string typeBloc)
        {
            return BlocRepository.GetListeBlocsReferentielIncompatiblesAssocies(codeIdBloc, typeBloc);
        }

        public string EnregistrerBlocIncompatibleAssocie(string idAssociation, string mode, string idBloctraite, string idBlocIncompatible, string typeBloc, string user)
        {
            return BlocRepository.EnregistrerBlocIncompatibleAssocie(idAssociation, mode, idBloctraite, idBlocIncompatible, typeBloc, user);
        }

        #endregion

        #region Categorie
        public CategorieDto ObtenirCategorieByCode(string code)
        {
            return CategorieRepository.ObtenirCategorieByCode(code);
        }

        #endregion

        #region Cible
        public List<CibleDto> CiblesGet(string codeBranche)
        {
            return CibleRepository.RechercherCibles(codeBranche);
        }

        public List<ParamCiblesDto> LoadListCible(string codeCiblePattern, string libelleCiblePattern)
        {
            return ParamCiblesRepository.LoadListCible(codeCiblePattern, libelleCiblePattern);
        }

        public List<ParamListCibleBranchesDto> GetCibleBranches(Int64 codeCibleId)
        {
            return ParamCiblesRepository.GetCibleBranches(codeCibleId);
        }

        public List<ParametreDto> GetBranches(int codeCibleId)
        {
            return ParamCiblesRepository.GetBranches(codeCibleId);
        }
        public List<ParametreDto> GetBranchesEdit(int codeCibleId, string codeBranche)
        {
            return ParamCiblesRepository.GetBranchesEdit(codeCibleId, codeBranche);
        }

        public List<ParametreDto> GetListeBranches()
        {
            return CommonRepository.GetParametres(string.Empty, string.Empty, "GENER", "BRCHE", tCod: new List<string> { "PP", "ZZ" }, notIn: true, tPcn2: "1");
        }

        public List<ParametreDto> GetSousBranches(string codeBranche)
        {
            return ParamCiblesRepository.GetSousBranches(codeBranche);
        }

        public List<ParametreDto> GetCategories(string codeBranche, string codeSousBranche)
        {
            return ParamCiblesRepository.GetCategories(codeBranche, codeSousBranche);
        }

        public int AjouterBSCCible(string guidIdCible, string codecible, string codeBranche, string codeSousBranche, string codeCategorie)
        {
            return ParamCiblesRepository.AjouterBSCCible(guidIdCible, codecible, codeBranche, codeSousBranche, codeCategorie);
        }
        public int AjouterCible(string code, string description, string grille, string famille, string concept, string user)
        {
            return ParamCiblesRepository.AjouterCible(code, description, grille, famille, concept, user);
        }
        public void ModifierCible(string guidIdCible, string descriptionCible, string grille, string famille, string concept, string user)
        {
            ParamCiblesRepository.ModifierCible(guidIdCible, descriptionCible, grille, famille, concept, user);
        }
        public string SupprimerCible(int guidIdCible, string infoUser)
        {
            return ParamCiblesRepository.SupprimerCible(guidIdCible, infoUser);
        }
        public string SupprimerCibleBSCByGuidId(string guidId, string infoUser)
        {
            return ParamCiblesRepository.SupprimerCibleBSCByGuidId(guidId, infoUser);
        }
        public int ModifierCibleBSCByGuidId(string guidIdCible, string codeCible, string codeBranche, string codeSousBranche, string codeCategorie)
        {
            return ParamCiblesRepository.ModifierCibleBSCByGuidId(guidIdCible, codeCible, codeBranche, codeSousBranche, codeCategorie);
        }
        public ParamCiblesDto GetCible(string guidId)
        {
            return ParamCiblesRepository.GetCible(guidId);
        }
        public bool ExisteCiblePortefeuille(int guidId)
        {
            return ParamCiblesRepository.ExisteCiblePortefeuille(guidId);
        }

        public List<ParametreDto> GetGrilles()
        {
            return ParamCiblesRepository.GetGrilles();
        }
        #endregion

        #region Inventaire

        public List<ParamInventairesDto> LoadInventaire(string codeInventaire, string descriptionInventaire, bool isAdmin = true)
        {
            return ParamInventairesRepository.LoadInventaire(codeInventaire, descriptionInventaire, isAdmin = true);
        }
        public List<ParametreDto> GetFiltres()
        {
            return ParamInventairesRepository.GetFiltres();
        }

        public ParamInventairesListDto ModifierInventaire(string guidIdCible, string codeInventaire, string descriptionCible, int kagtmap, string codeFiltre)
        {
            return ParamInventairesRepository.ModifierInventaire(guidIdCible, codeInventaire, descriptionCible, kagtmap, codeFiltre);
        }

        public ParamInventairesListDto AjouterInventaire(string codeInventaire, string descriptionInventaire, int kagtmap, string codeFiltre)
        {
            return ParamInventairesRepository.AjouterInventaire(codeInventaire, descriptionInventaire, kagtmap, codeFiltre);
        }

        public ParamInventairesListDto SupprimerInventaire(int guidIdInventaire)
        {
            return ParamInventairesRepository.SupprimerInventaire(guidIdInventaire);
        }

        #endregion

        #region Verouillage Offres


        public void AjouterOffreVerouille(string sevice, string typ, string ipb, int alx, int avn, int kavsua, int kavnum, string kavsbr, string kavactg, string kavact, string kavverr, string user, string kavlib)
        {
            //VerouillageOffresRepository.AjouterOffreVerouille(sevice, typ, ipb, alx, avn, kavsua, kavnum, kavsbr, kavactg, kavact, kavverr, user, kavlib);
            if (this.affaireService.TryLockAffaire(new AffaireId { CodeAffaire = ipb, IsHisto = false, NumeroAliment = alx, NumeroAvenant = avn, TypeAffaire = typ.ParseCode<AffaireType>() }, kavact, kavactg) is Albingia.Kheops.DTO.VerrouAffaire verrou) {
                throw new Exception($"L'affaire est verrouillé par {verrou.User}");
            }
        }
        public List<OffreVerouilleeDto> GetOffresVerouillees(bool TypeOffre_O, bool TypeOffre_P, string NumOffre, string Version, string numAvn, string Utilisateur, string DateVerouillageDebut, string DateVerouillageFin)
        {
            return VerouillageOffresRepository.GetOffresVerouillees(TypeOffre_O, TypeOffre_P, NumOffre, Version, numAvn, Utilisateur, DateVerouillageDebut, DateVerouillageFin);
        }
        public void SupprimerOffreVerouillee(string NumOffre, string Version, string type, string numAvn, string user, string acteGestion, bool isAlimStat, bool isModifHorsAvn, bool isAnnul)
        {
            VerouillageOffresRepository.SupprimerOffreVerouillee(NumOffre, Version, type, numAvn, user, acteGestion, isAlimStat, isModifHorsAvn, isAnnul);
        }
        public bool IsOffreVeruouille(string codeOffre, string version, string type, string numAvn)
        {
            return VerouillageOffresRepository.IsOffreVeruouille(codeOffre, version, type, numAvn);
        }
        public string GetUserVerrou(string codeOffre, string version, string type, string numAvn)
        {
            return VerouillageOffresRepository.GetUserVerrou(codeOffre, version, type, numAvn);
        }
        #endregion

        #region Paramètrage Engagement

        public ParamEngagementDto InitParamEngment()
        {
            return ParamEngagementRepository.InitParamEngagement();

            //return BLParamEngagement.InitParamEngagement();
        }

        public ParamEngagementDto GetListColonne(string codeTraite)
        {
            return ParamEngagementRepository.GetListColonne(codeTraite);

            //return BLParamEngagement.GetListColonne(codeTraite);
        }

        public ParamEngmentColonneDto LoadColonne(string codeTraite, string code)
        {
            return ParamEngagementRepository.LoadColonne(codeTraite, code);

            //return BLParamEngagement.LoadColonne(codeTraite, code);
        }

        public void SaveColonne(string codeTraite, string code, string libelle, string separation, string mode)
        {
            ParamEngagementRepository.SaveColonne(codeTraite, code, libelle, separation, mode);

            //BLParamEngagement.SaveColonne(codeTraite, code, libelle, separation, mode);
        }

        public string DeleteColonne(string codeTraite, string code, string infoUser)
        {
            return ParamEngagementRepository.DeleteColonne(codeTraite, code, infoUser);

            //return BLParamEngagement.DeleteColonne(codeTraite, code, infoUser);
        }

        #endregion

        #region Paramétrage IS

        public List<LigneModeleISDto> GetISReferenciel(string referentielId = "", bool modeAutocomplete = false)
        {
            return ParamISRepository.GetISReferenciel(referentielId, modeAutocomplete);
        }

        public string SaveISReferenciel(LigneModeleISDto reference)
        {
            string toReturn = ParamISRepository.SaveISReferenciel(reference);
            //Mise à jour du cache
            Common.CommonOffre.InitISModeles(true);
            var lignesModeles = ParamISRepository.GetParamISLignesInfo("");
            Common.CommonOffre.SetWsDataCache(Common.CommonOffre.WSKEYCACHE_ISMODL, lignesModeles);
            return toReturn;
        }

        public void SupprimerISReferenciel(string code)
        {
            ParamISRepository.SupprimerISReferenciel(code);
        }

        public string SaveISModeleDetails(ModeleISDto modele, string user)
        {
            return ParamISRepository.SaveISModeleDetails(modele, user);
        }

        public string SaveISModeleLigne(ParamISLigneModeleDto ligne, string user)
        {
            string toReturn = ParamISRepository.SaveISModeleLigne(ligne, user);
            return toReturn;
        }

        public void SupprimerModeleEtDependances(string modeleName)
        {
            Common.CommonOffre.InitWsCache(Common.CommonOffre.WSKEYCACHE_ISMODL);
            ParamISRepository.SupprimerModeleEtDependances(modeleName);
        }

        public List<ModeleISDto> GetISModeles(string modeleId = "")
        {
            return ParamISRepository.GetISModeles(modeleId);
        }

        public List<ParamISLigneModeleDto> GetISModeleLignes(string modeleId, string dbMapCol, string ligneId)
        {
            //var lignesModeles = new ParamISLigneModeleDto();
            var lignesModeles = Common.CommonOffre.GetWsDataCache<List<ParamISLigneInfoDto>>(Common.CommonOffre.WSKEYCACHE_ISMODL);
            if (lignesModeles == null)
            {
                Common.CommonOffre.InitWsCache(Common.CommonOffre.WSKEYCACHE_ISMODL);
                lignesModeles = ParamISRepository.GetParamISLignesInfo("");
                Common.CommonOffre.SetWsDataCache(Common.CommonOffre.WSKEYCACHE_ISMODL, lignesModeles);
            }
            return ParamISRepository.GetISModeleLignes(modeleId, dbMapCol, ligneId);
        }

        public string GetDbMapColFromLigneModele(string ligneModeleId)
        {
            return ParamISRepository.GetDbMapColFromLigneModele(ligneModeleId);
        }
        #endregion

        #region Impression

        public string GenererCP(string codeOffreContrat, int version, string type, string acteGestion, string chemin,
            string storagePrefixDirectory, int storageMaxFiles, int storageNumPosDirectory, string user, string wrkStation, string ipStation, string userAD,
            int switchModuleGestDoc)
        {
            var ip = string.Empty;
#if DEBUG
            wrkStation = Environment.MachineName;
            ip = System.Net.Dns.GetHostName();
#endif
            using (var serviceContext = new WSKheoBridge.KheoBridge())
            {
                var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
                if (!string.IsNullOrEmpty(kheoBridgeUrl))
                    serviceContext.Url = kheoBridgeUrl;

                var path = FinOffreRepository.ReloadPathFile(AlbConstantesMetiers.TypologieDoc.CP.ToString());
                if (string.IsNullOrEmpty(path) || !IOFileManager.IsExistDirectory(path))
                    throw new AlbFoncException("Le dossier cible n'existe pas", trace: true, sendMail: true, onlyMessage: true);
                var offerFolder = string.Format(@"{0}_{1}_{2}", codeOffreContrat.Trim(), version.ToString("D4"), type);
                path = path + offerFolder;
                if (!IOFileManager.IsExistDirectory(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                var ret = string.Empty;// serviceContext.GenererCp(acteGestion, type, codeOffreContrat.PadLeft(9, ' '), version, path);

                if (switchModuleGestDoc == 1)
                {
                    var pushDto1 = new KheoPushDto
                    {
                        Fonction = PushFonction.GENERER_CP,
                        Adresse_IP = ipStation,
                        UserAD = userAD,
                        TypContrat = type,
                        NumeroContrat = codeOffreContrat,
                        Aliment = version,
                        Chemin = path,
                        NomFichier = ret // ret.Replace(path + "\\", string.Empty)
                    };
                    serviceContext.ExecuterPush(pushDto1);

                    CommonRepository.SetTraceLog(string.Empty, string.Empty, string.Empty, 0, string.Empty, "PUSHDOC", DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture), wrkStation + '_' + ipStation);
                }

                return ret;
            }
        }

        #endregion

        #region Familles
        public List<ParamFamilleDto> GetFamilles(string codeConcept, string codeFamille, string descriptionFamille, string additionalParam, string restriction)
        {
            return ParamConceptFamilleCodeRepository.GetFamilles(codeConcept, codeFamille, descriptionFamille, additionalParam, restriction);
        }
        public ParamFamilleDto GetFamille(string codeConcept, string codeFamille, string additionalParam, string restriction)
        {
            return ParamConceptFamilleCodeRepository.GetFamille(codeConcept, codeFamille, additionalParam, restriction);
        }
        public List<ParametreDto> GetFamillesByConcept(string codeConcept, string additionalParam, string restriction)
        {
            return ParamConceptFamilleCodeRepository.GetFamillesByConcept(codeConcept, additionalParam, restriction);
        }
        public List<ParametreDto> FamillesGet(string value, string mode, string additionalParam, string restriction)
        {
            return ParamConceptFamilleCodeRepository.FamillesGet(value, mode, additionalParam, restriction);
        }
        public string EnregistrerFamille(string mode, ParamFamilleDto famille, string additionalParam)
        {
            return ParamConceptFamilleCodeRepository.EnregistrerFamille(mode, famille, additionalParam);
        }
        public string SupprimerFamille(ParamFamilleDto famille)
        {
            return ParamConceptFamilleCodeRepository.SupprimerFamille(famille);
        }
        public List<ParamValeurDto> GetValeursByFamille(string codeConcept, string codeFamille, string additionalParam, string restriction)
        {
            return ParamConceptFamilleCodeRepository.GetValeursByFamille(codeConcept, codeFamille, additionalParam, restriction);
        }
        public ParamValeurDto GetValeur(string codeConcept, string codeFamille, string codeValeur, string additionalParam, string restriction)
        {
            return ParamConceptFamilleCodeRepository.GetValeur(codeConcept, codeFamille, codeValeur, additionalParam, restriction);
        }
        public string EnregistrerValeur(string mode, ParamValeurDto valeur, string additionalParam)
        {
            return ParamConceptFamilleCodeRepository.EnregistrerValeur(mode, valeur, additionalParam);
        }
        public string SupprimerValeur(ParamValeurDto valeur)
        {
            return ParamConceptFamilleCodeRepository.SupprimerValeur(valeur);
        }

        public string CheckConceptFamille(string codeConcept, string codeFamille)
        {
            return ParamConceptFamilleCodeRepository.CheckConceptFamille(codeConcept, codeFamille);
        }
        #endregion

        #region Concepts

        public List<ParametreDto> LoadListConcepts(string codeConcept, string descriptionConcept, bool modeAutocomplete, bool isAdmin)
        {
            return ParamConceptFamilleCodeRepository.LoadListConcepts(codeConcept, descriptionConcept, modeAutocomplete, isAdmin);
        }

        public List<ParametreDto> EnregistrerConcept(string mode, ParametreDto concept)
        {
            return ParamConceptFamilleCodeRepository.EnregistrerConcept(mode, concept);
        }

        public string SupprimerConcept(ParametreDto concept)
        {
            return ParamConceptFamilleCodeRepository.SupprimerConcept(concept);
        }

        #endregion

        #region Filtres

        public List<ModeleFiltreLigneDto> LoadListFiltres(string codeFiltre, string descriptionFiltre, string idFiltre)
        {
            return ParamConceptFamilleCodeRepository.LoadListFiltres(codeFiltre, descriptionFiltre, idFiltre);
        }

        public ModeleDetailsFiltreDto GetFiltreDetails(Int64 idFiltre)
        {
            return ParamConceptFamilleCodeRepository.GetFiltreDetails(idFiltre);
        }

        public List<ParametreDto> GetBranchesFiltre()
        {
            return ParamConceptFamilleCodeRepository.GetBranchesFiltre();
        }

        public List<ParametreDto> GetCiblesFiltre(string codeBranche)
        {
            return ParamConceptFamilleCodeRepository.GetCiblesFiltre(codeBranche);
        }

        public List<ModeleFiltreLigneDto> EnregistrerFiltre(string mode, ModeleFiltreLigneDto filtre, string user)
        {
            return ParamConceptFamilleCodeRepository.EnregistrerFiltre(mode, filtre, user);
        }

        public List<ModeleLigneBrancheCibleDto> EnregistrerPaireBrancheCible(string mode, Int64 idFiltre, ModeleLigneBrancheCibleDto paire, string user)
        {
            return ParamConceptFamilleCodeRepository.EnregistrerPaireBrancheCible(mode, idFiltre, paire, user);
        }

        public string SupprimerFiltre(ModeleFiltreLigneDto filtre)
        {
            return ParamConceptFamilleCodeRepository.SupprimerFiltre(filtre);
        }

        public string SupprimerPaireBrancheCible(ModeleLigneBrancheCibleDto paire)
        {
            return ParamConceptFamilleCodeRepository.SupprimerPaireBrancheCible(paire);
        }

        public List<ModeleLigneBrancheCibleDto> GetListeBrancheCible(Int64 idFiltre, Int64? idPaire, string branche, string cible)
        {
            return ParamConceptFamilleCodeRepository.GetListeBrancheCible(idFiltre, idPaire, branche, cible);
        }

        #endregion

        #region Type Valeur (code)

        public List<ModeleLigneTypeValeurDto> LoadListTypesValeur(string codeTypeValeur, string descriptionTypeValeur)
        {
            return ParamConceptFamilleCodeRepository.LoadListTypesValeur(codeTypeValeur, descriptionTypeValeur);
        }

        public List<ModeleLigneTypeValeurDto> EnregistrerTypeValeur(string mode, ModeleLigneTypeValeurDto typeValeur, string user)
        {
            return ParamConceptFamilleCodeRepository.EnregistrerTypeValeur(mode, typeValeur, user);
        }

        public void EnregistrerTypeValeurComp(string mode, string typeValeurId, string typeValeurCompId, string typeValeurCompCode, string user)
        {
            ParamConceptFamilleCodeRepository.EnregistrerTypeValeurComp(mode, typeValeurId, typeValeurCompId, typeValeurCompCode, user);
        }

        public void SupprimerTypeValeur(string mode, string code)
        {
            ParamConceptFamilleCodeRepository.SupprimerTypeValeur(mode, code);
        }

        public ModeleDetailsTypeValeurDto GetTypeValeurDetails(string codeTypeValeur)
        {
            return ParamConceptFamilleCodeRepository.GetTypeValeurDetails(codeTypeValeur);
        }

        public List<ModeleLigneTypeValeurCompatibleDto> GetListeReferentielTypeValeurComp(string typeValeur)
        {
            return ParamConceptFamilleCodeRepository.GetListeReferentielTypeValeurComp(typeValeur);
        }

        public List<ModeleLigneTypeValeurCompatibleDto> GetListeTypeValeurComp(string codeTypeValeur, string codeTypeValeurComp, string idTypeValeurComp)
        {
            return ParamConceptFamilleCodeRepository.GetListeTypeValeurComp(codeTypeValeur, codeTypeValeurComp, idTypeValeurComp);
        }

        public bool IsValueFree(string codeValeur)
        {
            return ParamConceptFamilleCodeRepository.IsValueFree(codeValeur);
        }


        #endregion

        #region Garanties

        public List<ParamGarantieDto> GetGaranties(string code, string designation, string additionalParam, bool modeAutocomplete)
        {
            return ParamGarantieRepository.GetGaranties(code, designation, additionalParam, modeAutocomplete);
        }
        public ParamGarantieDto GetGarantie(string code, string additionalParam)
        {
            return ParamGarantieRepository.GetGarantie(code, additionalParam);
        }

        public ParamGarantieListesDto GetParamGarantieListes()
        {
            var ParamGarantieListesDto = ParamGarantieRepository.GetParamGarantieListes();
            ParamGarantieListesDto.TypesInventaire = ReferenceRepository.RechercherTypesInventaire();
            return ParamGarantieListesDto;
        }

        public string EnregistrerGarantie(ParamGarantieDto garantie, string mode, string additionalParam)
        {
            return ParamGarantieRepository.EnregistrerGarantie(garantie, mode, additionalParam);
        }

        public string SupprimerGarantie(string codeGarantie, string additionalParam)
        {
            return ParamGarantieRepository.SupprimerGarantie(codeGarantie, additionalParam);

        }

        public List<TypeValeurDto> GetGarTypesValeur(string codeGarantie, string additionalParam)
        {
            return ParamGarantieRepository.GetGarTypesValeur(codeGarantie, additionalParam);
        }

        public TypeValeurDto GetGarTypeValeurById(int id, string codeGarantie, string additionalParam)
        {
            return ParamGarantieRepository.GetGarTypeValeurById(id, codeGarantie, additionalParam);
        }

        public List<ParametreDto> LoadGarListTypesValeur(string codeGarantie, string id)
        {
            return ParamGarantieRepository.LoadGarListTypesValeur(codeGarantie, id);
        }

        public string EnregistrerGarTypeValeur(TypeValeurDto typeValeur, string mode, string additionalParam)
        {
            return ParamGarantieRepository.EnregistrerGarTypeValeur(typeValeur, mode, additionalParam);
        }

        public List<FamilleReassuranceDto> GetGarFamillesReassurance(string codeGarantie, string additionalParam)
        {
            return ParamGarantieRepository.GetGarFamillesReassurance(codeGarantie, additionalParam);
        }

        public List<ParametreDto> LoadListBranches()
        {
            return ParamGarantieRepository.LoadListBranches();
        }

        /// <summary>
        /// Call the Repository Tier to  Load the list of Famille as specified in the spec using for tCon = "REASS" and tFam = "GARAN"
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
        public List<ParametreDto> LoadListFamilles(string branche, string cible, string tCon, string tFam, string tPca1 = null, List<String> tCod = null, bool notIn = false, bool isBO = false, string tPcn2 = null)
        {
            return ParamGarantieRepository.LoadListFamilles(branche, cible, tCon, tFam, tPca1, tCod, notIn, isBO, tPcn2);
        }

        public List<ParametreDto> LoadListSousBranches(string codeBranche)
        {
            return ParamGarantieRepository.LoadListSousBranches(codeBranche);

        }

        public List<ParametreDto> LoadListCategories(string codeBranche, string codeSousBranche)
        {
            return ParamGarantieRepository.LoadListCategories(codeBranche, codeSousBranche);
        }

        public string GetGarFamille(string codeCategorie)
        {
            return ParamGarantieRepository.GetGarFamille(codeCategorie);
        }

        public string EnregistrerFamilleReassurance(FamilleReassuranceDto familleReassurance, FamilleReassuranceDto familleReassuranceAncienne, string mode, string additionalParam)
        {
            return ParamGarantieRepository.EnregistrerFamilleReassurance(familleReassurance, familleReassuranceAncienne, mode, additionalParam);
        }

        public string SupprimerGarTypeValeur(string codeGarantie, int id, string additionalParam)
        {
            return ParamGarantieRepository.SupprimerGarTypeValeur(codeGarantie, id, additionalParam);
        }

        public string SupprimerFamilleReassurance(string codeGarantie, string codeBranche, string codeSousBranche, string codeCategorie, string codeFamille, string additionalParam)
        {
            return ParamGarantieRepository.SupprimerFamilleReassurance(codeGarantie, codeBranche, codeSousBranche, codeCategorie, codeFamille, additionalParam);
        }
        public List<OffreInventaireGarantieDto> GetListOffreInventaireByGarantie(string codeGarantie)
        {
            return ParamGarantieRepository.GetListOffreInventaireByGarantie(codeGarantie);
        }

        public List<GarTypeRegulDto> AddTypeRegul(string codeGarantie, string codeTypeRegul)
        {
            ParamGarantieRepository.AddTypeRegul(codeGarantie, codeTypeRegul);
            return ParamGarantieRepository.GetTypeRegulByGarantie(codeGarantie);
        }

        public List<GarTypeRegulDto> DeleteTypeRegul(string codeGarantie, string codeTypeRegul)
        {
            ParamGarantieRepository.DeleteTypeRegul(codeGarantie, codeTypeRegul);
            return ParamGarantieRepository.GetTypeRegulByGarantie(codeGarantie);
        }

        public bool IsTypeRegulAssociated(string codeGarantie, string codeTypeRegul)
        {
            return ParamGarantieRepository.IsTypeRegulAssociated(codeGarantie, codeTypeRegul);
        }
        #endregion

        #region Templates

        public List<ModeleLigneTemplateDto> LoadListTemplates(Int64 idTemplate, string codeTemplate, string descriptionTemplate, string typeTemplate, string lienCible, string branche, bool modeAutocomplete, bool existOnly)
        {
            return ParamTemplatesRepository.LoadListTemplates(idTemplate, codeTemplate, descriptionTemplate, typeTemplate, lienCible, branche, modeAutocomplete, existOnly);
        }

        public List<ModeleLigneTemplateDto> LoadListTemplatesCNVA(string codeTemplate, string type, bool modeAutocomplete, bool existOnly)
        {
            return ParamTemplatesRepository.LoadListTemplatesCNVA(codeTemplate, type, modeAutocomplete, existOnly);
        }

        public ModeleLigneTemplateDto GetDetailsTemplate(Int64 idTemplate)
        {
            return ParamTemplatesRepository.GetDetailsTemplate(idTemplate);
        }

        public string EnregistrerTemplate(string mode, ModeleLigneTemplateDto template, string user)
        {
            return ParamTemplatesRepository.EnregistrerTemplate(mode, template, user);
        }

        public string SupprimerTemplate(Int64 idTemplate)
        {
            return ParamTemplatesRepository.SupprimerTemplate(idTemplate);
        }

        public EditTemplateDto GetCibleTemplate(string idCible, string cible)
        {
            EditTemplateDto model = new EditTemplateDto
            {
                Cible = cible,
                Templates = ParamTemplatesRepository.LoadListTemplates(0, null, null, null, idCible, null, false, false)
            };
            return model;
        }

        public void UpdateCibleTemplate(string idTemp, bool isChecked)
        {
            ParamTemplatesRepository.UpdateCibleTemplate(idTemp, isChecked);
        }

        public EditTemplateDto DelCibleTemplate(string idCible, string idTemp)
        {
            EditTemplateDto model = new EditTemplateDto
            {
                Templates = ParamTemplatesRepository.DeleteCibleTemplate(idCible, idTemp)
            };
            return model;
        }

        public EditTemplateDto AssociateCibleTemplate(string idCible, string idTemp)
        {
            EditTemplateDto model = new EditTemplateDto
            {
                Templates = ParamTemplatesRepository.AssociateCibleTemplate(idCible, idTemp)
            };
            return model;
        }

        public bool ExistCanevas(string idTemplate)
        {
            return ParamTemplatesRepository.ExistCanevas(idTemplate);
        }

        public string GetParamTemplate(string idTemp, string tempFlag)
        {
            return ParamTemplatesRepository.GetParamTemplate(idTemp, tempFlag);
        }

        /// <summary>
        /// Charge le template par défaut de la branche/cible en paramètre        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="codeCible"></param>
        /// <param name="codeBranche"></param>
        /// <param name="modeNavig"></param>
        /// <returns>
        /// Null si aucun template par défaut</returns>
        public OffreDto GetDefaultTemplateOffre(string type, string codeCible, string codeBranche, ModeConsultation modeNavig)
        {
            OffreDto toReturn = null;
            var lstTemplate = LoadListTemplates(0, string.Empty, string.Empty, type, codeCible, codeBranche, false, true);
            if (lstTemplate != null)
            {
                var defaultTemplate = lstTemplate.Find(elm => elm.Default);
                if (defaultTemplate != null)
                {
                    toReturn = new PoliceServices().OffreGetDto(defaultTemplate.CodeTemplate, 0, type, modeNavig);
                }
            }
            return toReturn;
        }

        public ContratInfoBaseDto GetDefaultTemplateContrat(string type, string codeAvn, string codeCible, string codeBranche, ModeConsultation modeNavig)
        {
            ContratInfoBaseDto toReturn = null;
            var lstTemplate = LoadListTemplates(0, string.Empty, string.Empty, type, codeCible, codeBranche, false, true);
            if (lstTemplate != null)
            {
                var defaultTemplate = lstTemplate.Find(elm => elm.Default);
                if (defaultTemplate != null)
                {
                    toReturn = AffaireNouvelleRepository.InitContratInfoBase(defaultTemplate.CodeTemplate, "0", type, codeAvn, string.Empty, modeNavig);
                    //suppression de l'encaissement lorsqu'on choisit un canevas
                    toReturn.Encaissement = string.Empty;
                }
            }
            return toReturn;
        }

        public void RegenerateCanevas(string user, bool totalRegeneration, string canevas= "%CV%")
        {
            FormuleRepository.RegenerateCanevas(user, totalRegeneration, canevas);
        }

        public void CopyCanevas(String source, String cible, String user)
        {
            FormuleRepository.CopyCanevas(source, cible, user);
        }

        #endregion

        #region Menus Contextuels

        public List<UsersContextMenuDto> GetAllUsersContextMenu(string appl)
        {
            return CommonRepository.GetAllUsersContextMenu(appl);
            //return BLCommonOffre.GetAllUsersContextMenu(appl);
        }

        public List<ContextMenuPlatDto> GetAllUsersFlatContextMenu(string appl)
        {
            return CommonRepository.GetAllUsersFlatContextMenu(appl);
            //return BLCommonOffre.GetAllUsersFlatContextMenu(appl);
        }

        #endregion

        #region Gestion nomenclatures

        public GestionNomenclatureDto LoadInfoNomenclature()
        {
            return ParamGestionNomenclatureRepository.LoadInfoGestionNomenclature();
        }

        public GestionNomenclatureDto LoadListNomenclature(string typologie, string branche, string cible)
        {
            return ParamGestionNomenclatureRepository.LoadListNomenclature(typologie, branche, cible);
        }

        public NomenclatureDto OpenNomenclature(string idNomenclature, string typologie)
        {
            return ParamGestionNomenclatureRepository.OpenNomenclature(idNomenclature, typologie);
        }

        public string SaveNomenclature(string idNomenclature, string codeNomenclature, string ordreNomenclature, string libelleNomenclature, string typologie)
        {
            return ParamGestionNomenclatureRepository.SaveNomenclature(idNomenclature, codeNomenclature, ordreNomenclature, libelleNomenclature, typologie);
        }

        public void DeleteNomenclature(string idNomenclature)
        {
            ParamGestionNomenclatureRepository.DeleteNomenclature(idNomenclature);
        }

        #endregion

        #region Gestion grille nomenclature

        public List<GrilleDto> LoadInfoGestionGrille(string searchGrille)
        {
            return ParamGestionNomenclatureRepository.LoadInfoGestionGrille(searchGrille);
        }

        public GrilleDto OpenGrille(string idGrille)
        {
            return ParamGestionNomenclatureRepository.OpenGrille(idGrille);
        }

        public string SaveGrille(string codeGrille, string libelleGrille, int newGrille)
        {
            return ParamGestionNomenclatureRepository.SaveGrille(codeGrille, libelleGrille, newGrille);
        }

        public void DeleteGrille(string codeGrille)
        {
            ParamGestionNomenclatureRepository.DeleteGrille(codeGrille);
        }

        public string SaveLineGrille(string codeGrille, string libelleGrille, string newGrille,
            string typologie, string libTypologie, string lienTypologie, string ordreTypologie)
        {
            return ParamGestionNomenclatureRepository.SaveLineGrille(codeGrille, libelleGrille, newGrille,
                typologie, libTypologie, lienTypologie, ordreTypologie);
        }

        public void DeleteLineGrille(string codeGrille, string ordreTypologie)
        {
            ParamGestionNomenclatureRepository.DeleteLineGrille(codeGrille, ordreTypologie);
        }

        public GrilleDto OpenSelectionValeur(string codeGrille, string typoGrille, string niveau)
        {
            return ParamGestionNomenclatureRepository.OpenSelectionValeur(codeGrille, typoGrille, niveau);
        }

        public void SaveValeurs(string codeGrille, string typologie, string niveau, string niveauMere,
            string selVal1, string selVal2, string selVal3, string selVal4, string selVal5,
            string selVal6, string selVal7, string selVal8, string selVal9, string selVal10)
        {
            ParamGestionNomenclatureRepository.SaveValeurs(codeGrille, typologie, niveau, niveauMere,
                                selVal1, selVal2, selVal3, selVal4, selVal5,
                             selVal6, selVal7, selVal8, selVal9, selVal10);
        }

        public GrilleDto SearchValeurNomenclature(string codeGrille, string typologie, string idMere, string searchTerm)
        {
            return ParamGestionNomenclatureRepository.SearchValeurNomenclature(codeGrille, typologie, idMere, searchTerm);
        }

        public GrilleDto LoadValeurs(string codeGrille, string idMere, string niveau)
        {
            return ParamGestionNomenclatureRepository.LoadValeurs(codeGrille, idMere, niveau);
        }

        public TypologieDto LoadListValeurs(string codeGrille, string idMere, string niveau)
        {
            return ParamGestionNomenclatureRepository.LoadListValeurs(codeGrille, idMere, niveau);
        }

        public TypologieDto ReloadListValeurs(string codeGrille, string idMere, string niveau)
        {
            return ParamGestionNomenclatureRepository.ReloadListValeurs(codeGrille, idMere, niveau);
        }

        #endregion

        #region Gest Utilisateurs

        public List<UtilisateurBrancheDto> GetUtilisateurBranchesByCriteria(KUSRDRT_COl criteriaCol, string criteriaValue, WHERE_OPER comapreOpertor)
        {
            return CommonRepository.GetUtilisateurBranchesByCriteria(criteriaCol, criteriaValue, comapreOpertor);
        }

        public UtilisateurBrancheDto GetUtilisateurBrancheDto(string utilisateur, string branche)
        {
            return CommonRepository.FirstOrDefaultUtilisateurBranche(utilisateur, branche);
        }

        public List<UtilisateurBrancheDto> LoadUtilisateurBrancheDtos()
        {
            return CommonRepository.LoadListUtilisateurBranche();
        }

        public List<UtilisateurBrancheDto> GetUtilisateurBrancheDtos(string utilisateur, string branche, string typeDroit)
        {
            return CommonRepository.GetUtilisateurBranches(utilisateur, branche, typeDroit);
        }

        public void AjouterUtilisateurBranche(UtilisateurBrancheDto utilisateurBranche)
        {
            CommonRepository.AddUtilisateurBranche(utilisateurBranche);
        }

        public void ModifierUtilisateurBranche(UtilisateurBrancheDto utilisateurBranche, string newTypeDroit)
        {
            CommonRepository.UpdaterUtilisateurBranche(utilisateurBranche, newTypeDroit);
        }

        public void SupprimerUtilisateurBranche(UtilisateurBrancheDto utilisateurBranche)
        {
            CommonRepository.DeleteUtilisateurBranche(utilisateurBranche);
        }

        #endregion

        #region Stat offre/Contrat
        public List<StatOffreContratDto> GetStatOffreContrat(StatOffreContratFiltreDto filtre)
        {
            return CommonRepository.GetStatOffreContrat(filtre);
        }
        #endregion

        #region Stat Clauses libres

        public List<StatClausesLibresDto> GetStatClausesLibres(ParamRecherClausLibDto paramrecherche)
        {
            return CommonRepository.GetStatClausesLibres(paramrecherche);
        }
        #endregion
        #region Log ParamCibleRecup
        public List<LogParamCibleRecupDto> GetLogParamCibleRecup(LogParamCibleRecupDto filtre)
        {
            return CommonRepository.GetLogParamCibleRecup(filtre);
        }
        #endregion
        #region Frais Accessoires
        public List<FraisAccessoiresEngDto> GetFraisAccessoires(FraisAccessoiresEngDto filtre, bool likeCategorie)
        {
            return CommonRepository.GetFraisAccessoires(filtre, likeCategorie);
        }


        public void InsertFraisAccessoires(FraisAccessoiresEngDto toSave)
        {
            CommonRepository.InsertFraisAccessoires(toSave);
        }

        public void UpdateFraisAccessoires(FraisAccessoiresEngDto filtre, FraisAccessoiresEngDto toSave)
        {
            CommonRepository.UpdateFraisAccessoires(filtre, toSave);
        }


        public void DeleteFraisAccessoires(FraisAccessoiresEngDto filtre)
        {
            CommonRepository.DeleteFraisAccessoires(filtre);
        }

        #endregion

        #region Informations Base de données

        public IList<ColumnInfoDto> GetTableDescription(string env, string tableName)
        {
            if (string.IsNullOrEmpty(env) || string.IsNullOrEmpty(tableName))
                return new List<ColumnInfoDto>();

            return CommonRepository.GeTableDescription(env, tableName);
        }

        #endregion

        #region OffreRapide

        public OffreRapideResultDto GetOffresRapides(OffreRapideFiltreDto filtre)
        {
            if (filtre == null)
            {
                return new OffreRapideResultDto
                {
                    NbCount = 0,
                    Offres = new List<OffreRapideInfoDto>()
                };
            }

            return OffreRepository.GetOffresRapideByFiltre(filtre);
        }

        public RechercheOffresRapideSaisieDto GetRechercheOffresRapideReferentiel()
        {
            return new RechercheOffresRapideSaisieDto
            {
                TypeTraitements = CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "PBTTR"),
                Branches = CommonRepository.GetParametres(string.Empty, string.Empty, "GENER", "BRCHE", tCod: new List<string> { "PP", "ZZ" }, notIn: true, tPcn2: "1"),
                Cibles = BrancheRepository.GetCibles(string.Empty, true, true, true),
                Periodicites = CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "PBPER"),

            };
        }

        #endregion

        #region LogPerf

        public List<LogPerf> GetLogPerfs(DateTime? startDate, DateTime? endDate)
        {
            return CommonRepository.GetLogPerfs(startDate, endDate);
        }

        #endregion 

        //public void RestartBridge() {
        //    using (var serviceContext = new WSKheoBridge.KheoBridge())
        //    {
        //        var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
        //        if (!string.IsNullOrEmpty(kheoBridgeUrl))
        //            serviceContext.Url = kheoBridgeUrl;
        //        serviceContext.Restart();
        //    }
        //}
        //public void UpdateScriptsForBridge() {
        //    using (var serviceContext = new WSKheoBridge.KheoBridge())
        //    {
        //        var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
        //        if (!string.IsNullOrEmpty(kheoBridgeUrl))
        //            serviceContext.Url = kheoBridgeUrl;
        //        serviceContext.UpdateScripts();
        //    }
        //}

    }
}
