using Albingia.Common;
using Albingia.Kheops.Mvc.Models;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Mvc.Common;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.ServiceFactory;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Helpers;
using OPServiceContract.IAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers {
    public abstract class BaseController : Controller {
        internal static string GetSurroundedTabGuid(string g) {
            if (g?.StartsWith(PageParamContext.TabGuidKey) == true) {
                return g;
            }
            return $"{PageParamContext.TabGuidKey}{g ?? string.Empty}{PageParamContext.TabGuidKey}";
        }

        /// <summary>
        /// Retourne l'état actuel du readonly ou non
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="currentFolder"></param>
        /// <returns></returns>
        protected virtual bool GetIsReadOnly(string guid, string currentFolder, string numAvenant = "0", bool isPopup = false, string modeAvenant = "") {
            //if (currentFolder.IsEmptyOrNull()) {
            //    //warning:
            //    throw new ArgumentNullException(nameof(currentFolder));
            //}
            //guid = GetSurroundedTabGuid(guid);
            //var numAvt = string.IsNullOrEmpty(numAvenant) ? "0" : numAvenant;

            ////Déverrouillage de l'offre/contrat si la session doit être redémarrer
            //if (System.Web.HttpContext.Current.Session.IsNewSession) {
            //    using (var channelClient = ServiceClientFactory.GetClient<IVoletsBlocsCategories>()) {
            //        string[] array = currentFolder.Split('_');
            //        channelClient.Channel.SupprimerOffreVerouillee(
            //            array[0], array[1], array[2], numAvt,
            //            AlbSessionHelper.ConnectedUser, string.Empty, false, AlbTransverse.GetIsModifHorsAvn(guid, string.Format("{0}_{1}", currentFolder, numAvt)), false);
            //    }
            //    return true;
            //}

            //return AlbTransverse.GetIsReadOnly(guid, currentFolder + "_" + numAvt);
            return true;
        }

        internal static ControllerState GetState(Guid guid, AffaireId affaireId) {
            if (affaireId is null || guid == default) {
                return default;
            }

            var acces = MvcApplication.ListeAccesAffaires.FirstOrDefault(x =>
                x.TabGuid == guid
                && x.Code?.ToIPB() == affaireId.CodeAffaire?.ToIPB()
                && x.Version == affaireId.NumeroAliment);

            if (acces is null || !acces.VerrouillageEffectue || acces.ModeAcces == AccesOrigine.Consulter) {
                return ControllerState.Readonly;
            }
            if (acces != null && acces.ModeAcces == AccesOrigine.ModifierHorsAvenant) {
                return ControllerState.PartialEdit;
            }
            return ControllerState.FullEdit;
        }

        protected virtual ViewResult AutoReadOnlyView(string viewName, string masterName, object model, bool isReadOnly, bool isModifHorsAvenant = false) {
            if (model != null) {
                base.ViewData.Model = model;
            }
            var viewResult = new AutoReadOnlyViewResult(isReadOnly, isModifHorsAvenant);
            viewResult.ViewName = viewName;
            viewResult.MasterName = masterName;
            viewResult.ViewData = base.ViewData;
            viewResult.TempData = base.TempData;
            viewResult.ViewEngineCollection = ViewEngineCollection;
            return viewResult;
        }

    }
}
