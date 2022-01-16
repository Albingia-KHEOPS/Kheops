using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDoubleSaisie
{
    public class ModeleDoubleSaisieCourtier
    {
        public string ValidCourtier { get; set; }
        public Int64 Code { get; set; }
        public string Courtier { get; set; }
        public string Delegation { get; set; }
        public string Souscripteur { get; set; }
        public string SouscripteurCode { get; set; }
        public DateTime? SaisieDate { get; set; }
        public TimeSpan? SaisieHeure { get; set; }
        public DateTime? EnregistrementDate { get; set; }
        public TimeSpan? EnregistrementHeure { get; set; }
        public string Motif { get; set; }
        public string LibelleMotif { get; set; }
    }
}