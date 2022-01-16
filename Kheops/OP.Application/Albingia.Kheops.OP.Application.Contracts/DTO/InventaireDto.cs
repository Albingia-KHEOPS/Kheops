using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Inventaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.DTO
{
    public class InventaireDto
    {
        public long Id { get; set; }
        public int NumChrono { get; set; }
        public string Descriptif { get; set; }
        public TypeInventaireDto TypeInventaire { get; set; }
        public string Designation { get; set; }
        public long ChronoDesi { get; set; }
        public bool? ReportvaleurObjet { get; set; }
        public bool? IsHTnotTTC { get; set; }
        public ICollection<InventaireItem> Items { get; set; }
        public ValeursUnite Valeurs { get; set; }
        public TypeValeurRisque Typedevaleur { get; set; }
        public Valeurs ValeursIndice { get; set; }
    }
}
