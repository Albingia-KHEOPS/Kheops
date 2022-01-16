using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ALBINGIA.OP.OP_MVC.ExcelWeb.Common
{

    [Serializable]
    [XmlRoot("Root")]
    public class ExcelBrancheEntity
    {
        [XmlArray("Branches")]
        public List<Branche> InfosExcel { get; set; }
    }

    [Serializable]
    [XmlType("Branche")]
    public class Branche
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlAttribute("FileName")]
        public string FileName { get; set; }
    }

}