using EmitMapper;
using OP.WSAS400.DTO.Quittance;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    public class QuittanceCalculForce
    {
        public QuittanceForceTotal ForceTotal { get; set; }
        public QuittanceForceFormule ForceFormule { get; set; }

        public static explicit operator QuittanceCalculForce(QuittanceForceDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<QuittanceForceDto, QuittanceCalculForce>().Map(modeleDto);
        }


    }
}