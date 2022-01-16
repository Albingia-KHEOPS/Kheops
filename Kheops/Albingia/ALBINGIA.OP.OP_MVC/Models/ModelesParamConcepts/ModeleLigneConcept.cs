using OP.WSAS400.DTO.Offres.Parametres;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamConcepts
{
    public class ModeleLigneConcept
    {
        public string CodeConcept { get; set; }
        public string DescriptionConcept { get; set; }
        public string ModeSaisie { get; set; }
        public string AdditionalParam { get; set; }

        public static explicit operator ModeleLigneConcept(ParametreDto parametreDto)
        {
            var toReturn = new ModeleLigneConcept()
            {
                CodeConcept = parametreDto.Code,
                DescriptionConcept = parametreDto.Libelle
            };
            return toReturn;
        }
    }
}