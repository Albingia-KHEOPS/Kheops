using Albingia.Kheops.OP.Domain.Regularisation;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.DTO {
    public class SelectionRegularisationDto : SelectionRegularisation {
        public bool HasItsOwnDateSortie { get; set; }
        public bool IsRCGarantie => CodeGarantie.IsIn(AlbOpConstants.RCFrance, AlbOpConstants.RCExport, AlbOpConstants.RCUSA);
    }
}
