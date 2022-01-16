using Albingia.Kheops.OP.Domain;
using ALBINGIA.Framework.Common.Tools;
using System.Collections.Generic;

namespace Albingia.Kheops.OP.Application.Port.Driven {
    public interface ISinistreRepository {
        PagingList<Sinistre> GetAll(string[] branches, int page = 0, int codePreneur = 0, int nbAnnees = 3);
        IEnumerable<Sinistre> GetAllOfPreneur(int codePreneur);
        decimal GetSumOfChargementsSinistresPreneur(int codePreneur);
    }
}
