using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Port.Driver;
using ALBINGIA.Framework.Common.Extensions;
using System.Linq;

namespace Albingia.Kheops.OP.Application.Infrastructure.Services {
    public class EtapeService : IEtapePort {
        private readonly IControleRepository controleRepository;
        public EtapeService(IControleRepository controleRepository) {
            this.controleRepository = controleRepository;
        }
        public void UpdateEtapes(ContextEtapeDto contextEtape) {
            if (contextEtape.MustUpdate) {
                this.controleRepository.AnnulerEtapes(
                    contextEtape.AffaireId.CodeAffaire,
                    contextEtape.AffaireId.NumeroAliment,
                    contextEtape.StepsToUpdate.Where(s => s.State == ContextEtateStateValue.Invalidate).Select(s => s.Step.AsCode()),
                    (contextEtape.NumRisque ?? 0) > 0 ? new[] { contextEtape.NumRisque.Value } : null);
            }
        }
    }
}
