using EmitMapper;
using OP.WSAS400.DTO.ParametreCibles;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamCible
{
    public class LigneCible
    {
        public int GuidId { get; set; }
        public string Code { get; set; }
        public string Libelle { get; set; }
        public string Grille { get; set; }
        public string LibGrille { get; set; }

        public static explicit operator LigneCible(ParamCiblesDto paramListCiblesDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamCiblesDto, LigneCible>().Map(paramListCiblesDto);
        }
    }
}