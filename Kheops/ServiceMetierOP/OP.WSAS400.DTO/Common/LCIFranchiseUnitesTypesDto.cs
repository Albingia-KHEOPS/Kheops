using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Common
{
    public class LCIFranchiseUnitesTypesDto
    {
        public List<ParametreDto> UnitesLCI { get; set; }
        public List<ParametreDto> TypesLCI { get; set; }
        public List<ParametreDto> UnitesFranchise { get; set; }
        public List<ParametreDto> TypesFranchise { get; set; }
    }
}
