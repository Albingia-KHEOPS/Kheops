using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesGarantieModeles;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleGarantieModelePage : MetaModelsBase
    {
        public ModeleRechercheGarantieModele RechercheModelesGarantie { get; set; }
        public List<ModeleGarantieModele> Modeles { get; set; }
        public ModeleGarantieModele Modele { get; set; }

        public ModeleGarantieModelePage()
        {
            this.RechercheModelesGarantie = new ModeleRechercheGarantieModele();
            this.Modeles = new List<ModeleGarantieModele>();
            this.Modele = new ModeleGarantieModele();
        }
    }
}