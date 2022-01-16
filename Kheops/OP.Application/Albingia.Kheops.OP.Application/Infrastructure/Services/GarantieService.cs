using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Garantie;
using Albingia.Kheops.OP.Domain.Referentiel;
using Mapster;

namespace Albingia.Kheops.OP.Application.Infrastructure.Services {
    public class GarantieService : IGarantiePort {
        private readonly IAffaireRepository affaireRepository;
        private readonly IGarantieRepository garantieRepository;
        private readonly IReferentialRepository refRep;
        public GarantieService(IAffaireRepository affaireRepository, IGarantieRepository garantieRepository, IReferentialRepository referentialRepository) {
            this.garantieRepository = garantieRepository;
            this.affaireRepository = affaireRepository;
            this.refRep = referentialRepository;
        }
        public string GetLibelleGarantie(string code) {
            return this.garantieRepository.GetRefLibelle(code) ?? string.Empty;
        }
        public GarantieDto GetGarantieLight(long id, int? numeroAvenant = null) {
            Garantie g = null;
            if (numeroAvenant.HasValue && numeroAvenant >= 0) {
                g = this.garantieRepository.GetHistoById(id, numeroAvenant.Value);
            }
            else {
                g = this.garantieRepository.GetById(id);
            }
            if (g is null) {
                return null;
            }
            var dto = g.Adapt<GarantieDto>();
            dto.Libelle = this.garantieRepository.GetRefLibelle(g.CodeGarantie);
            return dto;
        }

        public GarantieDto GetLatestGarantieLight(long id) {
            var g = this.garantieRepository.GetById(id);
            if (g is null) {
                g = this.garantieRepository.GetLatestById(id);
            }
            if (g is null) {
                return null;
            }
            var dto = g.Adapt<GarantieDto>();
            dto.Libelle = this.garantieRepository.GetRefLibelle(g.CodeGarantie);
            return dto;
        }

        public GareatStateDto ComputeGareat(AffaireId affaireId, GareatStateDto gareatStateDto, decimal? tauxCommissionsBase = null) {
            var state = gareatStateDto.Adapt<GareatState>();
            var fraisGeneraux = this.refRep.GetValue<FraisGareat>(FraisGareat.DefaultCode);
            state.TrancheGareat.RateFraisGeneraux = fraisGeneraux.ParamNum1 / 100M;
            if (state.TrancheGareat.RateCommissions <= decimal.Zero) {
                var affaire = this.affaireRepository.GetById(affaireId);
                state.TrancheGareat.RateCommissions = affaire.TauxCommission / 100M;
            }
            if (state.TrancheGareat.RateCommissions <= decimal.Zero) {
                if (affaireId.TypeAffaire == AffaireType.Offre) {
                    state.TrancheGareat.RateCommissions = tauxCommissionsBase??0M;
                }
            }
            state.Compute();
            return state.Adapt<GareatStateDto>();
        }

     

    }
}
