using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.MetaModels
{
    /// <summary>
    /// ConfirmationSaisie_Index_MetaModel
    /// </summary>
    [Serializable]
    public class ConfirmationSaisie_Index_MetaModel : MetaModelsBase
    {

        public string CodeOffre { get; set; }
        public string Version { get; set; }
        public string Type { get; set; }
        #region Informations de présentation

        #region GroupInformationTiersTitle

        public object GroupInformationTiersTitle { get { return "Information Tiers impliqués"; } }

        #endregion

        #region GroupInformationSaisieTitle

        public object GroupInformationSaisieTitle { get { return "Information saisie"; } }

        #endregion

        #region GroupActionsARealiserTitle

        public object GroupActionsARealiserTitle { get { return "Actions à réaliser"; } }

        #endregion

        #endregion

        #region Courtier

        [Display(Name = "Courtier Apporteur")]
        public string ApporteurCourtierNom { get; set; }
        public ContactAddresse_MetaData ApporteurAdresse { get; set; }
        [Display(Name = "Délégation")]
        public string ApporteurCourtierDelegation { get; set; }
        [Display(Name = "Inspecteur")]
        public string ApporteurCourtierInspecteur { get; set; }
        [Display(Name = "Valide")]
        public bool ApporteurCourtierValide { get; set; }

        #endregion

        #region Preneur

        [Display(Name = "Preneur assur")]
        public string PreneurNom { get; set; }
        public ContactAddresse_MetaData PreneurAdresse { get; set; }

        #endregion

        #region Information saisie

        [Display(Name = "N° Saisie attribué")]
        public string InfoSaisieNoSaisieAttribuee { get; set; }
        [Display(Name = "N° alim")]
        public string InfoSaisieNoAliment { get; set; }
        [Display(Name = "Date saisie"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InfoSaisieDateSaisie { get; set; }
        [Display(Name = "Branche")]
        public string InfoSaisieBrancheNom { get; set; }

        #endregion

        #region Actions à réaliser

        public bool ConfirmationSaisie { get; set; }
        public bool AttenteSaisie { get; set; }
        public bool RefusSaisie { get; set; }

        [Display(Name = "MotifRefus")]
        public string MotifRefus { get; set; }

        public List<AlbSelectListItem> MotifsRefus { get; set; }

        public string MotifAttente { get; set; }
        public List<AlbSelectListItem> MotifsAttente { get; set; }

        #endregion


    }
}