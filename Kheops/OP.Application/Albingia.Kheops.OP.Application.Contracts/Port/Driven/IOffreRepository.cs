using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Port.Driven {
    public interface IOffreRepository {
        PagingList<Affaire> GetRelancesByGESorSOU(string gestionnaire, string souscripteur, int page = 0);
        Affaire GetSingleRelance(AffaireId affaireId);
        void UpdateSituationsRelances(IEnumerable<Affaire> affaires, string user);
        void UpdateDelaisRelances(IEnumerable<Affaire> affaires, string user);
        void UpdateFlagAttenteCourtier(IEnumerable<(string ipb, int alx, bool? expecting)> flags, string userId);
        CotisationOffre GetCotisationOffre(string code, int version);
    }
}
