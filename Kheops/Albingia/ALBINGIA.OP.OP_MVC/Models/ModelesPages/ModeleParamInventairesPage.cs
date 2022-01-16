using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamInventaire;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleParamInventairesPage : MetaModelsBase
    {
        //Critères de recherche/ajout
        public string Code { get; set; }
        public string Description { get; set; }

        public List<LigneInventaires> ListeInventaires { get; set; }      
    }
}