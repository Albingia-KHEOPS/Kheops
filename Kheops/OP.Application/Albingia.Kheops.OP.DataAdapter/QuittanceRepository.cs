using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Constants;
using System.Linq;

namespace Albingia.Kheops.OP.DataAdapter {
    public partial class QuittanceRepository : IQuittanceRepository {
        private readonly KpCotisRepository kpCotisRepository;
        private readonly HpcotisRepository hpcotisRepository;
        private readonly KpCotgaRepository kpCotgaRepository;
        private readonly HpcotgaRepository hpcotgaRepository;

        public QuittanceRepository(
            KpCotisRepository kpCotisRepository,
            HpcotisRepository hpcotisRepository,
            KpCotgaRepository kpCotgaRepository,
            HpcotgaRepository hpcotgaRepository) {
            this.kpCotisRepository = kpCotisRepository;
            this.hpcotisRepository = hpcotisRepository;
            this.kpCotgaRepository = kpCotgaRepository;
            this.hpcotgaRepository = hpcotgaRepository;
        }

        public void Reprise(AffaireId id) {
            var listCotis = this.kpCotisRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            listCotis.ForEach(x => this.kpCotisRepository.Delete(x));
            var histoCotis = this.hpcotisRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            histoCotis.ForEach(x => this.hpcotisRepository.Delete(x));
            histoCotis.ForEach(x => this.kpCotisRepository.Insert(x));
            var listCotga = this.kpCotgaRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            listCotga.ForEach(x => this.kpCotgaRepository.Delete(x));
            var histoCotga = this.hpcotgaRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            histoCotga.ForEach(x => this.hpcotgaRepository.Delete(x));
            histoCotga.ForEach(x => this.kpCotgaRepository.Insert(x));
        }
    }
}

