using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    [Serializable]
    public class FormulesInformation
    {
        public List<Formule> Formules { get; set; }
        public Commission Commission { get; set; }
        public Totaux Totaux { get; set; }        
    }
}