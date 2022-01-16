using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamCible;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleParamCiblePage : MetaModelsBase
    {       
        //Critères de recherche/ajout
        public string Code { get; set; }
        public string Description { get; set; }

        public List<LigneCible> ListeCibles { get; set; }

        public DetailsCible DetailsBody { get; set; }       
    }
}