using EmitMapper;
using OP.WSAS400.DTO.Cotisations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    public class CoAssureurGarantie
    {
        public string Code { get; set; }
        public string Libelle { get; set; }
        public double Part { get; set; }
        public double FraisAperition { get; set; }
        public double HTHCatnat { get; set; }
        public double HTCatnat { get; set; }
        public double HTTotal { get; set; }
        public double CommHCatnat { get; set; }
        public double CommCatnat { get; set; }
        public double CommTotal { get; set; }

        public static explicit operator CoAssureurGarantie(QuittanceTabAperitionLigneDto dataDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<QuittanceTabAperitionLigneDto, CoAssureurGarantie>().Map(dataDto);
        }
    }
}