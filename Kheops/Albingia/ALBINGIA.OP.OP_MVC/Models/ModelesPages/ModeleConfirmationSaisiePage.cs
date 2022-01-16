using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleConfirmationSaisiePage : MetaModelsBase
    {
        public string CodeOffre { get; set; }
        public string Version { get; set; }
        public string Type { get; set; }

        public object GroupInformationTiersTitle { get { return "Information Tiers impliqués"; } }
        public object GroupInformationSaisieTitle { get { return "Information saisie"; } }
        public object GroupActionsARealiserTitle { get { return "Actions à réaliser"; } }

        [Display(Name = "Nom")]
        public string ApporteurCourtierNom { get; set; }
        public ModeleContactAdresse ApporteurAdresse { get; set; }
        [Display(Name = "Délégation")]
        public string ApporteurCourtierDelegation { get; set; }
        [Display(Name = "Inspecteur")]
        public string ApporteurCourtierInspecteur { get; set; }
        [Display(Name = "Valide")]
        public bool ApporteurCourtierValide { get; set; }

        [Display(Name = "Nom")]
        public string PreneurNom { get; set; }
        public ModeleContactAdresse PreneurAdresse { get; set; }


        [Display(Name = "N° Saisie attribué")]
        public string InfoSaisieNoSaisieAttribuee { get; set; }
        [Display(Name = "N° alim")]
        public string InfoSaisieNoAliment { get; set; }
        [Display(Name = "Date saisie"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InfoSaisieDateSaisie { get; set; }
        [Display(Name = "Branche")]
        public string InfoSaisieBrancheNom { get; set; }

        public bool ConfirmationSaisie { get; set; }
        public bool AttenteSaisie { get; set; }
        public bool RefusSaisie { get; set; }

        [Display(Name = "MotifRefus")]
        public string MotifRefus { get; set; }

        public List<AlbSelectListItem> MotifsRefus { get; set; }

        public string MotifAttente { get; set; }
        public List<AlbSelectListItem> MotifsAttente { get; set; }

    }
}