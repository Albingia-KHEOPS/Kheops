using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDetailsRisque
{
    public class ModeleListeNomenclatures
    {
        public string Nomenclature { get; set; }
        public List<AlbSelectListItem> Nomenclatures { get; set; }
        public string LibelleNomenclature { get; set; }
        public string NiveauNomenclature { get; set; }     
        public bool NomenclatureReadOnly { get; set; }
        public bool ReadOnly { get; set; }
        public string NumeroCombo { get; set; }
    }
}