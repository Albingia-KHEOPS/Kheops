using EmitMapper;
using OP.WSAS400.DTO.Cotisations;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    public class VentilationDetailleeGarantie
    {
        public string CodeGarantie;
        public string LibelleGarantie;
        public Double HorsCATNAT;
        public Double CATNAT;
        public Double MontantTaxes;
        public Double MontantTTC;     

        public static explicit operator VentilationDetailleeGarantie(QuittanceVentilationDetailleeGarantieDto garantieDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<QuittanceVentilationDetailleeGarantieDto, VentilationDetailleeGarantie>().Map(garantieDto);
        }
    }
}