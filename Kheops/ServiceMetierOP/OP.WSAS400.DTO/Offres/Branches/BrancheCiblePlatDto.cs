using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Offres.Branches
{
    public class BrancheCiblePlatDto
    {
        [Column(Name="BRANCHE")]
        public string Branche { get; set; }
        [Column(Name="CIBLE")]
        public string Cible { get; set; }
        [Column(Name = "IDCIBLE")]
        public Int64 Idcible { get; set; }
    }
}
