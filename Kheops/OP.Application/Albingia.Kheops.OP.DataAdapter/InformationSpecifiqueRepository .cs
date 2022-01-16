using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Constants;
using System.Linq;

namespace Albingia.Kheops.OP.DataAdapter {
    public partial class InformationSpecifiqueRepository : IInformationSpecifiqueRepository {
        private readonly KpIoptRepository kpIOptRepository;
        private readonly HpioptRepository hpioptRepository;

        public InformationSpecifiqueRepository(
            KpIoptRepository kpIOptRepository,
            HpioptRepository hpioptRepository) {
            this.kpIOptRepository = kpIOptRepository;
            this.hpioptRepository = hpioptRepository;
        }

        public void Reprise(AffaireId id) {
            var listIopt = this.kpIOptRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            listIopt.ForEach(x => this.kpIOptRepository.Delete(x));
            var histoIopt = this.hpioptRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            histoIopt.ForEach(x => this.hpioptRepository.Delete(x));
            histoIopt.ForEach(x => this.kpIOptRepository.Insert(x));
        }
    }
}

