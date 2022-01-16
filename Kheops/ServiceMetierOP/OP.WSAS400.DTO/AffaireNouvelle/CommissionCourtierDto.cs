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
    public class CommissionCourtierDto
    {
        [DataMember]
        public double TauxStandardHCAT { get; set; }

        [DataMember]
        public double TauxStandardCAT { get; set; }

        [DataMember]
        [Column(Name = "JDXCM")]
        public double TauxContratHCAT { get; set; }

        [DataMember]
        [Column(Name = "JDCNC")]
        public double TauxContratCAT { get; set; }

        [DataMember]
        [Column(Name = "KAJOBSV")]
        public string Commentaires { get; set; }

        [DataMember]
        public string Erreur { get; set; }

        [DataMember]
        [Column(Name = "KAAXCMS")]
        public string IsStandardHCAT { get; set; }

        [DataMember]
        [Column(Name = "KAACNCS")]
        public string IsStandardCAT { get; set; }

        [DataMember]
        public bool IsTraceAvnExist { get; set; }
        [DataMember]
        public DateTime? DateEffetAvenantModificationLocale { get; set; }

        [DataMember]
        public double CommissionAperition { get; set; }

        [DataMember]
        public Int32 EchError { get; set; }
    }
}
