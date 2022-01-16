using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleCreationAttestation
{
    public class ModeleAttestationGarantieNiv3
    {
        public Int64 IdGaran { get; set; }
        public string CodeGarantie { get; set; }
        public string LibelleGarantie { get; set; }
        public string Montant { get; set; }
        public string Unite { get; set; }
        public string LibUnite { get; set; }
        public string Base { get; set; }
        public DateTime? DateDebut { get; set; }
        public string DateDebutStr
        {
            get
            {
                if (DateDebut.HasValue)
                    return DateDebut.Value.Day.ToString().PadLeft(2, '0') + "/" + DateDebut.Value.Month.ToString().PadLeft(2, '0') + "/" + DateDebut.Value.Year;
                else
                    return string.Empty;
            }
        }
        public DateTime? DateFin { get; set; }
        public string DateFinStr
        {
            get
            {
                if (DateFin.HasValue)
                    return DateFin.Value.Day.ToString().PadLeft(2, '0') + "/" + DateFin.Value.Month.ToString().PadLeft(2, '0') + "/" + DateFin.Value.Year;
                else
                    return string.Empty;
            }
        }
        public bool IsUsed { get; set; }
        public bool IsShown { get; set; }
        public List<ModeleAttestationGarantieNiv4> Garanties { get; set; }
    }
}