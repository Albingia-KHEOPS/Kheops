using EmitMapper;
using OP.WSAS400.DTO.Offres.Branches;
using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleCibles
{
    public class ModeleCible
    {
        public bool ReadOnly { get; set; }
        public string GuidId { get; set; }

        [Display(Name="Code Cible")]
        [Required]
        public string Code { get; set; }
        [Display(Name = "Description")]
        [Required]
        public string Description { get; set; }
        [Display(Name = "Créé le")]
        public DateTime? DateCreation { get; set; }
        [Display(Name = "Branche")]
        public string CodeBranche { get; set; }
        [Display(Name = "Caractère")]
        public string Caractere { get; set; }

        public static explicit operator ModeleCible(CibleDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<CibleDto, ModeleCible>().Map(modeleDto);
        }

}
}