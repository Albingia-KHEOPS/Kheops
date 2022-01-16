using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.MatriceFormule
{
    [DataContract]
    public class MatriceFormuleFormDto
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Libelle { get; set; }
        [DataMember]
        public string Designation { get; set; }
        [DataMember]
        public List<MatriceFormuleOptDto> Options { get; set; }
        [DataMember]
        public string NumeroChrono { get; set; }
        [DataMember]
        public bool IsAlertePeriode { get; set; }
        [DataMember]
        public bool IsAvenantModificationLocale { get; set; }
        [DataMember]
        public DateTime? DateEffetAvenantModificationLocale { get; set; }
        [DataMember]
        public int AvnCreationFor { get; set; }
        [DataMember]
        public Int64 CodeRsq { get; set; }
        [DataMember]
        public bool BlockFormConditions { get; set; }
        [DataMember]
        public bool SupprForm { get; set; }
    }
}
