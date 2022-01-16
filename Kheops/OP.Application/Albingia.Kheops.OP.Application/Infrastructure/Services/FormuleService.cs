using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Extension;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe;
using Albingia.Kheops.OP.Domain.Inventaire;
using Albingia.Kheops.OP.Domain.Model;
using Albingia.Kheops.OP.Domain.Parametrage.Formule;
using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Kheops.OP.Domain.Risque;
using Albingia.Kheops.OP.Domain.Transverse;
using ALBINGIA.Framework.Common.Extensions;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using TruthTable = System.Collections.Generic.IDictionary<(Albingia.Kheops.OP.Domain.Model.CaractereSelection caractere, Albingia.Kheops.OP.Domain.Referentiel.NatureValue nature), Albingia.Kheops.OP.Domain.Parametrage.Formule.NatureSelection>;
using Albingia.Kheops.OP.Application.Contracts;

namespace Albingia.Kheops.OP.Application.Infrastructure.Services {
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FormuleService : IFormulePort
    {
        private readonly IFormuleRepository formuleRepository;
        private readonly IGarantieRepository garantieRepository;
        private readonly IReferentialRepository referentialRepository;
        private readonly ISessionContext context;
        private readonly ILiveDataCache cache;
        private readonly ISharedDataCache sharedCache;
        private readonly IParamRepository parameterRepository;
        private readonly IAffaireRepository affaireRepository;
        private readonly IRisqueRepository risqueRepository;
        private readonly IInventaireRepository inventaireRepository;

        public FormuleService(
            IParamRepository parameterRepository,
            ISessionContext context,
            ILiveDataCache cache,
            ISharedDataCache sharedCache,
            IAffaireRepository affaireRepository,
            IFormuleRepository formuleRepository,
            IReferentialRepository referentialRepository,
            IRisqueRepository risqueRepository,
            IInventaireRepository inventaireRepository,
            IGarantieRepository garantieRepository) {
            this.formuleRepository = formuleRepository;
            this.referentialRepository = referentialRepository;
            this.context = context;
            this.cache = cache;
            this.sharedCache = sharedCache;
            this.parameterRepository = parameterRepository;
            this.affaireRepository = affaireRepository;
            this.risqueRepository = risqueRepository;
            this.inventaireRepository = inventaireRepository;
            this.garantieRepository = garantieRepository;
        }

        private static int GetAvailableNumOption(Formule formule) {
            if (formule == null) {
                return 0;
            }
            int newOption = 0;
            for (int num = 1; num <= Option.MaxNbByFormula && newOption == 0; num++) {
                if (!formule.Options.Any(o => o.OptionNumber == num)) {
                    newOption = num;
                }
            }

            return newOption;
        }

        public FormuleDto GetFormuleAffaire(AffaireId affId, int numFormule, bool isReadonly = false) {
            if (numFormule <= 0) {
                throw new ArgumentException("Numéro de formule invalide", nameof(numFormule));
            }

            var res = GetFormulesAffaire(affId, isReadonly);
            if (res.ValidationErrors.Any(e => e.TargetCode == numFormule.ToString())) {
                throw new BusinessValidationException(res.ValidationErrors);
            }
            FormuleDto formuleDto = res.Formules.FirstOrDefault(f => f.FormuleNumber == numFormule);
            return formuleDto;
        }

        public bool AffaireHasFormules(AffaireId affaireId) {
            var options = this.formuleRepository.GetOptionsSimple(affaireId);
            if (!options.Any()) {
                return false;
            }
            var risques = this.risqueRepository.GetRisquesByAffaire(affaireId);
            var numRisques = risques.Select(r => r.Numero).Distinct();
            var optNumRisques = options.SelectMany(o => o.Applications.Select(ap => ap.NumRisque)).Distinct();
            if (numRisques.Intersect(optNumRisques).Count() == numRisques.Count()) {
                var objetsApp = options.SelectMany(o => o.Applications.Select(a => (a.NumRisque, a.NumObjet))).Distinct();
                if (!objetsApp.Any(x => x.NumObjet > 0)) {
                    return true;
                }
                var objets = risques.SelectMany(r => r.Objets.Select(o => (r.Numero, o.Id.NumObjet)));
                return objetsApp.Intersect(objets).Count() == objets.Count();
            }
            return false;
        }

        public void ResetFormulesOffreSelection(AffaireId affId, string codeContrat, IDictionary<int, int[]> newAffairFilter) {
            if (affId.TypeAffaire != AffaireType.Offre) {
                throw new ArgumentException("Operation invalide pour ce type d'affaire", $"{nameof(affId)}.{nameof(affId.TypeAffaire)}");
            }
            (IEnumerable<Formule> cached, IEnumerable<Risque> risqueListCached) = GetFormulesRisques(affId, false);
            var formules = cached.ToList();
            var tempAffair = this.affaireRepository.GetTempAffairs(affId).First(x => x.CodeContrat == codeContrat);
            NouvelleAffaireFormule tempFormule;
            NouvelleAffaire nouvelleAffaire = new NouvelleAffaire {
                CodeContrat = codeContrat,
                Offre = affId,
                Formules = new List<NouvelleAffaireFormule>(),
                Risques = new List<NouvelleAffaireRisque>()
            };
            formules.ForEach(f => {
                if (!newAffairFilter.ContainsKey(f.FormuleNumber)) {
                    nouvelleAffaire.Formules.Add(new NouvelleAffaireFormule {
                        AffaireId = affId,
                        IsSelected = false,
                        Numero = f.FormuleNumber,
                        SelectedOptionNumber = null
                    });
                }
                else {
                    tempFormule = tempAffair.Formules.FirstOrDefault(x => x.Numero == f.FormuleNumber);
                    if (tempFormule?.IsSelected == true
                        && tempFormule.SelectedOptionNumber.HasValue
                        && !newAffairFilter[f.FormuleNumber].Contains(tempFormule.SelectedOptionNumber.Value)) {

                        nouvelleAffaire.Formules.Add(new NouvelleAffaireFormule {
                            AffaireId = affId,
                            IsSelected = true,
                            Numero = f.FormuleNumber,
                            SelectedOptionNumber = null
                        });
                    }
                }
            });
            this.affaireRepository.SaveSelectionFormulesNouvelleAffaire(nouvelleAffaire);
            CancelFormuleAffaireChanges(affId);
        }

        public IEnumerable<FormuleDto> GetAllFormulesOffre(AffaireId affId, IDictionary<int, int[]> newAffairFilter) {
            if (affId.TypeAffaire != AffaireType.Offre) {
                throw new ArgumentException("Operation invalide pour ce type d'affaire", $"{nameof(affId)}.{nameof(affId.TypeAffaire)}");
            }
            (IEnumerable<Formule> cached, IEnumerable<Risque> risqueListCached) = GetFormulesRisques(affId, false);
            var formules = cached.ToList();
            if (newAffairFilter?.Any() ?? false) {
                // force modifing cache to create a new affair from offer
                formules.RemoveAll(f => !newAffairFilter.ContainsKey(f.FormuleNumber));
                if (formules.Count == 0) {
                    throw new BusinessException("Aucunes formules ne correspond au filtre");
                }
                formules.ForEach(f => {
                    f.Options.RemoveAll(o => !newAffairFilter[f.FormuleNumber].Contains(o.OptionNumber));
                });
                if (formules.Any(f => !f.Options.Any())) {
                    throw new BusinessException("Certaines formules n'ont aucune option sélectionnée");
                }
                SetCache(affId, false, formules);
            }

            foreach (var item in formules) {
                item.EnsureHierarchy();
            }

            var formuleList = formules.Adapt<List<FormuleDto>>();

            for (int indexFormule = 0; indexFormule < formules.Count; indexFormule++) {
                Formule formule = formules[indexFormule];
                for (int indexOption = 0; indexOption < formule.Options.Count(); indexOption++) {
                    var option = formule.Options[indexOption];
                    formuleList[indexFormule].Options[indexOption].IsModifiedForAvenant = option.IsModifiedForAvenant(affId.NumeroAvenant ?? 0);
                }
            }

            var risqueList = risqueListCached.Adapt<IEnumerable<RisqueDto>>();
            formuleList.ForEach(f => f.Risque = risqueList.FirstOrDefault(x => f.Options.FirstOrDefault()?.Applications.Any(ap => ap.NumRisque == x.Numero) ?? false));
            var affaire = affaireRepository.GetById(affId);
            AffaireFormuleDto offre = new AffaireFormuleDto() { AffaireId = affId, DateEffetAvenant = affaire.DateEffetAvenant };
            offre.Formules.AddRange(formuleList);

            foreach (var formule in offre.Formules) {
                formule.IsSelected = true;
                if (formule.Options.Count == 1) {
                    formule.SelectedOptionNumber = formule.Options.First().OptionNumber;
                }
            }
            return offre.Formules;
        }

        public FormuleDto InitFormuleAffaire(AffaireId affId, int numRisque, IEnumerable<int> numObjets, DateTime? dateAvenant) {
            var (f, r) = InitFormule(affId, numRisque, numObjets, dateAvenant, false);
            var formule = f.Adapt<FormuleDto>();
            formule.Risque = r.Adapt<RisqueDto>();
            return formule;
        }

        public void SetFormuleJson(AffaireId affId, global::OP.WSAS400.DTO.Contrats.Formule formuleDto, int codeFor) {
            var (cached, expressions, risqueListCached) = GetFormulesExpressionsRisques(affId, false);
            var formule = cached.FirstOrDefault(f => f.FormuleNumber == codeFor);
            formule.Options.First().OptionVolets.Where(v => v.Caractere != CaractereSelection.Obligatoire).ToList()
                .ForEach(v => v.IsChecked = false);

            foreach (var volet in formule.Options.First().OptionVolets) {
                var selVolet = formuleDto.Volets.FirstOrDefault(v => v.Code == volet.ParamVolet.Code);
                var volChecked = selVolet != null;
                volet.IsChecked = volet.Caractere != CaractereSelection.Obligatoire ? volChecked : volet.IsChecked;
                foreach (var bloc in volet.Blocs) {
                    var selBloc = volChecked ? selVolet.Blocs.FirstOrDefault(b => b.Code == bloc.ParamBloc.Code) : null;
                    var blocChecked = selBloc != null;
                    bloc.IsChecked = bloc.Caractere != CaractereSelection.Obligatoire ? blocChecked : bloc.IsChecked;

                    SetGarantieJson(affId, cached, bloc.Garanties, selBloc?.Garanties ?? new List<global::OP.WSAS400.DTO.Contrats.Garanty>(), blocChecked);

                }
            }
            formule.Description = formuleDto.Libelle;
            SetCache(affId, false, cached);
        }
        public IEnumerable<GarantieDto> GetGarantiesFullHistoForRegul(string codeAffaire, int version, DateTime debut, DateTime fin)
        {
            var garanties = this.formuleRepository.GetGarantiesFullHisto(codeAffaire, version);

            return garanties.Where(x => x.NatureRetenue.IsIn(NatureValue.Accordee, NatureValue.Comprise)
                && x.ParamGarantie.Niveau == 1
                && x.ParamGarantie.ParamGarantie.IsRegularisable.GetValueOrDefault()
                && (x.DateDebut is null || x.DateDebut <= fin)
                && (x.DateSortieEffective is null || x.DateSortieEffective >= debut)).Select(x => x.Adapt<GarantieDto>());
        }

        public IEnumerable<string> SaveConditionsGaranties(AffaireId affaireId, bool hasGareat, IEnumerable<ConditionGarantieDto> conditions) {
            var errors = new List<ValidationError>();
            bool isCanevas = affaireId.CodeAffaire.Trim().StartsWith("CV");
            conditions.Where(c => c.CodeGarantie != Garantie.CodeGareatAttent || !hasGareat).ToList().ForEach(condition => {
                if (condition.AssietteGarantie.IsObligatoire == true && condition.AssietteGarantie.IsEmpty && !isCanevas
                    || !condition.AssietteGarantie.IsEmpty && !condition.AssietteGarantie.IsValid) {
                    errors.Add(new ValidationError(string.Empty, condition.CodeGarantie, condition.IdGarantie.ToString(), "ASSIETTE", string.Empty));
                }
                if (condition.TarifsGarantie.LCI.IsObligatoire == true && condition.TarifsGarantie.LCI.IsEmpty && !isCanevas
                    || !condition.TarifsGarantie.LCI.IsEmpty
                        && (condition.TarifsGarantie.LCI.IsObligatoire != true && !condition.TarifsGarantie.LCI.IsValidIgnoreValue
                            || condition.TarifsGarantie.LCI.IsObligatoire == true && !condition.TarifsGarantie.LCI.IsValid)) {
                    errors.Add(new ValidationError(string.Empty, condition.CodeGarantie, condition.IdGarantie.ToString(), "LCI", string.Empty));
                }
                if (condition.TarifsGarantie.Franchise.IsObligatoire == true && condition.TarifsGarantie.Franchise.IsEmpty && !isCanevas
                    || !condition.TarifsGarantie.Franchise.IsEmpty
                        && (condition.TarifsGarantie.Franchise.IsObligatoire != true && !condition.TarifsGarantie.Franchise.IsValidIgnoreValue
                            || condition.TarifsGarantie.Franchise.IsObligatoire == true && !condition.TarifsGarantie.Franchise.IsValid)) {
                    errors.Add(new ValidationError(string.Empty, condition.CodeGarantie, condition.IdGarantie.ToString(), "FRH", string.Empty));
                }
                if (condition.TarifsGarantie.PrimeValeur.IsObligatoire == true && condition.TarifsGarantie.PrimeValeur.IsEmpty && !isCanevas
                    || !condition.TarifsGarantie.PrimeValeur.IsEmpty && !condition.TarifsGarantie.PrimeValeur.IsValidIgnoreBase) {
                    errors.Add(new ValidationError(string.Empty, condition.CodeGarantie, condition.IdGarantie.ToString(), "PRIME", string.Empty));
                }
                if (!errors.Any()) {
                    if (condition.AssietteGarantie.Unite?.Code == ValeursUnite.CodeUniteNombre
                        && (condition.TarifsGarantie.PrimeValeur.Unite?.Code).ContainsChars()
                        && condition.TarifsGarantie.PrimeValeur.Unite?.Code != UniteBase.Devise.AsCode()) {
                        errors.Add(new ValidationError(string.Empty, condition.CodeGarantie, condition.IdGarantie.ToString(), "PRIME", "Une assiette en nombre nécessite une prime en devise"));
                    }
                }
            });

            if (errors.Any()) {
                if (errors.Any(e => e.FieldName.ContainsChars() && e.Error.IsEmptyOrNull())) {
                    var error = errors.First(e => e.FieldName.ContainsChars() && e.Error.IsEmptyOrNull());
                    errors[errors.IndexOf(error)] = error.Clone("Saisie(s) invalide(s)");
                }
                throw new BusinessValidationException(errors);
            }

            // update every Tarif
            return this.garantieRepository.UpdateConditions(affaireId, conditions.ToDictionary(c => c.IdGarantie, c => (c.AssietteGarantie, c.TarifsGarantie.Adapt<TarifGarantie>()))).ToList();
        }

        public IEnumerable<ConditionGarantieDto> GetConditionsGaranties(AffaireId affaireId, int option, int formule) {
            var list = this.garantieRepository.GetConditionsGaranties(affaireId, option, formule);
            foreach (var key in list.Keys) {
                yield return new ConditionGarantieDto {
                    AssietteGarantie = list[key].assiete,
                    CodeGarantie = key.codeGar,
                    IdGarantie = key.idGar,
                    TarifsGarantie = list[key].tarif.Adapt<TarifGarantieDto>()
                };
            }
        }

        public bool HasGareat(AffaireId affaireId, int risque) {
            var (allowed, numFormules) = AllowGareat(affaireId, risque);
            if (!allowed) {
                return false;
            }
            var garanties = this.garantieRepository.GetGaranties(affaireId).Where(g => numFormules.Contains(g.Formule));
            return garanties.Any(g => this.garantieRepository.GetParamGarantie(g.CodeGarantie)?.IsAttentatGareat ?? false);
        }

        public (bool allowed, IEnumerable<int> numFormules) AllowGareat(AffaireId affaireId, int risque) {
            var options = this.formuleRepository.GetOptionsSimple(affaireId);
            var numFormules = options.Where(o => o.Applications.Any(a => a.NumRisque == risque)).Select(o => o.Formule.FormuleNumber);
            var includedNums = new List<int>();
            foreach (var num in numFormules) {
                var blocs = this.formuleRepository.GetOptionsBlocsByFormule(affaireId, num);
                if (blocs.Any(x => x.ParamModele?.Code == Garantie.CodeGareat)) {
                    includedNums.Add(num);
                }
            }
            return (includedNums.Any(), includedNums);
        }

        private void SetGarantieJson(AffaireId affId, IEnumerable<Formule> cached, List<Garantie> garantiesParam, List<global::OP.WSAS400.DTO.Contrats.Garanty> garantiesJson, bool isChecked) {
            foreach (var gar in garantiesParam) {
                var selGar = isChecked ? garantiesJson.FirstOrDefault(g => g.Code == gar.CodeGarantie) : null;
                var garChecked = selGar != null;
                gar.NatureRetenue = garChecked ? NatureValue.Accordee : NatureValue.None;

                if (selGar != null) {
                    gar.Tarif.LCI.ValeurActualise = selGar.LCI.Unite == "CPX" ? 0 : decimal.TryParse(selGar.LCI.Valeur, out var valaLCI) ? valaLCI : 0;
                    gar.Tarif.LCI.ValeurOrigine = selGar.LCI.Unite == "CPX" ? 0 : decimal.TryParse(selGar.LCI.Valeur, out var valoLCI) ? valoLCI : 0;
                    gar.Tarif.LCI.ValeurTravail = selGar.LCI.Unite == "CPX" ? 0 : decimal.TryParse(selGar.LCI.Valeur, out var valwLCI) ? valwLCI : 0;
                    gar.Tarif.LCI.Unite = new Unite { Code = selGar.LCI.Unite };
                    gar.Tarif.LCI.Base = new BaseLCI { Code = selGar.LCI.Type };

                    gar.Tarif.Franchise.ValeurActualise = selGar.Franchise.Unite == "CPX" ? 0 : decimal.TryParse(selGar.Franchise.Valeur, out var valaFRH) ? valaFRH : 0;
                    gar.Tarif.Franchise.ValeurOrigine = selGar.Franchise.Unite == "CPX" ? 0 : decimal.TryParse(selGar.Franchise.Valeur, out var valoFRH) ? valoFRH : 0;
                    gar.Tarif.Franchise.ValeurTravail = selGar.Franchise.Unite == "CPX" ? 0 : decimal.TryParse(selGar.Franchise.Valeur, out var valwFRH) ? valwFRH : 0;
                    gar.Tarif.Franchise.Unite = new Unite { Code = selGar.Franchise.Unite };
                    gar.Tarif.Franchise.Base = new BaseLCI { Code = selGar.Franchise.Type };

                    gar.Tarif.PrimeValeur.ValeurActualise = decimal.TryParse(selGar.Prime.Valeur, out var valaPRI) ? valaPRI : 0;
                    gar.Tarif.PrimeValeur.ValeurOrigine = decimal.TryParse(selGar.Prime.Valeur, out var valoPRI) ? valoPRI : 0;
                    gar.Tarif.PrimeValeur.ValeurTravail = decimal.TryParse(selGar.Prime.Valeur, out var valwPRI) ? valwPRI : 0;
                    gar.Tarif.PrimeValeur.Unite = new Unite { Code = selGar.Prime.Unite };
                    gar.Tarif.PrimeProvisionnelle = decimal.TryParse(selGar.Prime.PrimeMini, out var valPRI) ? valPRI : 0;

                    gar.Assiette.ValeurActualise = decimal.TryParse(selGar.Assiette.Valeur, out var valaASS) ? valaASS : 0;
                    gar.Assiette.ValeurOrigine = decimal.TryParse(selGar.Assiette.Valeur, out var valoASS) ? valoASS : 0;
                    gar.Assiette.ValeurTravail = decimal.TryParse(selGar.Assiette.Valeur, out var valwASS) ? valwASS : 0;
                    gar.Assiette.Unite = new Unite { Code = selGar.Assiette.Unite };
                    gar.Assiette.Base = new BaseDeCalcul { Code = selGar.Assiette.Type };

                }
                SetGarantieJson(affId, cached, gar.SousGaranties, selGar?.Garanties ?? new List<global::OP.WSAS400.DTO.Contrats.Garanty>(), garChecked);

                SetSelectionGarantieJson(gar, garChecked);
                SetCache(affId, false, cached);
            }
        }

        public FormuleDto InitFirstFormuleAffaire(AffaireId affId) {
            var (f, r) = InitFormule(affId, 0, Enumerable.Empty<int>(), null, false);
            var formule = f.Adapt<FormuleDto>();
            formule.Risque = r.Adapt<RisqueDto>();
            return formule.Adapt<FormuleDto>();
        }

        public FormuleDto InitOptionFormuleOffre(AffaireId affId, int numFormule, int numRisque, IEnumerable<int> numObjets, int codeOption) {
            if (affId.TypeAffaire != AffaireType.Offre) {
                throw new BusinessValidationException(new ValidationError("Impossible d'ajouter une option. Type de police invalide"));
            }
            CheckCurrentApplications(affId, codeOption, numFormule, numRisque, numObjets);
            var (cached, expressions, risqueListCached) = GetFormulesExpressionsRisques(affId, false);
            var formules = cached.ToList();
            Formule formule = formules.First(x => x.FormuleNumber == numFormule);
            var affaire = affaireRepository.GetById(affId);
            affaire.Expressions = expressions;
            var parameters = parameterRepository.GetParamVolets();
            var truthTable = parameterRepository.GetParamNatures();
            var paramBlocRelations = parameterRepository.GetRelationBlocs();
            var paramGrRelations = parameterRepository.GetRelationGaranties();
            Risque risque = risqueListCached.FirstOrDefault(x => x.Numero == numRisque);
            var objs = numObjets.Select(x => risque.Objets.FirstOrDefault(o => o.Id.NumObjet == x)).Where(y => y != null);
            InitOption(affaire, parameters, truthTable, risque, objs, formule, null, paramBlocRelations, paramGrRelations, codeOption);
            formule.EnsureHierarchy();
            SetCache(affId, false, formules);
            return GetFormuleAffaire(affId, numFormule);
        }

        public (int, int) SetApplication(AffaireId affId, int numFormule, int numRisque, IEnumerable<int> numObjets, DateTime? dateAvenant, int? numOption = null) {
            var (cached, expressions, risqueListCached) = GetFormulesExpressionsRisques(affId, true);
            CheckCurrentApplications(affId, numOption ?? 1, numFormule, numRisque, numObjets);
            var affaire = affaireRepository.GetById(affId);
            var formule = cached.FirstOrDefault(x => x.FormuleNumber == numFormule);
            var newRisque = risqueListCached.FirstOrDefault(x => x.Numero == numRisque);
            affaire.Expressions = expressions;
            int newOpt = numOption.GetValueOrDefault(1);
            int newFor = numFormule;
            int risque = formule.Options.First(o => o.OptionNumber == numOption.GetValueOrDefault(1)).Applications.First().NumRisque;
            if (affId.TypeAffaire == AffaireType.Contrat) {
                SetApplicationContrat(numObjets, dateAvenant, affaire, formule, numOption, newRisque);
            }
            else if (affId.TypeAffaire == AffaireType.Offre) {
                var (f, o) = SetApplicationOffre(affaire, cached.ToList(), numFormule, numOption.Value, newRisque, numObjets);
                newOpt = o.OptionNumber;
                newFor = f.FormuleNumber;
                if (newFor != numFormule) {
                    cached = cached.Concat(new[] { f });
                }
            }
            SetCache(affId, false, cached);
            if (formule.FormuleNumber != newFor) {
                // force save of both formulas
                ValidateMultipleFormules(affId, new[] { formule.FormuleNumber, newFor });
            }
            return (newFor, newOpt);
        }

        public int DuplicateOption(AffaireId affaireId, int numFormule, int numOption) {
            if (affaireId.TypeAffaire != AffaireType.Offre) {
                throw new BusinessValidationException(new ValidationError("Impossible d'ajouter une option. Type de police invalide"));
            }
            var (cached, expressions, risqueListCached) = GetFormulesExpressionsRisques(affaireId, false);
            var formules = cached.ToList();
            Formule formule = formules.First(x => x.FormuleNumber == numFormule);
            var affaire = affaireRepository.GetById(affaireId);
            VerifyCloningOption(affaire, formule, numOption);
            int newOption = GetAvailableNumOption(formule);
            var optionCopy = formule.Options.First(o => o.OptionNumber == numOption).Adapt<Option>();
            optionCopy.ResetIds(newOption);
            formule.Options.Add(optionCopy);
            formule.EnsureHierarchy();
            SetCache(affaireId, false, formules);
            ValidateFormuleAffaire(new FormuleId(affaireId) { NumeroFormule = numFormule });
            return newOption;
        }

        public void DeleteOption(AffaireId affaireId, int numFormule, int numOption) {
            var (cached, expressions, risqueListCached) = GetFormulesExpressionsRisques(affaireId, false);
            var formules = cached.ToList();
            Formule formule = formules.First(x => x.FormuleNumber == numFormule);
            var affaire = affaireRepository.GetById(affaireId);
            VerifyDeletingOption(affaire, formule, numOption);
            if (formule.Options.Count == 1) {
                // trigger delete formula
                DeleteFormule(formule);
            }
            else {
                formule.EnsureHierarchy();
                formule.Options.RemoveAll(o => o.OptionNumber == numOption);
                SetCache(affaireId, false, formules);
                ValidateFormuleAffaire(new FormuleId(affaireId) { NumeroFormule = numFormule });
            }
        }

        private void DeleteFormule(Formule formule) {
            this.formuleRepository.Delete(formule);
            CancelFormuleAffaireChanges(formule.AffaireId);
        }

        private (Formule, Risque) InitFormule(AffaireId affId, int numRisque, IEnumerable<int> numObjets, DateTime? dateAvenant, bool isReadonly) {
            var (cached, expressions, risqueListCached) = GetFormulesExpressionsRisques(affId, isReadonly);
            if (numRisque <= 0) {
                numRisque = risqueListCached.Min(x => x.Numero);
            }
            CheckCurrentApplications(affId, 0, 0, numRisque, numObjets);
            Risque risque = risqueListCached.FirstOrDefault(x => x.Numero == numRisque);
            var formules = cached.ToList();
            var objs = numObjets.Select(x => risque.Objets.FirstOrDefault(o => o.Id.NumObjet == x)).Where(y => y != null);

            var affaire = affaireRepository.GetById(affId);
            affaire.Expressions = expressions;

            var parameters = parameterRepository.GetParamVolets();
            var truthTable = parameterRepository.GetParamNatures();
            var paramBlocRelations = parameterRepository.GetRelationBlocs();
            var paramGrRelations = parameterRepository.GetRelationGaranties();
            var formule = new Formule {
                AffaireId = affId,
                Alpha = Next(cached.Max(x => x.Alpha)),
                Chrono = (cached.Max(x => new int?(x.Chrono)) ?? 0) + 1,
                Cible = referentialRepository.GetCibleCatego(risque.Cible.Code, affaire.Branche.Code),
                FormuleNumber = (cached.Max(x => new int?(x.FormuleNumber)) ?? 0) + 1,
                Description = string.Empty,
                Id = 0
            };
            InitOption(affaire, parameters, truthTable, risque, objs, formule, dateAvenant, paramBlocRelations, paramGrRelations);
            formule.EnsureHierarchy();
            formules.Add(formule);
            SetCache(affId, false, formules);
            return (formule, risque);
        }

        private static Option InitOption(Affaire affaire, IEnumerable<OP.Domain.Parametrage.Formules.ParamVolet> parameters, TruthTable truthTable, Risque risque, IEnumerable<Objet> objets, Formule formule, DateTime? dateAvenant, ILookup<long, BlocRelation> blocRelations, ILookup<long, GarantieRelation> grRelations, int? numero = null) {
            if (numero.GetValueOrDefault() > 0) {
                VerifyNewOptionNumber(affaire, formule, numero.Value);
            }
            var option = new Option {
                DateAvenant = dateAvenant,
                NumeroAvenant = affaire.NumeroAvenant,
                NumAvenantCreation = affaire.NumeroAvenant,
                NumAvenantModif = affaire.NumeroAvenant,
                MontantsOption = new MontantsOption {

                },
                OptionNumber = numero.GetValueOrDefault() < 1 ? 1 : numero.Value
            };

            if (risque.NumeroAvenantCreation != affaire.NumeroAvenant) {
                var objetsSelection = (objets.Any() ? objets : risque.Objets).Where(x => x.NumeroAvenantCreation == affaire.NumeroAvenant);
                var isNewNiveauApplicationObjet = objetsSelection.Any() && objetsSelection.Count() != risque.Objets.Count();
                if (isNewNiveauApplicationObjet) {
                    option.Applications = objetsSelection.Select(x => new OP.Domain.Formule.Application {
                        Niveau = ApplicationNiveau.Objet,
                        NumObjet = x.Id.NumObjet,
                        NumRisque = x.Id.NumRisque
                    }).ToList();
                }
                else {
                    option.Applications = risque.MapSingle(r => new OP.Domain.Formule.Application {
                        Niveau = ApplicationNiveau.Risque,
                        NumObjet = 0,
                        NumRisque = r.Numero
                    }).ToList();
                }
            }
            else {
                var isNewNiveauApplicationObjet = objets.Any() && objets.Count() != risque.Objets.Count();

                if (isNewNiveauApplicationObjet) {
                    option.Applications = objets.Select(o => new OP.Domain.Formule.Application {
                        Niveau = ApplicationNiveau.Objet,
                        NumObjet = o.Id.NumObjet,
                        NumRisque = o.Id.NumRisque
                    }).ToList();
                }
                else {
                    option.Applications = risque.MapSingle(r => new OP.Domain.Formule.Application {
                        Niveau = ApplicationNiveau.Risque,
                        NumObjet = 0,
                        NumRisque = r.Numero
                    }).ToList();
                }
            }
            formule.Options.Add(option);
            formule.ApplyParameters(affaire, parameters, truthTable, false, affaire.Typologie, affaire.DateModeleApplicable);
            formule.SetDates(affaire, risque);
            formule.InitParamRelations(blocRelations, grRelations);
            return option;
        }

        private static void VerifyNewOptionNumber(Affaire affaire, Formule formule, int number) {
            int nbMax = Option.MaxNbByFormula;
            if (affaire.TypeAffaire != AffaireType.Offre) {
                throw new BusinessValidationException(new ValidationError("Impossible d'ajouter une option. Type de police invalide"));
            }
            else if (formule.Options.Count == nbMax) {
                throw new BusinessValidationException(new ValidationError("option", "Nombre maximum d'options atteint pour l'offre"));
            }
            else if (!formule.Options.Any() && number < 1 || number > nbMax) {
                throw new BusinessValidationException(new ValidationError("option", $"Le numéro d'option doit être compris entre 1 et {nbMax}"));
            }
            else if (formule.Options.Any(x => x.OptionNumber == number)) {
                throw new BusinessValidationException(new ValidationError("option", $"Le numéro d'option est déjà utilisé"));
            }
        }

        private static void VerifyCloningOption(Affaire affaire, Formule formule, int number) {
            int nbMax = Option.MaxNbByFormula;
            if (affaire.TypeAffaire != AffaireType.Offre) {
                throw new BusinessValidationException(new ValidationError("Opération invalide pour ce type de police"));
            }
            else if (formule.Options.Count == nbMax) {
                throw new BusinessValidationException(new ValidationError("option", "L'offre contient le nombre maximal d'options, duplication impossible"));
            }
            else if (number < 1 || number > nbMax) {
                throw new BusinessValidationException(new ValidationError("option", $"Le numéro d'option doit être compris entre 1 et {nbMax}"));
            }
            else if (!formule.Options.Any(x => x.OptionNumber == number)) {
                throw new BusinessValidationException(new ValidationError("option", $"Le numéro d'option n'existe pas, duplication impossible"));
            }
        }

        private static void VerifyDeletingOption(Affaire affaire, Formule formule, int number) {
            int nbMax = Option.MaxNbByFormula;
            if (affaire.TypeAffaire != AffaireType.Offre) {
                throw new BusinessValidationException(new ValidationError("Opération invalide pour ce type de police"));
            }
            else if (formule.Options.Count == 0) {
                throw new BusinessValidationException(new ValidationError("option", "L'offre ne contient aucune option"));
            }
            else if (number < 1 || number > nbMax) {
                throw new BusinessValidationException(new ValidationError("option", $"Le numéro d'option doit être compris entre 1 et {nbMax}"));
            }
            else if (!formule.Options.Any(x => x.OptionNumber == number)) {
                throw new BusinessValidationException(new ValidationError("option", $"Le numéro d'option n'existe pas"));
            }
        }

        private static void VerifyChangingRisqueOption(IEnumerable<Formule> allFormules, int numFormule, int newNumRisque) {
            Formule formule = allFormules.First(f => f.FormuleNumber == numFormule);
            if (formule.AffaireId.TypeAffaire != AffaireType.Offre) {
                throw new BusinessValidationException(new ValidationError("Opération invalide pour ce type de police"));
            }
            else {
                Formule newRsqFormule = allFormules.FirstOrDefault(f => f.FormuleNumber != numFormule && f.Options.FirstOrDefault()?.Applications.FirstOrDefault()?.NumRisque == newNumRisque);
                if (newRsqFormule != null) {
                    if (newRsqFormule.Options.Count == Option.MaxNbByFormula) {
                        throw new BusinessValidationException(new ValidationError($"Impossible changer de risque car la formule cible contient déjà {Option.MaxNbByFormula} options"));
                    }
                }
            }
        }

        public string Next(string alpha) {
            if (string.IsNullOrWhiteSpace(alpha)) {
                return "A";
            }
            Stack<char> stack = new Stack<char>();
            var retain = 1;
            foreach (var x in alpha.Reverse()) {
                if (x + retain > 'Z') {
                    retain = 1;
                    stack.Push('A');
                }
                else {
                    stack.Push((char)(x + retain));
                    retain = 0;
                }
            }
            var res = new string(stack.ToArray());
            if (retain == 1) {
                res = res + 'A';
            }
            return res;
        }

        public void ValidateFormuleAffaire(FormuleId formId) {
            var (formules, risques) = GetFormulesRisques(formId);
            var formule = formules.FirstOrDefault(f => f.FormuleNumber == formId.NumeroFormule);
            if (formule == null) {
                throw new Exception("Formule introuvable");
            }
            SaveFormuleFromCache(formId, risques, formule);
            CancelFormuleAffaireChanges(formule.AffaireId);
        }

        public void CancelFormuleAffaireChanges(AffaireId affId) {
            this.sharedCache.Invalidate<List<Formule>>($"editionFormule_{affId.AsAffaireKey()}");
            this.cache.Invalidate<List<Formule>>($"editionFormule_{affId.AsAffaireKey()}");
            this.cache.Invalidate<List<Risque>>($"editionFormule_{affId.AsAffaireKey()}_risques");
        }

        public void SetSelection(FormuleDto formuleInput) {
            var truthTable = parameterRepository.GetParamNatures();
            var formules = GetFormulesRisques(formuleInput.AffaireId, false).formules;
            var formule = formules.FirstOrDefault(f => f.FormuleNumber == formuleInput.FormuleNumber);
            if (formule == null) { return; }
            if (!string.IsNullOrWhiteSpace(formuleInput.Description)) {
                formule.Description = formuleInput.Description;
            }
            foreach (var optIn in formuleInput.Options) {
                var opt = formule.Options.FirstOrDefault(o => o.OptionNumber == optIn.OptionNumber);
                if (opt != null) {
                    SetSelectionVolets(truthTable, optIn.OptionVolets.ToArray(), opt.OptionVolets.ToArray());
                }
            }

            SetCache(formule.AffaireId, false, formules);
        }

        public void SetSelectionVolet(AffaireId affaire, OptionsDetailVoletDto voletDto, int numFormule, int numOption, DateTime? dateAvenant) {
            var truthTable = parameterRepository.GetParamNatures();
            var formules = GetFormulesRisques(affaire, false).formules;
            var formule = formules.FirstOrDefault(f => f.FormuleNumber == numFormule);
            if (formule == null) { return; }

            // assumes parent is already checked (should check this)
            var volet = formule.Options
                .First(o => o.OptionNumber == numOption)
                .OptionVolets
                .FirstOrDefault(v => voletDto.ParamVolet.CatVoletId == v.ParamVolet.CatVoletId);

            SetSelectionVolets(truthTable, new[] { voletDto }, new[] { volet }, affaire.NumeroAvenant, dateAvenant);
            SetCache(affaire, false, formules);
        }

        public void SetSelectionBloc(AffaireId affaire, OptionsDetailBlocDto blocDto, int numFormule, int numOption, DateTime? dateAvenant) {
            var truthTable = parameterRepository.GetParamNatures();
            var formules = GetFormulesRisques(affaire, false).formules;
            var formule = formules.FirstOrDefault(f => f.FormuleNumber == numFormule);
            if (formule == null) { return; }

            // assumes parent is already checked (should check this)
            var bloc = formule.Options.First(o => o.OptionNumber == numOption).OptionVolets
                .SelectMany(v => v.Blocs)
                .FirstOrDefault(b => blocDto.ParamBloc.CatBlocId == b.ParamBloc.CatBlocId);

            SetSelectionBlocs(truthTable, new[] { blocDto }, new[] { bloc }, affaire.NumeroAvenant, dateAvenant);
            SetCache(affaire, false, formules);
        }

        public void SetSelectionGarantie(AffaireId affaire, GarantieDto garantie, int numFormule, int numOption, DateTime? dateAvenant) {
            var truthTable = parameterRepository.GetParamNatures();
            var (g, formules) = GetGarantie(affaire, numFormule, numOption, garantie.CodeBloc, garantie.Sequence);

            // assumes parent is already checked (should check this)
            SetSelectionGarantiesRec(truthTable, new[] { garantie }, new[] { g }, affaire.NumeroAvenant, dateAvenant);
            SetCache(affaire, false, formules);
        }

        private static void SetSelectionVolets(TruthTable truthTable, IEnumerable<OptionsDetailVoletDto> voletsIn, IEnumerable<OptionVolet> volets, int? numeroAvenant = null, DateTime? dateAvenant = null) {
            foreach (var volIn in voletsIn) {
                var vol = volets.FirstOrDefault(o => o.ParamVolet.CatVoletId == volIn.ParamVolet.CatVoletId);
                if (vol == null) { continue; }
                vol.IsChecked = volIn.IsChecked;
                SetSelectionBlocs(truthTable, volIn.Blocs.ToArray(), vol.Blocs.ToArray(), numeroAvenant, dateAvenant);
            }
        }

        private static void SetSelectionBlocs(TruthTable truthTable, IEnumerable<OptionsDetailBlocDto> blocsIn, IEnumerable<OptionBloc> blocs, int? numeroAvenant = null, DateTime? dateAvenant = null) {
            foreach (var blocIn in blocsIn) {
                var bloc = blocs.FirstOrDefault(o => o.ParamBloc.CatBlocId == blocIn.ParamBloc.CatBlocId);
                if (bloc == null) { continue; }
                bloc.IsChecked = blocIn.IsChecked;
                var garantiesIn = blocIn.Garanties;
                var garanties = bloc.Garanties;
                SetSelectionGarantiesRec(truthTable, garantiesIn, garanties, numeroAvenant, dateAvenant);
            }
        }

        private static void SetSelectionGarantiesRec(TruthTable truthTable, IEnumerable<GarantieDto> garantiesIn, IEnumerable<Garantie> garanties, int? numeroAvenant = null, DateTime? dateAvenant = null) {
            foreach (var garIn in garantiesIn) {
                var gar = garanties.FirstOrDefault(o => o.ParamGarantie.Sequence == garIn.Sequence);
                gar.UpdateCheck(truthTable, garIn.NatureRetenue, garIn.IsChecked);
                if (dateAvenant.HasValue) {
                    DateTime? dtDeb = gar.DateDebut;
                    DateTime? dtStdDeb = gar.DatestandardDebut;
                    if (gar.IsVirtual) {
                        dtDeb = (gar.IsChecked ? dateAvenant : null);
                        dtStdDeb = gar.IsChecked ? dateAvenant : null;
                    }
                    else {
                        if (gar.IsChecked) {
                            gar.AssignNumerosAvenant(numeroAvenant.GetValueOrDefault());

                            if (gar.IsVirtualProposed(numeroAvenant) || gar.IsVirtualOptional(numeroAvenant)) {
                                dtDeb = dateAvenant;
                                dtStdDeb = dateAvenant;
                            }
                        }
                        else {
                            // restore previous values
                            gar.RestoreNumerosAvenant(numeroAvenant.GetValueOrDefault());
                            dtDeb = null;
                            dtStdDeb = null;
                        }
                    }

                    gar.DateDebut = dtDeb;
                    gar.DatestandardDebut = dtStdDeb;

                    gar.AdjustDates();
                }

                var sousGarantiesIn = garIn.SousGaranties ?? Enumerable.Empty<GarantieDto>();
                var sousGaranties = gar.SousGaranties;
                gar.AdjustDates();
                SetSelectionGarantiesRec(truthTable, sousGarantiesIn, sousGaranties, numeroAvenant, dateAvenant);
            }
        }

        private void SetSelectionGarantieJson(Garantie garantie, bool isChecked) {
            var truthTable = parameterRepository.GetParamNatures();
            garantie.UpdateCheck(truthTable, garantie.NatureRetenue, isChecked);
            garantie.AdjustDates();
        }

        public AffaireFormuleDto GetFormulesAffaire(AffaireId affId, bool isReadOnly = false) {
            (IEnumerable<Formule> cached, IEnumerable<Risque> risqueListCached) = GetFormulesRisques(affId, isReadOnly);

            foreach (var item in cached) {
                item.EnsureHierarchy();
            }

            var formuleList = cached.Adapt<List<FormuleDto>>();

            for (int indexFormule = 0; indexFormule < cached.Count(); indexFormule++) {
                Formule formule = cached.ElementAt(indexFormule);
                for (int indexOption = 0; indexOption < formule.Options.Count(); indexOption++) {
                    var option = formule.Options[indexOption];
                    formuleList[indexFormule].Options[indexOption].IsModifiedForAvenant = option.IsModifiedForAvenant(affId.NumeroAvenant ?? 0);
                }
            }

            var risqueList = risqueListCached.Adapt<IEnumerable<RisqueDto>>();
            formuleList.ForEach(f => f.Risque = risqueList.FirstOrDefault(x => f.Options.FirstOrDefault()?.Applications.Any(ap => ap.NumRisque == x.Numero) ?? false));
            var affaire = this.affaireRepository.GetById(affId);
            AffaireFormuleDto affaireFormuleDto = new AffaireFormuleDto() { AffaireId = affId, DateEffetAvenant = affaire.DateEffetAvenant };
            affaireFormuleDto.Formules.AddRange(formuleList);

            return affaireFormuleDto;
        }

        public void SetGarantieDetails(OptionId id, GarantieDetailsDto garantie) {
            const bool IsReadonly = false;
            var (formules, risques) = GetFormulesRisques(id, IsReadonly);
            var gar = FindGarantie(formules, id.NumeroFormule, id.NumeroOption, garantie.CodeBloc, garantie.Sequence);

            var formule = formules.FirstOrDefault(x => x.FormuleNumber == id.NumeroFormule);
            formule.EnsureHierarchy();

            var garTest = gar.ShallowCopy();
            UpdateGarantieWith(id, garTest, garantie);

            var errors = garTest.CheckDates();
            if (errors.Any()) {
                throw new BusinessValidationException(errors, $"Dates invalides : {errors.JoinString(x => x.Error, "\r\n")}");
            }

            UpdateGarantieWith(id, gar, garantie);
            SetCache(id, IsReadonly, formules);
        }

        public IEnumerable<ValidationError> CheckGarantiesDatesInFormule(AffaireId affaireId, int numOption, int numFormule) {
            var (formules, risques) = GetFormulesRisques(affaireId);
            var affaire = affaireRepository.GetById(affaireId);
            formules.FirstOrDefault(x => x.FormuleNumber == numFormule).EnsureHierarchy();
            var option = formules.FirstOrDefault(x => x.FormuleNumber == numFormule).Options.FirstOrDefault(x => x.OptionNumber == numOption);
            var errors = option.OptionVolets.Where(v => v.IsChecked)
                        .SelectMany(x => x.Blocs).Where(b => b.IsChecked)
                        .SelectMany(x => x.Garanties).Where(x => x.IsChecked)
                        .SelectMany(g => g.Validate()).ToList();
            return errors;
        }

        private void SaveFormuleFromCache(AffaireId affaireId, IEnumerable<Risque> risques, Formule formule) {
            formule.AffaireId.NumeroAvenant = affaireId.NumeroAvenant;
            Risque risque = formule.Options.First().FilterRisques(risques);
            foreach (var option in formule.Options.Where(x => x.DateAvenant.HasValue))
            {
                CheckDateAvenantOption(affaireId, risque, option, formule);
            }

            var errors = formule.Validate();
            if (errors.Any()) {
                throw new BusinessException($"Erreurs de validation :{Environment.NewLine}{errors.Select(e => $"{e.TargetType} {e.TargetCode} : {e.Error} ").JoinString(Environment.NewLine)}");
            }
            this.formuleRepository.SaveFormule(risque, formule, this.context.UserId);
        }

        private void ValidateMultipleFormules(AffaireId affId, IEnumerable<int> numsFormules) {
            var (formules, risques) = GetFormulesRisques(affId);
            foreach (int num in numsFormules) {
                var formule = formules.FirstOrDefault(f => f.FormuleNumber == num);
                if (formule == null) {
                    throw new Exception($"Formule {num} introuvable");
                }
                SaveFormuleFromCache(affId, risques, formule);
            }
            CancelFormuleAffaireChanges(affId);
        }

        private void SetApplicationContrat(IEnumerable<int> numObjets, DateTime? dateAvenant, Affaire affaire, Formule formule, int? numOption, Risque newRisque) {
            var hasNewCible = formule.Cible.Cible.Code != newRisque.Cible.Code;
            if (hasNewCible) {
                formule.Cible = this.referentialRepository.GetCibleCatego(newRisque.Cible.Code, formule.Cible.Branche.Code);
            }

            Option option = formule?.Options.FirstOrDefault(o => o.OptionNumber == numOption.GetValueOrDefault(1));
            if (option != null) {
                Option opt = option;
                if (hasNewCible) {
                    var parameters = parameterRepository.GetParamVolets();
                    var truthTable = parameterRepository.GetParamNatures();
                    var paramBlocRelations = parameterRepository.GetRelationBlocs();
                    var paramGrRelations = parameterRepository.GetRelationGaranties();
                    formule.Options.Remove(opt);
                    opt = InitOption(affaire, parameters, truthTable, newRisque, newRisque.Objets.Where(x => numObjets.Contains(x.Id.NumObjet)), formule, dateAvenant, paramBlocRelations, paramGrRelations);
                }
                else {
                    option.UpdateApplications(numObjets, newRisque);
                }
            }
        }

        private (Formule newFormule, Option newOption) SetApplicationOffre(Affaire affaire, IEnumerable<Formule> allFormules, int numFormule, int numOption, Risque newRisque, IEnumerable<int> numObjets) {
            Formule formule = allFormules.First(f => f.FormuleNumber == numFormule);
            Option option = formule.Options.First(o => o.OptionNumber == numOption);
            Formule newFormule = formule;
            Option newOption = option;
            if (option.Applications.First().NumRisque == newRisque.Numero) {
                SetApplicationContrat(numObjets, null, affaire, formule, numOption, newRisque);
            }
            else {
                VerifyChangingRisqueOption(allFormules, numFormule, newRisque.Numero);
                newFormule = allFormules.FirstOrDefault(f => f.FormuleNumber != numFormule && f.Options.FirstOrDefault()?.Applications.FirstOrDefault()?.NumRisque == newRisque.Numero);
                bool isNewFormula = newFormule == null;
                int newNumOption = 1;
                if (isNewFormula) {
                    (newFormule, _) = InitFormule(formule.AffaireId, newRisque.Numero, numObjets, null, false);
                    newFormule.Description = $"Nouvelle Formule ({newFormule.FormuleNumber})";
                }
                else {
                    newNumOption = GetAvailableNumOption(newFormule);
                }
                numOption = newNumOption;
                formule.Options.Remove(option);
                if (formule.Cible.Cible.Code == newRisque.Cible.Code) {
                    var autoOption = newFormule.Options.First();
                    newFormule.Options.RemoveAt(0);
                    newOption = option.Adapt<Option>();
                    newOption.ResetIds(newNumOption);
                    newOption.Applications = autoOption.Applications;
                    newFormule.Options.Add(newOption);
                }
                else {
                    if (!isNewFormula) {
                        InitOptionFormuleOffre(formule.AffaireId, newFormule.FormuleNumber, newRisque.Numero, numObjets, newNumOption);
                    }
                }
                newFormule.EnsureHierarchy();
            }
            return (newFormule, newOption);
        }

        internal class CacheEntry<T> {
            internal bool IsReadOnly = false;
            internal T Value = default(T);
            public CacheEntry() {

            }
            public CacheEntry(bool isReadOnly, T value) {
                this.Value = value;
                this.IsReadOnly = isReadOnly;
            }
        }

        private (IEnumerable<Formule> formules, Expressions expressions, IEnumerable<Risque> risqueList) GetFormulesExpressionsRisques(AffaireId affId, bool isReadOnly = false) {
            IEnumerable<Formule> formules = default(List<Formule>);
            var expressions = this.cache.Get<Expressions>($"editionFormule_{affId.AsAffaireKey()}_expr").Adapt<Expressions>();
            var cached = this.cache.Get<List<Formule>>($"editionFormule_{affId.AsAffaireKey()}").Adapt<List<Formule>>();
            var risqueList = this.cache.Get<List<Risque>>($"editionFormule_{affId.AsAffaireKey()}_risques").Adapt<List<Risque>>();
            if (cached == default(List<Formule>)) {
                (formules, expressions) = this.formuleRepository.GetFormulesExpressionForAffaire(affId);
                risqueList = this.risqueRepository.GetRisquesByAffaire(affId).ToList();
                ApplyParameters(affId, expressions, isReadOnly, formules, risqueList);
                SetCache(affId, isReadOnly, expressions);
                SetCache(affId, isReadOnly, formules);
                SetCache(affId, isReadOnly, risqueList);
            }
            else {
                formules = cached;
                var exprForm = Formule.GetAllGaranties(formules)/*.Where(g => g.Tarif != null)*/.Select(x => x.Tarif).SelectMany(t => new[] { t.LCI?.ExpressionComplexe, t.Franchise?.ExpressionComplexe }).Where(x => x != null).Distinct();
                var exprAll = exprForm.Concat(expressions.All.Where(t => !exprForm.Any(f => t.GetType() == f.GetType() && t.Code == f.Code)));
                expressions = new Expressions(exprAll);
                SetCache(affId, isReadOnly, expressions);

            }
            return (formules, expressions, risqueList);
        }
        private (IEnumerable<Formule> formules, IEnumerable<Risque> risqueList) GetFormulesRisques(AffaireId affId, bool isReadOnly = false) {
            var (f, _, r) = (GetFormulesExpressionsRisques(affId, isReadOnly));
            return (f, r);
        }

        private void ApplyParameters(AffaireId affId, Expressions expressions, bool isReadOnly, IEnumerable<Formule> formules, IEnumerable<Risque> risqueList) {
            var affaire = this.affaireRepository.GetById(affId);
            if (isReadOnly || affId.IsHisto) {
                formules.ToList().ForEach(formule => formule.SetDates(affaire, risqueList));
            }
            else {
                var paramVolets = this.parameterRepository.GetParamVolets();
                var blocRelations = parameterRepository.GetRelationBlocs();
                var grRelations = parameterRepository.GetRelationGaranties();
                affaire.Expressions = expressions;
                var table = this.parameterRepository.GetParamNatures();

                foreach (var formule in formules) {
                    formule.ApplyParameters(affaire,
                        paramVolets,
                        table,
                        affaire.Etat == EtatAffaire.NonValidee && isReadOnly,
                        affaire.Typologie,
                        affaire.DateModeleApplicable);
                    formule.SetDates(affaire, risqueList);
                    formule.InitParamRelations(blocRelations, grRelations);
                }
            }
        }

        private void SetCache(AffaireId affId, bool isReadOnly, IEnumerable<Formule> formules) {
            if (isReadOnly) {
                this.sharedCache.Set($"{affId.AsAffaireKey()}", formules as List<Formule> ?? formules.ToList());
            }
            else {
                this.cache.Set($"editionFormule_{affId.AsAffaireKey()}", formules as List<Formule> ?? formules.ToList());
            }
        }
        private void SetCache(AffaireId affId, bool isReadOnly, Expressions expressions) {
            if (!isReadOnly) {
                this.cache.Set($"editionFormule_{affId.AsAffaireKey()}_expr", expressions);
            }
        }
        private void SetCache(AffaireId affId, bool isReadOnly, IEnumerable<Risque> risques) {
            this.cache.Set($"editionFormule_{affId.AsAffaireKey()}_risques", risques as List<Risque> ?? risques.ToList());
        }

        private void UpdateGarantieWith(OptionId optionId, Garantie gar, GarantieDetailsDto updater) {
            if (
                gar.DateDebut != updater.DateDebut ||
                gar.DateFinDeGarantie != (updater.IsDuree ? null : updater.DateFin) ||
                !gar.DureeUnite.IsSameCodeAs(updater.CodeDureeUnite) ||
                (gar.Duree ?? 0) != (updater.IsDuree ? updater.Duree : 0) ||
                (gar.IsApplicationCATNAT ?? false) != updater.HasCATNAT ||
                (gar.IsAlimMontantReference ?? false) != updater.InclusMontant ||
                (gar.IsIndexe ?? false) != updater.IsGarantieIndexee ||
                gar.TypeAlimentation != updater.CodeAlimentationAssiette.AsEnum<AlimentationValue>() ||
                !gar.Taxe.IsSameCodeAs(updater.CodeTaxe) ||
                !gar.PeriodeApplication.IsSameCodeAs(updater.CodeTypeApplication) ||
                !gar.TypeEmission.IsSameCodeAs(updater.CodeTypeEmission)
            ) {
                gar.IsFlagModifie = true;
                gar.DateDebut = updater.DateDebut;
                gar.DateFinDeGarantie = updater.IsDuree ? null : updater.DateFin;
                gar.DureeUnite = updater.IsDuree ? new OP.Domain.Referentiel.UniteDuree { Code = updater.CodeDureeUnite } : null;
                gar.Duree = updater.IsDuree ? updater.Duree : 0;
                gar.IsApplicationCATNAT = updater.HasCATNAT;
                gar.IsAlimMontantReference = updater.InclusMontant;
                gar.IsIndexe = updater.IsGarantieIndexee;
                gar.TypeAlimentation = updater.CodeAlimentationAssiette.AsEnum<AlimentationValue>();
                gar.Taxe = new Taxe { Code = updater.CodeTaxe };
                gar.PeriodeApplication = new PeriodeApplication { Code = updater.CodeTypeApplication };
                gar.TypeEmission = new TypeEmission { Code = updater.CodeTypeEmission };
                gar.NumeroAvenantModif = optionId.NumeroAvenant.GetValueOrDefault();
            }
        }

        public GarantieDetailsDto GetGarantieDetails(AffaireId id, int numOption, int numFormule, string codeBloc, long sequence, bool isReadOnly, DateTime? dateAvenant = null) {
            Garantie garantie = null;
            var (formules, risques) = GetFormulesRisques(id, isReadOnly);
            var option = formules.First(f => f.FormuleNumber == numFormule).Options.First(o => o.OptionNumber == numOption);
            var risque = option.FilterRisques(risques);

            option.OptionVolets.Any(v => (garantie = v.FindGarantieBySeq(codeBloc, sequence)) != null);

            if (garantie == null) {
                return null;
            }

            garantie.ApplyDatesCurrentAvenant(id, option, dateAvenant);

            var dto = garantie.Adapt<GarantieDetailsDto>();
            if (dto.NatureRetenue == NatureValue.None) {
                dto.LabelNatureRetenue = string.Empty;
            }
            else {
                dto.LabelNatureRetenue = referentialRepository.GetValues<NatureGarantie>().First(x => x.Code == dto.NatureRetenue.AsString()).Libelle;
            }

            dto.IsTemporaire = risque.IsTemporaire;
            if (!dto.IsTemporaire) {
                var affaire = affaireRepository.GetById(id);
                dto.IsTemporaire = affaire.Periodicite?.Code == "U" || affaire.Periodicite?.Code == "E";
            }

            return dto;
        }

        public GarantieDto GetBasicGarantieInfos(AffaireId affaireId, long sequence) {
            var dto = this.garantieRepository.GetBySequence(new GarantieUniqueId { AffaireId = affaireId, Sequence = sequence }).Adapt<GarantieDto>();
            if (dto != null) {
                dto.Libelle = this.garantieRepository.GetRefLibelle(dto.CodeGarantie);
            }
            return dto;
        }

        public GarantieDto GetGarantie(AffaireId id, int numOption, int numFormule, string codeBloc, long sequence, bool isReadonly) {
            Garantie garantie = FindGarantie(id, numOption, numFormule, codeBloc, sequence, isReadonly);

            if (garantie == null) {
                return null;
            }

            var dto = garantie.Adapt<GarantieDto>();
            return dto;
        }

        public InventaireDto GetGarantieInventaire(AffaireId id, int numOption, int numFormule, string codeBloc, long sequenceGarantie, bool isReadOnly) {
            Inventaire inventaire = null;
            Garantie garantie = FindGarantie(id, numOption, numFormule, codeBloc, sequenceGarantie, isReadOnly);
            if (garantie == null || !garantie.InventairePossible) {
                return null;
            }

            if (garantie.Inventaire == null) {
                garantie.EnsureInventaire();
            }
            inventaire = garantie.Inventaire;
            inventaire.EnsureItemNumbers();
            return inventaire.Adapt<InventaireDto>();
        }

        public void AddOrUpdateGarantieInventaireItem(AffaireId id, int numOption, int numFormule, string codeBloc, long sequenceGarantie, InventaireItem item) {
            Garantie gar;
            IEnumerable<Formule> formules;
            (gar, formules) = GetGarantie(id, numFormule, numOption, codeBloc, sequenceGarantie);

            if (gar.Inventaire == null) {
                InitInventaireInternal(id, gar);
            }

            var p = gar.Inventaire.Items.FirstOrDefault(x => x.Id > 0 && x.Id == item.Id || x.NumeroLigne == item.NumeroLigne);

            if (p == null) {
                p = gar.Inventaire.MakeItem();
                p.NumeroLigne = item.NumeroLigne;
                gar.Inventaire.Items.Add(p);
            }
            if (p is PersonneDesigneeIndispo pdi) {
                var pdid = item as PersonneDesigneeIndispo;
                pdi.DateNaissance = pdid.DateNaissance;
                pdi.Extention = pdid.Extention;
                pdi.Fonction = pdid.Fonction;
                pdi.Franchise = pdid.Franchise;
                pdi.Montant = pdid.Montant;
                pdi.Nom = pdid.Nom;
                pdi.NumeroLigne = pdid.NumeroLigne;
                pdi.Prenom = pdid.Prenom;
            }
            else {
                if (p is PersonneDesigneeIndispoTournage pdit) {
                    var pdid = item as PersonneDesigneeIndispoTournage;
                    pdit.DateNaissance = pdid.DateNaissance;
                    pdit.Extention = pdid.Extention;
                    pdit.Fonction = pdid.Fonction;
                    pdit.Franchise = pdid.Franchise;
                    pdit.Montant = pdid.Montant;
                    pdit.Nom = pdid.Nom;
                    pdit.NumeroLigne = pdid.NumeroLigne;
                    pdit.Prenom = pdid.Prenom;
                }
            }

            SetCache(id, false, formules);
        }

        public void DeleteGarantieInventaireItem(AffaireId id, int numOption, int numFormule, string codeBloc, long sequenceGarantie, int numeroLigne) {
            var formules = GetFormulesRisques(id, false).formules;
            var gar =
                formules
                .FirstOrDefault(f => f.FormuleNumber == numFormule)?.Options
                .FirstOrDefault(o => o.OptionNumber == numOption)
                ?.FindGarantieBySeq(codeBloc, sequenceGarantie);
            if (gar == null) { return; }
            var items = gar.Inventaire.Items;
            items.Remove(items.FirstOrDefault(x => x.NumeroLigne == numeroLigne));
            SetCache(id, false, formules);
        }

        public void DeleteWholeGarantieInventaire(AffaireId id, int numOption, int numFormule, string codeBloc, long sequenceGarantie) {
            var formules = GetFormulesRisques(id, false).formules;
            var gar =
                formules
                .FirstOrDefault(f => f.FormuleNumber == numFormule)?.Options
                .FirstOrDefault(o => o.OptionNumber == numOption)
                ?.FindGarantieBySeq(codeBloc, sequenceGarantie);
            if (gar == null) { return; }
            gar.Inventaire = null;
            SetCache(id, false, formules);
        }

        public void SetDateEffetOption(AffaireId id, int numOption, int numFormule, DateTime dateEffetModif) {
            if (id.IsHisto) {
                throw new BusinessException("Il est interdit de modifer l'historique");
            }
            var (formules, risques) = GetFormulesRisques(id);

            Formule formule = formules.FirstOrDefault(x => x.FormuleNumber == numFormule);
            var option = formule.Options.FirstOrDefault(x => x.OptionNumber == numOption);
            var affaire = affaireRepository.GetById(id);

            option.NumAvenantModif = id.NumeroAvenant.Value;
            option.DateAvenant = dateEffetModif;
            formule.SetDates(affaire, risques);
            SetCache(id, false, formules);
            CheckDateAvenantOption(id, option.FilterRisques(risques), option, formule);
        }

        public void StartNewAvenant(AffaireId id, int numOption, int numFormule, DateTime dateEffetModif) {
            if (id.IsHisto) {
                throw new BusinessException("Il est interdit de modifer l'historique");
            }
            var (formules, risques) = GetFormulesRisques(id);

            Formule formule = formules.FirstOrDefault(x => x.FormuleNumber == numFormule);
            var option = formule.Options.FirstOrDefault(x => x.OptionNumber == numOption);
            var risque = option.FilterRisques(risques);

            option.NumAvenantModif = id.NumeroAvenant.Value;

            // when starting AVN, assign date of Risque if greater than AVN Date
            option.DateAvenant = risque.DateEffetAvenant > dateEffetModif ? risque.DateEffetAvenant : dateEffetModif;
            SetCache(id, false, formules);
            CheckDateAvenantOption(id, risque, option, formule);
        }

        private void CheckDateAvenantOption(AffaireId id, Risque risque, Option option, Formule formule)
        {
            var affaire = affaireRepository.GetById(id);
            var errors = option.CheckDateAvenant(risque, affaire, formule);
            if (errors.Any())
            {
                throw new BusinessValidationException(
                    errors,
                    errors.Select(e => $"{e.TargetType} {e.TargetCode} : {e.Error} ").JoinString(Environment.NewLine));
            }
        }

        public void SaveInventaire(AffaireId id, int numOption, int numFormule, string codeBloc, long sequenceGarantie, InventaireDto inventaire) {
            Garantie gar;
            IEnumerable<Formule> formules;
            var aff = affaireRepository.GetById(id);
            (gar, formules) = GetGarantie(id, numFormule, numOption, codeBloc, sequenceGarantie);

            Inventaire invent = InitInventaireInternal(id, gar);
            bool wasReport = invent.ReportvaleurObjet ?? false;

            invent.NumChrono = inventaire.NumChrono;
            invent.Descriptif = inventaire.Descriptif;
            invent.Designation = inventaire.Designation;
            invent.IsHTnotTTC = inventaire.IsHTnotTTC;
            invent.ReportvaleurObjet = inventaire.ReportvaleurObjet;
            invent.Typedevaleur = inventaire.Typedevaleur;
            invent.ValeursIndice.ValeurOrigine = aff.ValeurIndiceOrigine;
            invent.ValeursIndice.ValeurActualise = aff.ValeurIndiceActualisee;
            invent.Valeurs = new ValeursUnite {
                Unite = inventaire.Valeurs.Unite,
                ValeurActualise = inventaire.Valeurs.ValeurActualise,
                ValeurOrigine = inventaire.Valeurs.ValeurOrigine,
                ValeurTravail = inventaire.Valeurs.ValeurTravail,
            };
            invent.Items = inventaire.Items.Adapt<IEnumerable<InventaireItem>>().ToList();

            gar.ReporterInventaire(inventaire.ReportvaleurObjet.GetValueOrDefault(), wasReport);

            SetCache(id, false, formules);

        }

        public IEnumerable<OptionDto> GetOptionsAppsWithHisto(string codeAffaire, int version) {
            var histo = this.formuleRepository.GetOptionsApplicationsFullHisto(codeAffaire, version);
            var currentId = this.affaireRepository.GetCurrentId(codeAffaire, version);
            var current = this.formuleRepository.GetOptionsSimple(currentId).ToList();
            current.AddRange(histo.OrderByDescending(x => x.Formule.AffaireId.NumeroAvenant));
            return current.Select(x => x.Adapt<OptionDto>());
        }

        public IEnumerable<RisqueDto> GetApplicationsFormule(AffaireId affaireId, int numOption, int numFormule) {
            var risques = this.risqueRepository.GetRisquesByAffaire(affaireId);
            var option = this.formuleRepository.GetOptionsSimple(affaireId).First(x => x.OptionNumber == numOption && x.Formule.FormuleNumber == numFormule);
            return option.Applications.GroupBy(a => a.NumRisque).Select(g => {
                var rsq = risques.First(x => x.Numero == g.Key);
                var objs = rsq.Objets.ToDictionary(o => o.Id.NumObjet);
                return new RisqueDto {
                    Numero = g.Key,
                    AffaireId = affaireId,
                    Designation = rsq.Designation,
                    Objets = g.Any(o => o.NumObjet > 0) ? g.Select(o => new ObjetDto {
                        NumRisque = g.Key,
                        Code = o.NumObjet,
                        Description = objs[o.NumObjet].Description,
                        Designation = objs[o.NumObjet].Designation
                    }).ToList() : objs.Values.Select(o => new ObjetDto {
                        NumRisque = g.Key,
                        Code = o.Id.NumObjet,
                        Description = o.Description
                    }).ToList()
                };
            });
        }

        public bool IsAvnDisabled(AffaireId affaireId, int numeroOption, int numeroFormule) {
            if (affaireId.IsHisto || affaireId.TypeAffaire == AffaireType.Offre || numeroFormule == 0) {
                return false;
            }

            var option = this.formuleRepository.GetOptionsSimple(affaireId).FirstOrDefault(x => x.OptionNumber == numeroOption && x.Formule.FormuleNumber == numeroFormule);
            return option?.NumAvenantModif != affaireId.NumeroAvenant;
        }

        private Inventaire InitInventaireInternal(AffaireId id, Garantie gar) {
            var invent = gar.Inventaire;
            if (invent == null) {
                invent = new Inventaire { };
                invent.TypeInventaire = gar.ParamGarantie.ParamGarantie.TypeInventaire;
                gar.Inventaire = invent;
            }
            if (invent.Id == 0) {
                invent.Affaire = id;
                invent.Id = -1;

            }
            return invent;
        }

        private Garantie FindGarantie(AffaireId id, int numOption, int numFormule, string codeBloc, long sequence, bool isReadOnly) {
            var formule = GetFormulesRisques(id, isReadOnly).formules.FirstOrDefault(f => f.FormuleNumber == numFormule);
            if (formule == null) {
                return null;
            }

            //formule.EnsureInventaires();
            var singleOption = formule.Options?.First(o => o.OptionNumber == numOption);
            if (singleOption == null) {
                return null;
            }

            Garantie garantie = null;
            singleOption.OptionVolets.Any(v => (garantie = v.FindGarantieBySeq(codeBloc, sequence)) != null);

            return garantie;
        }

        public void AddOrUpdatePortees(AffaireId id, int numOption, int numFormule, string codeBloc, long sequenceGarantie, ICollection<PorteeGarantieDto> portees, bool reportCalcul) {
            Garantie gar;
            IEnumerable<Formule> formules;
            (gar, formules) = GetGarantie(id, numFormule, numOption, codeBloc, sequenceGarantie);

            gar.Portees = gar.Portees ?? new List<PorteeGarantie>();
            foreach (var portee in portees.Where(x => !x.IsRemoved)) {
                var p = gar.Portees.FirstOrDefault(x => x.NumObjet == portee.NumObjet);

                if (p == null) {
                    p = new PorteeGarantie {
                        NumObjet = portee.NumObjet,
                        RisqueId = portee.RisqueId,
                    };
                    gar.Portees.Add(p);
                }
                p.Action = portee.CodeAction.AsEnum<ActionValue>();
                p.CodeAction = portee.CodeAction;
                p.GarantieId = gar.Id;
                p.Montant = portee.Valeurs.ValeurActualise;
                p.TypeCalcul = portee.TypeCalcul;
                p.ValeursPrime = portee.Valeurs.Adapt<ValeursUnite>();
            }
            foreach (var portee in portees.Where(x => x.IsRemoved)) {
                var p = gar.Portees.FirstOrDefault(x => x.NumObjet == portee.NumObjet);
                gar.Portees.Remove(p);
            }
            SetCache(id, false, formules);
        }
        private (Garantie, IEnumerable<Formule>) GetGarantie(AffaireId id, int numFormule, int numOption, string codeBloc, long sequenceGarantie) {
            var formules = GetFormulesRisques(id, false).formules;
            Garantie gar = FindGarantie(formules, numFormule, numOption, codeBloc, sequenceGarantie);
            return (gar, formules);

        }

        private void CheckCurrentApplications(AffaireId affaireId, int numOption, int numFormule, int numRisque, IEnumerable<int> numObjets) {
            var allOptions = this.formuleRepository.GetOptionsSimple(affaireId).ToList();
            var options = allOptions.Where(x => x.Formule.FormuleNumber != numFormule);
            if (!options.Any(o => o.Applications.Any(a => a.NumRisque == numRisque))) {
                return;
            }
            var applications = options.SelectMany(o => o.Applications).Where(a => a.NumRisque == numRisque);
            if ((!numObjets?.Any() ?? true) || applications.Any(a => a.Niveau == ApplicationNiveau.Risque)) {
                throw new BusinessException("Le risque est déjà appliqué à une ou plusieures formules");
            }
            if (applications.Any(a => numObjets.Contains(a.NumObjet))) {
                throw new BusinessException("Le risque ou les objets sont déjà appliqués à d'autres formules");
            }
        }

        private static Garantie FindGarantie(IEnumerable<Formule> formules, int numFormule, int numOption, string codeBloc, long sequenceGarantie) {
            return formules
                .FirstOrDefault(f => f.FormuleNumber == numFormule)?.Options
                .FirstOrDefault(o => o.OptionNumber == numOption)
                ?.FindGarantieBySeq(codeBloc, sequenceGarantie);
        }

    }
}
