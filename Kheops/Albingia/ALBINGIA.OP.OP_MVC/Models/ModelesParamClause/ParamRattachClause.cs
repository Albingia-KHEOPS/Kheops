using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.ParametreClauses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamClause
{
    public class ParamRattachClause
    {
        public int Code { get; set; }
        public string CodeService { get; set; }
        [Display(Name = "Service")]
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
        [Display(Name = "Elément Générateur")]
        public bool EG { get; set; }
        [Display(Name = "Déclencheur Incond.")]
        public bool DI { get; set; }
        [Display(Name = "Code EG/DI")]
        public string CodeEGDI { get; set; }
        [Display(Name = "Libellé")]
        public string LibelleEGDI { get; set; }
        [Display(Name="Restriction (mot clé et/ou libellé)")]
        public string Restriction { get; set; }
        [Display(Name="Affichage")]
        public string Affichage { get; set; }
        public List<AlbSelectListItem> Affichages { get; set; }

        public List<ParamListClauses> Clauses { get; set; }

        public static explicit operator ParamRattachClause(ParamRattachClauseDto paramRattachClauseDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamRattachClauseDto, ParamRattachClause>().Map(paramRattachClauseDto);
        }

        public static ParamRattachClauseDto LoadDto(ParamRattachClause modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamRattachClause, ParamRattachClauseDto>().Map(modele);
        }

    }
}