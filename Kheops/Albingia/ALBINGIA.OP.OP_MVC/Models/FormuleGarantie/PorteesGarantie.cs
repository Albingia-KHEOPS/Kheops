using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.FormuleGarantie
{
    public class PorteesGarantie
    {
        public PorteesGarantie()
        {
            ObjetsRisque = new List<PorteeObjet>();
            Actions = new List<LabeledValue>();
            UnitesTaux = new List<LabeledValue>();
            TypesCalculs = new List<LabeledValue>();
        }

        public string Code { get; set; }

        public string Designation { get; set; }

        public string CodeAction { get; set; }

        public string AlimentationAssiette { get; set; }

        public string CodeRisque { get; set; }

        public string DesignationRisque { get; set; }

        public ICollection<PorteeObjet> ObjetsRisque { get; set; }

        public IEnumerable<LabeledValue> Actions { get; set; }

        public IEnumerable<LabeledValue> UnitesTaux { get; set; }

        public IEnumerable<LabeledValue> TypesCalculs { get; set; }

        public bool ReportCalcul { get; set; }
    }
}