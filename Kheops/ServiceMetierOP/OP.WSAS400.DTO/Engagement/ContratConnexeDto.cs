using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Engagement
{
    [DataContract]
    public class ContratConnexeDto
    {
        [DataMember]
        [Column(Name = "NUMCONNEXITE")]
        public int NumConnexite { get; set; }

        [DataMember]
        [Column(Name = "IDECONNEXITE")]
        public long IdeConnexite { get; set; }

        [DataMember]
        [Column(Name = "CODETYPECONNEXITE")]
        public string CodeTypeConnexite { get; set; }

        [DataMember]
        [Column(Name = "NUMCONTRAT")]
        public string NumContrat { get; set; }

        [DataMember]
        [Column(Name = "VERSIONCONTRAT")]
        public short VersionContrat { get; set; }

        [DataMember]
        [Column(Name = "TYPECONTRAT")]
        public string TypeContrat { get; set; }

        [DataMember]
        [Column(Name = "DESCRIPTIONCONTRAT")]
        public string DescriptionContrat { get; set; }

        [DataMember]
        [Column(Name = "CODEBRANCHE")]
        public string CodeBranche { get; set; }

        [DataMember]
        [Column(Name = "PBSBR")]
        public string CodeSousBranche { get; set; }

        [DataMember]
        [Column(Name = "PBCAT")]
        public string CodeCategorie { get; set; }

        [DataMember]
        [Column(Name = "LIBELLEBRANCHE")]
        public string LibelleBranche { get; set; }

        [DataMember]
        [Column(Name = "CODEPRENEUR")]
        public int CodePreneur { get; set; }

        [DataMember]
        [Column(Name = "NOMPRENEUR")]
        public string NomPreneur { get; set; }

        [DataMember]
        [Column(Name = "CODECIBLE")]
        public string CodeCible { get; set; }

        [DataMember]
        [Column(Name = "LIBELLECIBLE")]
        public string LibelleCible { get; set; }

        [DataMember]
        [Column(Name = "OBSERVATION")]
        public string Observation { get; set; }
        [DataMember]
        [Column(Name = "CODEOBSERVATION")]
        public long CodeObservation { get; set; }

        [DataMember]
        [Column(Name = "AD1")]
        public string Adresse1 { get; set; }

        [DataMember]
        [Column(Name = "AD2")]
        public string Adresse2 { get; set; }

        [DataMember]
        [Column(Name = "DEP")]
        public string Departement { get; set; }

        [DataMember]
        [Column(Name = "CP")]
        public string CodePostal { get; set; }

        [DataMember]
        [Column(Name = "VILLE")]
        public string Ville { get; set; }

        [DataMember]
        [Column(Name = "ETAT")]
        public string Etat { get; set; }

        [DataMember]
        [Column(Name = "SITUATION")]
        public string Situation { get; set; }
        [DataMember]
        [Column(Name = "CODEENGAGEMENT")]
        public string CodeEngagement { get; set; }
        [DataMember]
        [Column(Name = "VALEURENGAGEMENT")]
        public long ValeurEngagement { get; set; }
        [DataMember]
        [Column(Name = "TOTALENGAGEMENT")]
        public long TotalEngagement { get; set; }
    }
}
