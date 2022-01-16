using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.GestionDocument
{
    [DataContract]
    public class GestionDocumentDiffusionDto
    {
        [DataMember]
        public string TypeDiffusion { get; set; }
        [DataMember]
        public string Partenaire { get; set; }
        [DataMember]
        public string Destinataire { get; set; }
        [DataMember]
        public DateTime? CreationDate { get; set; }
        [DataMember]
        public DateTime? TraitementDate { get; set; }
    }
}
