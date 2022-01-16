using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.ParametreEngagement
{
    public class ParamEngagementDto
    {
        public string Traite { get; set; }
        public List<ParametreDto> Traites { get; set; }
        public List<ParamEngmentColonneDto> ParamsColonne { get; set; }
    }
}
