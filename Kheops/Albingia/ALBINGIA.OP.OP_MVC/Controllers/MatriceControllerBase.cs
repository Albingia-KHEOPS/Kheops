using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class MatriceControllerBase<T>: RemiseEnVigueurController<T> where T : MetaModelsBase {
        public override bool AllowUpdate {
            get {
                return !GetIsReadOnly(this.model.TabGuid, $"{Model.CodePolicePage}_{Model.VersionPolicePage}_{Model.TypePolicePage}")
                    && !IsModifHorsAvenant;
            }
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string codeFormule, string codeOption, string codeRisque,string CopieBnsPb, string tabGuid, string formGen, string paramRedirect, string modeNavig, string addParamType, string addParamValue) {
            //Redirection vers la page sélectionnée dans le menu
            if (paramRedirect.ContainsChars()) {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }
            var folder = new Folder { CodeOffre = codeOffre, Version = int.Parse(version), Type = type };
            string[] contextInfos = null;
            switch (cible?.ToLower()) {
                case "recherchesaisie":
                    return RedirectToAction(job, cible);
                case "formulegarantie":
                    contextInfos = new[] { codeFormule, codeOption, formGen };
                    break;
                case "creationformulegarantie":
                    cible = job == "Index" ? "FormuleGarantie" : cible;
                    contextInfos = new[] { codeFormule, codeOption, formGen };
                    break;
                case "detailsobjetrisque":
                    contextInfos = new[] { byte.MinValue.ToString() };
                    break;
                case "detailsrisque":
                    contextInfos = new[] { codeRisque, CopieBnsPb };
                    break;
                case "engagements":
                    var typeAvenant = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
                    if (typeAvenant.IsIn(AlbConstantesMetiers.TYPE_AVENANT_MODIF, AlbConstantesMetiers.TYPE_AVENANT_RESIL, AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR)) {
                        cible = "EngagementPeriodes";
                    }
                    break;
                default:
                    SpecialRedirection(cible, job, codeOffre, version, type, codeFormule, codeOption, codeRisque, tabGuid, formGen, paramRedirect, modeNavig, addParamType, addParamValue, out cible, out contextInfos);
                    break;
            }

            return RedirectToAction(job, cible, new {
                id = AlbParameters.BuildFullId(folder, contextInfos, tabGuid, addParamValue, modeNavig),
            });
        }

        protected virtual void SpecialRedirection(string cible, string job, string codeOffre, string version, string type, string codeFormule, string codeOption, string codeRisque, string tabGuid, string formGen, string paramRedirect, string modeNavig, string addParamType, string addParamValue, out string newCible, out string[] contextInfos) {
            newCible = cible;
            contextInfos = null;
        }
    }
}