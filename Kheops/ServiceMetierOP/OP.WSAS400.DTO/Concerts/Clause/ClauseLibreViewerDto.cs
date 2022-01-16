using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;

namespace OP.WSAS400.DTO.Concerts.Clause
{
    public class ClauseLibreViewerDto
    {
        [Column(Name="CODEOFFRE")]
        public string CodeOffre { get; set; }
        [Column(Name="VERSION")]
        public Int32 Version { get; set; }
        [Column(Name="TYPE")]
        public string Type { get; set; }
        [DataMember]
        [Column(Name="CONTEXTE")]
        public string Contexte { get; set; }
        [DataMember]
        [Column(Name="LIBCONTEXTE")]
        public string LibContexte { get; set; }
        [DataMember]
        [Column(Name="EMPLACEMENT")]
        public string Emplacement { get; set; }
        [DataMember]
        [Column(Name="SOUSEMPLACEMENT")]
        public string SousEmplacement { get; set; }
        [DataMember]
        [Column(Name="ORDRE")]
        public Single Ordre { get; set; }
        [DataMember]
        [Column(Name="CODERSQ")]
        public Int32 CodeRisque { get; set; }
        [DataMember]
        [Column(Name="CODEOBJ")]
        public Int32 CodeObjet { get; set; }
        [DataMember]
        [Column(Name="IDDOC")]
        public Int64 IdDoc { get; set; }
        [DataMember]
        public string AppliqueA { get; set; }
        [DataMember]
        [Column(Name = "LEDES")]
        public string Titre { get; set; }
        [DataMember]
        [Column(Name="USERAJT")]
        public string UserAjt { get; set; }
        [DataMember]
        [Column(Name = "TYPEAJOUT")]
        public string TypeAjout { get; set; }
        [DataMember]
        public List<ParametreDto> Emplacements { get; set; }
        [DataMember]
        public RisqueDto Risque { get; set; }
        [DataMember]
        public bool Modifiable { get; set; }
        [DataMember]
        [Column(Name = "DATEDEBUT")]
        public Int32 DateDebut { get; set; }
        [DataMember]
        [Column(Name = "DATEFIN")]
        public Int32 DateFin { get; set; }
    }
}
