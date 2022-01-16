using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAffaireNouvelle;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ISaisieCreationOffre;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class FormVolAffNouvController : ControllersBase<ModeleFormVolAffNouvPage>
    {

        #region Méthodes Publiques
        
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            id = HttpUtility.UrlDecode(id);
            model.PageTitle = "Sélection des Formules et Volets";
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        /// <summary>
        /// Met à jour la formule/option/volet dans la table KPOFOPT
        /// </summary>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="codeForm"></param>
        /// <param name="guidForm"></param>
        /// <param name="codeOpt"></param>
        /// <param name="guidOpt"></param>
        /// <param name="guidVol"></param>
        /// <param name="type"></param>
        /// <param name="isChecked"></param>
        [ErrorHandler]
        public void UpdateFormVol(string codeContrat, string versionContrat, string codeOffre, string version, string typeOffre, string codeForm, string guidForm, string codeOpt,
                string guidOpt, string guidVol, string type, string isChecked)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                serviceContext.UpdateFormVol(codeContrat, versionContrat, codeOffre, version, typeOffre, codeForm, guidForm, codeOpt,
                 guidOpt, guidVol, type, isChecked);
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
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string infoContrat)
        {
            if (cible == "RechercheSaisie")
                return RedirectToAction(job, cible);
            else if (cible == "AnCreationContrat")
                return RedirectToAction(job, cible, new { id = infoContrat });
            else
                return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + "_" + infoContrat });
        }
        
        [ErrorHandler]
        public ActionResult GetListRsqForm(string codeOffre, string version, string type, string codeContrat, string versionContrat, string splitHtmlChar, string acteGestion, string guid, string codeAvn)
        {
            var folder = string.Format("{0}_{1}_{2}", codeContrat, versionContrat, type);
            var isModifHorsAvn = GetIsModifHorsAvn(guid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn));
            
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                ModeleFormVolAffNouvRecap model = new ModeleFormVolAffNouvRecap();
                var result = serviceContext.GetListRsqForm(codeOffre, version, type, codeContrat, versionContrat, GetUser(), splitHtmlChar, acteGestion, isModifHorsAvn);

                if (result != null)
                {
                    model = ((ModeleFormVolAffNouvRecap)result);
                }

                return PartialView("LstRsqFormAffNouv", model);
            }
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
            LoadDataFormVolAffNouv(id);
        }

        /// <summary>
        /// Charge les informations spécifiques à la page
        /// </summary>
        /// <param name="id"></param>
        private void LoadDataFormVolAffNouv(string id)
        {
            ModeleFormVolAffNouvPage model = new ModeleFormVolAffNouvPage();

            string[] tId = id.Split('_');

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.InitFormVolAffNouv(tId[0], tId[1], tId[2], tId[3], tId[4]);

                if (result != null)
                {
                    model = ((ModeleFormVolAffNouvPage)result);

                    base.model.CodeOffre = model.CodeOffre;
                    base.model.Version = model.Version;
                    base.model.Type = model.Type;
                    base.model.CodeContrat = model.CodeContrat;
                    base.model.VersionContrat = model.VersionContrat;
                    base.model.Risques = model.Risques;
                }
            }
        }


        #endregion

    }
}
