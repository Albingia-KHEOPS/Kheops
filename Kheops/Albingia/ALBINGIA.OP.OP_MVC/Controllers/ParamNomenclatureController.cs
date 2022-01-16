using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamNomenclature;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamTemplateNomenclature;
using OPServiceContract.IAdministration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ParamNomenclatureController : ControllersBase<ModeleParamNomenclaturePage>
    {
        #region Membres static

        public static List<AlbSelectListItem> _lstConcepts;
        public static List<AlbSelectListItem> _lstFamilles;

        #endregion

        #region Méthodes publiques
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Paramétrages des nomenclatures";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            DisplayBandeau();
            return View(model);
        }

        [ErrorHandler]
        public ActionResult Recherche(string description)
        {
            var toReturn = new List<ModeleLigneTemplateNomenclature>();
            //Ajout de données fictives
            toReturn.Add(new ModeleLigneTemplateNomenclature
            {
                Branche = "RS",
                Cible = "SPE",
                GuidId = "1",
                Libelle = "Ligne fictive 1"
            });
            return PartialView("ListeNomenclatures", toReturn);
        }

        [ErrorHandler]
        public ActionResult AfficherDetailsNomenclature(string idNomenclature)
        {
            ModeleDetailsNomenclature toReturn = new ModeleDetailsNomenclature();
            //Ajout de données fictives

            toReturn.GuidId = "1";
            toReturn.Branche = "RS - Risques spéciaux";
            toReturn.Cible = "SPE - Spectacles";
            toReturn.Libelle = "Ligne fictive 1";
            toReturn.Concept01 = "PRODU";
            toReturn.Famille01 = "QCVAT";
            toReturn.Concept02 = string.Empty;
            toReturn.Famille02 = string.Empty;
            toReturn.Concept03 = string.Empty;
            toReturn.Famille03 = string.Empty;
            toReturn.Concept04 = string.Empty;
            toReturn.Famille04 = string.Empty;


            //Chargement des listes seulement si il n'y a pas de ligne pour la colonne correspondante
            toReturn.ListeConcepts01 = LstConcepts();
            toReturn.ListeConcepts02 = LstConcepts();
            toReturn.ListeConcepts03 = LstConcepts();
            toReturn.ListeConcepts04 = LstConcepts();

            toReturn.ListeFamilles01 = LstFamilles(toReturn.Concept01);
            toReturn.ListeFamilles02 = LstFamilles(toReturn.Concept02);
            toReturn.ListeFamilles03 = LstFamilles(toReturn.Concept03);
            toReturn.ListeFamilles04 = LstFamilles(toReturn.Concept04);

            toReturn.ListeLignesDetails = new List<ModeleLigneDetails>();
            //Ajout de données fictives
            toReturn.ListeLignesDetails.Add(new ModeleLigneDetails
            {
                GuidId = 1,
                ListeValeurs01 = LstValeurs(toReturn.Concept01, toReturn.Famille01),
                Valeur01 = "CALOC",
                ListeValeurs02 = LstValeurs(toReturn.Concept02, toReturn.Famille02),
                Valeur02 = string.Empty,
                ListeValeurs03 = LstValeurs(toReturn.Concept03, toReturn.Famille03),
                Valeur03 = string.Empty,
                ListeValeurs04 = LstValeurs(toReturn.Concept04, toReturn.Famille04),
                Valeur04 = string.Empty,
            });

            return PartialView("DetailsParamNomenclature", toReturn);
        }

        [ErrorHandler]
        public ActionResult GetLigneDetailsNomenclature(string modeAffichage, int idNomenclature, int idLigneDetail,
                                                        string concept01, string concept02, string concept03, string concept04,
                                                        string famille01, string famille02, string famille03, string famille04)
        {
            ModeleLigneDetails toReturn = null;
            if (idLigneDetail > 0 && idNomenclature > 0)
            {
                //Chargement des info de base
                var dataInfoBase = LoadInfoBaseDetailsNomenclature(idNomenclature.ToString(),
                                                                   concept01, concept02, concept03, concept04,
                                                                   famille01, famille02, famille03, famille04);

                //Données fictives
                toReturn = new ModeleLigneDetails
                {
                    GuidId = 1,
                    ListeValeurs01 = LstValeurs(dataInfoBase.Concept01, dataInfoBase.Famille01),
                    Valeur01 = "CALOC",
                    ListeValeurs02 = LstValeurs(dataInfoBase.Concept02, dataInfoBase.Famille02),
                    Valeur02 = string.Empty,
                    ListeValeurs03 = LstValeurs(dataInfoBase.Concept03, dataInfoBase.Famille03),
                    Valeur03 = string.Empty,
                    ListeValeurs04 = LstValeurs(dataInfoBase.Concept04, dataInfoBase.Famille04),
                    Valeur04 = string.Empty,
                };
            }
            else if (idNomenclature > 0 && idLigneDetail == ALBINGIA.OP.OP_MVC.MvcApplication.ALBINDEXLIST)
            {
                //Chargement des info de base
                var dataInfoBase = LoadInfoBaseDetailsNomenclature(idNomenclature.ToString(),
                                                                   concept01, concept02, concept03, concept04,
                                                                   famille01, famille02, famille03, famille04);

                toReturn = new ModeleLigneDetails
                {
                    GuidId = ALBINGIA.OP.OP_MVC.MvcApplication.ALBINDEXLIST,
                    ListeValeurs01 = LstValeurs(dataInfoBase.Concept01, dataInfoBase.Famille01),
                    ListeValeurs02 = LstValeurs(dataInfoBase.Concept02, dataInfoBase.Famille02),
                    ListeValeurs03 = LstValeurs(dataInfoBase.Concept03, dataInfoBase.Famille03),
                    ListeValeurs04 = LstValeurs(dataInfoBase.Concept04, dataInfoBase.Famille04)
                };
            }
            if (modeAffichage == "edition")
            {
                return PartialView("LigneDetailsEdition", toReturn);
            }
            else if (modeAffichage == "readonly")
            {
                return PartialView("LigneDetailsReadonly", toReturn);
            }
            else
                return null;//TODO renvoie exception
        }

        [ErrorHandler]
        public ActionResult GetListeFamilleByConcept(string concept, string id)
        {
            ModeleListeFamille toReturn = new ModeleListeFamille();
            toReturn.ListeFamilles = LstFamilles(concept);
            toReturn.Id = id;
            return PartialView("DrlFamilles", toReturn);
        }

        [ErrorHandler]
        public ActionResult GetListeValeurByConceptFamille(string concept, string famille, string idColonne, string guidIdLigne)
        {
            ModeleListeValeur toReturn = new ModeleListeValeur();
            toReturn.ListeValeurs = LstValeurs(concept, famille);
            toReturn.GuidIdLigne = guidIdLigne;
            toReturn.IdColonne = idColonne;
            return PartialView("DrlValeurs", toReturn);
        }

        [ErrorHandler]
        public ActionResult EnregistrerLigneDetail(string idNomenclature, string idLigne, string valeur1, string valeur2, string valeur3, string valeur4)
        {
            //TODO sauvegarde des valeurs en BDD

            return AfficherDetailsNomenclature(idNomenclature);
        }

        [ErrorHandler]
        public ActionResult SupprimerLigneDetail(string idNomenclature, string idLigne)
        {
            //TODO suppression de la ligne en BDD

            return AfficherDetailsNomenclature(idNomenclature);
        }

        #endregion

        #region Méthodes privées

        private ModeleDetailsNomenclature LoadInfoBaseDetailsNomenclature(string idNomenclature,
                                                                          string concept01, string concept02, string concept03, string concept04,
                                                                          string famille01, string famille02, string famille03, string famille04)
        {
            ModeleDetailsNomenclature toReturn = new ModeleDetailsNomenclature();
            //Ajout de données fictives

            toReturn.GuidId = "1";
            toReturn.Branche = "RS - Risques spéciaux";
            toReturn.Cible = "SPE - Spectacles";
            toReturn.Libelle = "Ligne fictive 1";
            toReturn.Concept01 = concept01;
            toReturn.Famille01 = famille01;
            toReturn.Concept02 = concept02;
            toReturn.Famille02 = famille02;
            toReturn.Concept03 = concept03;
            toReturn.Famille03 = famille03;
            toReturn.Concept04 = concept04;
            toReturn.Famille04 = famille04;

            return toReturn;
        }


        private List<AlbSelectListItem> LstConcepts()
        {
            //Si les données ont déjà été chargé en mémoire, on renvoit une nouvelle instance de la liste
            if (_lstConcepts != null)
            {
                var toReturn = new List<AlbSelectListItem>();
                _lstConcepts.ForEach(elm => toReturn.Add(new AlbSelectListItem
                {
                    Value = elm.Value,
                    Text = elm.Text,
                    Title = elm.Title,
                    Selected = false
                }));
                return toReturn;
            }

            //Sinon on charge les données à partir de la BDD et on renvoit une nouvelle instance de la liste
            var value = new List<AlbSelectListItem>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.LoadListConcepts(string.Empty, string.Empty, false, true);
                if (result.Any())
                {
                    result.ForEach(elm => value.Add(new AlbSelectListItem
                    {
                        Value = elm.Code,
                        Text = elm.Code + " - " + elm.Libelle,
                        Title = elm.Code + " - " + elm.Libelle,
                        Selected = false
                    }));
                }

            }
            _lstConcepts = value;
            return value;
        }

        private List<AlbSelectListItem> LstFamilles(string codeConcept)
        {
            //Sinon on charge les données à partir de la BDD et on renvoit une nouvelle instance de la liste
            var value = new List<AlbSelectListItem>();
            if (!string.IsNullOrEmpty(codeConcept))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    var result = serviceContext.GetFamilles(codeConcept, string.Empty, string.Empty, string.Empty, string.Empty);
                    if (result.Any())
                    {
                        result.ForEach(elm => value.Add(new AlbSelectListItem
                        {
                            Value = elm.CodeFamille,
                            Text = elm.CodeFamille + " - " + elm.LibelleFamille,
                            Title = elm.CodeFamille + " - " + elm.LibelleFamille,
                            Selected = false
                        }));
                    }
                }
            }
            return value;
        }

        private List<AlbSelectListItem> LstValeurs(string codeConcept, string codeFamille)
        {
            //Sinon on charge les données à partir de la BDD et on renvoit une nouvelle instance de la liste
            var value = new List<AlbSelectListItem>();
            if (!string.IsNullOrEmpty(codeConcept) && !string.IsNullOrEmpty(codeFamille))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    var result = serviceContext.GetValeursByFamille(codeConcept, codeFamille, string.Empty, string.Empty);
                    if (result.Any())
                    {
                        result.ForEach(elm => value.Add(new AlbSelectListItem
                        {
                            Value = elm.CodeValeur,
                            Text = elm.CodeValeur + " - " + elm.LibelleValeur,
                            Title = elm.CodeValeur + " - " + elm.LibelleValeur,
                            Selected = false
                        }));
                    }
                }
            }
            return value;
        }

        private static void SetSelectedItem(List<AlbSelectListItem> lookUp, string compareValue, AlbSelectListItem selectedItem = null)
        {
            selectedItem = lookUp.FirstOrDefault(elm => elm.Value == compareValue);
            if (selectedItem != null)
                selectedItem.Selected = true;
        }

        #endregion
    }
}
