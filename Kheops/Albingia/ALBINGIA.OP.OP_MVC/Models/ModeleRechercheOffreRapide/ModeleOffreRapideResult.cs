using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleRechercheOffreRapide
{
    public class ModeleOffreRapideResult
    {
        public IList<ModeleOffreRapideInfo> Offres { get; set; }

        public int PageNumber { get; set; }
        public int NbCount { get; set; }
        public int PageSize { get; set; }

    }
}