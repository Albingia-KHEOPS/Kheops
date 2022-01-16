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
    public class GestionGrilleNomenclatureController : ControllersBase<ModeleGestionGrilleNomenclaturePage>
    {

        #region Méthodes publiques
        
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Grille de nomenclatures";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            DisplayBandeau();
            LoadInfoPage();
            return View(model);
        }

        [ErrorHandler]
        public ActionResult LoadListGrille(string searchGrille)
        {
            ModeleGestionGrilleNomenclaturePage model = new ModeleGestionGrilleNomenclaturePage { Grilles = new List<ModeleGrille>() };
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.LoadInfoGestionGrille(searchGrille);
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        model.Grilles.Add((ModeleGrille)item);
                    }
                }
            }
            return PartialView("ListeGrilles", model);
        }

        [ErrorHandler]
        public ActionResult OpenGrille(string idGrille)
        {
            ModeleGrille model = new ModeleGrille { Typologies = new List<ModeleTypologie>(), LstTypologie = new List<AlbSelectListItem>(), LstLien = new List<AlbSelectListItem>() };
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.OpenGrille(idGrille);
                if (result != null)
                {
                    model = (ModeleGrille)result;
                    model.LstTypologie = result.LstTypologie.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.LstLien = new List<AlbSelectListItem>();// result.LstLien.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                }
                LoadLienTypologieGrille(model);
            }
            return PartialView("AjoutGrille", model);
        }

        [ErrorHandler]
        public string SaveGrille(string codeGrille, string libelleGrille, int newGrille)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                return serviceContext.SaveGrille(codeGrille, libelleGrille, newGrille);
            }
        }

        [ErrorHandler]
        public void DeleteGrille(string codeGrille)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                serviceContext.DeleteGrille(codeGrille);
            }
        }

        [ErrorHandler]
        public string SaveLineGrille(string codeGrille, string libelleGrille, string newGrille,
            string typologie, string libTypologie, string lienTypologie, string ordreTypologie)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                return serviceContext.SaveLineGrille(codeGrille, libelleGrille, newGrille, typologie, libTypologie, lienTypologie, ordreTypologie);
            }
        }

        [ErrorHandler]
        public void DeleteLineGrille(string codeGrille, string ordreTypologie)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                serviceContext.DeleteLineGrille(codeGrille, ordreTypologie);
            }
        }

        [ErrorHandler]
        public ActionResult OpenSelectionValeur(string codeGrille, string typoGrille, string niveau, string lien)
        {
            ModeleSelectionValeurs model = new ModeleSelectionValeurs();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.OpenSelectionValeur(codeGrille, typoGrille, niveau);
                model = (ModeleSelectionValeurs)result;
                model.Lien = lien;
            }
            model.Filtres = LoadListFiltres();

            return PartialView("SelectionValeurs", model);
        }

        [ErrorHandler]
        public void SaveValeurs(string codeGrille, string typologie, string niveau, string niveauMere,
            string selVal1, string selVal2, string selVal3, string selVal4, string selVal5,
            string selVal6, string selVal7, string selVal8, string selVal9, string selVal10)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                serviceContext.SaveValeurs(codeGrille, typologie, niveau, niveauMere,
                             selVal1, selVal2, selVal3, selVal4, selVal5,
                             selVal6, selVal7, selVal8, selVal9, selVal10);
            }
        }

        [ErrorHandler]
        public ActionResult SearchValeurNomenclature(string codeGrille, string typologie, string idMere, string searchTerm)
        {
            ModeleSelectionValeurs model = new ModeleSelectionValeurs();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.SearchValeurNomenclature(codeGrille, typologie, idMere, searchTerm);
                model = (ModeleSelectionValeurs)result;
            }
            model.Filtres = LoadListFiltres();

            return PartialView("ListeSelectionValeurs", model);
        }

        [ErrorHandler]
        public ActionResult LoadValeurs(string codeGrille, string idMere, string niveau)
        {
            ModeleSelectionValeurs model = new ModeleSelectionValeurs();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.LoadValeurs(codeGrille, idMere, niveau);
                model = (ModeleSelectionValeurs)result;
            }
            return PartialView("ListeSelectionValeurs", model);
        }

        [ErrorHandler]
        public ActionResult LoadListValeurs(string codeGrille, string idMere, string niveau)
        {
            ModeleTypologie model = new ModeleTypologie();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.LoadListValeurs(codeGrille, idMere, niveau);
                model = (ModeleTypologie)result;
            }

            return PartialView("ListeDeroulanteValeurs", model);
        }

        [ErrorHandler]
        public ActionResult ReloadListValeurs(string codeGrille, string idMere, string niveau)
        {
            ModeleTypologie model = new ModeleTypologie();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.ReloadListValeurs(codeGrille, idMere, niveau);
                model = (ModeleTypologie)result;
            }

            return PartialView("ListeDeroulanteValeurs", model);
        }

        #endregion

        #region Méthodes privées

        protected override void LoadInfoPage(string context = null)
        {
            model.Grilles = new List<ModeleGrille>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.LoadInfoGestionGrille(string.Empty);
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        model.Grilles.Add((ModeleGrille)item);
                    }
                }
            }
        }

        private void LoadLienTypologieGrille(ModeleGrille model)
        {
            string lien = string.Empty;

            foreach (var item in model.Typologies)
            {
                List<AlbSelectListItem> liens = new List<AlbSelectListItem>();
                switch (lien)
                {
                    case "1":
                        liens.Add(new AlbSelectListItem { Value = "2", Text = "2", Selected = false, Title = "2" });
                        break;
                    case "2":
                        liens.Add(new AlbSelectListItem { Value = "1", Text = "1", Selected = false, Title = "1" });
                        liens.Add(new AlbSelectListItem { Value = "3", Text = "3", Selected = false, Title = "3" });
                        break;
                    case "3":
                        liens.Add(new AlbSelectListItem { Value = "1", Text = "1", Selected = false, Title = "1" });
                        liens.Add(new AlbSelectListItem { Value = "4", Text = "4", Selected = false, Title = "4" });
                        break;
                    case "4":
                        liens.Add(new AlbSelectListItem { Value = "1", Text = "1", Selected = false, Title = "1" });
                        liens.Add(new AlbSelectListItem { Value = "5", Text = "5", Selected = false, Title = "5" });
                        break;
                    default:
                        liens.Add(new AlbSelectListItem { Value = "1", Text = "1", Selected = false, Title = "1" });
                        break;
                }
                liens.Add(new AlbSelectListItem { Value = "I", Text = "I", Selected = false, Title = "I" });

                lien = item.Lien;
                item.Liens = liens;
            }
        }

        private List<AlbSelectListItem> LoadListFiltres()
        {
            List<AlbSelectListItem> lstFiltre = new List<AlbSelectListItem>();

            lstFiltre.Add(new AlbSelectListItem { Value = "", Text = "Toutes", Selected = false, Title = "Toutes les valeurs" });
            lstFiltre.Add(new AlbSelectListItem { Value = "C", Text = "Cochées", Selected = false, Title = "Valeurs cochées" });
            lstFiltre.Add(new AlbSelectListItem { Value = "N", Text = "Non cochées", Selected = false, Title = "Valeurs non cochées" });

            return lstFiltre;
        }

        #endregion

    }
}
