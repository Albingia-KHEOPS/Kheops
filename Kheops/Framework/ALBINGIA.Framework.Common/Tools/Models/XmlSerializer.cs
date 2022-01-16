using System;
using System.IO;
using System.Xml.Serialization;

namespace ALBINGIA.Framework.Common.IOFile
{
    public class XmlSerializer<T> where T:new()
    {
        /// <summary>
        /// Déserialise un fichier XML dans ses entités correspondantes
        /// </summary>
        /// <param name="fileLongPath">Chemin du fichier</param>
        /// <returns></returns>
        public static T LoadXmlToEntity(string fileLongPath)
        {
            using (TextReader reader = new StreamReader(fileLongPath))
            {
                var serializer = new XmlSerializer(Activator.CreateInstance<T>().GetType());
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
