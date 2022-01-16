using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Branches;
using OPServiceContract.IAdministration;
using OPServiceContract.IAffaireNouvelle;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ParamCibleRecupController : ControllersBase<ModeleParamCibleRecupPage>
    {
        #region Méthodes publiques

        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Paramètre cibles de Recupération";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            DisplayBandeau();
            return View(model);
        }

        /// <summary>
        /// Retourner une vue partiel pour effectuer la recuperation
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult RecupOffre(string codeOffre, string version)
        {
            model.CodeOffre = codeOffre;
            model.Version = int.Parse(version).ToString();
            model.CodeCibleRecup = string.Empty;

            //lancer la recherche e la mise a jour
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceAffaire=channelClient.Channel;
                OffreRecupDto offre = serviceAffaire.RecupOffre(codeOffre.Trim(), version);
                if (offre != null)
                {
                    using (var channelClientVol = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                    {
                        var serviceContext=channelClientVol.Channel;
                        //preparer la liste des cibles disponibles de la branche
                        List<CibleDto> cibles;
                        cibles = serviceContext.CiblesGet(offre.Branche);
                        if (cibles != null && cibles.Any())
                        {
                            model.Cibles = cibles.Where(el => !el.Code.StartsWith("RECUP") && !el.Code.Equals(offre.Cible)).
                            Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Description, Selected = false }).ToList();
                        }
                    }

                    model.CodeCibleRecup = offre.Cible;
                    model.CodeCibleRecupLabel = string.Format("{0} - {1}", offre.Cible, offre.CibleLabel);
                    model.Erreur = offre.Erreur;
                    model.Type = offre.Type;
                    model.MultiObj = offre.MultiObj;
                }
            }
            return PartialView("ParamCibleRecupResult", model);
        }

        /// <summary>
        /// submit la migration de offre/contrat vers cible 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [ErrorHandler]
        [AlbApplyUserRole]
        public string MigrationCible(string codeOffre, string version, string cible, string cibleRecup, string type)
        {
            //lancer la migration 
            string message = "Echec du transfert.";
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceAffaire=channelClient.Channel;
                if (serviceAffaire.MigrationOffre(codeOffre, version, type, cibleRecup, cible))
                {
                    message = "Le transfert a été effectué avec succès.";
                }
            }
            return message;
        }

        #endregion
    }
}
