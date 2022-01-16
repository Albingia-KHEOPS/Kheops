using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    public class DeblocageTermeDto
    {
        [DataMember]
        [Column(Name = "DATEDEBDERNIEREPERIODEJOUR")]
        public Int16 DateDebutDernierePeriodeJour { get; set; }
        [DataMember]
        [Column(Name = "DATEDEBDERNIEREPERIODEMOIS")]
        public Int16 DateDebutDernierePeriodeMois { get; set; }
        [DataMember]
        [Column(Name = "DATEDEBDERNIEREPERIODEANNEE")]
        public Int16 DateDebutDernierePeriodeAnnee { get; set; }
        [DataMember]
        [Column(Name = "DATEFINDERNIEREPERIODEJOUR")]
        public Int16 DateFinDernierePeriodeJour { get; set; }
        [DataMember]
        [Column(Name = "DATEFINDERNIEREPERIODEMOIS")]
        public Int16 DateFinDernierePeriodeMois { get; set; }
        [DataMember]
        [Column(Name = "DATEFINDERNIEREPERIODEANNEE")]
        public Int16 DateFinDernierePeriodeAnnee { get; set; }
        [DataMember]
        [Column(Name = "DATEECHEANCEEMISSIONJOUR")]
        public Int16 DateEcheanceEmissionJour { get; set; }
        [DataMember]
        [Column(Name = "DATEECHEANCEEMISSIONMOIS")]
        public Int16 DateEcheanceEmissionMois { get; set; }
        [DataMember]
        [Column(Name = "DATEECHEANCEEMISSIONANNEE")]
        public Int16 DateEcheanceEmissionAnnee { get; set; }
        public string  MsgErreur { get; set; }


    }
}
