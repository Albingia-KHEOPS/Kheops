using OPServiceContract.OffreSimple;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using LigneInfo = ALBINGIA.Framework.Common.ExcelXmlMap.LigneInfo;
namespace ALBINGIA.OP.OP_MVC.ExcelWeb.Common
{
    public static class ExcelIOParam
    {
        
        //public static  string GetExcelLongPath(string branche)
        //{
        //    if (string.IsNullOrEmpty(branche))
        //        return string.Empty;
        //    var paramBranche =
        //        XmlSerializer<ExcelBrancheEntity>.LoadXmlToEntity(MvcApplication.EXCELXML_ParamPath).////FileContentManager.GetConfigValue("XmlParamPath")).
                    
        //    InfosExcel.FirstOrDefault(
        //                el => el.Name == branche);
        //    return paramBranche==null?string.Empty:paramBranche.FileName;
        //}
        //public static bool IsExistBranche(string branche)
        //{
        //    var fileXmlPramBranch = MvcApplication.EXCELXML_ParamPath;//FileContentManager.GetConfigValue("XmlParamPath");
        //    return
        //        XmlSerializer<ExcelBrancheEntity>.LoadXmlToEntity(fileXmlPramBranch).InfosExcel.FirstOrDefault(
        //            el => el.Name == branche) == null;
        //}
      /// <summary>
      /// 
      /// </summary>
      /// <param name="branche"></param>
      /// <returns></returns>
        public static List<LigneInfo> GetXmlParameters(string branche)
        {
            List<LigneInfo> xmlLigneInfo;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IOffreSimplifieExcel>())
            {
                var wsExcelData=client.Channel;
               var lignesInfo = wsExcelData.GetLignesInfos("OffreSimple", HttpUtility.UrlDecode(branche));
                xmlLigneInfo = lignesInfo.Select(ligneInfo => new LigneInfo
                  {
                    Cells = ligneInfo.Cells, ConvertTo = ligneInfo.ConvertTo, DbMapCol = ligneInfo.DbMapCol
                    , Lib = ligneInfo.Lib, SqlOrder = ligneInfo.SqlOrder, Type = ligneInfo.Type,
                  }).ToList();
            }
            return xmlLigneInfo;
        }
        public static bool IsExistSection(string section)
        {
            switch (section)
            {
                case Framework.Common.Constants.AlbConstantesMetiers.INFORMATIONS_ENTETE:
                    return true;
                case Framework.Common.Constants.AlbConstantesMetiers.INFORMATIONS_RISQUES:
                    return true;
                case Framework.Common.Constants.AlbConstantesMetiers.INFORMATIONS_OBJETS:
                    return true;
                case Framework.Common.Constants.AlbConstantesMetiers.INFORMATIONS_GARANTIES:
                    return true;
            }
            return false;
        }


        public static IEnumerable<KeyValuePair<string, string>> 
            PrepareParameter(IEnumerable<object> param)
        {
           // throw new NotImplementedException();
            //new Dictionary<object, object> { { "{P_0}", "O" }, { "{P_1}", "    23812" }, { "{P_2}", "0" }, { "{P_3}", "1" }, { "{P_4}", "1" } };

            foreach (KeyValuePair<string, string> elem in param)
            {
                yield return new KeyValuePair<string, string>("{P_" + elem.Key+ "}", elem.Value);
            }
        }

        public static IEnumerable<object> GetParams(string parameters, string spliChars)
        {
            var lstParams = parameters.Split(new[] {spliChars}, StringSplitOptions.None);
           
            for (var i = 0; i < lstParams.Length; i++)
            {
                yield return new KeyValuePair<string, string>(i.ToString(CultureInfo.CurrentCulture), lstParams[i]);
            }
        }
    }
}