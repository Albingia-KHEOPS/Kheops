using EmitMapper;
using OP.WSAS400.DTO.ParametreClauses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamClause
{
    public class ParamActeGestionEtape
    {
        public string CodeService { get; set; }
        [Display(Name="Service")]
        public string Service { get; set; }
        public string CodeActeGestion { get; set; }
        [Display(Name = "Acte de gestion")]
        public string ActeGestion { get; set; }
        public List<ParamListParam> ListEtapes { get; set; }

        public static explicit operator ParamActeGestionEtape(ParamActeGestionEtapeDto paramActeGestionEtapeDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamActeGestionEtapeDto, ParamActeGestionEtape>().Map(paramActeGestionEtapeDto);
        }

        public static ParamActeGestionEtapeDto LoadDto(ParamActeGestionEtape modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamActeGestionEtape, ParamActeGestionEtapeDto>().Map(modele);
        }
    }
}