using System;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Common
{
    public class VisuInfosContratDto
    {

        [DataMember]
        [Column(Name="CODEOFFRE")]
        public string CodeOffre { get; set; }

        [DataMember]
        [Column(Name="VERSION")]
        public int Version { get; set; }

        [DataMember]
        public DateTime? DateDebutEffet { get; set; }
        [DataMember]
        public DateTime? DateFinEffet { get; set; }
        [DataMember]
        public DateTime? DateEcheance { get; set; }

        [DataMember]
        [Column(Name = "PERIODICITE")]
        public string Periodicite { get; set; }
        [DataMember]
        [Column(Name = "COURTIERA")]
        public string CourtierA { get; set; }
        [DataMember]
        [Column(Name = "COURTIERG")]
        public string CourtierG { get; set; }
        [DataMember]
        [Column(Name = "ASSURE")]
        public string Assure { get; set; }
        
        [DataMember]
        [Column(Name = "IDENTIF")]
        public string Identification { get; set; }

        [DataMember]
        [Column(Name = "NATURECONTRAT")]
        public string NatureContrat { get; set; }

        [DataMember]
        [Column(Name = "TYPECONTRAT")]
        public string TypeContrat   { get; set; }

        [DataMember]
        [Column(Name = "SOUSCRIPTEUR")]
        public string Souscripteur { get; set; }

        [DataMember]
        [Column(Name = "GESTIONNAIRE")]
        public string Gestionnaire { get; set; }

        [DataMember]
        [Column(Name = "OBSERVATION")]
        public string Observation { get; set; }

        [DataMember]
        [Column(Name = "RETSIGNE")]
        public string RetourSigne { get; set; }





        [Column(Name = "DATEDEBUTEFFET")]
        public long? DateDebutEffetInt { get; set; }

        [Column(Name = "DATEFINEFFET")]
        public long? DateFinEffetInt { get; set; }

        [Column(Name = "DATEECHEANCE")]
        public long? DateEcheanceInt { get; set; }
    }
}
