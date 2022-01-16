using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ParametreCibles
{
    public class TemplatePlatDto
    {
        [Column(Name = "CODETEMP")]
        public string Code { get; set; }
        [Column(Name = "TYPETEMP")]
        public string Type { get; set; }
        [Column(Name = "BRANCHETEMP")]
        public string Branche { get; set; }
        [Column(Name = "CIBLETEMP")]
        public string Cible { get; set; }
        [Column(Name="SITUATIONTEMP")]
        public string Situation { get; set; }
    }
}
