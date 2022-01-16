using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.GestionDocument
{
    [DataContract]
    public class GestionDocumentCourrierDto
    {
        [DataMember]
        public string CodeCourrier { get; set; }
        [DataMember]
        public string TypePartenaire { get; set; }
        [DataMember]
        public string CodePartenaire { get; set; }
        [DataMember]
        public string NomPartenaire { get; set; }
        [DataMember]
        public string Interlocuteur { get; set; }
        [DataMember]
        public string TypeCourrierPart { get; set; }
        [DataMember]
        public string NbExp { get; set; }
        [DataMember]
        public string DestinatairePart { get; set; }
    }
}
