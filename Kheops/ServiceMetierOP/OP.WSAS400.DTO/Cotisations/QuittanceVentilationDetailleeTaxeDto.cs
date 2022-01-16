using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Cotisations
{
    public class QuittanceVentilationDetailleeTaxeDto
    {
        [DataMember]
        [Column(Name = "CODETAXE")]
        public string CodeTaxe { get; set; }
        [DataMember]
        [Column(Name = "LIBELLETAXE")]
        public string LibelleTaxe { get; set; }
        [DataMember]
        [Column(Name = "BASETAXABLE")]
        public Double BaseTaxable { get; set; }
        [DataMember]
        [Column(Name = "MONTANTTAXES")]
        public Double MontantTaxes { get; set; }
    }
}
