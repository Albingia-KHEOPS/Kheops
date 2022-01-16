using Albingia.Kheops.OP.Application.Port.Driver.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Port.Driver {
    public interface IEtapePort {
        void UpdateEtapes(ContextEtapeDto contextEtape);
    }
}