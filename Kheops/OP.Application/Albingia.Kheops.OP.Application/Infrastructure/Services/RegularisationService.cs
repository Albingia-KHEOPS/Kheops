using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Kheops.OP.Domain.Regularisation;
using ALBINGIA.Framework.Common.Business;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using WSDTOFrm = OP.WSAS400.DTO.FormuleGarantie;
using WSDTORegul = OP.WSAS400.DTO.Regularisation;
using Albingia.Kheops.OP.Application.Contracts;

namespace Albingia.Kheops.OP.Application.Infrastructure.Services {
    public class RegularisationService : IRegularisationPort {
        private readonly IRegularisationRepository regularisationRepository;
        private readonly IGarantieRepository garantieRepository;
        private readonly IFormuleRepository formuleRepository;
        private readonly IReferentialRepository referentialRepository;
        private readonly IAffaireRepository affaireRepository;
        private readonly ISessionContext sessionContext;
        private readonly ITraceRepository traceRepository;
        public RegularisationService(IRegularisationRepository regularisationRepository, IGarantieRepository garantieRepository, IFormuleRepository formuleRepository, IReferentialRepository referentialRepository, IAffaireRepository affaireRepository, ISessionContext sessionContext, ITraceRepository traceRepository) {
            this.regularisationRepository = regularisationRepository;
            this.garantieRepository = garantieRepository;
            this.formuleRepository = formuleRepository;
            this.referentialRepository = referentialRepository;
            this.affaireRepository = affaireRepository;
            this.sessionContext = sessionContext;
            this.traceRepository = traceRepository;
        }
        public RegularisationDto GetDto(long id) {
            return this.regularisationRepository.GetById(id).Adapt<RegularisationDto>();
        }
        public RegularisationDto GetDtoByAffaire(AffaireId affaireId) {
            return this.regularisationRepository.GetByAffaire(affaireId).Adapt<RegularisationDto>();
        }
        public RegularisationDto GetBNSDtoByAffaire(AffaireId affaireId)
        {
            return this.regularisationRepository.GetBNSByAffaire(affaireId).Adapt<RegularisationDto>();
        }
        public long ResetByLot(long idLot) {
            if (idLot > 0) {
                this.regularisationRepository.ResetByLot(idLot);
            }
            else {
                idLot = this.regularisationRepository.InitLot();
            }
            return idLot;
        }

        public void AddSelectionRisque(SelectionRisqueRegularisationDto selectionRsq) {
            this.regularisationRepository.AddSelection(selectionRsq.Adapt<SelectionRegularisation>());
        }

        public void AddSelectionObjet(SelectionObjetRegularisationDto selectionObj) {
            this.regularisationRepository.AddSelection(selectionObj.Adapt<SelectionRegularisation>());
        }

        public void AddSelectionFormule(SelectionFormuleRegularisationDto selectionFrm) {
            AddSelection(selectionFrm);
        }

        public void AddSelection(SelectionRegularisationDto selection) {
            this.regularisationRepository.AddSelection(selection);
        }

        public (DateTime debut, DateTime fin) GetPeriodBounds(AffaireId affaireId, long reguleId) {
            var regul = this.regularisationRepository.GetById(reguleId);
            if (regul is null) {
                return default;
            }
            return (regul.DateDebut, regul.DateFin);
        }

        public IEnumerable<GarantieDto> GetGaranties(string codeAffaire, int version, DateTime debut, DateTime fin, bool fullHisto = true) {
            var garanties = fullHisto ? this.formuleRepository.GetGarantiesFullHisto(codeAffaire, version)
                : this.formuleRepository.GetCurrentGaranties(codeAffaire, version);

            return garanties.Where(x => x.NatureRetenue.IsIn(NatureValue.Accordee, NatureValue.Comprise)
                && x.ParamGarantie.Niveau == 1
                && x.ParamGarantie.ParamGarantie.IsRegularisable.GetValueOrDefault()
                && (x.DateDebut is null || x.DateDebut <= fin)
                && (x.DateSortieEffective is null || x.DateSortieEffective >= debut)).Select(x => x.Adapt<GarantieDto>());
        }

        public bool CanSimplifyStandard(long idLot) {
            var selections = this.regularisationRepository.GetSelection(idLot);
            var numsRisques = selections.Select(x => x.NumeroRisque).Distinct();
            var selectionsGar = selections.Select(x => x.Adapt<SelectionRegularisationDto>()).Where(x => x.Perimetre == PerimetreSelectionRegul.Garantie);
            var allRc = selectionsGar.Any(x => x.CodeGarantie == AlbOpConstants.RCFrance) && selectionsGar.All(x => x.IsRCGarantie);
            return numsRisques.Count() == 1 && (selectionsGar.Count() == 1 || allRc);
        }

        /// <summary>
        /// Delete risques+formules without garantie. 
        /// </summary>
        public void CleanUpSelections(long idLot) {
            var selections = this.regularisationRepository.GetSelection(idLot);
            var selRsq = selections.Where(x => x.Perimetre == PerimetreSelectionRegul.Risque).ToList();
            var selObj = selections.Where(x => x.Perimetre == PerimetreSelectionRegul.Objet).ToList();
            var selFrm = selections.Where(x => x.Perimetre == PerimetreSelectionRegul.Formule).ToList();
            var selGar = selections.Where(x => x.Perimetre == PerimetreSelectionRegul.Garantie).ToList();

            var numsRisquesToKeep = selGar.Select(x => x.NumeroRisque).Distinct().ToArray();
            var rsqObjList = selGar.Where(x => x.NumeroObjet > 0).Select(x => (x.NumeroRisque, x.NumeroObjet)).Distinct();

            var numsFormules = selFrm.Where(x => !x.NumeroRisque.IsIn(numsRisquesToKeep)).Select(x => x.NumeroFormule).Distinct();
            var groupsObjets = selObj.Where(x =>
                /*rsqObjList.Any() && !rsqObjList.Contains((x.NumeroRisque, x.NumeroObjet))
                ||*/ !x.NumeroRisque.IsIn(numsRisquesToKeep)).GroupBy(x => x.NumeroRisque);
            var numsRisques = selRsq.Where(x => !x.NumeroRisque.IsIn(numsRisquesToKeep)).Select(x => x.NumeroRisque).Distinct();

            this.regularisationRepository.DeleteSelectionsFormules(idLot, numsFormules);
            foreach (var g in groupsObjets) {
                this.regularisationRepository.DeleteSelectionsObjets(idLot, g.Key, g.Select(x => x.NumeroObjet).ToList());
            }
            this.regularisationRepository.DeleteSelectionsRisques(idLot, numsRisques);
        }

        public IEnumerable<SelectionRegularisationDto> GetSelection(long idLot) {
            return this.regularisationRepository.GetSelection(idLot).Select(x => x.Adapt<SelectionRegularisationDto>());
        }

        public IEnumerable<PeriodeGarantieDto> GetPeriodesGaranties(long rgId) {
            return this.regularisationRepository.GetPeriodesGaranties(rgId).Select(x => x.Adapt<PeriodeGarantieDto>());
        }

        public IEnumerable<SelectionRegularisationDto> GetSelectionGaranties(long idLot) {
            return this.regularisationRepository.GetSelectionGaranties(idLot).Select(x => x.Adapt<SelectionRegularisationDto>());
        }

        public bool HasMultiRCSelection(long idLot, long garId = 0) {
            var selectionGaranties = this.regularisationRepository.GetSelectionGaranties(idLot).Select(x => x.Adapt<SelectionRegularisationDto>());
            if (garId > 0) {
                int? formule = null;
                formule = selectionGaranties.FirstOrDefault(x => x.IdGarantie == garId)?.NumeroFormule;
                selectionGaranties = selectionGaranties.Where(x => !formule.HasValue || x.NumeroFormule == formule);
                return selectionGaranties.Any(x => x.CodeGarantie == AlbOpConstants.RCFrance)
                    && selectionGaranties.Count(x => x.IsRCGarantie) > 1;
            }
            return selectionGaranties.GroupBy(x => x.NumeroFormule).Any(g => {
                var rcList = g.Where(x => x.IsRCGarantie);
                return rcList.Any(x => x.CodeGarantie == AlbOpConstants.RCFrance) && rcList.Count() > 1;
            });
        }

        public int PurgeTempPeriodes(AffaireId affaireId) {
            return this.regularisationRepository.PurgeTempPeriodes(affaireId.CodeAffaire, affaireId.NumeroAliment);
        }

        public (DateTime min, DateTime max) GetMouvementsMinMax(AffaireId affaireId, long rgId, int numRisque, int numFormule, AccessMode mode) {
            var currentAffaire = this.affaireRepository.GetById(AffaireType.Contrat, affaireId.CodeAffaire, affaireId.NumeroAliment);
            affaireId.IsHisto = affaireId.NumeroAvenant < currentAffaire.NumeroAvenant;
            var allMouvements = this.regularisationRepository.GetMouvementsGaranties(affaireId, rgId, numRisque, numFormule, mode);
            return (allMouvements.Min(x => x.DateDebut), allMouvements.Max(x => x.DateFin));
        }

        public RegularisationDto AddOrUpdateWithRsq(WSDTORegul.RegularisationContext regularisationContext) {
            var dto = AddOrUpdate(new RegularisationDto {
                Id = regularisationContext.RgId,
                AffaireId = regularisationContext.IdContrat.Adapt<AffaireId>(),
                CodeApplicationFraisAcc = null,
                MontantFraisAcc = 0,
                CodeEtat = "N",
                DateDebut = DateTime.Parse(regularisationContext.DateDebut),
                DateFin = DateTime.Parse(regularisationContext.DateFin),
                Encaissement = new Domain.Referentiel.Quittancement { Code = regularisationContext.CodeEnc },
                Exercice = regularisationContext.Exercice,
                IdCourtier = regularisationContext.CodeICT.ParseInt().Value,
                IdCourtierCommission = regularisationContext.CodeICC.ParseInt().Value,
                Mode = new Domain.Referentiel.ModeRegularisation { Code = regularisationContext.Mode.AsCode() },
                Motif = new Domain.Referentiel.MotifRegularisation { Code = regularisationContext.ModeleAvtRegul.MotifAvt },
                Niveau = regularisationContext.Scope.AsCode().ParseCode<NiveauRegularisation>(),
                SuivieAvenant = regularisationContext.TypeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF,
                TauxCommission = decimal.TryParse(regularisationContext.TauxCom, out var tx) ? tx : 0M,
                TauxCommissionCATNAT = decimal.TryParse(regularisationContext.TauxComCATNAT, out var txcn) ? txcn : 0M,
                IsAvenant = true
            }, regularisationContext.LotId);
            if (!this.regularisationRepository.GetRegulRisques(dto.Adapt<Regularisation>()).Any()) {
                this.regularisationRepository.GetSelection(regularisationContext.LotId)
                    .Where(x => x.Perimetre == PerimetreSelectionRegul.Risque)
                    .ToList().ForEach(s => this.regularisationRepository.CreateRegulRisque(new RegulRisque { AffaireId = dto.AffaireId, IdRegul = dto.Id, Numero = s.NumeroRisque }));
            }
            this.traceRepository.TraceInfo(new Domain.TraceContext {
                AffaireId = dto.AffaireId,
                IsInfoSuspension = false,
                Libelle = string.Empty,
                Ordre = 1,
                TypeAction = "G",
                TypeTraitement = dto.SuivieAvenant ? AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF : AlbConstantesMetiers.TYPE_AVENANT_REGUL,
                User = this.sessionContext.UserId
            });
            return dto;
        }

        public RegularisationDto AddOrUpdate(RegularisationDto regularisationDto, long lotId) {
            Regularisation rg = null;
            var a = this.affaireRepository.GetById(regularisationDto.AffaireId);
            bool newRg = regularisationDto.Id <= 0;
            regularisationDto.AffaireId = a.Adapt<AffaireId>();
            regularisationDto.DateAvenant = a.DateEffetAvenant.Value;
            if (newRg) {
                rg = new Regularisation();
            }
            else {
                rg = this.regularisationRepository.GetById(regularisationDto.Id);
                if (regularisationDto.CodeApplicationFraisAcc is null && regularisationDto.MontantFraisAcc == 0) {
                    regularisationDto.CodeApplicationFraisAcc = rg.CodeApplicationFraisAcc;
                    regularisationDto.MontantFraisAcc = rg.MontantFraisAcc;
                }
            }
            if (newRg || rg.Motif.Code != regularisationDto.Motif.Code) {
                if (regularisationDto.Motif.Code == Motif.PrimeInferieure.AsCode()) {
                    regularisationDto.CodeApplicationFraisAcc = "N";
                    regularisationDto.MontantFraisAcc = 0;
                }
                else {
                    var categorisation = this.referentialRepository.GetCategory(a.Branche.Code, a.SousBranche, a.Categorie);
                    regularisationDto.CodeApplicationFraisAcc = a.ApplicationFraisAccessoire.Code;
                    regularisationDto.MontantFraisAcc = a.ApplicationFraisAccessoire.Code == "S" ? categorisation.MontantFraisAccessoires : a.FraisAccessoires;
                }
                if (!newRg && (regularisationDto.Motif.Code == Motif.PrimeInferieure.AsCode() || rg.Motif.Code == Motif.PrimeInferieure.AsCode()))
                {
                    this.regularisationRepository.ResetRsqGar(regularisationDto.Id);
                }
            }

            if (newRg) {
                regularisationDto.Adapt(rg);
                this.regularisationRepository.Create(rg, this.sessionContext.UserId);
                regularisationDto.Id = rg.Id;
            }
            else {
                UpdateFromDto(regularisationDto, rg);
                this.regularisationRepository.Update(rg, this.sessionContext.UserId);
            }
            return regularisationDto;
        }

        public WSDTORegul.LigneRegularisationGarantieDto GetDetailsInfoGarantie(long id, long idLot, long idRegul) {
            return this.regularisationRepository.GetDetailsInfoGarantie(id, idLot, idRegul).FirstOrDefault();
        }

        public IEnumerable<WSDTORegul.LigneMouvementDto> GetMouvementsGarantie(AffaireId affaireId, long rgId, int numRisque, int numFormule, string codeGarantie) {
            var currentAffaire = this.affaireRepository.GetById(AffaireType.Contrat, affaireId.CodeAffaire, affaireId.NumeroAliment);
            affaireId.IsHisto = affaireId.NumeroAvenant < currentAffaire.NumeroAvenant;
            return this.regularisationRepository.GetMouvementsGarantie(affaireId, rgId, numRisque, numFormule, codeGarantie).Select(x => x.Adapt<WSDTORegul.LigneMouvementDto>());
        }

        public void EnsureTempPeriodes(AffaireId affaireId, long rgId, int risque, int formule, long idGarantie, string codeGarantie, AccessMode mode) {
            var currentId = this.affaireRepository.GetCurrentId(affaireId.CodeAffaire, affaireId.NumeroAliment);
            var affaireHistoList = this.affaireRepository.GetFullHisto(affaireId.CodeAffaire, affaireId.NumeroAliment)
                .Concat(new[] { this.affaireRepository.GetById(currentId) });
            // pas de controle si la périodicité a toujours été différente de R
            if (!affaireHistoList.Any(x => x.Periodicite.Code == Periodicite.Regularisation.Code)) {
                return;
            }

            var rg = this.regularisationRepository.GetById(rgId);
            if (rg is null) {
                return;
            }
            var tempPeriodes = this.regularisationRepository.GetMouvementsGaranties(currentId, rgId, risque, formule, mode).Where(m => m.IdGarantie == idGarantie);
            if (tempPeriodes.Any()) {
                return;
            }
            var prmg = this.garantieRepository.GetParamGarantie(codeGarantie);
            this.regularisationRepository.InsertBlankTempPeriode(affaireId, risque, formule, idGarantie, codeGarantie, rg.DateDebut, rg.DateFin, prmg.GrilleRegul?.Code ?? string.Empty);
        }

        public void EnsureTempPeriodesRC(AffaireId affaireId, long rgId, int risque, int formule, long[] idsRC, AccessMode mode) {
            var currentId = this.affaireRepository.GetCurrentId(affaireId.CodeAffaire, affaireId.NumeroAliment);
            var affaireHistoList = this.affaireRepository.GetFullHisto(affaireId.CodeAffaire, affaireId.NumeroAliment)
                .Concat(new[] { this.affaireRepository.GetById(currentId) });
            // pas de controle si la périodicité a toujours été différente de R
            if (!affaireHistoList.Any(x => x.Periodicite.Code == Periodicite.Regularisation.Code)) {
                return;
            }

            var rg = this.regularisationRepository.GetById(rgId);
            if (rg is null) {
                return;
            }
            var tempPeriodes = this.regularisationRepository.GetMouvementsGaranties(currentId, rgId, risque, formule, mode).Where(m => AlbOpConstants.AllRCGar.Contains(m.CodeGarantie));
            if (tempPeriodes.Any()) {
                return;
            }
            var rcGarList = GetGaranties(affaireId.CodeAffaire, affaireId.NumeroAliment, rg.DateDebut, rg.DateFin).Where(g => idsRC.Contains(g.Id));
            rcGarList.GroupBy(x => x.CodeGarantie).Select(x => x.First()).ToList().ForEach(g => {
                var prmg = this.garantieRepository.GetParamGarantie(g.CodeGarantie);
                this.regularisationRepository.InsertBlankTempPeriode(affaireId, risque, formule, g.Id, g.CodeGarantie, rg.DateDebut, rg.DateFin, prmg.GrilleRegul?.Code ?? string.Empty);
            });
        }

        public void EnsureInsertMouvements(WSDTORegul.RegularisationContext context) {
            if (context.HasMultiRC) {
                this.regularisationRepository.EnsureInsertMouvementsRC(context.IdContrat.CodeOffre, context.IdContrat.Version, (int)context.RsqId, context.CodeFormule, context.RgId, context.LotId);
            }
            else {
                this.regularisationRepository.EnsureInsertMouvements(context.IdContrat.CodeOffre, context.IdContrat.Version, (int)context.RsqId, context.CodeFormule, context.GrId, context.RgId);
            }
        }

        private static Regularisation UpdateFromDto(RegularisationDto dto, Regularisation regularisation) {
            if (regularisation is null) {
                return dto.Adapt(new Regularisation());
            }
            regularisation.CodeApplicationFraisAcc = dto.CodeApplicationFraisAcc;
            regularisation.MontantFraisAcc = dto.MontantFraisAcc;
            regularisation.CodeEtat = dto.CodeEtat;
            regularisation.DateDebut = dto.DateDebut;
            regularisation.DateFin = dto.DateFin;
            regularisation.Encaissement = dto.Encaissement;
            regularisation.Exercice = dto.Exercice;
            regularisation.IdCourtier = dto.IdCourtier;
            regularisation.IdCourtierCommission = dto.IdCourtierCommission;
            regularisation.Mode = dto.Mode;
            regularisation.Motif = dto.Motif;
            regularisation.Niveau = dto.Niveau;
            regularisation.SuivieAvenant = dto.SuivieAvenant;
            regularisation.TauxCommission = dto.TauxCommission;
            regularisation.TauxCommissionCATNAT = dto.TauxCommissionCATNAT;
            return regularisation;
        }
    }
}
