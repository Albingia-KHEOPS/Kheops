using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamInventaire;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamInventaires;
using OP.WSAS400.DTO.ParametreInventaires;
using OPServiceContract.IAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ParamInventairesController : ControllersBase<ModeleParamInventairesPage>
    {
        //
        // GET: /ParamInventaire/

        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Paramétrages des inventaires";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            DisplayBandeau();
            model.ListeInventaires = RechercheInventaire(string.Empty, string.Empty);

            return View(model);
        }

        [ErrorHandler]
        public ActionResult Recherche(string code, string description)
        {
            var toReturn = RechercheInventaire(code, description);

            return PartialView("ParamListInventaires", toReturn);
        }


        [ErrorHandler]
        public ActionResult ConsultInventaire(string codeId, string codeInventaire, string libInventaire, string kagtmap, string codeFiltre, string readOnly)
        {
            DetailsInventaire toReturn = new DetailsInventaire
            {
                GuidId = !string.IsNullOrEmpty(codeId) ? Convert.ToInt32(codeId) : 0,
                Code = codeInventaire,
                Libelle = libInventaire,
                Kagtmap = !string.IsNullOrEmpty(kagtmap) ? Convert.ToInt32(kagtmap) : 0,
                CodeFiltre = !string.IsNullOrEmpty(codeFiltre) ? Convert.ToInt64(codeFiltre) : 0
            };

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                var filtres = voletsBlocsCategoriesClient.GetFiltres();

                if (filtres != null)
                    toReturn.Filtres = filtres.Select(p => new AlbSelectListItem
                    {
                        Text = string.Format("{0} - {1}", p.Code, p.Libelle),
                        Value = p.LongId.ToString(),
                        Selected = false,
                        Title = !string.IsNullOrEmpty(p.Code) ? string.Format("{0} - {1}", p.Code, p.Libelle) : ""
                    }).ToList();
            }

            if (!string.IsNullOrEmpty(codeId))
                toReturn.Mode = "Update";
            else
                toReturn.Mode = "Insert";
            return PartialView("ParamDetailInventaire", toReturn);

        }

        [ErrorHandler]
        public ActionResult Enregistrer(string mode, string codeId, string codeLib, string description, int kagtmap, string codeFiltre)
        {
            List<LigneInventaires> toReturn = new List<LigneInventaires>();
            ParamInventairesListDto result = new ParamInventairesListDto();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                if (mode == "Update")
                {
                    result = voletsBlocsCategoriesClient.ModifierInventaire(codeId, codeLib.Replace("'", "''"), description.Replace("'", "''"), kagtmap, codeFiltre);
                }
                else if (mode == "Insert")
                {
                    result = voletsBlocsCategoriesClient.AjouterInventaire(codeLib.Replace("'", "''"), description.Replace("'", "''"), kagtmap, codeFiltre);
                }

                if (result != null)
                {
                    if (result.Inventaires != null && result.Inventaires.Any())
                    {
                        result.Inventaires.ForEach(m => toReturn.Add((LigneInventaires)m));
                    }

                    if (result.ReturnValue == 0)
                        throw new AlbFoncException("Le Code inventaire existe déjà", false, false, true);
                }
                else
                {
                    throw new AlbFoncException("Erreur lors de l'enregistrement", false, false, true);
                }
            }
            return PartialView("ParamListInventaires", toReturn);
        }

        [ErrorHandler]
        public bool SupprimerInventaire(string code)
        {
            int Id;
            if (int.TryParse(code, out Id))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var voletsBlocsCategoriesClient=client.Channel;
                    return voletsBlocsCategoriesClient.SupprimerInventaire(Id).ReturnValue == 1 ? true : false;
                }
            }
            return false;
        }


        #region Méthode privée
        private static List<LigneInventaires> RechercheInventaire(string code, string description)
        {
            var toReturn = new List<LigneInventaires>();
            var masterRole = ALBINGIA.OP.OP_MVC.Common.CacheUserRights.UserRights.Any(el => el.TypeDroit == TypeDroit.M.ToString());

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                var result = voletsBlocsCategoriesClient.LoadInventaire(code, description, masterRole).ToList();

                if (result != null && result.Any())
                {
                    result.ForEach(m => toReturn.Add((LigneInventaires)m));
                }
            }
            return toReturn;
        }
        #endregion
    }
}
