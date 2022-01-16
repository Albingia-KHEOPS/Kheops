using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.ServiceFactory;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ISaisieCreationOffre;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers {
    public class ChoixRisquesAffaireNouvelleController : ControllersBase<ModeleCreationAffaireNouvellePage> {

        public ActionResult Index(string id) {
            string context = InitializeParams(id, false);
            var folder = this.model.AllParameters.Folder;
            using (var client = ServiceClientFactory.GetClient<IPoliceServices>()) {
                this.model.Offre = new Offre_MetaModel();
                this.model.Offre.LoadOffre(client.Channel.OffreGetDto(folder.CodeOffre, folder.Version, folder.Type, ModeConsultation.Standard));
            }

            this.model.Bandeau = null;
            if (this.model.Offre != null) {
                this.model.AfficherBandeau = base.DisplayBandeau(true, id);
                this.model.AfficherNavigation = this.model.AfficherBandeau;

                //Affichage de la navigation latérale en arboresence
                this.model.NavigationArbre = GetNavigationArbre(string.Empty);
            }

            if (this.model.AfficherBandeau) {
                this.model.Bandeau = GetInfoBandeau(id);
                //Gestion des Etapes
                this.model.Navigation = new Navigation_MetaModel {
                    Etape = Navigation_MetaModel.ECRAN_FORMVOLAFFNOUV,
                    IdOffre = this.model.Offre.CodeOffre,
                    Version = this.model.Offre.Version
                };
            }
            this.model.SelectionRisquesObjets = BuildSelectionRisquesModel();
            return View(this.model);
        }

        [HttpGet]
        public ActionResult LoadRisquesSelection(SelectionRisquesObjets selection) {
            ModelState.Clear();
            if (!selection.Risques.Any(r => r.Selected)) {
                ModelState.AddModelError(nameof(selection.Risques), "Au moins un risque doit être sélectionné");
            }
            return PartialView("_RisquesAffaireNouvelle", selection);
        }

        [HttpPost]
        public ActionResult SetRisquesSelection(SelectionRisquesObjets selection) {
            ModelState.Clear();
            // hide first load flag
            selection.IsValid = true;
            var submitter = (Request.Headers.GetValues("submitter") ?? new string[0]).FirstOrDefault();
            var array = submitter.Split('[');
            SelectionRisqueObjets risqueChanged = null;
            //SelectionObjet objetChanged = null;
            if (array.Length > 1) {
                risqueChanged = selection.Risques[int.Parse(array[1].Split(']')[0])];
            }
            if (array.Length == 3) {
                //objetChanged = risqueChanged.Objets[int.Parse(array[2].Split(']')[0])];
                risqueChanged = null;
            }
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
                    risque.Objets.ForEach(x => x.Selected = true);
                }
            }
            if (!selection.Risques.Any(r => r.Selected)) {
                ModelState.AddModelError(nameof(selection.Risques), "Au moins un risque doit être sélectionné");
            }
            return PartialView("_RisquesAffaireNouvelle", selection);
        }

        private SelectionRisquesObjets BuildSelectionRisquesModel() {
            var selectionModel = new SelectionRisquesObjets() {
                Folder = this.model.AllParameters.Folder,
                CodeAffaireNouvelle = this.model.AllParameters[PipedParameter.IPB, 2],
                Risques = new List<SelectionRisqueObjets>()
            };

            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                var result = client.Channel.InitRsqObjAffNouv(
                    selectionModel.Folder.CodeOffre,
                    selectionModel.Folder.Version.ToString(),
                    selectionModel.Folder.Type,
                    selectionModel.CodeAffaireNouvelle,
                    "0");

                if (result != null) {
                    foreach (var group in result.ListRsqObj.GroupBy(x => x.CodeRsq)) {
                        var risque = group.First(x => x.TypeEnr == "R");
                        var risqueSelection = new SelectionRisqueObjets {
                            Name = risque.Libelle,
                            Selected = risque.CheckRow,
                            Objets = group.Where(x => x.TypeEnr == "O").Select(o => new SelectionObjet {
                                Name = o.Libelle,
                                Selected = o.CheckRow
                            }).ToList()
                        };
                        selectionModel.Risques.Add(risqueSelection);
                    }

                    if (!selectionModel.Risques.Any(r => r.Selected)) {
                        selectionModel.IsValid = false;
                    }
                }
            }

            return selectionModel;
        }
    }
}