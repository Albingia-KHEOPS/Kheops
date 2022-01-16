using Albingia.Kheops.OP.Domain.Model;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.DTO
{
    public class ParamVoletDto
    {
        public int Ordre { get; set; }
        public CaractereSelection Caractere { get; set; }
        public Branche Branche { get; set; }
        public CibleCatego Cible { get; set; }
        public long CatVoletId { get; set; }
        public long VoletId { get; set; }

        // KAKPRES : "RP" ou vide
        public bool IsVoletCollapsed { get; set; }
        // KAKFGEN 
        public bool? IsFormuleGenerale { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public IEnumerable<ParamBlocDto> Blocs { get; set; }
    }
}
