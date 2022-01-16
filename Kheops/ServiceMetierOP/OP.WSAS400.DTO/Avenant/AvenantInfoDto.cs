using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Avenant
{
    [DataContract]
    public class AvenantInfoDto
    {
        [DataMember]
        public ContratDto Avenant { get; set; }
        [DataMember]
        public bool ExistEcheancier { get; set; }
        [DataMember]
        public List<ParametreDto> MotsClef { get; set; }
        [DataMember]
        public List<ParametreDto> RegimesTaxe { get; set; }
        [DataMember]
        public List<ParametreDto> Antecedents { get; set; }
        [DataMember]
        public List<ParametreDto> Stops { get; set; }
        [DataMember]
        public List<ParametreDto> Periodicites { get; set; }
        [DataMember]
        public List<ParametreDto> Durees { get; set; }
        [DataMember]
        public List<ParametreDto> Indices { get; set; }
        [DataMember]
        public List<ParametreDto> NaturesContrat { get; set; }
        [DataMember]
        public List<ParametreDto> Motifs { get; set; }
        [DataMember]
        public string PeriodiciteHisto { get; set; }
        [DataMember]
        public DateTime? ProchEchHisto { get; set; }
        [DataMember]
        public DateTime? DebPeriodHisto { get; set; }
        //public List<ParametreDto> Devises { get; set; }
        [DataMember]
        public bool HasOppBenef { get; set; }

        [DataMember]
        public string EtatHisto { get; set; }
        [DataMember]
        public string SituationHisto { get; set; }
    }
}
