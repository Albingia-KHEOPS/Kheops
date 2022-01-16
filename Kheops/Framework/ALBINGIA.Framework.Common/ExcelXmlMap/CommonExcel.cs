 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
 using ALBINGIA.Framework.Common.IOFile;
using ALBINGIA.Framework.Common.AlbingiaExceptions;

namespace ALBINGIA.Framework.Common.ExcelXmlMap
{
    public static class CommonExcel
    {
      private const string _pathFile = @"C:\Parametrage\";

      /// <summary>
      /// Sérialize le fichier xml dans l'objet correspondant
      /// </summary>
      /// <param name="xmlParamExcel"> </param>
      /// <param name="typeInfo"></param>
      /// <returns></returns>
      public static ParamBranche GetControlsFromXml(string xmlParamExcel, string typeInfo)
      {
        ParamBranche exInf;
        var fileLongPath = GetExcelXmlPathDto(xmlParamExcel, typeInfo);
        if (string.IsNullOrEmpty(fileLongPath))
          return null;
        using (TextReader reader = new StreamReader(fileLongPath))
        {
          var serializer = new XmlSerializer(typeof(ParamBranche));
          exInf = (ParamBranche)serializer.Deserialize(reader);
        }
        return exInf;

      }

      /// <summary>
      /// Récupère les lignes de paramétrage celon la branche et la section
      /// </summary>
      /// <param name="typeInfo">TypeInfo:Prefix Name File</param>
      /// <param name="branche">Branche</param>
      /// <returns></returns>
        public static List<LigneInfo> GetLignesInfos(string typeInfo, string branche)
      {
        var lngInfosSection = GetControlsFromXmlDto(typeInfo);

          //.InfosExcel.FirstOrDefault(e => e.Name.Contains(branche));
         if(lngInfosSection == null || lngInfosSection.InfosExcel == null)
         {
           var lngInfos = lngInfosSection.InfosExcel.FirstOrDefault(e => e.Name.Contains(branche));

           return lngInfos == null ? null : lngInfos.LignesInfosIn;
         }
        return null;
      }

        /// <summary>
        /// Méthode utilisée pour la génération automatique des DTOs
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        private static ParamBranche GetControlsFromXmlDto( string typeInfo)
        {
            ParamBranche exInf;
           var fileLongPath = GetExcelXmlPathDto(_pathFile, typeInfo);
            //var fileLongPath = GetExcelXmlPathDTO(HttpContext.Current.Server.MapPath("~/Parametrage/"), branche);
            if (string.IsNullOrEmpty(fileLongPath))
                return null;
            using (TextReader reader = new StreamReader(fileLongPath))
            {
                var serializer = new XmlSerializer(typeof(ParamBranche));
                exInf = (ParamBranche)serializer.Deserialize(reader);
            }
            return exInf;



        }

       /// <summary>
        ///  Méthode utilisée pour la génération automatique des DTOs IS
       /// </summary>
       /// <param name="typeInfo"></param>
       /// <returns></returns>
        public static XmlInfo GetControlsFromXmlDtoIS(string typeInfo)
        {
          XmlInfo exInf;
          var fileLongPath = GetExcelXmlPathDto(_pathFile, typeInfo);
          //var fileLongPath = GetExcelXmlPathDTO(HttpContext.Current.Server.MapPath("~/Parametrage/"), branche);
          if (string.IsNullOrEmpty(fileLongPath))
            return null;
          using (TextReader reader = new StreamReader(fileLongPath))
          {
            var serializer = new XmlSerializer(typeof(XmlInfo));
            exInf = (XmlInfo)serializer.Deserialize(reader);
          }
          return exInf;



        }

      /// <summary>
      /// Sauvegarde les données de paramétrage dans la structure XML ParamBranche
      /// </summary>
      /// <param name="exclInfo"></param>
      /// <param name="branche"></param>
      /// <param name="section"></param>
      public static void SetDataToXml(ExcelInfos exclInfo, string branche,string section)
      {
        try
        {
          var fileLongPath = GetExcelXmlPathDto(_pathFile, branche);
          //FileContentManager.SerializeToFile(exclInfo, fileLongPath);
          var xmlBrancheParam = GetControlsFromXmlDto(branche) ??
                                new ParamBranche {InfosExcel = new List<ExcelInfos> {exclInfo}};
          if (xmlBrancheParam.InfosExcel == null || xmlBrancheParam.InfosExcel.Count == 0)
            xmlBrancheParam.InfosExcel = new List<ExcelInfos> {exclInfo};
         
          var index = xmlBrancheParam.InfosExcel.FindIndex(0, xmlBrancheParam.InfosExcel.Count, el => el.Name == section);
          xmlBrancheParam.InfosExcel.Remove(xmlBrancheParam.InfosExcel.FirstOrDefault(el=>el.Name==section));
          xmlBrancheParam.InfosExcel.Insert(index,exclInfo);

          File.Copy(fileLongPath, fileLongPath.Replace(branche,"SAVE_"+branche).Replace(".xml",".OLDXML").ToUpper(),true);
          File.Delete(fileLongPath);
          FileContentManager.SerializeToFile(xmlBrancheParam, fileLongPath);

        }
        catch
        {
          
          throw new AlbFoncException(string.Format("Impossible de sérialiser les paramètres IS dela branche {0}", branche),sendMail:true,trace:true); 
          
        }
      }

        /// <summary>
        ///  Méthode utilisée pour la génération automatique des DTOs
        /// </summary>
        /// <param name="xmlParmExcel"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetExcelXmlPathDto(string xmlParmExcel, string type)
        {


          string filePath = xmlParmExcel + type + "_PARAM.xml";

            if (File.Exists(filePath))
                return filePath;
          throw new Exception(string.Format("Le ficher {0} est inexistant",filePath));
        }

        
    }
}
