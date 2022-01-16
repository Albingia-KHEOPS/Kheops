using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.DynamicGuiIS.Common;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamISAssocier;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamISReference;
using OP.WSAS400.DTO.ExcelDto;
using OP.WSAS400.DTO.ParamIS;
using OPServiceContract.IAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ParamISModeleAssocierController : ControllersBase<ModeleParamISModelesPage>
    {
        #region Membres Privés

        private const string msgModeleNomVide = "Le modèle et la description du modèle ne peuvent être vides";
        private const string msgModeleDateVide = "Les dates ne peuvent être vides";
        private const string msgModeleRequeteVide = "Les requêtes ne peuvent être vides";
        private const string msgTypeOperationVide = "Le type d'opération à effectuer en BDD est vide";
        private const string msgPbEnregistrement = "Un problème est survenu lors de l'enregistrement";
        private const string msgChampsInvalides = "Certains champs obligatoires sont vides ou incorrects";

        private static List<AlbSelectListItem> LstRefOuiNon
        {
            get
            {
                //Nouvelle instance à chaque récupération de la référence
                List<AlbSelectListItem> value = new List<AlbSelectListItem>();
                value.Add(new AlbSelectListItem() { Value = "O", Text = "Oui" });
                value.Add(new AlbSelectListItem() { Value = "N", Text = "Non" });
                return value;
            }
        }
        private static List<AlbSelectListItem> _lstReferentiels;
        public static List<AlbSelectListItem> LstReferentiels
        {
            get
            {
                //Si les données ont déjà été chargé en mémoire, on renvoit une nouvelle instance de la liste
                if (_lstReferentiels != null)
                {
                    var toReturn = new List<AlbSelectListItem>();
                    _lstReferentiels.ForEach(elm => toReturn.Add(new AlbSelectListItem
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
                value.Add(new AlbSelectListItem { Value = "", Text = "Aucun référentiel", Title = "Aucun référentiel", Selected = false });
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    var lstRef = ListeLignesModele;
                    lstRef.ForEach(elm => value.Add(new AlbSelectListItem
                    {
                        Value = elm.Code,
                        Text = elm.Description,
                        Title = BuildReferentielTitle(elm),
                        Selected = false
                    }
                    ));
                }
                _lstReferentiels = value;
                return value;
            }
        }

        private static List<LigneModeleISDto> _listeLignesModele;
        private static List<LigneModeleISDto> ListeLignesModele
        {
            get
            {
                //Si les données ont déjà été chargé en mémoire, on renvoit la liste en mémoire
                if (_listeLignesModele != null)
                {
                    return _listeLignesModele;
                }

                //Sinon on charge les données à partir de la BDD et on renvoit la liste
                List<LigneModeleISDto> value = new List<LigneModeleISDto>();
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    value = serviceContext.GetISReferenciel(string.Empty, false);
                }
                _listeLignesModele = value;
                return value;
            }
        }


        private static List<AlbSelectListItem> LstRefHierarchyOrder
        {
            get
            {
                //Nouvelle instance à chaque récupération de la référence
                List<AlbSelectListItem> value = new List<AlbSelectListItem>();
                value.Add(new AlbSelectListItem() { Value = "1", Text = "Titre" });
                value.Add(new AlbSelectListItem() { Value = "2", Text = "SousTitre" });
                value.Add(new AlbSelectListItem() { Value = "3", Text = "Champ" });
                return value;
            }
        }
        private static List<AlbSelectListItem> _lstLinkBehaviour;
        private static List<AlbSelectListItem> LstLinkBehaviour(string modele)
        {
            //Si les données ont déjà été chargé en mémoire, on renvoit une nouvelle instance de la liste
            if (_lstLinkBehaviour != null)
            {
                var toReturn = new List<AlbSelectListItem>();
                _lstLinkBehaviour.ForEach(elm => toReturn.Add(new AlbSelectListItem
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
            value.Add(new AlbSelectListItem { Value = "0", Text = "Aucun parent", Title = "Aucun parent", Selected = false });
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var lstLnk = serviceContext.GetISModeleLignes(modele, string.Empty, string.Empty);
                lstLnk.ForEach(elm => value.Add(new AlbSelectListItem
                {
                    Value = elm.GuidId.ToString(),
                    Text = elm.GuidId + " - " + elm.Libelle,
                    Title = elm.GuidId + " - " + elm.Libelle,
                    Selected = false
                }
                ));
            }
            _lstLinkBehaviour = value;
            return value;
        }
        #endregion

        #region Méthodes publiques
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Paramétrage IS Associer Modèles";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            LoadInfoPage();
            DisplayBandeau();
            return View(model);
        }

        [ErrorHandler]
        public PartialViewResult SaveModele(string nomModele, string descriptionModele, string datedebut, string datefin,
                                             string typeOperation)
        {
            #region Vérification des valeurs avant insertion

            if (string.IsNullOrEmpty(nomModele) || string.IsNullOrEmpty(descriptionModele))
            {
                throw new AlbFoncException(msgModeleNomVide, false, false, true);
            }

            #region Vérification et assignation des dates début et fin
            DateTime dDateDeb = DateTime.Now;
            DateTime dDateFin = DateTime.Now;
            int iDateDeb = 0;
            int iDateFin = 0;

            if (!DateTime.TryParse(datedebut, out dDateDeb))
                iDateDeb = 0;
            else
                iDateDeb = int.Parse(dDateDeb.ToString("yyyyMMdd"));
            if (!DateTime.TryParse(datefin, out dDateFin))
                iDateFin = 0;
            else
                iDateFin = int.Parse(dDateFin.ToString("yyyyMMdd"));

            if (iDateDeb == 0)
                throw new AlbFoncException(msgModeleDateVide, false, false, true);

            #endregion

           

            if (string.IsNullOrEmpty(typeOperation))
            {
                throw new AlbTechException(new Exception(msgTypeOperationVide));
            }

            #endregion

            ModeleISDto modele = new ModeleISDto
            {
                NomModele = nomModele,
                Description = descriptionModele,
                DateDebut = iDateDeb,
                DateFin = iDateFin,
                TypeOperation = typeOperation
            };

            string res = string.Empty;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                res = serviceContext.SaveISModeleDetails(modele, GetUser());
            }

            #region Vérification de l'enregistrement en BDD

            int affectedRows = 0;
            int.TryParse(res, out affectedRows);
            if (affectedRows != 1)
            {
                throw new AlbTechException(new Exception(msgPbEnregistrement));
            }

            #endregion

            LoadListeModeles();
            return PartialView("ListeDrlModelesIS", model.ListeModelesISModele);
        }

        [ErrorHandler]
        public PartialViewResult SaveLineModele(int code, string modeleIS, string referentiel, string libelle, float numOrdreAff, int sautLigne,
                                                string modifiable, string obligatoire, string tcon, string tfam, int presentation,
                                                string affichage, string controle, int lienParent, string parentComportement,
                                                string parentEvenement, string typeOperation)
        {
            #region Vérification des valeurs avant insertion
        
            if (string.IsNullOrEmpty(modeleIS))
                throw new AlbFoncException(msgModeleNomVide, false, false, true);
            if (typeOperation != "Delete")
            {
                if (numOrdreAff == 0 || sautLigne < 1)
                    throw new AlbFoncException(msgChampsInvalides, false, false, true);
            }
            #endregion

            ParamISLigneModeleDto ligne = null;
            if (typeOperation != "Delete")
            {
                ligne = new ParamISLigneModeleDto
                {
                    GuidId = code,
                    ModeleIS = modeleIS,
                    Referentiel = referentiel,
                    Libelle = libelle,
                    NumOrdreAffichage = numOrdreAff,
                    SautDeLigne = sautLigne,
                    Modifiable = modifiable,
                    Obligatoire = obligatoire,
                    Presentation = presentation,
                    Tcon = tcon,
                    Tfam = tfam,
                    Affichage = affichage,
                    Controle = controle,
                    LienParent = lienParent,
                    ParentComportement = parentComportement,
                    ParentEvenement = parentEvenement,
                    TypeOperation = typeOperation
                };
            }
            else {
                ligne = new ParamISLigneModeleDto
                {
                    GuidId = code,
                    ModeleIS = modeleIS,                 
                    TypeOperation = typeOperation
                };
            }
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                serviceContext.SaveISModeleLigne(ligne, GetUser());
            }

            List<ParamISLigneModele> toReturn = InitListeLignes(ligne.ModeleIS);
            return PartialView("ListeISLignesModele", toReturn);
        }

        [ErrorHandler]
        public PartialViewResult OuvrirAjoutModele()
        {
            ParamISModele toReturn = new ParamISModele();
            return PartialView("AjoutModele", toReturn);
        }

        [ErrorHandler]
        public PartialViewResult PreRemplirLigneAvecReferentiel(int codeLigne, string codeReferentiel)
        {
            ParamISLigneModele toReturn = null;
            if (!string.IsNullOrEmpty(codeReferentiel))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    var lstRef = serviceContext.GetISReferenciel(codeReferentiel, false);
                    if (lstRef != null && lstRef.Count == 1)
                    {
                        var referentiel = lstRef.FirstOrDefault();
                        toReturn = new ParamISLigneModele()
                        {
                            GuidId = codeLigne,
                            Libelle = referentiel.LibelleAffiche,
                            Obligatoire = referentiel.Obligatoire,
                            Affichage = referentiel.Affichage,
                            Controle = referentiel.Controle,
                            Mappage = referentiel.Mappage,
                            Referentiel = codeReferentiel,
                            ReferentielId = codeReferentiel,
                            SautDeLigne = 1,
                            Presentation = referentiel.Presentation.ToString(),
                            NumOrdreAffichage = 1,
                            Tcon = referentiel.Tcon,
                            Tfam = referentiel.Tfam
                        };
                    }
                }
            }
            if (toReturn == null)
                toReturn = new ParamISLigneModele();

            toReturn = InitLigne(toReturn);

            return PartialView("LigneModeleISEdition", toReturn);
        }

        [ErrorHandler]
        public PartialViewResult GetDetailsReferentiel(string codeReferentiel)
        {
            LigneModeleIS toReturn = new LigneModeleIS();

            #region récupération et initialisation du référentiel
            if (!string.IsNullOrEmpty(codeReferentiel))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext = client.Channel;
                    var lstRef = serviceContext.GetISReferenciel(codeReferentiel, false);
                    if (lstRef != null && lstRef.Count > 0)
                    {
                        toReturn = (LigneModeleIS)lstRef.FirstOrDefault();

                        SetReadOnlyValueReferentielFromListe(toReturn);
                    }
                }
            }
            #endregion

            return PartialView("BandeauReferentiel", toReturn);
        }

        [ErrorHandler]
        public PartialViewResult GetDetailsModele(string modeleName, bool modeAssociation)
        {
            ParamISModele toReturn = null;
            if (!string.IsNullOrEmpty(modeleName))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    var lstModeles = serviceContext.GetISModeles(modeleName);
                    if (lstModeles != null && lstModeles.Count == 1)
                        toReturn = (ParamISModele)lstModeles.FirstOrDefault();
                }
                toReturn.ListeLignesModele = InitListeLignes(modeleName);
            }
            if (toReturn == null)
                toReturn = new ParamISModele();
            //toReturn.LigneVide = InitLigneVide(modeleName);
            toReturn.ModeAssociation = modeAssociation;
            return PartialView("ModeleComplet", toReturn);
        }

        [ErrorHandler]
        public PartialViewResult GetSelecteurReferentiel(string selectedReferentiel)
        {
            List<AlbSelectListItem> toReturn = LstReferentiels;
            SetSelectedItemModeleIS(toReturn, selectedReferentiel);
            return PartialView("SelectPopup", toReturn);
        }

        [ErrorHandler]
        public PartialViewResult GetSelecteurParent(string modeleId, string selectedParent)
        {
            List<AlbSelectListItem> toReturn = LstLinkBehaviour(modeleId);
            SetSelectedItemModeleIS(toReturn, selectedParent);
            return PartialView("SelectPopup", toReturn);
        }

        [ErrorHandler]
        public string VisualiserLigneInfo(string modeleId)
        {
            RefreshCacheIS();
            string branche = "RS";
            string section = modeleId;//Entete, Objets etc.

            string html = string.Empty;

            var parameters = DbIOParam.GetParams(HttpUtility.UrlDecode("O#**# 38212#**#0"), HttpUtility.UrlDecode("#**#"));


            ParamISInfo DbParamIS = DbIOParam.GetControlsFromDB(modeleId);
            html = new GenerationDbHTML(HttpUtility.UrlDecode(branche), new List<ParamISLigneInfo>(DbParamIS.ParamISDBLignesInfo), null, MvcApplication.SPLIT_CONST_HTML, "||", "38212", "0", "O", null, string.Empty, HttpUtility.UrlDecode(string.Empty), ModeConsultation.Standard.AsCode()).Generate(HttpUtility.UrlDecode(branche), HttpUtility.UrlDecode(section), onlyPreview: false);

            return html;
        }

        [ErrorHandler]
        public PartialViewResult AfficherLigne(string mode, int code)
        {
            ParamISLigneModele toReturn = null;

            if (code != ALBINGIA.OP.OP_MVC.MvcApplication.ALBINDEXLIST)
            {
                string partialViewName = (mode == "readonly" ? "LigneModeleISReadOnly" : "LigneModeleISEdition");
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    var lstRef = serviceContext.GetISModeleLignes(string.Empty, string.Empty, code.ToString());
                    if (lstRef.Any())
                    {
                        toReturn = (ParamISLigneModele)lstRef.FirstOrDefault();
                        if (mode == "edition")
                        {
                            toReturn.ListAffichage = LstRefOuiNon;
                            SetSelectedItemModeleIS(toReturn.ListAffichage, toReturn.Affichage);

                            toReturn.ListControle = LstRefOuiNon;
                            SetSelectedItemModeleIS(toReturn.ListControle, toReturn.Controle);

                            toReturn.ListObligatoire = LstRefOuiNon;
                            SetSelectedItemModeleIS(toReturn.ListObligatoire, toReturn.Obligatoire);

                            toReturn.ListModifiable = LstRefOuiNon;
                            SetSelectedItemModeleIS(toReturn.ListModifiable, toReturn.Modifiable);

                            toReturn.ReferentielId = toReturn.Referentiel;
                            toReturn.Referentiel = LstReferentiels.FirstOrDefault(elm => elm.Value == toReturn.Referentiel) != null ? LstReferentiels.FirstOrDefault(elm => elm.Value == toReturn.Referentiel).Text : toReturn.Referentiel;

                            toReturn.ListPresentation = LstRefHierarchyOrder;
                            SetSelectedItemModeleIS(toReturn.ListPresentation, toReturn.Presentation);

                            toReturn.LienParentLibelle = LstLinkBehaviour(toReturn.ModeleIS).FirstOrDefault(elm => elm.Value == toReturn.LienParent.ToString()) != null ? LstLinkBehaviour(toReturn.ModeleIS).FirstOrDefault(elm => elm.Value == toReturn.LienParent.ToString()).Text : toReturn.LienParentLibelle;

                        }
                        else
                            SetReadOnlyValueModeleFromListe(toReturn);
                        return PartialView(partialViewName, toReturn);
                    }
                }
            }
            else//Mode nouvelle ligne
            {
                toReturn = InitLigneVide();
                return PartialView("LigneModeleISEdition", toReturn);
            }

            throw new AlbFoncException("Impossible d'afficher cette ligne", trace: false, sendMail: false, onlyMessage: true);

        }


        [ErrorHandler]
        public void InitialiserCacheIS()
        {
            RefreshCacheIS();
        }

        #endregion

        #region Méthodes Privées

        /// <summary>
        /// Initialise les éléments de base de la page
        /// </summary>      
        protected override void LoadInfoPage(string context = null)
        {
            _listeLignesModele = null;
            _lstReferentiels = null;
            LoadListeModeles();
            model.SelectedModele = new ParamISModele();
            //model.SelectedModele.LigneVide = InitLigneVide(string.Empty);
            model.SelectedModele.ModeAssociation = true;
        }

        private void LoadListeModeles()
        {
            model.ListeModelesISModele = new ParamISDrlModele();
            model.ListeModelesISModele.ListeModelesIS = new List<AlbSelectListItem>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var lstModeles = serviceContext.GetISModeles(string.Empty);

                lstModeles.ForEach(elm => model.ListeModelesISModele.ListeModelesIS.Add(new AlbSelectListItem
                {
                    Text = elm.NomModele + " - " + elm.Description,
                    Value = elm.NomModele,
                    Title = elm.NomModele + " - " + elm.Description,
                    Selected = false
                }));
            }
        }

        private ParamISLigneModele InitLigneVide()
        {
            ParamISLigneModele toReturn = new ParamISLigneModele();
            toReturn.ListAffichage = LstRefOuiNon;
            toReturn.ListControle = LstRefOuiNon;
            toReturn.ListObligatoire = LstRefOuiNon;
            toReturn.ListModifiable = LstRefOuiNon;
            //toReturn.ListReferentiel = LstReferentiels;
            toReturn.ListPresentation = LstRefHierarchyOrder;
            //toReturn.ListLinkBehaviour = LstLinkBehaviour(modele);

            //Valeurs par défaut
            toReturn.ListAffichage.FirstOrDefault(elm => elm.Value == "N").Selected = true;
            toReturn.ListControle.FirstOrDefault(elm => elm.Value == "N").Selected = true;
            toReturn.ListObligatoire.FirstOrDefault(elm => elm.Value == "N").Selected = true;
            toReturn.ListModifiable.FirstOrDefault(elm => elm.Value == "N").Selected = true;
            toReturn.ListPresentation.FirstOrDefault(elm => elm.Value == "3").Selected = true;
            toReturn.SautDeLigne = 1;
            toReturn.GuidId = ALBINGIA.OP.OP_MVC.MvcApplication.ALBINDEXLIST;
            toReturn.NumOrdreAffichage = 1;
            return toReturn;
        }

        public static List<ParamISLigneModele> InitListeLignes(string modeleId)
        {
            List<ParamISLigneModele> toReturn = new List<ParamISLigneModele>();
            if (!string.IsNullOrEmpty(modeleId))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    var lstLignes = serviceContext.GetISModeleLignes(modeleId, string.Empty, string.Empty);
                    lstLignes.ForEach(elm => toReturn.Add((ParamISLigneModele)elm));
                    _lstLinkBehaviour = null;

                    foreach (ParamISLigneModele ligne in toReturn)
                    {
                        SetReadOnlyValueModeleFromListe(ligne);
                      
                        if (ligne.LienParent != 0)
                            ligne.LienParentLibelle = ligne.LienParent.ToString() + " - " + ligne.LienParentLibelle;
                    }
                }
            }
            return toReturn;
        }

        private ParamISLigneModele InitLigne(ParamISLigneModele ligne)
        {
            ligne.ListAffichage = LstRefOuiNon;
            SetSelectedItemModeleIS(ligne.ListAffichage, ligne.Affichage);

            ligne.ListControle = LstRefOuiNon;
            SetSelectedItemModeleIS(ligne.ListControle, ligne.Controle);

            ligne.ListModifiable = LstRefOuiNon;
            SetSelectedItemModeleIS(ligne.ListModifiable, ligne.Modifiable);

            ligne.ListObligatoire = LstRefOuiNon;
            SetSelectedItemModeleIS(ligne.ListObligatoire, ligne.Obligatoire);

            //ligne.ListReferentiel = LstReferentiels;
            //SetSelectedItemModeleIS(ligne.ListReferentiel, ligne.Referentiel);

            ligne.ListPresentation = LstRefHierarchyOrder;
            SetSelectedItemModeleIS(ligne.ListPresentation, ligne.Presentation);

            //ligne.ListLinkBehaviour = LstLinkBehaviour(ligne.ModeleIS);
            //SetSelectedItemModeleIS(ligne.ListLinkBehaviour, ligne.LienParent.ToString());

            return ligne;
        }

        private static void SetSelectedItemModeleIS(List<AlbSelectListItem> lookUp, string compareValue, AlbSelectListItem selectedItem = null)
        {
            selectedItem = lookUp.FirstOrDefault(elm => elm.Value == compareValue);
            if (selectedItem != null)
                selectedItem.Selected = true;
        }

        private static void SetReadOnlyValueModeleFromListe(ParamISLigneModele ligne)
        {
            ligne.Affichage = LstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Affichage) != null ? LstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Affichage).Text : ligne.Affichage;
            ligne.Controle = LstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Controle) != null ? LstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Controle).Text : ligne.Controle;
            ligne.Modifiable = LstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Modifiable) != null ? LstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Modifiable).Text : ligne.Modifiable;
            ligne.Obligatoire = LstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Obligatoire) != null ? LstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Obligatoire).Text : ligne.Obligatoire;

            ligne.ReferentielId = ligne.Referentiel;
            ligne.Referentiel = LstReferentiels.FirstOrDefault(elm => elm.Value == ligne.Referentiel) != null ? LstReferentiels.FirstOrDefault(elm => elm.Value == ligne.Referentiel).Text : ligne.Referentiel;
            ligne.Presentation = LstRefHierarchyOrder.FirstOrDefault(elm => elm.Value == ligne.Presentation) != null ? LstRefHierarchyOrder.FirstOrDefault(elm => elm.Value == ligne.Presentation).Text : ligne.Presentation;
            ligne.LienParentLibelle = LstLinkBehaviour(ligne.ModeleIS).FirstOrDefault(elm => elm.Value == ligne.LienParent.ToString()) != null ? LstLinkBehaviour(ligne.ModeleIS).FirstOrDefault(elm => elm.Value == ligne.LienParent.ToString()).Text : ligne.LienParentLibelle;
        }

        private static void SetReadOnlyValueReferentielFromListe(LigneModeleIS ligne)
        {
            ligne.Affichage = LstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Affichage) != null ? LstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Affichage).Text : ligne.Affichage;
            ligne.Controle = LstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Controle) != null ? LstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Controle).Text : ligne.Controle;
            ligne.Mappage = LstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Mappage) != null ? LstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Mappage).Text : ligne.Mappage;
            ligne.Obligatoire = LstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Obligatoire) != null ? LstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Obligatoire).Text : ligne.Obligatoire;

            ligne.Conversion = ParamISReferenceController._lstRefConvertTo.FirstOrDefault(elm => elm.Value == ligne.Conversion) != null ? ParamISReferenceController._lstRefConvertTo.FirstOrDefault(elm => elm.Value == ligne.Conversion).Text : ligne.Conversion;
            ligne.PresentationLabel = ParamISReferenceController._lstRefHierarchyOrder.FirstOrDefault(elm => elm.Value == ligne.Presentation.ToString()) != null ? ParamISReferenceController._lstRefHierarchyOrder.FirstOrDefault(elm => elm.Value == ligne.Presentation.ToString()).Text : ligne.Presentation.ToString();
            ligne.TypeUI = ParamISReferenceController._lstRefUIControl.FirstOrDefault(elm => elm.Value == ligne.TypeUI) != null ? ParamISReferenceController._lstRefUIControl.FirstOrDefault(elm => elm.Value == ligne.TypeUI).Text : ligne.TypeUI;
            ligne.TypeZone = ParamISReferenceController._lstRefType.FirstOrDefault(elm => elm.Value == ligne.TypeZone) != null ? ParamISReferenceController._lstRefType.FirstOrDefault(elm => elm.Value == ligne.TypeZone).Text : ligne.TypeZone;

        }

        private static string BuildReferentielTitle(LigneModeleISDto reference)
        {
            string toReturn = string.Empty;
            toReturn += "Nom interne : " + reference.Code;
            toReturn += "\nLibellé affiché : " + reference.LibelleAffiche;
            toReturn += "\nType de zone : " + reference.TypeZone;
            toReturn += "\nLongueur de zone : " + reference.LongueurZone;
            toReturn += "\nMappage : " + (LstRefOuiNon.FirstOrDefault(el => el.Value == reference.Mappage) == null ? reference.Mappage : LstRefOuiNon.FirstOrDefault(el => el.Value == reference.Mappage).Text);
            toReturn += "\nType de conversion : " + reference.Conversion;
            toReturn += "\nType de présentation : " + (LstRefHierarchyOrder.FirstOrDefault(el => el.Value == reference.Presentation.ToString()) == null ? reference.Presentation.ToString() : LstRefHierarchyOrder.FirstOrDefault(el => el.Value == reference.Presentation.ToString()).Text);
            toReturn += "\nType UI de contrôle : " + reference.TypeUI;
            toReturn += "\nObligatoire : " + (LstRefOuiNon.FirstOrDefault(el => el.Value == reference.Obligatoire) == null ? reference.Obligatoire : LstRefOuiNon.FirstOrDefault(el => el.Value == reference.Obligatoire).Text);
            toReturn += "\nPrésence script affichage : " + (LstRefOuiNon.FirstOrDefault(el => el.Value == reference.Affichage) == null ? reference.Affichage : LstRefOuiNon.FirstOrDefault(el => el.Value == reference.Affichage).Text);
            toReturn += "\nPrésence script contrôle : " + (LstRefOuiNon.FirstOrDefault(el => el.Value == reference.Controle) == null ? reference.Controle : LstRefOuiNon.FirstOrDefault(el => el.Value == reference.Controle).Text);
            toReturn += "\nObservations : " + reference.Observation;
            return toReturn;

        }

        private void RefreshCacheIS()
        {
            ALBINGIA.OP.OP_MVC.Common.CacheIS.InitCacheIS();
            ALBINGIA.OP.OP_MVC.Common.CacheIS.SetIsModelsEntete();
            ALBINGIA.OP.OP_MVC.Common.CacheIS.SetIsModelsDto();
            ALBINGIA.OP.OP_MVC.Common.CacheIS.SetIsModels();

        }

        #endregion


    }
}
