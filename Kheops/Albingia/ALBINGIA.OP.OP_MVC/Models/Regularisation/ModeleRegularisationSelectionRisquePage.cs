using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegularisation;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.Regularisation
{
    public class ModeleRegularisationSelectionRisquePage : MetaModelsBase
    {
        public DateTime? EffetGaranties { get; set; }
        public DateTime? FinEffet { get; set; }
        public TimeSpan? FinEffetHeure { get; set; }
        public DateTime? Echeance { get; set; }
        public string LibTypeContrat { get; set; }

        public ModeleRisquesRegularisation RsqRegule { get; set; }
    }
}