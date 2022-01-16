using EmitMapper;
using OP.WSAS400.DTO.Offres.Risque.Inventaire;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesInventaire
{
    [Serializable]
    public class ModeleInventaire
    {
        public string Code { get; set; }
        public string Descriptif { get; set; }

        public ModeleInventaire()
        {
            Code = string.Empty;
            Descriptif = string.Empty;
        }

        public static explicit operator ModeleInventaire(InventaireDto inventaireDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<InventaireDto, ModeleInventaire>().Map(inventaireDto);
        }
    }
}