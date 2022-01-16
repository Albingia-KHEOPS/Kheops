using EmitMapper;
using OP.WSAS400.DTO.Engagement;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesConnexite
{
    public class ModeleInfosConnexite
    {
        public List<ModeleTypeConnexite> TypesConnexite { get; set; }

        public static explicit operator ModeleInfosConnexite(InfoConnexiteDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<InfoConnexiteDto, ModeleInfosConnexite>().Map(modeleDto);
        }

        public static InfoConnexiteDto LoadDto(ModeleInfosConnexite modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleInfosConnexite, InfoConnexiteDto>().Map(modele);
        }
    }
}