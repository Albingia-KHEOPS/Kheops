using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModeleConditionsGarantie;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO;
using OP.WSAS400.DTO.NavigationArbre;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ICommon;
using OPServiceContract.ISaisieCreationOffre;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers {
    public class AttentatGareatController : ControllersBase<ModeleAttentatPage> {
        /// <summary>
        /// Indexes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        
        [ErrorHandler]
        public ActionResult Index(string id) {

            id = InitializeParams(id);
            LoadInfoPage(id);

            if (decimal.TryParse(this.model.CATNAT, out decimal i) && i > 0) {
                string[] tId = id.Split('_');
                var cible = "AnMontantReference";
                return Redirection(cible, "Index", tId[2], tId[0], tId[1], GetSurroundedTabGuid(model.TabGuid), string.Empty, model.ModeNavig, model.AddParamType, model.AddParamValue, model.CommentForce);
            }

            return View(model);
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult UpdateRedirect(string codeOffre, string version, string type, string codeAvn, string field, string value, string argModelCommentForce, string tabGuid, string saveCancel, string paramRedirect, string modeNavig, string acteGestion) {
            UpdateAttentats(codeOffre, version, type, codeAvn, field, value, argModelCommentForce, tabGuid, modeNavig, acteGestion);
            if (string.IsNullOrEmpty(paramRedirect)) {
                return null;
            }
            var tabParamRedirect = paramRedirect.Split('/');
            return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
        }
        [ErrorHandler]
        public ActionResult UpdateInPage(string codeOffre, string version, string type, string codeAvn, string field, string value, string argModelCommentForce, string tabGuid, string saveCancel, string modeNavig, string acteGestion) {
            UpdateAttentats(codeOffre, version, type, codeAvn, field, value, argModelCommentForce, tabGuid, modeNavig, acteGestion: acteGestion);


            if (saveCancel == "1") {
                return RedirectToRoute("/RechercheSaisie/Index");
            }
            return PartialView("AttentatBody", model);
        }

        private void UpdateAttentats(string codeOffre, string version, string type, string codeAvn, string field, string value, string argModelCommentForce, string tabGuid, string modeNavig, string acteGestion) {
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn)
                 && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>()) {
                //JavaScriptSerializer serialiser = AlbJsExtendConverter<ModeleAttentatPage>.GetSerializer();
                //var modelCommentForce = serialiser.ConvertToType<ModeleAttentatPage>(serialiser.DeserializeObject(argModelCommentForce));

                //UpdateDataAttentat(codeOffre, version, type, field, value, modelCommentForce.CommentForce, modeNavig);

                if (!string.IsNullOrEmpty(argModelCommentForce))
                    argModelCommentForce = argModelCommentForce.Replace("\r\n", "<br>").Replace("\n", "<br>");
                else
                    argModelCommentForce = string.Empty;

                UpdateDataAttentat(codeOffre, version, type, field, value, argModelCommentForce, modeNavig, acteGestion);

                var id = string.Format("{0}_{1}_{2}", codeOffre, version, type);
                model.PageTitle = "Engagements Attentats / GAREAT";
                LoadInfoPage(id);
            }
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string type, string codeOffre, string version, string tabGuid, string paramRedirect, string modeNavig, string addParamType, string addParamValue, string commentForce) {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);

            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvn)) {
                if (!string.IsNullOrEmpty(commentForce))
                    commentForce = commentForce.Replace("\r\n", "<br>").Replace("\n", "<br>");
                else
                    commentForce = string.Empty;

                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                    var serviceContext = client.Channel;
                    serviceContext.UpdateAttentatComment(codeOffre, version, type, commentForce);
                }
            }
            if (cible == "RechercheSaisie")
                return RedirectToAction(job, cible);

            if (cible == "Cotisations" || cible == "Quittance") {
                if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvn)
                    && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>()) {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                        var serviceContext = client.Channel;
                        serviceContext.SetTrace(new TraceDto {
                            CodeOffre = codeOffre.PadLeft(9, ' '),
                            Version = Convert.ToInt32(version),
                            Type = type,
                            EtapeGeneration = ContextStepName.Attentat.AsCode(),
                            NumeroOrdreDansEtape = 63,
                            NumeroOrdreEtape = 1,
                            Perimetre = ContextStepName.Attentat.AsCode(),
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

            if (!string.IsNullOrEmpty(paramRedirect)) {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }
            return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
        }
        [AlbAjaxRedirect]
        public RedirectToRouteResult RedirectionAnnuler(string type, string codeOffre, string version, string tabGuid, string modeNavig, string addParamType, string addParamValue) {
            //existEngCnx=true si le contrat contient une connexité de type engagement
            var cible = HasEngCnx() ? "EngagementsConnexite" : "Engagements";

            return RedirectToAction("Index", cible, new { id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
        }

        #region Méthode privée

        protected override void LoadInfoPage(string id) {
            string[] tId = id.Split('_');
            switch (tId[2]) {
                case "O":
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>()) {
                        var policeServicesClient = client.Channel;
                        model.Offre = new Offre_MetaModel();
                        model.Offre.LoadOffre(policeServicesClient.OffreGetDto(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig.ParseCode<ModeConsultation>()));
                        //model.Offre.LoadOffre(policeServicesClient.OffreGetDto(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig));
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    }
                    break;
                case "P":
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                        var serviceContext = client.Channel;
                        model.Contrat = serviceContext.GetContrat(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                    }
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                    break;
            }
            model.PageTitle = "Engagements Attentats / GAREAT";
            if (model.Offre != null || model.Contrat != null) {
                model.AfficherBandeau = DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }
            #region Navigation Arbre
            SetArbreNavigation();
            #endregion
            #region Bandeau
            model.Bandeau = null;
            SetBandeauNavigation(id);
            #endregion
            if (model.Offre != null) {
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
                LoadDataAttentat(model.Offre.CodeOffre, model.Offre.Version, model.Offre.Type, string.Empty, model.ModeNavig);
            }
            else if (model.Contrat != null) {
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
                LoadDataAttentat(model.Contrat.CodeContrat, int.Parse(model.Contrat.VersionContrat.ToString()), model.Contrat.Type, model.NumAvenantPage, model.ModeNavig);
            }
        }

        private void LoadDataAttentat(string codeOffre, int? version, string type, string codeAvn, string modeNavig) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                var finOffreClient = client.Channel;
                var result = finOffreClient.InitAttentat(codeOffre, version.HasValue ? version.Value.ToString() : "0", type, codeAvn, model.IsReadOnly, modeNavig.ParseCode<ModeConsultation>(), GetUser(), model.ActeGestion);

                ModeleAttentatPage attentatModele = ((ModeleAttentatPage)result);

                model.LCI = attentatModele.LCI;
                model.Capitaux = attentatModele.Capitaux;
                model.Surface = attentatModele.Surface;
                model.CATNAT = attentatModele.CATNAT;
                model.CapitauxForces = attentatModele.CapitauxForces;

                model.ParamStandard = attentatModele.ParamStandard;
                model.ParamRetenus = attentatModele.ParamRetenus;

                model.LibelleConstruit = model.ParamStandard.Tranche == "C" ? "Le contrat est soumis au GAREAT" : "";
                model.CommentForce = attentatModele.CommentForce;
            }
        }

        private void UpdateDataAttentat(string codeOffre, string version, string type, string field, string value, string commentForce, string modeNavig, string acteGestion) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                var finOffreClient = client.Channel;
                finOffreClient.UpdateAttentat(codeOffre, version, type, field, value, commentForce, GetUser(), modeNavig.ParseCode<ModeConsultation>(), acteGestion);
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
                model.NavigationArbre = GetNavigationArbre("AttentatGareat");
            }
            else if (model.Contrat != null) {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("AttentatGareat");
            }
        }
        #endregion
    }
}
