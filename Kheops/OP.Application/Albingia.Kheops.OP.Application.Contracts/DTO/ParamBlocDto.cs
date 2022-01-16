using Albingia.Kheops.OP.Domain.Model;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.DTO
{
    public class ParamBlocDto
    {
        public long BlocId { get; set; }
        public long CatBlocId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public CaractereSelection Caractere { get; set; }
        public decimal Ordre { get; set; }


        public virtual ICollection<ParamModeleGarantieDto> Modeles { get; set; } = new List<ParamModeleGarantieDto>();
    }
}
