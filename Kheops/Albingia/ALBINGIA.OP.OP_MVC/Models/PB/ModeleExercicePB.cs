using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.PB
{
    public class ModeleExercicePB
    {
        public DateTime PeriodeDeb { get; set; }
        public DateTime PeriodeFin { get; set; }

        public string Libelle
        {
            get
            {
                return PeriodeDeb.ToShortDateString() + " - " + PeriodeFin.ToShortDateString();
            }
        }
    }
}