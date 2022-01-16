using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesLCIFranchise;
using ALBINGIA.OP.OP_MVC.Models.ModelesRisque;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleInformationsSpecifiquesRisquesPage : MetaModelsBase
    {
        public string txtParamRedirect { get; set; }
        public string IsISLoaded { get; set; }
        public string Code { get; set; }
        [Display(Name = "Indice Réf.")]
        public string IndiceRef { get; set; }
        public string IndiceRefLibelle { get; set; }
        [Display(Name = "Valeur")]
        public string Valeur { get; set; }
        [Display(Name = "Risque Indexé")]
        public bool RisqueIndexe { get; set; }
        [Display(Name = "LCI")]
        public bool LCI { get; set; }
        [Display(Name = "Franchise")]
        public bool Franchise { get; set; }
        [Display(Name = "Assiette")]
        public bool Assiette { get; set; }
        [Required]
        [Display(Name = "Regime Taxe *")]
        public string RegimeTaxe { get; set; }
        public List<AlbSelectListItem> RegimesTaxe { get; set; }
        [Display(Name = "CATNAT")]
        public bool CATNAT { get; set; }
        public bool ForceAllowCatnat { get; set; }
        public string DescriptifRisque { get; set; }
        public List<ModeleOpposition> ListOppositions { get; set; }
        [Display(Name = "Régularisable")]
        public bool IsRegularisable { get; set; }
        public string TypeRegularisation { get; set; }
        public List<AlbSelectListItem> ListTypesRegularisation { get; set; }
        [Display(Name = "Taux d'appel (%)")]
        public int TauxAppel { get; set; }
        public string PartBenef { get; set; }
        public string PartBenefGenerale { get; set; }
        public List<AlbSelectListItem> ListPbBNS { get; set; }
        public int NbYear { get; set; }
        public int Ristourne { get; set; }
        public int CotisRetenue { get; set; }
        public int Seuil { get; set; }
        public int TauxComp { get; set; }

        public Single TauxMaxi { get; set; }
        public double PrimeMaxi { get; set; }
        #region LCIFranchise
        public Framework.Common.Constants.NomsInternesEcran NomEcran { get; set; }
        public ModeleLCIFranchise LCIRisque { get; set; }
        public ModeleLCIFranchise FranchiseRisque { get; set; }
        #endregion

        public List<AlbSelectListItem> LstTypesDest { get; set; }

        public bool IsHorsAvnRegularisable { get; set; }

        /// <summary>
        /// B3203
        /// Régularisation en modif hors avenant
        /// </summary>
        public bool CanDoRegularisable { get; set; }

        public ModeleInformationsSpecifiquesRisquesPage()
        {
            Code = string.Empty;
            IndiceRef = string.Empty;
            Valeur = string.Empty;
            RisqueIndexe = false;
            LCI = false;
            Franchise = false;
            Assiette = false;
            RegimeTaxe = string.Empty;
            RegimesTaxe = new List<AlbSelectListItem>();
            CATNAT = false;
        }
    }
}