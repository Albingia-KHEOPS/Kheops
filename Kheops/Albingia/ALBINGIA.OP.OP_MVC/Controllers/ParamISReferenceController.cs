using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamISReference;
using OP.WSAS400.DTO.ParamIS;
using OPServiceContract.IAdministration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ParamISReferenceController : ControllersBase<ModeleParamISReferencePage>
    {
        #region Membres Privés

        private const string msgTypeZoneVide = "Le type de données ne peut être vide";
        private const string msgTailleZoneVide = "La taille du champ de données ne peut être vide";
        private const string msgTailleZoneIncorrect = "Le format du champ taille est incorrect pour ce type de données";
        //private const string msgModeleRequeteVide = "Les requêtes ne peuvent être vides";
        //private const string msgTypeOperationVide = "Le type d'opération à effectuer en BDD est vide";
        //private const string msgPbEnregistrement = "Un problème est survenu lors de l'enregistrement";
        //private const string msgChampsInvalides = "Certains champs obligatoires sont vides ou incorrects";


        public static List<AlbSelectListItem> _lstRefConvertTo
        {
            get
            {
                //Nouvelle instance à chaque récupération de la référence
                var value = new List<AlbSelectListItem>
                  {
                    new AlbSelectListItem{Value = " ", Text = string.Empty},
                    new AlbSelectListItem{Value = "B", Text = "Bool"},
                    new AlbSelectListItem{Value = "D", Text = "Date"},
                    new AlbSelectListItem{Value = "H", Text = "Heure"},
                    new AlbSelectListItem{Value = "N", Text = "Numeric"},
                    new AlbSelectListItem{Value = "L", Text = "Decimal"},
                    new AlbSelectListItem{Value = "\"\"", Text = "String"}
                  };
              return value;
            }
        }

        public static List<AlbSelectListItem> _lstRefHierarchyOrder
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

        public static List<AlbSelectListItem> _lstRefOuiNon
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

        public static List<AlbSelectListItem> _lstRefUIControl
        {
            get
            {
                //Nouvelle instance à chaque récupération de la référence
                List<AlbSelectListItem> value = new List<AlbSelectListItem>();
                value.Add(new AlbSelectListItem() { Value = "Hidden", Text = "" });
                value.Add(new AlbSelectListItem() { Value = "Text", Text = "Text" });
                value.Add(new AlbSelectListItem() { Value = "TextArea", Text = "TextArea" });
                value.Add(new AlbSelectListItem() { Value = "Select", Text = "Select" });
                value.Add(new AlbSelectListItem() { Value = "Checkbox", Text = "Checkbox" });
                value.Add(new AlbSelectListItem() { Value = "Date", Text = "Date" });
                value.Add(new AlbSelectListItem() { Value = "Heure", Text = "Heure" });
                value.Add(new AlbSelectListItem() { Value = "Periode", Text = "Période" });
                value.Add(new AlbSelectListItem() { Value = "PeriodeHeure", Text = "Période heure" });

                return value;
            }
        }

        public static List<AlbSelectListItem> _lstRefType
        {
            get
            {
                //Nouvelle instance à chaque récupération de la référence
                List<AlbSelectListItem> value = new List<AlbSelectListItem>();
                value.Add(new AlbSelectListItem() { Value = "Double", Text = "Double" });
                value.Add(new AlbSelectListItem() { Value = "Int64", Text = "Int64" });
                value.Add(new AlbSelectListItem() { Value = "string", Text = "String" });
                return value;
            }
        }

        #endregion
        
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Paramétrage IS Référence";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            LoadInfoPage();
            DisplayBandeau();
            return View(model);
        }

        [ErrorHandler]
        public PartialViewResult SaveLineReference(string code, string description, string libelle, string typeZone, string longueurZone,
            string mappage, string conversion, int presentation, string typeUI, string obligatoire,
            string affichage, string controle, string observation, string typeOperation, string valeurDefaut, string tcon, string tfam)
        {
            #region Vérification des valeurs avant insertion

            if (string.IsNullOrEmpty(typeZone))
                throw new AlbFoncException(msgTypeZoneVide, false, false, true);

            if (string.IsNullOrEmpty(longueurZone))
                throw new AlbFoncException(msgTailleZoneVide, false, false, true);

            if (typeZone == "Double")
            {
                if (longueurZone.Split(':').Length == 2)
                {
                    int longueurEntier = -1;
                    int longueurDecimal = -1;
                    int.TryParse(longueurZone.Split(':')[0], out longueurEntier);
                    int.TryParse(longueurZone.Split(':')[1], out longueurDecimal);

                    if (longueurEntier < 1 || longueurEntier > 12)
                        throw new AlbFoncException(msgTailleZoneIncorrect, false, false, true);

                    if (longueurDecimal < 1 || longueurDecimal > 5)
                        throw new AlbFoncException(msgTailleZoneIncorrect, false, false, true);
                }
                else
                    throw new AlbFoncException(msgTailleZoneIncorrect, false, false, true);
            }
            else if (typeZone == "Int64")
            {
                int longueurEntier = -1;
                int.TryParse(longueurZone, out longueurEntier);

                if (longueurEntier < 1 || longueurEntier > 12)
                    throw new AlbFoncException(msgTailleZoneIncorrect, false, false, true);
            }
            else if (typeZone == "string")
            {
                int longueurEntier = -1;
                int.TryParse(longueurZone, out longueurEntier);

                if (longueurEntier < 1 || longueurEntier > 5000)
                    throw new AlbFoncException(msgTailleZoneIncorrect, false, false, true);
            }
            else
                throw new AlbFoncException(msgTypeZoneVide, false, false, true);

            #endregion

            LigneModeleISDto reference = new LigneModeleISDto
            {
                Code = code,
                Description = description,
                LibelleAffiche = libelle.Replace("'", "''"),
                TypeZone = typeZone,
                LongueurZone = longueurZone,
                Mappage = mappage,
                Conversion = conversion,
                Presentation = presentation,
                TypeUI = typeUI,
                Obligatoire = obligatoire,
                Affichage = affichage,
                Controle = controle,
                Observation = observation,
                TypeOperation = typeOperation,
                ValeurDefaut = valeurDefaut,
                Tcon = tcon,
                Tfam = tfam
            };

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                serviceContext.SaveISReferenciel(reference);
            }

            LoadReferences();
            return PartialView("ListeISReferentiels", model.ListeReferentiels);
        }

        [ErrorHandler]
        public PartialViewResult SupprimerLineReference(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    serviceContext.SupprimerISReferenciel(code);
                }
            }
            LoadReferences();
            return PartialView("ListeISReferentiels", model.ListeReferentiels);
        }

        [ErrorHandler]
        public PartialViewResult AfficherLigne(string mode, string code)
        {
            LigneModeleIS toReturn = null;
            if (!string.IsNullOrEmpty(code))
            {
                if (code != ALBINGIA.OP.OP_MVC.MvcApplication.ALBINDEXLIST.ToString())
                {
                    string partialViewName = (mode == "readonly" ? "LigneISReferentielReadOnly" : "LigneISReferentielEdition");
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                    {
                        var serviceContext=client.Channel;
                        var lstRef = serviceContext.GetISReferenciel(code, false);
                        if (lstRef.Any())
                        {
                            toReturn = (LigneModeleIS)lstRef.FirstOrDefault();
                            if (mode == "edition")
                            {
                                toReturn.ListAffichage = _lstRefOuiNon;
                                SetSelectedItemModeleIS(toReturn.ListAffichage, toReturn.Affichage);

                                toReturn.ListControle = _lstRefOuiNon;
                                SetSelectedItemModeleIS(toReturn.ListControle, toReturn.Controle);

                                toReturn.ListConversion = _lstRefConvertTo;
                                SetSelectedItemModeleIS(toReturn.ListConversion, toReturn.Conversion);

                                toReturn.ListMappage = _lstRefOuiNon;
                                SetSelectedItemModeleIS(toReturn.ListMappage, toReturn.Mappage);

                                toReturn.ListObligatoire = _lstRefOuiNon;
                                SetSelectedItemModeleIS(toReturn.ListObligatoire, toReturn.Obligatoire);

                                toReturn.ListPresentation = _lstRefHierarchyOrder;
                                SetSelectedItemModeleIS(toReturn.ListPresentation, toReturn.Presentation.ToString());

                                toReturn.ListTypesUI = _lstRefUIControl;
                                SetSelectedItemModeleIS(toReturn.ListTypesUI, toReturn.TypeUI);

                                toReturn.ListTypeZone = _lstRefType;
                                SetSelectedItemModeleIS(toReturn.ListTypeZone, toReturn.TypeZone);
                            }
                            else
                                SetReadOnlyValueFromListe(toReturn);
                            return PartialView(partialViewName, toReturn);
                        }
                    }
                }
                else//Mode nouvelle ligne
                {
                    toReturn = new LigneModeleIS()
                    {
                        ListAffichage = _lstRefOuiNon,
                        ListControle = _lstRefOuiNon,
                        ListConversion = _lstRefConvertTo,
                        ListMappage = _lstRefOuiNon,
                        ListObligatoire = _lstRefOuiNon,
                        ListPresentation = _lstRefHierarchyOrder,
                        ListTypesUI = _lstRefUIControl,
                        ListTypeZone = _lstRefType,
                        Code = ALBINGIA.OP.OP_MVC.MvcApplication.ALBINDEXLIST.ToString()
                    };
                    return PartialView("LigneISReferentielEdition", toReturn);
                }
            }
            throw new AlbFoncException("Impossible d'afficher cette ligne", trace: false, sendMail: false, onlyMessage: true);

        }

        #region Méthodes Privées

        /// <summary>
        /// Initialise les éléments de base de la page
        /// </summary>
        [ErrorHandler]
        protected override void LoadInfoPage(string context = null)
        {
            #region init ligne vide
            model.LigneVide = new LigneModeleIS()
            {
                ListAffichage = _lstRefOuiNon,
                ListControle = _lstRefOuiNon,
                ListConversion = _lstRefConvertTo,
                ListMappage = _lstRefOuiNon,
                ListObligatoire = _lstRefOuiNon,
                ListPresentation = _lstRefHierarchyOrder,
                ListTypesUI = _lstRefUIControl,
                ListTypeZone = _lstRefType
            };
            #endregion
            LoadReferences();
        }

        /// <summary>
        /// Charge les informations spécifiques référentiel
        /// </summary>
        private void LoadReferences()
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var lstRef = serviceContext.GetISReferenciel(string.Empty, false);
                model.ListeReferentiels = lstRef.Select(elm => (LigneModeleIS)elm).ToList();
                //init de chaque ligne
                foreach (LigneModeleIS reference in model.ListeReferentiels)
                {
                    SetReadOnlyValueFromListe(reference);
                }

            }
        }

        private static void SetSelectedItemModeleIS(List<AlbSelectListItem> lookUp, string compareValue, AlbSelectListItem selectedItem = null)
        {
            selectedItem = lookUp.FirstOrDefault(elm => elm.Value == compareValue);
            if (selectedItem != null)
                selectedItem.Selected = true;
        }

        private static void SetReadOnlyValueFromListe(LigneModeleIS ligne)
        {
            ligne.Affichage = _lstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Affichage) != null ? _lstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Affichage).Text : ligne.Affichage;
            ligne.Controle = _lstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Controle) != null ? _lstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Controle).Text : ligne.Controle;
            ligne.Mappage = _lstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Mappage) != null ? _lstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Mappage).Text : ligne.Mappage;
            ligne.Obligatoire = _lstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Obligatoire) != null ? _lstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.Obligatoire).Text : ligne.Obligatoire;
            ligne.ValeurDefaut = _lstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.ValeurDefaut) != null ? _lstRefOuiNon.FirstOrDefault(elm => elm.Value == ligne.ValeurDefaut).Text : ligne.ValeurDefaut;

            ligne.Conversion = _lstRefConvertTo.FirstOrDefault(elm => elm.Value == ligne.Conversion) != null ? _lstRefConvertTo.FirstOrDefault(elm => elm.Value == ligne.Conversion).Text : ligne.Conversion;
            ligne.PresentationLabel = _lstRefHierarchyOrder.FirstOrDefault(elm => elm.Value == ligne.Presentation.ToString()) != null ? _lstRefHierarchyOrder.FirstOrDefault(elm => elm.Value == ligne.Presentation.ToString()).Text : ligne.Presentation.ToString();
            ligne.TypeUI = _lstRefUIControl.FirstOrDefault(elm => elm.Value == ligne.TypeUI) != null ? _lstRefUIControl.FirstOrDefault(elm => elm.Value == ligne.TypeUI).Text : ligne.TypeUI;
            ligne.TypeZone = _lstRefType.FirstOrDefault(elm => elm.Value == ligne.TypeZone) != null ? _lstRefType.FirstOrDefault(elm => elm.Value == ligne.TypeZone).Text : ligne.TypeZone;

        }

        #endregion


    }
}
