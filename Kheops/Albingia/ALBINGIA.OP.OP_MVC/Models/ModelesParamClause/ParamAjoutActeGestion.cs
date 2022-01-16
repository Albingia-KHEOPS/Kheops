using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.ParametreClauses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamClause
{
    public class ParamAjoutActeGestion
    {
        public string CodeService { get; set; }
        [Display(Name="Service")]
        public string LibelleService { get; set; }
        [Display(Name="Acte de gestion")]
        public string ActeGestion { get; set; }
        public List<AlbSelectListItem> ActesGestion { get; set; }
        
        public static explicit operator ParamAjoutActeGestion(ParamAjoutActeGestionDto paramAjoutActeGestionDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamAjoutActeGestionDto, ParamAjoutActeGestion>().Map(paramAjoutActeGestionDto);
        }

        public static ParamAjoutActeGestionDto LoadDto(ParamAjoutActeGestion modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamAjoutActeGestion, ParamAjoutActeGestionDto>().Map(modele);
        }
    }
}