using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamConcepts;
using OP.WSAS400.DTO.Offres.Parametres;
using OPServiceContract.IAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    [ErrorHandler]
    public class ParamConceptsController : ControllersBase<ModeleParamConceptsPage>
    {
        #region Membres privés
        public static readonly string MODE_EDITION = "Update";
        public static readonly string MODE_CREATION = "Insert";
        #endregion

        #region Méthodes publiques
        
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index() {
            model.PageTitle = "Paramétrages des concepts";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            model.AdditionalParam = "**";
            DisplayBandeau();
            return View(model);
        }

        [ErrorHandler]
        public ActionResult RechercheConcepts(string codeConcept, string descriptionConcept, string userRights)
        {
            var toReturn = new List<ModeleLigneConcept>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.LoadListConcepts(codeConcept, descriptionConcept, false, true);
                if (result.Any())
                    result.ForEach(m => toReturn.Add((ModeleLigneConcept)m));
                toReturn.ForEach(elm => elm.AdditionalParam = userRights);
            }
            return PartialView("ListeConcepts", toReturn);
        }

        [ErrorHandler]
        public ActionResult GetDetailsConcept(string codeConcept, string userRights)
        {
            var toReturn = new ModeleLigneConcept();
            if (!string.IsNullOrEmpty(codeConcept))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    var result = serviceContext.LoadListConcepts(codeConcept, string.Empty, false, true);
                    if (result.Any())
                    {
                        toReturn = (ModeleLigneConcept)result.FirstOrDefault();
                        toReturn.ModeSaisie = MODE_EDITION;
                        toReturn.AdditionalParam = userRights;
                    }
                }
            }
            else
            {
                toReturn.ModeSaisie = MODE_CREATION;
                toReturn.AdditionalParam = userRights;
            }
            return PartialView("DetailsConcept", toReturn);
        }

        [ErrorHandler]
        public ActionResult EnregistrerConcept(string mode, string codeConcept, string descriptionConcept, string userRights)
        {
            if (!string.IsNullOrEmpty(mode) && (!string.IsNullOrEmpty(codeConcept) && (!string.IsNullOrEmpty(descriptionConcept))))
            {
                var toReturn = new List<ModeleLigneConcept>();
                //Enregistrement BDD
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    if (mode == MODE_CREATION)
                    {
                        if (!serviceContext.LoadListConcepts(codeConcept, string.Empty, false, true).Any())
                        {
                            var param = new ParametreDto { Code = codeConcept.Replace(" ", "").Replace("'", "''"), Libelle = descriptionConcept.Replace("'", "''") };
                            var result = serviceContext.EnregistrerConcept(mode, param);
                            if (result.Any())
                                result.ForEach(m => toReturn.Add((ModeleLigneConcept)m));
                            toReturn.ForEach(elm => elm.AdditionalParam = userRights);
                            return PartialView("ListeConcepts", toReturn);
                        }
                        else
                            throw new AlbFoncException("Ce code est déjà utilisé", trace: false, sendMail: false, onlyMessage: true);
                    }
                    else if (mode == MODE_EDITION)
                    {
                        if (serviceContext.LoadListConcepts(codeConcept, string.Empty, false, true).Any())
                        {
                            var param = new ParametreDto { Code = codeConcept.Replace(" ", "").Replace("'", "''"), Libelle = descriptionConcept.Replace("'", "''") };
                            var result = serviceContext.EnregistrerConcept(mode, param);
                            if (result.Any())
                                result.ForEach(m => toReturn.Add((ModeleLigneConcept)m));
                            toReturn.ForEach(elm => elm.AdditionalParam = userRights);
                            return PartialView("LigneConcept", toReturn.FirstOrDefault());
                        }
                        else
                            throw new AlbFoncException("Le concept que vous voulez modifier n'existe plus", trace: false, sendMail: false, onlyMessage: true);
                    }
                }
            }
            throw new AlbFoncException("Code et description ne peuvent être vides", trace: false, sendMail: false, onlyMessage: true);
        }

        [ErrorHandler]
        public ActionResult SupprimerConcept(string codeConcept, string userRights)
        {
            if (!string.IsNullOrEmpty(codeConcept))
            {
                var toReturn = new List<ModeleLigneConcept>();
                var param = new ParametreDto { Code = codeConcept };
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    string error = serviceContext.SupprimerConcept(param);
                    if (!string.IsNullOrEmpty(error))
                        throw new AlbFoncException(error, trace: true, sendMail: true, onlyMessage: true);

                    //Rechargement des concepts
                    var result = serviceContext.LoadListConcepts(string.Empty, string.Empty, false, true);
                    if (result.Any())
                        result.ForEach(m => toReturn.Add((ModeleLigneConcept)m));
                    toReturn.ForEach(elm => elm.AdditionalParam = userRights);
                }
                return PartialView("ListeConcepts", toReturn);
            }
            throw new AlbFoncException("Impossible de supprimer un concept sans code", trace: true, sendMail: true, onlyMessage: true);
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeConcept, string descriptionConcept, string userRights)
        {
            if (!string.IsNullOrEmpty(cible) && !string.IsNullOrEmpty(job) && !string.IsNullOrEmpty(codeConcept) && !string.IsNullOrEmpty(descriptionConcept))
                return RedirectToAction(job, cible, new { id = codeConcept + "_" + descriptionConcept + "_" + userRights });
            throw new AlbTechException(new Exception("Redirection impossible, paramètre(s) manquant(s)"));
        }

        #endregion

        #region Méthodes privées


        #endregion

    }
}
