using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleRechercheOffreRapide
{
    [Serializable]
    public class ModeleOffreRapideFiltre
    {
        public string CodeOffre { get; set; }
        public int? Version { get; set; }
        public string[] TypeOffres { get; set; }
        public int? CodeAvenant { get; set; }
        public string TypeTraitement { get; set; }
        public DateTime? DateEffetAvnDebut { get; set; }
        public DateTime? DateEffetAvnFin { get; set; }
        public string CodePeriodicite { get; set; }
        public string CodeBranche { get; set; }
        public string CodeCible { get; set; }
        public string UserCrea { get; set; }
        public string UserMaj { get; set; }
        public int PageNumber { get; set; }
        public int NbCount { get; set; }

    }
}