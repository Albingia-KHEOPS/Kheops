using EmitMapper;
using OP.WSAS400.DTO.ParametreGaranties;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamGaranties
{
    public class LigneFamilleReassurance
    {
        public string AdditionalParam { get; set; }
        
        public string CodeGarantie { get; set; }
        public string CodeBranche { get; set; }
        public string LibelleBranche { get; set; }
        public string CodeSousBranche { get; set; }
        public string LibelleSousBranche { get; set; }
        public string CodeCategorie { get; set; }
        public string LibelleCategorie { get; set; }
        public string CodeFamille { get; set; }
        public string LibelleFamille { get; set; }  
        public static explicit operator LigneFamilleReassurance(FamilleReassuranceDto familleReassuranceDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<FamilleReassuranceDto, LigneFamilleReassurance>().Map(familleReassuranceDto);
        }
    }
}