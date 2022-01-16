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
  public class LigneInfoold
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
    [XmlAttribute("Link")]
    public bool Link { get; set; }
    [DataMember]
    [XmlAttribute("type")]
    public string Type { get; set; }
    [DataMember]
    [XmlAttribute("SqlRequest")]
    public string SqlRequest { get; set; }
    [DataMember]
    [XmlAttribute("ConvertTo")]
    public string ConvertTo { get; set; }
    [DataMember]
    [XmlAttribute("HierarchyOrder")]
    public int HierarchyOrder { get; set; }
    [DataMember]
    [XmlAttribute("LineBreak")]
    public string LineBreak { get; set; }
    [DataMember]
    [XmlAttribute("TypeUIControl")]
    public string TypeUIControl { get; set; }
    [DataMember]
    [XmlAttribute("Required")]
    public string Required { get; set; }
    [DataMember]
    [XmlAttribute("TextLabel")]
    public string TextLabel { get; set; }
    [DataMember]
    [XmlAttribute("LinkBehaviour")]
    public string LinkBehaviour { get; set; }
    [DataMember]
    [XmlAttribute("Behaviour")]
    public string Behaviour { get; set; }
    [DataMember]
    [XmlAttribute("EventBehaviour")]
    public string EventBehaviour { get; set; }
    [DataMember]
    [XmlAttribute("Disabled")]
    public string Disabled { get; set; }



  }
  [Serializable]
  [XmlType("SqlRequests")]
  public class ISSqlRequests
  {
    [XmlElement("SelectExist")]
    public Request SelectExist { get; set; }
    [XmlElement("Select")]
    public Request Select { get; set; }
    [XmlElement("Insert")]
    public Request Insert { get; set; }
    [XmlElement("Update")]
    public Request Update { get; set; }
  }
  [Serializable]
  public class ISRequest
  {
    [XmlAttribute("Sql")]
    public string Sql { get; set; }
  }
}
