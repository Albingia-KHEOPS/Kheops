using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesLogTrace;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleLogRecupParamCiblePage : MetaModelsBase
    {
        /// <summary>
        /// context de la page a affecter dans les hidden inputs
        /// </summary>
        public string Albcontext { get; set; }

        public LogParamCibleRecupModel Filtre;
        public List<LogParamCibleRecupModel> ListLog;
    }
}