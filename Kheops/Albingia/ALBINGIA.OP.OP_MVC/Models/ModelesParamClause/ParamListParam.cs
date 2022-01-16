using EmitMapper;
using OP.WSAS400.DTO.ParametreClauses;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamClause
{
    [Serializable]
    public class ParamListParam
    {
        public int Code { get; set; }
        public string Type { get; set; }
        public string Param { get; set; }
        public string Libelle { get; set; }
        public int NumOrdre { get; set; }
        public bool AjoutClausier { get; set; }
        public bool AjoutLibre { get; set; }
        public bool EmplModif { get; set; }
        public string ScriptControle { get; set; }
        public string ModeleClause1 { get; set; }
        public string ModeleClause2 { get; set; }
        public string ModeleClause3 { get; set; }
        public string LibModelClause { get; set; }
        public string Emplacement { get; set; }
        public string LibEmplacement { get; set; }
        public string SousEmplacement { get; set; }
        public string NumOrdonnancement { get; set; }

        public static explicit operator ParamListParam(ParamListParamDto paramActeGestionDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamListParamDto, ParamListParam>().Map(paramActeGestionDto);
        }

        public static ParamListParamDto LoadDto(ParamListParam modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamListParam, ParamListParamDto>().Map(modele);
        }

    }
}