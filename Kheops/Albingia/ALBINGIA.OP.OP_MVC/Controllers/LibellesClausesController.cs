using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using Albingia.Kheops.OP.Domain.Referentiel;
using Newtonsoft.Json;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModeleLibellesClauses;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OPServiceContract.IClausesRisquesGaranties;
using OP.WSAS400.DTO.Bloc;
using OPServiceContract.IAdministration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OP.WSAS400.DTO.LibelleClauses;
using OPServiceContract.ISaisieCreationOffre;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamTemplateNomenclature;
using ALBINGIA.OP.OP_MVC.Common;
using System;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
  
    public class LibellesClausesController : ControllersBase<ModeleClausePage>
    {
        #region Méthode Publique

        [ErrorHandler]
        [AlbApplyUserRole]
        [AlbVerifLockedOffer("id")]

        public ActionResult Index()
        {
            model.PageTitle = "Clauses";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            DisplayBandeau();
            return View(model);
        }
        [ErrorHandler]
        public ActionResult GetBranchesClauses()
        {
            var result = new List<ClauseBrancheDto>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var ClausesCategoriesClient = client.Channel;
                result = ClausesCategoriesClient.GetClausesBranches().ToList();
               

            }
            return Json( result ,JsonRequestBehavior.AllowGet);
        }
        [ErrorHandler]
        public ActionResult GetCibles(string codeBranche)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext = client.Channel;
                var toReturn = new ModeleListeCibles();
                var result = serviceContext.GetCibles(codeBranche, true, CacheUserRights.UserRights.Any(
                el => el.TypeDroit == TypeDroit.A.ToString() || el.TypeDroit == TypeDroit.M.ToString()), MvcApplication.USER_HORSE.IndexOf(GetUser()) > 0);
                return Json(result, JsonRequestBehavior.AllowGet);
                
            }
        }
        

        [ErrorHandler]
        public ActionResult SaveClausesLibelle(string branche, string cible, string nm1, string nm2, string nm3,string libelle)
        {
            var ToReturn = new List<ModeleClause>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var ClausesCategoriesClient = client.Channel;
                ClausesCategoriesClient.SaveClauseLibelle(branche, cible, nm1, nm2, nm3, libelle);
                var result = ClausesCategoriesClient.GetClausesLibelle("", "", "", "", "").ToList();

                if (result != null && result.Count > 0)
                    result.ForEach(m => ToReturn.Add((ModeleClause)m));
            }
            return PartialView("ListeClauses", ToReturn);
        }
        [ErrorHandler]
        public ActionResult Recherche(string branche, string cible, string nm1, string nm2, string nm3)
        {
            var ToReturn = new List<ModeleClause>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var ClausesCategoriesClient=client.Channel;
                var result = ClausesCategoriesClient.GetClausesLibelle(branche, cible,nm1,nm2,nm3).ToList();

                if (result != null && result.Count > 0)
                    result.ForEach(m => ToReturn.Add((ModeleClause)m));
            }

            return PartialView("ListeClauses", ToReturn);
        }

        
        [ErrorHandler]
        public ActionResult DeleteClausesLibelle(string branche, string cible, string nm1, string nm2, string nm3, string libelle)
        {
            var ToReturn = new List<ModeleClause>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var ClausesCategoriesClient = client.Channel;
                ClausesCategoriesClient.DeleteClausesLibelle(branche, cible, nm1, nm2, nm3, libelle);
                var result = ClausesCategoriesClient.GetClausesLibelle("", "", "", "", "").ToList();

                if (result != null && result.Count > 0)
                    result.ForEach(m => ToReturn.Add((ModeleClause)m));

            }
            return PartialView("ListeClauses", ToReturn);

        }

        [ErrorHandler]
        public ActionResult EditClausesLibelle(string branche, string cible, string nm1, string nm2, string nm3,string libelle)
        {
            var ToReturn = new List<ModeleClause>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var ClausesCategoriesClient = client.Channel;
                ClausesCategoriesClient.UpdateClauseLibelle(branche, cible, nm1, nm2, nm3, libelle);
                var result = ClausesCategoriesClient.GetClausesLibelle("", "", "", "", "").ToList();

                if (result != null && result.Count > 0)
                    result.ForEach(m => ToReturn.Add((ModeleClause)m));

            }

           return PartialView("ListeClauses", ToReturn);
        }

        #endregion
    }
}