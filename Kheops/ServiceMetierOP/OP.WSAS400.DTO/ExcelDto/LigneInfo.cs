using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using EmitMapper;



namespace OP.WSAS400.DTO.ExcelDto
{
    [DataContract]
    public class LigneInfo
    {
        [DataMember]

        public string SqlOrder { get; set; }
        [DataMember]

        public string Lib { get; set; }
         [DataMember]
        public string Cells { get; set; }
        [DataMember]

        public string DbMapCol { get; set; }
        [DataMember]

        public bool Link { get; set; }

        [DataMember]
        public string Type { get; set; }
        [DataMember]

        public string SqlRequest { get; set; }
        [DataMember]

        public string ConvertTo { get; set; }
        [DataMember]

        public int HierarchyOrder { get; set; }
        [DataMember]

        public string LineBreak { get; set; }
        [DataMember]

        public string TypeUIControl { get; set; }
        [DataMember]

        public string Required { get; set; }
        [DataMember]

        public string TextLabel { get; set; }
        [DataMember]

        public string LinkBehaviour { get; set; }
        [DataMember]

        public string Behaviour { get; set; }
        [DataMember]

        public string EventBehaviour { get; set; }
        [DataMember]

        public string Disabled { get; set; }

        public static explicit  operator LigneInfo(ALBINGIA.Framework.Common.ExcelXmlMap.LigneInfo fLngInfo)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ALBINGIA.Framework.Common.ExcelXmlMap.LigneInfo, LigneInfo>().Map(fLngInfo);
        }
    }
}
