using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleEngagements;
using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleDetailCalculSMPPage : MetaModelsBase
    {
        public string IdRisque { get; set; }
        public string IdVolet { get; set; }
        public ModeleEnteteDetailCalculSMP Entete { get; set; }       
        public ModeleLignesDetailCalculSMP ListeGarantie { get; set; }
        [Display(Name = "Commentaires")]
        public string CommentForce { get; set; }
        public bool MontantForce { get; set; }

    }
}