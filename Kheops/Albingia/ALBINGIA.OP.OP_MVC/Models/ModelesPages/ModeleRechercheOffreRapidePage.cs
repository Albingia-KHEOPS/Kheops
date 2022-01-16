using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleRechercheOffreRapide;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleRechercheOffreRapidePage : MetaModelsBase
    {
        public ModeleOffreRapideResult Result { get; set; }


        public string CodeOffre { get; set; }
        public int? Version { get; set; }
        public IList<string> TypeOffres { get; set; }
        public int? CodeAvenant { get; set; }
        public string TypeTraitement { get; set; }
        public DateTime? DateEffetAvnDebut { get; set; }
        public DateTime? DateEffetAvnFin { get; set; }
        public string CodePeriodicite { get; set; }
        public string CodeBranche { get; set; }
        public string CodeCible { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }


        public List<AlbSelectListItem> TypeTraitements { get; set; }
        public List<AlbSelectListItem> Branches { get; set; }
        public List<AlbSelectListItem> Cibles { get; set; }
        public List<AlbSelectListItem> Periodicites { get; set; }

    }
}