using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ParametreEngagement
{
    public class ParamEngmentColonneDto
    {
        [Column(Name="CODETRAITE")]
        public string CodeTraite { get; set; }
        [Column(Name = "CODE")]
        public Int64 Code { get; set; }
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }
        [Column(Name="SEPARATION")]
        public string Separation { get; set; }
    }
}
