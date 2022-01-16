using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ISaisieCreationOffre;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class RsqObjAffNouvController : ControllersBase<ModeleRsqObjAffNouvPage>
    {

        #region Méthodes Publiques
        
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            model.PageTitle = "Sélection des Risques et Formules associés";
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        /// <summary>
        /// Met à jour le risque/objet dans la table KPOFRSQ
        /// </summary>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <param name="type"></param>
        /// <param name="codeRsq"></param>
        /// <param name="codeObj"></param>
        /// <param name="isChecked"></param>
        [ErrorHandler]
        public void UpdateRsqObj(string codeContrat, string versionContrat, string type, string codeRsq, string codeObj, string isChecked)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                serviceContext.UpdateRsqObj(codeContrat, versionContrat, type, codeRsq, codeObj, isChecked);
            }
        }

        /// <summary>
        /// Redirection
        /// </summary>
        /// <param name="cible"></param>
        /// <param name="job"></param>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string infoContrat, string tabGuid,string addParamType, string addParamValue)
        {
            return RedirectToAction(job, cible, new { id = AlbParameters.BuildFullId(
                new Folder(new[] { codeOffre, version, type }),
                cible == "CreationAffaireNouvelle" ? null : new[] { infoContrat },
                tabGuid,
                addParamValue,
                null)
            });
        }

        #endregion

        #region Méthodes Privées

        /// <summary>
        /// Charge les infos génériques de la page
        /// </summary>
        /// <param name="id"></param>
        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var policeServicesClient=client.Channel;
                model.Offre = new Offre_MetaModel();
                model.Offre.LoadOffre(policeServicesClient.OffreGetDto(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig.ParseCode<ModeConsultation>()));
                //model.Offre.LoadOffre(policeServicesClient.OffreGetDto(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig));
            }

            model.Bandeau = null;
            if (model.Offre != null)
            {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;

                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre(string.Empty);
            }

            if (model.AfficherBandeau)
            {
                model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                //Gestion des Etapes
                model.Navigation = new Navigation_MetaModel
                {
                    Etape = Navigation_MetaModel.ECRAN_FORMVOLAFFNOUV,
                    IdOffre = model.Offre.CodeOffre,
                    Version = model.Offre.Version
                };
            }
            LoadDataRsqObjAffNouv(id);
        }

        /// <summary>
        /// Charge les informations spécifiques à la page
        /// </summary>
        /// <param name="id"></param>
        private void LoadDataRsqObjAffNouv(string id)
        {
            var tId = id.Split('_');

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.InitRsqObjAffNouv(tId[0], tId[1], tId[2], tId[3], tId[4]);

                if (result == null) return;
                var model = ((ModeleRsqObjAffNouvPage)result);

                base.model.CodeOffre = model.CodeOffre;
                base.model.Version = model.Version;
                base.model.Type = model.Type;
                base.model.CodeContrat = model.CodeContrat;
                base.model.VersionContrat = model.VersionContrat;
                base.model.ListRsqObj = model.ListRsqObj;
            }
        }

        #endregion

    }
}
