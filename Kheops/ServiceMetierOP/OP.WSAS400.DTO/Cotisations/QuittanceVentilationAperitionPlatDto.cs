using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Cotisations
{
    public class QuittanceVentilationAperitionPlatDto
    {
        [Column(Name = "TYPEVALEUR")]
        public string TypeValeur { get; set; }

        [Column(Name = "PARTALBINGIA")]
        public double PartAlbingia { get; set; }

        [Column(Name = "CODEGARANTIE")]
        public string CodeGarantie { get; set; }

        [Column(Name = "LIBELLEGARANTIE")]
        public string LibelleGarantie { get; set; }

        [Column(Name = "GARHCATNAT")]
        public double GarantieHCatnat { get; set; }

        [Column(Name = "GARCATNAT")]
        public double GarantieCatnat { get; set; }

        [Column(Name = "COMMTAUXHCATNAT")]
        public double CommissionTauxHCatnat { get; set; }

        [Column(Name = "COMMTAUXCATNAT")]
        public double CommissionTauxCatnat { get; set; }

        [Column(Name = "COMMVALHCATNAT")]
        public double CommissionValHCatnat { get; set; }

        [Column(Name = "COMMVALCATNAT")]
        public double CommissionValCatnat { get; set; }

        [Column(Name = "FRAISAPERITION")]
        public double FraisAperition { get; set; }

        [Column(Name = "COASSHTHCATNAT")]
        public double CoassuranceHTHCatnat { get; set; }

        [Column(Name = "COASSHTCATNAT")]
        public double CoassuranceHTCatnat { get; set; }

        [Column(Name = "COASSCOMMHCATNAT")]
        public double CoassuranceCommHCatnat { get; set; }

        [Column(Name = "COASSCOMMCATNAT")]
        public double CoassuranceCommCatnat { get; set; }
    }
}
