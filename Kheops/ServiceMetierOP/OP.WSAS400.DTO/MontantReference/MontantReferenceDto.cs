using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.MontantReference
{
    [DataContract]
    public class MontantReferenceDto
    {
        [DataMember]
        [Column(Name = "TOTALMNTCALCULE")]
        public double TotalMntCalcule { get; set; }
        [DataMember]
        [Column(Name = "TOTALMNTFORCE")]
        public double TotalMntForce { get; set; }
        [DataMember]
        [Column(Name = "TOTALMNTPROVI")]
        public string TotalMntProvi { get; set; }
        [DataMember]
        [Column(Name = "TOTALMNTACQUIS")]
        public double TotalMntAcquis { get; set; }
        [DataMember]
        [Column(Name = "ISACQUIS")]
        public string IsAcquis { get; set; }

        [DataMember]
        public string Periodicite { get; set; }
        [DataMember]
        public string EcheancePrincipale { get; set; }
        [DataMember]
        public DateTime? ProchaineEcheance { get; set; }
        [DataMember]
        public DateTime? PeriodeDeb { get; set; }
        [DataMember]
        public DateTime? PeriodeFin { get; set; }
        [DataMember]
        public string TypeFraisAccessoires { get; set; }
        [DataMember]
        public decimal Montant { get; set; }
        [DataMember]
        public bool TaxeAttentat { get; set; }
        [DataMember]
        public bool MontantForce { get; set; }
        [DataMember]
        public string CommentForce { get; set; }
        [DataMember]
        public Int64 CodeCommentaire { get; set; }

        [DataMember]
        public int DureeGarantie { get; set; }
        [DataMember]
        public string UniteTemps { get; set; }

        [DataMember]
        public List<MontantReferenceInfoDto> MontantsReference { get; set; }

    }
}
