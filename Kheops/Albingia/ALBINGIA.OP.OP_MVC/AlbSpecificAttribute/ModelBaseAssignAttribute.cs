using Albingia.Common;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Controllers;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Albingia.Kheops.Mvc {
    public class ModelBaseAssignAttribute : ActionFilterAttribute {
        readonly static IDictionary<string, string[]> ModelProperties = new Dictionary<string, string[]> {
            { nameof(MetaModelsBase.CodePolicePage), new[] { "codeOffre", "codeContrat", "numeroContrat", "numeroOffre", "codeAffaire", "codeOffreContrat", "argCodeOffre", "codeDossier", "numAffaire" } },
            { nameof(MetaModelsBase.TabGuid), new [] { "tabGuid" } },
            { nameof(MetaModelsBase.VersionPolicePage), new [] { "version", "versionContrat", "versionOffreContrat", "argVersion", "versionDossier" } },
            { nameof(MetaModelsBase.TypePolicePage), new [] { "type", "typeContrat", "typeOffre", "typeOffreContrat", "argType", "typeDossier" } },
            { nameof(MetaModelsBase.ModeNavig), new [] { "modeNavig" } },
            { nameof(MetaModelsBase.NumAvenantPage), new [] { "codeAvn", "codeAvt", "codeAvenant", "numeroAvenant", "numAvenant", "avenant" } },
            { nameof(MetaModelsBase.AddParamValue), new [] { "addParamValue" } }
        };
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (filterContext.HttpContext.Request.IsAjaxRequest() && filterContext.Controller is IMetaModelsController controller) {

                string action = filterContext.ActionDescriptor.ActionName;
                if (controller.IsBackOfficeContext
                    || controller is ContextMenuController
                    || controller is ConditionsGarantieController
                        && action.IsIn(nameof(ConditionsGarantieController.OpenReferentiel), nameof(ConditionsGarantieController.SearchExprReferentiel))) {

                    // Ignore Back Office controllers
                    // or case when actions or controllers use same parameters names for another usage
                    base.OnActionExecuting(filterContext);
                    return;

                }

                filterContext.DecodeUrlParams();
                if (!IsModelParameter(filterContext, controller)) {
                    object value = null;
                    int integer;

                    if (filterContext.ActionParameters.Any(p => p.Value is AffaireId)) {
                        var id = filterContext.ActionParameters.First(p => p.Value is AffaireId).Value as AffaireId;
                        controller.Model.CodePolicePage = id.CodeAffaire.Trim();
                        controller.Model.VersionPolicePage = id.NumeroAliment.ToString();
                        controller.Model.TypePolicePage = id.TypeAffaire.AsCode();
                        controller.Model.NumAvenantPage = (id.NumeroAvenant ?? 0).ToString();
                        controller.Model.ModeNavig = id.IsHisto ? ModeConsultation.Historique.AsCode() : string.Empty;
                    }
                    else {
                        if (ModelProperties[nameof(controller.Model.CodePolicePage)].FirstOrDefault(x => filterContext.ActionParameters.TryGetValue(x, out value)) != default) {
                            controller.Model.CodePolicePage = value as string;
                        }
                        if (ModelProperties[nameof(controller.Model.VersionPolicePage)].FirstOrDefault(x => filterContext.ActionParameters.TryGetValue(x, out value)) != default) {
                            if (value.TryChangeType(out integer)) {
                                controller.Model.VersionPolicePage = integer.ToString();
                            }
                            else {
                                controller.Model.VersionPolicePage = value as string;
                            }
                        }
                        if (ModelProperties[nameof(controller.Model.TypePolicePage)].FirstOrDefault(x => filterContext.ActionParameters.TryGetValue(x, out value)) != default) {
                            controller.Model.TypePolicePage = value as string;
                        }
                        if (ModelProperties[nameof(controller.Model.NumAvenantPage)].FirstOrDefault(x => filterContext.ActionParameters.TryGetValue(x, out value)) != default) {
                            if (value.TryChangeType(out integer)) {
                                controller.Model.NumAvenantPage = integer.ToString();
                            }
                            else {
                                controller.Model.NumAvenantPage = value as string;
                            }
                        }
                    }
                    if (ModelProperties[nameof(controller.Model.ModeNavig)].FirstOrDefault(x => filterContext.ActionParameters.TryGetValue(x, out value)) != default) {
                        controller.Model.ModeNavig = value as string;
                    }
                    if (ModelProperties[nameof(controller.Model.TabGuid)].FirstOrDefault(x => filterContext.ActionParameters.TryGetValue(x, out value)) != default) {
                        if (value is string g) {
                            controller.Model.TabGuid = g.Replace(PageParamContext.TabGuidKey, string.Empty);
                        }
                    }
                    if (ModelProperties[nameof(controller.Model.AddParamValue)].FirstOrDefault(x => filterContext.ActionParameters.TryGetValue(x, out value)) != default) {
                        controller.Model.AddParamValue = value as string;
                        if (controller.Model.AddParamValue.ContainsChars()) {
                            controller.Model.AddParamType = "AVN";
                        }
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }

        private bool IsModelParameter(ActionExecutingContext filterContext, IMetaModelsController controller) {
            if (filterContext.ActionParameters.Values.Any(x => x is MetaModelsBase)) {
                var modelBase = filterContext.ActionParameters.Values.First(x => x is MetaModelsBase) as MetaModelsBase;
                modelBase.CopyTo(controller.Model);
                return true;
            }
            return false;
        }
    }
}