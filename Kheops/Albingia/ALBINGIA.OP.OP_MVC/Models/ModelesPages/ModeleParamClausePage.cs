using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using EmitMapper;
using OP.WSAS400.DTO.ParametreClauses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleParamClausePage : MetaModelsBase
    {
        public int Etape { get; set; }
        [Display(Name="Service")]
        public string Service { get; set; }
        public List<AlbSelectListItem> Services { get; set; }

        public static explicit operator ModeleParamClausePage(ParamClausesDto paramClausesDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamClausesDto, ModeleParamClausePage>().Map(paramClausesDto);
        }

        public static ParamClausesDto LoadDto(ModeleParamClausePage modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleParamClausePage, ParamClausesDto>().Map(modele);
        }

    }
}