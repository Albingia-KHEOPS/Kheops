using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamISAssocier;
using OPServiceContract.IAdministration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ParamISModelesController : ControllersBase<ModeleParamISModelesPage>
    {
        #region Membres Privés

        private List<AlbSelectListItem> lstRefOuiNon
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

        private List<AlbSelectListItem> _lstReferentiels;
        private List<AlbSelectListItem> lstReferentiels
        {
            get
            {
                //Si les données ont déjà été chargé en mémoire, on renvoit une nouvelle instance de la liste
                if (this._lstReferentiels != null)
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
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    var lstRef = serviceContext.GetISReferenciel(string.Empty, false);
                    lstRef.ForEach(elm => value.Add(new AlbSelectListItem
                    {
                        Value = elm.Code,
                        Text = elm.Code + " - " + elm.Description,
                        Title = elm.Code + " - " + elm.Description,
                        Selected = false
                    }
                    ));
                }
                this._lstReferentiels = value;
                return value;
            }
        }

        #endregion
        
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Paramétrage IS Modèles";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            LoadInfoPage();
            DisplayBandeau();
            return View(model);
        }

        [ErrorHandler]
        public PartialViewResult GetDetailsModele(string modeleName)
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
                toReturn.ListeLignesModele = null;
            }
            if (toReturn == null)
                toReturn = new ParamISModele();
            toReturn.ModeAssociation = false;
            return PartialView("ModeleComplet", toReturn);
        }

        [ErrorHandler]
        public PartialViewResult LoadPopupSupprModele(string modeleName)
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
                toReturn.ListeLignesModele = ParamISModeleAssocierController.InitListeLignes(modeleName);
            }
            if (toReturn == null)
                toReturn = new ParamISModele();
            toReturn.ModeAssociation = false;
            return PartialView("ConfirmSuppr", toReturn);
        }

        [ErrorHandler]
        public PartialViewResult SupprimerModele(string modeleName)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                serviceContext.SupprimerModeleEtDependances(modeleName);
            }

            LoadListeModeles();
            return PartialView("/Views/ParamISModeleAssocier/ListeDrlModelesIS.cshtml", model.ListeModelesISModele);
        }

        #region Méthodes Privées

        /// <summary>
        /// Initialise les éléments de base de la page
        /// </summary>       
        protected override void LoadInfoPage(string context = null)
        {
            LoadListeModeles();
            model.SelectedModele = new ParamISModele();
            model.SelectedModele.ModeAssociation = false;
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
             

        #endregion
    }
}
