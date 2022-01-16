using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.AssuresAdditionnels;
using ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.ModelePage;
//using ALBINGIA.OP.OP_MVC.Models.MetaData.SaisieCreation_MetaData;


using OPServiceContract.ITraitementAffNouv;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers.AffaireNouvelle
{
    public class AssuresAdditionnelsController : ControllersBase<AnAssuresAdditionnelsPage>
    {
        #region Méthodes Publiques

        
        public ActionResult Index()
        {
            model.PageTitle = "Assurés";

            return View(model);
        }

        
        [ErrorHandler]
        public ActionResult OpenAssuresAdditionnels(string id, string modeNavig, string readOnlyDisplay)
        {
            id = InitializeParams(id);
            AssuAdditionnels model = LoadInfoAssuAdditionnels(id, modeNavig);
            return PartialView("AssuAdditionnels", model);
        }

        [ErrorHandler]
        public ActionResult OpenAddAssureRef(string codeOffre, string version, string type, string codeAvn, string codeAssu, string modeNavig)
        {
            List<AlbSelectListItem> lstQualites1 = new List<AlbSelectListItem>();
            List<AlbSelectListItem> lstQualites2 = new List<AlbSelectListItem>();
            List<AlbSelectListItem> lstQualites3 = new List<AlbSelectListItem>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.GetListQualite(codeOffre, version, type, codeAvn, modeNavig.ParseCode<ModeConsultation>());

                if (result != null)
                {
                    lstQualites1 = result.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    lstQualites2 = result.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    lstQualites3 = result.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                }

                AssuresRef model = new AssuresRef();
                if (!string.IsNullOrEmpty(codeAssu))
                {
                    var resAssu = serviceContext.GetAssureRef(codeOffre, version, type, codeAssu);
                    if (resAssu != null)
                    {
                        model = ((AssuresRef)resAssu);
                        if (!string.IsNullOrEmpty(model.CodeQualite1))
                        {
                            var sItem = lstQualites1.FirstOrDefault(x => x.Value == model.CodeQualite1);
                            if (sItem != null)
                            {
                                sItem.Selected = true;
                            }
                        }
                        if (!string.IsNullOrEmpty(model.CodeQualite2))
                        {
                            var sItem = lstQualites2.FirstOrDefault(x => x.Value == model.CodeQualite2);
                            if (sItem != null)
                            {
                                sItem.Selected = true;
                            }
                        }
                        if (!string.IsNullOrEmpty(model.CodeQualite3))
                        {
                            var sItem = lstQualites3.FirstOrDefault(x => x.Value == model.CodeQualite3);
                            if (sItem != null)
                            {
                                sItem.Selected = true;
                            }
                        }
                    }
                }

                model.Qualites1 = lstQualites1;
                model.Qualites2 = lstQualites2;
                model.Qualites3 = lstQualites3;

                return PartialView("AddAssureRef", model);
            }
        }

        [ErrorHandler]
        public ActionResult OpenAddAssureNonRef(string codeOffre, string version, string type, string codeAvn, string idAssure,
            string codeOldQualite1, string codeOldQualite2, string codeOldQualite3, string oldQualiteAutre,
            string modeNavig)
        {
            List<AlbSelectListItem> lstQualites1 = new List<AlbSelectListItem>();
            List<AlbSelectListItem> lstQualites2 = new List<AlbSelectListItem>();
            List<AlbSelectListItem> lstQualites3 = new List<AlbSelectListItem>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.GetListQualite(codeOffre, version, type, codeAvn, modeNavig.ParseCode<ModeConsultation>());

                if (result != null)
                {
                    lstQualites1 = result.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    lstQualites2 = result.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    lstQualites3 = result.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                }
                AssuresNonRef model = new AssuresNonRef();
                if (!string.IsNullOrEmpty(idAssure))
                {
                    model.IdAssuNonRef = idAssure;
                    model.ModeEdit = true;
                    model.QualiteAutre = oldQualiteAutre;
                    if (!string.IsNullOrEmpty(codeOldQualite1))
                    {
                        var sItem = lstQualites1.FirstOrDefault(x => x.Value == codeOldQualite1);
                        if (sItem != null)
                        {
                            sItem.Selected = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(codeOldQualite2))
                    {
                        var sItem = lstQualites2.FirstOrDefault(x => x.Value == codeOldQualite2);
                        if (sItem != null)
                        {
                            sItem.Selected = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(codeOldQualite3))
                    {
                        var sItem = lstQualites3.FirstOrDefault(x => x.Value == codeOldQualite3);
                        if (sItem != null)
                        {
                            sItem.Selected = true;
                        }
                    }
                }
                model.Qualites1 = lstQualites1;
                model.Qualites2 = lstQualites2;
                model.Qualites3 = lstQualites3;

                return PartialView("AddAssureNonRef", model);
            }
        }

        [ErrorHandler]
        public string LoadInfoAssure(string codeAssu)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                return serviceContext.LoadInfoAssure(codeAssu);
            }
        }

        [ErrorHandler]
        public ActionResult SaveAssureRef(string codeOffre, string version, string type, string modeEdit, string codeAssu, string codeQual1, string codeQual2,
            string codeQual3, string qualAutre, string modeNavig, string codeAvenant, string idDesi, string designation)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.SaveAssureRef(codeOffre, version, type, codeAvenant, modeEdit, codeAssu, codeQual1, codeQual2, codeQual3, qualAutre, idDesi, designation, modeNavig.ParseCode<ModeConsultation>(), GetUser());

                if (result != null)
                {
                    var model = ((AssuAdditionnels)result);
                    if (model.CodeError == 1)
                        throw new AlbFoncException("Code assuré déjà enregistré.");
                    return PartialView("LstAssuresRef", ((AssuAdditionnels)result));
                }

                return PartialView("LstAssuresRef", null);
            }
        }

        [ErrorHandler]
        public ActionResult DeleteAssureRef(string codeOffre, string version, string type, string codeAssu, string modeNavig, string codeAvenant)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.DeleteAssureRef(codeOffre, version, type, codeAvenant, codeAssu, modeNavig.ParseCode<ModeConsultation>(), GetUser());
                if (result != null)
                    return PartialView("LstAssuresRef", ((AssuAdditionnels)result));

                return PartialView("LstAssuresRef", null);
            }
        }

        [ErrorHandler]
        public ActionResult SaveAssureNonRef(string codeOffre, string version, string type, string tabGuid, string modeEdit, string codeQual1, string codeQual2, string codeQual3, string qualAutre,
            string codeOldQual1, string codeOldQual2, string codeOldQual3, string oldQualAutre, string modeNavig, string codeAvenant)
        {
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvenant)
              && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>())
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaireNouvelle>())
                {
                    var serviceContext=client.Channel;
                    var result = serviceContext.SaveAssureNonRef(codeOffre, version, type, codeAvenant, modeEdit, codeQual1, codeQual2, codeQual3, qualAutre,
                        codeOldQual1, codeOldQual2, codeOldQual3, oldQualAutre, modeNavig.ParseCode<ModeConsultation>(), GetUser());

                    if (result != null)
                        return PartialView("LstAssuresNonRef", ((AssuAdditionnels)result));

                    return PartialView("LstAssuresNonRef", null);
                }
            }
            else
                throw new AlbFoncException("Impossible de sauvegarder en lecture seule ou en mode historique", trace: false, sendMail: false, onlyMessage: true);

        }

        [ErrorHandler]
        public ActionResult DeleteAssureNonRef(string codeOffre, string version, string type,
            string codeOldQual1, string codeOldQual2, string codeOldQual3, string oldQualAutre, string modeNavig, string codeAvenant)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.DeleteAssureNonRef(codeOffre, version, type, codeAvenant, codeOldQual1, codeOldQual2, codeOldQual3, oldQualAutre, modeNavig.ParseCode<ModeConsultation>(), GetUser());
                if (result != null)
                    return PartialView("LstAssuresNonRef", ((AssuAdditionnels)result));

                return PartialView("LstAssuresNonRef", null);
            }
        }

        #endregion

        #region Méthodes Privées

        private AssuAdditionnels LoadInfoAssuAdditionnels(string id, string modeNavig)
        {

            #region vérification des paramètres
            if (string.IsNullOrEmpty(id))
                throw new AlbFoncException("Erreur de chargement de la page:identifiant de la page est null", trace: true, sendMail: true, onlyMessage: true);
            string[] tIds = id.Split('_');
            if (tIds.Length < 3)
                throw new AlbFoncException("Erreur de chargement de la page:identifiant de la page est null", trace: true, sendMail: true, onlyMessage: true);
            string codeContrat = tIds[0];
            string versionContrat = tIds[1];
            string typeContrat = tIds[2];
            string codeAvenant = string.Empty;
            if (tIds.Length > 3)
                codeAvenant = tIds[3];
            int iVersion;
            if (string.IsNullOrEmpty(codeContrat) || string.IsNullOrEmpty(versionContrat) || string.IsNullOrEmpty(typeContrat))
                throw new AlbFoncException("Erreur de chargement de la page:Un des trois paramètres est vide (numéro coffre/Contrat, Version, Type)", trace: true, sendMail: true, onlyMessage: true);
            if (!int.TryParse(versionContrat, out iVersion))
                throw new AlbFoncException("Erreur de chargement de la page:Version non valide", trace: true, sendMail: true, onlyMessage: true);
            #endregion

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaireNouvelle>())
            {
                var serviceContext=client.Channel;

                AssuAdditionnels model = new AssuAdditionnels();
                var result = serviceContext.InitAssuresAdditionnels(codeContrat, versionContrat, typeContrat, codeAvenant, modeNavig.ParseCode<ModeConsultation>());
                if (result != null)
                {
                    model = ((AssuAdditionnels)result);
                }
                if (model.AssuresReference == null || model.AssuresReference.Count == 0)
                {
                    throw new AlbFoncException(string.Format("Les données du contrat <b>{0} - {1}</b> sont erronées : assurés référencés non valide", codeContrat, versionContrat), sendMail: true, trace: true);
                }
                model.IsReadOnly = GetIsReadOnly("tabGuid" + base.model.TabGuid + "tabGuid", codeContrat + "_" + versionContrat + "_" + typeContrat, codeAvenant);
                if (!string.IsNullOrEmpty(codeAvenant) && codeAvenant != "0")
                    model.IsModeAvenant = true;
                return model;
            }
        }

        #endregion

    }
}
