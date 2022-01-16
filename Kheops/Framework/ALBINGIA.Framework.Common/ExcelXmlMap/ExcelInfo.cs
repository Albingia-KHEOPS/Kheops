using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ALBINGIA.Framework.Common.ExcelXmlMap
{
     [Serializable]
    [XmlRoot("Root")]
    public class ParamBranche
     {
         [XmlArray("Branche")]
         public List<ExcelInfos> InfosExcel { get; set; }
     }
     [Serializable]
     [XmlType("Lignes")]
     public class ExcelInfos
     {
         [XmlAttribute("Name")]
         public string Name { get; set; }
         [XmlAttribute("R1")]
         public string R1 { get; set; }
         [XmlAttribute("R2")]
         public string R2 { get; set; }
         [XmlAttribute("R3")]
         public string R3 { get; set; }
         [XmlAttribute("R4")]
         public string R4 { get; set; }
         [XmlAttribute("NameVarCol")]
         public string NameVarCol { get; set; }
          
         [XmlArray("LignesInfosInputs")]
         public List<LigneInfo> LignesInfosIn { get; set; }
         [XmlArray("LignesInfosOutPuts")]
         public List<LigneInfo> LignesInfosOut { get; set; }

         [XmlElement("SqlRequests")]
         public SqlRequests SqlRequests { get; set; }
     }


   
}
