using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain.DTO
{
    public class OffreCriteresDeRecherche
    {
        public Offre Offre { get; set; }
        public string TexteContenuDansAdresseRisque { get; set; }
        public string RechercheLibre { get; set; }
        public TypeDateRecherche TypeDateRecherche { get; set; }
        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }
        public string SortingName { get; set; }
        public string SortingOrder { get; set; }
        public int StartLine { get; set; }
        public int EndLine { get; set; }
        public int LineCount { get; set; }
    }
}
