using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Port.Driven {
    public interface INomsCourtiersRepository {
        IDictionary<int, string> GetNomsCabinets(IEnumerable<int> codeList);
    }
}
