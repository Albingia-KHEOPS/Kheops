using EmitMapper;
using OP.WSAS400.DTO.ParametreClauses;
using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamClause
{
    public class ParamRattachSaisie
    {
        public bool ModifMode { get; set; }
        public int CodeRattachement { get; set; }
        public int CodeAttachClause { get; set; }
        public string CodeAttachService { get; set; }
        [Display(Name = "Service")]
        public string AttachService { get; set; }
        public string CodeAttachActeGestion { get; set; }
        [Display(Name = "Acte de gestion")]
        public string AttachActeGestion { get; set; }
        public string CodeAttachEtape { get; set; }
        [Display(Name = "Etape")]
        public string AttachEtape { get; set; }
        public string CodeAttachContexte { get; set; }
        [Display(Name = "Contexte")]
        public string AttachContexte { get; set; }
        public Int64 IdAttachEGDI { get; set; }
        [Display(Name = "Elément Générateur")]
        public bool AttachEG { get; set; }
        [Display(Name = "Déclencheur Incond.")]
        public bool AttachDI { get; set; }
        [Display(Name = "Code EG/DI")]
        public string CodeAttachEGDI { get; set; }
        [Display(Name = "Clause")]
        public string ClauseNom1 { get; set; }
        public string ClauseNom2 { get; set; }
        public int ClauseNom3 { get; set; }
        public string LibelleClause { get; set; }
        [Display(Name = "Ordre")]
        public Int64 AttachOrdre { get; set; }
        [Display(Name = "Obligatoire")]
        public bool Obligatoire { get; set; }
        [Display(Name = "Proposée")]
        public bool Proposee { get; set; }
        [Display(Name = "Suggérée")]
        public bool Suggeree { get; set; }
        [Display(Name = "Impression Annexe")]
        public bool ImpressAnnexe { get; set; }
        [Display(Name = "Code annexe")]
        public string CodeAnnexe { get; set; }
        [Display(Name = "Style Word")]
        public string StyleWord { get; set; }
        [Display(Name = "Taille")]
        public string Taille { get; set; }
        [Display(Name = "Gras")]
        public bool Gras { get; set; }
        [Display(Name = "Souligne")]
        public bool Souligne { get; set; }
        public int Version { get; set; }

        public static explicit operator ParamRattachSaisie(ParamRattachSaisieDto paramRattachSaisieDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamRattachSaisieDto, ParamRattachSaisie>().Map(paramRattachSaisieDto);
        }

        public static ParamRattachSaisieDto LoadDto(ParamRattachSaisie modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamRattachSaisie, ParamRattachSaisieDto>().Map(modele);
        }

    }
}