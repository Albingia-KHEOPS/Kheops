using Albingia.Kheops.Common;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.IOFile;
using ALBINGIA.Framework.Common.Tools;
using Mapster;
using OP.DataAccess;
using OP.Services.BLServices;
using OP.Services.REST.wsadel;
using OP.Services.WSKheoBridge;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.GestionIntervenants;
using OP.WSAS400.DTO.GestUtilisateurs;
using OP.WSAS400.DTO.NavigationArbre;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.ParamIS;
using OP.WSAS400.DTO.User;
using OPServiceContract.ICommon;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Caching;
using System.ServiceModel.Activation;
using System.Threading.Tasks;

namespace OP.Services.Common {
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CommonOffre : ICommonOffre
    {
        #region KeyCacheConstants
        public const string WSKEYCACHE_ISMODL = "ISMODL";
        public const string WSKEYCACHE_USRRIGHTS = "USRRIGHTS";
        public const string WSKEYCACHE_USRLOGIN = "USRLOGIN";

        #endregion

        #region Cache WS Data

        private static MemoryCache IsModeleLigneMemCache { get; set; }
        private static MemoryCache UserRightsMemCache { get; set; }
        private static MemoryCache UserLoginMemCache { get; set; }

        #region IS
        public static void InitWsCache(string key)
        {
            if (IsModeleLigneMemCache != null)
                IsModeleLigneMemCache.Remove(key);
            switch (key)
            {
                case WSKEYCACHE_ISMODL:
                    IsModeleLigneMemCache = MemoryCache.Default;
                    break;
                case WSKEYCACHE_USRLOGIN:
                    UserLoginMemCache = MemoryCache.Default;
                    break;

            }
        }
        public static T GetWsDataCache<T>(string key) where T : class
        {
            if (key == WSKEYCACHE_ISMODL && IsModeleLigneMemCache == null)
                return null;
            if (key == WSKEYCACHE_USRLOGIN && UserLoginMemCache == null)
                return null;
            switch (key)
            {
                case WSKEYCACHE_ISMODL:
                    return IsModeleLigneMemCache[key] as T;
                case WSKEYCACHE_USRLOGIN:
                    return UserLoginMemCache[key] as T;
            }
            return null;
        }
        #endregion
        #region User Rights
        public static void InitUserRightsCache(string key)
        {
            if (UserRightsMemCache != null)
                UserRightsMemCache.Remove(key);
            switch (key)
            {
                case WSKEYCACHE_USRRIGHTS:
                    UserRightsMemCache = MemoryCache.Default;
                    break;

            }
        }

        public static T GetUserRightsCache<T>(string key) where T : class
        {
            if (UserRightsMemCache == null)
                return null;
            switch (key)
            {
                case WSKEYCACHE_USRRIGHTS:
                    return UserRightsMemCache[key] as T;
            }
            return null;
        }

        #endregion

        public static void SetWsDataCache<T>(string key, T value)
        {
            var expireDate = DateTime.IsLeapYear(DateTime.Now.Year) && DateTime.Now.Month == 2 && DateTime.Now.Day == 29 ? new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day - 1) : new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day);
            switch (key)
            {
                case WSKEYCACHE_ISMODL:
                    IsModeleLigneMemCache.Set(key, value, expireDate);
                    break;
                case WSKEYCACHE_USRRIGHTS:
                    UserRightsMemCache.Set(key, value, expireDate);
                    break;
                case WSKEYCACHE_USRLOGIN:
                    UserLoginMemCache.Set(key, value, expireDate);
                    break;
            }
        }

        #endregion

        private readonly IUserPort userService;
        private readonly IAffairePort affaireService;
        private readonly ISinistrePort sinistreService;
        private readonly SinistresRestClient sinistresRestClient;
        private readonly ProgramAS400Repository as400Repository;
        internal IDbConnection DbConnection { get; private set; }

        public CommonOffre(IDbConnection connection, IUserPort userService, IAffairePort affaireService, SinistresRestClient sinistresRestClient, ProgramAS400Repository as400Repository, ISinistrePort sinistreService) {
            DbConnection = connection;
            this.userService = userService;
            this.affaireService = affaireService;
            this.sinistresRestClient = sinistresRestClient;
            this.sinistreService = sinistreService;
            this.as400Repository = as400Repository;
        }

        #region Méthodes Publiques
        public static void InitUserRights(bool forceInit)
        {
            if (forceInit)
            {
                InitUserRightsCache(WSKEYCACHE_USRRIGHTS);
            }
            else
            {
                if (GetUserRightsCache<List<UtilisateurBrancheDto>>(WSKEYCACHE_USRRIGHTS) != null) return;
                InitUserRightsCache(WSKEYCACHE_USRRIGHTS);
            }

            SetWsDataCache(WSKEYCACHE_USRRIGHTS, CommonRepository.GetUserRights(""));
            //SetWsDataCache(WSKEYCACHE_USRRIGHTS, BLCommonOffre.GetUserRights(""));
        }
        public static void InitISModeles(bool forceInit)
        {
            if (forceInit)
            {
                InitWsCache(WSKEYCACHE_ISMODL);
            }
            else
            {
                if (GetWsDataCache<List<ParamISLigneInfoDto>>(WSKEYCACHE_ISMODL) != null) return;
                InitWsCache(WSKEYCACHE_ISMODL);
            }

            SetWsDataCache(WSKEYCACHE_ISMODL, ParamISRepository.GetParamISLignesInfo(""));
        }
        public static void InitUsersLogin(bool forceInit)
        {
            if (forceInit)
            {
                InitWsCache(WSKEYCACHE_USRLOGIN);
            }
            else
            {
                if (GetWsDataCache<UsersDto>(WSKEYCACHE_USRLOGIN) != null) return;
                InitWsCache(WSKEYCACHE_USRLOGIN);
            }
            // Rafrachir la propriété depuis la BD
            SetWsDataCache(WSKEYCACHE_USRLOGIN, CommonRepository.GetListAlbUser());
        }



        public string GetAs400User(string adUser)
        {
            InitISModeles(false);
            return BLCommon.GetAs400User(adUser, DbConnection);
            //return BLCommonOffre.GetAs400User(adUser);
        }
        public bool GetIsAdmin(string adUser)
        {
            return CommonRepository.ExistRow(string.Format("SELECT COUNT(*) NBLIGN FROM YUTILIS WHERE UPPER(TRIM(UTPFX))='{0}' AND UTGRP = 'MA' AND UTSGR = '**'", adUser.ToUpper()));
            //return BLCommonOffre.GetIsAdminUser(adUser);
        }
        public NavigationArbreDto GetHierarchy(string codeOffre, int version, string type, string codeAvn, ModeConsultation modeNavig, string acteGestionRegule)
        {
            return NavigationArbreRepository.GetHierarchy(codeOffre, version, type, codeAvn, modeNavig, acteGestionRegule);
        }

        public void SetTrace(TraceDto trace)
        {
            NavigationArbreRepository.SetTraceArbre(trace);
            //BLCommonOffre.SetTrace(trace);
        }

        public bool ExisteTrace(string codeOffre, int version, string type, string etapeGeneration)
        {
            return NavigationArbreRepository.ExisteTrace(codeOffre, version, type, etapeGeneration);
            //return BLCommonOffre.ExisteTrace(codeOffre, version, type, etapeGeneration);
        }

        public List<ActeGestionDto> GetListActesGestion(string codeOffre, string version, string type, DateTime? dateDeb, DateTime? dateFin, string user, string traitement)
        {
            return CommonRepository.GetListActesGestion(codeOffre, version, type, dateDeb, dateFin, user, traitement);
            //return BLCommonOffre.GetListActesGestion(codeOffre, version, type);
        }

        public string GetInfoActeGstion(string codeAffaire, string version, string type)
        {
            return CommonRepository.GetInfoActeGstion(codeAffaire, version, type);
        }

        #endregion
        #region LCI Franchise
        public LCIFranchiseUnitesTypesDto GetLCIFranchiseUnitesTypes(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            return CommonRepository.GetLCIFranchiseUnitesTypes(codeOffre, version, type, codeAvn, modeNavig);
            //return BLCommonOffre.GetLCIFranchiseUnitesTypes(codeOffre, version, type, codeAvn, modeNavig);
        }
        #endregion
        #region Bandeau
        public BandeauDto GetBandeau(string codeOffre, string versionOffre, string typeOffre)
        {
            return CommonRepository.GetBandeau(codeOffre, versionOffre, typeOffre);
            //return BLCommonOffre.GetBandeau(codeOffre, versionOffre, typeOffre);
        }

        public BandeauDto GetBandeauHisto(string codeOffre, string versionOffre, string typeOffre, string numAvenant)
        {
            return CommonRepository.GetBandeauHisto(codeOffre, versionOffre, typeOffre, numAvenant);
            //return BLCommonOffre.GetBandeauHisto(codeOffre, versionOffre, typeOffre, numAvenant);
        }

        public VisuObservationsDto GetVisuObservations(string codeOffre, string version, string type)
        {
            return CommonRepository.GetVisuObservations(codeOffre, version, type);
            //return BLCommonOffre.GetVisuObservations(codeOffre, version, type);
        }

        public VisuSuspensionDto GetVisuSuspension(string codeOffre, string version, string type)
        {
            return CommonRepository.GetVisuSuspension(codeOffre, version, type);
        }

        public VisuInfosContratDto GetInfosContrat(string codeOffre, string version, string type)
        {
            return CommonRepository.GetInfosContrat(codeOffre, version, type);
        }

        #endregion
        #region gestion des documents

        public string OpenWordDocument(string type, string param, string spliChar, bool clauseResolu, bool clauseLibre, bool createClause, string rule,
            ModeConsultation modeNavig, string contexte, string paramGestionDossier, string user, string wrkStation, string ipStation, string userAD,
            int switchModuleGestDoc, int numAvenant)
        {
            var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
            var tabParam = param.Split(new[] { spliChar }, StringSplitOptions.None);

#if DEBUG
            wrkStation = Environment.MachineName;
#endif

            switch (type)
            {
                case AlbConstantesMetiers.WDOC_CLAUSE:
                    using (var serviceContext = new KheoBridge())
                    {
                        if (!string.IsNullOrEmpty(kheoBridgeUrl))
                            serviceContext.Url = kheoBridgeUrl;

                        string toReturn = string.Empty;
                        long idClause;
                        if (!long.TryParse(tabParam[0], out idClause))
                            toReturn = string.Empty;
                        Int64 idKalcont = 0;
                        Int64.TryParse(contexte, out idKalcont);

                        try
                        {
                            var tabActeGestion = paramGestionDossier.Split('_');
                            var prefixeActeGestion = string.Format(@"{0}_{1}_{2}", tabActeGestion[0], tabActeGestion[1].PadLeft(4, '0'), tabActeGestion[2]);

                            if (!clauseLibre)
                            {
                                var appel = clauseResolu
                                    ? modeNavig == ModeConsultation.Standard ? !rule.Contains('V') ?
                                    1 : 2 : 3 : 4;
                                toReturn = clauseResolu
                                    ? modeNavig == ModeConsultation.Standard ? !rule.Contains('V') ?
                                    serviceContext.ResoudreClause(idClause, FinOffreRepository.ReloadPathFile(AlbConstantesMetiers.TypologieDoc.ClauseResolu.ToString())) :
                                   serviceContext.ResoudreClausePush(userAD, ipStation, idClause, FinOffreRepository.ReloadPathFile(AlbConstantesMetiers.TypologieDoc.ClauseResolu.ToString()), numAvenant) :
                                   serviceContext.ResoudreClauseHisto(idClause, numAvenant, FinOffreRepository.ReloadPathFile(AlbConstantesMetiers.TypologieDoc.ClauseResolu.ToString()))
                                    : serviceContext.ChercherClausePush(userAD, ipStation, idClause, FinOffreRepository.ReloadPathFile(AlbConstantesMetiers.TypologieDoc.Clause.ToString()));
                            }
                            else
                            {
                                var pathFile = ClauseRepository.GetClauseFilePath(tabParam[0], modeNavig).Trim();

                                var prefixChemin = FinOffreRepository.ReloadPathFile(AlbConstantesMetiers.TypologieDoc.ClauseLibre.ToString());

                                var cheminClause = prefixChemin + prefixeActeGestion;

                                if (!IOFileManager.IsExistDirectory(cheminClause))
                                {
                                    System.IO.Directory.CreateDirectory(cheminClause);
                                }
                                cheminClause = cheminClause + "\\";

                                //toReturn = createClause ?
                                //                idKalcont == 0 ?
                                //                    serviceContext.ChercherCanevasClauseLibre(cheminClause) :
                                //                    serviceContext.ChercherCanevasClauseKalcont(idKalcont, cheminClause)
                                //                : string.IsNullOrEmpty(pathFile)
                                //                        ? serviceContext.ResoudreClausePush(userAD, ipStation, idClause, cheminClause)
                                //                        : pathFile;
                                var appel = createClause ?
                                                idKalcont == 0 ?
                                                    1 : 2
                                                : string.IsNullOrEmpty(pathFile) ?
                                                    3 : !rule.Contains('V') ? 4 : 5;

                                toReturn = createClause ?
                                                idKalcont == 0 ?
                                                    serviceContext.ChercherCanevasClauseLibre(cheminClause) :
                                                    serviceContext.ChercherCanevasClauseKalcont(idKalcont, cheminClause)
                                                : string.IsNullOrEmpty(pathFile)
                                                        ? serviceContext.ChercherClauseLibre(idClause, cheminClause)
                                                        : pathFile;
                                // !rule.Contains('V') ? pathFile : serviceContext.ResoudreClausePush(userAD, ipStation, idClause, cheminClause);
                            }

                            if (!rule.Contains('V'))
                            {
                                var idSessionClause = CommonRepository.GetAS400Id("IdTrt");
                                var pushDto = new KheoPushDto
                                {
                                    Fonction = PushFonction.SAISIR_CLAUSE_LIBRE,
                                    Adresse_IP = ipStation,
                                    UserAD = userAD,
                                    NomFichier = toReturn,
                                    ID = idSessionClause
                                };
                                toReturn += "-__-" + idSessionClause;
                                serviceContext.ExecuterPush(pushDto);
                            }
                            else
                            {
                                if (clauseLibre || modeNavig == ModeConsultation.Historique)
                                {
                                    var pushDto = new KheoPushDto
                                    {
                                        Fonction = PushFonction.OUVRIR_DOC_EN_PDF,
                                        Adresse_IP = ipStation,
                                        UserAD = user,
                                        NomFichier = toReturn
                                    };
                                    serviceContext.ExecuterPush(pushDto);
                                }
                                toReturn += "-__-";
                            }


                        }
                        catch (Exception ex)
                        {
                            var test = ex.Message;
                            toReturn = string.Format("ED - Erreur de résolution de la clause N° {0} - {1}", idClause, ex.Message);
                        }

                        return toReturn;
                    }
                case AlbConstantesMetiers.WDOC_CP:
                    if (tabParam.Count() < 3)
                        return string.Format("OP - Erreur de génèration - Paramètres manquants ");

                    var paramPolice = tabParam[0].Split('_');
                    if (paramPolice.Count() < 3)
                        return string.Format("OP - Erreur de génèration - Paramètres police invalides");
                    int iVersion;
                    if (!int.TryParse(paramPolice[1], out iVersion))
                        return string.Format("OP - Erreur de génèration - Paramètre version invalide");
                    using (var serviceContext = new KheoBridge())
                    {
                        if (!string.IsNullOrEmpty(kheoBridgeUrl))
                            serviceContext.Url = kheoBridgeUrl;

                        var file = serviceContext.GenererCp(tabParam[1], paramPolice[2], paramPolice[0], iVersion, FinOffreRepository.ReloadPathFile(AlbConstantesMetiers.TypologieDoc.CP.ToString()));

                        if (switchModuleGestDoc == 1)
                        {
                            CommonRepository.SetTraceLog(string.Empty, string.Empty, string.Empty, 0, string.Empty, "PUSHDOC", DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture), wrkStation + '_' + ipStation);
                            var pushDto = new KheoPushDto
                            {
                                Fonction = PushFonction.OUVRIR_DOC_EN_PDF,
                                Adresse_IP = ipStation,
                                UserAD = user,
                                NomFichier = file
                            };
                        }
                        return file;
                    }
                case AlbConstantesMetiers.WDOC_DOC:
                    using (var serviceContext = new KheoBridge())
                    {
                        if (!string.IsNullOrEmpty(kheoBridgeUrl))
                            serviceContext.Url = kheoBridgeUrl;

                        long idClause;
                        if (!long.TryParse(tabParam[0], out idClause))
                            return string.Format("Err : OP - Erreur de génèration - Paramètres manquants ");

                        var filePath = GestionDocumentRepository.GetFullPathDoc(idClause).Replace("/", "\\");

                        CommonRepository.SetTraceLog(string.Empty, string.Empty, string.Empty, 0, string.Empty, "GENDOCW", DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture), wrkStation + '_' + ipStation);
                        string fileFullPath = string.Empty;// string.IsNullOrEmpty(filePath) ? serviceContext.GenererKpdocw(idClause) : filePath;
                        string printFile = GestionDocumentRepository.GetParamPrintDoc(idClause);

                        if (switchModuleGestDoc == 1)
                        {
                            var Pushcli = new PushClient.ClientPush(user, wrkStation, "OP");
                            Pushcli.ConnexionPush(ConfigurationManager.AppSettings["UrlServeurPush"]);
                            Pushcli.Start();
                            if (printFile.Contains("V"))
                            {
                                bool isDocExt = GestionDocumentRepository.IsDocExterne(idClause);

                                CommonRepository.SetTraceLog(string.Empty, string.Empty, string.Empty, 0, string.Empty, "PUSHDOC", DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture), wrkStation + '_' + ipStation);

                                var pushDto = new KheoPushDto();
                                if (isDocExt)
                                {
                                    pushDto = new KheoPushDto
                                    {
                                        Fonction = PushFonction.OUVRIR_DOC_EN_PDF,
                                        Adresse_IP = ipStation,
                                        UserAD = user,
                                        NomFichier = filePath
                                    };
                                }
                                else
                                {
                                    pushDto = new KheoPushDto
                                    {
                                        Fonction = PushFonction.GENERER_KPDOCW,
                                        Adresse_IP = ipStation,
                                        UserAD = user,
                                        TypContrat = "",
                                        NumeroContrat = "",
                                        Aliment = 0,
                                        ID = idClause
                                    };
                                }

                                serviceContext.ExecuterPush(pushDto);
                            }
                            else
                            {
                                fileFullPath = string.IsNullOrEmpty(filePath) ? serviceContext.GenererKpdocw(idClause) : filePath;
                                Pushcli.SaisirClauseLibre(ipStation, fileFullPath);
                            }
                            Pushcli.Stop();
                        }

                        return !string.IsNullOrEmpty(printFile) ? fileFullPath + "#docrule#" + printFile : fileFullPath;
                    }
            }
            return string.Empty;
        }

        public string SaveDocMagnetic(string codeAffaire, string version, string type, string codeAvn,
            string codeRsq, string codeObj, string codeFor, string codeOpt,
            string typeDoc, string service, string acteGestion, string ajout, string contexte,
            string idClause, string titleClause, string emplacement, string sousemplacement, string ordre,
            string fullFilePath, string etape)
        {
            return CommonRepository.SaveDocMagnetic(codeAffaire, version, type, codeAvn, codeRsq, codeObj, codeFor, codeOpt, typeDoc, service, acteGestion, ajout, contexte,
                idClause, titleClause, emplacement, sousemplacement, ordre, fullFilePath, etape);
        }

        public string GetClauseFileName(string clauseId)
        {
            return ClauseRepository.GetClauseFileName(clauseId);
        }

        #endregion
        #region gestion des intervenants

        public IntervenantsInfoDto GetListeIntervenants(string code, string version, string type, string orderby, string ascDesc)
        {
            IntervenantsInfoDto toReturn = new IntervenantsInfoDto
            {
                IsAvenantModificationLocale = CommonRepository.ExistTraceAvenant(code, version, type, "INTERVE", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty),
                Intervenants = CommonRepository.GetListeIntervenants(code, version, type, orderby, ascDesc)
            };

            return toReturn;
        }

        public List<IntervenantDto> GetListeIntervenantsAutocomplete(string code, string version, string type, string name, string typeIntervenant, string codeIntervenant, bool fromAffaireOnly)
        {
            return CommonRepository.GetListeIntervenantsAutocomplete(code, version, type, name, typeIntervenant, codeIntervenant, fromAffaireOnly);
        }

        public IntervenantDto GetIntervenantByCodeAutocomplete(string codeOffre, string type, string version, string codeIntervenant, bool fromAffaireOnly)
        {
            return CommonRepository.GetIntervenantByCodeAutocomplete(codeOffre, type, version, codeIntervenant, fromAffaireOnly);
        }

        public List<IntervenantDto> GetListeInterlocuteursByIntervenant(Int64 codeIntervenant, string name)
        {
            return CommonRepository.GetListeInterlocuteursByIntervenant(codeIntervenant, name);
        }

        public List<ParametreDto> GetListeTypesIntervenant()
        {
            return CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "INTYI");
        }

        public List<IntervenantDto> EnregistrerDetailsIntervenant(string code, string version, string type, string codeAvenant, IntervenantDto toSave, string user)
        {
            return CommonRepository.EnregistrerDetailsIntervenant(code, version, type, codeAvenant, toSave, user);
        }

        public IntervenantDto GetDetailsIntervenant(Int64 guidId)
        {
            return CommonRepository.GetDetailsIntervenant(guidId);
        }

        public List<IntervenantDto> SupprimerIntervenant(string code, string version, string type, string codeAvenant, Int64 guidId, string user)
        {
            return CommonRepository.SupprimerIntervenant(code, version, type, codeAvenant, guidId, user);
        }

        #endregion

        public ProfileKheops GetProfileKheops() {
            return this.userService.GetProfile();
        }

        public ProfileKheops SetProfileKheops(ProfileKheops profile, IEnumerable<ProfileKheopsData> specificUpdate = null) {
            return this.userService.SetProfile(profile, specificUpdate);
        }

        public PagingList<ImpayeDto> GetImpayes(int page = 0, int codeAssure = 0) {
            return this.affaireService.GetImpayes(page, codeAssure);
        }

        public PagingList<RetardPaiementDto> GetRetardsPaiement(int page = 0, int codeAssure = 0) {
            return this.affaireService.GetRetardsPaiement(page, codeAssure);
        }

        public PagingList<SinistreDto> GetSinistres(int page = 0, int codeAssure = 0) {
            var result = this.sinistreService.GetSinistres(page, codeAssure);
            var tasks = result.List.Select(s => this.sinistresRestClient.GetCalculsChargementAsync(s)).ToArray();
            result.List = Task.WhenAll(tasks).Result.ToList();
            return result;
        }

        public PagingList<RelanceDto> GetUserRelances(int page = 0) {
            return this.affaireService.GetUserRelances(page);
        }

        public string GetNumAvn(string codeOffre, string version, string type)
        {
            return CommonRepository.GetNumAvn(codeOffre, version, type);
        }
        #region Avenants

        public bool ExistTraceAvenant(string codeOffre, string version, string type, string perimetre, string codeRisque, string codeObjet, string codeFormule, string codeOption, string etape)
        {
            return CommonRepository.ExistTraceAvenant(codeOffre, version, type, perimetre, codeRisque, codeObjet, codeFormule, codeOption, etape);
        }

        public void SaveTraceAvenant(string codeOffre, string version, string type, string perimetre, string codeRisque, string codeObjet, string codeFormule, string codeOption, string etape,
                                            string champ, string action, string oldValue, string newValue, string obligatoire, string operation, string user)
        {
            CommonRepository.SaveTraceAvenant(codeOffre, version, type, perimetre, codeRisque, codeObjet, codeFormule, codeOption, etape, champ, action, oldValue, newValue, obligatoire, operation, user);
        }

        public void CheckAndSaveTraceAvenant(string codeOffre, string version, string type, string perimetre, string codeRisque, string codeObjet, string codeFormule, string codeOption, string etape,
                                            string champ, string action, string oldValue, string newValue, string obligatoire, string operation, string user)
        {
            if (!CommonRepository.ExistTraceAvenant(codeOffre, version, type, perimetre, codeRisque, codeObjet, codeFormule, codeOption, etape))
                CommonRepository.SaveTraceAvenant(codeOffre, version, type, perimetre, codeRisque, codeObjet, codeFormule, codeOption, etape, champ, action, oldValue, newValue, obligatoire, operation, user);
        }

        #endregion

        #region Gestion des droits utilisateurs

        public List<UtilisateurBrancheDto> GetUserRights()
        {
            return GetAllUserRights();
        }

        internal static List<UtilisateurBrancheDto> GetAllUserRights() {
            InitUserRights(true);
            //var toReturn = BLCommonOffre.GetUserRights(userName);
            //Common.CommonOffre.SetWsDataCache(Common.CommonOffre.WSKEYCACHE_USRRIGHTS, toReturn);
            var toReturn = GetUserRightsCache<List<UtilisateurBrancheDto>>(WSKEYCACHE_USRRIGHTS);
            return toReturn;
        }

        #endregion
        #region Gestion des UsersLogin

        public UsersDto GetUsersLogin()
        {
            InitUsersLogin(true);
            return GetWsDataCache<UsersDto>(WSKEYCACHE_USRLOGIN);

        }
        #endregion

        public int SetTraceLog(string codeAffaire, string version, string type, int id, string statut, string methode, string date, string diff)
        {
            return CommonRepository.SetTraceLog(codeAffaire, version, type, id, statut, methode, date, diff);
        }
        
        public InfosBaseDto LoadInfosBase(string codeAffaire, string version, string type, string codeAvn, string modeNavig)
        {
            return CommonRepository.GetInfosBaseOffre(codeAffaire, version, type, codeAvn, modeNavig).Refresh();
        }
        public InfosBaseDto LoadaffaireNouvelleBase(string codeAffaire, string version, string type, string codeAvn, string modeNavig)
        {
            return CommonRepository.LoadaffaireNouvelleBase(codeAffaire, version, type, codeAvn, modeNavig).Refresh();
        }

        public string UpdateCotisationsAS400(AffaireId affaireId, string field, decimal? oldValue, decimal? value) {
            var pgmFolder = affaireId.Adapt<PGMFolder>();
            pgmFolder.ActeGestion = AlbConstantesMetiers.TYPE_OFFRE;
            pgmFolder.User = WCFHelper.GetFromHeader("UserAS400");
            string result = this.as400Repository.UpdateCotisationsOffre(pgmFolder, field, oldValue, value);
            if (result?.ToUpper()?.Trim() == "ERREUR") {
                throw new AlbTechException(new Exception("Erreur programme 400 KA030"));
            }
            CommonRepository.AlimStatistiques(affaireId.CodeAffaire, affaireId.NumeroAliment.ToString(), pgmFolder.User, AlbConstantesMetiers.TYPE_OFFRE, "X");

            NavigationArbreRepository.SetTraceArbre(new TraceDto {
                CodeOffre = affaireId.CodeAffaire,
                Version = affaireId.NumeroAliment,
                Type = AlbConstantesMetiers.TYPE_OFFRE,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation),
                NumeroOrdreDansEtape = 64,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = pgmFolder.User,
                PassageTag = "O",
                PassageTagClause = "N"
            });

            return result;
        }

        public void UpdateRelances(IEnumerable<RelanceDto> relances) {
            var errors = new List<ValidationError>();
            relances.Where(relance => relance.Situation == SituationAffaire.SansSuite && (relance.MotifSituation?.Code.IsEmptyOrNull() ?? true)).ToList().ForEach(relance => {
                errors.Add(new ValidationError($"{relance.CodeOffre}_{relance.Version} : Le motif de situation n'a pas été défini"));
            });

            relances.Where(relance => relance.Situation == SituationAffaire.Inconnu).ToList().ForEach(relance => {
                var actual = this.affaireService.GetSingleRelance(new AffaireId { CodeAffaire = relance.CodeOffre.ToString(), NumeroAliment = relance.Version, TypeAffaire = AffaireType.Offre });
                if (actual.DateValidation?.AddDays(actual.DelaisRelanceJours) != relance.DateValidation.AddDays(relance.DelaisRelanceJours)) {
                    if (relance.DateValidation.AddDays(relance.DelaisRelanceJours) <= DateTime.Today) {
                        errors.Add(new ValidationError($"{relance.CodeOffre}_{relance.Version} : La date de relance doit être superieure à la date courante"));
                    }
                    else if (relance.DelaisRelanceJours > 99) {
                        errors.Add(new ValidationError($"{relance.CodeOffre}_{relance.Version} : Le report de délais de relance ne peut exéder 99 jours"));
                    }
                }
            });

            if (errors.Any()) {
                throw new BusinessValidationException(errors);
            }

            this.affaireService.ClasserOffresSansSuite(relances
                .Where(relance => relance.Situation == SituationAffaire.SansSuite)
                .Select(relance => (relance.Adapt<AffaireId>(), relance.MotifSituation.Code)));

            var activeOffres = relances.Where(relance => relance.Situation == SituationAffaire.Inconnu).ToList();

            this.affaireService.DiffererRelances(activeOffres.Select(relance => (relance.Adapt<AffaireId>(), relance.DelaisRelanceJours)));
            this.affaireService.UpdateFlagAttenteCourtier(activeOffres.Select(relance => (relance.Adapt<AffaireId>(), relance.IsAttenteDocCourtier)));
        }
    }
}
