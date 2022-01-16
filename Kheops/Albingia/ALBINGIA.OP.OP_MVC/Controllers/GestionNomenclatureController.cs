using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesGestionNomenclature;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OPServiceContract.IAdministration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class GestionNomenclatureController : ControllersBase<ModeleGestionNomenclaturePage>
    {
        #region Méthodes Publiques
        
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Gestion des nomenclatures";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            DisplayBandeau();
            LoadInfoPage();
            return View(model);
        }

        [ErrorHandler]
        public ActionResult LoadListNomenclature(string typologie, string branche, string cible)
        {
            ModeleGestionNomenclaturePage model = new ModeleGestionNomenclaturePage { Nomenclatures = new List<ModeleNomenclature>() };

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.LoadListNomenclature(typologie, branche, cible);
                if (result != null)
                {
                    foreach (var item in result.Nomenclatures)
                    {
                        model.Nomenclatures.Add((ModeleNomenclature)item);
                    }
                }
            }

            return PartialView("ListeNomenclatures", model);
        }

        [ErrorHandler]
        public ActionResult OpenNomenclature(string idNomenclature, string typologie)
        {
            ModeleNomenclature model = new ModeleNomenclature { Grilles = new List<ModeleGrille>() };

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.OpenNomenclature(idNomenclature, typologie);
                if (result != null) {
                    model = (ModeleNomenclature)result;
                }
                //model.Grilles = new List<ModeleGrille>();
            }

            return PartialView("AjoutNomenclature", model);
        }

        [ErrorHandler]
        public string SaveNomenclature(string idNomenclature, string codeNomenclature, string ordreNomenclature, string libelleNomenclature, string typologie)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                return serviceContext.SaveNomenclature(idNomenclature, codeNomenclature, ordreNomenclature, libelleNomenclature, typologie);
            }
        }

        [ErrorHandler]
        public void DeleteNomenclature(string idNomenclature)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                serviceContext.DeleteNomenclature(idNomenclature);
            }
        }

        #endregion

        #region Méthodes Privées

        protected override void LoadInfoPage(string context = null)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.LoadInfoNomenclature();
                if (result != null)
                {
                    model.Typologies = result.Typologies.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.Branches = result.Branches.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.Cibles = result.Cibles.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();

                    model.Nomenclatures = new List<ModeleNomenclature>();
                    model.AddNomenclature = new ModeleNomenclature();
                }

            }
        }

        #endregion

    }
}
