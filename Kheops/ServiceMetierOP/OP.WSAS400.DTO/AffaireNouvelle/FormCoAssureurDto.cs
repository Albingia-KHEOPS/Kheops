using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class FormCoAssureurDto
    {
        [DataMember]
        [Column(Name = "PARTALBINGIA")]
        public Single PartAlbingia { get; set; }
        [DataMember]
        [Column(Name = "PARTCOUVERTE")]
        public Single PartCouverte { get; set; }

        [DataMember]
        [Column(Name = "DATEEFFETAVNJOUR")]
        public int DateEffetAvnJour { get; set; }

        [DataMember]
        [Column(Name = "DATEEFFETAVNMOIS")]
        public int DateEffetAvnMois { get; set; }

        [DataMember]
        [Column(Name = "DATEEFFETAVNANNEE")]
        public int DateEffetAvnAnnee { get; set; }


        [DataMember]
        public List<CoAssureurDto> ListeCoAssureurs { get; set; }
        [DataMember]
        public bool IsTraceAvnExist { get; set; }
        [DataMember]
        public DateTime? DateEffetAvenantModificationLocale { get; set; }
    }
}
