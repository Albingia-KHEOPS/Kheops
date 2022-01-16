using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel.Activation;
using System.Text;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.ExcelXmlMap;
using OP.DataAccess;
using OP.Services.ClausesRisquesGaranties;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.ExcelDto;
using OPServiceContract.OffreSimple;
using LigneInfo = ALBINGIA.Framework.Common.ExcelXmlMap.LigneInfo;

namespace OP.Services.Excel
{
 
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  [KnownType(typeof(InputExcel))]

  public class OffreSimplifieExcel : IOffreSimplifieExcel
  {
    #region varibles membres
    private static List<KeyValuePair<string, string>> _hsqlParam;
    #endregion
    #region Méthodes publiques


    public SimpleFolderState EditSimpleFolder(string xmlParamExcel, string typeInfo, string branche, List<KeyValuePair<string, string>> hsqlParam)
    {
            var infosParam = CommonExcel.GetControlsFromXml(xmlParamExcel, typeInfo);
            var infos = infosParam.InfosExcel.FirstOrDefault(el => el.Name == branche);
      if (infos == null)
        return SimpleFolderState.NSFNew;
      var sql = infos.SqlRequests.SelectOSEncours.Sql;
            sql = hsqlParam.Aggregate(sql, (current, elem) => current.Replace(elem.Key.ToString(CultureInfo.InvariantCulture), elem.Value.ToString(CultureInfo.InvariantCulture)));
      var valRes = CommonRepository.GetStrValue(sql);
      switch (valRes)
            {
                case "-1":
          return SetNewVersion(hsqlParam, infos);

        case "1": 
        case "2":
                    return CommonRepository.GetStrValue(sql) == "1" ? SimpleFolderState.SFCurrent : SimpleFolderState.SFFinalaized;
      }
      return SetNewVersion(hsqlParam, infos);
    }

   /// <summary>
    /// 
    /// </summary>
    /// <param name="typeInfo"></param>
    /// <param name="branche"></param>
    /// <returns></returns>
    public List<WSAS400.DTO.ExcelDto.LigneInfo> GetLignesInfos(string typeInfo, string branche)
    {
      var lngInfo = new List<WSAS400.DTO.ExcelDto.LigneInfo>();
      CommonExcel.GetLignesInfos(branche, branche).ForEach(el => lngInfo.Add((WSAS400.DTO.ExcelDto.LigneInfo)el));
      return lngInfo;
    }

    /// <summary>
    /// Charge les données spécifiques à un fichier excel
    /// </summary>
    /// <param name="xmlParamExcel">chmain des fichiers</param>
    /// <param name="typeInfo">type correspondante</param>
    /// <param name="branche">Branche correspondante</param>
    /// <param name="nouvelleVersion"></param>
    /// <param name="lineInfoType"></param>
    /// <param name="splitChars"> ensemble de caractères de splits utiliser</param>
    /// <param name="hsqlParam">liste de paramètres</param>
    /// <param name="user"></param>
    /// <returns></returns>
    public string LoadData(string xmlParamExcel, string typeInfo, string branche, bool nouvelleVersion, SimpleFolderInfoData lineInfoType, string splitChars, List<KeyValuePair<string, string>> hsqlParam, string user)
    {
      string returndExcelData = string.Empty;

   
      string returnedVersion;

      _hsqlParam = hsqlParam;
      //-----Lire les paramétres
      var paramBranche = CommonExcel.GetControlsFromXml(xmlParamExcel, typeInfo);
      var excelInfos = paramBranche.InfosExcel.Find(el => el.Name.Contains(branche));
      dynamic dataRes = GetDataFromDb(excelInfos, lineInfoType);

      switch (lineInfoType)
      {
        case SimpleFolderInfoData.Input:
          GetDataCells(dataRes, excelInfos.LignesInfosIn);
          returndExcelData = GetDataResult(dataRes, splitChars, out returnedVersion);
          break;
        case SimpleFolderInfoData.OutPut:
          GetDataCells(dataRes, excelInfos.LignesInfosOut);
          returndExcelData = GetDataResult(dataRes, splitChars, out returnedVersion);
          break;
        case SimpleFolderInfoData.InOut:
          var resHash = (Hashtable)dataRes;
          dynamic resIn = resHash[SimpleFolderInfoData.Input];
          GetDataCells(resIn, excelInfos.LignesInfosIn);
          dynamic resOut = new OutPutExcel();
          GetDataCells(resOut, excelInfos.LignesInfosOut);
          var allObjExcel = new AllObjExcel { InExcel = resIn, OutExcel = resOut };
          dataRes = allObjExcel;

          returndExcelData = GetStrResult(dataRes,
                                            SimpleFolderInfoData.InOut, splitChars,
                                            out returnedVersion);
          break;
      }

      return returndExcelData;
    }
      /// <summary>
      /// Commentaires pour le verrouillage
      /// </summary>
    [Obsolete("ZBO:En attente de confirmation")]
    public string LoadDataWithVersionning(string xmlParamExcel, string typeInfo, string branche, bool nouvelleVersion, SimpleFolderInfoData lineInfoType, string splitChars, List<KeyValuePair<string, string>> hsqlParam, string user)
    {
      string returndExcelData = string.Empty;
     
        string oldCodeOffre = string.Empty;
        string oldVersion = string.Empty;
        string newVersion = string.Empty;

     
        string returnedVersion;
        if (nouvelleVersion)
        {
          if (hsqlParam.Any() && hsqlParam.Count >= 3)
          {
            var type = hsqlParam[0].Value;
            oldCodeOffre = hsqlParam[1].Value;
            oldVersion = hsqlParam[2].Value;
          }
        }
        _hsqlParam = hsqlParam;
        //-----Lire les paramétres
        var paramBranche = CommonExcel.GetControlsFromXml(xmlParamExcel, typeInfo);
        var excelInfos = paramBranche.InfosExcel.Find(el => el.Name.Contains(branche));
        dynamic dataRes = GetDataFromDb(excelInfos, lineInfoType);

        switch (lineInfoType)
        {
          case SimpleFolderInfoData.Input:
            GetDataCells(dataRes, excelInfos.LignesInfosIn);
            returndExcelData = GetDataResult(dataRes, splitChars, out returnedVersion);
            break;
          case SimpleFolderInfoData.OutPut:
            GetDataCells(dataRes, excelInfos.LignesInfosOut);
            returndExcelData = GetDataResult(dataRes, splitChars, out returnedVersion);
            break;
          case SimpleFolderInfoData.InOut:
            var resHash = (Hashtable)dataRes;
            dynamic resIn = resHash[SimpleFolderInfoData.Input];
            GetDataCells(resIn, excelInfos.LignesInfosIn);
            dynamic resOut = resHash[SimpleFolderInfoData.OutPut];
            GetDataCells(resOut, excelInfos.LignesInfosOut);
            var allObjExcel = new AllObjExcel { InExcel = resIn, OutExcel = resOut };
            dataRes = allObjExcel;

            string lineDataExcel = GetStrResult(dataRes,
                                              SimpleFolderInfoData.InOut, splitChars,
                                              out returnedVersion);
            returndExcelData = dataRes == null ? string.Empty : returnedVersion + "#NewVer#" + lineDataExcel;
            break;
        }

      return returndExcelData;
    }

    /// <summary>
    /// Mise à jour des données depuis le fichier excel vers DB
    /// </summary>
    /// <param name="xmlParamExcel">chmain des fichiers</param>
    /// <param name="typeInfo">type correspondante</param>
    /// <param name="branche">Branche correspondante</param>
    /// <param name="spliChar"> ensemble de caractères de splits utiliser</param>
    /// <param name="hsqlParam">liste de paramètres</param>
    /// <param name="strData">données à méttre à jour</param>
    /// <param name="user"></param>
    /// <returns></returns>
        public bool UpdatedData(string xmlParamExcel, string typeInfo, string branche, string spliChar, List<KeyValuePair<string, string>> hsqlParam, string strData, string user)
    {
      var paramBranche = CommonExcel.GetControlsFromXml(xmlParamExcel, typeInfo);
      var excelInfos = paramBranche.InfosExcel.Find(el => el.Name.Contains(branche));
      var sql = ExcelRepository.ExistRow(excelInfos.SqlRequests.SelectExistOS.Sql, hsqlParam) ? excelInfos.SqlRequests.Update.Sql : excelInfos.SqlRequests.Insert.Sql;
      sql = hsqlParam.Aggregate(sql, (current, elem) => current.Replace(elem.Key.ToString(CultureInfo.InvariantCulture), elem.Value.ToString(CultureInfo.InvariantCulture)));
      var elemsData = strData.Split(new[] { spliChar }, StringSplitOptions.None);
      var hValElems = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("{0}", string.Empty) };
            hValElems.AddRange(elemsData.Select((t, i) => new KeyValuePair<string, string>("{" + (i + 1).ToString(CultureInfo.InvariantCulture) + "}", t)));
      sql = hValElems.Aggregate(sql, (current, elem) => current.Replace(elem.Key.ToString(CultureInfo.InvariantCulture), elem.Value.ToString(CultureInfo.InvariantCulture)));
            bool result = false;

            try
            {
                result = DataAccess.ExcelRepository.UpdateDB(sql);
            }
            catch
            {
                throw;
            }

      //OffreRepository.LoadInfoOffreSimpl(hsqlParam[1].Value, hsqlParam[2].Value, hsqlParam[0].Value, user);


      //*********Appel Génèration des clauses***********


      var rsqClauses = new RisquesGaranties();
      {
        RetGenClauseDto retGenClause = rsqClauses.GenerateClause(hsqlParam[0].Value, hsqlParam[1].Value, Convert.ToInt32(hsqlParam[2].Value),
                                     new ParametreGenClauseDto { ActeGestion = "**", Letape = "**" });
        if (retGenClause != null && !string.IsNullOrEmpty(retGenClause.MsgErreur))
        {
          throw new AlbFoncException(retGenClause.MsgErreur);
        }
      }
      return result;
    }

    #endregion
    #region Méthode privées

    /// <summary>
    /// Retourne une liste qui va alimenter une DropDownList
    /// </summary>
    /// <returns>Liste de LIBCodeDto (Code /Valeur)</returns>
    /// <summary>                                                                                                                 
    /// 
    /// </summary>
    /// <param name="hsqlParam"></param>
    /// <param name="infos"></param>
    /// <returns></returns>
    private static SimpleFolderState SetNewVersion(IEnumerable<KeyValuePair<string, string>> hsqlParam, ExcelInfos infos)
    {
      var sqlExists = infos.SqlRequests.SelectExist.Sql;
      sqlExists = hsqlParam.Aggregate(sqlExists,
                                      (current, elem) =>
                                      current.Replace(elem.Key.ToString(CultureInfo.InvariantCulture),
                                                      elem.Value.ToString(CultureInfo.InvariantCulture)));
      return CommonRepository.ExistRow(sqlExists) ? SimpleFolderState.NSFNewWithVersion : SimpleFolderState.NSFNew;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataRes"></param>
    /// <param name="excelInfos"></param>
    private void GetDataCells(dynamic dataRes, List<LigneInfo> excelInfos)
    {
     
        var dataresTypes = dataRes.GetType();
        var properties = dataRes.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var propertyInfo in properties)
        {
          if (!propertyInfo.CanRead) continue;
          var lng = excelInfos.Find(el => el.Lib == propertyInfo.Name);
          if (lng == null) continue;
          var elem = dataresTypes.GetProperty("Cells" + propertyInfo.Name);
          var elemVal = dataresTypes.GetProperty(propertyInfo.Name);
          if (elem != null)
          {
            elem.SetValue(dataRes, lng.Cells.ToString(CultureInfo.InvariantCulture), null);
            var propVal = dataRes.GetType().GetProperty(propertyInfo.Name).GetValue(dataRes, null);
            string val = propVal == null ? string.Empty : propVal.ToString();

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
        default:
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
          elem.SetValue(dataRes, string.IsNullOrEmpty(val) || !int.TryParse(val, out valDate) || valDate == 0
                                     ? 0
                                     : valDate
                        , null);
          break;
        case "h":
          int valhour;
          elem.SetValue(dataRes, string.IsNullOrEmpty(val) || !int.TryParse(val, out valhour) || valhour == 0
                                     ? 0
                                     : valhour
                        , null);
          break;

      }
    }

        private dynamic GetDataFromDb(ExcelInfos excelInfo, SimpleFolderInfoData lineInfoType, bool useOut = false)
    {
    
      switch (lineInfoType)
      {
        case SimpleFolderInfoData.Input:
         return DataAccess.ExcelRepository.GetDataFromDB<InputExcel>(excelInfo.SqlRequests.Select.Sql, _hsqlParam).FirstOrDefault() ?? new InputExcel();
         
        case SimpleFolderInfoData.OutPut:
          return DataAccess.ExcelRepository.GetDataFromDB<InputExcel>(excelInfo.SqlRequests.SelectOutPut.Sql, _hsqlParam).FirstOrDefault() ?? new InputExcel();
        
        case SimpleFolderInfoData.InOut:
          var lstRes = new Hashtable();
          lstRes.Add(SimpleFolderInfoData.Input, DataAccess.ExcelRepository.GetDataFromDB<InputExcel>(excelInfo.SqlRequests.Select.Sql, _hsqlParam).FirstOrDefault() ?? new InputExcel());
          if (useOut)
            lstRes.Add(SimpleFolderInfoData.OutPut, DataAccess.ExcelRepository.GetDataFromDB<OutPutExcel>(excelInfo.SqlRequests.SelectOutPut.Sql, _hsqlParam).FirstOrDefault() ?? new OutPutExcel());
          return lstRes;
      }

      return null;
    }
    #endregion

    #region Métodes privées GetStr

    /// <summary>
    /// 
    /// </summary>
    /// <param name="returnedData"></param>
    /// <param name="typeRequest"></param>
    /// <param name="returnedVersion"></param>
    /// <returns></returns>
    private static string GetStrResult(dynamic returnedData, SimpleFolderInfoData typeRequest, string splitChars, out string returnedVersion)
    {
     
        returnedVersion = string.Empty;
        if (typeRequest == SimpleFolderInfoData.InOut)
        {
          var retData = returnedData;

          if (retData == null)
            throw new AlbTechException(new Exception("Donnée excel Invalide. Problème de donnée ou de cast"));
          string inStr = GetDataResult(retData.InExcel, splitChars, out returnedVersion);
          string returnedVersionOut;
          string outStr = GetDataResult(retData.OutExcel, splitChars, out returnedVersionOut);
          return string.Format("{0}#IN{1}OUT#{2}", inStr, splitChars, outStr);
        }
        return GetDataResult(returnedData, splitChars, out returnedVersion);
    
      
    }

    private static string GetDataResult(dynamic returnedData, string splitChars, out string returnedVersion)
    {
     
        returnedVersion = string.Empty;
        var strBuilder = new StringBuilder();
        var propsData = returnedData.GetType();
        foreach (var propDataInfo in propsData.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
          if (!propDataInfo.CanRead || propDataInfo.Name.Contains("Cells") || propDataInfo.Name.Contains("ExtensionData"))
            continue;

          var val = propDataInfo.GetValue(returnedData, null);
          var cell = propsData.GetProperty("Cells" + propDataInfo.Name).GetValue(returnedData, null);
          if (propDataInfo.Name.ToLower() == "version")
            returnedVersion = propsData.GetProperty(propDataInfo.Name).GetValue(returnedData, null).ToString();
          strBuilder.Append(val + "||" + cell + splitChars);
        }
        return strBuilder.ToString();
   
      
    }

    #endregion
  }

}
