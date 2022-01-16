using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamEngment;
using OPServiceContract.IAdministration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ParamEngagementsController : ControllersBase<ModeleParamEngmentPage>
    {
        #region Méthodes Publiques
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Gérer les colonnes engagement";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            LoadInfoPage();
            model.ParamsColonne = new List<Models.ModelesParamEngment.ParamEngmentColonne>();
            DisplayBandeau();
            return View(model);
        }
        [ErrorHandler]
        public ActionResult DisplayListColonne(string codeTraite)
        {
            return PartialView("ParamEngmentListCol", LoadListColonne(codeTraite));
        }
        [ErrorHandler]
        public ActionResult DisplayAddColonne(string codeTraite, string code)
        {
            return PartialView("ParamEngmentAddCol", LoadAddColonne(codeTraite, code));
        }
        [ErrorHandler]
        public void SaveColonne(string codeTraite, string code, string libelle, string separation, string mode)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                serviceContext.SaveColonne(codeTraite, code, libelle, separation, mode);
            }
        }
        [ErrorHandler]
        public string DeleteColonne(string codeTraite, string code)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                return serviceContext.DeleteColonne(codeTraite, code, UserId);
            }
        }
        #endregion
        #region Méthodes Privées
        /// <summary>
        /// Charge les informations de la page
        /// au démarrage
        /// </summary>
        protected override void LoadInfoPage(string context = null)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.InitParamEngment();
                if (result != null && result.Traites != null)
                {
                    model.Traites = result.Traites.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                }
            }
        }
        /// <summary>
        /// Charge la liste de colonne
        /// en fonction du code traité
        /// </summary>
        private List<ParamEngmentColonne> LoadListColonne(string codeTraite)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                ModeleParamEngmentPage model = new ModeleParamEngmentPage();
                var result = serviceContext.GetListColonne(codeTraite);
                if (result != null)
                {
                    model = (ModeleParamEngmentPage)result;
                }
                return model.ParamsColonne;
            }
        }
        /// <summary>
        /// Charge la ligne d'ajout/MAJ
        /// d'une colonne pour un traité
        /// </summary>
        private ParamEngmentColonne LoadAddColonne(string codeTraite, string code)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                ParamEngmentColonne model = new ParamEngmentColonne();
                var result = serviceContext.LoadColonne(codeTraite, code);
                if (result != null)
                {
                    model = (ParamEngmentColonne)result;
                }
                return model;
            }
        }
        #endregion
    }
}
