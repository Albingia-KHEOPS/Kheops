using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Models.ModeleActeGestion;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OPServiceContract.ICommon;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class SuiviActesGestionController : ControllersBase<ModeleSuiviActesGestionPage>
    {
        #region Méthodes publiques

        [ErrorHandler]
        public ActionResult Index(string codeOffre, string version, string type)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var commonOffreClient=client.Channel;
                var actesDto = commonOffreClient.GetListActesGestion(codeOffre, version, type, null, null, string.Empty, string.Empty);
                model.ActesGestion = ActeGestion.LoadActesGestion(actesDto);
            }
            return View(model);
        }

        [ErrorHandler]
        public ActionResult OpenSuiviActesGestion(string codeOffre, string version, string type)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var commonOffreClient=client.Channel;
                var actesDto = commonOffreClient.GetListActesGestion(codeOffre, version, type, null, null, string.Empty, string.Empty);
                model.ActesGestion = ActeGestion.LoadActesGestion(actesDto);
                model.Type = type;
                model.CodeOffre = codeOffre;
                model.Version = version;
                var affichages = new List<AlbSelectListItem>();
                affichages.Add(new AlbSelectListItem { Value = "T", Text = "Tout", Descriptif = "Tout", Title = "Tout" });
                affichages.Add(new AlbSelectListItem { Value = "A", Text = "Actes de gestion", Descriptif = "Actes de gestion", Title = "Actes de gestion", Selected = true });
                model.Affichages = affichages;

                var utilisateurs = new List<AlbSelectListItem>();
                utilisateurs = actesDto.Select(x => x.Utilisateur).Distinct().Select(x => new AlbSelectListItem { Text = x, Value = x, Selected = false, Title = x }).OrderBy(x => x.Value).ToList();
                model.Utilisateurs = utilisateurs;

                var typeTraitements = new List<AlbSelectListItem>();
                typeTraitements = actesDto.Select(x => x.CodeTraitement + " - " + x.Libelle).Distinct().Select(x => new AlbSelectListItem { Text = x, Value = x.Split('-')[0].Trim(), Selected = false, Title = x }).OrderBy(x => x.Value).ToList();
                model.TypeTraitements = typeTraitements;
            }
            return PartialView("Index", model);
        }

        public ActionResult Filtrer(string codeOffre, string version, string type, string dateDeb, string dateFin, string user, string traitement)
        {
            var model = new List<ActeGestion>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var serviceContext=client.Channel;
                var actesDto = serviceContext.GetListActesGestion(codeOffre, version, type, AlbConvert.ConvertStrToDate(dateDeb), AlbConvert.ConvertStrToDate(dateFin), user, traitement);
                model = ActeGestion.LoadActesGestion(actesDto);
            }

            return PartialView("ListeActesGestion", model);
        }


        #endregion
    }
}
