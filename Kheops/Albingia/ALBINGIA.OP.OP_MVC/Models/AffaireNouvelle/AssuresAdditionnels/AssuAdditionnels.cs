using EmitMapper;
using OP.WSAS400.DTO.AssuresAdditionnels;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.AssuresAdditionnels
{
    [Serializable]
    public class AssuAdditionnels
    {
        public Int32 CodeError { get; set; }
        public List<AssuresRef> AssuresReference { get; set; }
        public List<AssuresNonRef> AssuresNonReference { get; set; }
        public bool IsReadOnly { get; set; }

        public bool IsModeAvenant { get; set; }
        public bool IsAvenantModificationLocale { get; set; }
        public DateTime? DateEffetAvenantModificationLocale { get; set; }

        public static explicit operator AssuAdditionnels(AssuresAdditionnelsDto assureDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AssuresAdditionnelsDto, AssuAdditionnels>().Map(assureDto);
        }

        public static AssuresAdditionnelsDto LoadDto(AssuAdditionnels modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AssuAdditionnels, AssuresAdditionnelsDto>().Map(modele);
        }
    }
}