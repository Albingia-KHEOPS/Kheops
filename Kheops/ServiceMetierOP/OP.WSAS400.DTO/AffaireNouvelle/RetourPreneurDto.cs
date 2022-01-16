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
    public class RetourPreneurDto
    {
        [DataMember]
        [Column(Name = "DATERETOUR")]
        public int DateRetour { get; set; }

        [DataMember]
        [Column(Name = "TYPEACCORD")]
        public string TypeAccord { get; set; }

        [DataMember]
        [Column(Name = "ISREGLEMENTRECU")]
        public string IsReglementRecu { get; set; }

        [DataMember]
        [Column(Name = "MNTREGLEMENT")]
        public double MntReglement { get; set; }

    }
}
