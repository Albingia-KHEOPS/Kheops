using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesVolets;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleVoletPage : MetaModelsBase
    {
        public ModeleRechercheVolets RechercheVolets { get; set; }
        public List<ModeleVolet> Volets { get; set; }
        public ModeleVolet Volet { get; set; }

        public ModeleVoletPage()
        {
            this.RechercheVolets = new ModeleRechercheVolets();
            this.Volets = new List<ModeleVolet>();
            this.Volet = new ModeleVolet();
        }
    }
}