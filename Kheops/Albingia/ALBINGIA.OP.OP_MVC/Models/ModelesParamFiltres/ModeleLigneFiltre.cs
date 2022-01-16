using OP.WSAS400.DTO.ParametreFiltre;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamFiltres
{
    public class ModeleLigneFiltre
    {
        public Int64 GuidId { get; set; }
        public string CodeFiltre { get; set; }
        public string DescriptionFiltre { get; set; }
        public string ModeSaisie { get; set; }
        public string AdditionalParam { get; set; }

        public static explicit operator ModeleLigneFiltre(ModeleFiltreLigneDto filtreLigneDto)
        {
            var toReturn = new ModeleLigneFiltre()
            {
                CodeFiltre = filtreLigneDto.Code,
                DescriptionFiltre = filtreLigneDto.Libelle,
                GuidId = filtreLigneDto.Id
            };
            return toReturn;
        }
    }
}