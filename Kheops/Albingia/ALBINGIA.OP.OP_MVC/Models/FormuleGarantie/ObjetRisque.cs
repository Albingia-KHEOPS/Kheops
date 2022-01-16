using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.FormuleGarantie
{
    public class ObjetRisque : Albingia.Kheops.DTO.ObjetDto {
        public bool? IsApplique { get; set; } = null;
    }
}