using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.ParametreClauses
{
    public class ParamClauseEtapeDto
    {
        [Column(Name="SERVICE")]
        public string Service { get; set; }
        [Column(Name = "ACTEGESTION")]
        public string ActeGestion { get; set; }
        [Column(Name = "CODEETAPE")]
        public int CodeEtape { get; set; }
        [Column(Name = "ETAPE")]
        public string Etape { get; set; }
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }
    }
}
