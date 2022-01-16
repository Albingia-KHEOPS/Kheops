using EmitMapper;
using OP.WSAS400.DTO.Avenant;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant
{
    public class ModeleAvenantRegularisation
    {
        public string ErrorAvt { get; set; }
        public string TypeAvt { get; set; }
        public Int64 NumInterneAvt { get; set; }
        public Int64 NumAvt { get; set; }
        public string DescriptionAvt { get; set; }
        public string ObservationAvt { get; set; }
        public string MotifAvt { get; set; }
        public static explicit operator ModeleAvenantRegularisation(AvenantRegularisationDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AvenantRegularisationDto, ModeleAvenantRegularisation>().Map(modeleDto);
        }

        public static AvenantRegularisationDto LoadDto(ModeleAvenantRegularisation modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleAvenantRegularisation, AvenantRegularisationDto>().Map(modele);
        }

    }
}