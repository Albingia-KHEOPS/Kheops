using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Contracts {
    public interface IRepositoryCache {
        void InitCache();
        void ResetCache();
    }
}
