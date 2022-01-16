using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Regularisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.OP.OP_MVC.Models.Regularisation
{
    public interface IRegulModel
    {
        RegularisationContext Context { get; set; }

        string AddParamValue { get; }

        IdContratDto IdContrat { get; }

        long RgId { get; }

        long LotId { get; }
    }
}
