using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.NavigationArbre
{
    [DataContract]
    public class TraceDto
    {
        [Column(Name = "KEVIPB")]
        [DataMember]
        public string CodeOffre { get; set; }

        [Column(Name = "KEVALX")]
        [DataMember]
        public int Version { get; set; }

        [Column(Name = "KEVTYP")]
        [DataMember]
        public string Type { get; set; }

        [Column(Name = "KEVETAPE")]
        [DataMember]
        public string EtapeGeneration { get; set; }

        [Column(Name = "KEVETORD")]
        [DataMember]
        public int NumeroOrdreEtape { get; set; }

        [Column(Name = "KEVORDR")]
        [DataMember]
        public int NumeroOrdreDansEtape { get; set; }

        [Column(Name = "KEVPERI")]
        [DataMember]
        public string Perimetre { get; set; }

        [Column(Name = "KEVRSQ")]
        [DataMember]
        public int Risque { get; set; }

        [Column(Name = "KEVOBJ")]
        [DataMember]
        public int Objet { get; set; }

        [Column(Name = "KEVKBEID")]
        [DataMember]
        public Int64 IdInventaire { get; set; }

        [Column(Name = "KEVFOR")]
        [DataMember]
        public int Formule { get; set; }

        [Column(Name = "KEVOPT")]
        [DataMember]
        public int Option { get; set; }

        [Column(Name = "KEVNIVM")]
        [DataMember]
        public string Niveau { get; set; }

        [Column(Name = "KEVCRU")]
        [DataMember]
        public string CreationUser { get; set; }

        [Column(Name = "KEVCRD")]
        [DataMember]
        public int CreationDate { get; set; }

        [Column(Name = "KEVCRH")]
        [DataMember]
        public int CreationHeure { get; set; }

        [Column(Name = "KEVMAJU")]
        [DataMember]
        public string MAJuser { get; set; }

        [Column(Name = "KEVMAJD")]
        [DataMember]
        public int MAJDate { get; set; }

        [Column(Name = "KEVMAJH")]
        [DataMember]
        public int MAJheure { get; set; }

        [Column(Name="KEVTAG")]
        [DataMember]
        public string PassageTag { get; set; }

        [Column(Name = "KEVTAGC")]
        [DataMember]
        public string PassageTagClause { get; set; }

        public TraceDto()
        {
            CodeOffre = string.Empty;
            Version = 0;
            Type = string.Empty;
            EtapeGeneration = string.Empty;
            NumeroOrdreEtape = 0;
            NumeroOrdreDansEtape = 0;
            Perimetre = string.Empty;
            Risque = 0;
            Objet = 0;
            IdInventaire = 0;
            Formule = 0;
            Option = 0;
            Niveau = string.Empty;
            CreationUser = string.Empty;
            CreationDate = 0;
            CreationHeure = 0;
            MAJuser = string.Empty;
            MAJDate = 0;
            MAJheure = 0;
            PassageTag = "N";
            PassageTagClause = string.Empty;
        }
    }
}
