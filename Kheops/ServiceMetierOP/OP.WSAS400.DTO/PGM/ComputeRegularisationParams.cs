using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.PGM
{
    public class ComputeRegularisationParams
    {
        [Display(Name = "P0RET")]
        public string Result { get; set; }

        [Display(Name = "P0TYP")]
        public string TypeContrat { get; set; }

        [Display(Name = "P0ALX")]
        public int VersionContrat { get; set; }

        [Display(Name = "P0IPB")]
        public string CodeOffre { get; set; }

        [Display(Name = "P0RSQ")]
        public long Risque { get; set; }

        [Display(Name = "P0FOR")]
        public int Formule { get; set; }

        [Display(Name = "P0GAR")]
        public string Garantie { get; set; }

        [Display(Name = "P0ID")]
        public long Id { get; set; }

        [Display(Name = "P0MRG")]
        public string Mode { get; set; }

        [Display(Name = "P0ACC")]
        public string Acces { get; set; }

        [Display(Name = "P0IDA")]
        public long ContextId { get; set; }

        [Display(Name = "P0TOP")]
        public string Top { get; set; }
    }
}
