using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleRechercheOffreRapide
{
    [Serializable]
    public class ModeleOffreRapideInfo
    {
        public string CodeOffre { get; set; }
        public string CodeTypeOffre { get; set; }
        public string Version { get; set; }
        public string CodeAvenant { get; set; }
        public string TypeTraitement { get; set; }
        public DateTime? DateDeSaisieDate { get; set; }
        public string CodeBranche { get; set; }
        public string LibelleBranche { get; set; }
        public string CodeCible { get; set; }
        public string LibelleCible { get; set; }
        public string CodeEtat { get; set; }
        public string LibellleEtat { get; set; }
        public string CodeSituation { get; set; }
        public string LibelleSituation { get; set; }
        public string Descriptif { get; set; }
        public string CodePeriodicite { get; set; }
        public string LibelllePeriodicite { get; set; }
        public string UserCrea { get; set; }
        public string UserMaj { get; set; }
    }
}