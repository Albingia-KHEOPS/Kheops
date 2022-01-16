using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.ParametreGaranties
{
    public class ParamGarantieListesDto
    {
        public List<ParametreDto> Taxes { get; set; }
        public List<ParametreDto> CatNats { get; set; }
        public List<ParametreDto> TypesDefinition { get; set; }
        public List<ParametreDto> TypesInformation { get; set; }
        public List<ParametreDto> TypesGrille { get; set; }
        public List<ParametreDto> TypesInventaire { get; set; }
        public List<ParametreDto> TypesRegul { get; set; }
    }
}
