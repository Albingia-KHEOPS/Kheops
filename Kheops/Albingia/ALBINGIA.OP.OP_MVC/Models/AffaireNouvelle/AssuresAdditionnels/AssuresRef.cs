using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.AssuresAdditionnels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.AssuresAdditionnels
{
    [Serializable]
    public class AssuresRef
    {
        [Display(Name = "Code *")]
        public int CodeAssure { get; set; }
        [Display(Name = "Nom *")]
        public string NomAssure { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        [Display(Name = "Qualités")]
        public string CodeQualite1 { get; set; }
        public string LibQualite1 { get; set; }
        [Display(Name = "Qualités")]
        public List<AlbSelectListItem> Qualites1 { get; set; }
        public string CodeQualite2 { get; set; }
        public string LibQualite2 { get; set; }
        public List<AlbSelectListItem> Qualites2 { get; set; }
        public string CodeQualite3 { get; set; }
        public string LibQualite3 { get; set; }
        public List<AlbSelectListItem> Qualites3 { get; set; }
        [Display(Name = "Autre")]
        public string QualiteAutre { get; set; }
        public int AssureBase { get; set; }
        public Int32 IdDesi { get; set; }
        public string Designation { get; set; }

        public static explicit operator AssuresRef(AssuresRefDto assureDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AssuresRefDto, AssuresRef>().Map(assureDto);
        }

        public static AssuresRefDto LoadDto(AssuresRef modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AssuresRef, AssuresRefDto>().Map(modele);
        }
    }
}