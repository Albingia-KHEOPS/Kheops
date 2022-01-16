using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.AssuresAdditionnels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.AssuresAdditionnels
{
    [Serializable]
    public class AssuresNonRef
    {
        [Display(Name = "Qualités")]
        public string CodeQualite1 { get; set; }
        public string LibQualite1 { get; set; }
        public List<AlbSelectListItem> Qualites1 { get; set; }
        public string CodeQualite2 { get; set; }
        public string LibQualite2 { get; set; }
        public List<AlbSelectListItem> Qualites2 { get; set; }
        public string CodeQualite3 { get; set; }
        public string LibQualite3 { get; set; }
        public List<AlbSelectListItem> Qualites3 { get; set; }
        [Display(Name = "Autre")]
        public string QualiteAutre { get; set; }
        public bool ModeEdit { get; set; }
        public string IdAssuNonRef { get; set; }

        public static explicit operator AssuresNonRef(AssuresNonRefDto assureDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AssuresNonRefDto, AssuresNonRef>().Map(assureDto);
        }

        public static AssuresNonRefDto LoadDto(AssuresNonRef modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AssuresNonRef, AssuresNonRefDto>().Map(modele);
        }
    }
}