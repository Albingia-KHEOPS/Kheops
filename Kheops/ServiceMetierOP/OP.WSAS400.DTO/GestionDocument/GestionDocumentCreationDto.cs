using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.GestionDocument
{
    [DataContract]
    public class GestionDocumentCreationDto
    {
        [DataMember]
        public string CodeDocument { get; set; }
        [DataMember]
        public string TypeDocument { get; set; }
        [DataMember]
        public List<ParametreDto> TypesDocument { get; set; }
        [DataMember]
        public string TypeCourrier { get; set; }
        [DataMember]
        public List<ParametreDto> TypesCourrier { get; set; }
        [DataMember]
        public string PieceJointe { get; set; }
        [DataMember]
        public List<GestionDocumentCourrierDto> Courriers { get; set; }
        [DataMember]
        public List<GestionDocumentCourrierDto> Emails { get; set; }

        [DataMember]
        public List<ParametreDto> TypesPartenaire { get; set; }
        [DataMember]
        public List<ParametreDto> TypesCourrierPart { get; set; }
        [DataMember]
        public List<ParametreDto> DestinatairesPart { get; set; }
    }
}
