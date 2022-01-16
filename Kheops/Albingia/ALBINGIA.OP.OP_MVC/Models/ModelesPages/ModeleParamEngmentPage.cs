using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamEngment;
using EmitMapper;
using OP.WSAS400.DTO.ParametreEngagement;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleParamEngmentPage : MetaModelsBase
    {
        [Display(Name="Famille de réassurance")]
        public string Traite { get; set; }
        public List<AlbSelectListItem> Traites { get; set; }
        public List<ParamEngmentColonne> ParamsColonne { get; set; }


        public static explicit operator ModeleParamEngmentPage(ParamEngagementDto modelDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamEngagementDto, ModeleParamEngmentPage>().Map(modelDto);
        }

        public static ParamEngagementDto LoadDto(ModeleParamEngmentPage modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleParamEngmentPage, ParamEngagementDto>().Map(modele);
        }
    }
}