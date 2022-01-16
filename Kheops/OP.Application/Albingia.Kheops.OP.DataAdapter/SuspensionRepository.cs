using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter {
    public class SuspensionRepository : ISuspensionRepository {
        private readonly KpSuspRepository kpSuspRepository;
        public SuspensionRepository(KpSuspRepository kpSuspRepository) {
            this.kpSuspRepository = kpSuspRepository;
        }

        public void Reprise(AffaireId id) {
            var currentSup = this.kpSuspRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment, AlbConstantesMetiers.TYPE_CONTRAT).ToList();
            var latest = currentSup.OrderByDescending(x => x.Kicid).FirstOrDefault();
            if (latest is null) {
                return;
            }
            this.kpSuspRepository.Delete(latest);
            currentSup.Remove(latest);
            var newLatest = currentSup.OrderByDescending(x => x.Kicid).FirstOrDefault();
            if (newLatest is null) {
                return;
            }
            newLatest.Kicsit = "A";
            newLatest.Kicaca = "N";
            this.kpSuspRepository.Update(newLatest);
        }
    }
}
