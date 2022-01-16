using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Common;
using OP.WSAS400.DTO.Contrats;
using OP.WSAS400.DTO.VerouillageOffres;
using OPServiceContract.IAdministration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;


namespace ALBINGIA.OP.OP_MVC.AlbSpecificAttribute {
    /// <summary>
    /// Attribut de vérification de vérouillage d'une offre
    /// </summary>
    public class AlbVerifLockedOfferAttribute : ActionFilterAttribute {
        private readonly string keyNumeroOffre;
        private class Context {
            private readonly string keyNumeroOffre;
            private readonly ActionExecutingContext filterContext;
            private RequestContext requestContext => filterContext.RequestContext;
            internal string valeurNumeroOffre;
            internal Folder folder;
            internal IMetaModelsController metaController;

            internal AlbParameters parameters;
            public Context(string keyNumeroOffre, ActionExecutingContext filterContext) {
                this.keyNumeroOffre = keyNumeroOffre;
                this.filterContext = filterContext;
            }
            internal bool TryInit() {
                this.filterContext.DecodeUrlParams();
                object value = null;
                bool initialized = !IsUrlIgnored(filterContext) && CheckAndGetParams(filterContext.ActionParameters, out value);
                if (initialized) {
                    if (value is string) {
                        valeurNumeroOffre = value as string;
                    }
                    else if (value is IKeyLocker locker) {
                        valeurNumeroOffre = string.Join("_", locker.KeyValues);
                    }
                    else {
                        throw new AlbException(new Exception("Impossible de verifier la requête"));
                    }
                    this.metaController = filterContext.Controller as IMetaModelsController;

                    InitMetaParamerters();
                    parameters = metaController?.Model?.AllParameters;
                }
                return initialized;
            }

            internal bool IsReadonlyRequest() {
                if (parameters is null || parameters.IgnoreReadonly) {
                    return false;
                }
                return parameters.ForceReadonly
                    //|| this.valeurNumeroOffre.ToLower().Contains("readonly")
                    || this.valeurNumeroOffre.ToLower().Contains("consultonly")
                    || parameters.ModeNavig == ModeConsultation.Historique;
            }

            internal bool IsReadonlyScreen() {
                bool readonlyRequest = IsReadonlyRequest();
                if (parameters is null || parameters.IgnoreReadonly) {
                    return false;
                }
                return parameters.ForceReadonly || readonlyRequest || parameters[AlbParameters.ConsultOnlyKey].AsBoolean().GetValueOrDefault();
            }

            internal bool IsModifHorAvenant() {
                if (parameters is null) {
                    return false;
                }
                return parameters[AlbParameters.ConsultOnlyAndEditKey].AsBoolean().GetValueOrDefault()
                    || AlbSessionHelper.IsModfiHorsAvenant(this.metaController.Model.TabGuid, this.folder.FullIdentifier);
            }

            internal bool IsFolderValid() {
                if (this.metaController == null || this.folder == null) {
                    return false;
                }
                return parameters.ModeConsultationAvenant != "CREATE" && !parameters.IgnoreReadonly && !parameters.ForceReadonly && AlbUserRoles.GetValiditeOffre(this.folder);
            }

            internal void InitMetaParamerters() {
                this.folder = null;
                if (this.metaController == null) {
                    return;
                }
                this.metaController.Model.AllParameters = AlbParameters.Parse(valeurNumeroOffre as string);
                this.folder = this.metaController.Model.AllParameters.Folder;
                if (this.folder.NumeroAvenant == 0) {
                    this.folder.NumeroAvenant = this.metaController.Model.AllParameters.NumeroAvenant.GetValueOrDefault();
                }
            }
            private bool CheckAndGetParams(IDictionary<string, object> paramController, out object parameters) {
                parameters = null;
                return paramController.Count > 0
                    && (!paramController.TryGetValue(keyNumeroOffre, out parameters)
                        || (parameters is string && !string.IsNullOrEmpty((string)parameters))
                        || (parameters is IKeyLocker && ((IKeyLocker)parameters).KeyValues?.Any() == true));
            }

            internal bool IsModifValideAllowed() {
                string url = (requestContext.HttpContext.Request.Url?.AbsoluteUri ?? string.Empty).ToLowerInvariant();
                return new string[] { "engagementperiode" }.Any(x => url.Contains(x));
            }

            internal bool IsModifValidatedRequest() {
                return !(parameters.ForceReadonly
                    || this.valeurNumeroOffre.ToLower().Contains("consultonly")
                    || parameters.ModeNavig == ModeConsultation.Historique) && (parameters.AccessMode.ToLower() == "recherche");
            }


            internal FlowName GetFlowName() {
                string url = (requestContext.HttpContext.Request.Url?.AbsoluteUri ?? string.Empty).ToLowerInvariant();
                var isEngagement = new string[] { "engagementperiode" }.Any(x => url.Contains(x));
                var isRegul = new string[] { "creationregularisation" }.Any(x => url.Contains(x));
                if (this.parameters.AccessMode.ToLower() == "recherche" && isEngagement) {
                    return FlowName.Engagement;
                }
                if (this.parameters.IsConsultOnlyAndEdit) {
                    return FlowName.ModifHorsAvenenant;
                }
                if (((this.parameters.Folder.OriginalArray?.Length ?? 0) == 0)
                    || this.parameters.FolderId.EndsWith("_C")
                    || this.parameters[Framework.Business.PipedParameter.ACTION] == "OffreToAffaire") {
                    var isOffre = new string[] { "CreationSaisie" }.Any(x => url.Contains(x));

                    if (isOffre) {
                        return FlowName.Offre;
                    }
                    return FlowName.Contrat;
                }
                if (((this.parameters.Folder.OriginalArray?.Length ?? 0) < 2)
                    || this.parameters.ModeConsultationAvenant == "CREATE"
                    ) {
                    if (isRegul) {
                        return FlowName.Regularisation;
                    }
                    switch (parameters.TypeAvenant) {
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            return FlowName.AvenantRemiseEnVigueur;
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            return FlowName.AvenantModif;
                        case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                            return FlowName.AvenantModif;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                            return FlowName.AvenantRegul;
                    }
                }
                if (this.parameters.Folder.Type == "O") {
                    return FlowName.Offre;
                }
                return FlowName.Contrat;
            }

            internal FlowAccessMode GetAction(string flow) {
                if (parameters.AccessMode == "recherche") {
                    return FlowAccessMode.Engagement;
                }
                if (parameters.IsConsultOnlyAndEdit) {
                    return FlowAccessMode.ModifHorsAvenant;
                }
                if (parameters.ModeNavig == ModeConsultation.Historique ||
                    parameters.ModeConsultationAvenant == "CONSULT" ||
                    parameters.IsConsultOnly) {
                    return FlowAccessMode.ReadOnly;
                }
                return FlowAccessMode.ReadWrite;
            }
        }

        public AlbVerifLockedOfferAttribute(string keyNumeroOffre, string type = "O") {
            this.keyNumeroOffre = keyNumeroOffre ?? string.Empty;
        }

        public string RedirectPath { get; set; }
        public static string SplitChar { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
        }

        public /*override*/ void old_OnActionExecuting(ActionExecutingContext filterContext) {
            string user = AlbSessionHelper.ConnectedUser;
            var context = new Context(this.keyNumeroOffre, filterContext);
            if (!context.TryInit()) {
                return;
            }
            if (context.folder == null) {
                context.folder = new Folder(context.valeurNumeroOffre.Split('_')) {
                    NumeroAvenant = context.metaController.Model.AllParameters.NumeroAvenant.GetValueOrDefault()
                };
            }
            var folder = context.folder;

            bool readonlyRequested = context.IsReadonlyRequest();
            bool modifValidatedRequest = context.IsModifValidatedRequest();
            var readonlyAccess = readonlyRequested;
            bool readonlyScreen = context.IsReadonlyScreen();
            bool modifHorsAvenant = context.IsModifHorAvenant();
            bool isFolderValid = context.IsFolderValid();

            int numAvn = folder.NumeroAvenant;
            string token = context.metaController.Model.TabGuid;
            var parameters = context.metaController.Model.AllParameters;

            bool isFirstScreenOnly = IsFirstScreenOnlyRequest(filterContext.RequestContext, modifHorsAvenant);
            bool isFirstScreen = isFirstScreenOnly || MayBeFirstScreenRequest(filterContext.RequestContext);
            bool isModifingValideAllowed = context.IsModifValideAllowed();
            bool offerisIncache = IsElementLocked(folder, user, SplitChar, out string lockUser, out bool isIncache);

            var ignoreReadOnly = parameters.IgnoreReadonly || isModifingValideAllowed && modifValidatedRequest;

            var flow = isFirstScreen ? context.GetFlowName().ToString() : null;
            var requestedAction = isFirstScreen ? context.GetAction(flow) : FlowAccessMode.ReadOnly;

            var profile = AlbUserRoles.SetUserProfil(
                tabGuid: token,
                userName: user,
                offre: folder.CodeOffre,
                version: folder.Version.ToString(),
                type: folder.Type,
                lockUser: lockUser,
                firstScreen: isFirstScreen,
                isValideFolder: isFolderValid,
                readOnlyFolder: readonlyAccess,
                forceReadOnly: parameters.ForceReadonly,
                ignoreReadOnly: ignoreReadOnly,
                modeConsultationEcran: readonlyScreen,
                numAvn: numAvn.ToString(CultureInfo.InvariantCulture),
                modifHorsAvenant: modifHorsAvenant,
                currentFlow: flow,
                flowInitAction : requestedAction
                );

            if (profile == AlbUserProfileTask.FirstScreen) {
                isFirstScreen = true;
            }
            else if (profile == AlbUserProfileTask.ReadOnly) {
                readonlyAccess = true;
            }

            readonlyAccess = readonlyAccess && !modifHorsAvenant;

            if (!isFirstScreen) {
                if (!modifHorsAvenant && (readonlyAccess || isFolderValid)) {
                    base.OnActionExecuting(filterContext);
                    return;
                }
            }
            else {
                if (readonlyAccess) {
                    if (readonlyRequested) {
                        base.OnActionExecuting(filterContext);
                        return;
                    }
                    if (!isModifingValideAllowed) {
                        RedirectToSearchHome(filterContext, folder.CodeOffre, folder.Version.ToString(), folder.Type, lockUser);
                        return;
                    }
                }
            }

            bool userTabFolderLock = AlbSessionHelper.IsUserTabFolderLock(token, folder.FullIdentifier);
            bool isReadOnlyFolder = AlbUserRoles.IsReadOnlyFolder(
                tabGuid: token,
                userName: user,
                offre: folder.CodeOffre.Trim(),
                version: folder.Version.ToString(),
                type: folder.Type,
                numAvn: numAvn.ToString(CultureInfo.InvariantCulture))
               && ContainsScreenNameReferrer(filterContext: filterContext, screenName: NomsInternesEcran.RechercheSaisie)
               && !modifHorsAvenant;

            #region Traitement du cas de la lecture seule
            if (isReadOnlyFolder && readonlyRequested) {
                if (isFirstScreen && offerisIncache) {
                    if (userTabFolderLock ||
                        (!string.IsNullOrEmpty(lockUser) && lockUser != user)) {

                        DeleteTabGuid(token, folder.Identifier + "_" + numAvn, lockUser);
                    }
                }
                if (!folder.CodeOffre.ToLower().Contains("cv") || !folder.CodeOffre.ToLower().Contains("cnva")) {
                    //*** Si l'offre est en cache ET l'une des conditions soient remplies :
                    //**********l'utilisateur de verrou n'est pas vide et il est différent de l'utiliateur qui consulte
                    //********* l'utilisateur consultant la même offre  dans une autre tab
                    if (offerisIncache && (string.IsNullOrEmpty(lockUser) && lockUser != user) || userTabFolderLock) {
                        //****Retire l'offre du cache
                        DeleteTabGuid(token, folder.Identifier + "_" + numAvn, lockUser);
                    }

                    RedirectToSearchHome(filterContext, folder.CodeOffre, folder.Version.ToString(), folder.Type, lockUser);
                    return;
                }
                base.OnActionExecuting(filterContext);
                return;
            }

            #endregion

            #region Offre Vérouillée séléctionnée
            bool addTolock;
            string oldLockUser = lockUser;
            lockUser = GetOffresVerouillees(folder.CodeOffre, folder.Version, folder.Type, numAvn, user, lockUser, out addTolock);

            if (ApplyProfile(
                numeroOffre: folder.CodeOffre.Trim(),
                version: folder.Version.ToString(),
                type: folder.Type,
                guid: token,
                lockUser: lockUser,
                ifValideFolder: isFolderValid,
                readOnlyFolder: readonlyAccess,
                numAvn: numAvn.ToString(CultureInfo.InvariantCulture),
                modifHorsAvenant: modifHorsAvenant,
                currentFlow: flow) == AlbUserProfileTask.FirstScreen) {
                isFirstScreen = true;
            }
            userTabFolderLock = AlbSessionHelper.IsUserTabFolderLock(token, folder.FullIdentifier);

            #endregion

            if (!userTabFolderLock && lockUser == user) {
                base.OnActionExecuting(filterContext);
                return;
            }

            if (!readonlyAccess && addTolock) {
                OfferAccesAuthorization.VerouillerOffre(user, folder.CodeOffre.Trim(), folder.Version.ToString(), folder.Type + "_" + numAvn, SplitChar);
                base.OnActionExecuting(filterContext);
                return;
            }

            // Cet offre n'est pas valide, n'est pas en lecture seule, et l'utilisateur peut être différent
            // de l'utilisateur de vérrouillage ou le même utilisateur pour la même offre dans != tabulation
            if (isFirstScreen) {
                DeleteTabGuid(token, folder.Identifier + "_" + numAvn, lockUser);
                RedirectToSearchHome(filterContext, folder.CodeOffre, folder.Version.ToString(), folder.Type, lockUser);
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        private static string GetOffresVerouillees(string numOffre, int version, string type, int numAvn, string user, string lockUser, out bool addToLock) {
            addToLock = false;
            numOffre = numOffre.Trim();
            List<OffreVerouilleeDto> offresVerouilles;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>()) {
                var voletsBlocsCategoriesClient = client.Channel;

                offresVerouilles = voletsBlocsCategoriesClient.GetOffresVerouillees(
                    TypeOffre_O: false,
                    TypeOffre_P: false,
                    NumOffre: string.Empty,
                    Version: string.Empty,
                    numAvn: string.Empty,
                    Utilisateur: string.Empty,
                    DateVerouillageDebut: null,
                    DateVerouillageFin: null).ToList();
            }

            //MAJ CACHE
            offresVerouilles.ForEach(el =>
                OfferAccesAuthorization.VerouillerOffre(
                    user: el.Utilisateur,
                    numOffre: el.NumOffre,
                    version: el.Version.ToString(CultureInfo.InvariantCulture),
                    typeOffre: el.Type + "_" + el.NumAvenant.ToString(CultureInfo.CurrentCulture),
                    splitCaracter: SplitChar));


            var selectedOffre =
             offresVerouilles.FirstOrDefault(el =>
                       el.Type == type
                    && el.NumOffre.Trim() == numOffre
                    && el.Version == version
                    && el.NumAvenant == numAvn
            );
            if (selectedOffre == null) {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>()) {
                    client.Channel.AjouterOffreVerouille(
                        sevice: "PRODU",
                        typ: type,
                        ipb: numOffre,
                        alx: version,
                        avn: numAvn,
                        kavsua: 0,
                        kavnum: 0,
                        kavsbr: "",
                        kavactg: "Contrat",
                        kavact: "GESTION",
                        kavverr: "O",
                        user: user,
                        kavlib: $"Verrouillage de l'affaire à {DateTime.Now.ToString(CultureInfo.CurrentCulture)}");
                }
                addToLock = true;
            }
            else {

                //MAJ de tous le cache offre
                if (lockUser != selectedOffre.Utilisateur) {
                    OfferAccesAuthorization.RemoveCacheOffre(MvcApplication.SPLIT_CONST_HTML);
                    offresVerouilles.ForEach(offre =>
                        OfferAccesAuthorization.VerouillerOffre(
                            user: user,
                            numOffre: offre.NumOffre,
                            version: offre.Version.ToString(CultureInfo.CurrentCulture),
                            typeOffre: offre.Type + "_" + offre.NumAvenant,
                            splitCaracter: MvcApplication.SPLIT_CONST_HTML)
                        );
                }
                lockUser = string.IsNullOrEmpty(lockUser) ? selectedOffre.Utilisateur : lockUser;
            }

            return lockUser;

        }

        private static bool IsElementLocked(Folder folder, string user, string spliChar, out string lockUser, out bool isIncache) {
            var offerisIncache = OfferAccesAuthorization.OfferInCache(
                user: user,
                numOffre: folder.CodeOffre.Trim(),
                version: folder.Version.ToString(),
                type: folder.Type + "_" + folder.NumeroAvenant.ToString(),
                splitCaracter: SplitChar,
                lockUser: out lockUser,
                isIncache: out isIncache);

            if (lockUser.IsEmptyOrNull()) {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>()) {
                    lockUser = client.Channel.GetUserVerrou(
                        codeOffre: folder.CodeOffre.Trim(),
                        version: folder.Version.ToString(),
                        type: folder.Type,
                        numAvn: folder.NumeroAvenant.ToString());
                }
            }
            lockUser = lockUser.OrDefault(user);
            return offerisIncache;
        }

        private static void RedirectToSearchHome(ActionExecutingContext filterContext, string numOffre, string version, string type, string lockUser) {
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest()) {
                filterContext.Result = new JsonResult() {
                    Data = new { lockingUser = lockUser },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else {
                filterContext.Result = new RedirectResult($"/RechercheSaisie/Index/{filterContext.ActionParameters["id"]}{AlbOpConstants.LOCKUSER_SPLITSTR}{lockUser}");
            }
        }

        private static bool IsUrlIgnored(ControllerContext filterContext) {
            Uri referrer = filterContext.HttpContext.Request.UrlReferrer;
            if (referrer == null) { return false; }
            var ignoredReferrers = new[] {
                NomsInternesEcran.RechercheSaisie.ToString().ToLower(),
                NomsInternesEcran.ConfirmationSaisie.ToString().ToLower(),
                "recherche",
                string.Format("http://{0}/", referrer.Host.ToLower())
            };
            string referrerUri = referrer.AbsoluteUri.ToLower();

            bool isReferrerOK = ignoredReferrers.Any(x => referrerUri.Contains(x) || referrerUri == x);

            return isReferrerOK
                    && (
                           ContainsScreenCurrentExecPath(filterContext, NomsInternesEcran.CreationSaisie)
                        || ContainsScreenCurrentExecPath(filterContext, NomsInternesEcran.CreationContrat)
                           && !ContainsScreenCurrentExecPath(filterContext, "consultonlyandedit")
                    );
        }
        private static bool ContainsScreenCurrentExecPath(ControllerContext filterContext, NomsInternesEcran screenName) =>
            ContainsScreenCurrentExecPath(filterContext, screenName.ToString());
        private static bool ContainsScreenCurrentExecPath(ControllerContext filterContext, string screenName) =>
            filterContext.HttpContext.Request.CurrentExecutionFilePath.ToLowerInvariant().Contains(screenName.ToLowerInvariant());
        private static bool ContainsScreenNameReferrer(ControllerContext filterContext, NomsInternesEcran screenName) =>
            filterContext.HttpContext.Request.UrlReferrer?.AbsoluteUri.ToLowerInvariant().Contains(screenName.ToString().ToLowerInvariant()) ?? false;

        /// <summary>
        ///    Vérifie si c'est le premier écran
        /// </summary>
        /// <param name="requestContext">Contexte de la requête</param>
        /// <returns></returns>
        private static bool IsFirstScreenOnlyRequest(RequestContext requestContext, bool isModifHorsAvenant) {
            var request = requestContext.HttpContext.Request;
            //if (urlRequest == null) {
            //    return false;
            //}
            string url = request.Url.AbsoluteUri.ToUpperInvariant();
            string urlReferrer = request.UrlReferrer?.AbsolutePath.ToUpperInvariant() ?? string.Empty;
            return
                (
                       (urlReferrer.Contains("RECHERCHE") || urlReferrer == "/")
                    && (
                           url.ContainsAny(new[] { "MODIFIEROFFRE", "CREATIONAVENANT" })
                        || !isModifHorsAvenant && url.Contains("ANINFORMATIONSGENERALES")
                        || isModifHorsAvenant && url.Contains("ANCREATIONCONTRAT")
                       )
                )
                || (
                    (url.ContainsAny(new[] { "OPENREGULE", "UPDATETYPEREGUL", "CHECKLOCKEDBEFOREREGULCREATION" }))
                    && urlReferrer.Contains("CREATIONREGULARISATION")
                )
                || url.Contains("STEP1_CHOIXPERIODE_LOCK");
        }

        private static bool MayBeFirstScreenRequest(RequestContext requestContext) {
            var request = requestContext.HttpContext.Request;


            string url = request.Url.AbsoluteUri.ToUpperInvariant();
            string urlReferrer = request.UrlReferrer?.AbsolutePath.ToUpperInvariant() ?? string.Empty;
            return (urlReferrer.Contains("/RECHERCHESAISIE/") || urlReferrer == "/")
                && (url.ContainsAny(new[] { "/ENGAGEMENTS/", "/ENGAGEMENTPERIODES/", "/PRISEPOSITION/" }));
        }

        /// <summary>
        /// Mise à jour du profil de l'utilisateur
        /// </summary>
        /// <param name="numeroOffre">Numéro d el'offre à vérifier</param>
        /// <param name="version">Version de l'offre</param>
        /// <param name="type">Type</param>
        /// <param name="guid">Guid de la fenêtre du navigateur courante</param>
        /// <param name="lockUser">Utilisateur vérrouillant l'offre</param>
        /// <param name="firstScreen">Indique c'est le premier écran Offre/Contrat</param>
        /// <param name="ifValideFolder">Si l'offre ou le contrat est valide</param>
        /// <param name="readOnlyFolder">Si On est en lecture seule</param>
        /// <param name="ignoreReadOnly">Forcer à Ignorer le readOnly </param>
        /// <param name="modeConsultationEcran">Définit si le mode d'affichage de l'écran est en consultation ou en modification</param>
        /// <param name="forceReadOnly">Forcer le readOnly</param>
        private static AlbUserProfileTask ApplyProfile(
            string numeroOffre,
            string version,
            string type,
            string guid,
            string lockUser = "",
            bool firstScreen = false,
            bool ifValideFolder = false,
            bool readOnlyFolder = false,
            bool forceReadOnly = false,
            bool ignoreReadOnly = false,
            bool modeConsultationEcran = false,
            string numAvn = "0",
            bool modifHorsAvenant = false,
            string currentFlow = null) {
            if (string.IsNullOrEmpty(guid) || guid.Contains("tabGuidtabGuid")) {
                return AlbUserProfileTask.None;
            }
            var user = AlbSessionHelper.ConnectedUser;
            return AlbUserRoles.SetUserProfil(
                tabGuid: guid,
                userName: user,
                offre: numeroOffre,
                version: version,
                type: type,
                lockUser: lockUser,
                firstScreen: firstScreen,
                isValideFolder: ifValideFolder,
                readOnlyFolder: readOnlyFolder,
                forceReadOnly: forceReadOnly,
                ignoreReadOnly: ignoreReadOnly,
                modeConsultationEcran: modeConsultationEcran,
                numAvn: numAvn,
                modifHorsAvenant: modifHorsAvenant,
                currentFlow: currentFlow
                );
        }
        /// <summary>
        /// Set the response result removing the GuidTab parameter
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private static string SetResult(ActionExecutingContext filterContext) {
            var routeValues = (((RedirectToRouteResult)(filterContext.Result))).RouteValues;
            if (routeValues == null)
                return string.Empty;
            var values = routeValues.ToList();
            var controller = string.Empty;
            var action = string.Empty;
            var strProvider = new StringBuilder();
            var guidTab = string.Empty;

            foreach (var value in values) {

                switch (value.Key) {
                    case "action":
                        action = value.Value.ToString();
                        break;
                    case "controller":
                        controller = value.Value.ToString();
                        break;
                    default:
                        var id = value.Value;
                        if (value.Key == "id" && value.Value.ToString().Contains("tabGuid")) {
                            var elms = value.Value.ToString().Split(new[] { "tabGuid" }, StringSplitOptions.None);
                            id = elms[0];
                            guidTab = elms[1];
                        }
                        strProvider.Append("/" + id);
                        break;
                }
                var responseUrl = new StringBuilder();
                responseUrl.Append(string.Format("/{0}/{1}{2}", controller, action, strProvider));
                filterContext.Result = new RedirectResult(responseUrl.ToString());
            }
            return guidTab;
        }
        /// <summary>
        /// Supprime un une ligne (tab courante) du dictionnaires des affaires consultées
        /// </summary>
        /// <param name="tabGuid">Guid de la tabulation du browser</param>
        /// <param name="currentFolder">numéro de l'offre_version_type</param>
        /// <param name="lockUser">Utilisateur qui vverouile l'offre</param>
        private static void DeleteTabGuid(string tabGuid, string currentFolder, string lockUser) {
            if (AlbSessionHelper.ConnectedUser == lockUser) {
                AlbSessionHelper.CurrentFolders.Remove(new FolderKey(AlbSessionHelper.ConnectedUser, tabGuid, currentFolder));
            }
        }
    }
}
