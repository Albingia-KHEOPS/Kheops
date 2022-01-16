using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleTraites
{
    [Serializable]
    public class ModeleTraiteUpdate
    {
        public string LCIValeur { get; set; }
        public string LCIUnite { get; set; }
        public string LCIType { get; set; }
        public bool LCIIndexee { get; set; }

        public int CodeRisque { get; set; }
        public string CodeRsqVentilation { get; set; }
        public string ValueRsq { get; set; }
        public bool SelectedRsq { get; set; }

        public string CodeVentilation { get; set; }
        public Int64 IdVentilation { get; set; }
        public string ValueVen { get; set; }
        public bool SelectedVen { get; set; }
    }
}