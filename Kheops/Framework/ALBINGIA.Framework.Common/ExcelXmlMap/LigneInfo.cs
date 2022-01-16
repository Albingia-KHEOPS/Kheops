using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace ALBINGIA.Framework.Common.ExcelXmlMap
{
    //[Serializable]
    //public class LignesInfos
    //{
    //    [XmlArray("LignesInfos")]
    //    public List<LigneInfo> LignesInfos { get; set; }
    //}
    [DataContract]
    [Serializable]
    [XmlType("LigneInfo")]
    public class LigneInfo
    {
        [DataMember]
        [XmlAttribute("SqlOrder")]
        public string SqlOrder { get; set; }
        [DataMember]
        [XmlAttribute("Lib")]
        public string Lib { get; set; }
        [XmlAttribute("Cells")]
        public string Cells { get; set; }
        [DataMember]
        [XmlAttribute("DbMapCol")]
        public string DbMapCol { get; set; }
         [DataMember]
        [XmlAttribute("type")]
        public string Type { get; set; }
       [DataMember]
        [XmlAttribute("ConvertTo")]
        public string ConvertTo { get; set; }
    }
    [Serializable]
    [XmlType("SqlRequests")]
    public class SqlRequests
    {
        [XmlElement("SelectExistOS")]
        public Request SelectExistOS { get; set; }
        [XmlElement("SelectOSEncours")]
        public Request SelectOSEncours { get; set; }
        [XmlElement("SelectExist")]
        public Request SelectExist { get; set; }
        [XmlElement("Select")]
        public Request Select { get; set; }
        [XmlElement("Insert")]
        public Request Insert { get; set; }
        [XmlElement("Update")]
        public Request Update { get; set; }
        [XmlElement("SelectOutPut")]
        public Request SelectOutPut { get; set; }
    }
    [Serializable]
    public class Request
    {
        [XmlAttribute("Sql")]
        public string Sql { get; set; }
    }
}
