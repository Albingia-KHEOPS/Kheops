using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ParametreClauses
{
    public class ParamClauseContextePlatDto
    {
        [Column(Name = "IDCONTEXTE")]
        public Int64 IdContexte { get; set; }
        [Column(Name = "CODECONTEXTE")]
        public string CodeContexte { get; set; }
        [Column(Name = "CODESPECIFICITE")]
        public string CodeSpecificite { get; set; }
        [Column(Name = "MODIF")]
        public string Modif { get; set; }
        [Column(Name = "AJTCLAUSIER")]
        public string AjtClausier { get; set; }
        [Column(Name = "AJTLIBRE")]
        public string AjtLibre { get; set; }
        [Column(Name = "RUBRIQUE")]
        public string Rubrique { get; set; }
        [Column(Name = "SOUSRUBRIQUE")]
        public string SousRubrique { get; set; }
        [Column(Name = "SEQUENCE")]
        public Int32 Sequence { get; set; }
        [Column(Name = "SCRIPT")]
        public string Script { get; set; }
        [Column(Name = "EMPLACEMENT")]
        public string Emplacement { get; set; }
        [Column(Name = "SOUSEMPLACEMENT")]
        public string SousEmplacement { get; set; }
        [Column(Name = "NUMORDONNANCEMENT")]
        public Single NumOrdonnancement { get; set; }
    }
}
