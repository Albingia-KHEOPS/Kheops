using Albingia.Kheops.Common;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Application.Contracts;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using Mapster;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Domain.Extension;
using Albingia.Kheops.OP.Domain.Transverse;
using Albingia.Kheops.OP.Domain.Referentiel;
using OP.WSAS400.DTO.Ecran.ConditionRisqueGarantie;
using ALBINGIA.Framework.Common.Constants;
using Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;

namespace Albingia.Kheops.OP.Application.Infrastructure.Services {
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AffaireService : IAffairePort
    {
        const string NouvellesAffairesCacheKey = "editionNouvelleAffair_";
        private static readonly object _syncroot = new object();
        private readonly IConfig config;
        private readonly IAffaireRepository affRepo;
        private readonly IParamRepository parRepo;
        private readonly IReferentialRepository referentialRepository;
        private readonly IRisqueRepository risqueRepository;
        private readonly IPrimeRepository primeRepository;
        private readonly ISinistreRepository sinistreRepository;
        private readonly IOffreRepository offreRepository;
        private readonly IEnumerable<IRepriseAvenantRepository> repriseRepositories;
        private readonly IAdresseRepository adresseRepository;
        private readonly IConnexiteRepository connexiteRepository;
        private readonly ISessionContext context;
        private readonly ILiveDataCache cache;
        private readonly IOffreRepository offresRepository;
        private readonly IRegularisationRepository regularisationRepository;
        private readonly IFormuleRepository formuleRepository;
        private readonly IGarantieRepository garantieRepository;
        private readonly IUserPort userService;
        private readonly IEngagementRepository engagementRepository;
        private readonly IObservationRepository observationRepository;

        public AffaireService(IConfig conf,
            IInfosSpecifiquesRepository infosSpecifiqueRepository,
            IAffaireRepository affaireRepository,
            IReferentialRepository referentialRepository,
            IParamRepository parRepo,
            IRisqueRepository risqueRepository,
            IFormuleRepository formuleRepository,
            IGarantieRepository garantieRepository,
            IDesignationRepository designationRepository,
            IClauseRepository clauseRepository,
            IQuittanceRepository quittanceRepository,
            IInformationSpecifiqueRepository informationSpecifiqueRepository,
            IEngagementRepository engagementRepository,
            IInventaireRepository inventaireRepository,
            IAdresseRepository adresseRepository,
            ISuspensionRepository suspensionRepository,
            IOppositionRepository oppositionRepository,
            ITransverseRepository transverseRepository,
            IControleRepository controleRepository,
            IObservationRepository observationRepository,
            IPrimeRepository primeRepository,
            ISinistreRepository sinistreRepository,
            IOffreRepository offreRepository,
            IRegularisationRepository regularisationRepository,
            ISessionContext context,
            ILiveDataCache cache,
            IConnexiteRepository connexiteRepository,
            IOffreRepository offresRepository,
            IUserPort userService) {
            this.config = conf;
            this.affRepo = affaireRepository;
            this.parRepo = parRepo;
            this.risqueRepository = risqueRepository;
            this.cache = cache;
            this.context = context;
            this.adresseRepository = adresseRepository;
            this.repriseRepositories = new IRepriseAvenantRepository[] {
                designationRepository,
                informationSpecifiqueRepository,
                infosSpecifiqueRepository,
                engagementRepository,
                clauseRepository,
                quittanceRepository,
                inventaireRepository,
                formuleRepository,
                risqueRepository,
                regularisationRepository,
                affaireRepository,
                suspensionRepository,
                oppositionRepository,
                transverseRepository,
                controleRepository,
                observationRepository
            };
            this.referentialRepository = referentialRepository;
            this.primeRepository = primeRepository;
            this.sinistreRepository = sinistreRepository;
            this.userService = userService;
            this.offreRepository = offreRepository;
            this.connexiteRepository = connexiteRepository;
            this.regularisationRepository = regularisationRepository;
            this.offresRepository = offresRepository;
            this.garantieRepository = garantieRepository;
            this.formuleRepository = formuleRepository;
            this.engagementRepository = engagementRepository;
            this.observationRepository = observationRepository;
        }

        public AffaireDto GetAffaire(AffaireId affaireId, bool includeRegul = false) {
            if (!affaireId.IsHisto) {
                affaireId = this.GetCurrentAffaireId(affaireId.CodeAffaire, affaireId.NumeroAliment);
            }
            var affaire = this.affRepo.GetById(affaireId.TypeAffaire, affaireId.CodeAffaire, affaireId.NumeroAliment, affaireId.NumeroAvenant, affaireId.IsHisto);
            if (includeRegul && affaireId.TypeAffaire == AffaireType.Contrat) {
                affaire.Regularisation = this.regularisationRepository.GetByAffaire(affaireId);
            }
            return affaire.Adapt<AffaireDto>();
        }

        public AffaireDto GetAffaire1(AffaireId affaireId) {
            return this.affRepo
                .GetByIdWithoutDependencies(affaireId.CodeAffaire, affaireId.NumeroAliment, affaireId.IsHisto ? affaireId.NumeroAvenant : null)
                .Adapt<AffaireDto>();
        }

        public AffaireDto GetAffaireCanevas(int templateId) {
            var canevas = this.affRepo.GetAffaireCanevas(templateId);
            if (canevas is null) {
                return null;
            }
            return canevas.Adapt<AffaireDto>();
        }

        public AffaireId GetCurrentAffaireId(string codeAffaire, int version) {
            return this.affRepo.GetCurrentId(codeAffaire, version);
        }

        /// <summary>
        /// Builds the Affaire identifier, defining either the IsHisto flag if the avenant is supplied, or the avenant
        /// </summary>
        /// <param name="code"></param>
        /// <param name="version"></param>
        /// <param name="avenant"></param>
        /// <returns></returns>
        public AffaireId GetAffaireId(string code, int version, int? avenant = null) {
            AffaireId id = null;
            if (avenant.GetValueOrDefault(-1) >= 0) {
                id = this.affRepo.GetPriorId(code, version, avenant.Value);
            }
            if (id is null) {
                id = GetCurrentAffaireId(code, version);
            }
            return id;
        }

        /// <summary>
        /// Retrieves all folders (proposal, histo and current) involved with unique key IPB-ALX
        /// </summary>
        /// <param name="codeContrat">IPB code</param>
        /// <param name="version">ALX code</param>
        /// <returns></returns>
        public IEnumerable<AffaireDto> GetFullAffaire(string codeContrat, int version) {
            var folders = new List<AffaireDto>();
            var mainFolder = GetAffaire(new AffaireId { CodeAffaire = codeContrat, NumeroAliment = version, TypeAffaire = AffaireType.Contrat });
            AffaireDto proposal = mainFolder.OffreOrigine is null ? null : GetAffaire(mainFolder.OffreOrigine);
            if (mainFolder.NumeroAvenant > 0) {
                folders.AddRange(this.affRepo.GetFullHisto(codeContrat, version).Select(x => x.Adapt<AffaireDto>()));
            }
            folders.Insert(0, mainFolder);
            folders.Add(proposal);
            return folders;
        }

        public decimal? GetTauxCommission(AffaireId affaireId, decimal? tauxCommDef = null, AffaireDto affaire = null) {
            if (affaire is null) {
                affaire = GetAffaire(affaireId);
            }
            if (affaireId.TypeAffaire == AffaireType.Contrat) {
                return affaire.TauxCommission;
            }
            else {
                var ct = this.offreRepository.GetCotisationOffre(affaireId.CodeAffaire, affaireId.NumeroAliment);
                if (ct.TauxCommission <= decimal.Zero) {
                    return tauxCommDef.Value;
                }
                return ct.TauxCommission;
            }
        }

        public VerrouAffaire TryLockAffaire(AffaireId affaireId, string action, string acteGestion = null) {
            var verrou = GetCurrentLock(affaireId);
            if (verrou != null) {
                return verrou;
            }
            lock (_syncroot) {
                verrou = GetCurrentLock(affaireId);
                if (verrou is null) {
                    if (acteGestion.IsEmptyOrNull()) {
                        acteGestion = GetAffaire(affaireId).TypeTraitement.Code;
                    }
                    this.affRepo.SetLockState(affaireId, new LockState {
                        Action = action,
                        ActeGestion = acteGestion,
                        User = this.context.UserId,
                        IsLocked = true
                    });
                }
                else {
                    return verrou;
                }
            }
            return null;
        }

        public IEnumerable<VerrouAffaire> TryLockAffaireList(IEnumerable<AffaireId> affaireIds, string action, string acteGestion = null) {
            lock (_syncroot) {
                var list = affaireIds.ToList();
                var locks = list.Select(a => GetCurrentLock(a)).Where(x => x != null);
                if (!locks.Any()) {
                    list.ForEach(a => {
                        string acg = acteGestion;
                        if (acg.IsEmptyOrNull()) {
                            acg = GetCurrentAffaireId(a.CodeAffaire, a.NumeroAliment).TypeTraitement;
                        }
                        this.affRepo.SetLockState(
                            a,
                            new LockState {
                                Action = action,
                                ActeGestion = acg,
                                User = this.context.UserId,
                                IsLocked = true
                            });
                    });
                }
                else {
                    return locks.ToList();
                }
            }
            return Enumerable.Empty<VerrouAffaire>();
        }

        public void UnockAffaireList(IEnumerable<AffaireId> affaireIds) {
            affaireIds.ToList().ForEach(a => this.affRepo.SetLockState(a, new LockState(this.context.UserId)));
        }

        /// <summary>
        /// Deverrouille les affaires de l'utilisateur courant
        /// </summary>
        public void UnockAffaires() {
            var locks = this.affRepo.GetUserLocks(this.context.UserId).ToList();
            locks.ForEach(x => {
                var @lock = x.Value;
                @lock.IsLocked = false;
                this.affRepo.SetLockState(x.Key, @lock);
            });
        }

        public VerrouAffaire GetCurrentLock(AffaireId affaireId) {
            var state = this.affRepo.GetLockState(affaireId);
            if (state?.IsLocked == true) {
                return new VerrouAffaire {
                    AffaireId = affaireId,
                    ActeGestion = state.ActeGestion,
                    Action = state.Action,
                    User = state.User,
                    IsLocked = true
                };
            }
            return null;
        }

        public IEnumerable<VerrouAffaire> GetUserLocks() {
            return this.affRepo.GetUserLocks(this.context.UserId).Select(kv => new VerrouAffaire {
                ActeGestion = kv.Value.ActeGestion,
                Action = kv.Value.Action,
                User = kv.Value.User,
                AffaireId = kv.Key
            }).ToList();
        }

        public string GetEnvironementName() {
            return this.config.Environement;
        }

        public NouvelleAffaireDto GetNouvelleAffaire(AffaireId offre, string codeContrat, int versionContrat) {
            var newContrats = GetNewContractsTempSelections(offre);
            return newContrats.First(c => c.CodeContrat == codeContrat && c.VersionContrat == versionContrat).Adapt<NouvelleAffaireDto>();
        }

        public void SetSelectionRisqueNouvelleAffaire(NouvelleAffaireDto dto, int numRisque) {
            var newContracts = GetNewContractsTempSelections(dto.Offre);
            var contract = newContracts.First(c => c.CodeContrat == dto.CodeContrat && c.VersionContrat == dto.VersionContrat);
            var risque = contract.Risques.First(x => x.Numero == numRisque);
            var rsqDto = dto.Risques.First(x => x.Numero == numRisque);
            if (!rsqDto.Objets.Any(o => !o.IsSelected)) {
                risque.IsSelected = true;
                risque.Objets.ForEach(o => o.IsSelected = true);
            }
            else {
                risque.IsSelected = rsqDto.Objets.Any(o => o.IsSelected);
                risque.Objets.ForEach(o => o.IsSelected = rsqDto.Objets.First(x => x.Numero == o.Numero).IsSelected);
            }
            this.affRepo.SaveSelectionRisquesNouvelleAffaire(contract, numRisque);
            EnsureCache(dto.Offre, newContracts);
        }

        public void SetSelectionFormuleNouvelleAffaire(NouvelleAffaireDto dto, int numFormule) {
            var newContracts = GetNewContractsTempSelections(dto.Offre);
            var contract = newContracts.First(c => c.CodeContrat == dto.CodeContrat && c.VersionContrat == dto.VersionContrat);
            var frmDto = dto.Formules.First(x => x.Numero == numFormule);
            var formule = contract.Formules.FirstOrDefault(x => x.Numero == numFormule);
            formule.IsSelected = frmDto.IsSelected;
            formule.SelectedOptionNumber = frmDto.SelectedOptionNumber;
            this.affRepo.SaveSelectionFormulesNouvelleAffaire(contract, numFormule);
            EnsureCache(dto.Offre, newContracts);
        }

        public void SaveSelectionFormulesNouvelleAffaire(AffaireId offre, string codeContrat, int version) {
            var newContracts = GetNewContractsTempSelections(offre);
            var errors = ValidateSelectionFormulesNouvelleAffaire(offre, codeContrat, version, newContracts);
            if (errors.Any()) {
                throw new BusinessValidationException(errors);
            }
        }

        public void ValidateNewAffair(AffaireId offre, string code, int version = 0) {
            SaveSelectionFormulesNouvelleAffaire(offre, code, version);
        }

        public void CancelNewAffairChanges(AffaireId id) {
            this.cache.Invalidate<List<NouvelleAffaire>>(NouvellesAffairesCacheKey + id.AsAffaireKey());
        }

        public PagingList<ImpayeDto> GetImpayes(int page = 0, int codeAssure = 0) {
            var profile = this.userService.GetProfile();
            var pagingList = this.primeRepository.GetImpayes(profile.Branches.Select(x => x.branche).ToArray(), page, codeAssure);
            var impayes = pagingList.List.Select(p => new ImpayeDto {
                Folder = p.affaire.Adapt<AffaireDto>(),
                Prime = p.prime.Adapt<PrimeDto>()
            }).ToList();

            return new PagingList<ImpayeDto> {
                NbTotalLines = pagingList.NbTotalLines,
                PageNumber = pagingList.PageNumber,
                List = impayes,
                Totals = pagingList.Totals
            };
        }

        public PagingList<RetardPaiementDto> GetRetardsPaiement(int page, int codeAssure) {
            var profile = this.userService.GetProfile();
            var pagingList = this.primeRepository.GetRetardsPaiement(profile.Branches.Select(x => x.branche).ToArray(), page, codeAssure);
            var retards = pagingList.List.Select(p => new RetardPaiementDto {
                Folder = p.affaire.Adapt<AffaireDto>(),
                Prime = p.prime.Adapt<PrimeDto>()
            }).ToList();

            return new PagingList<RetardPaiementDto> {
                NbTotalLines = pagingList.NbTotalLines,
                PageNumber = pagingList.PageNumber,
                List = retards,
                Totals = pagingList.Totals
            };
        }

        public PagingList<RelanceDto> GetUserRelances(int page = 0) {
            var profile = this.userService.GetProfile();
            if (ConfigurationManager.AppSettings["EnvironnementDeploiment"]?.ToUpper() == "PRODUCTION"
                && !profile.Utilisateur.Famille.IsIn(FamilleUtilisateur.Interne, FamilleUtilisateur.Sorti)) {
                return null;
            }
            var pgList = this.offreRepository.GetRelancesByGESorSOU(profile.Utilisateur.Id, profile.Utilisateur.Id, page);
            var offres = pgList.List.Select(o => o.Adapt<RelanceDto>()).ToList();
            return new PagingList<RelanceDto> {
                PageNumber = pgList.PageNumber,
                NbTotalLines = pgList.NbTotalLines,
                List = offres
            };
        }

        public (decimal tauxCom, decimal tauxComCATNAT) GetTauxCommissions(AffaireId affaireId) {
            var a = this.affRepo.GetById(affaireId);
            return (a.TauxCommission, a.TauxCommissionCATNAT);
        }

        public ConnexiteDto GetAffaireConnexite(AffaireId affaireId, int numero = 0, int type = 0) {
            return this.connexiteRepository.GetByAffaire(new ConnexiteId { AffaireId = affaireId, Numero = numero, Type = type }).Adapt<ConnexiteDto>();
        }

        public AffaireDto GetSingleRelance(AffaireId affaireId) {
            return this.offreRepository.GetSingleRelance(affaireId).Adapt<AffaireDto>();
        }

        public void ClasserOffresSansSuite(IEnumerable<(AffaireId id, string motif)> affaireIds) {
            if (!affaireIds.Any()) {
                return;
            }
            // ensure ids
            var idList = affaireIds.Select(x => this.affRepo.GetCurrentId(x.id.CodeAffaire, x.id.NumeroAliment)).ToList();
            var affaires = idList.Select(x => x.Adapt<Affaire>()).ToList();
            if (affaires.Any(a => a.TypeAffaire != AffaireType.Offre)) {
                throw new ArgumentException("Action réalisable seulement pour des offres");
            }
            var locks = idList.Select(x => GetCurrentLock(x));
            if ((!locks?.Any() ?? true) || locks.Any(x => x.User != this.context.UserId)) {
                throw new Exception("Les offres ne peuvent pas être mises à jour sans être préalablement verrouillées par l'utilisateur");
            }
            affaires.ForEach(a => {
                var (id, motif) = affaireIds.First(y => y.id.CodeAffaire.Trim() == a.CodeAffaire.Trim() && a.NumeroAliment == y.id.NumeroAliment);
                a.CodeMotifSituation = motif;
                a.Situation = SituationAffaire.SansSuite;
            });
            this.offresRepository.UpdateSituationsRelances(affaires, this.context.UserId);
            //this.offresRepository.UpdateFlagAttenteCourtier(affaires.Select(x => (x.CodeAffaire, x.NumeroAliment, default(bool?))), this.context.UserId);
        }

        public void DiffererRelances(IEnumerable<(AffaireId id, int delaisJours)> affaireIds) {
            if (!affaireIds.Any()) {
                return;
            }
            // ensure ids
            var idList = affaireIds.Select(x => this.affRepo.GetCurrentId(x.id.CodeAffaire, x.id.NumeroAliment)).ToList();
            var affaires = idList.Select(x => x.Adapt<Affaire>()).ToList();
            if (affaires.Any(a => a.TypeAffaire != AffaireType.Offre)) {
                throw new ArgumentException("Action réalisable seulement pour des offres");
            }
            var locks = idList.Select(x => GetCurrentLock(x));
            if ((!locks?.Any() ?? true) || locks.Any(x => x.User != this.context.UserId)) {
                throw new Exception("Les offres ne peuvent pas être mises à jour sans être préalablement verrouillées par l'utilisateur");
            }
            affaires.ForEach(a => {
                var (id, delais) = affaireIds.First(y => y.id.CodeAffaire.Trim() == a.CodeAffaire.Trim() && a.NumeroAliment == y.id.NumeroAliment);
                a.DelaisRelanceJours = delais;
            });
            this.offresRepository.UpdateDelaisRelances(affaires, this.context.UserId);
        }

        public IEnumerable<ConditionGarantieDto> GetGarantiesGareat(AffaireId affaireId) {
            var options = this.formuleRepository.GetOptionsSimple(affaireId);
            foreach (var o in options) {
                if (this.formuleRepository.GetOptionsBlocsByFormule(affaireId, o.Formule.FormuleNumber).Any(b => b.IsChecked && b.ParamModele?.Code == Garantie.CodeGareat)) {
                    var listConditions = this.garantieRepository.GetConditionsGaranties(affaireId, o.OptionNumber, o.Formule.FormuleNumber);
                    foreach (var c in listConditions.Where(x => x.Key.codeGar == Garantie.CodeGareatAttent || this.parRepo.GetGarantie(x.Key.codeGar).IsAttentatGareat)) {
                        yield return new ConditionGarantieDto {
                            AssietteGarantie = c.Value.assiete,
                            CodeGarantie = c.Key.codeGar,
                            IdGarantie = c.Key.idGar,
                            IdOption = o.Id,
                            TarifsGarantie = c.Value.tarif.Adapt<TarifGarantieDto>(),
                            NumeroRisque = o.Applications.First().NumRisque
                        };
                    }
                }
            }
        }

        public void UpdateFlagAttenteCourtier(IEnumerable<(AffaireId id, bool expecting)> flags) {
            this.offresRepository.UpdateFlagAttenteCourtier(flags.Select(x => (x.id.CodeAffaire, x.id.NumeroAliment, new bool?(x.expecting))), this.context.UserId);
        }

        public PrimesGareatDto GetPrimesGareat(AffaireId affaireId, bool isReadonly = false) {
            var affaire = this.affRepo.GetById(affaireId);
            if (!affaire.GareatRetenu?.IsApplique ?? true) {
                return new PrimesGareatDto();
            }
            var primesGareat = new PrimesGareatDto();
            var primes = this.primeRepository.GetPrimesGaranties(affaireId, isReadonly).ToList();
            foreach (var p in primes.Where(x => x.CodeGarantie == Garantie.CodeGareatAttent)) {
                primesGareat.Primes.Add((p.MontantComptable.Adapt<PrimeGarantieGareatDto>(), p.MontantDevise.Adapt<PrimeGarantieGareatDto>()));
            }
            return primesGareat;
        }

        public void SaveConditions(AffaireId affaireId, ConditionRisqueGarantieGetResultDto conditions) {
            var affaire = this.affRepo.GetById(affaireId);
            var franchise = decimal.TryParse(conditions.Franchise, out var d) ? d : default;
            var lci = decimal.TryParse(conditions.LCI, out d) ? d : default;
            affaire.ChangeTarifsConditions(
                new TarifAffaire {
                    Base = new BaseDeCalcul { Code = conditions.TypeLCI },
                    IdExpCpx = long.TryParse(conditions.LienComplexeLCIGenerale, out long x) && x > 0 ? x : default,
                    Indexe = conditions.IsIndexeLCI,
                    Unite = new Unite { Code = conditions.UniteLCI },
                    ValeurActualisee = lci,
                    ValeurOrigine = lci
                },
                new TarifAffaire {
                    Base = new BaseDeCalcul { Code = conditions.TypeFranchise },
                    IdExpCpx = long.TryParse(conditions.LienComplexeFranchiseGenerale, out x) && x > 0 ? x : default,
                    Indexe = conditions.IsIndexeFranchise,
                    Unite = new Unite { Code = conditions.UniteFranchise },
                    ValeurActualisee = franchise,
                    ValeurOrigine = franchise
                },
                conditions.ExpAssiette.ParseCode<TypeMontant>());

            this.affRepo.SaveConditions(affaire, this.context.UserId);
        }

        public void SetGareat(AffaireId affaireId, string trancheMax) {
            string flag = trancheMax.IsEmptyOrNull() ? Booleen.Non.AsCode() : Booleen.Oui.AsCode();
            this.affRepo.SetGareat(affaireId.CodeAffaire, affaireId.NumeroAliment, trancheMax, flag);
        }

        public void ResetGareat(AffaireId affaireId, int numeroRisque) {
            var numFormules = this.formuleRepository.GetNumerosFormules(affaireId, numeroRisque);
            var garanties = this.garantieRepository.GetGarantiesWithTarifs(affaireId)
                .Where(g => {
                    var blocs = this.formuleRepository.GetOptionsBlocsByFormule(affaireId, g.Formule);
                    return blocs.Any(b => b.IsChecked && b.ParamModele?.Code == Garantie.CodeGareat)
                        && numFormules.Contains(g.Formule) && g.CodeGarantie == Garantie.CodeGareatAttent;
                });
            garanties.ToList().ForEach(g => {
                g.Tarif.PrimeValeur.ValeurActualise = decimal.Zero;
                g.Tarif.PrimeValeur.ValeurOrigine = decimal.Zero;
                this.garantieRepository.TryUpdateTarif(g.Tarif);
            });
        }

        public void SaveGareat(AffaireId affaireId, IEnumerable<(long idTarif, decimal valeur)> valeursGareat) {
            var garanties = this.garantieRepository.GetGarantiesWithTarifs(affaireId).Where(g => {
                var blocs = this.formuleRepository.GetOptionsBlocsByFormule(affaireId, g.Formule);
                return blocs.Any(b => b.IsChecked && b.ParamModele?.Code == Garantie.CodeGareat) && g.CodeGarantie == Garantie.CodeGareatAttent;
            });
            garanties.ToList().ForEach(g => {
                if (valeursGareat.Any(x => x.idTarif == g.Tarif.Id)) {
                    g.Tarif.PrimeValeur.ValeurActualise = valeursGareat.First(x => x.idTarif == g.Tarif.Id).valeur;
                    g.Tarif.PrimeValeur.ValeurOrigine = valeursGareat.First(x => x.idTarif == g.Tarif.Id).valeur;
                    this.garantieRepository.TryUpdateTarif(g.Tarif);
                }
            });
        }

        public void SaveEngagementsTraites(AffaireId affaireId, EngagementTraiteDto traiteDto) {
            var errors = new List<ValidationError>();
            var lci = new ValeursOptionsTarif {
                Unite = this.referentialRepository.GetUnite(TypeDeValeur.LCI, traiteDto.Traite.LCIUnite),
                ValeurActualise = (decimal)traiteDto.Traite.LCIValeur,
                ValeurOrigine = (decimal)traiteDto.Traite.LCIValeur,
                ExpressionComplexe = traiteDto.Traite.LienComplexeLCI > 0 ? new ExpressionComplexeBase { Id = traiteDto.Traite.LienComplexeLCI } : null,
                Base = this.referentialRepository.GetBase(TypeDeValeur.LCI, traiteDto.Traite.LCIType)
            };
            if (!lci.IsEmpty && !lci.IsValid) {
                errors.Add(("Valeur_LCIGenerale", "LCI invalide"));
            }
            this.affRepo.UpdateLCI(affaireId.CodeAffaire, affaireId.NumeroAliment, lci, traiteDto.Traite.LCIIndexee);
            if (traiteDto.Traite.TotalSMP != 0) {

              // this.engagementRepository.UpdateTraiteSMPTotal(traiteDto.Traite.IdVentilation, traiteDto.Traite.TotalSMP);
            }
           /* traiteDto.Traite.TraiteInfoRsqVen.TraiteRisques.ForEach(trrsq => {
                foreach (var v in trrsq.TraiteVentilations) {
                    if (!long.TryParse(v.SMP, out long x) || x < 0) {
                        errors.Add(($"smp_{trrsq.CodeRisque}_{v.IdVentilation}", string.Empty));
                    }
                }
               if (!errors.Any()) {
                    this.engagementRepository.UpdateTraiteSMP(
                        affaireId.CodeAffaire, affaireId.NumeroAliment, affaireId.TypeAffaire.AsCode(),
                        trrsq.TraiteVentilations.Select(x => (x.IdVentilation, trrsq.CodeRisque, long.Parse(x.SMP))).ToArray());
                }
            });*/
            if (errors.Any()) {
                throw new BusinessValidationException(errors);
            }
            var engagement = int.TryParse(traiteDto.CodePeriode, out int eng)
                ? this.engagementRepository.GetEngagement(eng)
                : this.engagementRepository.GetEngagement(affaireId.CodeAffaire, affaireId.NumeroAliment, affaireId.TypeAffaire.AsCode());

            engagement.Observation.Texte = traiteDto.Traite.CommentForce;
            this.observationRepository.AddOrUpdate(engagement.Observation);
        }

        private static IEnumerable<ValidationError> ValidateSelectionRisquesNouvelleAffaire(AffaireId offre, string codeContrat, int version, IEnumerable<NouvelleAffaire> newContracts) {
            NouvelleAffaire contract = newContracts.First(c => c.CodeContrat == codeContrat && c.VersionContrat == version);
            if (contract == null) {
                throw new ArgumentException("Nouvelle affaire introuvable", nameof(codeContrat));
            }
            var errors = new List<ValidationError>();
            if (!contract?.Risques.Any(f => f.IsSelected) ?? true) {
                errors.Add(new ValidationError("contrat", codeContrat, offre.CodeAffaire, "Au moins un risque doit être sélectionné"));
            }
            return errors;
        }

        private static IEnumerable<ValidationError> ValidateSelectionFormulesNouvelleAffaire(AffaireId offre, string codeContrat, int version, IEnumerable<NouvelleAffaire> newContracts) {
            NouvelleAffaire contract = newContracts.First(c => c.CodeContrat == codeContrat && c.VersionContrat == version);
            if (contract == null) {
                throw new ArgumentException("Nouvelle affaire introuvable", nameof(codeContrat));
            }
            var errors = new List<ValidationError>();
            if (!contract?.Formules.Any(f => f.IsSelected) ?? true) {
                errors.Add(new ValidationError("contrat", codeContrat, offre.CodeAffaire, "Au moins une formule doit être sélectionnée"));
            }
            var noOptFormules = contract.Formules.Where(f => f.IsSelected && f.SelectedOptionNumber.GetValueOrDefault() < 1);
            errors.AddRange(noOptFormules.Select(f => new ValidationError("formule", f.Numero.ToString(), "", "Au moins une option doit être sélectionnée")));
            return errors;
        }

        private IEnumerable<NouvelleAffaire> GetNewContractsTempSelections(AffaireId offerId) {
            var newContracts = this.cache.Get<List<NouvelleAffaire>>(NouvellesAffairesCacheKey + offerId.AsAffaireKey()).Adapt<List<NouvelleAffaire>>();
            if (newContracts == null) {
                newContracts = GetNewContractsTempSelectionsFromDb(offerId).ToList();
                EnsureCache(offerId, newContracts);
            }
            return newContracts;
        }

        private IEnumerable<NouvelleAffaire> GetNewContractsTempSelectionsFromDb(AffaireId offerId) {
            return this.affRepo.GetTempAffairs(offerId);
        }

        public void RepriseAvenant(AffaireId affaireId) {
            var folder = CheckRollback(ref affaireId);
            affaireId.TypeTraitement = this.affRepo.GetAffaireTraitement(affaireId);
            int idAdresse = folder.IdAdresse;
            this.adresseRepository.Reprise(affaireId, idAdresse);
            foreach (var irep in this.repriseRepositories) {
                try {
                    irep.Reprise(affaireId);
                }
                catch (Exception ex) {
                    System.Diagnostics.Trace.WriteLine(ex);
                    throw;
                }
            }
        }

        private Affaire CheckRollback(ref AffaireId id) {
            var errors = new List<ValidationError>();
            var currentId = GetCurrentAffaireId(id.CodeAffaire, id.NumeroAliment);
            if (currentId is null) {
                errors.Add(new ValidationError(string.Empty, string.Empty, string.Empty, "Contrat inconnu"));
            }
            if (currentId.NumeroAvenant.GetValueOrDefault() < 1) {
                errors.Add(new ValidationError(string.Empty, nameof(id.NumeroAvenant), string.Empty, "La reprise ne peut être effectuée que sur avenant"));
            }
            var affaire = this.affRepo.GetById(currentId);
            if (affaire.TypeAffaire != AffaireType.Contrat) {
                errors.Add(new ValidationError(string.Empty, nameof(affaire.TypeAffaire), string.Empty, "Le dossier sélectionné n'est pas un contrat"));
            }
            if (affaire.Etat != EtatAffaire.NonValidable) {
                errors.Add(new ValidationError(string.Empty, nameof(affaire.Etat), string.Empty, "L'état du contrat ne convient pas"));
            }
            if (affaire.TypeTraitement.Type.IsIn(TraitementAffaire.Inconnu, TraitementAffaire.Offre, TraitementAffaire.Resiliation, TraitementAffaire.Attestation)) {
                errors.Add(new ValidationError(string.Empty, nameof(affaire.Etat), string.Empty, "Le type de traitement du contrat ne convient pas"));
            }

            if (errors.Any()) {
                throw new BusinessValidationException(errors);
            }
            id = currentId;
            return affaire;
        }

        private void EnsureCache(AffaireId offerId, IEnumerable<NouvelleAffaire> newContracts) {
            this.cache.Set(NouvellesAffairesCacheKey + offerId.AsAffaireKey(), newContracts);
        }
    }
}
