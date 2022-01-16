using EmitMapper;
using OP.WSAS400.DTO.ParametreGaranties;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamGaranties
{
    public class LigneTypeValeur
    {
        public string AdditionalParam { get; set; }
        public int Id { get; set; }        
        public string CodeGarantie { get; set; }
        public Single NumOrdre { get; set; }    
        public string CodeTypeValeur { get; set; }     
        public string LibelleTypeValeur { get; set; }
        public static explicit operator LigneTypeValeur(TypeValeurDto typeValeurDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<TypeValeurDto, LigneTypeValeur>().Map(typeValeurDto);
        }
    }  
}