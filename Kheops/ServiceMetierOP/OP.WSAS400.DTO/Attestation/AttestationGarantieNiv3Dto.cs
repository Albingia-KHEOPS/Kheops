using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Attestation
{
    [DataContract]
    public class AttestationGarantieNiv3Dto
    {
        [DataMember]
        public Int64 IdGaran { get; set; }
        [DataMember]
        public string CodeGarantie { get; set; }
        [DataMember]
        public string LibelleGarantie { get; set; }
        [DataMember]
        public Double Montant { get; set; }
        [DataMember]
        public string Unite { get; set; }
        [DataMember]
        public string LibUnite { get; set; }
        [DataMember]
        public string Base { get; set; }
        [DataMember]
        public DateTime? DateDebut { get; set; }
        [DataMember]
        public DateTime? DateFin { get; set; }
        [DataMember]
        public bool IsShown { get; set; }
        [DataMember]
        public bool IsUsed { get; set; }
        [DataMember]
        public List<AttestationGarantieNiv4Dto> Garanties { get; set; }
    }
}
