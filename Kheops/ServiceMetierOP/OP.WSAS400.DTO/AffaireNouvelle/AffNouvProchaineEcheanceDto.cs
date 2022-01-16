using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class AffNouvProchaineEcheanceDto
    {
        [Column(Name="PERIODICITE")]
        [DataMember]
        public string Periodicite { get; set; }
        [Column(Name = "EFFETGARANTIE")]
        [DataMember]
        public string EffetGarantie { get; set; }
        [Column(Name = "FINGARANTIE")]
        [DataMember]
        public string FinGarantie { get; set; }
        [Column(Name = "ECHPRINC")]
        [DataMember]
        public string EcheancePrincipale { get; set; }
        [Column(Name="DUREE")]
        [DataMember]
        public Int32 Duree { get; set; }
        [Column(Name = "DUREEUNITE")]
        public string DureeUnite { get; set; }
    }
}
