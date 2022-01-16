using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class CourtierDto
    {
        [DataMember]
        [Column(Name = "CODE")]
        public int CodeCourtier { get; set; }
        [DataMember]
        [Column(Name = "NOM")]
        public string NomCourtier { get; set; }
        [DataMember]
        [Column(Name = "CP")]
        public string CodePostal { get; set; }
        [DataMember]
        [Column(Name = "COMMISSION")]
        public Single Commission { get; set; }
        [DataMember]
        [Column(Name = "APPORTEUR")]
        public int CodeApporteur { get; set; }
        [DataMember]
        [Column(Name = "GEST")]
        public int CodeGest { get; set; }
        [DataMember]
        [Column(Name = "PAYEUR")]
        public int CodePayeur { get; set; } 
    }
}
