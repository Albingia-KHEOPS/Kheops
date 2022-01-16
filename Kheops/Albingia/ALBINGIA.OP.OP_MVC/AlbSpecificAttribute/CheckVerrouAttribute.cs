using Albingia.Common;
using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Mvc.Common;
using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.ServiceFactory;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Controllers;
using ALBINGIA.OP.OP_MVC.Controllers.AffaireNouvelle;
using ALBINGIA.OP.OP_MVC.Controllers.BNS;
using ALBINGIA.OP.OP_MVC.Controllers.PB;
using ALBINGIA.OP.OP_MVC.Controllers.Regularisation;
using Mapster;
using OP.WSAS400.DTO.Contrats;
using OP.WSAS400.DTO.Regularisation;
using OPServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace Albingia.Kheops.Mvc {
    public class CheckVerrouAttribute : ActionFilterAttribute {
        const string CannotAccess = "CANNOT_ACCESS";
        const string FolderLocked = "FOLDER_LOCKED";
        const string InvalidAccesMessage = "Demande d'accès invalide.";
        public const string SessionLost = "SESSION_LOST";
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            CheckVerrouContext context = null;
            try {
                filterContext.DecodeUrlParams();
                context = new CheckVerrouContext(filterContext);
                if (context.HasLostSession && !context.IsIgnored) {
                    AlbSessionHelper.MessageErreurEcran = SessionLost;
                    RedirectHome(filterContext);
                }
                else if (!context.IsIgnored && !context.IsAjaxContext) {
                    if (!context.IsValid) {
                        throw new BusinessValidationException(new ValidationError(CannotAccess));
                    }

                  EffectuerVerrouillage(context);
                 EffectuerVerrouillage(context, true);
                }
                base.OnActionExecuting(filterContext);
            }
            catch (Exception ex) {
                try {
                    string fullExp = Regex.Replace(System.Web.HttpUtility.HtmlEncode(ex.ToString()), "\r?\n", "<br/>");
                    string expMessage = GetLockErrorMessage(ex, context);
                    string erreurParametersUser = ErrorHandlerBaseAttribute.GetParamValues(filterContext, true);
                    AlbSessionHelper.MessageErreurEcran = "<b>Utilisateur :</b> "
                        + AlbSessionHelper.ConnectedUser + "<br/>"
                        + AlbOpConstants.OPENV_GENERAL_ERROR_MESSAGE + "<br/>"
                        + erreurParametersUser + "<br/>"
                        + Regex.Replace(System.Web.HttpUtility.HtmlEncode(expMessage), "\r?\n", "<br />");
                    new AlbTechException(ex, erreurParameters: erreurParametersUser + fullExp);
                    RedirectHome(filterContext);
                }
                catch(BusinessValidationException) {
                    throw;
                }
                catch {
                    throw new Exception("Unable to handle error. For futher info, look up inner exception", ex);
                }
            }
        }

        private static void EffectuerVerrouillage(CheckVerrouContext context, bool secondary = false) {
            if (context is null) {
                throw new ArgumentNullException(nameof(context));
            }

            var acces = secondary ? context.SecondaryAccesAffaire : context.AccesAffaire;
            if (acces is null) {
                return;
            }
            var affaireId = secondary ? new AffaireId { CodeAffaire = acces.Code, NumeroAliment = acces.Version, TypeAffaire = acces.Type.ParseCode<AffaireType>() } : context.AffaireId;
            if (!acces.VerrouillageEffectue) {
                VerrouAffaire verrou = null;
                if (acces.ModeAcces != AccesOrigine.Consulter) {
                    using (var client = ServiceClientFactory.GetClient<IAffairePort>()) {
                        verrou = client.Channel.TryLockAffaire(affaireId, acces.ModeAcces.ToString());
                    }
                }
                if (verrou != null) {
                    context.SupprimerDemandesAcces();
                    throw new BusinessValidationException(new[] { new ValidationError(GetStatutVerrou(context, verrou).Value.ToString(), verrou.User, affaireId.AsAffaireKey(), null) }, FolderLocked);
                }
                context.ValiderVerrouillage(secondary);
            }
        }

        private static AccesStatutVerrou? GetStatutVerrou(CheckVerrouContext context, VerrouAffaire verrou) {
            if (context is null || !context.IsValid) {
                return null;
            }
            if (verrou is null) {
                return AccesStatutVerrou.Proprietaire;
            }
            if (verrou.User == AlbSessionHelper.ConnectedUser) {
                return AccesStatutVerrou.VerrouilleProprietaire;
            }
            return verrou.IsLocked ? AccesStatutVerrou.Verrouille : AccesStatutVerrou.NonVerrouille;
        }

        private static bool AccesMatchesAffaire(AccesAffaire accesAffaire, AffaireId affaireId) {
            if (affaireId is null) {
                return false;
            }
            return accesAffaire.Code?.Trim() == affaireId.CodeAffaire.Trim()
                && accesAffaire.Avenant == affaireId.NumeroAvenant
                && accesAffaire.Version == affaireId.NumeroAliment
                && !affaireId.IsHisto || accesAffaire.ModeAcces.IsIn(AccesOrigine.Consulter);
        }

        private static string GetLockErrorMessage(Exception ex, CheckVerrouContext c) {
            string message = string.Empty;
            if (ex is BusinessValidationException vex) {
                if (vex.Errors?.Any() == true) {
                    var e = vex.Errors.First();
                    if (e.Error == CannotAccess) {
                        message = InvalidAccesMessage;
                    }
                    else if (vex.Message == FolderLocked) {
                        if (!Enum.TryParse(e.TargetType, out AccesStatutVerrou s)) {
                            message = $"L'affaire {c.AffaireId.AsAffaireKey()} est verrouillée";
                        }
                        else {
                            switch (s) {
                                case AccesStatutVerrou.Verrouille:
                                    message = $"L'affaire {e.TargetId} est verrouillée par {e.TargetCode}";
                                    break;
                                case AccesStatutVerrou.VerrouilleProprietaire:
                                    message = $"Vous avez déjà ouvert cette affaire pour modification";
                                    break;
                                default:
                                    message = vex.Message;
                                    break;
                            }
                        }
                    }
                    else if (e.Error.StartsWith(SessionLost)) {
                        throw vex;
                    }
                }
            }
            else {
                message = ex.Message;
            }
            return message;
        }

        static void RedirectHome(ActionExecutingContext context) {
            if (context is null) {
                return;
            }
            var actionMethodInfo = context.Controller.GetType().GetMethod(context.ActionDescriptor.ActionName);
            if (actionMethodInfo.GetCustomAttributes(typeof(AlbAjaxRedirectAttribute), true).Count() > 0) {
                context.Result = new JavaScriptResult {
                    Script = $"window.location.replace('{RedirectionController.HomeUrl}');"
                };
            }
            else {
                if (context.HttpContext.Request.IsAjaxRequest()) {
                    AlbSessionHelper.MessageErreurEcran = null;
                    throw new BusinessValidationException(new ValidationError(SessionLost + "_RELOAD"));
                }
                else {
                    context.Result = new RedirectResult(RedirectionController.HomeUrl);
                }
            }
        }

        class CheckVerrouContext {
            private readonly IMetaModelsController controller;
            private readonly string action;
            private readonly static Regex localhostRegex = new Regex(@"^http\://localhost(\:\d+)?/?$", RegexOptions.Compiled | RegexOptions.Singleline);
            internal IEnumerable<AccesAffaire> AllAcces =>
                AffaireId is null || AffaireId.CodeAffaire.IsEmptyOrNull() ? Enumerable.Empty<AccesAffaire>()
                    : MvcApplication.ListeAccesAffaires.Where(x =>
                        x.Code == AffaireId.CodeAffaire.Trim() 
                        && x.Version == AffaireId.NumeroAliment
                        && x.Avenant == AffaireId.NumeroAvenant);

            public AccesAffaire AccesAffaire { get; set; }
            public AccesAffaire SecondaryAccesAffaire { get; set; }
            public AffaireId AffaireId{ get; set; }
            public Guid Tab { get; }
            public string UrlReferrer { get; }
            public bool IsAjaxContext { get; }
            public bool IsBackOfficeContext { get; }
            public bool IsCreateMode { get; }
            public bool IsCanevasMode { get; private set; }
            public bool IsSessionNew { get; }
            public bool IsCreateFromOffre { get; }
            public Uri RequestUri { get; }
            public string Url => RequestUri?.AbsoluteUri.ToUpperInvariant() ?? string.Empty;
            public bool IsValid => !(AffaireId is null || AccesAffaire is null);
            public bool IsIgnored => this.controller is null
                //|| IsAjaxContext && !ModifierOffreController.IsActionResultAjaxGetAllowed(this.controller as Controller, this.action)
                || IsBackOfficeContext
                || this.controller is HomeController
                || this.controller is CommonNavigationController
                || this.controller is CreationAttestationController
                || this.controller is RechercheSaisieController
                || this.controller is ConditionsGarantieController && this.action == nameof(ConditionsGarantieController.ExportFile)
                || this.controller is CreationRegularisationController && this.action == nameof(CreationRegularisationController.Index)
                || this.controller is CreationPBController && this.action == nameof(CreationPBController.Index)
                //|| this.controller is CreationBNSController && this.action == nameof(CreationBNSController.Index)
                || this.controller is ExcelContratController
                || this.controller is OffreRelanceController
#if DEBUG
                || Url == "/" || Url == "HTTP://OPMVC.LOCAL" || Url == "HTTP://OPMVC.LOCAL/"
#endif
                || localhostRegex.IsMatch(Url);

            public bool HasLostSession => IsSessionNew && RequestUri.LocalPath != RedirectionController.HomeUrl;

            public CheckVerrouContext(ActionExecutingContext controllerContext) {
                IDictionary<string, object> parameters = controllerContext.ActionParameters;
                var request = controllerContext.RequestContext.HttpContext.Request;
                IsSessionNew = controllerContext.HttpContext?.Session.IsNewSession ?? false;
                RequestUri = request.Url;
                UrlReferrer = request.UrlReferrer?.AbsolutePath.ToUpperInvariant() ?? string.Empty;
                IsAjaxContext = controllerContext.RequestContext.HttpContext.Request.IsAjaxRequest();
                this.controller = controllerContext.Controller as IMetaModelsController;
                this.action = controllerContext.ActionDescriptor.ActionName;
                IsBackOfficeContext = this.controller?.IsBackOfficeContext == true;
                if (IsIgnored || IsAjaxContext || HasLostSession) {
                    return;
                }
                string[] idParts = null;
                if (!parameters.TryGetValue("id", out var value)
                    && !parameters.TryGetValue("context", out value)
                    && !parameters.TryGetValue("selectionRisquesObjets", out value)) {
                    return;
                }
                switch (value) {
                    case string s:
                        IsCreateFromOffre = DefineCreateFromOffre(s, out idParts);
                        if (!IsCreateFromOffre) {
                            idParts = s.Split('_');
                        }
                        break;
                    case IKeyLocker key:
                        idParts = key.KeyValues;
                        break;
                    default:
                        return;
                }

                AffaireDto canevas = null;
                IsCreateMode = !IsCreateFromOffre && (DefineCreateMode(idParts) || DefineCreateCanevas(string.Join("_", idParts), out canevas));
                string temp = null;
                if (IsCreateMode) {
                    AffaireId = new AffaireId {
                        CodeAffaire = string.Empty,
                        IsHisto = false,
                        TypeAffaire = this.controller?.GetType() == typeof(CreationSaisieController) ? AffaireType.Offre : AffaireType.Contrat
                    };
                }
                else {
                    if (IsCanevasMode) {
                        AffaireId = new AffaireId { CodeAffaire = canevas.CodeAffaire, NumeroAliment = 0, TypeAffaire = canevas.TypeAffaire };
                    }
                    else {
                        AffaireId = new AffaireId { CodeAffaire = idParts[0], TypeAffaire = idParts[2].Substring(0, 1).ParseCode<AffaireType>(), NumeroAliment = idParts[1].ParseInt().Value };
                        temp = idParts.FirstOrDefault(x => Regex.IsMatch(x, $"{PageParamContext.ModeNavigKey}[SH]?{PageParamContext.ModeNavigKey}"));
                        if (temp.IsEmptyOrNull()) {
                            EnsureAffaireId(idParts, value);
                        }
                        else {
                            AffaireId.IsHisto = temp.Split(new[] { PageParamContext.ModeNavigKey }, StringSplitOptions.None)[1] == "H";
                            if (AffaireId.IsHisto) {
                                SetAffaireIdAvn(value, idParts);
                            }
                        }
                    }
                }
                temp = idParts.FirstOrDefault(x => x.Contains(PageParamContext.TabGuidKey));
                Tab = temp.IsEmptyOrNull() ? default : Guid.TryParse(temp.Split(new[] { PageParamContext.TabGuidKey }, StringSplitOptions.None)[1], out var g) ? g : default;
                SetAccesAffaire(MvcApplication.ListeAccesAffaires.AsEnumerable());
            }

            private void EnsureAffaireId(string[] idParts, object parameter) {
                SetAffaireIdAvn(parameter, idParts);
                using (var client = ServiceClientFactory.GetClient<IAffairePort>()) {
                    var tempId = client.Channel.GetAffaireId(AffaireId.CodeAffaire, AffaireId.NumeroAliment, AffaireId.NumeroAvenant);
                    if (tempId != null) {
                        AffaireId = tempId;
                    }
                }
            }

            private void SetAffaireIdAvn(object parameter, string[] idParts) {
                if (AffaireId is null) {
                    throw new NullReferenceException($"{nameof(AffaireId)} must have a value");
                }
                if (this.controller is RetoursController) {
                    AffaireId.NumeroAvenant = int.Parse(idParts[3]);
                }
                else if (parameter is RegularisationContext rgContext) {
                    AffaireId.NumeroAvenant = (int?)rgContext.ModeleAvtRegul?.NumAvt;
                }
                else {
                    string temp = idParts.FirstOrDefault(x => x.Contains(PageParamContext.ParamKey));
                    if (temp.IsEmptyOrNull()) {
                        AffaireId.NumeroAvenant = 0;
                    }
                    else {
                        var avnParams = temp.ToParamDictionary();
                        AffaireId.NumeroAvenant = avnParams.TryGetValue(AlbParameterName.AVNID.ToString(), out var s) ? s.ParseInt() : 0;
                    }
                }
            }

            private bool DefineCreateFromOffre(string parameters, out string[] parts) {
                parts = new string[0];
                if (!AlbParameters.RegexPipes.IsMatch(parameters)) {
                    return false;
                }
                var list = parameters.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries).ToLookup(x=> x.Split('|').First());
                if (list.Count > 1) {
                    var values = new List<string> {
                        list[PipedParameter.IPB.ToString()].First().Split('|').Last(),
                        list[PipedParameter.ALX.ToString()].First().Split('|').Last(),
                        list[PipedParameter.TYP.ToString()].First().Split('|').Last(),
                        BaseController.GetSurroundedTabGuid(list[PipedParameter.GUIDKEY.ToString()].First().Split('|').Last())
                    };
                    parts = values.ToArray();
                }
                return list.Count > 1;
            }

            private bool DefineCreateMode(string[] idParts) {
                if (idParts == null) {
                    throw new ArgumentNullException(nameof(idParts));
                }

                return (idParts.Length == 1
                        && (this.controller?.GetType() == typeof(CreationSaisieController)
                        || this.controller?.GetType() == typeof(AnCreationContratController)))
                    || idParts.Length > 3 && idParts[3].StartsWith($"C{PageParamContext.TabGuidKey}");
            }

            private bool DefineCreateCanevas(string templateCode, out AffaireDto canevas) {
                var match = HomeController.RegexCanevasId.Match(templateCode);
                canevas = null;
                if (!match.Success) {
                    match = HomeController.RegexCanevas.Match(templateCode);
                    if (match.Success) {
                        IsCanevasMode = true;
                        canevas = new AffaireDto { CodeAffaire = match.Groups[1].Value, TypeAffaire = AffaireType.Offre, NumeroAliment = 0 };
                    }
                }
                else {
                    IsCanevasMode = true;
                    using (var client = ServiceClientFactory.GetClient<IAffairePort>()) {
                        canevas = client.Channel.GetAffaireCanevas(int.Parse(match.Groups[1].Value));
                    }
                }
                return canevas is null && IsCanevasMode;
            }

            public bool IsFirstPage => UrlReferrer.ContainsChars() && Url.ContainsChars()
                && ((
                    (UrlReferrer.Contains("/RECHERCHESAISIE/") || UrlReferrer == "/")
                    && Url.ContainsAny(new[] { "/MODIFIEROFFRE", "/CREATIONAVENANT", "/ANINFORMATIONSGENERALES", "/ANCREATIONCONTRAT", "/ENGAGEMENTS/", "/ENGAGEMENTPERIODES/", "/PRISEPOSITION/" })
                )
                || (
                    Url.ContainsAny(new[] { "OPENREGULE", "UPDATETYPEREGUL", "CHECKLOCKEDBEFOREREGULCREATION" }) && UrlReferrer.Contains("CREATIONREGULARISATION")
                )
                || Url.Contains("STEP1_CHOIXPERIODE_LOCK"));

            internal void ValiderVerrouillage(bool secondary = false) {
                var acces = secondary ? SecondaryAccesAffaire : AccesAffaire;
                MvcApplication.ListeAccesAffaires.Remove(secondary ? SecondaryAccesAffaire : AccesAffaire);
                acces.Valider();
                MvcApplication.ListeAccesAffaires.Add(secondary ? SecondaryAccesAffaire : AccesAffaire);
            }

            internal void SupprimerDemandesAcces() {
                MvcApplication.ListeAccesAffaires.Remove(AccesAffaire);
                MvcApplication.ListeAccesAffaires.Remove(SecondaryAccesAffaire);
            }

            private AccesAffaire SetAccesAffaire(IEnumerable<AccesAffaire> listeAcces) {
                if (AffaireId is null) {
                    return null;
                }
                if (IsCreateMode) {
                    var a = AffaireId.Adapt<AccesAffaire>();
                    a.ModeAcces = AccesOrigine.CreationAffaire;
                    a.TabGuid = Tab;
                    a.Valider();
                    AccesAffaire = a;
                }
                else {
                    var a = listeAcces.FirstOrDefault(x => x.Code.ToIPB() == AffaireId.CodeAffaire.ToIPB()
                        && (!AffaireId.IsHisto || x.Avenant == AffaireId.NumeroAvenant)
                        && x.Version == AffaireId.NumeroAliment
                        && (!AffaireId.IsHisto || x.ModeAcces.IsIn(AccesOrigine.Consulter, AccesOrigine.RetourPiece))
                        && Tab == x.TabGuid);
                    if (a is null) {
                        return null;
                    }
                    AccesAffaire = a;
                    var allAcces = listeAcces.Where(x => Tab == x.TabGuid);
                    if (allAcces.Count() > 1) {
                        SecondaryAccesAffaire = allAcces.First(x => !Equals(x, a));
                    }
                }
                return AccesAffaire;
            }
        }
    }
}