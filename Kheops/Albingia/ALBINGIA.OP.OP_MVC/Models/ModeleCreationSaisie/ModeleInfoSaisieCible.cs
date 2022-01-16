using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie
{
    public class ModeleInfoSaisieCible
    {
        [Display(Name = "Cible")]

        public String CibleLibelle { get; set; }
        [Display(Name = "Cible*")]
        public String Cible { get; set; }
        public List<AlbSelectListItem> Cibles { get; set; }

        public bool CopyMode { get; set; }
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Permet de savoir si la page est chargée dynamiquement sur changement de template dans la liste
        /// </summary>
        public bool LoadTemplateMode { get; set; }

        public bool IsConfirmation { get; set; }

        public bool IsReadOnlyDisplay { get; set; }

        public ModeleInfoSaisieCible()
        {
            Cibles = new List<AlbSelectListItem>();
        }
    }
}