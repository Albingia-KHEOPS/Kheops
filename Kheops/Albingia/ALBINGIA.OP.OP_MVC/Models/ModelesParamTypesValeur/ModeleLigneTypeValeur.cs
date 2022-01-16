using OP.WSAS400.DTO.ParametreTypeValeur;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamTypesValeur
{
    public class ModeleLigneTypeValeur
    {        
        public string AdditionalParam { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
            
        public static explicit operator ModeleLigneTypeValeur(ModeleLigneTypeValeurDto ligneTypeValeurDto)
        {
            var toReturn = new ModeleLigneTypeValeur()
            {
                Code = ligneTypeValeurDto.Code,
                Description = ligneTypeValeurDto.Description              
            };            
            return toReturn;
        }
    }
}