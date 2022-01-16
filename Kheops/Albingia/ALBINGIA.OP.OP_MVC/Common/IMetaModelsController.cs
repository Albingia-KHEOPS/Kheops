using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.OP.OP_MVC.Common {
    public interface IMetaModelsController {
        MetaModelsBase Model { get; }
        bool IsReadonly { get; }
        bool IsModifHorsAvenant { get; }
        bool IsBackOfficeContext { get; }
        bool? IsAvnDisabled { get; }
    }
}
