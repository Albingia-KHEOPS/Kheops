﻿using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamCible
{
    public class ModeleRechercheFamille
    {
        public List<LigneFamille> ListFamilles;
        public int NbCount { get; set; }
        public int StartLigne { get; set; }
        public int EndLigne { get; set; }
        public int PageNumber { get; set; }
        public int LineCount { get; set; }
        public string Order { get; set; }
        public string concept { get; set; }
        public string famille { get; set; }
       

    }
}