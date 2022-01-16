using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamTemplateNomenclature;
using OPServiceContract.IAdministration;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    [ErrorHandler]
    public class ParamTemplateNomenclatureController : ControllersBase<ModeleParamTemplateNomenclaturePage>
    {
        #region Membres statiques
        public static readonly string MODE_EDITION = "Update";
        public static readonly string MODE_CREATION = "Insert";
        public static readonly Int64 ID_LIGNE_VIDE = -9999;
        public static List<AlbSelectListItem> _lstBranches;

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
        public ActionResult RechercheTemplatesNomenclature(string descriptionTemplate)
        {
            //return Liste
            var toReturn = new List<ModeleLigneTemplateNomenclature>();
            return PartialView("ListeNomenclatures", toReturn);
        }

        [ErrorHandler]
        public ActionResult GetDetailsTemplateNomenclature(string idTemplate)
        {
            ModeleDetailsTemplateNomenclature toReturn = null;
            if (string.IsNullOrEmpty(idTemplate))
            {
                toReturn = new ModeleDetailsTemplateNomenclature();
                toReturn.ModeSaisie = MODE_CREATION;
                toReturn.GuidId = ID_LIGNE_VIDE;
                toReturn.Branches = LstBranches();
                toReturn.ModeleCibles = new ModeleListeCibles
                {
                    Cibles = new List<AlbSelectListItem>(),
                    Cible = string.Empty,
                    ModeSaisie = MODE_CREATION
                };
            }
            else
            {
                //Chargement à partir de la bdd
            }
            if (toReturn != null)
                return PartialView("DetailsNomenclature", toReturn);
            else
                throw new AlbTechException(new Exception("Impossible d'afficher les détails de ce template"));
        }

        [ErrorHandler]
        public ActionResult GetCibles(string codeBranche)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext=client.Channel;
                ModeleListeCibles toReturn = new ModeleListeCibles();
                var result = serviceContext.GetCibles(codeBranche, true, true, true);
                if (result != null)
                {
                    toReturn.Cible = string.Empty;
                    toReturn.Cibles = result.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList(); ;
                    toReturn.ModeSaisie = MODE_CREATION;
                }

                return PartialView("ListeCibles", toReturn);
            }
        }

        [ErrorHandler]
        public ActionResult EnregistrerTemplateNomenclature(string modeSaisie, string guidId, string branche, string cible, string libelle)
        {
            //TODO : enregistrement  bdd

            //Return liste complete des templates nomenclature
            var toReturn = new List<ModeleLigneTemplateNomenclature>();
            return RechercheTemplatesNomenclature(string.Empty);
        }


        #endregion

        #region Méthodes privées
        private List<AlbSelectListItem> LstBranches()
        {
            //Si les données ont déjà été chargé en mémoire, on renvoit une nouvelle instance de la liste
            if (_lstBranches != null)
            {
                var toReturn = new List<AlbSelectListItem>();
                _lstBranches.ForEach(elm => toReturn.Add(new AlbSelectListItem
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
                var lstBrch = serviceContext.GetListeBranches();
                lstBrch.ForEach(elm => value.Add(new AlbSelectListItem
                {
                    Value = elm.Code.ToString(CultureInfo.InvariantCulture),
                    Text = elm.Code + " - " + elm.Libelle,
                    Title = elm.Code + " - " + elm.Libelle,
                    Selected = false
                }
                ));
            }
            _lstBranches = value;
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
