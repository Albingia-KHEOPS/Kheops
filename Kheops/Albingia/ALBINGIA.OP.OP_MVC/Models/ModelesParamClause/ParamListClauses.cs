using EmitMapper;
using OP.WSAS400.DTO.ParametreClauses;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamClause
{
    public class ParamListClauses
    {
        public Int64 Code { get; set; }
        public string Nom1 { get; set; }
        public string Nom2 { get; set; }
        public int Nom3 { get; set; }
        public string Libelle { get; set; }
        public Int64 Rattache { get; set; }

        public static explicit operator ParamListClauses(ParamListClausesDto paramListClausesDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamListClausesDto, ParamListClauses>().Map(paramListClausesDto);
        }

        public static ParamListClausesDto LoadDto(ParamListClauses modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamListClauses, ParamListClausesDto>().Map(modele);
        }
    }
}