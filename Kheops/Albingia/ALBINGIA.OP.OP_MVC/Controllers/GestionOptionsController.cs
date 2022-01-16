using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OPServiceContract.ISaisieCreationOffre;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class GestionOptionsController : ControllersBase<ModeleGestionOptionsPage>
    {
        #region Méthodes Publiques

        [ErrorHandler]
        public ActionResult Index(string id)
        {
            id = InitializeParams(id);
            #region vérification des paramètres
            if (string.IsNullOrEmpty(id))
                throw new AlbFoncException("Erreur de chargement de la page:identifiant de la page est null", trace: true, sendMail: true, onlyMessage: true);
            string[] tIds = id.Split('_');
            if (tIds.Length != 3)
                throw new AlbFoncException("Erreur de chargement de la page:identifiant de la page est null", trace: true, sendMail: true, onlyMessage: true);
            string numeroOffre = tIds[0];
            string version = tIds[1];
            string type = tIds[2];
            int iVersion;
            if (string.IsNullOrEmpty(numeroOffre) || string.IsNullOrEmpty(version) || string.IsNullOrEmpty(type))
                throw new AlbFoncException("Erreur de chargement de la page:Un des trois paramètres est vide (numéro Offre/Contrat, Version, Type)", trace: true, sendMail: true, onlyMessage: true);
            if (!int.TryParse(version, out iVersion))
                throw new AlbFoncException("Erreur de chargement de la page:Version non valide", trace: true, sendMail: true, onlyMessage: true);
            #endregion

            model.PageTitle = "Rédaction offre";

            LoadInfoPage(numeroOffre, version, type);

            #region Chargement du bandeau et de la navigation
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
                    Etape = Navigation_MetaModel.ECRAN_RISQUEETGARANTIE,
                    IdOffre = model.Offre.CodeOffre,
                    Version = model.Offre.Version
                };
            }

            #endregion

            return View(model);

        }

        [AlbAjaxRedirect]
        public ActionResult Redirection(string cible, string job, string codeOffre, string version, string type)
        {
            return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type });
        }

        #endregion

        #region Méthodes Privées

        private void LoadInfoPage(string numeroOffre, string version, string type)
        {

            #region Chargement de l'offre
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var policeServicesClient=client.Channel;
                model.Offre = new Offre_MetaModel();
                model.Offre.LoadOffre(policeServicesClient.OffreGetDto(numeroOffre, int.Parse(version), type, model.ModeNavig.ParseCode<ModeConsultation>()));
                //model.Offre.LoadOffre(policeServicesClient.OffreGetDto(numeroOffre, int.Parse(version), type, model.ModeNavig));
            }
            #endregion

            #region Chargement des options
            model.Options = new List<AlbSelectListItem>();

            AlbSelectListItem item = new AlbSelectListItem();
            item.Text = "Option";
            item.Value = "Option";
            item.Selected = true;
            model.Options.Add(item);

            item = new AlbSelectListItem();
            item.Text = "Option2";
            item.Value = "Option2";
            model.Options.Add(item);

            item = new AlbSelectListItem();
            item.Text = "Option3";
            item.Value = "Option3";
            model.Options.Add(item);
            #endregion

            #region Chargement des Informations Spécifiques
            model.InformationsSpecifiques = new List<AlbSelectListItem>();
            item = new AlbSelectListItem();
            item.Text = "(demo) Identification de l'assuré";
            item.Value = "(demo) Identification de l'assuré";
            model.InformationsSpecifiques.Add(item);

            item = new AlbSelectListItem();
            item.Text = "(demo) Clause de bonification";
            item.Value = "(demo) Clause de bonification";
            model.InformationsSpecifiques.Add(item);

            item = new AlbSelectListItem();
            item.Text = "(demo) Clause de renonciation à recours";
            item.Value = "(demo) Clause de renonciation à recours";
            model.InformationsSpecifiques.Add(item);

            item = new AlbSelectListItem();
            item.Text = "(demo) Délais de préavis";
            item.Value = "(demo) Délais de préavis";
            model.InformationsSpecifiques.Add(item);

            #endregion
        }

        #endregion
    }
}
