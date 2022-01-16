using System;
using System.ComponentModel.DataAnnotations;

namespace OP.WSAS400.DTO
{
    public class RemiseEnVigueurDto
    {
        [Display(Name = "PARET")]
        public string Result { get; set; }

        [Display(Name = "PATYP")]
        public string Type { get; set; }

        [Display(Name = "PAIPB")]
        public string CodeContrat { get; set; }

        [Display(Name = "PAALX")]
        public Int32 Version { get; set; }

        [Display(Name = "PAARIPK")]
        public Int32 PrimeReglee { get; set; }

        [Display(Name = "PAPKRD")]
        public Int32 PrimeReglementDate { get; set; }

        [Display(Name = "PASUDD")]
        public Int32 SuspDebDate { get; set; }

        [Display(Name = "PASUDH")]
        public Int32 SuspDebH { get; set; }

        [Display(Name = "PASUFD")]
        public Int32 SuspFinDate { get; set; }

        [Display(Name = "PASUFH")]
        public Int32 SuspFinH { get; set; }

        [Display(Name = "PARSDD")]
        public Int32 ResileDebDate { get; set; }

        [Display(Name = "PARSDH")]
        public Int32 ResileDebH { get; set; }
    }
}
