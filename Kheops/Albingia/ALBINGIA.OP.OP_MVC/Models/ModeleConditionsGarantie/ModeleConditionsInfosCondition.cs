using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModelesRisque;
using OP.WSAS400.DTO.Offres.Branches;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleConditionsGarantie
{
    [Serializable]
    public class ModeleConditionsInfosCondition {
        private bool isReadOnly;

        public string CodeBranche { get; set; }
        public bool FullScreen { get; set; }
        [Display(Name = "Formule")]
        public string Formule { get; set; }
        [Display(Name = "s'applique")]
        public string AppliqueA { get; set; }
        [Display(Name = "Garantie")]
        public string Garantie { get; set; }
        public List<AlbSelectListItem> Garanties { get; set; }
        [Display(Name = "Volets/blocs")]
        public string VoletBloc { get; set; }
        public List<AlbSelectListItem> VoletsBlocs { get; set; }
        [Display(Name = "Niveaux")]
        public string Niveau { get; set; }
        public List<AlbSelectListItem> Niveaux { get; set; }

        public List<ModeleConditionsGarantie> ListGaranties { get; set; }

        public ModeleConditionsFiltre FiltreGarantie { get; set; }
        public ModeleConditionsFiltre FiltreVoletsBlocs { get; set; }
        public ModeleConditionsFiltre FiltreNiveau { get; set; }

        public string Type { get; set; }

        public bool IsReadOnly {
            get {
                return IsAvnDisabled || this.isReadOnly;
            }
            set {
                if (IsAvnDisabled) return;
                this.isReadOnly = value;
            }
        }

        public ModeleRisque LstRisque { get; set; }

        public bool IsAvnDisabled { get; set; }
        public CibleDto CodeCible { get; internal set; }
    }
}