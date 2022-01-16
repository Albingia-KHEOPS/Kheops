using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.OP.OP_MVC.Common;
using OPServiceContract.OffreSimple;
using System.Collections.Generic;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.ExcelWeb.Common
{
    public class ExcelDataEntity
    {
        #region Méthodes publiques

      /// <summary>
      /// retourne les données de la base de données formatées dans une chaine de caractère.
      /// Les données sont séparées par un caractère de split. La donnée correspondant à la case excel
      /// est présenté par le couple Data+ Séparateur + Coordonnées cellule excel 
      /// </summary>
      /// <param name="branche">Code Branche</param>
      /// <param name="nouvelleVersion"></param>
      /// <param name="param">Paramètres de selections de l'enregistrement dans la base</param>
      /// <param name="user"></param>
      /// <returns></returns>
      public static string GetExcelData(string branche, bool nouvelleVersion, IEnumerable<object> param, string user, string tabGuid) {
            //---------- paramètres à déterminer celon le contexte
            var hParam = ExcelIOParam.PrepareParameter(param).ToList();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IOffreSimplifieExcel>())
            {
                var wsExcelData=client.Channel;
              dynamic returnedData = wsExcelData.LoadData(MvcApplication.EXCELXML_PARAMPATH, "OffreSimple", branche, nouvelleVersion, Framework.Common.Constants.SimpleFolderInfoData.Input, MvcApplication.SPLIT_CONST_HTML, hParam, user);
              if (nouvelleVersion) {
                    string guidSess = tabGuid.Replace("tabGuid", string.Empty);
                    var key = new FolderKey(hParam[1].Value, int.Parse(hParam[2].Value), hParam[0].Value, int.Parse(hParam[3].Value), user, guidSess);
                    AlbSessionHelper.CurrentFolders.Remove(key);
                    AlbSessionHelper.CurrentFolders.Add(key, AlbUserRoles.MakeNewUserInfo(guidSess, user, hParam[1].Value,
                        hParam[2].Value, hParam[0].Value, hParam[3].Value, user,
                        false, false, false));
              }
              return returnedData;
            }
         
        }

        /// <summary>
        /// Met à jour les données excel dans la base de données
        /// </summary>
        /// <param name="branche">Code Branche</param>
        /// <param name="param">Paramètres de selections de l'enregistrement dans la base</param>
        /// <param name="strData">Donées à mettre à jour</param>
        /// <returns>Ok si les donées sont mis à jour. Ko dans le cas échéant</returns>
        public static bool SetExcelData(string branche,  string strData, IEnumerable<object> param,string user)
        {
          //if (branche == null && ExcelIOParam.IsExistBranche(branche)) throw new ArgumentNullException("branche");
         // if (section == null && ExcelIOParam.IsExistSection(section)) throw new ArgumentNullException("section");
          //---------- paramètres à déterminer celon le contexte
          //var hParam = new Dictionary<object, object> { { "{P_0}", "O" }, { "{P_1}", "    23812" }, { "{P_2}", "0" }, { "{P_3}", "1" }, { "{P_4}", "1" } };
          var hParam = ExcelIOParam.PrepareParameter(param).ToList();

          using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IOffreSimplifieExcel>())
          {
              var wsExcelData=client.Channel;
            return wsExcelData.UpdatedData(MvcApplication.EXCELXML_PARAMPATH,"OffreSimple", branche, MvcApplication.SPLIT_CONST_HTML, hParam, strData, user);

          }
        }
        #endregion
      //  #region Métodes privées

      ///// <summary>
      ///// 
      ///// </summary>
      ///// <param name="returnedData"></param>
      ///// <param name="typeRequest"></param>
      ///// <returns></returns>
      //private static string GetStrResult(dynamic returnedData,Framework.Common.Constants.SimpleFolderInfoData typeRequest, out string returnedVersion)
      //{
      //  returnedVersion=string.Empty;
      //  if (typeRequest == Framework.Common.Constants.SimpleFolderInfoData.InOut)
      //  {
      //    var retData = (AllObjExcel) returnedData;
        
      //    if(retData==null)
      //      throw new AlbTechException(new Exception("Donnée excel Invalide. Problème de donnée ou de cast"));
      //    string inStr = GetDataResult(retData.InExcel,out returnedVersion);
      //    string returnedVersionOut;
      //    string outStr = GetDataResult(retData.OutExcel, out returnedVersionOut);
      //    return string.Format("{0}#IN{1}OUT#{2}", inStr, MvcApplication.SPLIT_CONST_HTML, outStr);
      //  }
      //  return GetDataResult(returnedData, out returnedVersion);

        
      //}

      //private static string GetDataResult(dynamic returnedData, out string returnedVersion)
      //{
      //  returnedVersion = string.Empty;
      //  var strBuilder = new StringBuilder();
      //  var propsData = returnedData.GetType();
      //  foreach (var propDataInfo in propsData.GetProperties(BindingFlags.Public | BindingFlags.Instance))
      //  {
      //    if (!propDataInfo.CanRead || propDataInfo.Name.Contains("Cells") || propDataInfo.Name.Contains("ExtensionData"))
      //      continue;
          
      //    var val = propDataInfo.GetValue(returnedData, null);
      //    var cell = propsData.GetProperty("Cells" + propDataInfo.Name).GetValue(returnedData, null);
      //    if (propDataInfo.Name.ToLower() == "version")
      //      returnedVersion = propsData.GetProperty(propDataInfo.Name).GetValue(returnedData, null).ToString();
      //    strBuilder.Append(val + "||" + cell + MvcApplication.SPLIT_CONST_HTML);
      //  }
      //  return strBuilder.ToString();
      //}

      //#endregion
    }
}