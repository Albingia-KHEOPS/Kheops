using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.ParamIS;
using OPServiceContract.IS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
namespace ALBINGIA.OP.OP_MVC.DynamicGuiIS.Common
{
    public class DbDataEntity
    {
        #region Méthodes publiques

        public static string GetDbDefaultData(List<ParamISLigneInfo> dbLigneInfo, string split, string cut)
        {
            StringBuilder sb = new StringBuilder();
            using (var wsInfoData = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfoSpecif>())
            {
              var defaultValuesIS =   wsInfoData.Channel.GetISDefaultValueData(dbLigneInfo.Select(paramItem =>  paramItem.InternalPropertyName).ToList());

                foreach (var isLineInfo in dbLigneInfo)
                {
                    string key = string.Format("{0}||{1}", isLineInfo.InternalPropertyName, isLineInfo.DbMapCol);
                    if (defaultValuesIS.ContainsKey(key))
                        sb.Append(isLineInfo.InternalPropertyName + cut + (isLineInfo.TypeUIControl.ToLower() == "checkbox" ? defaultValuesIS[key] == "O" ? "VRAI" : "FAUX" : defaultValuesIS[key]) + split);
                }
            }

                return sb.ToString();
        }

        /// <summary>
        /// retourne les données de la base de données formatées dans une chaine de caractère.
        /// Les données sont séparées par un caractère de split. La donnée correspondant à la case excel
        /// est présenté par le couple Data+ Séparateur + Coordonnées cellule excel 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="idModele"></param>
        /// <param name="section">Section ex: Information spécifiques de base, Risques, Objets, Garanties, Options ...</param>
        /// <param name="param">Paramètres de selections de l'enregistrement dans la base</param>
        /// <param name="codeOption"></param>
        /// <param name="etapeIs"></param>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="modeNavig"></param>
        /// <param name="codeObjet"></param>
        /// <param name="codeRisque"></param>
        /// <param name="codeFormule"></param>
        /// <param name="paramForGenIs"></param>
        /// <param name="idModel">idmodel</param>
        /// <param name="isModeleEntete"></param>
        /// <returns></returns>
        //public static string GetDbData(string codeObjet, string codeRisque, string codeFormule, string codeOption, string etapeIs, string codeOffre, string version, string type, string idModele, string section, IEnumerable<object> param, ParametreGenClauseDto paramForGenIs, List<ParamISLigneInfoDto> isModelLines, List<ModeleISDto> isModeleEntete)
      public static string GetDbData(string modeNavig,string codeObjet, string codeRisque, string codeFormule, string codeOption, string etapeIs, string codeOffre, string version, string type, string idModele, string section, IEnumerable<object> param, ParametreGenClauseDto paramForGenIs, string idModel, List<ModeleISDto> isModeleEntete)
        {
          
            if (section == null && DbIOParam.IsExistSection(section)) throw new ArgumentNullException("section");
            //---------- paramètres à déterminer celon le contexte
            var hParam = DbIOParam.PrepareParameter(param).ToArray();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfoSpecif>())
            {
                var wsInfoData=client.Channel;

                //OP.WSAS400.DTO.ExcelDto.RS.Risques
                //ALBINGIA.OP.OP_MVC.Risques

              dynamic returnedData = wsInfoData.LoadData(modeNavig,codeOffre, version, type, paramForGenIs, idModele, section, MvcApplication.SPLIT_CONST_HTML, hParam,  idModel, isModeleEntete);
                
              return returnedData == null ? string.Empty : GetStrResult(returnedData);
            }
        }

      public static bool RowsExists(string section, IEnumerable<object> param,List<ModeleISDto> isModeleEntete)
        {
           var hParam = DbIOParam.PrepareParameter(param).ToArray();
          using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfoSpecif>())
          {
              var wsInfoData=client.Channel;
              return wsInfoData.RowsIsExist(section, hParam, isModeleEntete);
          }
          
        }
      /// <summary>
      /// Met à jour les données excel dans la base de données
      /// </summary>
      /// <param name="modeleId">Id du modèle</param>
      /// <param name="section">Section ex: Information spécifiques de base, Risques, Objets, Garanties, Options ...</param>
      /// <param name="param">Paramètres de selections de l'enregistrement dans la base</param>
      /// <param name="strData">Donées à mettre à jour</param>
      /// <returns>Ok si les donées sont mis à jour. Ko dans le cas échéant</returns>
        public static bool SetIsData(string modeleId, string section, string strData, IEnumerable<object> param)
        {
           
            if (section == null && DbIOParam.IsExistSection(section)) throw new ArgumentNullException("section");
            //---------- paramètres à déterminer celon le contexte
            //var hParam = new Dictionary<object, object> { { "{P_0}", "O" }, { "{P_1}", "    23812" }, { "{P_2}", "0" }, { "{P_3}", "1" }, { "{P_4}", "1" } };
            var hParam = DbIOParam.PrepareParameter(param).ToArray();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfoSpecif>())
            {
                var wsIsInfo=client.Channel;
              
              return wsIsInfo.UpdatedData(modeleId, section, MvcApplication.SPLIT_CONST_HTML, hParam, strData);

            }
        }
        public static ParametreGenClauseDto GetParamGen(string etapeIs, string modeleIs, string codeRisque, string codeObjet, string codeFormule, string codeOption)
        {
          if (!Enum.IsDefined(typeof(AlbConstantesMetiers.Etapes), etapeIs))
            return null;
          switch ((AlbConstantesMetiers.Etapes)Enum.Parse(typeof(AlbConstantesMetiers.Etapes), etapeIs, true))
          {
            case AlbConstantesMetiers.Etapes.InfoGenerale:
              return new ParametreGenClauseDto
              {
                ActeGestion = "**",
                Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale)
              };
            case AlbConstantesMetiers.Etapes.Risque:
              return new ParametreGenClauseDto
              {
                ActeGestion = "**",
                Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Risque)
                ,
                NuRisque = Convert.ToInt32(codeRisque)
                ,
                NuObjet = 0
              };
            case AlbConstantesMetiers.Etapes.Objet:
              return new ParametreGenClauseDto
              {
                ActeGestion = "**",
                Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Objet)
                ,
                NuRisque = Convert.ToInt32(codeRisque)
                ,
                NuObjet = Convert.ToInt32(codeObjet)
              };
            case AlbConstantesMetiers.Etapes.Option:
              return new ParametreGenClauseDto
              {
                ActeGestion = "**",
                Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option)
                ,
                NuRisque = Convert.ToInt32(codeRisque)
                ,
                NuObjet = Convert.ToInt32(codeObjet)
                ,
                NuFormule = Convert.ToInt32(codeFormule)
                ,
                NuOption = Convert.ToInt32(codeOption)
              };
            case AlbConstantesMetiers.Etapes.Garantie:
              return new ParametreGenClauseDto
              {
                ActeGestion = "**",
                Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Garantie)
                ,
                NuRisque = Convert.ToInt32(codeRisque)
                ,
                NuObjet = Convert.ToInt32(codeObjet)
                ,
                NuFormule = Convert.ToInt32(codeFormule)
                ,
                NuOption = Convert.ToInt32(codeOption)
              };
          }
          return null;
        }
        #endregion
        #region Métodes privées
     
        private static string GetStrResult(dynamic returnedData)
        {
            var strBuilder = new StringBuilder();
            var propsData = returnedData.GetType();
            foreach (var propDataInfo in propsData.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
              if (propDataInfo.Name.ToUpper() == "DISPLAYLINEIS" || !propDataInfo.CanRead || propDataInfo.Name.Contains("Cells") || propDataInfo.Name.Contains("ExtensionData"))
                    continue;
                var val = propDataInfo.GetValue(returnedData, null);
                var cell = propsData.GetProperty("Cells" + propDataInfo.Name).GetValue(returnedData, null);
                strBuilder.Append(cell + "||" + val + MvcApplication.SPLIT_CONST_HTML);
            }
            return strBuilder.ToString();
        }
        #endregion
    }
}