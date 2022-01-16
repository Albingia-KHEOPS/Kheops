using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant;
using EmitMapper;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Regularisation;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegularisation
{
    public class ModeleRegularisation 
    {
        public List<ModeleLigneRegularisation> Regularisations { get; set; }
        public List<ModeleAvenantAlerte> Alertes { get; set; }
        public List<ParametreDto> TypesContrat { get; set; }

        public static explicit operator ModeleRegularisation(RegularisationDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<RegularisationDto, ModeleRegularisation>().Map(modeleDto);
        }

        public static RegularisationDto LoadDto(ModeleRegularisation modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleRegularisation, RegularisationDto>().Map(modele);
        }
    }
}