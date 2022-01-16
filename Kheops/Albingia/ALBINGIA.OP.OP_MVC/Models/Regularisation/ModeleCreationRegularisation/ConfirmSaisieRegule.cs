using EmitMapper;
using OP.WSAS400.DTO.Regularisation;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegularisation
{
    public class ConfirmSaisieRegule
    {
        public string CodeGar { get; set; }
        public string LibGar { get; set; }
        public Double MntHT { get; set; }
        //public Double MntTaxe { get; set; }
        //public Double CodeGar { get; set; }
        public Double AttentatHT { get; set; }


        public static explicit operator ConfirmSaisieRegule(ConfirmSaisieReguleDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ConfirmSaisieReguleDto, ConfirmSaisieRegule>().Map(modeleDto);
        }

        public static ConfirmSaisieReguleDto LoadDto(ConfirmSaisieRegule modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ConfirmSaisieRegule, ConfirmSaisieReguleDto>().Map(modele);
        }
    }
}