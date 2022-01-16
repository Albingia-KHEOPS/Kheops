using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.DataAccess {
    public interface IFolderAct {
        string CodeOffre { get; }
        int Version { get; }
        string Type { get; }
        int NumeroAvenant { get; }
        string User { get; }
        string ActeGestion { get; }
        DateTime Now { get; }
        string Name { get; }
    }
}
