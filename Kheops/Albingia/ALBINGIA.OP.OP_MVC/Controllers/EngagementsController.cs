using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Business;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Filters;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Engagement;
using OP.WSAS400.DTO.NavigationArbre;
using OPServiceContract;
using OPServiceContract.ICommon;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ALBINGIA.OP.OP_MVC.Controllers {
    public class EngagementsController : ControllersBase<ModeleEngagementsPage> {
        public override bool IsReadonly => base.IsReadonly || this.model.IsReadOnly;

        /// <summary>
        /// Indexes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        
        [WhitespaceFilter]
        [ErrorHandler]
        public ActionResult Index(string id, AccessMode? accessMode = null) {
            this.model.NomEcran = NomsInternesEcran.EngagementParTraite;
            this.model.AccessMode = id.Contains("accessMode") ? id.Split(new[] { "accessMode" }, StringSplitOptions.None)[1] : string.Empty;
            SetCdPeriodeGuidTab(id, out string formattedId);
            formattedId = InitializeParams(formattedId);
            this.model.CurrentAccessMode = accessMode ?? (IsReadonly ? AccessMode.CONSULT : AccessMode.UPDATE);
            LoadInfoPage(formattedId);

            if (!string.IsNullOrEmpty(model.AccessMode)) {
                model.LCIGenerale.AccessMode = model.AccessMode;
                return View("EngagementBody", model);
            }

            return View(model);
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult UpdateRedirect(string codeOffre, string version, string type, string codeAvn, string argEngagement, string argModelCommentForce, string field, string tabGuid, string saveCancel, string paramRedirect
            , string valeurLCI, string typeLCI, string uniteLCI, bool isIndexeeLCI, string lienCpxLCI, string modeNavig, string acteGestion, string codePeriode, string accessMode) {
            model.CodePeriode = codePeriode;
            model.NumAvenantPage = codeAvn;
            model.AccessMode = accessMode;
            UpdateEngagements(codeOffre, version, type, codeAvn, argEngagement, argModelCommentForce, field, tabGuid, valeurLCI, typeLCI, uniteLCI, isIndexeeLCI, lienCpxLCI, modeNavig, acteGestion, codePeriode);

            if (string.IsNullOrEmpty(paramRedirect)) {
                return null;
            }
            var tabParamRedirect = paramRedirect.Split('/');
            return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] + (!string.IsNullOrEmpty(accessMode) ? "accessMode" + accessMode + "accessMode" : string.Empty) });

        }
        [ErrorHandler]
        public ActionResult UpdateInPage(string codeOffre, string version, string type, string codeAvn, string argEngagement, string argModelCommentForce, string field, string tabGuid, string saveCancel
            , string valeurLCI, string typeLCI, string uniteLCI, bool isIndexeeLCI, string lienCpxLCI, string modeNavig, string acteGestion, string codePeriode, string accessMode) {
            model.CodePeriode = codePeriode;
            model.NumAvenantPage = codeAvn;
            model.AccessMode = accessMode;
            UpdateEngagements(codeOffre, version, type, codeAvn, argEngagement, argModelCommentForce, field, tabGuid, valeurLCI, typeLCI, uniteLCI, isIndexeeLCI, lienCpxLCI, modeNavig, acteGestion, codePeriode);

            //Redirection vers la page sélectionnée dans le menu

            if (saveCancel == "1") {
                return RedirectToRoute("/RechercheSaisie/Index");
            }
            model.NomEcran = NomsInternesEcran.EngagementParTraite;

            return PartialView("EngagementBody", model);
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string numAvn, string traite, string argModelCommentForce, string tabGuid, string paramRedirect,
            string modeNavig, string addParamType, string addParamValue, string codePeriode, string accessMode, AccessMode actionEngagement) {
            if (cible == typeof(RechercheSaisieController).ControllerName()) {
                return RedirectToAction(job, cible);
            }
            this.model.CurrentAccessMode = actionEngagement;
            if (!this.model.IsReadOnly && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>()) {
                if (cible == "AttentatGareat") {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                        var serviceContext = client.Channel;
                        serviceContext.SetTrace(new TraceDto {
                            CodeOffre = codeOffre.PadLeft(9, ' '),
                            Version = Convert.ToInt32(version),
                            Type = type,
                            EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Engagement),
                            NumeroOrdreDansEtape = 61,
                            NumeroOrdreEtape = 1,
                            Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Engagement),
                            Risque = 0,
                            Objet = 0,
                            IdInventaire = 0,
                            Formule = 0,
                            Option = 0,
                            Niveau = string.Empty,
                            CreationUser = GetUser(),
                            PassageTag = "O",
                            PassageTagClause = string.Empty
                        });
                    }
                }
                if (cible == typeof(EngagementTraiteController).ControllerName()) {
                    UpdateCommentaireOnly(codeOffre, version, type, argModelCommentForce, codePeriode);
                }
            }
            if (paramRedirect.ContainsChars()) {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            string[] contextInfos = null;
            switch (cible?.ToLower()) {
                case "engagementtraite":
                    contextInfos = new[] { traite + (codePeriode.ContainsChars() ? ("cdPeriode" + codePeriode + "cdPeriode") : string.Empty) };
                    break;
                case "engagements":
                    if (codePeriode.ContainsChars()) {
                        contextInfos = new[] { "cdPeriode" + codePeriode + "cdPeriode" };
                    }
                    break;
            }

            return RedirectToAction(job, cible, new {
                id = AlbParameters.BuildFullId(
                    new Folder(new[] { codeOffre, version, type }),
                    contextInfos,
                    tabGuid,
                    addParamValue,
                    modeNavig,
                    new[] { accessMode.ContainsChars() ? ("accessMode" + accessMode + "accessMode") : string.Empty }),
                actionEngagement
            });
        }
        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult RedirectionSuivant(string codeOffre, string version, string type, string argModelCommentForce, string tabGuid, string paramRedirect,
            string modeNavig, string addParamType, string addParamValue, string codePeriode, string accessMode, AccessMode actionEngagement) {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            this.model.CurrentAccessMode = actionEngagement;
            if (!this.model.IsReadOnly) {
                UpdateCommentaireOnly(codeOffre, version, type, argModelCommentForce, codePeriode);
            }
            bool existEngCnx = HasEngCnx();
            bool reachPageAttentat = HasCatnatBase();

            // existEngCnx=true si le contrat contient une connexité de type engagement
            var cible = existEngCnx ? "EngagementsConnexite" : (reachPageAttentat ? "AttentatGareat" : "AnMontantReference");

            if (cible == "AttentatGareat" || cible == "Cotisations" || cible == "AnMontantReference") {
                if (!this.model.IsReadOnly) {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                        client.Channel.SetTrace(new TraceDto {
                            CodeOffre = codeOffre.PadLeft(9, ' '),
                            Version = Convert.ToInt32(version),
                            Type = type,
                            EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Engagement),
                            NumeroOrdreDansEtape = 61,
                            NumeroOrdreEtape = 1,
                            Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Engagement),
                            Risque = 0,
                            Objet = 0,
                            IdInventaire = 0,
                            Formule = 0,
                            Option = 0,
                            Niveau = string.Empty,
                            CreationUser = GetUser(),
                            PassageTag = "O",
                            PassageTagClause = string.Empty
                        });
                    }
                }
                if (!string.IsNullOrEmpty(paramRedirect)) {
                    var tabParamRedirect = paramRedirect.Split('/');
                    return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
                }
                if (!string.IsNullOrEmpty(codePeriode)) {
                    return RedirectToAction("Index", "EngagementPeriodes", new {
                        id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) + (!string.IsNullOrEmpty(accessMode) ? "accessMode" + accessMode + "accessMode" : string.Empty),
                        actionEngagement
                    });
                }

                return RedirectToAction("Index", cible, new {
                    id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig),
                    actionEngagement
                });
            }
            else
            {
                if (!string.IsNullOrEmpty(paramRedirect)) {
                    var tabParamRedirect = paramRedirect.Split('/');
                    return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
                }
                if (!string.IsNullOrEmpty(codePeriode)) {
                    return RedirectToAction("Index", "EngagementPeriodes", new { id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) + (!string.IsNullOrEmpty(accessMode) ? "accessMode" + accessMode + "accessMode" : string.Empty) });
                }

                return RedirectToAction("Index", cible, new { id = codeOffre + "_" + version + "_" + type + tabGuid + GetFormatModeNavig(modeNavig) });
            }
        }

        #region Méthode Privée

        protected override void LoadInfoPage(string id) {
            string codeOffre = string.Empty;
            string version = string.Empty;
            string type = string.Empty;

            string[] tId = id.Split('_');

            switch (tId[2]) {
                case "O":
                    model.Offre = new Offre_MetaModel();
                    using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                        var CommonOffreClient = chan.Channel;
                        model.Offre.LoadInfosOffre(CommonOffreClient.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig));
                    }
                    AlbSessionHelper.SetCurrentEntityById(tId[0] + "_" + tId[1] + "_" + tId[2], model.Offre);

                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    codeOffre = model.Offre.CodeOffre;
                    version = model.Offre.Version.HasValue ? model.Offre.Version.Value.ToString() : "0";
                    type = model.Offre.Type;
                    break;
                case "P":

                    using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                        var CommonOffreClient = chan.Channel;
                        var infosBase = CommonOffreClient.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig);
                        model.Contrat = new ContratDto() {
                            CodeContrat = infosBase.CodeOffre,
                            VersionContrat = Convert.ToInt64(infosBase.Version),
                            Type = infosBase.Type,
                            Branche = infosBase.Branche.Code,
                            BrancheLib = infosBase.Branche.Nom,
                            Cible = infosBase.Branche.Cible.Code,
                            CibleLib = infosBase.Branche.Cible.Nom,
                            CourtierGestionnaire = infosBase.CabinetGestionnaire.Code,
                            Descriptif = infosBase.Descriptif,
                            CodeInterlocuteur = infosBase.CabinetGestionnaire.Code,
                            NomInterlocuteur = infosBase.CabinetGestionnaire.Inspecteur,
                            CodePreneurAssurance = Convert.ToInt32(infosBase.PreneurAssurance.Code),
                            NomPreneurAssurance = infosBase.PreneurAssurance.NomAssure,
                            PeriodiciteCode = infosBase.Periodicite,
                            Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                            Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur
                        };
                    }
                    AlbSessionHelper.SetCurrentEntityById(tId[0] + "_" + tId[1] + "_" + tId[2], model.Contrat);

                    var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt) {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            break;
                        default:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }

                    codeOffre = model.Contrat.CodeContrat;
                    version = model.Contrat.VersionContrat.ToString();
                    type = model.Contrat.Type;
                    break;
            }
            LoadDataEngagement(tId[0], tId[1], tId[2]);
            model.PageTitle = "Engagements par traité";
            if (model.Offre != null || model.Contrat != null) {
                model.AfficherBandeau = DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }
            
            SetArbreNavigation();
            model.Bandeau = null;
            SetBandeauNavigation(id);
        }

        private void LoadDataEngagement(string codeOffre, string version, string type) {
            ModeleEngagementsPage modele = new ModeleEngagementsPage();
            EngagementDto result = null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                
                result = client.Channel.InitEngagement(codeOffre, version, type, model.NumAvenantPage, model.CodePeriode, model.ModeNavig.ParseCode<ModeConsultation>(), model.IsReadOnly, false, GetUser(), model.ActeGestion, model.AccessMode, "engagement");
            }
            if (result != null) {
                modele = (ModeleEngagementsPage)result;
            }

            model.Nature = modele.Nature;
            model.PartAlb = modele.PartAlb;
            model.Couverture = modele.Couverture;
            model.Traites = modele.Traites;
            model.DateDeb = modele.DateDeb;
            model.DateFin = modele.DateFin;
            model.CommentForce = modele.CommentForce;
          
            DateTime? finEffetDossier = new DateTime();
            if (model.Offre != null) {
                finEffetDossier = model.Offre.DateFinEffetGarantie;
                if (!finEffetDossier.HasValue) {
                    finEffetDossier = AlbConvert.GetFinPeriode(modele.DateDeb, model.Offre.DureeGarantie.HasValue ? model.Offre.DureeGarantie.Value : 0, model.Offre.UniteDeTemps != null ? model.Offre.UniteDeTemps.Code : string.Empty);
                }
            }
            else if (model.Contrat != null) {
                finEffetDossier = AlbConvert.ConvertStrToDate(string.Format("{0}/{1}/{2}", model.Contrat.FinEffetJour, model.Contrat.FinEffetMois, model.Contrat.FinEffetAnnee));
                if (!finEffetDossier.HasValue) {
                    finEffetDossier = AlbConvert.GetFinPeriode(modele.DateDeb, model.Contrat.DureeGarantie, model.Contrat.UniteDeTemps);
                }
            }

            if (!model.DateFin.HasValue) {
                model.DateFin = finEffetDossier;
            }
            if (result != null) {
                var unites = result.LCIUnites.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                if (!string.IsNullOrEmpty(result.LCIUnite)) {
                    var sItem = unites.FirstOrDefault(x => x.Value == result.LCIUnite);
                    if (sItem != null)
                        sItem.Selected = true;
                }

                var types = result.LCITypes.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                if (!string.IsNullOrEmpty(result.LCIType)) {
                    var sItem = types.FirstOrDefault(x => x.Value == result.LCIType);
                    if (sItem != null)
                        sItem.Selected = true;
                }

                //Région LCI Generale
                model.LCIGenerale = new Models.ModelesLCIFranchise.ModeleLCIFranchise {
                    TypeVue = AlbConstantesMetiers.ExpressionComplexe.LCI,
                    TypeAppel = AlbConstantesMetiers.TypeAppel.Generale,
                    Valeur = result.LCIValeur,
                    Unite = result.LCIUnite,
                    Unites = unites,
                    Type = result.LCIType,
                    Types = types,
                    IsIndexe = result.LCIIndexee,
                    LienComplexe = result.LienComplexeLCI,
                    LibComplexe = result.LibComplexeLCI,
                    CodeComplexe = result.CodeComplexeLCI
                };
            }
            modele.LCIGenerale = model.LCIGenerale;


        }

        private void UpdateDataEngagement(string codeOffre, string version, string type, EngagementDto engagement, string field, string acteGestion, string codePeriode) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                var finOffreClient = client.Channel;
                finOffreClient.UpdateEngagement(codeOffre, version, type, engagement, field, GetUser(), acteGestion, codePeriode);
            }
        }

        private void SetBandeauNavigation(string id) {
            if (model.AfficherBandeau) {
                model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                //Gestion des Etapes
                if (model.Offre != null) {
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    model.Navigation = new Navigation_MetaModel {
                        Etape = Navigation_MetaModel.ECRAN_ENGAGEMENTS,
                        IdOffre = model.Offre.CodeOffre,
                        Version = model.Offre.Version
                    };
                }
                else if (model.Contrat != null) {
                    string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt) {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            model.Bandeau.StyleBandeau = model.ScreenType;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        default:
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                    model.Navigation = new Navigation_MetaModel {
                        Etape = Navigation_MetaModel.ECRAN_ENGAGEMENTS,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                }
            }
        }

        private void SetArbreNavigation() {
            if (model.Offre != null) {
                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre("Engagement");
            }
            else if (model.Contrat != null) {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Engagement");
            }
        }

        private void SetCdPeriodeGuidTab(string id, out string outid) {
            model.CodePeriode = id.Contains("cdPeriode") ? id.Split(new[] { "cdPeriode" }, StringSplitOptions.None)[1] : string.Empty;
            outid = id.Replace("cdPeriode" + model.CodePeriode + "cdPeriode", string.Empty);
        }

        private void UpdateEngagements(string codeOffre, string version, string type, string codeAvn, string argEngagement, string argModelCommentForce, string field,
                             string tabGuid, string valeurLCI, string typeLCI, string uniteLCI, bool isIndexeeLCI, string lienCpxLCI, string modeNavig, string acteGestion, string codePeriode) {
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn)
                 && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>()) {
                JavaScriptSerializer serialiser = AlbJsExtendConverter<ModeleEngagementsPage>.GetSerializer();
                var engagement = serialiser.ConvertToType<ModeleEngagementsPage>(serialiser.DeserializeObject(argEngagement));
                engagement.LCIGenerale = new Models.ModelesLCIFranchise.ModeleLCIFranchise {
                    Valeur = valeurLCI,
                    Unite = uniteLCI,
                    Type = typeLCI,
                    IsIndexe = isIndexeeLCI,
                    LienComplexe = lienCpxLCI
                };
                //var modelCommentForce = serialiser.ConvertToType<ModeleEngagementsPage>(serialiser.DeserializeObject(argModelCommentForce));

                //if (modelCommentForce != null)
                //    engagement.CommentForce = modelCommentForce.CommentForce;

                if (!string.IsNullOrEmpty(argModelCommentForce)) {
                    engagement.CommentForce = argModelCommentForce.Replace("\r\n", "<br>").Replace("\n", "<br>");
                }
                else {
                    engagement.CommentForce = string.Empty;
                }
                UpdateDataEngagement(codeOffre, version, type, ModeleEngagementsPage.LoadDto(engagement), field, acteGestion, codePeriode);

            }
            string id = string.Format("{0}_{1}_{2}", codeOffre, version, type);
            model.PageTitle = "Engagements par traité";
            LoadInfoPage(id);
        }

        private void UpdateCommentaireOnly(string codeOffre, string version, string type, string argModelCommentForce, string codePeriode) {
            string commentaireDecode = Server.UrlDecode(argModelCommentForce);

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                client.Channel.SaveEngagementCommentaire(codeOffre, version, type, commentaireDecode, codePeriode);
            }
        }

        #endregion
    }
}
