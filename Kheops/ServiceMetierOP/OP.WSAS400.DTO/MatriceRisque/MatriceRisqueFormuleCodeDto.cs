using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.MatriceRisque
{
    public class MatriceRisqueFormuleCodeDto
    {
        [Column(Name = "CODERISQUE")]
        public int CodeRsq { get; set; }
        [Column(Name = "CODEFORMULE")]
        public int CodeFor { get; set; }
    }
}
