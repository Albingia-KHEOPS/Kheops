using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace OP.WSAS400.DTO.MatriceRisque
{
    public class MatriceRisqueFormuleDto
    {
        [Column(Name = "NUMCHRONO")]
        public int NumChrono { get; set; }
        [Column(Name = "CODEALPHA")]
        public string CodeAlpha { get; set; }
    }
}
