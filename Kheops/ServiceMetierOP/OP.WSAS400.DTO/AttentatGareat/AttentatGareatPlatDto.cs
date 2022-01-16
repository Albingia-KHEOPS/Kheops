using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.AttentatGareat
{
    public class AttentatGareatPlatDto
    {
        [DataMember]
        [Column(Name = "LCI")]
        public Double LCI { get; set; }
        [DataMember]
        [Column(Name = "CAPITAUX")]
        public Double Capitaux { get; set; }
        [DataMember]
        [Column(Name = "SURFACE")]
        public Int32 Surface { get; set; }
        [DataMember]
        [Column(Name = "CATNAT")]
        public Double CATNAT { get; set; }
        [DataMember]
        [Column(Name = "CAPITAUXFORCE")]
        public Double CapitauxForces { get; set; }
        [DataMember]    
        public string LibelleConstruit { get; set; }   
        [DataMember]
        [Column(Name = "SOUMISSTD")]
        public String ParamStdrSoumis { get; set; }
        [DataMember]
        [Column(Name = "TRANCHESTD")]
        public String ParamStdrTranche { get; set; }
        [DataMember]
        [Column(Name = "TAUXSTD")]
        public Double ParamStdrTauxCession { get; set; }
        [DataMember]
        [Column(Name = "FRAISSTD")]
        public Double ParamStdrFraisGestion { get; set; }
        [DataMember]
        [Column(Name = "COMMISSIONSTD")]
        public Double ParamStdrCommission { get; set; }
        [DataMember]
        [Column(Name = "FACTURESTD")]
        public Double ParamStdrFacture { get; set; }      
        [DataMember]
        [Column(Name = "SOUMISRET")]
        public String ParamRetenusSoumis { get; set; }
        [DataMember]
        [Column(Name = "TRANCHERET")]
        public String ParamRetenusTranche { get; set; }
        [DataMember]
        [Column(Name = "TAUXRET")]
        public Double ParamRetenusTauxCession { get; set; }
        [DataMember]
        [Column(Name = "FRAISRET")]
        public Double ParamRetenusFraisGestion { get; set; }
        [DataMember]
        [Column(Name = "COMMISSIONRET")]
        public Double ParamRetenusCommission { get; set; }
        [DataMember]
        [Column(Name = "FACTURERET")]
        public Double ParamRetenusFacture { get; set; }
        [DataMember]
        [Column(Name = "COMMENTFORCE")]
        public string CommentForce { get; set; }
        [DataMember]
        [Column(Name = "MONTANTFORCE")]
        public String MontantForce { get; set; }
    }
}
