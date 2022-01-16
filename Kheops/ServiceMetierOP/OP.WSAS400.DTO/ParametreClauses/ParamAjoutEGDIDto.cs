using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.ParametreClauses
{
    [DataContract]
    public class ParamAjoutEGDIDto : ParamClauses_Base
    {
        [Column(Name="TYPE")]
        public string Type { get; set; }
        [DataMember]
        [Column(Name="CODEEGDI")]
        public string CodeEGDI { get; set; }
        [DataMember]
        [Column(Name="NUMORDRE")]
        public Int64 NumOrd { get; set; }
        [DataMember]
        [Column(Name="LIBELLEEGDI")]
        public string LibelleEGDI { get; set; }
        [DataMember]
        [Column(Name = "COMMENTAIRES")]
        public string Commentaires { get; set; }
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public int ModeSaisie { get; set; }
        [DataMember]
        public string CodeService { get; set; }
        [DataMember]
        public string LibelleService { get; set; }
        [DataMember]
        public string CodeActGes { get; set; }
        [DataMember]
        public string LibelleActGes { get; set; }
        [DataMember]
        public string CodeEtape { get; set; }
        [DataMember]
        public string LibelleEtape { get; set; }
        [DataMember]
        public string CodeContexte { get; set; }
        [DataMember]
        public string LibelleContexte { get; set; }
        [DataMember]
        public bool EG { get; set; }
        [DataMember]
        public bool DI { get; set; }
    }
}
