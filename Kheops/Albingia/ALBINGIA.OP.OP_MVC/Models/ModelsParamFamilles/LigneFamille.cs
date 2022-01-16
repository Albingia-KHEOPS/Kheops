using OP.WSAS400.DTO.ParametreFamilles;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamFamilles
{
    public class LigneFamille
    {
        public string AdditionalParam { get; set; }
        public string RestrictionParam { get; set; }
        //public int Id { get; set; }
        public string CodeConcpet { get; set; }
        public string CodeFamille { get; set; }
        public string LibelleFamille { get; set; }

        public static explicit operator LigneFamille(ParamFamilleDto parametreDto)
        {
            var ligneFamille = new LigneFamille
            {
                CodeConcpet=parametreDto.CodeConcpet,
                CodeFamille = parametreDto.CodeFamille,                
                LibelleFamille = parametreDto.LibelleFamille
            };
            return ligneFamille;
        }
    }
}