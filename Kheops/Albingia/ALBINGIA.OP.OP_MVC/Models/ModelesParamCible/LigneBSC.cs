using EmitMapper;
using OP.WSAS400.DTO.ParametreCibles;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamCible
{
    public class LigneBSC
    {    

        public int GuidId { get; set; }

        public string CodeBranche { get; set; }
        public string LibelleBranche { get; set; }

        public string CodeSousBranche { get; set; }
        public string LibelleSousBranche { get; set; }

        public string CodeCategorie { get; set; }
        public string LibelleCategorie { get; set; }

        public string Templates { get; set; }

        public static explicit operator LigneBSC(ParamListCibleBranchesDto paramListBranchesCibleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamListCibleBranchesDto, LigneBSC>().Map(paramListBranchesCibleDto);
        }

    }
}