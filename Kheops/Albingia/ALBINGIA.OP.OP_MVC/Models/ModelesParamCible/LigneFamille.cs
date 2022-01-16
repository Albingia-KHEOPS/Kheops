using OP.WSAS400.DTO.ParametreFamilles;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamCible
{
    public class LigneFamille
    {
        public string AdditionalParam { get; set; }
        public string RestrictionParam { get; set; }
        public string CodeConcept { get; set; }
        public string CodeFamille { get; set; }
        public string LibelleFamille { get; set; }
        public string Famille { get; set; }
        public static explicit operator LigneFamille(ParamFamilleDto parametreDto)
        {
            var ligneFamille = new LigneFamille
            {
                CodeConcept = parametreDto.CodeConcpet,
                CodeFamille = parametreDto.CodeFamille,
                LibelleFamille = parametreDto.LibelleFamille
            };
            return ligneFamille;
        }
    }
    
}