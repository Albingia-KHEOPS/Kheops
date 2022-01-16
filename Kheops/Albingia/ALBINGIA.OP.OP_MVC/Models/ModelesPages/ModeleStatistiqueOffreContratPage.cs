using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleStatisque;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleStatistiqueOffreContratPage : MetaModelsBase
    {
        /// <summary>
        /// context de la page a affecter dans les hidden inputs
        /// </summary>
        public string Albcontext { get; set; }

        public StatOffreContratFiltreModel Filtre;
        public List<StatOffreContratModel> ListStat;
    }
}