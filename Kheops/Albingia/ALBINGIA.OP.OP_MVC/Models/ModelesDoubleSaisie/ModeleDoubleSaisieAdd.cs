using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDoubleSaisie
{
    [Serializable]
    public class ModeleDoubleSaisieAdd
    {
        public string Code { get; set; }
        public string Courtier { get; set; }
        public string Souscripteur { get; set; }
        public string Delegation { get; set; }
        public DateTime? SaisieDate { get; set; }
        public TimeSpan? SaisieHeure { get; set; }
        public string Action { get; set; }
        public string MotifRemp { get; set; }
        public string Interlocuteur { get; set; }
        public string Reference { get; set; }
    }
}