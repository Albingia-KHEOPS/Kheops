
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.IOFile;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModeleStatisque;
using OP.WSAS400.DTO.Ecran.Rercherchesaisie;
using OPServiceContract.IAdministration;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class StatistiqueController : ControllersBase<ModeleStatistiqueOffreContratPage>
    {
        #region Méthodes publiques

        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Statistique Offre/Contrat";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            DisplayBandeau();

            LoadData();

            return View(model);
        }

        private void LoadData()
        {
            model.Filtre = new StatOffreContratFiltreModel
            {
                Categorie="2",
                Annee = DateTime.Now.Year.ToString(),
                Mois = DateTime.Now.Month.ToString(),
                Jour = DateTime.Now.Day.ToString()
            };

            SetTypesListModel();

            SetEtatsSituationsListModel();

            SetBranchesListModel();
        }

        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult List(StatOffreContratFiltreModel filtre)
        {
            SetOffreStatContratQueryResult(filtre);
            return PartialView("ListResultOffreContrat", model.ListStat);
        }

        private void SetTypesListModel()
        {
            model.Filtre.Types = new List<AlbSelectListItem> {
                new AlbSelectListItem {Value="O",Text="O - Offre" }, 
                new AlbSelectListItem {Value="P",Text="P - Contrat",Selected=true} };
        }

        private void SetBranchesListModel()
        {
            //preparer la liste des branches disponibles
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var branches = serviceContext.BranchesGet().ToList();
                model.Filtre.Branches = branches.Select(m => new AlbSelectListItem
                {
                    Value = m.Code,
                    Text = m.Code + " - " + m.Nom,
                    Selected = false
                }).ToList();
            }

            //select default RS
            var rs = model.Filtre.Branches.FirstOrDefault(x => x.Value == "RS");
            rs.Selected = true;
        }

        private void SetOffreStatContratQueryResult(StatOffreContratFiltreModel filtre)
        {
            // GET STAT
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                model.ListStat = serviceContext.GetStatOffreContrat(filtre.ToDto())
                    .Select(x => (StatOffreContratModel)x).ToList();
            }
        }

        private void SetEtatsSituationsListModel()
        {
            //get situation LIST
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRechercheSaisie>())
            {
                var screenClient=client.Channel;
                var query = new RechercheSaisieGetQueryDto
                {
                    IsAdmin = ALBINGIA.OP.OP_MVC.Common.CacheUserRights.UserRights.Any(
                        el => el.TypeDroit == TypeDroit.A.ToString() || el.TypeDroit == TypeDroit.M.ToString()),
                    IsUserHorse = MvcApplication.USER_HORSE.IndexOf(GetUser()) > 0
                };
                var result = screenClient.RechercheSaisieGet(query);

                model.Filtre.Etats = result.Etats.Select(m => new AlbSelectListItem
                {
                    Value = m.Code,
                    Text = !string.IsNullOrEmpty(m.Code) ? string.Format("{0} - {1}", m.Code, m.Libelle) : string.Empty,
                    Selected = false,
                    Title = !string.IsNullOrEmpty(m.Code) ? string.Format("{0} - {1}", m.Code, m.Libelle) : string.Empty
                }).ToList();

                model.Filtre.Situations = result.Situation.Select(m => new AlbSelectListItem
                {
                    Value = m.Code,
                    Text = !string.IsNullOrEmpty(m.Code) ? string.Format("{0} - {1}", m.Code, m.Libelle) : string.Empty,
                    Selected = false,
                    Title = !string.IsNullOrEmpty(m.Code) ? string.Format("{0} - {1}", m.Code, m.Libelle) : string.Empty
                }).ToList();
            }
        }


        [ErrorHandler]
        [AlbApplyUserRole]
        public ExportToCSVResult<StatOffreContratModel> ExportFile(string branche, string categorie, string situation,
                                                                 string etat, string type, string annee, string mois, string jour)
        {

            string fileName = "ResultatRecherche.csv";
            string columns = "Num;Version;Type;Reference;Gestionnaire;Souscripteur;Branche;Cible";

            StatOffreContratFiltreModel filtre = new StatOffreContratFiltreModel
            {
                Categorie = categorie,
                Etat = etat,
                Type = type,
                Annee = annee,
                Mois = mois,
                Jour = jour
            };

            SetOffreStatContratQueryResult(filtre);
            var ret = new ExportToCSVResult<StatOffreContratModel>(model.ListStat, fileName, columns);
            return ret;
        }

        #endregion
    }
}
