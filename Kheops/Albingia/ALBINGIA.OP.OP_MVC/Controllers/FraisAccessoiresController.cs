using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.IOFile;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesFraisAccessoires;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamCible;
using OPServiceContract.IAdministration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class FraisAccessoiresController : ControllersBase<ModeleFraisAccessoiresPage>
    {
        #region Méthodes publiques

        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index(FraisAccessoiresModel filtre)
        {
            model.PageTitle = "Frais Accessoires";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            DisplayBandeau();

            LoadInfosPage(filtre);

            if (this.Request.IsAjaxRequest())
            {
                return PartialView("Body", model);
            }

            return View(model);
        }


        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult List(FraisAccessoiresModel filtre)
        {
            SetFraisAccessoiresQueryResult(filtre,true);
            return PartialView("List", model.List);
        }

        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult New()
        {
            FraisAccessoiresModel model = new FraisAccessoiresModel();

            model.Branches = GetBranchesListModel();
            model.SousBranches = new List<AlbSelectListItem>();
            model.Categories = new List<AlbSelectListItem>();

            return PartialView("EditItem", model);
        }

        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult SaveNew(FraisAccessoiresModel model)
        {
            SetFraisAccessoiresQueryResult(model, false);
            if (base.model.List.Count > 0)
            {
                throw new AlbFoncException("Clé en double", false, false, true);
            }

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                serviceContext.InsertFraisAccessoires(model.ToDto());
            }

            SetFraisAccessoiresQueryResult(model,false);
            //reselectionner la ligne inserrer a partir de la base
            model = base.model.List.FirstOrDefault();

            if (model != null)
            {
                return PartialView("Item", model);
            }
            else
            {
                throw new AlbFoncException("Echec de l'enregistrement", false, false, true);
            }
        }

        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Edit(FraisAccessoiresModel filtre)
        {
            SetFraisAccessoiresQueryResult(filtre,false);
            FraisAccessoiresModel fraisAccessoiresModel = model.List.FirstOrDefault();
            if (model != null)
            {
                fraisAccessoiresModel.Branches = GetBranchesListModel();
                fraisAccessoiresModel.SousBranches = GetSousBranchesListModel(fraisAccessoiresModel.Branche);
                fraisAccessoiresModel.Categories = new List<AlbSelectListItem>();

                return PartialView("EditItem", model);
            }
            else
            {
                throw new AlbFoncException("Donnée introuvable dans la base", false, false, true);
            }
        }

        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult SaveEdit(string filtre, string toSave)
        {
            JavaScriptSerializer serialiser = AlbJsExtendConverter<FraisAccessoiresModel>.GetSerializer();
            FraisAccessoiresModel filtreModel = serialiser.ConvertToType<FraisAccessoiresModel>(serialiser.DeserializeObject(filtre));

            FraisAccessoiresModel toSaveModel = serialiser.ConvertToType<FraisAccessoiresModel>(serialiser.DeserializeObject(toSave));
            
            if (!filtreModel.Equals(toSaveModel))
            {
                SetFraisAccessoiresQueryResult(toSaveModel, false);
                if (model.List.Count > 0)
                {
                    throw new AlbFoncException("Clé en double", false, false, true);
                }
            }

            //save model
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                serviceContext.UpdateFraisAccessoires(filtreModel.ToDto(), toSaveModel.ToDto());
            }

            SetFraisAccessoiresQueryResult(toSaveModel,false);
            //reselectionner la ligne inserrer a partir de la base
            toSaveModel = model.List.FirstOrDefault();

            return PartialView("Item", toSaveModel);
        }


        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Delete(FraisAccessoiresModel filtre)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                serviceContext.DeleteFraisAccessoires(filtre.ToDto());
            }

            return Json(filtre);
        }

        [ErrorHandler]
        public ActionResult GetSousBranches(string codeBranche, string id)
        {
            DropListSousBranches toReturn = new DropListSousBranches();
            toReturn.SousBranches = new List<AlbSelectListItem>();

            toReturn.SousBranches = GetSousBranchesListModel(codeBranche);

            toReturn.Id = id;
            return PartialView("DrlSousBranches", toReturn);
        }



        [ErrorHandler]
        public ActionResult GetCategories(string codeBranche, string codeSousBranche, string id)
        {
            DropListCategories toReturn = new DropListCategories();
            toReturn.Categories = new List<AlbSelectListItem>();

            if (!string.IsNullOrEmpty(codeBranche) && !string.IsNullOrEmpty(codeSousBranche))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var voletsBlocsCategoriesClient=client.Channel;
                    var result = voletsBlocsCategoriesClient.GetCategories(codeBranche, codeSousBranche);
                    result.ForEach(m => toReturn.Categories.Add(new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Code + " - " + m.Libelle }));
                }
            }
            toReturn.Id = id;
            return PartialView("DrlCategories", toReturn);
        }

        [ErrorHandler]
        [AlbApplyUserRole]
        public ExportToCSVResult<FraisAccessoiresModel> ExportFile(string branche,
                                                                   string sousBranche,
                                                                   string categorie,
                                                                   int? annee)
        {

            string fileName = "FraisAccessoires";
            string columns = "Code Branche;Nom Branche;Code SousBranche;Nom SousBranche;Categorie;Annee;Montant;FRaiSACCMIN;FRaiSACCMAX; Clé de la ligne";

            FraisAccessoiresModel filtre = new FraisAccessoiresModel
            {
                Branche = branche,
                SousBranche = sousBranche,
                Categorie = categorie,
                Annee = annee
            };

            SetFraisAccessoiresQueryResult(filtre,true);
          
            var ret = new ExportToCSVResult<FraisAccessoiresModel>(model.List, fileName, columns);
            return ret;
        }

        #endregion

        #region Méthodes private



        private void LoadInfosPage(FraisAccessoiresModel filtre)
        {
            model.Filtre = filtre;
            model.Filtre.SousBranches = new List<AlbSelectListItem>();
            model.Filtre.Categories = new List<AlbSelectListItem>();
            model.Filtre.Branches = GetBranchesListModel();
        }

        private List<AlbSelectListItem> GetBranchesListModel()
        {
            //preparer la liste des branches disponibles
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var branches = serviceContext.BranchesGet().ToList();
                return branches.Select(m => new AlbSelectListItem
                {
                    Value = m.Code,
                    Text = m.Code + " - " + m.Nom,
                    Selected = false
                }).ToList();
            }
        }

        private List<AlbSelectListItem> GetSousBranchesListModel(string codeBranche)
        {
            List<AlbSelectListItem> sousBranches = new List<AlbSelectListItem>();
            if (!string.IsNullOrEmpty(codeBranche))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var voletsBlocsCategoriesClient=client.Channel;
                    var result = voletsBlocsCategoriesClient.GetSousBranches(codeBranche);
                    result.ForEach(m => sousBranches.Add(new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Code + " - " + m.Libelle }));

                }
            }

            return sousBranches;
        }

        private void SetFraisAccessoiresQueryResult(FraisAccessoiresModel filtre ,bool likeCategorie)
        {
            List<AlbSelectListItem> branches = GetBranchesListModel();
            List<AlbSelectListItem> souBraches ;
            Dictionary<string, List<AlbSelectListItem>> sousBranchesCache = new Dictionary<string, List<AlbSelectListItem>>(); ;
            // GET FRAIS
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                model.List = serviceContext.GetFraisAccessoires(filtre.ToDto(), likeCategorie)
                    .Select(x => (FraisAccessoiresModel)x).ToList();
            }

            foreach (var item in model.List)
            {
                item.BrancheLabel = GetSelectedItemLabel(branches, item.Branche);

                if (!sousBranchesCache.TryGetValue(item.Branche, out souBraches))
                {
                    souBraches = GetSousBranchesListModel(item.Branche);
                    sousBranchesCache.Add(item.Branche, souBraches);
                }

                item.SousBrancheLabel = GetSelectedItemLabel(souBraches, item.SousBranche);
            }
        }

        /// <summary>
        /// retourne le text d'un AlbSelectListItem
        /// </summary>
        /// <param name="lookUp"></param>
        /// <param name="compareValue"></param>
        /// <param name="selectedItem"></param>
        /// <returns></returns>
        private string GetSelectedItemLabel(List<AlbSelectListItem> lookUp, string compareValue, AlbSelectListItem selectedItem = null)
        {
            if (!string.IsNullOrEmpty(compareValue) && lookUp != null)
            {
                selectedItem = lookUp.FirstOrDefault(elm => elm.Value == compareValue.Trim());
                if (selectedItem != null)
                    return selectedItem.Text;
            }

            return string.Empty;
        }

        #endregion
    }
}
