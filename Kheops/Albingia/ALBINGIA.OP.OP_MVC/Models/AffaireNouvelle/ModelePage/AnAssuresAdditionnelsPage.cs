using ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.AssuresAdditionnels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using EmitMapper;
using OP.WSAS400.DTO.AssuresAdditionnels;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.ModelePage
{
    [Serializable]
    public class AnAssuresAdditionnelsPage : MetaModelsBase
    {
        public List<AssuresRef> AssuresReference { get; set; }
        public List<AssuresNonRef> AssuresNonReference { get; set; }

        public static explicit operator AnAssuresAdditionnelsPage(AssuresAdditionnelsDto assureDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AssuresAdditionnelsDto, AnAssuresAdditionnelsPage>().Map(assureDto);
        }

        public static AssuresAdditionnelsDto LoadDto(AnAssuresAdditionnelsPage modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AnAssuresAdditionnelsPage, AssuresAdditionnelsDto>().Map(modele);
        }
    }
}