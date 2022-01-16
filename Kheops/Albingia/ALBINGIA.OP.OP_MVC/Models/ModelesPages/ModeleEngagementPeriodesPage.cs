using ALBINGIA.Framework.Common.Business;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModeleEngagements;
using System.Collections.Generic;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages {
    public class ModeleEngagementPeriodesPage : ModeleEngagementBase {
        public override AccessMode? CurrentAccessMode {
            get => Framework.Common.Business.AccessMode.UPDATE;
            set { }
        }
        public string ModeActif { get; set; }
        public List<AlbSelectListItem> ListeModesActif { get; set; }
        public string ModeUtilise { get; set; }
        public List<AlbSelectListItem> ListeModesUtilise { get; set; }
        public List<ModeleEngagementPeriode> EngagementPeriodes { get; set; }        
        public int DebutDernierePeriode { get; set; }
        public int FinDernierePeriode { get; set; }
        public int DateControle { get; set; }
        public int CodePremierePeriode { get; set; }
        public int CodeDernierePeriode { get; set; }
    }
}