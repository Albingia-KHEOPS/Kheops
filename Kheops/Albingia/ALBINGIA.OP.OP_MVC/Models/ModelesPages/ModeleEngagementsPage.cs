using ALBINGIA.Framework.Common.Business;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleEngagements;
using ALBINGIA.OP.OP_MVC.Models.ModelesLCIFranchise;
using EmitMapper;
using OP.WSAS400.DTO.Engagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleEngagementsPage : ModeleEngagementBase {
        public override AccessMode? CurrentAccessMode {
            get;
            set;
        }
        [Display(Name = "Nature du contrat")]
        public string Nature { get; set; }
        [Display(Name = "Part Albingia")]
        public string PartAlb { get; set; }
        [Display(Name = "Couverture")]
        public string Couverture { get; set; }
        [Display(Name = "Engagement du")]
        public DateTime? DateDeb { get; set; }
        [Display(Name = "au")]
        public DateTime? DateFin { get; set; }

        public List<ModeleEngagementTraite> Traites { get; set; }

        [Display(Name = "Base totale (€)")]
        public string BaseTotale { get; set; }
        [Display(Name = "Base Albingia (€)")]
        public string BaseAlb { get; set; }
        public string CodePeriode { get; set; }

        [Display(Name = "Commentaires")]
        public string CommentForce { get; set; }
        public bool MontantForce { get; set; }


        #region LCIFranchise
        public ALBINGIA.Framework.Common.Constants.NomsInternesEcran NomEcran { get; set; }
        public ModeleLCIFranchise LCIGenerale { get; set; }
        #endregion

        public ModeleEngagementsPage()
        {

            this.Nature = string.Empty;
            this.PartAlb = string.Empty;
            this.Couverture = string.Empty;

            this.Traites = new List<ModeleEngagementTraite>();

            this.BaseTotale = string.Empty;
            this.BaseAlb = string.Empty;
        }

        public static explicit operator ModeleEngagementsPage(EngagementDto engagementDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<EngagementDto, ModeleEngagementsPage>().Map(engagementDto);
        }

        public static EngagementDto LoadDto(ModeleEngagementsPage modele)
        {
            var engagementDto = ObjectMapperManager.DefaultInstance.GetMapper<ModeleEngagementsPage, EngagementDto>().Map(modele);
            engagementDto.LCIValeur = modele.LCIGenerale.Valeur;
            engagementDto.LCIUnite = modele.LCIGenerale.Unite;
            engagementDto.LCIType = modele.LCIGenerale.Type;
            engagementDto.LCIIndexee = modele.LCIGenerale.IsIndexe;
            engagementDto.LienComplexeLCI = modele.LCIGenerale.LienComplexe;
            return engagementDto;
        }

    }
}