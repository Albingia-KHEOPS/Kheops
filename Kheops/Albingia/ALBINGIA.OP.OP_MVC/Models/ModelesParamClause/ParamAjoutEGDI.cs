using EmitMapper;
using OP.WSAS400.DTO.ParametreClauses;
using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamClause
{
    public class ParamAjoutEGDI
    {
        public int Code { get; set; }
        public int ModeSaisie { get; set; }
        public string CodeService { get; set; }
        [Display(Name = "Service")]
        public string LibelleService { get; set; }
        public string CodeActGes { get; set; }
        [Display(Name = "Acte de gestion")]
        public string LibelleActGes { get; set; }
        public string CodeEtape { get; set; }
        [Display(Name = "Etape")]
        public string LibelleEtape { get; set; }
        public string CodeContexte { get; set; }
        [Display(Name = "Contexte")]
        public string LibelleContexte { get; set; }
        [Display(Name = "Elément Générateur")]
        public bool EG { get; set; }
        [Display(Name = "Déclencheur Incond.")]
        public bool DI { get; set; }
        [Display(Name = "Code EG/DI")]
        public string CodeEGDI { get; set; }
        [Display(Name = "Ordre")]
        public Int64 NumOrd { get; set; }
        [Display(Name = "Libellé")]
        public string LibelleEGDI { get; set; }
        [Display(Name = "Commentaires")]
        public string Commentaires { get; set; }

        public static explicit operator ParamAjoutEGDI(ParamAjoutEGDIDto paramAjoutEGDIDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamAjoutEGDIDto, ParamAjoutEGDI>().Map(paramAjoutEGDIDto);
        }

        public static ParamAjoutEGDIDto LoadDto(ParamAjoutEGDI modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamAjoutEGDI, ParamAjoutEGDIDto>().Map(modele);
        }
    }
}