using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Configuration;

namespace ALBINGIA.Framework.Common.IOFile
{
    public static class FileContentManager
    {
        #region Méthodes publiques
        /// <summary>
        /// Retourne une valeur de config
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigValue(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static void SerializeToFile<T>(T element, string fileName)
        {
          FileStream sw = null;
          try
          {
           
            var mySerializer = new XmlSerializer(typeof(T));

            // Call the Deserialize method and cast to the object type.
         
             sw = new FileStream(fileName, FileMode.OpenOrCreate);
            mySerializer.Serialize(sw, element);
            sw.Close();
           

          }
          catch (Exception)
          {
            if (sw != null) sw.Close();

            throw;
          }
        }

        public static string SerializeToFile<T>(T element, string fileName, XmlRootAttribute xmlRoot)
        {
            // Construct an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            var mySerializer = new XmlSerializer(typeof(T), xmlRoot);
            // Call the Deserialize method and cast to the object type.

            var sw = new FileStream(fileName, FileMode.OpenOrCreate);
            mySerializer.Serialize(sw, element);
            sw.Close();
            return sw.ToString();
        }

        /// <summary>
        /// Renvoyer le contenu d'un fichier passé en paramètre sous la forme d'une chaine de caractère
        /// </summary>
        /// <param name="fileName">Chemin complet du fichier</param>
        /// <returns>Contenu du fichier</returns>
        public static string ReadContentFile(string fileName)
        {
            try
            {
                using (TextReader tr = new StreamReader(fileName))
                {
                    return tr.ReadToEnd();
                }
            }
            catch
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// Récupère le contenu d'une requête GET
        /// </summary>
        /// <param name="uriString">adresse</param>
        /// <returns></returns>
        public static string ReadHttpContent(string uriString)
        {
            try
            {
                var hwr = (HttpWebRequest)WebRequest.Create(uriString);
                hwr.UserAgent = "Albingia service";
                WebResponse wr = hwr.GetResponse();
                Stream st = wr.GetResponseStream();
                var sr = new StreamReader(st);
                string strRet = sr.ReadToEnd();
                sr.Close();
                wr.Close();
                return strRet;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Lecturue du fichier Js
        /// </summary>
        /// <param name="urlSpecificJqueryJsFile">Url du fichier Js</param>
        /// <param name="filePath">chemin du fichier</param>
        /// <returns></returns>
        public static string ReadFileJsStream(string urlSpecificJqueryJsFile, string filePath)
        {
            var sr = new StreamReader(filePath);
            string ligne = sr.ReadToEnd();
            sr.Close();
            return ligne;
        }

        /// <summary>
        /// Lecture des fichiers JS/CSS à  partir du config XML
        /// </summary>
        /// <param name="fullFilePath">Chemin complet du fichier Js/CSS</param>
        /// <param name="section">Section cible</param>
        /// <returns></returns>
        public static List<string> GetSectionJsCssFile(this string fullFilePath, string section)
        {
            var lstJquery = new List<string>();
            var filePath = fullFilePath;

            const int maxContent = 10;

            var xmlDoc = new System.Xml.XmlDocument();

            var fileMap = new ExeConfigurationFileMap {ExeConfigFilename = filePath};
            Configuration config =
                ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            int i = 0;
            string xmlString = null;
            while (xmlString != null || i < maxContent)
            {
                xmlString = null;
                if (config.GetSection(section + i.ToString(CultureInfo.CurrentCulture)) != null)
                {
                    xmlString = config.GetSection(section + i.ToString(CultureInfo.CurrentCulture)).SectionInformation.GetRawXml();
                    xmlDoc.LoadXml(xmlString);

                    var nodeList = xmlDoc.ChildNodes[0];
                    if (nodeList.Attributes != null) lstJquery.Add(nodeList.Attributes[0].Value);
                }
                i++;
            }
            return lstJquery;

        }
        #endregion
    }
}
