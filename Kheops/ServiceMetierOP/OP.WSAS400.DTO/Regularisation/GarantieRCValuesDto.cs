using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class GarantieRCValuesDto
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string CodeGarantie { get; set; }

        [DataMember]
        public string Label { get; set; }

        [DataMember]
        public GarantieValuesDto BasicValues { get; set; }

        [DataMember]
        public double CalculAssiette { get; set; }

        [DataMember]
        public double CalculTaxesAssiette { get; set; }

        [DataMember]
        public string CalculError { get; set; }

        [DataMember]
        public double CotisationEmise { get; set; }

        [DataMember]
        public double TaxesCotisationEmise { get; set; }

        [DataMember]
        public double Coefficient { get; set; }
    }
}
