using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.MontantReference
{
    [DataContract]
    public class MontantReferenceInfoDto
    {
        [DataMember]
        [Column(Name = "LETTREFORM")]
        public string LettreForm { get; set; }
        [DataMember]
        [Column(Name = "CODEFORM")]
        public Int64 CodeForm { get; set; }
        [DataMember]
        [Column(Name = "LIBFORM")]
        public string LibFormule { get; set; }
        [DataMember]
        [Column(Name = "CODERSQ")]
        public Int64 CodeRsq { get; set; }
        [DataMember]
        [Column(Name = "LIBRSQ")]
        public string LibRisque { get; set; }
        [DataMember]
        [Column(Name = "MNTCALCULE")]
        public double MontantCalcule { get; set; }
        [DataMember]
        [Column(Name = "MNTFORCE")]
        public double MontantForce { get; set; }
        [DataMember]
        [Column(Name="MNTPROVI")]
        public string MontantProvisionnel { get; set; }
        [DataMember]
        [Column(Name = "MNTACQUIS")]
        public double MontantAcquis { get; set; }
        [DataMember]
        [Column(Name="CHKMNTACQUIS")]
        public string ChkMntAcquis { get; set; }
    }
}
