using Albingia.Kheops.OP.Domain.Parametrage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Parametrage.Inventaire
{
    public class TypeInventaire : IFiltered
    {
        //KAGID
        public long Id { get; set; }
        //KAGTYINV
        public string CodeInventaire { get; set; }
        //KAGDESC
        public string Description { get; set; }
        //KAGTMAP
        public TypeInventaireItem TypeItem { get; set; }
        //KAGTABLE
        //public long TableDeDétail {get; set;}
        //KAGKGGID
        public Filtre Filtre { get; set; }
    }
}
