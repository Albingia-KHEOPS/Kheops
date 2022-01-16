using EmitMapper;
using OP.WSAS400.DTO.Engagement;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesConnexite
{
    public class ModeleTypeConnexite
    {
        public string CodeConnexite { get; set; }
        public string LibConnexite { get; set; }
        public List<ModeleConnexite> Connexites { get; set; }

        public static explicit operator ModeleTypeConnexite(TypeConnexiteDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<TypeConnexiteDto, ModeleTypeConnexite>().Map(modeleDto);
        }

        public static TypeConnexiteDto LoadDto(ModeleTypeConnexite modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleTypeConnexite, TypeConnexiteDto>().Map(modele);
        }
    }
}