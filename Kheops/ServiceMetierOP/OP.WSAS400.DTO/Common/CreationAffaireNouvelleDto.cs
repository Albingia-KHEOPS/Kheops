using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class CreationAffaireNouvelleDto
    {
        [DataMember]
        [Column(Name = "CODEOFFRE")]
        public string CodeOffre { get; set; }
        [DataMember]
        [Column(Name = "VERSION")]
        public Int32 Version { get; set; }
        [DataMember]
        [Column(Name = "DATESAISIE")]
        public string DateSaisie { get; set; }
        [DataMember]
        [Column(Name = "CODEBRANCHE")]
        public string CodeBranche { get; set; }
        [DataMember]
        [Column(Name = "LIBBRANCHE")]
        public string LibBranche { get; set; }
        [DataMember]
        [Column(Name = "CODECIBLE")]
        public string CodeCible { get; set; }
        [DataMember]
        [Column(Name = "LIBCIBLE")]
        public string LibCible { get; set; }
        [DataMember]
        [Column(Name = "CODEDEVISE")]
        public string CodeDevise { get; set; }
        [DataMember]
        [Column(Name = "LIBDEVISE")]
        public string LibDevise { get; set; }
        [DataMember]
        [Column(Name = "IDENTIFICATION")]
        public string Identification { get; set; }
        [DataMember]
        [Column(Name = "CODECOURTIER")]
        public Int32 CodeCourtier { get; set; }
        [DataMember]
        [Column(Name = "NOMCOURTIER")]
        public string NomCourtier { get; set; }
        [DataMember]
        [Column(Name = "CODEASSURE")]
        public Int32 CodeAssure { get; set; }
        [DataMember]
        [Column(Name = "NOMASSURE")]
        public string NomAssure { get; set; }
        [DataMember]
        [Column(Name = "CODENATCON")]
        public string CodeNatureContrat { get; set; }
        [DataMember]
        [Column(Name = "LIBNATCON")]
        public string LibNatureContrat { get; set; }
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
        public bool PossedeUnContratEnCours { get; set; }

        [DataMember]
        public List<CreationAffaireNouvelleContratDto> Contrats;



    }
}
