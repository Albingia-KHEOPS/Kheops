using EmitMapper;
using OP.WSAS400.DTO.ParametreClauses;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamClause
{
    public class ParamEtapeContexte
    {
        public string CodeService { get; set; }
        public string Service { get; set; }
        public string CodeActeGestion { get; set; }
        public string ActeGestion { get; set; }
        public string CodeEtape { get; set; }
        public string Etape { get; set; }
        public List<ParamListParam> ListContextes { get; set; }

        public static explicit operator ParamEtapeContexte(ParamEtapeContexteDto paramEtapeContexteDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamEtapeContexteDto, ParamEtapeContexte>().Map(paramEtapeContexteDto);
        }

        public static ParamEtapeContexteDto LoadDto(ParamEtapeContexte modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamEtapeContexte, ParamEtapeContexteDto>().Map(modele);
        }
    }
}