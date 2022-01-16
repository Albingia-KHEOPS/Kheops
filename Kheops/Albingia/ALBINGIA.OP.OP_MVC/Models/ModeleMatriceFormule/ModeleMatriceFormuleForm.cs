using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleMatriceFormule
{
    [Serializable]
    public class ModeleMatriceFormuleForm
    {
        public string Code { get; set; }
        public string Libelle { get; set; }
        public string Designation { get; set; }
        public List<ModeleMatriceFormuleOpt> Options { get; set; }

        public bool IsAlertePeriode { get; set; }
        public int AvnCreationFor { get; set; }
        public Int64 CodeRsq { get; set; }
        public bool BlockFormConditions { get; set; }
        public bool SupprForm { get; set; }
    }
}