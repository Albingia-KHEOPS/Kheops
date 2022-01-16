using EmitMapper;
using OP.WSAS400.DTO.Cotisations;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    public class VentilationDetailleeTaxe
    {
        public string CodeTaxe;
        public string LibelleTaxe;
        public Double BaseTaxable;
        public Double MontantTaxes;

        public static explicit operator VentilationDetailleeTaxe(QuittanceVentilationDetailleeTaxeDto taxeDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<QuittanceVentilationDetailleeTaxeDto, VentilationDetailleeTaxe>().Map(taxeDto);
        }
    }
}