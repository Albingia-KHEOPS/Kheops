using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using Mapster;
using OP.WSAS400.DTO.Ecran.ConditionRisqueGarantie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using Albingia.Kheops.OP.Application.Contracts;

namespace Albingia.Kheops.OP.Application.Infrastructure.Services {
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RisqueService : IRisquePort {
        readonly IRisqueRepository risqueRepository;
        private readonly ISessionContext context;

        public RisqueService(IRisqueRepository risqueRepository, ISessionContext context) {
            this.risqueRepository = risqueRepository;
            this.context = context;
        }

        public IEnumerable<RisqueDto> GetRisques(AffaireId affaire) {
            return risqueRepository.GetRisquesByAffaire(affaire).Adapt<IEnumerable<RisqueDto>>();
        }

        /// <summary>
        /// Retrieves all risques-objets from the beginning AFFNOUV till now
        /// </summary>
        /// <param name="codeOffre">IPB code</param>
        /// <param name="version">ALX code</param>
        /// <returns></returns>
        public IEnumerable<RisqueDto> GetAllRisquesByAffaire(string codeOffre, int version) {
            var risques = this.risqueRepository.GetAllRisquesByAffaire(codeOffre, version);
            return risques.Select(x => x.Adapt<RisqueDto>()).ToList();
        }

        public bool IsAvnDisabled(AffaireId affaireId, int numeroRisque) {
            if (affaireId.IsHisto || affaireId.TypeAffaire == AffaireType.Offre || numeroRisque == 0) {
                return false;
            }

            var risque = this.risqueRepository.GetRisquesByAffaire(affaireId).FirstOrDefault(x => x.Numero == numeroRisque);
            return risque?.NumeroAvenantModification != affaireId.NumeroAvenant;
        }

        public void SaveConditions(AffaireId affaireId, RisqueDto risqueDto, ConditionRisqueGarantieGetResultDto conditions) {
            var risque = this.risqueRepository.GetRisquesByAffaire(affaireId).First(x => x.Numero == risqueDto.Numero);
            var lci = decimal.TryParse(conditions.LCIRisque, out var d) ? d : default;
            var franchise = decimal.TryParse(conditions.FranchiseRisque, out d) ? d : default;
            risque.ChangeTarifsConditions(
                new Domain.TarifGeneral {
                    Base = new Domain.Referentiel.BaseDeCalcul { Code = conditions.TypeLCIRisque },
                    IdExpCpx = long.TryParse(conditions.LienComplexeLCIRisque, out long l) && l > 0 ? l : default,
                    Unite = new Domain.Referentiel.Unite { Code = conditions.UniteLCIRisque },
                    ValeurActualisee = lci,
                    ValeurOrigine = lci
                },
                new Domain.TarifGeneral {
                    Base = new Domain.Referentiel.BaseDeCalcul { Code = conditions.TypeFranchiseRisque },
                    IdExpCpx = long.TryParse(conditions.LienComplexeFranchiseRisque, out l) && l > 0 ? l : default,
                    Unite = new Domain.Referentiel.Unite { Code = conditions.UniteFranchiseRisque },
                    ValeurActualisee = franchise,
                    ValeurOrigine = franchise
                });
            this.risqueRepository.SaveConditions(risque, this.context.UserId);
        }

        public void ToggleCanatFlag(AffaireId affaireId, int numeroRisque, bool allowCanat) {
            this.risqueRepository.ToggleCanatFlag(new Domain.Risque.Risque { AffaireId = affaireId, Numero = numeroRisque, AllowCANAT = allowCanat });
        }
    }
}
