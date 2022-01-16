using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModeleLibellesClauses;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OPServiceContract.IClausesRisquesGaranties;
using OP.WSAS400.DTO.Bloc;
using OPServiceContract.IAdministration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace ALBINGIA.OP.OP_MVC.Controllers
{
  
    public class __ClausesController : ControllersBase<ModeleClausePage>
    {
        #region Méthode Publique

        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Clauses";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            DisplayBandeau();
            return View(model);
        }


        /*  [ErrorHandler]
          public ActionResult Recherche(string code, string description)
          {
            /*  var ToReturn = new List<ModeleClause>();
              //using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
              using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
              {
                  var voletsBlocsCategoriesClient=client.Channel;
                  //var result = voletsBlocsCategoriesClient.GetClausesLibelle(code, description).ToList();

                  if (result != null && result.Count > 0)
                      result.ForEach(m => ToReturn.Add((ModeleClause)m));
              }

              return PartialView("ListeClauses", ToReturn);
          }*/
        #endregion
    }
}