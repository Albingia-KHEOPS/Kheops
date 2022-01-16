using ALBINGIA.Framework.Common.Business;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesLCIFranchise;
using ALBINGIA.OP.OP_MVC.Models.ModeleTraites;
using EmitMapper;
using OP.WSAS400.DTO.Traite;
using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleEngagementTraitePage : ModeleEngagementBase {
        public override AccessMode? CurrentAccessMode { get; set; }

        [Display(Name = "Nom du traité")]
        public string NomTraite { get; set; }
        public string CodeTraite { get; set; }
        [Display(Name = "Début d'effet")]
        [Required]
        public DateTime? DDebutEffet { get; set; }
        [Display(Name = "Fin d'effet")]
        public DateTime? DFinEffet { get; set; }

        public ModeleTraiteInfoRsqVen TraiteInfoRsqVen { get; set; }

        [Display(Name = "Enga. total (€)")]
        public string EngagementTotal { get; set; }
        [Display(Name = "Part Albingia (%)")]
        public string PartAlb { get; set; }
        [Display(Name = "Enga. Albingia (€)")]
        public string EngagementAlbingia { get; set; }

        [Display(Name = "Commentaires")]
        public string CommentForce { get; set; }
        public bool MontantForce { get; set; }
        public string CodePeriodeEng { get; set; }
        #region LCIFranchise
        public ALBINGIA.Framework.Common.Constants.NomsInternesEcran NomEcran { get; set; }
        public ModeleLCIFranchise LCIGenerale { get; set; }
        public bool IsAjax { get; internal set; }
        #endregion

        public ModeleEngagementTraitePage()
        {
            TraiteInfoRsqVen = new ModeleTraiteInfoRsqVen();
        }

        public static explicit operator ModeleEngagementTraitePage(TraiteDto traiteDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<TraiteDto, ModeleEngagementTraitePage>().Map(traiteDto);
        }

        public static TraiteDto LoadDto(ModeleEngagementTraitePage modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleEngagementTraitePage, TraiteDto>().Map(modele);
        }

    }
}