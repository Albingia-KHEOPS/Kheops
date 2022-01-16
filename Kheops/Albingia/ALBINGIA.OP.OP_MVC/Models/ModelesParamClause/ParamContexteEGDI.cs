using EmitMapper;
using OP.WSAS400.DTO.ParametreClauses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamClause
{
    public class ParamContexteEGDI
    {
        public string CodeService { get; set; }
        [Display(Name="Service")]
        public string Service { get; set; }
        public string CodeActeGestion { get; set; }
        [Display(Name = "Acte de gestion")]
        public string ActeGestion { get; set; }
        public string CodeEtape { get; set; }
        [Display(Name = "Etape")]
        public string Etape { get; set; }
        public string CodeContexte { get; set; }
        [Display(Name = "Contexte")]
        public string Contexte { get; set; }
        public List<ParamListParam> ListEGDI { get; set; }

        public static explicit operator ParamContexteEGDI(ParamContexteEGDIDto paramContexteEGDIDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamContexteEGDIDto, ParamContexteEGDI>().Map(paramContexteEGDIDto);
        }

        public static ParamContexteEGDIDto LoadDto(ParamContexteEGDI modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamContexteEGDI, ParamContexteEGDIDto>().Map(modele);
        }
    }
}