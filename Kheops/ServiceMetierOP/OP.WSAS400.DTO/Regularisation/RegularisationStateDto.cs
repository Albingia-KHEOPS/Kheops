using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class RegularisationStateDto
    {
        [DataMember]
        [Column(Name = "TYP")]
        public string Type { get; set; }

        [DataMember]
        [Column(Name = "NBV")]
        public int NbValidated { get; set; }

        [DataMember]
        [Column(Name = "NB")]
        public int NbTotal { get; set; }
    }
}
