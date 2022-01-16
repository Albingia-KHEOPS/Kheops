using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Partenaire
{
    [DataContract]
    public class PartenaireDto
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Nom { get; set; }
        [DataMember]
        public int? CodeInterlocuteur { get; set; }
        [DataMember]
        public string NomInterlocuteur { get; set; }

    }
}
