using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Transverse;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using Mapster;
using OP.DataAccess;
using OP.WSAS400.DTO.Offres;
using OPServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using AppDto = Albingia.Kheops.DTO;
using Domain = Albingia.Kheops.OP.Domain;

namespace OP.Services.ClausesRisquesGaranties {
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RisqueService : IRisque {
        private readonly IAffairePort serviceAffaire;
        private readonly IFormulePort serviceFormule;
        private readonly IRisquePort serviceRisque;
        private readonly IEtapePort serviceEtape;
        public RisqueService(IRisquePort serviceRisque, IFormulePort serviceFormule, IAffairePort serviceAffaire, IEtapePort serviceEtape) {
            this.serviceRisque = serviceRisque;
            this.serviceFormule = serviceFormule;
            this.serviceAffaire = serviceAffaire;
            this.serviceEtape = serviceEtape;
        }

        public void SaveInfosComplementairesRisque(OffreDto offreDto, AppDto.ValeursUniteDto lci, AppDto.ValeursUniteDto franchise, bool isModifHorsAvn = false, IEnumerable<string> ignoredCheckingFields = null) {
            var ignoredFields = ignoredCheckingFields?.ToList() ?? new List<string>();
            if (isModifHorsAvn) {
                ignoredFields.AddRange(Enum.GetNames(typeof(ModifHorsAvn.InfosRisqueIgnorees)));
            }
            CheckInfosComplementairesRisque(offreDto, lci, franchise, ignoredFields);
            var updatedRisque = offreDto.Risques.First();
            var affaireId = new Domain.Affaire.AffaireId {
                CodeAffaire = offreDto.CodeOffre,
                NumeroAliment = offreDto.Version.GetValueOrDefault(),
                TypeAffaire = offreDto.Type.ParseCode<Domain.Affaire.AffaireType>()
            };
            var listRisque = this.serviceRisque.GetRisques(affaireId);
            PoliceRepository.UpdateDetailsRisque_YPRTRSQ(offreDto, true);
            ConditionRepository.InfoSpecRisqueLCIFranchiseSet(
                offreDto.CodeOffre, offreDto.Version.ToString(), offreDto.Type,
                updatedRisque.Code.ToString(),
                (lci?.ValeurActualise ?? default(decimal?)).ToString(), lci?.CodeUnite ?? string.Empty, lci?.CodeBase ?? string.Empty,
                (lci?.IdCPX ?? default).ToString(),
                (franchise?.ValeurActualise ?? default(decimal?)).ToString(), franchise?.CodeUnite ?? string.Empty, franchise?.CodeBase ?? string.Empty,
                (franchise?.IdCPX ?? default).ToString());

            if (this.serviceFormule.HasGareat(affaireId, updatedRisque.Code)) {
                var currentRisque = listRisque.First(x => x.Numero == updatedRisque.Code);
                bool changeToMonaco = updatedRisque.RegimeTaxe.IsIn(Domain.Referentiel.RegimeTaxe.Monaco, Domain.Referentiel.RegimeTaxe.MonacoProfessionLiberaleHabitation);
                bool isMonaco = currentRisque.RegimeTaxe.Code.IsIn(Domain.Referentiel.RegimeTaxe.Monaco, Domain.Referentiel.RegimeTaxe.MonacoProfessionLiberaleHabitation);
                if (changeToMonaco ^ isMonaco) {
                    if (changeToMonaco) {
                        // maz GAREAT
                        this.serviceAffaire.ResetGareat(affaireId, currentRisque.Numero);
                    }
                    else if (isMonaco) {
                        // cancel RGTX Monaco
                    }

                    // delete steps Formule and further
                    var contextEtape = new AppDto.ContextEtapeDto(WSAS400.DTO.ContextStepName.InformationsComplementairesRisque) {
                        AffaireId = affaireId,
                        NumRisque = updatedRisque.Code
                    };
                    contextEtape.NotifyUpdate(AppDto.ContextEtapeData.Garantie);
                    this.serviceEtape.UpdateEtapes(contextEtape);
                }
            }
        }

        public AppDto.RisqueDto GetRisque(Domain.Affaire.AffaireId affaireId, int numero) {
            var risques = this.serviceRisque.GetRisques(affaireId);
            return risques?.FirstOrDefault(x => x.Numero == numero);
        }

        private void CheckInfosComplementairesRisque(OffreDto offreDto, AppDto.ValeursUniteDto lci, AppDto.ValeursUniteDto franchise, IEnumerable<string> ignoredFields = null) {
            var errors = new List<ValidationError>();
            var rsq = offreDto.Risques.First();
            if (ignoredFields is null) { ignoredFields = new string[0]; }
            if (rsq.RegimeTaxe.IsEmptyOrNull()) {
                errors.Add(new ValidationError(string.Empty, rsq.Code.ToString(), string.Empty, nameof(rsq.RegimeTaxe), string.Empty));
            }
            if (rsq.CATNAT) {
                if (!PoliceRepository.ValiderCatnat(offreDto, offreDto.NumAvenant.ToString(), ModeConsultation.Standard)) {
                    errors.Add(new ValidationError(string.Empty, string.Empty, string.Empty, nameof(rsq.CATNAT), $"{nameof(rsq.CATNAT)} non soumise sur ce risque"));
                }
                else if (Domain.Referentiel.RegimeTaxe.ListeNonCATNAT.Contains(rsq.RegimeTaxe)) {
                    errors.Add(new ValidationError(
                        string.Empty, rsq.Code.ToString(), string.Empty,
                        $"{nameof(rsq.CATNAT)},{nameof(rsq.RegimeTaxe)}",
                        $"{nameof(rsq.CATNAT)} non autorisé pour le régime de taxe sélectionné"));
                }
            }
            if ((rsq.IsRegularisable.AsBoolean() ?? false) && rsq.TypeRegularisation.IsEmptyOrNull()) {
                errors.Add(new ValidationError(string.Empty, rsq.Code.ToString(), string.Empty, nameof(rsq.TypeRegularisation), string.Empty));
            }
            if (rsq.Ristourne == 0 && rsq.PartBenef == "O") {
                errors.Add(new ValidationError(string.Empty, rsq.Code.ToString(), string.Empty, nameof(rsq.Ristourne), string.Empty));
            }
            if (rsq.CotisRetenue == 0 && rsq.PartBenef == "O") {
                errors.Add(new ValidationError(string.Empty, rsq.Code.ToString(), string.Empty, nameof(rsq.CotisRetenue), string.Empty));
            }
            if (rsq.PartBenef == "U") {
                if (rsq.TauxMaxi == 0F) {
                    errors.Add(new ValidationError(string.Empty, rsq.Code.ToString(), string.Empty, nameof(rsq.TauxMaxi), string.Empty));
                }
                if (rsq.PrimeMaxi == 0D) {
                    errors.Add(new ValidationError(string.Empty, rsq.Code.ToString(), string.Empty, nameof(rsq.PrimeMaxi), string.Empty));
                }
            }
            if (lci != null) {
                var tarif = lci.Adapt<Domain.Formule.ValeursOptionsTarif>();
                if (!tarif.IsEmpty && !tarif.IsValid) {
                    errors.Add(new ValidationError(string.Empty, rsq.Code.ToString(), string.Empty, "LCI", string.Empty));
                }
            }
            if (franchise != null) {
                var tarif = franchise.Adapt<Domain.Formule.ValeursOptionsTarif>();
                if (!tarif.IsEmpty && !tarif.IsValid) {
                    errors.Add(new ValidationError(string.Empty, rsq.Code.ToString(), string.Empty, "Franchise", string.Empty));
                }
            }
            
            bool test(string s1, string s2) {
                const char sp = ',';
                if (s1.Contains(sp) && s2.Contains(sp)) {
                    var a = s1.Split(sp);
                    return a.Intersect(s2.Split(sp)).Count() == a.Length;
                }
                return s1 == s2;
            }
            if (ignoredFields.Any()) {
                errors.RemoveAll(e => ignoredFields.Any(f => test(e.FieldName, f)));
            }
            if (errors.Any()) {
                throw new BusinessValidationException(errors);
            }
        }
    }
}
