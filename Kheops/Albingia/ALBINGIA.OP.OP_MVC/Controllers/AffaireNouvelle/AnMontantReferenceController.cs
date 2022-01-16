using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Filters;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesMontantReference;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.MontantReference;
using OP.WSAS400.DTO.NavigationArbre;
using OP.WSAS400.DTO.Offres.Parametres;
using OPServiceContract.ICommon;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers.AffaireNouvelle {
    public class AnMontantReferenceController : ControllersBase<ModeleMontantReferencePage> {
        private static List<AlbSelectListItem> _lstTypesAcc;
        public static List<AlbSelectListItem> LstTypesAcc {
            get {
                //Si les données ont déjà été chargé en mémoire, on renvoit une nouvelle instance de la liste
                if (_lstTypesAcc != null) {
                    var toReturn = new List<AlbSelectListItem>();
                    _lstTypesAcc.ForEach(elm => toReturn.Add(new AlbSelectListItem {
                        Value = elm.Value,
                        Text = elm.Text,
                        Title = elm.Title,
                        Selected = false
                    }));
                    return toReturn;
                }

                //Sinon on charge les données à partir de la BDD et on renvoit une nouvelle instance de la liste
                List<AlbSelectListItem> value = new List<AlbSelectListItem>();
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                    var serviceContext = client.Channel;
                    var lstTypeAcc = serviceContext.GetListeTypesAccessoire();
                    lstTypeAcc.ForEach(elm => value.Add(new AlbSelectListItem {
                        Value = elm.Code.ToString(),
                        Text = elm.Code + " - " + elm.Libelle,
                        Title = elm.Code + " - " + elm.Libelle,
                        Selected = false
                    }
                    ));
                }
                _lstTypesAcc = value;
                return value;
            }
        }

        #region Méthodes publiques
        [AlbVerifLockedOffer("id", "P")]
        [ErrorHandler]
        [WhitespaceFilter]
        public ActionResult Index(string id) {
            model.PageTitle = "Montant de référence";
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }
        [ErrorHandler]
        public ActionResult LoadMontantFormule(string codeOffre, string version, string type, string codeAvn, string codeRsq, string codeForm, string modeNavig) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                var serviceContext = client.Channel;
                var montantFormule = new ModeleMontantReference();

                var result = serviceContext.GetMontantFormule(codeOffre, version, type, codeAvn, codeRsq, codeForm, modeNavig.ParseCode<ModeConsultation>());
                if (result != null)
                    montantFormule = (ModeleMontantReference)result;
                return PartialView("ModificationMontantFormule", montantFormule);
            }
        }
        [ErrorHandler]
        public ActionResult LoadMontantTotal(string codeOffre, string version, string type, string codeAvn, string modeNavig) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                var serviceContext = client.Channel;
                var montantTotal = new ModeleMontantReferencePage();

                var result = serviceContext.GetMontantTotal(codeOffre, version, type, codeAvn, modeNavig.ParseCode<ModeConsultation>());
                if (result != null)
                    montantTotal = (ModeleMontantReferencePage)result;
                return PartialView("ModificationMontantTotal", montantTotal);
            }
        }

        [ErrorHandler]
        public ActionResult ReloadMontant(string codeOffre, string version, string type, string codeAvn, string mode, bool isReadonly, string modeNavig) {
            var model = new ModeleMontantReferencePage();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                var serviceContext = client.Channel;
                var result = serviceContext.ReloadMontantReference(codeOffre, version, type, codeAvn, mode, isReadonly, modeNavig.ParseCode<ModeConsultation>(), GetUser());
                if (result != null) {
                    model = (ModeleMontantReferencePage)result;
                }
                return PartialView("MontantReferenceBody", model);
            }
        }

        [ErrorHandler]
        public void ValidMontantFormule(string codeOffre, string version, string type, string codeRsq, string codeForm,
            decimal mntForce, bool mntProvi, string mntAcquis, bool chkMntAcquis) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                var serviceContext = client.Channel;
                serviceContext.ValidMontantFormule(codeOffre, version, type, codeRsq, codeForm, mntForce, mntProvi, Convert.ToDecimal(mntAcquis), chkMntAcquis);
            }
        }

        [ErrorHandler]
        public void ValidMontantTotal(string codeOffre, string version, string type, decimal mntForce, decimal mntAcquis, bool checkedA, bool checkedP) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                var serviceContext = client.Channel;
                serviceContext.ValidMontantTotal(codeOffre, version, type, mntForce, mntAcquis, checkedA, checkedP);
            }
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult UpdateRedirect(string codeOffre, string version, string type, string codeAvn, bool topForce, bool topAcquis, bool topForceTotal, string argModelCommentForce, Int64 codeCommentForce, string tabGuid, string saveCancel, string paramRedirect, string modeNavig, string acteGestion) {
            var result = UpdateMontantRef(codeOffre, version, type, codeAvn, topForce, topAcquis, topForceTotal, argModelCommentForce, codeCommentForce, tabGuid, modeNavig.ParseCode<ModeConsultation>(), acteGestion);

            if (string.IsNullOrEmpty(paramRedirect)) {
                return null;
            }
            var tabParamRedirect = paramRedirect.Split('/');
            return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });

        }

        [ErrorHandler]
        public ActionResult UpdateInPage(string codeOffre, string version, string type, string codeAvn, bool topForce, bool topAcquis, bool topForceTotal, string argModelCommentForce, Int64 codeCommentForce, string tabGuid, string saveCancel, string modeNavig, string acteGestion) {
            var model = new ModeleMontantReferencePage();
            var result = UpdateMontantRef(codeOffre, version, type, codeAvn, topForce, topAcquis, topForceTotal, argModelCommentForce, codeCommentForce, tabGuid, modeNavig.ParseCode<ModeConsultation>(), acteGestion);
            if (result != null) {
                model = (ModeleMontantReferencePage)result;
                model.ListeTypesFraisAcc = LstTypesAcc;
                model.TabGuid = tabGuid;
                model.ModeNavig = modeNavig;
                model.CodePolicePage = codeOffre;
                model.VersionPolicePage = version;
                model.TypePolicePage = type;
                model.NumAvenantPage = codeAvn;
            }
            return PartialView("MontantReferenceBody", model);
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string argModelCommentForce, Int64 codeCommentForce, string tabGuid, string paramRedirect, string modeNavig, string addParamType, string addParamValue) {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            var isReadonly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvn);
            if (cible == "Engagements" && numAvn != "0" && !string.IsNullOrEmpty(numAvn)) {
                cible = "EngagementPeriodes";
            }

            if (cible == "RechercheSaisie")
                return RedirectToAction(job, cible);
            #region Arbre de navigation
            if (cible == "Quittance") {
                if (type == AlbConstantesMetiers.TYPE_OFFRE) {
                    cible = "Cotisations";
                }
                if (!isReadonly && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>()) {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                        var serviceContext = client.Channel;
                        if (!string.IsNullOrEmpty(argModelCommentForce)) {
                            argModelCommentForce = argModelCommentForce.Replace("\r\n", "<br>").Replace("\n", "<br>");
                        }

                        serviceContext.SaveCommentairesMontantRef(codeOffre, version, type, codeCommentForce, argModelCommentForce);
                    }
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                        var serviceContext = client.Channel;
                        serviceContext.SetTrace(new TraceDto {
                            CodeOffre = codeOffre.PadLeft(9, ' '),
                            Version = Convert.ToInt32(version),
                            Type = type,
                            EtapeGeneration = ContextStepName.EditionMontantsReference.AsCode(),
                            NumeroOrdreDansEtape = 63,
                            NumeroOrdreEtape = 1,
                            Perimetre = ContextStepName.EditionMontantsReference.AsCode(),
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

            }

            if (cible == "ChoixClauses") {
                if (isReadonly && ModeConsultation.Historique == modeNavig.ParseCode<ModeConsultation>()) {
                    return RedirectToAction("Index", "ChoixClauses", new { id = codeOffre + "_" + version + "_" + type + "_¤AnMontantReference¤Index¤" + codeOffre + "£" + version + "£" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
                }

            }



            #endregion
            if (!string.IsNullOrEmpty(paramRedirect)) {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }
            return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
        }

        [ErrorHandler]
        public ActionResult ResetPage(string codeOffre, string version, string type, string codeAvn, string modeNavig, string acteGestion) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                var finOffreClient = client.Channel;
                var model = new ModeleMontantReferencePage();
                var result = finOffreClient.UpdateMontantRef(codeOffre, version, type, codeAvn, false, false, false, string.Empty, 0, GetUser(), modeNavig.ParseCode<ModeConsultation>(), acteGestion, true);
                if (result != null) {
                    model = (ModeleMontantReferencePage)result;
                    model.ListeTypesFraisAcc = LstTypesAcc;
                }
                return PartialView("MontantReferenceBody", model);
            }
        }


        #endregion
        #region Méthodes privées
        protected override void LoadInfoPage(string id) {
            string[] tId = id.Split('_');
            if (tId[2] == "O") {
                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                    var CommonOffreClient = chan.Channel;
                    model.Offre = new Offre_MetaModel();
                    model.Offre.LoadInfosOffre(CommonOffreClient.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig));
                }
                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
            }
            if (tId[2] == "P") {
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
                var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                switch (typeAvt) {
                    case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                        break;
                    default:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                        break;
                }
            }
            if (model.Offre != null || model.Contrat != null) {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }

            SetArbreNavigation();

            model.Bandeau = null;
            SetBandeauNavigation(id);

            model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), tId[0] + "_" + tId[1] + "_" + tId[2], model.NumAvenantPage);

            //Montants de référence
            LoadMontantsReference(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
        }

        private void LoadMontantsReference(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                var serviceContext = client.Channel;
                var result = serviceContext.InitInfoMontantReference(codeOffre, version, type, codeAvn, model.IsReadOnly, modeNavig, GetUser(), model.ActeGestion);
                if (result != null) {
                    var model = (ModeleMontantReferencePage)result;

                    base.model.Periodicite = model.Periodicite;
                    base.model.EcheancePrincipale = model.EcheancePrincipale;
                    base.model.ProchaineEcheance = model.ProchaineEcheance;
                    base.model.PeriodeDeb = model.PeriodeDeb;
                    base.model.PeriodeFin = model.PeriodeFin;
                    base.model.TypeFraisAccessoires = model.TypeFraisAccessoires;
                    base.model.Montant = model.Montant;
                    base.model.TaxeAttentat = model.TaxeAttentat;
                    base.model.MontantForce = model.MontantForce;
                    base.model.CommentForce = model.CommentForce;
                    base.model.CodeCommentaire = model.CodeCommentaire;
                    base.model.MontantsReference = model.MontantsReference;

                    base.model.TotalMntAcquis = model.TotalMntAcquis;
                    base.model.TotalMntForce = model.TotalMntForce;
                    base.model.TotalMntCalcule = model.TotalMntCalcule;
                    base.model.TotalMntProvi = model.TotalMntProvi;

                    DateTime? finEffetContrat = null;
                    if (base.model.Offre != null) {
                        base.model.Offre.DureeGarantie = model.DureeGarantie;
                        base.model.Offre.UniteDeTemps = new ParametreDto() {
                            Code = model.UniteTemps == null ? "" : model.UniteTemps
                        };

                        finEffetContrat = base.model.Offre.DateFinEffetGarantie;
                        if (!finEffetContrat.HasValue) {
                            finEffetContrat = AlbConvert.GetFinPeriode(base.model.Offre.DateEffetGarantie, base.model.Offre.DureeGarantie.HasValue ? base.model.Offre.DureeGarantie.Value : 0, base.model.Offre.UniteDeTemps.Code);
                        }
                    }
                    if (base.model.Contrat != null) {
                        base.model.Contrat.DureeGarantie = model.DureeGarantie;
                        base.model.Contrat.UniteDeTemps = model.UniteTemps;

                        finEffetContrat = AlbConvert.ConvertStrToDate(string.Format("{0}/{1}/{2}", base.model.Contrat.FinEffetJour, base.model.Contrat.FinEffetMois, base.model.Contrat.FinEffetAnnee));
                        if (!finEffetContrat.HasValue) {
                            finEffetContrat = AlbConvert.GetFinPeriode(AlbConvert.ConvertStrToDate(string.Format("{0}/{1}/{2}", base.model.Contrat.DateEffetJour, base.model.Contrat.DateEffetMois, base.model.Contrat.DateEffetAnnee)), base.model.Contrat.DureeGarantie, base.model.Contrat.UniteDeTemps);
                        }
                    }
                    base.model.PeriodeFin = finEffetContrat;

                }
                model.ListeTypesFraisAcc = LstTypesAcc;
            }
        }
        private void SetBandeauNavigation(string id) {
            if (model.AfficherBandeau) {
                model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                //Gestion des Etapes
                if (model.Offre != null) {
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    model.Navigation = new Navigation_MetaModel {
                        Etape = Navigation_MetaModel.ECRAN_COTISATIONS,
                        IdOffre = model.Offre.CodeOffre,
                        Version = model.Offre.Version
                    };
                }
                if (model.Contrat != null) {
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
                            model.Bandeau.StyleBandeau = model.ScreenType;
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
                        Etape = Navigation_MetaModel.ECRAN_COTISATIONS,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                    //model.NavigationArbre = GetNavigationArbreRegule(ContentData, "Regule");
                    //model.NavigationArbre.IsRegule = typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL;
                }
            }
        }
        private void SetArbreNavigation() {
            if (model.Offre != null) {
                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre("MontantRef");
            }
            if (model.Contrat != null) {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("MontantRef");
            }
        }
        private MontantReferenceDto UpdateMontantRef(string codeOffre, string version, string type, string codeAvn, bool topForce, bool topAcquis, bool topForceTotal,
            string argModelCommentForce, Int64 codeCommentForce, string tabGuid, ModeConsultation modeNavig, string acteGestion) {
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn)
                 && modeNavig == ModeConsultation.Standard) {
                //JavaScriptSerializer serialiser = AlbJsExtendConverter<ModeleMontantReferencePage>.GetSerializer();
                //var modelCommentForce = serialiser.ConvertToType<ModeleMontantReferencePage>(serialiser.DeserializeObject(argModelCommentForce));
                if (!string.IsNullOrEmpty(argModelCommentForce)) {
                    argModelCommentForce = argModelCommentForce.Replace("\r\n", "<br>").Replace("\n", "<br>");
                }

                return UpdateDataMontantRef(codeOffre, version, type, codeAvn, topForce, topAcquis, topForceTotal, argModelCommentForce, codeCommentForce, modeNavig, acteGestion);
            }
            return null;
        }
        private MontantReferenceDto UpdateDataMontantRef(string codeOffre, string version, string type, string codeAvn, bool topForce, bool topAcquis, bool topForceTotal, string commentForce, Int64 codeCommentForce, ModeConsultation modeNavig, string acteGestion) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                var finOffreClient = client.Channel;
                return finOffreClient.UpdateMontantRef(codeOffre, version, type, codeAvn, topForce, topAcquis, topForceTotal, commentForce, codeCommentForce, GetUser(), modeNavig, acteGestion, false);
            }
        }
        #endregion
    }
}
