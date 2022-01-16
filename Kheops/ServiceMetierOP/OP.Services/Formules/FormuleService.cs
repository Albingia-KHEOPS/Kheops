using Albingia.Kheops.Common;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe;
using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Kheops.OP.Domain.Transverse;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using Mapster;
using OP.DataAccess;
using OP.WSAS400.DTO.Condition;
using OP.WSAS400.DTO.Ecran.ConditionRisqueGarantie;
using OP.WSAS400.DTO.Offres;
using OPServiceContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Activation;
using Domain = Albingia.Kheops.OP.Domain;

namespace OP.Services.BLServices {
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FormuleService : IFormule {
        private readonly IDbConnection connection;
        private readonly FormuleRepository repository;
        private readonly ProgramAS400Repository programAS400Repository;
        private readonly IAffairePort affaireService;
        private readonly IFormulePort formuleService;
        private readonly IGarantiePort garantieService;
        private readonly IEtapePort etapeService;
        private readonly IRisquePort risqueService;

        public FormuleService(
            IDbConnection connection, FormuleRepository formuleRepository, ProgramAS400Repository programAS400Repository, IAffairePort affaireService, IFormulePort formuleService, IGarantiePort garantieService, IEtapePort etapeService, IRisquePort risqueService) {
            this.connection = connection;
            this.repository = formuleRepository;
            this.formuleService = formuleService;
            this.garantieService = garantieService;
            this.etapeService = etapeService;
            this.affaireService = affaireService;
            this.risqueService = risqueService;
            this.programAS400Repository = programAS400Repository;
        }

        public bool IsSortie(string codeAffaire, int numRisque, int numFormule, DateTime dateDebutAvn) {
            var dateRisque = this.repository.GetDateFinRisque(codeAffaire, numRisque, numFormule);
            return dateRisque.HasValue && dateRisque < dateDebutAvn;
        }

        public IEnumerable<ConditionGarantieDto> GetConditionsGaranties(AffaireId affaireId, int option, int formule) {
            return this.formuleService.GetConditionsGaranties(affaireId, option, formule);
        }

        public void ValiderConditions(AffaireId affaireId, int option, int formule, ConditionRisqueGarantieGetResultDto conditions) {
            var contextEtape = new ContextEtapeDto(WSAS400.DTO.ContextStepName.ConditionsGarantie) { AffaireId = affaireId };
            if (conditions.LstEnsembleLigne.Any()) {
                bool hasGareat = conditions.LstEnsembleLigne.Any(x => x.IsAttentatGareat);
                var updatedCodes = this.formuleService.SaveConditionsGaranties(affaireId, hasGareat, conditions.LstEnsembleLigne.Select(x => {
                    var condition = x.LstLigneGarantie.First();
                    return BuildConditionGarantie(condition, int.TryParse(x.Id, out int i) && i > 0 ? i : default, x.Code);
                }));
                if (updatedCodes.Any()) {
                    contextEtape.NotifyUpdate(updatedCodes.Any(c => c == Garantie.CodeIncendie) ? ContextEtapeData.Gareat : ContextEtapeData.Garantie);
                }
            }

            this.affaireService.SaveConditions(affaireId, conditions);
            var gareats = ComputeEachGareat(affaireId, true);
            if (gareats.Any()) {
                var tauxMax = gareats.Values.Max(x => x.TauxRetenu);
                var trancheMax = gareats.Values.First(x => x.TauxRetenu == tauxMax).CodeTranche;
                this.affaireService.SetGareat(affaireId, trancheMax);
            }
            else {
                this.affaireService.SetGareat(affaireId, null);
            }
            var applicationsRisques = this.formuleService.GetApplicationsFormule(affaireId, option, formule);
            this.risqueService.SaveConditions(affaireId, applicationsRisques.First(), conditions);
            this.etapeService.UpdateEtapes(contextEtape);
        }

        public bool HasGareat(AffaireId affaireId, int risque) {
            return this.formuleService.HasGareat(affaireId, risque);
        }

        public GareatStateDto ComputeGareat(AffaireId affaireId, GareatStateDto gareatStateDto) {
            decimal? tauxCommissionBase = null;
            if (affaireId.TypeAffaire == AffaireType.Offre) {
                var result = this.programAS400Repository.LoadCommissions(new PGMFolder(affaireId.Adapt<Folder>()) {
                    User = WCFHelper.GetFromHeader("UserAS400"),
                    ActeGestion = affaireId.TypeAffaire.ToString()
                });
                tauxCommissionBase = Convert.ToDecimal(result.TauxStandardHCAT) / 100M;
            }
            return this.garantieService.ComputeGareat(affaireId, gareatStateDto, tauxCommissionBase);
        }

        public IDictionary<long, GareatStateDto> ComputeEachGareat(AffaireId affaireId, bool computeOnly = false) {
            var tarifsGareat = this.affaireService.GetGarantiesGareat(affaireId);
            if (!tarifsGareat.Any()) {
                return new Dictionary<long, GareatStateDto>();
            }
            var affaire = this.affaireService.GetAffaire(affaireId);
            var risques = this.risqueService.GetRisques(affaireId);
            var newTarifs = new Dictionary<long, GareatStateDto>();
            foreach (var group in tarifsGareat.GroupBy(t => t.IdOption)) {
                var tarifGareat = group.FirstOrDefault(g => g.CodeGarantie == Garantie.CodeGareatAttent);
                if (tarifGareat is null) {
                    continue;
                }
                var risque = risques.First(x => x.Numero == group.First().NumeroRisque);
                var gareatStateDto = new GareatStateDto {
                    TauxCommissions = affaire.TauxCommission / 100M,
                    LCIGenerale = affaire.TarifAffaireLCI?.ValeurActualisee > decimal.Zero && affaire.TarifAffaireLCI.Unite == "D"
                        ? affaire.TarifAffaireLCI.ValeurActualisee
                        : default(decimal?),
                    PrimeGaranties = group.Where(g => g.CodeGarantie != Garantie.CodeGareatAttent).Sum(c => {
                        if ((c.TarifsGarantie?.PrimeValeur?.ValeurActualise ?? default) <= decimal.Zero) {
                            return null;
                        }
                        var primeValeur = c.TarifsGarantie.PrimeValeur;
                        bool isTauxPcent = primeValeur.Unite?.Code == Albingia.Kheops.OP.Domain.UniteBase.PourCent.AsCode();
                        bool isTauxPml = primeValeur.Unite?.Code == Albingia.Kheops.OP.Domain.UniteBase.PourMille.AsCode();
                        decimal? primeMin = c.TarifsGarantie.PrimeProvisionnelle;
                        decimal? prime = !isTauxPcent && !isTauxPml ? primeValeur.ValeurActualise : default(decimal?);
                        decimal? taux = isTauxPcent || isTauxPml
                            ? (primeValeur.ValeurActualise / (isTauxPcent ? 100 : 1000))
                            : default(decimal?);
                        decimal? assiette = c.AssietteGarantie.Unite?.Code != "D" ? default(decimal?) : c.AssietteGarantie.ValeurActualise;

                        if (prime.HasValue) {
                            return prime > primeMin.GetValueOrDefault() ? prime : primeMin;
                        }
                        if (assiette.GetValueOrDefault() == default || taux.GetValueOrDefault() == default) {
                            return null;
                        }
                        prime = assiette * taux;
                        return prime > primeMin.GetValueOrDefault() ? prime : primeMin;
                    }),
                    CodeRegimeTaxe = risque.RegimeTaxe.Code
                };
                gareatStateDto = this.garantieService.ComputeGareat(affaireId, gareatStateDto);
                newTarifs.Add(tarifGareat.TarifsGarantie.Id, gareatStateDto);
            }
            if (!computeOnly) {
                this.affaireService.SaveGareat(affaireId, newTarifs.Select(kv => (kv.Key, kv.Value.Prime)));
            }
            return newTarifs;
        }

        private static ConditionGarantieDto BuildConditionGarantie(LigneGarantieDto condition, int idGarantie, string codeGarantie) {
            var assiette = decimal.TryParse(condition.AssietteValeur, out var d) ? d : default;
            var franchise = decimal.TryParse(condition.FranchiseValeur, out d) ? d : default;
            var lci = decimal.TryParse(condition.LCIValeur, out d) ? d : default;
            var prime = decimal.TryParse(condition.TauxForfaitHTValeur, out d) ? d : default;
            var primeOrigine = prime;
            if (condition.PrimeValeur != null) {
                // means Gareat
                if (condition.PrimeValeur.ValeurOrigine == 0) {
                    prime = primeOrigine = decimal.Zero;
                }
                else {
                    prime = condition.PrimeValeur.ValeurActualise != condition.PrimeValeur.ValeurOrigine ? condition.PrimeValeur.ValeurActualise : condition.PrimeValeur.ValeurOrigine;
                    primeOrigine = condition.PrimeValeur.ValeurOrigine;
                }
            }

            var primePro = decimal.TryParse(condition.TauxForfaitHTMinimum, out d) ? d : default;
            long idTarif = long.TryParse(condition.Code.Split('_').First(), out var sh) && sh > 0 ? sh : default;
            return new ConditionGarantieDto {
                AssietteGarantie = new ValeursOptionsTarif {
                    ValeurActualise = assiette,
                    ValeurOrigine = assiette,
                    Unite = new Unite { Code = condition.AssietteUnite },
                    Base = new BaseDeCalcul { Code = condition.AssietteType },
                    IsModifiable = !condition.AssietteLectureSeule.AsBoolean(),
                    IsObligatoire = condition.AssietteObligatoire.AsBoolean()
                },
                CodeGarantie = codeGarantie,
                IdGarantie = idGarantie,
                TarifsGarantie = new TarifGarantieDto {
                    Id = idTarif,
                    Franchise = new ValeursOptionsTarif {
                        Base = new BaseDeCalcul { Code = condition.FranchiseType },
                        ExpressionComplexe = long.TryParse(condition.LienFRHComplexe.Split('¤')[0], out long l) && l > 0
                            ? new ExpressionComplexeBase { Id = l } : null,
                        IsModifiable = !condition.FranchiseLectureSeule.AsBoolean(),
                        IsObligatoire = condition.FranchiseObligatoire.AsBoolean(),
                        Unite = new Unite { Code = condition.FranchiseUnite },
                        ValeurActualise = franchise,
                        ValeurOrigine = franchise,
                        ValeurTravail = franchise
                    },
                    LCI = new ValeursOptionsTarif {
                        Base = new BaseDeCalcul { Code = condition.LCIType },
                        ExpressionComplexe = long.TryParse(condition.LienLCIComplexe.Split('¤')[0], out l) && l > 0
                            ? new ExpressionComplexeBase { Id = l } : null,
                        IsModifiable = !condition.LCILectureSeule.AsBoolean(),
                        IsObligatoire = condition.LCIObligatoire.AsBoolean(),
                        Unite = new Unite { Code = condition.LCIUnite },
                        ValeurActualise = lci,
                        ValeurOrigine = lci,
                        ValeurTravail = lci
                    },
                    PrimeValeur = new ValeursOptionsTarif {
                        Base = new BaseDeCalcul { Code = string.Empty },
                        ExpressionComplexe = null,
                        IsModifiable = !condition.TauxForfaitHTLectureSeule.AsBoolean(),
                        IsObligatoire = condition.TauxForfaitHTObligatoire.AsBoolean(),
                        Unite = new Unite { Code = condition.TauxForfaitHTUnite },
                        ValeurActualise = prime,
                        ValeurOrigine = primeOrigine,
                        ValeurTravail = 0
                    },
                    PrimeProvisionnelle = primePro
                }
            };
        }

        public void SaveInfosComplementairesRisque(OffreDto offreDto, ValeursUniteDto lci, ValeursUniteDto franchise, bool isModifHorsAvn = false, IEnumerable<string> ignoredCheckingFields = null)
        {
            var ignoredFields = ignoredCheckingFields?.ToList() ?? new List<string>();
            if (isModifHorsAvn)
            {
                ignoredFields.AddRange(Enum.GetNames(typeof(ModifHorsAvn.InfosRisqueIgnorees)));
            }
            CheckInfosComplementairesRisque(offreDto, lci, franchise, ignoredFields);
            var updatedRisque = offreDto.Risques.First();
            var affaireId = new AffaireId
            {
                CodeAffaire = offreDto.CodeOffre,
                NumeroAliment = offreDto.Version.GetValueOrDefault(),
                TypeAffaire = offreDto.Type.ParseCode<AffaireType>()
            };
            var listRisque = this.risqueService.GetRisques(affaireId);
            PoliceRepository.UpdateDetailsRisque_YPRTRSQ(offreDto, true);
            ConditionRepository.InfoSpecRisqueLCIFranchiseSet(
                offreDto.CodeOffre, offreDto.Version.ToString(), offreDto.Type,
                updatedRisque.Code.ToString(),
                (lci?.ValeurActualise ?? default(decimal?)).ToString(), lci?.CodeUnite ?? string.Empty, lci?.CodeBase ?? string.Empty,
                (lci?.IdCPX ?? default).ToString(),
                (franchise?.ValeurActualise ?? default(decimal?)).ToString(), franchise?.CodeUnite ?? string.Empty, franchise?.CodeBase ?? string.Empty,
                (franchise?.IdCPX ?? default).ToString());

            if (HasGareat(affaireId, updatedRisque.Code))
            {
                var currentRisque = listRisque.First(x => x.Numero == updatedRisque.Code);
                bool changeToMonaco = updatedRisque.RegimeTaxe.IsIn(RegimeTaxe.Monaco, RegimeTaxe.MonacoProfessionLiberaleHabitation);
                bool isMonaco = currentRisque.RegimeTaxe.Code.IsIn(RegimeTaxe.Monaco, RegimeTaxe.MonacoProfessionLiberaleHabitation);
                if (changeToMonaco ^ isMonaco)
                {
                    if (changeToMonaco)
                    {
                        // maz GAREAT
                        this.affaireService.ResetGareat(affaireId, currentRisque.Numero);
                    }
                    else if (isMonaco)
                    {
                        // cancel RGTX Monaco
                    }

                    // delete steps Formule and further
                    var contextEtape = new ContextEtapeDto(WSAS400.DTO.ContextStepName.InformationsComplementairesRisque)
                    {
                        AffaireId = affaireId,
                        NumRisque = updatedRisque.Code
                    };
                    contextEtape.NotifyUpdate(ContextEtapeData.Garantie);
                    this.etapeService.UpdateEtapes(contextEtape);
                }
            }
        }

        public RisqueDto GetRisque(Domain.Affaire.AffaireId affaireId, int numero)
        {
            var risques = this.risqueService.GetRisques(affaireId);
            return risques?.FirstOrDefault(x => x.Numero == numero);
        }




        private void CheckInfosComplementairesRisque(OffreDto offreDto, ValeursUniteDto lci, ValeursUniteDto franchise, IEnumerable<string> ignoredFields = null)
        {
            var errors = new List<ValidationError>();
            var rsq = offreDto.Risques.First();
            if (ignoredFields is null) { ignoredFields = new string[0]; }
            if (rsq.RegimeTaxe.IsEmptyOrNull())
            {
                errors.Add(new ValidationError(string.Empty, rsq.Code.ToString(), string.Empty, nameof(rsq.RegimeTaxe), string.Empty));
            }
            if (rsq.CATNAT)
            {
                if (!PoliceRepository.ValiderCatnat(offreDto, offreDto.NumAvenant.ToString(), ModeConsultation.Standard))
                {
                    errors.Add(new ValidationError(string.Empty, string.Empty, string.Empty, nameof(rsq.CATNAT), $"{nameof(rsq.CATNAT)} non soumise sur ce risque"));
                }
                else if (Domain.Referentiel.RegimeTaxe.ListeNonCATNAT.Contains(rsq.RegimeTaxe))
                {
                    errors.Add(new ValidationError(
                        string.Empty, rsq.Code.ToString(), string.Empty,
                        $"{nameof(rsq.CATNAT)},{nameof(rsq.RegimeTaxe)}",
                        $"{nameof(rsq.CATNAT)} non autorisé pour le régime de taxe sélectionné"));
                }
            }
            if ((rsq.IsRegularisable.AsBoolean() ?? false) && rsq.TypeRegularisation.IsEmptyOrNull())
            {
                errors.Add(new ValidationError(string.Empty, rsq.Code.ToString(), string.Empty, nameof(rsq.TypeRegularisation), string.Empty));
            }
            if (rsq.Ristourne == 0 && rsq.PartBenef == "O")
            {
                errors.Add(new ValidationError(string.Empty, rsq.Code.ToString(), string.Empty, nameof(rsq.Ristourne), string.Empty));
            }
            if (rsq.CotisRetenue == 0 && rsq.PartBenef == "O")
            {
                errors.Add(new ValidationError(string.Empty, rsq.Code.ToString(), string.Empty, nameof(rsq.CotisRetenue), string.Empty));
            }
            if (rsq.PartBenef == "U")
            {
                if (rsq.TauxMaxi == 0F)
                {
                    errors.Add(new ValidationError(string.Empty, rsq.Code.ToString(), string.Empty, nameof(rsq.TauxMaxi), string.Empty));
                }
                if (rsq.PrimeMaxi == 0D)
                {
                    errors.Add(new ValidationError(string.Empty, rsq.Code.ToString(), string.Empty, nameof(rsq.PrimeMaxi), string.Empty));
                }
            }
            if (lci != null)
            {
                var tarif = lci.Adapt<Domain.Formule.ValeursOptionsTarif>();
                if (!tarif.IsEmpty && !tarif.IsValid)
                {
                    errors.Add(new ValidationError(string.Empty, rsq.Code.ToString(), string.Empty, "LCI", string.Empty));
                }
            }
            if (franchise != null)
            {
                var tarif = franchise.Adapt<Domain.Formule.ValeursOptionsTarif>();
                if (!tarif.IsEmpty && !tarif.IsValid)
                {
                    errors.Add(new ValidationError(string.Empty, rsq.Code.ToString(), string.Empty, "Franchise", string.Empty));
                }
            }

            bool test(string s1, string s2)
            {
                const char sp = ',';
                if (s1.Contains(sp) && s2.Contains(sp))
                {
                    var a = s1.Split(sp);
                    return a.Intersect(s2.Split(sp)).Count() == a.Length;
                }
                return s1 == s2;
            }
            if (ignoredFields.Any())
            {
                errors.RemoveAll(e => ignoredFields.Any(f => test(e.FieldName, f)));
            }
            if (errors.Any())
            {
                if (errors.Any(e => e.FieldName.ContainsChars() && e.Error.IsEmptyOrNull()))
                {
                    var error = errors.First(e => e.FieldName.ContainsChars() && e.Error.IsEmptyOrNull());
                    errors[errors.IndexOf(error)] = error.Clone("Saisie(s) invalide(s)");
                }
                throw new BusinessValidationException(errors);
            }
        }

        
    }
}
