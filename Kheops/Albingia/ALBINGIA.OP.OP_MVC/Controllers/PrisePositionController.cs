using Albingia.Common;
using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Mvc.Common;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.CustomResult;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using Mapster;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class PrisePositionController : ControllersBase<ModelePrisePositionPage>
    {

        [ErrorHandler]
        public ActionResult Index(string id)
        {
            Model.PageTitle = "Prendre position";
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public ActionResult EnregistrerPosition(string codeOffre, string version, string type, string tabGuid, string modeNavig, string saveCancel, string addParamType, string addParamValue, string position, string motif, string oldSituation, string oldEtat) {
            string cible = "RechercheSaisie";
            if (saveCancel == "1") {
                addParamValue = null;
            }
            else {
                var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
                if ((!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvn) || (oldEtat == "V" && oldSituation == "A"))
                    && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>()) {
                    var newEtat = string.Empty;
                    var newSituation = string.Empty;

                    if ((oldEtat == "N" && oldSituation == "A") ||
                        (oldEtat == "N" && oldSituation == "W") ||
                        (oldEtat == "A" && oldSituation == "W") ||
                        (oldEtat == "A" && oldSituation == "A")
                        ) {
                        if (position == "accepter") {
                            newEtat = "A";
                            newSituation = "A";
                            if (type == AlbConstantesMetiers.TYPE_OFFRE)
                                cible = "ModifierOffre";
                            if (type == AlbConstantesMetiers.TYPE_CONTRAT && (numAvn == "0" || string.IsNullOrEmpty(numAvn)))
                                cible = "AnInformationsGenerales";
                            if (type == AlbConstantesMetiers.TYPE_CONTRAT && !string.IsNullOrEmpty(numAvn) && numAvn != "0")
                                cible = "AvenantInfoGenerales";
                        }
                        if (position == "attente") {
                            newEtat = "N";
                            newSituation = "A";
                        }
                        if (position == "refus") {
                            newEtat = "N";
                            newSituation = "W";
                        }
                    }


                    if (oldEtat == "V" && type == AlbConstantesMetiers.TYPE_OFFRE) {
                        if (position == "refus") {
                            newEtat = "V";
                            newSituation = "W";
                        }
                    }

                    if (motif == "AAS" && position != "refus") {
                        newEtat = "A";
                    }

                    // Enregistrement
                    if (!string.IsNullOrEmpty(newEtat) && !string.IsNullOrEmpty(newSituation)) {
                        using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IConfirmationSaisie>()) {
                            client.Channel.EnregistrerNouvellePosition(codeOffre, version, type, newEtat, newSituation, motif, GetUser());
                        }
                    }
                }
            }

            RefreshLock(cible, tabGuid);
            return RedirectToAction(
                "Index",
                cible,
                new { id = AlbParameters.BuildStandardId(new Folder(new[] { codeOffre, version, type }), tabGuid, addParamValue, modeNavig) });
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public ActionResult Annuler(string codeOffre, string version, string type, string tabGuid, string addParamType, string addParamValue, string modeNavig)
        {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            if (string.IsNullOrEmpty(numAvn)) {
                numAvn = "0";
            }
            var folder = string.Format("{0}_{1}_{2}_{3}", codeOffre, version, type, numAvn);
            var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, folder);

            //Déverouillage de l'offre/contrat
            Common.CommonVerouillage.DeverrouilleFolder(codeOffre, version, type, numAvn, tabGuid, false, false, isModifHorsAvn, true);
            return RedirectToAction("Index", "RechercheSaisie", new { id = AlbParameters.BuildStandardId(new Folder(new[] { codeOffre, version, type }), tabGuid, addParamValue, modeNavig) });
        }

        [HttpPost]
        [ErrorHandler]
        public JsonResult GetMotifsSituations() {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IReferentielPort>()) {
                return JsonNetResult.NewResultToGet(client.Channel.GetMotifsSituations().ToArray());
            }
        }

        [HttpPost]
        [HandleJsonError]
        public void ClasserRelancesSansSuite(ModelRelances modelRelances) {
            var list = modelRelances.Relances.Where(x => x.Situation == "S");
            bool mustUnlock = true;
            var affaireIds = list.Select(x => x.Adapt<AffaireId>());
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffairePort>()) {
                try {
                    if (client.Channel.TryLockAffaireList(affaireIds, "Gestion", "Offre sans-suite")?.Any() ?? false) {
                        mustUnlock = false;
                        throw new BusinessValidationException(new ValidationError("Au moins une offre est verrouillée. Veuillez ressayer ultérieurement"));
                    }
                    client.Channel.ClasserOffresSansSuite(list.Select(x => (x.Adapt<AffaireId>(), x.MotifStatut)));
                }
                finally {
                    if (mustUnlock) {
                        try {
                            client.Channel.UnockAffaireList(affaireIds);
                        }
                        catch {
                            AlbLog.Warn($"Unable to unlock {string.Join(", ", affaireIds.Select(x => $"{x.CodeAffaire} ({x.NumeroAliment})"))}");
                        }
                    }
                }
            }
        }

        #region Méthodes privées

        protected override void LoadInfoPage(string id)
        {
            string codeOffre = string.Empty;
            string version = string.Empty;
            string type = string.Empty;

            string[] tId = id.Split('_');
            switch (tId[2])
            {
                case AlbConstantesMetiers.TYPE_OFFRE:
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                    {
                        model.Offre = new Offre_MetaModel();
                        model.Offre.LoadOffre(client.Channel.OffreGetDto(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig.ParseCode<ModeConsultation>()));
                    }
                    Model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    codeOffre = model.Offre.CodeOffre;
                    version = model.Offre.Version.Value.ToString();
                    type = model.Offre.Type;
                    break;
                case AlbConstantesMetiers.TYPE_CONTRAT:
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                    {
                        client.Channel.GetContrat(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                    }
                    var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt)
                    {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            Model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                            Model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            Model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            break;
                        default:
                            Model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                    codeOffre = model.Contrat.CodeContrat;
                    version = model.Contrat.VersionContrat.ToString();
                    type = model.Contrat.Type;
                    break;
            }
            model.IsReadOnly = GetIsReadOnly(model.TabGuid, codeOffre + "_" + version + "_" + type, model.NumAvenantPage);

            if (model.Offre != null || model.Contrat != null)
            {
                Model.AfficherBandeau = base.DisplayBandeau(true, id);
                Model.AfficherNavigation = Model.AfficherBandeau;
            }

            SetArbreNavigation();
            Model.Bandeau = null;
            SetBandeauNavigation(id);
            this.model.ListeMotifsAttente = LstMotifs;
            this.model.ListeMotifsRefus = LstMotifs;
        }

        private void SetBandeauNavigation(string id)
        {
            if (model.AfficherBandeau)
            {
                model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                //Gestion des Etapes
                if (model.Offre != null)
                {
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    model.Navigation = new Navigation_MetaModel
                    {
                        Etape = Navigation_MetaModel.ECRAN_ENGAGEMENTS,
                        IdOffre = model.Offre.CodeOffre,
                        Version = model.Offre.Version
                    };
                }
                else if (model.Contrat != null)
                {
                    string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt)
                    {
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
                    model.Navigation = new Navigation_MetaModel
                    {
                        Etape = Navigation_MetaModel.ECRAN_ENGAGEMENTS,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                }
            }
        }

        private void SetArbreNavigation()
        {
            if (model.Offre != null)
                model.NavigationArbre = GetNavigationArbre(string.Empty, returnEmptyTree: true);
            else if (model.Contrat != null)
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle(string.Empty, returnEmptyTree: true);
        }

        private void RefreshLock(string controller, string tabGuid) {
            if (controller?.ToUpperInvariant() == "MODIFIEROFFRE") {
                var g = Guid.TryParse(tabGuid.Replace(PageParamContext.TabGuidKey, string.Empty).ToLower(), out var x) ? x : Guid.Empty;
                var acces = MvcApplication.ListeAccesAffaires.First(a => a.ModeAcces == AccesOrigine.PrisePosition && a.TabGuid == g);
                MvcApplication.ListeAccesAffaires.Remove(acces);
                acces.ModeAcces = AccesOrigine.Modifier;
                MvcApplication.ListeAccesAffaires.Add(acces);
            }
            else {
                FolderController.DeverrouillerAffaire(tabGuid);
            }
        }

        private static List<AlbSelectListItem> _lstMotifs;
        public static List<AlbSelectListItem> LstMotifs
        {
            get
            {
                //Si les données ont déjà été chargé en mémoire, on renvoit une nouvelle instance de la liste
                if (_lstMotifs != null)
                {
                    var toReturn = new List<AlbSelectListItem>();
                    _lstMotifs.ForEach(elm => toReturn.Add(new AlbSelectListItem
                    {
                        Value = elm.Value,
                        Text = elm.Text,
                        Title = elm.Title,
                        Selected = false
                    }));
                    return toReturn;
                }

                //Sinon on charge les données à partir de la BDD et on renvoit une nouvelle instance de la liste
                List<AlbSelectListItem> value = new List<AlbSelectListItem>();
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IConfirmationSaisie>())
                {
                    var serviceContext=client.Channel;
                    var lstMotifs = serviceContext.GetListeMotifs();
                    lstMotifs.ForEach(elm => value.Add(new AlbSelectListItem
                    {
                        Value = elm.Code.ToString(),
                        Text = elm.Code + " - " + elm.Libelle,
                        Title = elm.Code + " - " + elm.Libelle,
                        Selected = false
                    }
                    ));
                }
                _lstMotifs = value;
                return value;
            }
        }

        #endregion
    }
}
