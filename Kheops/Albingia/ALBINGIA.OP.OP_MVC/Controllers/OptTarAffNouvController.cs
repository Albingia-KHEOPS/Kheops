using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class OptTarAffNouvController : ControllersBase<ModeleOptTarAffNouvPage>
    {
        #region Méthodes Publiques
        
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            model.PageTitle = "Sélection des options tarifaires";
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        /// <summary>
        /// Met à jour le tarif dans la table KPOFTAR
        /// </summary>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <param name="guidTarif"></param>
        [ErrorHandler]
        public void UpdateOptTarif(string codeContrat, string versionContrat, string guidTarif)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                serviceContext.UpdateOptTarif(codeContrat, versionContrat, guidTarif);
            }
        }

        /// <summary>
        /// Validation du contrat créé
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        [ErrorHandler]
        public void ValidContrat(string codeOffre, string version, string type, string codeContrat, string versionContrat, string splitHtmlChar, string acteGestion, string guid, string codeAvn)
        {
            var folder = string.Format("{0}_{1}_{2}", codeContrat, versionContrat, type);
            var isModifHorsAvn = GetIsModifHorsAvn(guid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn));
            
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                serviceContext.ValidContrat(codeOffre, version, type, codeContrat, versionContrat, GetUser(), splitHtmlChar, isModifHorsAvn, acteGestion: acteGestion);
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
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string infoContrat, string addParamType, string addParamValue)
        {
            if (cible == "RechercheSaisie") {
                return RedirectToAction(job, cible);
            }
            else if (cible == "AnCreationContrat") {
                return RedirectToAction(job, cible, new { id = infoContrat + BuildAddParamString(addParamType, addParamValue) });
            }
            else {
                return RedirectToAction(job, cible, new { id = AlbParameters.BuildFullId(new Folder(new[] { codeOffre, version, type }), new[] { infoContrat }, null, addParamValue, null) });
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
                    Etape = Navigation_MetaModel.ECRAN_OPTTARAFFNOUV,
                    IdOffre = model.Offre.CodeOffre,
                    Version = model.Offre.Version
                };
            }
            LoadDataOptTarAffNouv(id);
        }

        private void LoadDataOptTarAffNouv(string id)
        {
            ModeleOptTarAffNouvPage model = new ModeleOptTarAffNouvPage();

            string[] tId = id.Split('_');

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.InitOptTarifAffNouv(tId[3], tId[4]);

                if (result != null)
                {
                    model = ((ModeleOptTarAffNouvPage)result);

                    base.model.CodeOffre = tId[0];
                    base.model.Version = Convert.ToInt64(tId[1]);
                    base.model.CodeContrat = model.CodeContrat;
                    base.model.VersionContrat = model.VersionContrat;
                    base.model.Garanties = model.Garanties;
                }
            }
        }

        #endregion

    }
}
