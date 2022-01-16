using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Risque;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    [DataContract]
    public class FormuleDto : _FormuleGarantie_Base
    {
        [DataMember]
        [Column(Name = "ISAVTMODIF")]
        public bool IsTraceAvnExist { get; set; }
        [Column(Name = "DATEEFFETAVTMODIF")]
        public Int64 DateEffetAvtModif { get; set; }
        [DataMember]
        public DateTime? DateEffetAvenantModificationLocale { get; set; }

        [DataMember]
        [Column(Name = "CODE")]
        public Int64 Code { get; set; }
        [DataMember]
        [Column(Name = "CODECIBLE")]
        public Int64 CodeCible { get; set; }
        [DataMember]
        [Column(Name = "CIBLE")]
        public string Cible { get; set; }
        [DataMember]
        [Column(Name = "DESCCIBLE")]
        public string DescCible { get; set; }
        [DataMember]
        [Column(Name = "LETTRELIB")]
        public string LettreLib { get; set; }
        [DataMember]
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }
        [DataMember]
        public string Categorie { get; set; }
        [DataMember]
        public string ObjetRisqueCode { get; set; }
        [DataMember]
        public List<RisqueDto> Risques { get; set; }
        [DataMember]
        public string MonoRisque { get; set; }
        [DataMember]
        public string MonoObjet { get; set; }
        [DataMember]
        public bool OffreAppliqueA { get; set; }

        [DataMember]
        public FormGarDto Formule { get; set; }

        [DataMember]
        public bool IsSorti { get; set; }
    }
}
