using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleSaisieRegulGarantie
{
    public class ModeleDataRegularisation
    {

        public decimal AssietteGarantie { get; set; }
        public string TauxMontantGar { get; set; }
        public string UniteApplique { get; set; }
        public List<AlbSelectListItem> ListeUnitesApplique { get; set; }
        public string CodeTaxe { get; set; }
        public List<AlbSelectListItem> ListeCodesTaxe { get; set; }
        public decimal MontantCotisatisationCalcul { get; set; }
        public decimal TaxesApplique { get; set; }
        public decimal MontantCotisationsHT { get; set; }
        public decimal MontantTaxeCalcul { get; set; }
        public decimal MontantCotisEmises { get; set; }
        public decimal MontantTaxeEmise { get; set; }
        public decimal MontantCotisEmiseForce { get; set; }
        public decimal MontantTaxePrimeEmise { get; set; }
        public decimal coefficient { get; set; }
        public decimal MontantRegularisationHT { get; set; }
        public decimal AttentatGareat { get; set; }
        public decimal MontantForcePrimecalcul { get; set; }

        public bool ForcerA { get; set; }

        #region Propriétés régularisation PB/BNS/BURNER

        public decimal CotisEmises { get; set; }
        public decimal TauxAppel { get; set; }
        public decimal TxRistourneAnticipee { get; set; }
        public decimal CotisDues { get; set; }
        public Int32 NbAnnee { get; set; }
        public decimal CotisRetenues { get; set; }
        public decimal TxCotisRetenues { get; set; }
        public decimal ChargementSinistres { get; set; }
        public decimal PBBNS { get; set; }
        public decimal Ristourne { get; set; }
        public decimal RistourneAnticipee { get; set; }
        public decimal ComptantHorsAtt { get; set; }
        public decimal ComptantAttentat { get; set; }
        public decimal TxAttentat { get; set; }

        #endregion
    }
}