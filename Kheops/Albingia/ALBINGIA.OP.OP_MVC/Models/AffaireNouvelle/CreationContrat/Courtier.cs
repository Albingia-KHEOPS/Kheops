using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.CreationContrat
{
    public class Courtier
    {
        [Display(Name = "Code *")]
        [Required(ErrorMessage = "Veuillez choisir un code de courtier.")]
        public string CodeCourtier { get; set; }

        [Display(Name = "Nom")]
        public string NomCourtier { get; set; }

        public int CodeInterlocuteur { get; set; }
        [Display(Name = "Interlocuteur")]
        public string NomInterlocuteur { get; set; }    

        [Display(Name = "Référence")]
        public string Reference { get; set; }
        
        public bool EditMode { get; set; }

        public bool IdentiqueChecked { get; set; }

        [Display(Name = "Quittancement")]
        public string Encaissement { get; set; }
        public List<AlbSelectListItem> Encaissements { get; set; }
        public bool IsReadOnly { get; set; }

    }
}