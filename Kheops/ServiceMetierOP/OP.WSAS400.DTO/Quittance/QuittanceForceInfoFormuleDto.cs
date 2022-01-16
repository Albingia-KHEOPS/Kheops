using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Quittance
{
    [DataContract]
    public class QuittanceForceInfoFormuleDto
    {
        [Column(Name = "CODEFOR")]
        [DataMember]
        public Int64 CodeFor { get; set; }
        [Column(Name = "FORMLETTRE")]
        [DataMember]
        public string FormLettre { get; set; }
        [Column(Name = "FORMDESC")]
        [DataMember]
        public string FormDesc { get; set; }
        [Column(Name = "CODERSQ")]
        [DataMember]
        public int CodeRsq { get; set; }
        [Column(Name = "RSQDESC")]
        [DataMember]
        public string RsqDesc { get; set; }
        [Column(Name = "MONTANTCAL")]
        [DataMember]
        public double MontantCal { get; set; }
        [Column(Name = "MONTANTFORCE")]
        [DataMember]
        public double MontantForce { get; set; }
        [Column(Name = "NUMFOR")]
        [DataMember]
        public int NumeroFormule { get; set; }


    }
}
