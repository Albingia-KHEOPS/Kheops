using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamDocumentChemin;
using OPServiceContract.ITraitementAffNouv;
using System.Collections.Generic;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ParamDocumentCheminController : ControllersBase<ModeleParamDocumentCheminPage>
    {

        #region Membres privés

        private static List<AlbSelectListItem> _lstTypesChemin;
        private static List<AlbSelectListItem> _lstTypologiesChemin;

        #endregion

        #region Méthodes publiques
        
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Paramétrages des chemins";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            LoadInfoPage();
            DisplayBandeau();
            return View(model);
        }

        [ErrorHandler]
        public ActionResult GetDetailsChemin(string identifiant)
        {
            ModeleLigneDocumentChemin toReturn = null;
            if (!string.IsNullOrEmpty(identifiant))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaireNouvelle>())
                {
                    var serviceContext=client.Channel;
                    var result = serviceContext.GetLigneDocumentChemin(identifiant);
                    if (result != null)
                        toReturn = (ModeleLigneDocumentChemin)result;
                }
            }
            if (toReturn == null)
                toReturn = new ModeleLigneDocumentChemin();
            toReturn.ListeTypes = GetListeTypesChemin();
            toReturn.ListeTypologies = GetListeTypologiesChemin();
            return PartialView("DetailsParamChemin", toReturn);
        }

        [ErrorHandler]
        public ActionResult GetTypologies(string typeChemin)
        {
            ModeleTypologies toReturn = new ModeleTypologies();
            if (!string.IsNullOrEmpty(typeChemin) && typeChemin == "D")
                toReturn.ListeTypologies = GetListeTypologiesChemin();
            return PartialView("Typologie", toReturn);
        }

        [ErrorHandler]
        public ActionResult EnregistrerChemin(string identifiantChemin, string typeChemin, string typologie, string libelleChemin, string chemin, string serveur, string racine, string environnement)
        {
            List<ModeleLigneDocumentChemin> toReturn = new List<ModeleLigneDocumentChemin>();
            if (string.IsNullOrEmpty(typologie) && string.IsNullOrEmpty(identifiantChemin))
                throw new AlbFoncException("L'identifiant ne peut être vide", trace: false, sendMail: false, onlyMessage: true);
            //Construction de la clé de chemin et vérification de son unicité
            if (string.IsNullOrEmpty(identifiantChemin))
            {
                //Mode création
                if (!string.IsNullOrEmpty(typeChemin) && typeChemin != "D")
                    identifiantChemin = typologie;
                else
                    identifiantChemin = string.Concat("DOC_", typologie);
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaireNouvelle>())
                {
                    var serviceContext=client.Channel;
                    var result = serviceContext.GetLigneDocumentChemin(identifiantChemin);
                    if (result == null)
                    {
                        var resultLst = serviceContext.AddLigneDocumentChemin(identifiantChemin, libelleChemin, typeChemin, chemin, serveur, racine, environnement, GetUser());
                        if (resultLst != null)
                            resultLst.ForEach(elm => toReturn.Add((ModeleLigneDocumentChemin)elm));
                    }
                    else
                        throw new AlbFoncException("L'identifiant de chemin existe déjà", trace: false, sendMail: false, onlyMessage: true);
                }
            }
            else
            { //Mode mise à jour
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaireNouvelle>())
                {
                    var serviceContext=client.Channel;
                    var result = serviceContext.GetLigneDocumentChemin(identifiantChemin);
                    if (result != null)
                    {
                        var resultLst = serviceContext.UpdateLigneDocumentChemin(identifiantChemin, libelleChemin, chemin, serveur, racine, GetUser());
                        if (resultLst != null)
                            resultLst.ForEach(elm => toReturn.Add((ModeleLigneDocumentChemin)elm));
                    }
                    else
                        throw new AlbFoncException("L'identifiant de chemin est introuvable", trace: false, sendMail: false, onlyMessage: true);

                }
            }
            return PartialView("ListeChemins", toReturn);
        }

        [ErrorHandler]
        public ActionResult SupprimerChemin(string identifiant)
        {
            List<ModeleLigneDocumentChemin> toReturn = new List<ModeleLigneDocumentChemin>();
            if (string.IsNullOrEmpty(identifiant))
                throw new AlbFoncException("L'identifiant est incorrect", trace: false, sendMail: false, onlyMessage: true);

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.GetLigneDocumentChemin(identifiant);
                if (result != null)
                {
                    var resultLst = serviceContext.DeleteLigneDocumentChemin(identifiant);
                    if (resultLst != null)
                        resultLst.ForEach(elm => toReturn.Add((ModeleLigneDocumentChemin)elm));
                }
                else
                    throw new AlbFoncException("L'identifiant de chemin est introuvable", trace: false, sendMail: false, onlyMessage: true);
            }
            return PartialView("ListeChemins", toReturn);
        }

        #endregion

        #region Méthodes privées

        protected override void LoadInfoPage(string context = null)
        {
            //Chargement des chemins
            model.ListeChemins = GetListeChemins();
        }

        private List<ModeleLigneDocumentChemin> GetListeChemins()
        {
            List<ModeleLigneDocumentChemin> toReturn = new List<ModeleLigneDocumentChemin>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.GetListeLignesDocumentChemin();
                if (result != null)
                    result.ForEach(elm => toReturn.Add((ModeleLigneDocumentChemin)elm));
            }
            return toReturn;
        }

        private List<AlbSelectListItem> GetListeTypesChemin()
        {

            var listeType = new List<AlbSelectListItem>();
            if (_lstTypesChemin != null)
            {

                _lstTypesChemin.ForEach(elm => listeType.Add(new AlbSelectListItem
                {
                    Value = elm.Value,
                    Text = elm.Text,
                    Title = elm.Title,
                    Selected = false
                }));
                return listeType;
            }

            //Sinon on charge les données à partir de la BDD et on renvoit une nouvelle instance de la liste

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var lstTyp = serviceContext.GetListeTypesChemin();
                lstTyp.ForEach(elm => listeType.Add(new AlbSelectListItem
                {
                    Value = elm.Code,
                    Text = elm.Code + " - " + elm.Libelle,
                    Title = elm.Code + " - " + elm.Libelle,
                    Selected = false
                }
                ));
            }
            _lstTypesChemin = listeType;
            return listeType;
        }

        private List<AlbSelectListItem> GetListeTypologiesChemin()
        {

            var listeTypo = new List<AlbSelectListItem>();
            if (_lstTypologiesChemin != null)
            {

                _lstTypologiesChemin.ForEach(elm => listeTypo.Add(new AlbSelectListItem
                {
                    Value = elm.Value,
                    Text = elm.Text,
                    Title = elm.Title,
                    Selected = false
                }));
                return listeTypo;
            }

            //Sinon on charge les données à partir de la BDD et on renvoit une nouvelle instance de la liste

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var lstTyp = serviceContext.GetListeTypologiesChemin();
                lstTyp.ForEach(elm => listeTypo.Add(new AlbSelectListItem
                {
                    Value = elm.Code,
                    Text = elm.Code + " - " + elm.Libelle,
                    Title = elm.Code + " - " + elm.Libelle,
                    Selected = false
                }
                ));
            }
            _lstTypologiesChemin = listeTypo;
            return listeTypo;
        }

        #endregion

    }
}
