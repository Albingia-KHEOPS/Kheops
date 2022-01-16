using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.SMP
{
    [DataContract]
    public class LigneSMPdto
    {
        [DataMember]
        [Column(Name = "IdGarantie")]
        public Int64 IdGarantie { get; set; }

        [DataMember]
        [Column(Name = "CheckBox")]
        public string CheckBox { get; set; }

        [DataMember]
        [Column(Name = "Garantie")]
        public string NomGarantie { get; set; }

        [DataMember]
        public string LCI { get; set; }

        [DataMember]
        [Column(Name = "SMPcalcule")]
        public Int64 SMPcalcule { get; set; }

        [DataMember]
        [Column(Name = "TypeSMPforce")]
        public string Type { get; set; }

        [DataMember]
        [Column(Name = "ValeurSMPforce")]
        public Int64 Valeur { get; set; }

        [DataMember]
        [Column(Name = "SMPretenu")]
        public Int64 SMPretenu { get; set; }
    }
}
