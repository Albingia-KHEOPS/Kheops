using ALBINGIA.Framework.Common.Constants;
using System.Collections.Generic;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesRecherche
{
    public class ModeleResultRecherche
    {
        public int NbCount { get; set; }
        public int StartLigne { get; set; }
        public int EndLigne { get; set; }
        public int PageNumber { get; set; }
        public int LineCount { get; set; }
        public List<ModeleListResultRecherche> ListResult { get; set; }
        public AlbConstantesMetiers.CriterParam CritereParam { get; set; }
        public AlbConstantesMetiers.TypeDateRecherche TypeDate { get; set; }
        public AlbConstantesMetiers.TypeAccesRecherche AccesRecherche { get; set; }
        public string CodeBranche { get; set; }
    }
}