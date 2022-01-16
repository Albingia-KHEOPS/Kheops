using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    public class Inventaire
    {
        public String NumAttachement { get; set; }
        public String NumDescription { get; set; }
        public String Code { get; set; }
        public String Descriptif { get; set; }
        public Parametre Type { get; set; }
        public String Description { get; set; }
        public String Valeur { get; set; }
        public Parametre UniteLst { get; set; }
        public Parametre TypeLst { get; set; }
        public bool ActiverReport { get; set; }
        public String PerimetreApplication { get; set; }
        public List<Concert> Concerts { get; set; }
    }
}
