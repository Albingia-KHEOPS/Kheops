using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.IOFile;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModeleStatisque;
using OP.WSAS400.DTO.Stat;
using OPServiceContract.IAdministration;
using System.Collections.Generic;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class StatClausesLibresController : ControllersBase<ModeleStatistiqueClausesLibresPage>
    {
        #region Méthodes Publiques
        [ErrorHandler]
        public ActionResult Index()
        {
            model.PageTitle = "Statistique Clauses Libres";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            return View(model);
        }
        [ErrorHandler]
        public ActionResult Rechercher(string datedebutsaisie, string datefinsaisie, string datedebutcreation, string datefincreation, int pageNumber)
        {
            ResultStatClausesLibreModel toReturn = new ResultStatClausesLibreModel();

            ParamRecherClausLibDto paramRecherche = new ParamRecherClausLibDto
            {
                DateSaisieDebut = datedebutsaisie,
                DateSaisieFin = datefinsaisie,
                DateCreationDebut = datedebutcreation,
                DateCreationFin = datefincreation,
                PageNumber = pageNumber,
            };
            paramRecherche.StartLine = ((paramRecherche.PageNumber - 1) * 30) + 1;
            paramRecherche.EndLine = (paramRecherche.PageNumber) * 30;
            paramRecherche.LineCount = 300;

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.GetStatClausesLibres(paramRecherche);
                toReturn.ListResult = new List<StatClausesLibreModel>();
                if (result != null)
                {
                    result.ForEach(ligne => toReturn.ListResult.Add((StatClausesLibreModel)ligne));
                }
                int totalLines = 300;
                toReturn.NbCount = 300;
                toReturn.StartLigne = paramRecherche.StartLine;
                toReturn.EndLigne = paramRecherche.EndLine;
                toReturn.PageNumber = paramRecherche.PageNumber;
                toReturn.LineCount = totalLines;
            }
            return PartialView("BodyResultatsRecherche", toReturn);
        }


        [ErrorHandler]
        [AlbApplyUserRole]
        public ExportToCSVResult<StatClausesLibreModel> ExportFile(string Datesaisdebut, string Datesaisfin, string Datedebcreate,
                                                                 string Datefincreate)
        {
            //version à la suite numero de police
            string fileName = "ResultatRecherche.csv";
            string columns = "Délégation utilisateur;Utilisateur Creat;Date de création ; Date de saisie ;Numéro de police ; Version ;Code courtier ; Nom courtier; Délégation courtier ;Code preneurass;Nom preneurass ; Nom du souscripteur";
            ResultStatClausesLibreModel toReturn = new ResultStatClausesLibreModel();
            ParamRecherClausLibDto paramRecherche = new ParamRecherClausLibDto
            {
                DateSaisieDebut = Datesaisdebut,
                DateSaisieFin = Datesaisfin,
                DateCreationDebut = Datedebcreate,
                DateCreationFin = Datefincreate,

            };
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.GetStatClausesLibres(paramRecherche);
                toReturn.ListResult = new List<StatClausesLibreModel>();
                if (result != null)
                {
                    result.ForEach(ligne => toReturn.ListResult.Add((StatClausesLibreModel)ligne));
                }

            }
            var ret = new ExportToCSVResult<StatClausesLibreModel>(toReturn.ListResult, fileName, columns);
            return ret;
        }

        #endregion
    }




}
