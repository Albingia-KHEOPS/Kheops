using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Regularisation;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleCreationAvenantPage : MetaModelsBase
    {
        public string TypeAvt { get; set; }
        public string ModeAvt { get; set; }
        public DateTime? EffetGaranties { get; set; }
        public string HeureEffetGaranties { get; set; }
        public DateTime? FinEffet { get; set; }
        public TimeSpan? FinEffetHeure { get; set; }
        public DateTime? Echeance { get; set; }

        public ModeleAvenantModification AvenantModification { get; set; }
        public ModeleAvenantResiliation AvenantResiliation { get; set; }
        public ModeleAvenantRemiseVigueur AvenantRemiseEnVigueur { get; set; }
        public List<ModeleAvenantAlerte> Alertes { get; set; }

        public string LibTypeContrat { get; set; }

        //public RegularisationContext Context { get; set; }

        public IdContratDto IdContrat
        {
            get { return new IdContratDto { CodeOffre = CodePolicePage, Version = Int32.Parse(VersionPolicePage), Type = TypePolicePage }; }
        }

        public RegularisationBandeauContratDto BandeauContrat { get; set; }
    }
}