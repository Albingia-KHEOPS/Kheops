using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Constants;
using System.Linq;

namespace Albingia.Kheops.OP.DataAdapter {
    public class OppositionRepository : IOppositionRepository {
        private readonly KpOppRepository kpOppRepository;
        private readonly HpoppRepository hpoppRepository;
        private readonly KpOppApRepository kpOppApRepository;
        private readonly HpoppapRepository hpoppapRepository;

        public OppositionRepository(
            KpOppRepository kpOppRepository,
            HpoppRepository hpoppRepository,
            KpOppApRepository kpOppApRepository,
            HpoppapRepository hpoppapRepository) {
            this.kpOppRepository = kpOppRepository;
            this.hpoppRepository = hpoppRepository;
            this.kpOppApRepository = kpOppApRepository;
            this.hpoppapRepository = hpoppapRepository;
        }

        public void Reprise(AffaireId id) {
            var oppList = this.kpOppRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            oppList.ForEach(x => this.kpOppRepository.Delete(x));
            var hoppList = this.kpOppRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            hoppList.ForEach(x => this.kpOppRepository.Delete(x));
            hoppList.ForEach(x => this.kpOppRepository.Insert(x));
            var oppapList = this.kpOppApRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            oppapList.ForEach(x => this.kpOppApRepository.Delete(x));
            var hoppapList = this.hpoppapRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            oppapList.ForEach(x => this.hpoppapRepository.Delete(x));
            oppapList.ForEach(x => this.kpOppApRepository.Insert(x));
        }
    }
}
