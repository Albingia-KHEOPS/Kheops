using ALBINGIA.OP.OP_MVC.Models.ModelesRisque;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesFormuleGarantie
{
    [Serializable]
    public class ModeleFormuleGarantieLstObjRsq
    {
        public string TableName { get; set; }
        public List<ModeleRisque> Risques { get; set; }
        public bool IsReadonly { get; set; }

        public ModeleFormuleGarantieLstObjRsq()
        {
            this.TableName = string.Empty;
            this.Risques = new List<ModeleRisque>();
        }
    }
}