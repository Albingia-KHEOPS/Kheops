using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.DynamicGuiIS.Common;
using OP.WSAS400.DTO.ParamIS;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
namespace ALBINGIA.OP.OP_MVC.DynamicGuiIS.Ajax
{
    /// <summary>
    /// Summary description for AjaxExcelInteaction
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    // [System.Web.Script.Services.ScriptService]
    public class DbInteraction : WebService
    {
        [WebMethod, ScriptMethod]
        public string LoadDbData(string modeNavig, string codeObjet, string codeRisque, string codeFormule, string codeOption, string etapeIs, string codeOffre, string version, string type, string branche, string section, string cible, string additionalParams, string splitChars, string strParameters) {
            return BuildHtml(modeNavig, codeObjet, codeRisque, codeFormule, codeOption, etapeIs, codeOffre, version, type, branche, section, splitChars, strParameters);
        }

        internal static string BuildHtml(string modeNavig, string codeObjet, string codeRisque, string codeFormule, string codeOption, string etapeIs, string codeOffre, string version, string type, string branche, string section, string splitChars, string strParameters) {
            var idModele = DbIOParam.PrepareIsIdModele(branche, section);
            var parameters = DbIOParam.GetParams(HttpUtility.UrlDecode(strParameters), HttpUtility.UrlDecode(splitChars));


            var paramForGenIs = DbDataEntity.GetParamGen(etapeIs, idModele, codeRisque, codeObjet, codeFormule, codeOption);
            if (CacheIS.AllISEnteteModelesDto == null) {
                return string.Empty;
            }

            List<ModeleISDto> isModeleEntete = null;
            CacheIS.AllISEnteteModelesDto.ForEach(elm => {
                if (isModeleEntete == null) {
                    isModeleEntete = new List<ModeleISDto>();
                }

                if (elm.NomModele.ToLower() == idModele.ToLower()) {
                    isModeleEntete.Add(elm);
                }
            });

            // Test si on est en mode Historique et aucune donnée n'existe pour l'is alors pas de HTML à générer
            if (modeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Historique) {
                if (!DbDataEntity.RowsExists(section, parameters, isModeleEntete))
                    return string.Empty;
            }
            var dataToMap = DbDataEntity.GetDbData(modeNavig, codeObjet, codeRisque, codeFormule, codeOption, etapeIs, codeOffre, version, type, idModele, HttpUtility.UrlDecode(section), parameters, paramForGenIs, idModele.ToLower(), isModeleEntete);

            // Appel de la méthode de génèration HTML
            List<ParamISLigneInfo> dbLigneInfo = DbIOParam.GetControlsFromDB(idModele).ParamISDBLignesInfo;
            if (dbLigneInfo == null || !dbLigneInfo.Any()) {
                return string.Empty;
            }

            if (dataToMap == string.Empty) {
                if (dbLigneInfo.Any(it => !string.IsNullOrEmpty(it.InternalPropertyName))) {
                    dataToMap = DbDataEntity.GetDbDefaultData(dbLigneInfo.Where(it => !string.IsNullOrEmpty(it.InternalPropertyName)).ToList(), MvcApplication.SPLIT_CONST_HTML, "||");
                }
            }

            var generator = new GenerationDbHTML(
                HttpUtility.UrlDecode(branche),
                new List<ParamISLigneInfo>(dbLigneInfo),
                dataToMap,
                MvcApplication.SPLIT_CONST_HTML, "||",
                codeOffre, version, type, paramForGenIs, idModele,
                HttpUtility.UrlDecode(strParameters),
                modeNavig);

            string html = generator.Generate(HttpUtility.UrlDecode(branche), HttpUtility.UrlDecode(section));

            return html;
        }

        [WebMethod, ScriptMethod]
        public string SaveData(string branche, string section, string cible, string additionalParams, string dataToSave, string splitChars, string strParameters)
        {
            return DbIOParam.SaveISToDB(branche, section, cible, additionalParams, dataToSave, splitChars, strParameters) ? "ok" : "ko";
        }
    }
}
