using Albingia.Common;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Mvc.Common;
using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.ServiceFactory;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAffaireNouvelle;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesRecherche;
using Mapster;
using OP.WSAS400.DTO.Ecran.Rercherchesaisie;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class CreationAffaireNouvelleController : ControllersBase<ModeleCreationAffaireNouvellePage> {
        static readonly Regex regexSubmitterRisqueObjet = new Regex(
            @"^Risques\[(?<rsq>\d+)\]\.(Objets\[(?<obj>\d+)\]|\w+)",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);
        static readonly Regex regexSubmitterFormuleSelections = new Regex(
            @"^Formules\[(?<num>\d+)\]\.(?<prop>\w+)$",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

        [ErrorHandler]
        public ActionResult Index(string id) {
            this.model.PageTitle = "Création d'une Affaire Nouvelle";
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(this.model);
        }

        /// <summary>
        /// Ouvre la div flottante pour la saisie d'un 
        /// nouveau contrat
        /// </summary>
        /// <returns></returns>
        [ErrorHandler]
        public ActionResult OpenNewContrat(string codeOffre, string version, string type, string codeAvn, string modeNavig) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                var serviceContext = client.Channel;
                ModeleCreationAffaireNouvelleContrat model = new ModeleCreationAffaireNouvelleContrat();

                var result = serviceContext.InitAffaireNouvelleContrat(codeOffre, version, type, codeAvn, GetUser(), modeNavig.ParseCode<ModeConsultation>());

                if (result != null) {
                    model = ((ModeleCreationAffaireNouvelleContrat)result);
                    var branches = result.Branches.Select(m => new AlbSelectListItem() { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    if (!string.IsNullOrEmpty(result.Branche)) {
                        var sItem = branches.FirstOrDefault(x => x.Value == result.Branche);
                        if (sItem != null) {
                            sItem.Selected = true;
                        }
                    }
                    model.Branches = branches;
                    model.Souscripteur = !string.IsNullOrEmpty(result.Souscripteur) ? result.Souscripteur : string.Empty;
                }

                return PartialView("NouveauContrat", model);
            }
        }

        /// <summary>
        /// Préparation d'un nouveau contrat
        /// Récupération du numéro du nouveau contrat
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <param name="typeContrat"></param>
        /// <param name="dateAccord"></param>
        /// <param name="dateEffet"></param>
        /// <param name="heureEffet"></param>
        /// <param name="contratRemp"></param>
        /// <param name="versionRemp"></param>
        /// <param name="souscripteur"></param>
        /// <param name="gestionnaire"></param>
        /// <param name="branche"></param>
        /// <param name="cible"></param>
        /// <returns></returns>
        [ErrorHandler]
        public string NumeroAffaireNouvelle(
            string codeOffre, string version, string type, string codeContrat, string versionContrat, string typeContrat, string dateAccord,
            string dateEffet, string heureEffet, string contratRemp, string versionRemp, string souscripteur, string gestionnaire, string branche, string cible, string observation,
            string acteGestion, string tabGuid) {

            string result = null;
            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                var serviceContext = client.Channel;
                cible = cible.Split('-')[0].Trim();

                string errorStr = string.Empty;
                string MsgStr = string.Empty;
                if (!string.IsNullOrEmpty(codeContrat) && !string.IsNullOrEmpty(versionContrat)) {
                    errorStr += serviceContext.VerifContratMere(codeContrat, Convert.ToInt32(versionContrat), branche, cible);
                }
                if (!string.IsNullOrEmpty(contratRemp) && !string.IsNullOrEmpty(versionRemp)) {
                    errorStr += serviceContext.VerifContratRemp(contratRemp, Convert.ToInt32(versionRemp));
                }
                if (!string.IsNullOrEmpty(errorStr)) {
                    throw new AlbFoncException(errorStr, false, false, true);
                }

                MsgStr = serviceContext.ControleSousGest(souscripteur, gestionnaire);

                if (!string.IsNullOrEmpty(MsgStr)) {
                    throw new AlbFoncException(MsgStr, false, false, true);
                }

                result = serviceContext.CreateContrat(codeOffre, version, type, codeContrat, versionContrat, typeContrat, AlbConvert.ConvertStrToDate(dateAccord),
                    AlbConvert.ConvertStrToDate(dateEffet), AlbConvert.ConvertStrToIntHour(heureEffet), contratRemp, versionRemp, souscripteur, gestionnaire, branche, cible, Server.UrlDecode(observation),
                    GetUser(), acteGestion);
            }

            string[] array;
            if (result.IsEmptyOrNull() || (array = result.Split('_')).Length == 1) {
                throw new AlbFoncException("L'attribution d'un nouveau numéro de contrat a échouée", false, false, true);
            }
            string ipb = array[0].ToIPB();
            int alx = int.TryParse(array[1], out int i) ? i : 0;
            InitVerrouillage(ipb, alx);
            ValidateLockNewAffaire(ipb, alx, Guid.Parse(this.model.TabGuid));
            return result;
        }

        /// <summary>
        /// Validates session lock assuming the lock is already in db
        /// </summary>
        /// <param name="ipb"></param>
        /// <param name="alx"></param>
        /// <param name="guid"></param>
        private static void ValidateLockNewAffaire(string ipb, int alx, Guid guid) {
            var acces = MvcApplication.ListeAccesAffaires.First(x => x.TabGuid == guid && x.Code.ToIPB() == ipb.ToIPB() && x.Version == alx);
            MvcApplication.ListeAccesAffaires.Remove(acces);
            acces.Valider();
            MvcApplication.ListeAccesAffaires.Add(acces);
        }

        /// <summary>
        /// Redirection
        /// </summary>
        /// <param name="cible"></param>
        /// <param name="job"></param>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string infoContrat, string tabGuid, string addParamType, string addParamValue) {
            switch (cible) {
                case "RechercheSaisie":
                    return RedirectToAction(job, cible);
                case "AnInformationsGenerales":
                    return RedirectToAction(job, cible, new { id = infoContrat + "_P" + tabGuid + BuildAddParamString(addParamType, addParamValue) });
                default:
                    return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + "_" + infoContrat + tabGuid + BuildAddParamString(addParamType, addParamValue) });
            }
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult CopyAllInfo(string codeOffre, string version, string type, string infoContrat, string splitHtmlChar, string acteGestion, string tabGuid) {
            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                client.Channel.CopyAllInfo(codeOffre, version, type, infoContrat.Split('_')[0], infoContrat.Split('_')[1], GetUser(), splitHtmlChar, acteGestion);
            }
            FolderController.DeverrouillerAffaire(tabGuid, new AffaireId { CodeAffaire = codeOffre, NumeroAliment = int.Parse(version), TypeAffaire = AffaireType.Offre });
            return RedirectToAction("Index", "AnInformationsGenerales", new { id = infoContrat + "_P" + GetSurroundedTabGuid(tabGuid) + PageParamContext.ModeNavigKey + ModeConsultation.Standard.AsCode() + PageParamContext.ModeNavigKey });
        }
        
        [ErrorHandler]
        public ActionResult ChoixRisques(string id) {
            id = WebUtility.UrlDecode(id);
            string context = InitializeParams(id, false);
            var folder = this.model.AllParameters.Folder;
            using (var client = ServiceClientFactory.GetClient<IAffairePort>()) {
                client.Channel.CancelNewAffairChanges(folder.Adapt<AffaireId>());
            }
            using (var client = ServiceClientFactory.GetClient<IFormulePort>()) {
                client.Channel.CancelFormuleAffaireChanges(folder.Adapt<AffaireId>());
            }
            LoadOffre(context, folder);
            this.model.SelectionRisquesObjets = BuildSelectionRisquesModel();
            ModelState.Clear();
            if (!this.model.SelectionRisquesObjets.Risques.Any(r => r.Selected)) {
                ModelState.AddModelError($"{nameof(this.model.SelectionRisquesObjets)}.{nameof(this.model.SelectionRisquesObjets.CodeAffaireNouvelle)}", "");
            }
            return View(this.model);
        }

        [HttpPost]
        public ActionResult SetRisquesSelection(SelectionRisquesObjets selection) {
            var submitter = (Request.Headers.GetValues("submitter") ?? new string[0]).FirstOrDefault();
            bool handleSelection = true;
            int? numRisque = null;
            SelectionRisqueObjets risqueChanged = null;
            SelectionObjet objetChanged = null;
            if (submitter.ContainsChars()) {
                if (submitter.StartsWith("risque_")) {
                    // change expanding only
                    var rsq = selection.Risques.First(x => x.Code == int.Parse(submitter.Split('_')[1]));
                    numRisque = rsq.Code;
                    rsq.IsExpanded = !rsq.IsExpanded;
                    handleSelection = false;
                }
                else {
                    var match = regexSubmitterRisqueObjet.Match(submitter);
                    if (match.Success) {
                        if (int.TryParse(match.Groups["rsq"]?.Value, out int x)) {
                            risqueChanged = selection.Risques[x];
                            numRisque = risqueChanged.Code;
                        }
                        if (int.TryParse(match.Groups["obj"]?.Value, out int y)) {
                            objetChanged = risqueChanged.Objets[y];
                            risqueChanged = null;
                        }
                    }
                }
            }

            if (handleSelection) {
                HandleRisquesSelection(selection, numRisque, risqueChanged, objetChanged);
            }
            ModelState.Clear();
            if (!selection.Risques.Any(r => r.Selected)) {
                selection.AvailableOptions = new List<SelectionFormuleOption>();
                ModelState.AddModelError(nameof(selection.Risques), "Au moins un risque doit être sélectionné");
            }
            else {
                if (handleSelection) {
                    FindOptionsForSelection(selection);
                }
                if (!selection.AvailableOptions?.Any() ?? true) {
                    ModelState.AddModelError(nameof(selection.Risques), "Aucune formule n'est disponible pour ce(s) risque(s)");
                }
            }
            
            return PartialView("_RisquesAffaireNouvelle", selection);
        }

        [HttpPost]
        public ActionResult ChoixFormulesOptions(SelectionRisquesObjets selectionRisquesObjets) {
            var folder = selectionRisquesObjets.Folder;
            var affaireId = folder.Adapt<AffaireId>();
            var newFormulesFilter = selectionRisquesObjets.AvailableOptions.GroupBy(x => x.NumFormule).ToDictionary(g => g.Key, g => g.Select(x => x.NumOption).ToArray());
            using (var client = ServiceClientFactory.GetClient<IFormulePort>()) {
                client.Channel.ResetFormulesOffreSelection(affaireId, selectionRisquesObjets.CodeAffaireNouvelle, newFormulesFilter);
                var listFormules = client.Channel.GetAllFormulesOffre(affaireId, newFormulesFilter);
                BuildSelectionFormules(selectionRisquesObjets, folder, listFormules);
            }
            if (!this.model.NouvelleAffaire.Formules.Any(f => f.IsSelected == true) || this.model.NouvelleAffaire.Formules.Any(f => f.IsSelected == true && !f.SelectedOptionNumber.HasValue)) {
                ModelState.AddModelError($"{nameof(this.model.NouvelleAffaire)}.{nameof(this.model.NouvelleAffaire.Formules)}", "");
            }

            LoadOffre(folder.FullIdentifier, folder);
            return View(this.model);
        }

        [HttpPost]
        public ActionResult SetSelectionFormules(Models.FormuleGarantie.NouvelleAffaire nouvelleAffaire) {
            var submitter = (Request.Headers.GetValues("submitter") ?? new string[0]).FirstOrDefault();
            Models.FormuleGarantie.Formule currentFormula = null;
            bool needsSave = false;
            var match = regexSubmitterFormuleSelections.Match(submitter);
            if (match.Success) {
                currentFormula = nouvelleAffaire.Formules[int.Parse(match.Groups["num"].Value)];
                needsSave = true;
            }
            else if (submitter.Contains('_')) {
                currentFormula = ExpandOrCollapseSelectionFormule(nouvelleAffaire, submitter);
                ModelState.Clear();
            }

            try {
                for (int x = 0; x < nouvelleAffaire.Formules.Count; x++) {
                    var f = nouvelleAffaire.Formules[x];
                    if (f.SelectedOptionNumber.GetValueOrDefault() < 1 && f.IsSelected == true) {
                        ModelState.AddModelError($"{nameof(nouvelleAffaire.Formules)}[{x}].{nameof(f.SelectedOptionNumber)}", $"{f.Libelle} : au moins une option doit être sélectionnée");
                    }
                }
                if (needsSave && ModelState.IsValid && currentFormula != null) {
                    if (currentFormula.SelectedOptionNumber.GetValueOrDefault() > 0 && currentFormula.IsSelected == false && currentFormula.Options.Count > 1) {
                        currentFormula.IsSelected = true;
                    }
                    using (var client = ServiceClientFactory.GetClient<IAffairePort>()) {
                        client.Channel.SetSelectionFormuleNouvelleAffaire(nouvelleAffaire.Adapt<NouvelleAffaireDto>(), currentFormula.Numero);
                    }
                }
                if (!nouvelleAffaire.Formules.Any(f => f.IsSelected == true)) {
                    ModelState.AddModelError(nameof(nouvelleAffaire.Formules), "Au moins une formule doit être sélectionnée");
                }
                if (ModelState.IsValid) {
                    ModelState.Clear();
                }
            }
            catch(Exception ex) {
                ModelState.AddModelError($"{nameof(nouvelleAffaire.Formules)}", $"Erreur serveur:<br />{ex}");
            }
            return PartialView("_FormulesAffaireNouvelle", nouvelleAffaire);
        }

        [HttpPost]
        public void ValidationNewAffaire(Models.FormuleGarantie.NouvelleAffaire nouvelleAffaire) {
            using (var client = ServiceClientFactory.GetClient<IAffairePort>()) {
                var id = nouvelleAffaire.Offre.Adapt<AffaireId>();
                client.Channel.ValidateNewAffair(id, nouvelleAffaire.Code, nouvelleAffaire.Version);
                using (var clientAfNv = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                    clientAfNv.Channel.SaveNewAffair(nouvelleAffaire.Offre, new Folder(nouvelleAffaire.Code, 0, AlbConstantesMetiers.TYPE_CONTRAT[0]), GetUser());
                }
                FolderController.DeverrouillerAffaire(nouvelleAffaire.TabGuid, id);
                try {
                    using (var clientFrm = ServiceClientFactory.GetClient<IFormulePort>()) {
                        client.Channel.CancelNewAffairChanges(id);
                        clientFrm.Channel.CancelFormuleAffaireChanges(id);
                    }
                }
                catch (Exception ex) {
                    AlbLog.Warn(ex.ToString());
                }
            }
        }

        private static Models.FormuleGarantie.Formule ExpandOrCollapseSelectionFormule(Models.FormuleGarantie.NouvelleAffaire nouvelleAffaire, string submitter) {
            var elements = submitter.Split('_');
            Models.FormuleGarantie.Formule f = null;
            Models.FormuleGarantie.Option o;
            Models.FormuleGarantie.Volet v;
            Models.FormuleGarantie.Bloc b;
            switch (elements[0]) {
                case "formule":
                    f = nouvelleAffaire.Formules.First(x => x.Numero.ToString() == elements[1]);
                    f.IsExpanded = !f.IsExpanded;
                    break;
                case "option":
                    f = nouvelleAffaire.Formules.First(x => x.Numero == int.Parse(elements[1]));
                    o = f.Options.First(x => x.Numero.ToString() == elements[2]);
                    o.IsExpanded = !o.IsExpanded;
                    break;
                case "volet":
                    f = nouvelleAffaire.Formules.First(x => x.Numero == int.Parse(elements[1]));
                    o = f.Options.First(x => x.Numero.ToString() == elements[2]);
                    v = o.Volets.First(x => x.UniqueId.ToString() == elements[3]);
                    v.IsCollapsed = !v.IsCollapsed;
                    break;
                case "bloc":
                    f = nouvelleAffaire.Formules.First(x => x.Numero == int.Parse(elements[1]));
                    o = f.Options.First(x => x.Numero.ToString() == elements[2]);
                    v = o.Volets.First(x => x.UniqueId.ToString() == elements[3]);
                    b = v.Blocs.First(x => x.UniqueId.ToString() == elements[4]);
                    b.IsCollapsed = !b.IsCollapsed;
                    break;
            }
            return f;
        }

        private void FindOptionsForSelection(SelectionRisquesObjets selection) {
            var dto = selection.Adapt<global::OP.WSAS400.DTO.SelectionRisquesObjets>();
            selection.AvailableOptions = new List<SelectionFormuleOption>();
            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                var formulesOptions = client.Channel.FindAvailableOptions(dto);
                foreach (var key in formulesOptions.Keys) {
                    foreach (var opt in formulesOptions[key]) {
                        selection.AvailableOptions.Add(new SelectionFormuleOption {
                            NumFormule = key.Item1,
                            NomFormule = key.Item2,
                            NumOption = opt.Item1,
                            Application = opt.Item2,
                            Selected = false
                        });
                    }
                }
            }
        }

        private void HandleRisquesSelection(SelectionRisquesObjets selection, int? numRisque, SelectionRisqueObjets risqueChanged, SelectionObjet objetChanged) {
            foreach (var risque in selection.Risques) {
                bool hasObjSelected = false;
                foreach (var objet in risque.Objets) {
                    if (objet.Selected) {
                        hasObjSelected = true;
                        break;
                    }
                }
                if (!risque.Selected && hasObjSelected) {
                    if (risque == risqueChanged) {
                        risque.Objets.ForEach(x => x.Selected = false);
                    }
                    else {
                        risque.Selected = true;
                    }
                }
                else if (risque.Selected && !hasObjSelected) {
                    if (risque == risqueChanged) {
                        risque.Objets.ForEach(x => x.Selected = true);
                    }
                    else if (risque.Objets.Any(o => o == objetChanged)) {
                        risque.Selected = false;
                    }
                }
            }

            if (numRisque.HasValue) {
                using (var client = ServiceClientFactory.GetClient<IAffairePort>()) {
                    client.Channel.SetSelectionRisqueNouvelleAffaire(selection.Adapt<NouvelleAffaireDto>(), numRisque.Value);
                }
            }
        }

        private SelectionRisquesObjets BuildSelectionRisquesModel() {
            var selectionModel = new SelectionRisquesObjets() {
                Folder = this.model.AllParameters.Folder,
                CodeAffaireNouvelle = this.model.AllParameters[PipedParameter.IPB, 2],
                VersionAffaireNouvelle = int.Parse(this.model.AllParameters[PipedParameter.ALX, 2]),
                Risques = new List<SelectionRisqueObjets>(),
                AvailableOptions = new List<SelectionFormuleOption>()
            };

            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                var result = client.Channel.GetOffreSelections(selectionModel.Folder, new Folder(selectionModel.CodeAffaireNouvelle, selectionModel.VersionAffaireNouvelle, 'P'));

                if (result != null) {
                    selectionModel = result.Adapt<SelectionRisquesObjets>();
                    IEnumerable<UniteValeurRisque> unites;
                    IEnumerable<TypeValeurRisque> types;
                    using (var clientRef = ServiceClientFactory.GetClient<IReferentielPort>()) {
                        unites = clientRef.Channel.GetUnitesValeursRisques(new CibleFiltre(this.model.Offre.Branche.Code, this.model.Offre.Branche.Cible.Code));
                        types = clientRef.Channel.GetTypesValeursRisques(new CibleFiltre(this.model.Offre.Branche.Code, this.model.Offre.Branche.Cible.Code));
                    }
                    selectionModel.Risques.ForEach(r => {
                        r.IsExpanded = true;
                        if (r.Unite.ContainsChars()) {
                            r.Unite += $" - {unites.First(u => u.Code == r.Unite).LibelleLong}";
                        }
                        if (r.Type.ContainsChars()) {
                            r.Type += $" - {types.First(t => t.Code == r.Type).LibelleLong}";
                        }
                        r.Objets.ForEach(o => {
                            if (o.Unite.ContainsChars()) {
                                o.Unite += $" - {unites.First(u => u.Code == o.Unite).LibelleLong}";
                            }
                            if (o.Type.ContainsChars()) {
                                o.Type += $" - {types.First(t => t.Code == o.Type).LibelleLong}";
                            }
                        });
                    });
                }
                if (selectionModel.Risques.Any(x => x.Selected)) {
                    var formulesOptions = client.Channel.FindAvailableOptions(result);
                    selectionModel.AvailableOptions.Clear();
                    foreach (var key in formulesOptions.Keys) {
                        foreach (var opt in formulesOptions[key]) {
                            selectionModel.AvailableOptions.Add(new SelectionFormuleOption {
                                NumFormule = key.Item1,
                                NomFormule = key.Item2,
                                NumOption = opt.Item1,
                                Application = opt.Item2,
                                Selected = false
                            });
                        }
                    }
                }
            }

            return selectionModel;
        }

        private void BuildSelectionFormules(SelectionRisquesObjets selectionRisquesObjets, Folder folder, IEnumerable<FormuleDto> listFormules) {
            this.model.NouvelleAffaire = new Models.FormuleGarantie.NouvelleAffaire {
                Code = selectionRisquesObjets.CodeAffaireNouvelle,
                Version = selectionRisquesObjets.VersionAffaireNouvelle,
                Formules = listFormules.Select(x => x.Adapt<Models.FormuleGarantie.Formule>()).ToList(),
                Offre = folder
            };
            NouvelleAffaireDto currentNewAffair;
            using (var client = ServiceClientFactory.GetClient<IAffairePort>()) {
                currentNewAffair = client.Channel.GetNouvelleAffaire(
                    folder.Adapt<AffaireId>(),
                    selectionRisquesObjets.CodeAffaireNouvelle,
                    selectionRisquesObjets.VersionAffaireNouvelle);
            }
            this.model.NouvelleAffaire.Formules.ForEach(f => {
                var selectionFormule = currentNewAffair.Formules.FirstOrDefault(x => x.Numero == f.Numero);
                f.IsExpanded = true;
                f.IsSelected = selectionFormule?.IsSelected ?? false;
                if (f.Options.Count == 1) {
                    f.SelectedOptionNumber = f.Options.First().Numero;
                }
                else {
                    f.SelectedOptionNumber = selectionFormule?.SelectedOptionNumber;
                }
                foreach (var opt in f.Options) {
                    opt.IsExpanded = false;
                    foreach (var v in opt.Volets) {
                        v.IsCollapsed = true;
                    }
                }
            });
        }

        private void LoadOffre(string context, Folder folder) {
            using (var client = ServiceClientFactory.GetClient<IPoliceServices>()) {
                this.model.Offre = new Offre_MetaModel();
                this.model.Offre.LoadOffre(client.Channel.OffreGetDto(folder.CodeOffre, folder.Version, folder.Type, ModeConsultation.Standard));
            }

            this.model.Bandeau = null;
            if (this.model.Offre != null) {
                this.model.AfficherBandeau = base.DisplayBandeau(true, context);
                this.model.AfficherNavigation = this.model.AfficherBandeau;

                // Affichage de la navigation latérale en arboresence
                this.model.NavigationArbre = GetNavigationArbre(string.Empty);
            }

            if (this.model.AfficherBandeau) {
                this.model.Bandeau = GetInfoBandeau(folder.Type);
                // Gestion des Etapes
                this.model.Navigation = new Navigation_MetaModel {
                    Etape = Navigation_MetaModel.ECRAN_FORMVOLAFFNOUV,
                    IdOffre = this.model.Offre.CodeOffre,
                    Version = this.model.Offre.Version
                };
            }
        }

        #region Méthodes Privées

        /// <summary>
        /// Charge les infos génériques de la page
        /// </summary>
        /// <param name="id"></param>
        protected override void LoadInfoPage(string id) {
            string[] tId = id.Split('_');
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>()) {
                var policeServicesClient = client.Channel;
                model.Offre = new Offre_MetaModel();
                model.Offre.LoadOffre(policeServicesClient.OffreGetDto(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig.ParseCode<ModeConsultation>()));
                //model.Offre.LoadOffre(policeServicesClient.OffreGetDto(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig));
            }

            model.Bandeau = null;
            if (model.Offre != null) {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;

                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre(string.Empty, returnEmptyTree: true);
            }

            if (model.AfficherBandeau) {
                model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                //Gestion des Etapes
                model.Navigation = new Navigation_MetaModel {
                    Etape = Navigation_MetaModel.ECRAN_AFFAIRENOUVELLE,
                    IdOffre = model.Offre.CodeOffre,
                    Version = model.Offre.Version
                };
            }
            LoadDataCreationAffaireNouvelle(id);
            SetRechercheSaisie(model);
        }

        /// <summary>
        /// Charge les infos spécifiques à la page
        /// </summary>
        /// <param name="id"></param>
        private void LoadDataCreationAffaireNouvelle(string id) {
            string[] tId = id.Split('_');

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                var serviceContext = client.Channel;
                ModeleCreationAffaireNouvellePage model = new ModeleCreationAffaireNouvellePage();

                var result = serviceContext.InitCreateAffaireNouvelle(tId[0], tId[1], tId[2]);
                if (result != null) {
                    model = ((ModeleCreationAffaireNouvellePage)result);

                    base.model.CodeOffre = model.CodeOffre.Trim();
                    base.model.Version = model.Version;
                    base.model.DateSaisie = model.DateSaisie;
                    base.model.CodeBranche = model.CodeBranche;
                    base.model.LibBranche = model.LibBranche;
                    base.model.CodeCible = model.CodeCible;
                    base.model.LibCible = model.LibCible;
                    base.model.CodeDevise = model.CodeDevise;
                    base.model.LibDevise = model.LibDevise;
                    base.model.Gestionnaire = model.Gestionnaire;
                    base.model.Souscripteur = model.Souscripteur;
                    base.model.Identification = model.Identification;
                    base.model.CodeNatureContrat = model.CodeNatureContrat;
                    base.model.LibNatureContrat = model.LibNatureContrat;
                    base.model.CodeCourtier = model.CodeCourtier;
                    base.model.NomCourtier = model.NomCourtier;
                    base.model.CodeAssure = model.CodeAssure;
                    base.model.NomAssure = model.NomAssure;
                    base.model.Observation = model.Observation;
                    base.model.Contrats = model.Contrats;
                    base.model.PossedeUnContratEnCours = model.Contrats.Exists(c => c.Etat != "V" && c.Etat != "");
                    base.model.DateDuJour = DateTime.Now.ToShortDateString();
                    if (base.model.Offre.DateEffetGarantie.HasValue) {
                        base.model.DateEffet = base.model.Offre.DateEffetGarantie.Value.ToString("dd/MM/yyyy");
                        base.model.HeureEffet = string.Format("{0}:{1}:00", base.model.Offre.DateEffetGarantie.Value.Hour.ToString().PadLeft(2, '0'), base.model.Offre.DateEffetGarantie.Value.Minute.ToString().PadLeft(2, '0'));
                    }
                    InitLockContratEnCours();
                }
            }
        }

        private void InitLockContratEnCours() {
            if (!this.model.PossedeUnContratEnCours) {
                return;
            }
            var contrat = this.model.Contrats.First(c => c.Etat != "V" && c.Etat != "");
            int.TryParse(contrat.Aliment ?? "", out int alx);
            Guid guid = Guid.Parse(this.model.TabGuid);
            if (MvcApplication.ListeAccesAffaires.Any(x =>
                x.ModeAcces == AccesOrigine.Modifier
                && x.TabGuid == guid
                && x.VerrouillageEffectue
                && x.Code == contrat.CodeContrat.ToIPB()
                && x.Version == alx)) {
                return;
            }
            using (var client = ServiceClientFactory.GetClient<IAffairePort>()) {
                client.Channel.TryLockAffaire(new AffaireId { CodeAffaire = contrat.CodeContrat, NumeroAliment = alx, TypeAffaire = AffaireType.Contrat }, AccesOrigine.EtablirAffaireNouvelle.ToString());
            }
            InitVerrouillage(contrat.CodeContrat, alx);
            ValidateLockNewAffaire(contrat.CodeContrat, alx, guid);
        }

        private void SetRechercheSaisie(ModeleCreationAffaireNouvellePage contentData) {
            contentData.Recherche = new ModeleRecherchePage {
                CritereParam = AlbConstantesMetiers.CriterParam.ContratOnly,
                ProvenanceParam = "connexite",
                SituationParam = string.Empty,
                ListCabinetCourtage = new ModeleRechercheAvancee(),
                ListPreneurAssurance = new ModeleRechercheAvancee()
            };
            using (var client = ServiceClientFactory.GetClient<IRechercheSaisie>()) {
                var screenClient = client.Channel;
                var query = new RechercheSaisieGetQueryDto();
                var result = screenClient.RechercheSaisieGet(query);
                if (result != null) {
                    var cibles = result.Cibles.Select(m => new AlbSelectListItem() { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    contentData.Recherche.ModeleCibles = new ModeleListeCibles();
                    contentData.Recherche.ModeleCibles.Cibles = cibles;
                    var branches = result.Branches.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    contentData.Recherche.Branches = branches;
                    var etats = result.Etats.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    contentData.Recherche.Etats = etats;
                    var situations = result.Situation.Select(m => new AlbSelectListItem {
                        Value = m.Code,
                        Text = !string.IsNullOrEmpty(m.Code) ? string.Format("{0} - {1}", m.Code, m.Libelle) : string.Empty,
                        Selected = false,
                        Title = !string.IsNullOrEmpty(m.Code) ? string.Format("{0} - {1}", m.Code, m.Libelle) : string.Empty
                    }).ToList();
                    contentData.Recherche.Situations = situations;
                    var listRefus = result.ListRefus.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    contentData.Recherche.ListRefus = listRefus;
                    contentData.Recherche.DateTypes = AlbTransverse.InitDateType;
                }
            }
        }
        #endregion
    }
}
