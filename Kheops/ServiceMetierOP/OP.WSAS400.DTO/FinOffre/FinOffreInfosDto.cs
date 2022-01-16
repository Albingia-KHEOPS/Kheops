using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.FinOffre
{
    [DataContract]
    public class FinOffreInfosDto
    {
        [DataMember]
        public List<ParametreDto> Antecedents { get; set; }
        [DataMember]
        public string Antecedent { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int ValiditeOffre { get; set; }
        [DataMember]
        public DateTime? DateProjet { get; set; }
        [DataMember]
        public DateTime? DateStatistique { get; set; }
        [DataMember]
        public bool Relance { get; set; }
        [DataMember]
        public int RelanceValeur { get; set; }
        [DataMember]
        public int Preavis { get; set; }

        public FinOffreInfosDto()
        {
            Antecedents = new List<ParametreDto>();
            Description = string.Empty;
            ValiditeOffre = 0;
            DateProjet = new DateTime();
            DateStatistique = new DateTime();
            Relance = false;
            RelanceValeur = 0;
            Preavis = 0;
        }
    }
}
