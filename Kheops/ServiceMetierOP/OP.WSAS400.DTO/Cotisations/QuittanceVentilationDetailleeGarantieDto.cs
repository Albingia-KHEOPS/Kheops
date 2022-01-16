using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Cotisations
{
    public class QuittanceVentilationDetailleeGarantieDto
    {
        [DataMember]
        [Column(Name = "CODEGARANTIE")]
        public string CodeGarantie { get; set; }
        [DataMember]
        [Column(Name = "LIBELLEGARANTIE")]
        public string LibelleGarantie { get; set; }
        [DataMember]
        [Column(Name = "HORSCATNAT")]
        public Double HorsCATNAT { get; set; }
        [DataMember]
        [Column(Name = "CATNAT")]
        public Double CATNAT { get; set; }
        [DataMember]
        [Column(Name = "MONTANTTAXES")]
        public Double MontantTaxes { get; set; }
        [DataMember]
        [Column(Name = "MONTANTTTC")]
        public Double MontantTTC { get; set; }
    }
}
