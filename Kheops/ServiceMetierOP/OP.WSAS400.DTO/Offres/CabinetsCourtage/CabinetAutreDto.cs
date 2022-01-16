using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Offres.CabinetsCourtage
{
    [DataContract]
    public class CabinetAutreDto
    {
        [DataMember]
        public string CodeOffre { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public string  Type { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Courtier { get; set; }
        [DataMember]
        public string Delegation { get; set; }
        [DataMember]
        public DateTime? EnregistrementDate { get; set; }
        [DataMember]
        public TimeSpan? EnregistrementHeure { get; set; }
        [DataMember]
        public DateTime? SaisieDate { get; set; }
        [DataMember]
        public TimeSpan? SaisieHeure { get; set; }
        [DataMember]
        public string Souscripteur { get; set; }
        [DataMember]
        public string CodeSouscripteur { get; set; }
        [DataMember]
        public string Action { get; set; }
        [DataMember]
        public string Motif { get; set; }
        [DataMember]
        public string LibelleMotif { get; set; }
        [DataMember]
        public string MotifRefusGestionnaire { get; set; }
        [DataMember]
        public string MotifRefusDemandeur { get; set; }
        [DataMember]
        public string Interlocuteur { get; set; }
        [DataMember]
        public string Reference { get; set; }
    }
}
