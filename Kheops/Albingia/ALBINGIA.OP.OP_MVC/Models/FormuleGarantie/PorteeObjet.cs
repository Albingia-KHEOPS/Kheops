using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.FormuleGarantie
{
    public class PorteeObjet
    {
        public bool IsSelected { get; set; }

        public int NumObjet { get; set; }

        public string LabelObjet { get; set; }

        public decimal Valeur { get; set; }

        public string CodeUnite { get; set; }

        public string CodeType { get; set; }

        public decimal ValeurPortee { get; set; }

        public string UniteTaux { get; set; }

        public string TypeCalculPortee { get; set; }

        public decimal PrimeMntCalcule { get; set; }
    }
}