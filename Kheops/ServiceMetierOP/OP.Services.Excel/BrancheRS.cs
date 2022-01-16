using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Activation;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.ExcelXmlMap;
using OP.DataAccess;
using System.Collections.Generic;
using OP.WSAS400.DTO.ExcelDto;
using System.Runtime.Serialization;
using OPServiceContract.IS;

namespace OP.Services.Excel
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [KnownType(typeof(WSAS400.DTO.ExcelDto.RS.Entete))]
    [KnownType(typeof(WSAS400.DTO.ExcelDto.RS.Garanties))]
    [KnownType(typeof(WSAS400.DTO.ExcelDto.RS.Objets))]
  
    public class BrancheRS : IExcelInfoSpecif
    {
        #region varibles membres
        private static List<KeyValuePair<string, string>> _hsqlParam;
        #endregion

        #region Méthodes publiques
        /// <summary>
        /// 
        /// </summary>
        /// <param name="branche"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public List<OP.WSAS400.DTO.ExcelDto.LigneInfo> GetLignesInfosSection(string branche, string section)
        {
            List<OP.WSAS400.DTO.ExcelDto.LigneInfo>lngInfo=new List<OP.WSAS400.DTO.ExcelDto.LigneInfo>();
            CommonExcel.GetLignesInfos(branche, section).ForEach(el => lngInfo.Add((OP.WSAS400.DTO.ExcelDto.LigneInfo)el));
            return lngInfo;
        }

        /// <summary>
        /// Charge les données spécifiques à un fichier excel
        /// </summary>
        /// <param name="xmlParamExcel"> </param>
        /// <param name="branche">branche correspondante</param>
        /// <param name="section">section correspondante ex:Risque, objets, garanties...</param>
        /// <param name="splitChars"> ensemble de caractères de splits utiliser</param>
        /// <param name="hsqlParam">liste de paramètres</param>
        /// <returns></returns>
        public dynamic LoadData(string xmlParamExcel, string branche, string section, string splitChars, KeyValuePair<string, string>[] hsqlParam)
        {
            _hsqlParam = hsqlParam.ToList();
            //-----Lire les paramétres
            var paramBranche = CommonExcel.GetControlsFromXml(xmlParamExcel, branche);
            var excelInfos = paramBranche.InfosExcel.Find(el => el.Name == section);
            if (!DataAccess.ExcelRepository.ExistRow(excelInfos.SqlRequests.SelectExist.Sql, hsqlParam))
                return null;
           // string clsType = string.Empty;
            dynamic dataRes = GetDataFromDb(excelInfos, section);
            GetDataCells(dataRes, excelInfos);
            return dataRes;
        }

        /// <summary>
        /// Mise à jour des données depuis le fichier excel vers DB
        /// </summary>
        /// <param name="xmlParamExcel"> </param>
        /// <param name="branche">branche correspondante</param>
        /// <param name="section">section correspondante ex:Risque, objets, garanties...</param>
        /// <param name="spliChar"> ensemble de caractères de splits utiliser</param>
        /// <param name="hsqlParam">liste de paramètres</param>
        /// <param name="strData">données à méttre à jour</param>
        /// <returns></returns>
        public bool UpdatedData(string xmlParamExcel, string branche, string section, string spliChar, KeyValuePair<string, string>[] hsqlParam, string strData)
        {
            var paramBranche = CommonExcel.GetControlsFromXml(xmlParamExcel, branche);
            var excelInfos = paramBranche.InfosExcel.Find(el => el.Name == section);
            // if()
            //return DataAccess.ExcelRepository.GetDataExcel<WSAS400.DTO.ExcelDto.RS.Objets>(sql, hsqlParam);
            var sql = DataAccess.ExcelRepository.ExistRow(excelInfos.SqlRequests.SelectExist.Sql, hsqlParam.ToList()) ? excelInfos.SqlRequests.Update.Sql : excelInfos.SqlRequests.Insert.Sql;
            sql = hsqlParam.ToList().Aggregate(sql, (current, elem) => current.Replace(elem.Key.ToString(CultureInfo.InvariantCulture), elem.Value.ToString(CultureInfo.InvariantCulture)));
            var elemsData = strData.Split(new[] { spliChar }, StringSplitOptions.None);


            sql = elemsData.Aggregate(sql, (current, elem) =>
                                               {
                                                   var cellsData = elem.Split(new[] { "||" },
                                                                                 StringSplitOptions.None);
                                                   if (cellsData.Length == 1 & string.IsNullOrEmpty(cellsData[0]))
                                                       return current;
                                                   var elemLine =
                                                       excelInfos.LignesInfos.FirstOrDefault(
                                                           el => el.Cells == cellsData[0]);
                                                   if (elemLine != null)
                                                   {
                                                       
                                                       //current = current.Replace("{" + elemLine.SqlOrder + "}", cellsData[0].ToLower() == "true" ? "1" : cellsData[0].ToLower() == "false" ? "0" : cellsData[0]);
                                                       current = current.Replace("{" + elemLine.SqlOrder + "}", ConvertExcelDataToDbType(cellsData[1], elemLine));
                                                   }

                                                   return current;
                                               });
            return CommonRepository.UpdateDB(sql);
        }

        /// <summary>
        /// Retourne une liste qui va alimenter une DropDownList
        /// </summary>
        /// <param name="sqlRequest">Requête sql</param>
        /// <returns>Liste de LIBCodeDto (Code /Valeur)</returns>
        public List<LibCodeDto> GetDropdownlist(string sqlRequest, KeyValuePair<string, string>[] hsqlParam)
        {
            var sql = hsqlParam.ToList().Aggregate(sqlRequest, (current, elem) => current.Replace(elem.Key.ToString(CultureInfo.InvariantCulture), elem.Value.ToString(CultureInfo.InvariantCulture)));
            List<LibCodeDto> result = CommonRepository.GetDropDownValue<LibCodeDto>(sql);
            result.Insert(0, new LibCodeDto() { Code = "", Libelle = "" });
            return result;
        }

        #endregion
        #region Méthode privées
        private void GetDataCells(dynamic dataRes, ExcelInfos excelInfos)
        {
            var dataresTypes = dataRes.GetType();
            var properties = dataRes.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in properties)
            {
                if (!propertyInfo.CanRead) continue;
                var lng = excelInfos.LignesInfos.Find(el => el.Lib == propertyInfo.Name);
                if (lng == null) continue;
                var elem = dataresTypes.GetProperty("Cells" + propertyInfo.Name);
                var elemVal = dataresTypes.GetProperty( propertyInfo.Name);
                if (elem != null)
                {
                    elem.SetValue(dataRes, lng.Cells.ToString(CultureInfo.InvariantCulture), null);
                    string val= dataRes.GetType().GetProperty(propertyInfo.Name).GetValue(dataRes, null).ToString();
                   // string val = dataresTypes.GetType().GetProperty(propertyInfo.Name).GetValue(dataRes, null).ToString();
                    ConvertDataToDisplayType(dataRes, lng, elemVal, val);
                }
            }
        }
        private static string ConvertExcelDataToDbType(string val, ALBINGIA.Framework.Common.ExcelXmlMap.LigneInfo line)
        {
            switch (line.ConvertTo.ToLower())
            {
                //Boolean
                case "b":
                    return string.IsNullOrEmpty(val) || val.ToLower() == "faux" || val.ToLower() == "false" ? "N" : "O";
                //Date
                case "d":
                    int valDate;
                    //var convertDateToInt = DataAccess.CommonRepository.ConvertDateToInt(Convert.ToDateTime(val));


                    return string.IsNullOrEmpty(val) || !int.TryParse(val, out valDate)
                               ? "0"
                               : valDate.ToString(CultureInfo.InvariantCulture);
                 
                   
                //Heure
                case "h":
                    int valHour;
                         return string.IsNullOrEmpty(val) || !int.TryParse(val, out valHour)
                                    ? "0"
                                    : valHour.ToString(CultureInfo.InvariantCulture);
                   
                case "n":
                    if (string.IsNullOrEmpty(val))
                    {
                        return "0";
                    }
                    int result;
                    if (!int.TryParse(val, out result))
                    {
                        throw new InvalidCastException(line.Lib);
                    }
                    return val;
                default :
                    if (val.Equals("null"))
                    {
                        throw new ArgumentNullException(line.Lib);
                    }
                    return val;
            }
          
        }

        private static void ConvertDataToDisplayType(dynamic dataRes, ALBINGIA.Framework.Common.ExcelXmlMap.LigneInfo lng, dynamic elem, string val)
        {

            switch (lng.ConvertTo.ToLower())
            {
                case "b":
                    elem.SetValue(dataRes, string.IsNullOrEmpty(val) || val.ToLower() == "n" ? "FAUX" : "VRAI", null);
                    break;
                case "d":
                    int valDate;
                    elem.SetValue(dataRes, string.IsNullOrEmpty(val) || !int.TryParse(val, out valDate) || valDate==0
                                               ? 0
                                               :valDate
                                  , null);
                    break;
                case "h":
                    int valhour;
                    elem.SetValue(dataRes, string.IsNullOrEmpty(val) || !int.TryParse(val, out valhour) || valhour==0
                                               ? 0
                                               : valhour
                                  , null);
                    break;

            }
        }

        private dynamic GetDataFromDb(ExcelInfos excelInfo, string section)
        {
            var sql = excelInfo.SqlRequests.Select.Sql;
            dynamic lstRes = null;

            switch (section)
            {
                case AlbConstantesMetiers.INFORMATIONS_ENTETE:
                    lstRes = DataAccess.ExcelRepository.GetDataFromDB<WSAS400.DTO.ExcelDto.RS.Entete>(sql, _hsqlParam).FirstOrDefault() ?? new WSAS400.DTO.ExcelDto.RS.Entete();
                   
                    break;
                case AlbConstantesMetiers.INFORMATIONS_RISQUES:
                    break;
                case AlbConstantesMetiers.INFORMATIONS_OBJETS:
                    lstRes = DataAccess.ExcelRepository.GetDataFromDB<WSAS400.DTO.ExcelDto.RS.Objets>(sql, _hsqlParam).FirstOrDefault() ?? new WSAS400.DTO.ExcelDto.RS.Objets();
                    break;
                case AlbConstantesMetiers.INFORMATIONS_GARANTIES:
                    lstRes = DataAccess.ExcelRepository.GetDataFromDB<WSAS400.DTO.ExcelDto.RS.Garanties>(sql, _hsqlParam).FirstOrDefault() ?? new WSAS400.DTO.ExcelDto.RS.Garanties();
                    break;
                case AlbConstantesMetiers.INFORMATIONS_OPTIONS:
                    break;
            }
          
            return lstRes;
        }
        #endregion
    }



}
