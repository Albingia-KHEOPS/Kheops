using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesFraisAccessoires;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleFraisAccessoiresPage : MetaModelsBase
    {
        /// <summary>
        /// context de la page a affecter dans les hidden inputs
        /// </summary>
        public string Albcontext { get; set; }

        public FraisAccessoiresModel Filtre;
        public List<FraisAccessoiresModel> List;
    }
}