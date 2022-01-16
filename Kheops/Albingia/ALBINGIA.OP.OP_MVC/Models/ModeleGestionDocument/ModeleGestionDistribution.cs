using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleGestionDocument
{
    [Serializable]
    public class ModeleGestionDistribution
    {
        public int CodeDoc { get; set; }
        public string NomDoc { get; set; }
        public List<ModeleGestionDiffusion> Diffusions { get; set; }
    }
}