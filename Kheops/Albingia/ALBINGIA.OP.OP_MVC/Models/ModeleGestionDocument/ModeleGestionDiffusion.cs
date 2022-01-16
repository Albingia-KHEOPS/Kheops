using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleGestionDocument
{
    [Serializable]
    public class ModeleGestionDiffusion
    {
        public string TypeDiffusion { get; set; }
        public string Partenaire { get; set; }
        public string Destinataire { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreationDateStr
        {
            get
            {
                if (CreationDate.HasValue)
                {
                    return CreationDate.Value.Day.ToString().PadLeft(2, '0') + "/" + CreationDate.Value.Month.ToString().PadLeft(2, '0') + "/" + CreationDate.Value.Year;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public DateTime? TraitementDate { get; set; }
        public string TraitementDateStr
        {
            get
            {
                if (TraitementDate.HasValue)
                {
                    return TraitementDate.Value.Day.ToString().PadLeft(2, '0') + "/" + TraitementDate.Value.Month.ToString().PadLeft(2, '0') + "/" + TraitementDate.Value.Year;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}