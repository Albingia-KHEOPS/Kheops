using OP.WSAS400.DTO.Offres.Parametres;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleTransverse
{

    public class ModeleCommonConceptFamilleCode
    {
        public string Code { get; set; }
        public string Libelle { get; set; }

        public static explicit operator ModeleCommonConceptFamilleCode(ParametreDto parametreDto)
        {
            var modeleFamille = new ModeleCommonConceptFamilleCode
            {
                Code = parametreDto.Code,
                Libelle=parametreDto.Libelle
            };
            return modeleFamille;
            
        }       
    }
}