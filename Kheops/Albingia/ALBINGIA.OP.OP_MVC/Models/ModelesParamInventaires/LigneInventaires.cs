using EmitMapper;
using OP.WSAS400.DTO.ParametreInventaires;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamInventaire
{
    public class LigneInventaires
    {
        public Int64 GuidId { get; set; }
        public string Code { get; set; }
        public string Libelle { get; set; }
        public int Kagtmap { get; set; }
        public Int64 CodeFiltre { get; set; }

        public static explicit operator LigneInventaires(ParamInventairesDto paramListInventairesDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamInventairesDto, LigneInventaires>().Map(paramListInventairesDto);
        }
    }
}