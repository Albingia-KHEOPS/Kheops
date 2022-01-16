using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using EmitMapper;
using OP.DataAccess;
using OP.Services.WSKheoBridge;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.ParamIS;
using OPServiceContract.IS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel.Activation;

namespace OP.Services.IS
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [KnownType(typeof(WSAS400.DTO.ExcelDto.RS.Entete))]
    [KnownType(typeof(WSAS400.DTO.ExcelDto.RS.Garanties))]
    [KnownType(typeof(WSAS400.DTO.ExcelDto.RS.Objets))]
    public class InfoSpecif : IInfoSpecif
    {

        #region varibles membres

        private static List<KeyValuePair<string, string>> _hsqlParam;

        #endregion

        #region Méthodes publiques

        public List<AffichageISLineDto> GetISDisplayConditions(string codeOffre, string type, string version, string modeleId, ParametreGenClauseDto parmIS, out bool isError)
        {
            //// ZBO : Test sans appel KheoBridge : génèration d eclause
            //isError = false;
            //return null;
            isError = false;
            using (var serviceContext = new KheoBridge())
            {
                var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
                if (!string.IsNullOrEmpty(kheoBridgeUrl))
                    serviceContext.Url = kheoBridgeUrl;

                List<AffichageISLineDto> affichageIsLineDto = null;
                var ver = 0;
                if (version != "0")
                {
                    if (!int.TryParse(version, out ver))
                        throw new Exception("Erreur de paramètre version");
                }
                try
                {
                    var retParmISDto =
                      ObjectMapperManager.DefaultInstance.GetMapper<ParametreGenClauseDto, KpClausePar>().Map(parmIS);
                    retParmISDto.CodeModeleIs = modeleId;
                    retParmISDto.LeContexte = string.Empty;
                    var isTitlesToDisplay = serviceContext.ConditionnerAffichageIs(type, codeOffre, ver, retParmISDto).ToList();
                    if (isTitlesToDisplay != null)
                    {
                        affichageIsLineDto = new List<AffichageISLineDto>();
                        isTitlesToDisplay.ForEach(isTitle => affichageIsLineDto.Add(
                          ObjectMapperManager.DefaultInstance.GetMapper<KtIsAffiche, AffichageISLineDto>().Map(isTitle)));
                    }
                }
                catch
                {
                    isError = true;
                }
                return affichageIsLineDto;
            }
        }
        /// <summary>
        /// Retourne les Lignes du modèles
        /// </summary>
        /// <param name="modeleId">L'id du modèle</param>
        /// <returns></returns>
        public List<ParamISLigneInfoDto> GetParamISLignesInfo(string modeleId)
        {

            return GetParamLigneInfo(modeleId);
            // return ParamISRepository.GetParamISLignesInfo(modeleId);
        }


        /// <summary>
        /// Retrieve the list of default values for the parameters of IS
        /// </summary>
        /// <param name="parametersIS">List of parameters IS</param>
        /// <returns>Concatenation of IS parameter Name and default IS parameter value</returns>
        public Dictionary<string, string> GetISDefaultValueData(List<string> parametersIS)
        {

            return ParamISRepository.GetISDefaultValueData(parametersIS);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="branche"></param>
        ///// <param name="section"></param>
        ///// <returns></returns>
        //public List<WSAS400.DTO.ExcelDto.LigneInfo> GetLignesInfosSection(string branche, string section)
        //{
        //  List<OP.WSAS400.DTO.ExcelDto.LigneInfo> lngInfo = new List<WSAS400.DTO.ExcelDto.LigneInfo>();
        //  CommonExcel.GetLignesInfosSection(branche, section).ForEach(el => lngInfo.Add((WSAS400.DTO.ExcelDto.LigneInfo)el));
        //  return lngInfo;
        //}
        public List<ModeleISDto> GetParamEntetModIs(string modeleId)
        {
            return ParamISRepository.GetISModeles(modeleId);
        }


        /// <summary>
        /// Charge les données spécifiques à un fichier excel
        /// </summary>
        /// <param name="section">section correspondante ex:Risque, objets, garanties...</param>
        /// <param name="modeNavig"></param>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="parmClause"></param>
        /// <param name="modeleId"></param>
        /// <param name="splitChars"> ensemble de caractères de splits utiliser</param>
        /// <param name="hsqlParam">liste de paramètres</param>
        /// <param name="isModelLines"></param>
        /// <param name="idModel"></param>
        /// <param name="paramBranche"></param>
        /// <returns></returns>
        //public dynamic LoadData(string xmlParamExcel, string branche, string section, string splitChars, KeyValuePair<string, string>[] hsqlParam)
        //public dynamic LoadData(string codeOffre, string version, string type, ParametreGenClauseDto parmClause, string modeleId, string section, string splitChars, KeyValuePair<string, string>[] hsqlParam, List<ParamISLigneInfoDto> isModelLines, List<ModeleISDto> paramBranche)
        public dynamic LoadData(string modeNavig, string codeOffre, string version, string type, ParametreGenClauseDto parmClause, string modeleId, string section, string splitChars, KeyValuePair<string, string>[] hsqlParam, string idModel, List<ModeleISDto> paramBranche)
        {
            _hsqlParam = hsqlParam.ToList();
            //-----Lire les paramétres
            //var paramBranche1 = CommonExcel.GetControlsFromXml(xmlParamExcel, branche);
            // var paramBranche = ParamISRepository.GetISModeles(modeleId);
            var dbIsInfoModele = paramBranche.FirstOrDefault();
            if (dbIsInfoModele != null)
            {
                int? dateDeb = dbIsInfoModele.DateDebut;
                int? dateFin = dbIsInfoModele.DateFin;
                if (dbIsInfoModele.DateFin == 0 ||
                    (AlbConvert.ConvertIntToDate(dateDeb) <= DateTime.Now &&
                     AlbConvert.ConvertIntToDate(dateFin) >= DateTime.Now))
                {
                    var sql = modeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Historique ? dbIsInfoModele.SqlExist.Replace("FROM KP", "FROM HP").Replace("FROM  KP", "FROM  HP") : dbIsInfoModele.SqlExist;
                    if (!ExcelRepository.ExistRow(sql, hsqlParam))
                        return null;
                    dynamic dataRes = GetDataFromDb(dbIsInfoModele, section, modeNavig);
                    //GetDataCells(codeOffre, version, type, dataRes, modeleId, parmClause, isModelLines);
                    GetDataCells(codeOffre, version, type, dataRes, modeleId, parmClause, idModel);
                    return dataRes;
                }
            }
            return null;
        }

        /// <summary>
        /// Vérifie s'il ya au moin une ligne IS correspondant aux critères
        /// </summary>
        /// <param name="section">section correspondante ex:Risque, objets, garanties...</param>
        /// <param name="hsqlParam">liste de paramètres</param>
        /// <param name="paramBranche"></param>
        /// <returns></returns>
        public bool RowsIsExist(string section, KeyValuePair<string, string>[] hsqlParam, List<ModeleISDto> paramBranche)
        {
            _hsqlParam = hsqlParam.ToList();
            var dbIsInfoModele = paramBranche.FirstOrDefault();
            if (dbIsInfoModele != null)
            {
                var sql = dbIsInfoModele.SqlExist.Replace("FROM KP", "FROM HP").Replace("FROM  KP", "FROM  HP");
                return ExcelRepository.ExistRow(sql, hsqlParam);
            }
            return false;
        }
        /// <summary>
        /// Mise à jour des données depuis le fichier excel vers DB
        /// </summary>
        /// <param name="modeleId">ID du modèle</param>
        /// <param name="section">section correspondante ex:Risque, objets, garanties...</param>
        /// <param name="spliChar"> ensemble de caractères de splits utiliser</param>
        /// <param name="hsqlParam">liste de paramètres</param>
        /// <param name="strData">données à méttre à jour</param>
        /// <returns></returns>
        //public bool UpdatedData(string xmlParamExcel, string branche, string section, string spliChar, KeyValuePair<string, string>[] hsqlParam, string strData)
        public bool UpdatedData(string modeleId, string section, string spliChar,
                                KeyValuePair<string, string>[] hsqlParam, string strData)
        {
            //var paramBranche1 = CommonExcel.GetControlsFromXml(xmlParamExcel, branche);
            //var excelInfos = paramBranche1.InfosExcel.Find(el => el.Name == section);
            //return DataAccess.ExcelRepository.GetDataExcel<WSAS400.DTO.ExcelDto.RS.Objets>(sql, hsqlParam);
            var paramBranche = ParamISRepository.GetISModeles(modeleId);
            var dbIsInfoModele = paramBranche.FirstOrDefault();
            if (dbIsInfoModele != null)
            {
                int? dateDeb = dbIsInfoModele.DateDebut;
                int? dateFin = dbIsInfoModele.DateFin;
                if (!dateFin.HasValue ||
                    (AlbConvert.ConvertIntToDate(dateDeb) <= DateTime.Now &&
                     AlbConvert.ConvertIntToDate(dateFin) >= DateTime.Now))
                {
                    var sql = ExcelRepository.ExistRow(dbIsInfoModele.SqlExist, hsqlParam.ToList())
                                ? dbIsInfoModele.SqlUpdate
                                : dbIsInfoModele.SqlInsert;
                    sql = hsqlParam.ToList()
                                   .Aggregate(sql,
                                              (current, elem) => current.Replace(elem.Key.ToString(CultureInfo.InvariantCulture)
                                                                                 ,
                                                                                 elem.Value.ToString(CultureInfo.InvariantCulture)));
                    var elemsData = strData.Split(new[] { spliChar }, StringSplitOptions.None);

                    var paramISLignesInfo = GetParamLigneInfo(modeleId);//ParamISRepository.GetParamISLignesInfo(modeleId);
                    sql = elemsData.Aggregate(sql, (current, elem) =>
                      {
                          var cellsData = elem.Split(new[] { "||" },
                                                     StringSplitOptions.None);
                          if (cellsData.Length == 1 & string.IsNullOrEmpty(cellsData[0]))
                              return current;

                          var elemLine =
                            paramISLignesInfo.FirstOrDefault(el => el.Cells == cellsData[0]);
                          if (elemLine != null)
                          {

                              //current = current.Replace("{" + elemLine.SqlOrder + "}", cellsData[0].ToLower() == "true" ? "1" : cellsData[0].ToLower() == "false" ? "0" : cellsData[0]);
                              current = current.Replace("{" + elemLine.SqlOrder + "}",
                                                        ConvertExcelDataToDbType(cellsData[1], elemLine));
                          }

                          return current;
                      });
                    return CommonRepository.UpdateDB(sql);
                }
            }
            return false;
        }

        /// <summary>
        /// Retourne une liste qui va alimenter une DropDownList
        /// </summary>
        /// <param name="sqlRequest">Requête sql</param>
        /// <returns>Liste de LIBCodeDto (Code /Valeur)</returns>
        public List<DtoCommon> GetDropdownlist(string sqlRequest, KeyValuePair<string, string>[] hsqlParam)
        {
            var sql = hsqlParam.ToList().Aggregate(sqlRequest, (current, elem) => current.Replace(elem.Key.ToString(CultureInfo.InvariantCulture), elem.Value.ToString(CultureInfo.InvariantCulture)));
            var result = CommonRepository.GetDropDownValue<DtoCommon>(sql);
            result.Insert(0, new DtoCommon() { Code = "", Libelle = "" });
            return result;
        }
        public bool InitISCache()
        {
            Common.CommonOffre.InitISModeles(true);
            return true;
        }

        /// <summary>
        /// Verifie l'existance de données IS 
        /// Utilisée notamment pour vérifier la présence de données de RECUP IS
        /// </summary>
        /// <param name="sqlExist"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool IsDataISExist(string sqlExist, KeyValuePair<string, string>[] param)
        {
            if (!string.IsNullOrEmpty(sqlExist) && param != null)
            {
                return ExcelRepository.ExistRow(sqlExist, param);
            }
            return false;
        }

        #endregion
        #region Méthode privées
        private List<ParamISLigneInfoDto> GetParamLigneInfo(string modeleId)
        {
            var lignesModeles = Common.CommonOffre.GetWsDataCache<List<ParamISLigneInfoDto>>(Common.CommonOffre.WSKEYCACHE_ISMODL);
            if (lignesModeles != null)
            {
                return string.IsNullOrEmpty(modeleId) ? lignesModeles : lignesModeles.FindAll(el => el.ModeleID == modeleId);
            }
            lignesModeles = ParamISRepository.GetParamISLignesInfo("");
            Common.CommonOffre.InitWsCache(Common.CommonOffre.WSKEYCACHE_ISMODL);
            Common.CommonOffre.SetWsDataCache(Common.CommonOffre.WSKEYCACHE_ISMODL, lignesModeles);
            return string.IsNullOrEmpty(modeleId) ? lignesModeles : lignesModeles.FindAll(el => el.ModeleID == modeleId);
        }
        //private void GetDataCells(dynamic dataRes, ExcelInfos excelInfos)
        //private void GetDataCells(string codeOffre, string version, string type, dynamic dataRes, string modeleId, ParametreGenClauseDto parmIS, List<ParamISLigneInfoDto> isModelLines)
        private void GetDataCells(string codeOffre, string version, string type, dynamic dataRes, string modeleId, ParametreGenClauseDto parmIS, string idModel)
        {
            // var paramISLignesInfo = isModelLines;//

            var paramISLignesInfo = GetParamISLignesInfo(modeleId);
            //var paramISLignesInfo = GetParamLigneInfo(modeleId);//ParamISRepository.GetParamISLignesInfo(modeleId);
            //**********************************
            //TODO:ZBO - Appel du Kheo Bridge pour executer les scripts de conditionnements des IS -Objets de 
            // retour : isTitlesToDisplay qui traite seulement les titres
            //regroupper toutes les proriétes appartenant au titres
            //  var affichageIsLineDto = new List<AffichageISLineDto>();
            //  bool isDisplayConditions = true;
            //using (var serviceContext = new KheoBridge())
            //{



            //    int ver = 0;
            //  if (version != "0")
            //  {
            //    if (!int.TryParse(version, out ver))
            //      throw new Exception("Erreur de paramètre version");
            //  }
            //  try
            //    {
            //      var retParmISDto =
            // ObjectMapperManager.DefaultInstance.GetMapper<ParametreGenClauseDto, KpClausePar>().Map(parmIS);
            //      retParmISDto.CodeModeleIs = modeleId;
            //      retParmISDto.LeContexte = string.Empty;
            //      var isTitlesToDisplay = serviceContext.ConditionnerAffichageIs(type, codeOffre, ver, retParmISDto).ToList();
            //      if (isTitlesToDisplay != null)
            //      {
            //        isTitlesToDisplay.ForEach(isTitle => affichageIsLineDto.Add(
            //          ObjectMapperManager.DefaultInstance.GetMapper<KtIsAffiche, AffichageISLineDto>().Map(isTitle)));
            //      }
            //    }
            //    catch
            //    {
            //      isDisplayConditions = false;
            //    }
            //  }
            //  var titreElems = GetNumOrderByTitle(paramISLignesInfo, affichageIsLineDto);

            //**********************************
            var dataresTypes = dataRes.GetType();
            var properties = dataRes.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in properties)
            {
                // mettre la valeur d'affichage
                if (!propertyInfo.CanRead && propertyInfo.Name.ToUpper() == "DISPLAYLINEIS") continue;

                var lng = paramISLignesInfo.Find(el => el.InternalPropertyName == propertyInfo.Name);
                if (lng == null) continue;
                var elem = dataresTypes.GetProperty("Cells" + propertyInfo.Name);
                var elemVal = dataresTypes.GetProperty(propertyInfo.Name);
                //bool displayLine = true;
                //bool belongToLine = false;
                //string currentIdOrder = string.Empty;
                //string currentIdTitleOrder = string.Empty;
                if (elem != null)
                {
                    elem.SetValue(dataRes, lng.Cells.ToString(CultureInfo.InvariantCulture), null);
                    string val = dataRes.GetType().GetProperty(propertyInfo.Name).GetValue(dataRes, null).ToString();
                    ConvertDataToDisplayType(dataRes, lng, elemVal, val);
                    //**************traitement d'affichage des lignes IS **************************
                    //if (!isDisplayConditions)
                    //{
                    //  ConvertDataToDisplayType(dataRes, lng, elemVal, val);
                    //  continue;
                    //}

                    //foreach (var elT in titreElems.Keys.Cast<string>()
                    //  .Where(elT => elT.Contains(propertyInfo.CanRead && propertyInfo.Name.ToLower())))
                    //{
                    //  displayLine=elT.Split('_')[2] != "N";
                    //  currentIdTitleOrder = elT.Split('_')[0];
                    //  break;
                    //}
                    //if (!propertyInfo.CanRead && propertyInfo.Name.ToUpper() == "HIERARCHYORDER")
                    //{
                    //  if (titreElems.Cast<string>()
                    //    .Any(elT => elT[Convert.ToInt32(currentIdTitleOrder)].ToString(CultureInfo.InvariantCulture)
                    //    .Contains(";" + val + ";")))
                    //  {
                    //    belongToLine = true;
                    //  }
                    //}
                    ////Required
                    //if (!propertyInfo.CanRead && propertyInfo.Name.ToUpper() == "REQUIRED")
                    //{
                    //  if(!displayLine && belongToLine)
                    //    elem.SetValue(dataRes,"N");
                    //}
                    //if (!propertyInfo.CanRead && propertyInfo.Name.ToUpper() == "DISPLAYLINEIS" && belongToLine)
                    //{
                    //  elem.SetValue(dataRes, string.IsNullOrEmpty(displayLine.ToString()));
                    //}
                    //else
                    //{
                    //  //if (titreElems.Keys)
                    //  //{
                    //  //  if(val=="1") 

                    //  //  //titreElems
                    //  //}
                    //  //*****************************************************************************
                    //  // string val = dataresTypes.GetType().GetProperty(propertyInfo.Name).GetValue(dataRes, null).ToString();
                    //  ConvertDataToDisplayType(dataRes, lng, elemVal, val);
                    //}
                }
            }
        }

        /// <summary>
        ///   retourne la liste des titres et leurs sections correspondantes
        /// </summary>
        /// <param name="paramISLignesInfo"></param>
        /// <param name="affichageIsLineDto"></param>
        /// <returns>HashTable des sections grouppées par titres. Le grouppement se base sur le numéro d'affichage</returns>
        private static Hashtable GetNumOrderByTitle(List<ParamISLigneInfoDto> paramISLignesInfo, List<AffichageISLineDto> affichageIsLineDto)
        {
            var titreElems = new Hashtable();
            paramISLignesInfo.ForEach(elm =>
            {
                if (elm.HierarchyOrder != 1) return;
                AffichageISLineDto affichageISLineDto =
                  affichageIsLineDto.FirstOrDefault(elem => elem.IdLigne == elm.Code);
                if (affichageISLineDto == null)
                    return;
                titreElems.Add(elm.NumOrdreAffichage + "_" + affichageISLineDto.Libelle.ToUpper().Trim() + "_" + affichageISLineDto.Afficher, string.Empty);
                var numOrders = ";";
                paramISLignesInfo.ForEach(e =>
                {
                    if (e.NumOrdreAffichage >
                        Convert.ToInt32(elm.HierarchyOrder))
                    {
                        numOrders +=
                                     e.NumOrdreAffichage
                                       .ToString(CultureInfo.InvariantCulture) + ";";
                    }
                });
                titreElems[elm.NumOrdreAffichage] = numOrders;
            });
            ReorderElemTitles(titreElems);
            return titreElems;
        }

        /// <summary>
        /// reordonne les élements de chaque titre
        /// </summary>
        /// <param name="titreElems"></param>
        private static void ReorderElemTitles(Hashtable titreElems)
        {
            var hRes = new Hashtable();
            foreach (var titreElemNumOrder in titreElems.Keys)
            {

                var newElemsOrder = string.Empty;
                var elems = titreElems[titreElemNumOrder];
                var spltElems = elems.ToString().Split(';');
                foreach (var spltElem in spltElems)
                {
                    int order;
                    if (!int.TryParse(spltElem, out order))
                        continue;
                    if (order > Convert.ToInt32(titreElemNumOrder))
                    {
                        newElemsOrder += spltElem + ";";
                    }
                }
                hRes.Add(titreElemNumOrder, newElemsOrder);

            }
            titreElems = hRes.Keys.Count > 0 ? hRes : titreElems;
        }

        /// <summary>
        /// Convertis les types données excel  aux types de BD
        /// </summary>
        /// <param name="val"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private static string ConvertExcelDataToDbType(string val, ParamISLigneInfoDto line)
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

                    if (line.LongueurType.Contains(":"))
                    {
                        double result;
                        if (!Double.TryParse(val, NumberStyles.Any, CultureInfo.CurrentCulture, out result))
                        {
                            throw new InvalidCastException(line.TextLabel);
                        }
                        val = result.ToString();
                    }
                    else
                    {
                        Int64 result;
                        if (!Int64.TryParse(val, out result))
                        {
                            throw new InvalidCastException(line.TextLabel);
                        }
                    }

                    return val.Replace(",", ".");
                //caractérise un decimal
                case "l":
                    if (string.IsNullOrEmpty(val))
                    {
                        return "0";
                    }

                    if (line.LongueurType.Contains(":"))
                    {
                        decimal result;
                        if (!Decimal.TryParse(val, NumberStyles.Any, CultureInfo.CurrentCulture, out result))
                        {
                            throw new InvalidCastException(line.TextLabel);
                        }
                        val = result.ToString();
                    }
                    else
                    {
                        Int64 result;
                        if (!Int64.TryParse(val, out result))
                        {
                            throw new InvalidCastException(line.TextLabel);
                        }
                    }

                    return val.Replace(",", ".");
                default:
                    if (val.Equals("null"))
                    {
                        throw new ArgumentNullException(line.TextLabel);
                    }
                    return val;
            }

        }
        /// <summary>
        /// Préparation des données pour l'affichage
        /// </summary>
        /// <param name="dataRes"></param>
        /// <param name="lng"></param>
        /// <param name="elem"></param>
        /// <param name="val"></param>
        private static void ConvertDataToDisplayType(dynamic dataRes, ParamISLigneInfoDto lng, dynamic elem, string val)
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

        /// <summary>
        /// selection des données depuis la BD
        /// </summary>
        /// <param name="isInfo"></param>
        /// <param name="section"></param>
        /// <param name="modeNavig"></param>
        /// <returns></returns>
        private dynamic GetDataFromDb(ModeleISDto isInfo, string section, string modeNavig)
        {
            var sql = modeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Historique ? isInfo.SqlSelect.Replace("FROM KP", "FROM HP").Replace("FROM  KP", "FROM  HP") : isInfo.SqlSelect;
            dynamic lstRes = null;

            switch (section)
            {
                case AlbConstantesMetiers.INFORMATIONS_ENTETE:
                    lstRes = ExcelRepository.GetDataFromDB<WSAS400.DTO.ExcelDto.RS.Entete>(sql, _hsqlParam).FirstOrDefault() ?? new WSAS400.DTO.ExcelDto.RS.Entete();

                    break;
                case AlbConstantesMetiers.INFORMATIONS_RISQUES:
                    lstRes = ExcelRepository.GetDataFromDB<WSAS400.DTO.ExcelDto.RS.Risques>(sql, _hsqlParam).FirstOrDefault() ?? new WSAS400.DTO.ExcelDto.RS.Risques();
                    break;
                case AlbConstantesMetiers.INFORMATIONS_OBJETS:
                    lstRes = ExcelRepository.GetDataFromDB<WSAS400.DTO.ExcelDto.RS.Objets>(sql, _hsqlParam).FirstOrDefault() ?? new WSAS400.DTO.ExcelDto.RS.Objets();
                    break;
                case AlbConstantesMetiers.INFORMATIONS_GARANTIES:
                    lstRes = ExcelRepository.GetDataFromDB<WSAS400.DTO.ExcelDto.RS.Garanties>(sql, _hsqlParam).FirstOrDefault() ?? new WSAS400.DTO.ExcelDto.RS.Garanties();
                    break;
                case AlbConstantesMetiers.INFORMATIONS_OPTIONS:
                    break;
                case AlbConstantesMetiers.INFORMATIONS_RECUP_OBJETS:
                    lstRes = ExcelRepository.GetDataFromDB<WSAS400.DTO.ExcelDto.RS.RSRecupObjets>(sql, _hsqlParam).FirstOrDefault() ?? new WSAS400.DTO.ExcelDto.RS.RSRecupObjets();
                    break;
                case AlbConstantesMetiers.INFORMATIONS_RECUP_GARANTIES:
                    lstRes = ExcelRepository.GetDataFromDB<WSAS400.DTO.ExcelDto.RS.RSRecupGaranties>(sql, _hsqlParam).FirstOrDefault() ?? new WSAS400.DTO.ExcelDto.RS.RSRecupGaranties();
                    break;
            }

            return lstRes;
        }
        #endregion
    }
}
