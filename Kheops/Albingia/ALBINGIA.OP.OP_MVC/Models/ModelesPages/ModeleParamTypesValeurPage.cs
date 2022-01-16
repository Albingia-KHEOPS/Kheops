using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamTypesValeur;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleParamTypesValeurPage : MetaModelsBase
    {
        public string AdditionalParam { get; set; }
        public List<ModeleLigneTypeValeur> ListeTypesValeur { get; set; }     
    }
}