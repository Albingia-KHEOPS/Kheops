using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.SuiviDocuments
{
    [DataContract]
    public class SuiviDocumentsDto
    {
        [DataMember]
        public string Situation { get; set; }
        [DataMember]
        public List<ParametreDto> Situations { get; set; }
        [DataMember]
        public string TypeDestinataire { get; set; }
        [DataMember]
        public List<ParametreDto> TypesDestinataire { get; set; }
    }
}
