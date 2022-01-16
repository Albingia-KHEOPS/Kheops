using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.DTO {
    public class ParamModeleGarantieDto {
        public Int32 CatId { get; set; }
        public string Description { get; set; }
        public TypologieModele Typo { get; set; }
        public string Code { get; set; }
        public virtual DateTime DateApplication { get; set; }

        //public virtual ICollection<ParamGarantieHierarchie> Garanties { get; set; } = new List<ParamGarantieHierarchie>();
    }
}