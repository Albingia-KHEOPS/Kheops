using ALBINGIA.OP.OP_MVC.Models.FormuleGarantie;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesFormuleGarantie;
using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleFormuleGarantiePage : MetaModelsBase
    {
        [Display(Name = "Branche")]
        public string Branche { get; set; }
        public string BrancheLib { get; set; }
        public Int64 CodeCible { get; set; }
        [Display(Name = "Cible")]
        public string Cible { get; set; }
        public string CibleLib { get; set; }
        //[Display (Name="Catégories")]
        //public string Categorie { get; set; }
        //public List<AlbSelectListItem> Categories { get; set; }
        [Display(Name="Libellé formule")]
        public string LettreLib { get; set; }
        public string Libelle { get; set; }
        [Display(Name="s'applique")]
        public string ObjetRisque { get; set; }
        public string ObjetRisqueCode { get; set; }
        public int NumRisque { get; set; }

        public int NbObjets { get; set; }
        public int NbObjetsSelectionnes { get; set; }
        /// <summary>
        /// Liste des risques et objets de l'offre
        /// </summary>
        public ModeleFormuleGarantieLstObjRsq ObjetsRisquesAll { get; set; }
        /// <summary>
        /// Savoir si le risque est déjà sélectionné
        /// </summary>
        public string  RisqueCoche { get; set; }        

        public string CodeFormule { get; set; }
        public string CodeOption { get; set; }
        public string FormGen { get; set; }

        #region Avenant

        public DateTime? DateEffetAvenant { get; set; }
        public bool IsAvenantModificationLocale { get; set; }
        public DateTime? DateEffetAvenantModificationLocale { get; set; }
        public bool IsModeAvenant { get; set; }
        public int AvnCreationFor { get; set; } //Numéro d'avenant dans lequel a été créé la formule courante
        public bool IsIndexe { get; set; }
        public bool IsAlertePeriode { get; set; }
        public bool IsTraceAvnExist { get; set; }
        public string DateFinEffet { get; set; }
        public string HeureFinEffet { get; set; }
        public string DateModificationRsq { get; set; }
        public bool PreventChangeModifAvn
        {
            get
            {
                return IsTraceAvnExist && IsAvenantModificationLocale || IsModeConsultationEcran || IsAlertePeriode;
            }
        }
        #endregion

        public ModeleFormuleGarantie Formule { get; set; }

        public bool ModeDuplicate { get; set; }
        public DateTime? NewFormulaDateAvenant { get; set; } = null;
        public InfoApplication InfoApplication { get; set; }
        public Formule OptionsFormule { get; set; }
    }
}