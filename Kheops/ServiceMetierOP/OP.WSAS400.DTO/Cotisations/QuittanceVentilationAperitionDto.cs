using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Cotisations
{
    public class QuittanceVentilationAperitionDto
    {

        [DataMember]
        public double PartAlbingia { get; set; }

        [DataMember]
        public List<QuittanceTabAperitionLigneDto> ListeLignesGaranties { get; set; }

        [DataMember]
        public double CommissionTauxHCatnat { get; set; }
        [DataMember]
        public double CommissionTauxCatnat { get; set; }
        [DataMember]
        public double CommissionValHCatnat { get; set; }
        [DataMember]
        public double CommissionValCatnat { get; set; }
        [DataMember]
        public double CommissionTotal { get; set; }

        [DataMember]
        public double FraisAperition { get; set; }
        [DataMember]
        public double CoassuranceHTHCatnat { get; set; }
        [DataMember]
        public double CoassuranceHTCatnat { get; set; }
        [DataMember]
        public double CoassuranceHTTotal { get; set; }

        [DataMember]
        public double CoassuranceCommHCatnat { get; set; }
        [DataMember]
        public double CoassuranceCommCatnat { get; set; }
        [DataMember]
        public double CoassuranceCommTotal { get; set; }
    }
}
