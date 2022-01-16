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
    public class RetourCoassureurDto
    {
        [DataMember]
        [Column(Name = "GUIDID")]
        public string GuidId { get; set; }

        [DataMember]
        [Column(Name = "NOMCOASSUREUR")]
        public string NomCoassureur { get; set; }

        [DataMember]
        [Column(Name = "PARTCOASSUREUR")]
        public float Part { get; set; }

        [DataMember]
        [Column(Name = "DATERETOUR")]
        public int DateRetourCoAss { get; set; }

        [DataMember]
        [Column(Name = "TYPEACCORD")]
        public string TypeAccordVal { get; set; }

        [DataMember]
        public string SDateRetourCoAss { get; set; }
    }
}
