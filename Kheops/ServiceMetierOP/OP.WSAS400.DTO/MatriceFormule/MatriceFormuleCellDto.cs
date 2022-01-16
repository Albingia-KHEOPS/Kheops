using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace OP.WSAS400.DTO.MatriceFormule
{
    public class MatriceFormuleCellDto
    {
        [Column(Name = "NUMCHRONOLINE")]
        public Int32 NumChronoLine { get; set; }
        [Column(Name = "NUMCHRONOCOLUMN")]
        public Int32 NumChronoColumn { get; set; }
        [Column(Name = "ICONE")]
        public string Icone { get; set; }
    }
}
