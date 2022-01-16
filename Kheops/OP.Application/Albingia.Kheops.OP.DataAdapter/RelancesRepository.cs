using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter {
    public class RelancesRepository : IRelancesRepository {
        private readonly YpoBaseRepository ypoBaseRepository;
        public RelancesRepository(YpoBaseRepository ypoBaseRepository) {
            this.ypoBaseRepository = ypoBaseRepository;
        }
        public void UpdateSituationsRelances(IEnumerable<(AffaireId id, string motif, string code)> valeurs, string user) {
            var poList = this.ypoBaseRepository.SelectMany(valeurs.Select(x => (x.id.CodeAffaire, x.id.NumeroAliment)));
            var date = DateTime.Today;
            poList.ToList().ForEach(ypo => {
                var v = valeurs.First(x => x.id.CodeAffaire == ypo.Pbipb && x.id.NumeroAliment == ypo.Pbalx && ypo.Pbtyp == AffaireType.Offre.AsCode());
                ypo.Pbsit = v.code;
                ypo.Pbmju = user;
                ypo.Pbmja = date.Year;
                ypo.Pbmjm = date.Month;
                ypo.Pbmjj = date.Day;
                ypo.Pbstf = v.motif;
                this.ypoBaseRepository.Update(ypo);
            });
        }
    }
}
