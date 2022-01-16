using EmitMapper;
using OP.WSAS400.DTO.Inventaires;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesInventaire
{
    public class ModeleInventaireCPVille_JSON
    {
        public string CodePostal { get; set; }
        public string Ville { get; set; }

        public static explicit operator ModeleInventaireCPVille_JSON(CPVilleDto data)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<CPVilleDto, ModeleInventaireCPVille_JSON>().Map(data);
        }
    }
}