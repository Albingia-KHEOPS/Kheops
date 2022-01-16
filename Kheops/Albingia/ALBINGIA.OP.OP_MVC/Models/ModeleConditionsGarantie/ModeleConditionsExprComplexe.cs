using EmitMapper;
using OP.WSAS400.DTO.Condition;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleConditionsGarantie
{
    [Serializable]
    public class ModeleConditionsExprComplexe
    {
        public string Type { get; set; }
        public List<ModeleConditionsExprComplexeDetails> Expressions { get; set; }
        //public List<ParametreDto> Expressions { get; set; }
        public ModeleConditionsLigneGarantie LstLigneGarantie { get; set; }

        public bool IsReadOnly { get; set; }
        public bool isGenRsq { get; set; }//isGenRsq=true s'il s'agit d'une LCIFranchise Risque ou générale

        public static explicit operator ModeleConditionsExprComplexe(ConditionComplexeDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ConditionComplexeDto, ModeleConditionsExprComplexe>().Map(modeleDto);
        }
    }
}