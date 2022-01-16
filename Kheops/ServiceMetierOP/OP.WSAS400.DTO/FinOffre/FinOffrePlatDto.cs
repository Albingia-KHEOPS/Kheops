using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FinOffre
{
    public class FinOffrePlatDto
    {
        [DataMember]
        [Column(Name = "ANTECEDENT")]
        public string Antecedent { get; set; }
        [DataMember]
        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }
        [DataMember]
        [Column(Name = "VALIDITEOFFRE")]
        public int ValiditeOffre { get; set; }
        [DataMember]
        [Column(Name = "DATEPROJET")]
        public Int32 DateProjet { get; set; }
        [DataMember]
        [Column(Name = "DATESTATISTIQUE")]
        public Int32 DateStatistique { get; set; }
        [DataMember]
        [Column(Name = "DATESTATISTIQUEJOUR")]
        public Int32 DateStatistiqueJour { get; set; }
        [DataMember]
        [Column(Name = "DATESTATISTIQUEMOIS")]
        public Int32 DateStatistiqueMois { get; set; }
        [DataMember]
        [Column(Name = "DATESTATISTIQUEANNEE")]
        public Int32 DateStatistiqueAnnee { get; set; }
        [DataMember]
        [Column(Name = "RELANCE")]
        public string Relance { get; set; }
        [DataMember]
        [Column(Name = "RELANCEVALEUR")]
        public Int32 RelanceValeur { get; set; }
        [DataMember]
        [Column(Name = "PREAVIS")]
        public Int32 Preavis { get; set; }
        [DataMember]
        [Column(Name = "ANNOTATIONFIN")]
        public string AnnotationFin { get; set; }
    }
}
