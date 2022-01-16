using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.IOFile;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesLogTrace;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OPServiceContract.IAdministration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class LogRecupParamCibleController : ControllersBase<ModeleLogRecupParamCiblePage>
    {
        #region Méthodes publiques

        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index(LogParamCibleRecupModel filtre)
        {
            model.PageTitle = "Log . Récupération cible";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            DisplayBandeau();
            model.Filtre = filtre;
            SetTypesListModel();

            if (this.Request.IsAjaxRequest())
            {
                return PartialView("Body",model);
            }

            return View(model);
        }

        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult List(LogParamCibleRecupModel filtre)
        {
            SetLogQueryResult(filtre);
            return PartialView("List", model.ListLog);
        }

        private void SetTypesListModel()
        {
            model.Filtre.Types = new List<AlbSelectListItem> {
                new AlbSelectListItem {Value="O",Text="O - Offre" }, 
                new AlbSelectListItem {Value="P",Text="P - Contrat"} };
        }

        private void SetLogQueryResult(LogParamCibleRecupModel filtre)
        {
            // GET STAT
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                model.ListLog = serviceContext.GetLogParamCibleRecup(filtre.ToDto())
                    .Select(x => (LogParamCibleRecupModel)x).ToList();
            }
        }      

        [ErrorHandler]
        [AlbApplyUserRole]
        public ExportToCSVResult<LogParamCibleRecupModel> ExportFile(string typeOffrePolice, string numOffrePolice, int? numAliment,
                                                                 int? numAvenant, int? numHisto, int? idRisque, int? idObjet)
        {

            string fileName = "LogRecupCible.csv";
            string columns = "NumAliment;NumAvenant;CodeSituation;DateSituation;NumHisto;NumOffrePolice;NumTravail;IdObjet;OptionRecuperation;IdRisque;CommentaireSituation;TypeOffrePolice;UserSituation";

            LogParamCibleRecupModel filtre = new LogParamCibleRecupModel
            {
                TypeOffrePolice = typeOffrePolice,
                NumOffrePolice = numOffrePolice,
                NumAliment = numAliment,
                NumAvenant = numAvenant,
                NumHisto = numHisto,
                IdRisque = idRisque,
                IdObjet = idObjet
            };

            SetLogQueryResult(filtre);
            var ret = new ExportToCSVResult<LogParamCibleRecupModel>(model.ListLog, fileName, columns);
            return ret;
        }

        #endregion
    }
}
