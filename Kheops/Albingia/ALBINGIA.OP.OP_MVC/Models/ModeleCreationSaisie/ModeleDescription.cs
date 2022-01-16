using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie
{
    public class ModeleDescription
    {
        [Display(Name = "Mots-clés")]
        public List<AlbSelectListItem> MotsClefs { get; set; }

        public string MotClef1 { get; set; }
        public List<AlbSelectListItem> MotsClefs1 { get; set; }

        public string MotClef2 { get; set; }
        public List<AlbSelectListItem> MotsClefs2 { get; set; }

        public string MotClef3 { get; set; }
        public List<AlbSelectListItem> MotsClefs3 { get; set; }

        [Display(Name = "Identification *")]
        public string Descriptif { get; set; }

        [Display(Name = "Observations")]
        public string Observation { get; set; }
        public bool IsReadOnly { get; set; }

        public bool IsReadOnlyDisplay { get; set; }
            
        public ModeleDescription()
        {
            MotsClefs = new List<AlbSelectListItem>();
            MotsClefs1 = new List<AlbSelectListItem>();
            MotsClefs2 = new List<AlbSelectListItem>();
            MotsClefs3 = new List<AlbSelectListItem>();
        }
    }
}