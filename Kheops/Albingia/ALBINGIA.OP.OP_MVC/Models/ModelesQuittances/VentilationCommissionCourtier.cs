using EmitMapper;
using OP.WSAS400.DTO.Cotisations;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    public class VentilationCommissionCourtier
    {
        public string CodeCourtier;
        public string LibelleCourtier;
        public Double Repartition;
        public Double HorsCATNAT;
        public Double CATNAT;
        public Double Total;

        public static explicit operator VentilationCommissionCourtier(QuittanceVentilationCommissionCourtierDto courtierDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<QuittanceVentilationCommissionCourtierDto, VentilationCommissionCourtier>().Map(courtierDto);
        }
    }
}