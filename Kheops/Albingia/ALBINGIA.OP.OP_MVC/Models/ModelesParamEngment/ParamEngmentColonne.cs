using EmitMapper;
using OP.WSAS400.DTO.ParametreEngagement;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamEngment
{
    public class ParamEngmentColonne
    {
        public string CodeTraite { get; set; }
        public Int32 Code { get; set; }
        public string Libelle { get; set; }
        public string Separation { get; set; }

        public static explicit operator ParamEngmentColonne(ParamEngmentColonneDto modelDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamEngmentColonneDto, ParamEngmentColonne>().Map(modelDto);
        }

        public static ParamEngmentColonneDto LoadDto(ParamEngmentColonne modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamEngmentColonne, ParamEngmentColonneDto>().Map(modele);
        }
    }
}