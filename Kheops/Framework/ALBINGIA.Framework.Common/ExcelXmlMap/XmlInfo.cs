using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ALBINGIA.Framework.Common.ExcelXmlMap
{
  [Serializable]
  [XmlRoot("Root")]
  public class XmlInfo
  {
    [XmlArray("Branche")]
    public List<XmlInfos> InfosExcel { get; set; }
  }
  [Serializable]
  [XmlType("InformationsSpecifiques")]
  public class XmlInfos
  {
    [XmlAttribute("Name")]
    public string Name { get; set; }
    [XmlArray("LignesInfos")]
    public List<ISLigneInfo> LignesInfos { get; set; }

    [XmlElement("SqlRequests")]
    public SqlRequests SqlRequests { get; set; }
  }

}
