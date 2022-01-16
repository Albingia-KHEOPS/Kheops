using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.FormuleGarantie
{
    public class Bloc : VoletBase
    {
        public ICollection<Garantie> Garanties { get; set; }
    }
}