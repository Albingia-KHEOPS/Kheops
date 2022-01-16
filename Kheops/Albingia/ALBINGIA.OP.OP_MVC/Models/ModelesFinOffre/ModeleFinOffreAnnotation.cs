using System.ComponentModel.DataAnnotations;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesFinOffre
{
    public class ModeleFinOffreAnnotation
    {
        [Display(Name = "Observations")]
        public string AnnotationFin { get; set; }
        public bool IsReadOnly { get; set; }

        public ModeleFinOffreAnnotation()
        {
            AnnotationFin = string.Empty;
        }
    }
}