using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.PGM
{
    public class RemiseEnVigueurParams : RemiseEnVigueurDto {
        public string User { get; set; }
        public string ActeGestion { get; set; }
    }
}
