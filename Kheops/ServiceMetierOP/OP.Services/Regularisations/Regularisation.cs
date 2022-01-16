using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Regularisation;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Business;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using Mapster;
using OP.DataAccess;
using OP.Services.BLServices;
using OP.Services.BLServices.Regularisations;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.FormuleGarantie;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.PGM;
using OP.WSAS400.DTO.Regularisation;
using OPServiceContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Activation;
using ALBINGIA.Framework.Common.AlbingiaExceptions;

namespace OP.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Regularisation : IRegularisation
    {
        private readonly RegularisationManager regularisationManager;
        private readonly IRegularisationPort regularisationService;
        private readonly IGarantiePort garantieService;
        private readonly IRisquePort risqueService;
        private readonly IFormulePort formuleService;
        private readonly IReferentielPort referencielService;
        private readonly IAffairePort affaireService;
        private readonly ProgramAS400Repository as400Repository;
        private readonly RegularisationRepository repository;

        public Regularisation(RegularisationManager manager, RegularisationRepository repository, IGarantiePort garantieService, IFormulePort formuleService, IRegularisationPort regularisationService, IAffairePort affaireService, IReferentielPort referencielService, ProgramAS400Repository as400Repository, IRisquePort risqueService)
        {
            this.regularisationManager = manager;
            this.regularisationService = regularisationService;
            this.affaireService = affaireService;
            this.referencielService = referencielService;
            this.as400Repository = as400Repository;
            this.repository = repository;
            this.risqueService = risqueService;
            this.formuleService = formuleService;
            this.garantieService = garantieService;
        }

        public RegularisationInfoDto Init(Folder folder, RegularisationContext context, CauseResetRegularisation? causeReset = null)
        {
            var fullAffaire = this.affaireService.GetFullAffaire(folder.CodeOffre, folder.Version).Where(x => x != null && x.TypeAffaire != AffaireType.Offre);
            var currentAffaire = fullAffaire.First();
            if (currentAffaire.NumeroAvenant == 0 && currentAffaire.TypeAccord.Code.IsEmptyOrNull())
            {
                throw new BusinessValidationException(new ValidationError("Aucun accord sur l'affaire nouvelle"));
            }

            var result = new RegularisationInfoDto
            {
                TypeAvt = context.TypeAvt ?? currentAffaire.TypeTraitement.Code,
                LibelleAvt = currentAffaire.TypeTraitement.LibelleLong,
                NumInterneAvt = currentAffaire.NumeroAvenant,
                NumAvt = currentAffaire.NumeroAvenant,
                DescriptionAvt = currentAffaire.DesignationAvenant,
                ModeAvt = context.AccessMode.ToString(),
                LotId = context.LotId,
                Exercice = context.Exercice,
                PeriodeDebInt = (DateTime.TryParse(context.DateDebut, out var dtd) ? dtd : default(DateTime?)).ToIntYMD(),
                PeriodeFinInt = (DateTime.TryParse(context.DateFin, out var dtf) ? dtf : default(DateTime?)).ToIntYMD(),
                Quittancements = this.referencielService.GetQuittancements().Select(x => x.Adapt<ParametreDto>()).ToList(),
                Motifs = this.referencielService.GetMotifsRegularisation().Select(x => x.Adapt<ParametreDto>()).ToList(),
                IsHisto = false
            };

            if (context.AccessMode == AccessMode.CREATE && !context.Mode.IsIn(RegularisationMode.BNS, RegularisationMode.Burner))
            {
                result.NumInterneAvt++;
                result.NumAvt++;
            }

            // get avn regule
            InitInfos(folder, currentAffaire, result, context, causeReset, out string error);

            if (error.IsEmptyOrNull())
            {
                // fill SELW
                InitBatchData(result, context, causeReset, fullAffaire);
            }

            return result;
        }

        public void CancelRegularisationRisque(long rgId, int rsqId)
        {
            RegularisationRepository.SetRisqueStatus(rgId, rsqId, false);
            RegularisationRepository.SetRisqueGarantiesStatus(rgId, rsqId, false);
            RegularisationRepository.EraseFigures(rgId, rsqId);
            RegularisationRepository.ReportAmount_KPGRGU_KPRGUR(rgId);
            RegularisationRepository.SpreadSumCotisation(rgId, rsqId);
            RegularisationRepository.SwitchScopeRisque(rgId);
        }

        public void ReportDataRegulFromRsqToEnt(string codeContrat, string version, string codeAvn)
        {
            RegularisationManager.ReportDataRegulFromRsqToEnt(codeContrat, version, codeAvn);
        }

        public LigneRegularisationDto GetLastRegularisation(string codeContrat, string version, string type, string codeAvn)
        {
            return RegularisationManager.GetLastRegularisation(codeContrat, version, type, codeAvn);
        }

        public LigneRegularisationDto GetRegularisationByID(string codeContrat, string version, string type, long rgId)
        {
            return RegularisationManager.GetRegularisationByID(codeContrat, version, type, rgId);
        }

        public List<SaisieRisqueInfoDto> GetListSaisieRisqueRegulatisation(RegularisationContext context)
        {
            var list = RegularisationRepository.GetListSaisieRisqueRegulatisation(context.RgId, context.Mode);
            list.ForEach(it =>
            {
                if (it.DateDebutRsq.Equals("0/0/0") || it.DateDebutRsq.Trim().Equals(""))
                    it.DateDebutRsq = string.Empty;
                else
                    it.DateDebutRsq = AlbConvert.ConvertStrToDate(it.DateDebutRsq).Value.ToString("dd/MM/yyyy");


                if (it.DateFinRsq.Equals("0/0/0") || it.DateFinRsq.Trim().Equals(""))
                    it.DateFinRsq = string.Empty;
                else
                    it.DateFinRsq = AlbConvert.ConvertStrToDate(it.DateFinRsq).Value.ToString("dd/MM/yyyy");
            });
            return list;
        }

        public RegularisationContext GetPreviousStep(RegularisationContext context)
        {
            context.Step = StepManagerRegularisation.PreviousStep(context, regularisationManager);
            PerformBeforePreviousStep(context);
            return context.Refresh();
        }

        public RegularisationContext ValidateStepAndGetNext(RegularisationContext context, bool cancelMode = false)
        {
            this.regularisationManager.InitMatrix(context);
            ValidateAndSelectStep(context, StepManagerRegularisation.NextAvailableSteps(context), cancelMode);
            PerformAfterSelectStep(context);

            return context.Refresh();
        }

        public RegularisationContext ValidateStepAndRichOther(RegularisationContext context, RegularisationStep stepToRich)
        {
            RegularisationContext c = TryPreviousOrNext(context, stepToRich);
            if (c != null)
            {
                return c;
            }

            this.regularisationManager.InitMatrix(context);
            var result = ValidateBeforeRichingStep(context, stepToRich);
            if (result.Step != stepToRich && context.Error == null)
            {
                context.Error = new AlbErrorDto
                {
                    Code = "ERROR",
                    Label = "Impossible d'atteindre l'étape suivante" + Environment.NewLine + result.Message
                };
            }

            PerformAfterSelectStep(context);

            return context.Refresh();
        }

        public RegularisationComputeData GetInfosRegularisationContratTR(long rgId, long numAvt)
        {
            var toReturn = RegularisationRepository.GetInfosRegularisationContratTR(rgId, numAvt);

            toReturn = InitializeComputedDataForCalculationPBTR(toReturn);

            return toReturn;
        }

        public RegularisationComputeData GetInfosRegularisationRisqueTR(long rgId, long rsqId)
        {
            var toReturn = RegularisationRepository.GetInfosRegularisationRisqueTR(rgId, rsqId);

            toReturn = InitializeComputedDataForCalculationPBTR(toReturn);

            return toReturn;
        }

        public string GetMotifRegularisation(long reguleId)
        {
            return RegularisationRepository.GetMotifRegularisation(reguleId);
        }

        public static RegularisationStep GetStepSimplifiedRegule(RegularisationContext context, RegularisationManager regularisationManager)
        {
            var (canSimplifyStepFlow, nbPeriodes) = regularisationManager.CanSimplifyStepFlow(context);
            if (canSimplifyStepFlow)
            {
                if (nbPeriodes == 0 || nbPeriodes == 1)
                {
                    // must initalize periods to determine next step
                    regularisationManager.InitializeSimplifiedStepFlow(context);
                    if (context.IsSimplifiedRegule)
                    {
                        return RegularisationStep.Regularisation;
                    }
                }

                return RegularisationStep.ChoixPeriodesGarantie;
            }
            return RegularisationStep.ChoixRisques;
        }

        public RegularisationComputeData GetInfosRegularisationContrat(RegularisationMode mode, long regularisationId, long numAvt)
        {
            var data = RegularisationRepository.GetInfosRegularisationContrat(regularisationId, numAvt);

            data.Refresh();

            switch (mode)
            {
                case RegularisationMode.Burner:
                    InitializeComputedDataForCalculationBurner(data);
                    break;

                case RegularisationMode.PB:
                    InitializeComputedDataForCalculationPB(data);
                    break;
            }
            return data;
        }

        public RegularisationComputeData GetInfosRegularisationRisque(RegularisationMode mode, long regularisationId, long codeRsq, long numAvt)
        {
            var data = RegularisationRepository.GetInfosRegularisationRisque(regularisationId, codeRsq, numAvt);

            data.Refresh();
            switch (mode)
            {
                case RegularisationMode.Burner:
                    InitializeComputedDataForCalculationBurner(data);
                    break;

                case RegularisationMode.PB:
                    InitializeComputedDataForCalculationPB(data);
                    break;
            }

            return data;
        }

        public string ComputeRegularisation(RegularisationContext context, RegularisationComputeData figures)
        {
            long idRsq = 0;
            long idRsqRegul = 0;

            if (context.Scope == RegularisationScope.Contrat)
            {
                RegularisationRepository.PrepareComputeContrat(context.RgId, figures);
            }
            else if (context.Scope == RegularisationScope.Risque)
            {
                idRsq = context.RsqId;
                if (figures.ReguleMode == RegularisationMode.Burner)
                    figures.Refresh();
                else if (figures.ReguleMode == RegularisationMode.PB)
                    figures.UpdateLabelMontant();
                RegularisationRepository.PrepareComputeRisque(context.RgId, context.RsqId, figures);
                RegularisationRepository.SpreadSumCotisation(context.RgId, (int)context.RsqId);
                idRsqRegul = RegularisationRepository.GetIdRsqRegularisation(context.RgId, (int)context.RsqId);
            }

            string result = CommonRepository.ComputeRegularisation(new ComputeRegularisationParams
            {
                Acces = context.Scope.AsCode(),
                Risque = idRsq,
                Garantie = string.Empty,
                CodeOffre = context.IdContrat.CodeOffre,
                Formule = 0,
                Id = context.RgId,
                ContextId = context.Scope == RegularisationScope.Contrat ? context.RgId : idRsqRegul,
                Mode = context.Mode.AsCode(),
                Top = "X",
                TypeContrat = context.IdContrat.Type,
                VersionContrat = context.IdContrat.Version
            });

            context.ValidationDone = !result.StartsWith("ERR");

            return result;
        }

        public string ComputeRegularisationTR(RegularisationContext context, RegularisationComputeData figures)
        {
            long idRsq = 0;
            long idRsqRegul = 0;

            //Inscrit en base la valeur de Report de charge nouvelle
            RegularisationRepository.SetReportCharge(context, figures);

            if (context.Scope == RegularisationScope.Contrat)
            {
                RegularisationRepository.PrepareComputeContratTR(context.RgId, figures);
            }
            else if (context.Scope == RegularisationScope.Risque)
            {
                idRsq = context.RsqId;
                RegularisationRepository.PrepareComputeRisqueTR(context.RgId, context.RsqId, figures);
                RegularisationRepository.SpreadSumCotisation(context.RgId, (int)context.RsqId);
                idRsqRegul = RegularisationRepository.GetIdRsqRegularisation(context.RgId, (int)context.RsqId);
            }

            string result = CommonRepository.ComputeRegularisation(new ComputeRegularisationParams
            {
                Acces = context.Scope.AsCode(),
                Risque = idRsq,
                Garantie = string.Empty,
                CodeOffre = context.IdContrat.CodeOffre,
                Formule = 0,
                Id = context.RgId,
                ContextId = context.Scope == RegularisationScope.Contrat ? context.RgId : idRsqRegul,
                Mode = context.Mode.AsCode(),
                Top = "X",
                TypeContrat = context.IdContrat.Type,
                VersionContrat = context.IdContrat.Version
            });

            context.ValidationDone = !result.StartsWith("ERR");

            return result;
        }

        public RegularisationContext EnsureContext(RegularisationContext context)
        {
            context.States = GetCurrentStates(context.RgId).ToList();

            if ((context.Matrix == null || !context.Matrix.Any(x => x != null)) && context.LotId > 0)
            {
                context.Matrix = RegularisationRepository.GetRegulMatrice(context);
            }

            return context.Refresh();
        }

        public RegularisationContext BuildContext(RegularisationContext initialContext)
        {
            if (initialContext.AccessMode == AccessMode.CREATE)
            {
                return BuildInitialContext(initialContext);
            }

            return BuildCurrentContext(initialContext);
        }

        public List<ParametreDto> GetModeleTypeRegul(string codeOffre, string version, string type, string codeAvn)
        {
            var result = CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "RGMRG").OrderBy(i => i.CodeTpcn2).ToList();

            if (RegularisationRepository.IsCoassurance(codeOffre, version, type, codeAvn))
            {
                result = result.Where(i => i.Code.Equals("COASS", StringComparison.InvariantCulture)).ToList();

            }
            else
            {
                result.Remove(result.FirstOrDefault(i => i.Code.Equals("COASS", StringComparison.InvariantCulture)));

                var risques = RegularisationRepository.ParamHorsCoassurance(codeOffre, version);

                if (risques.All(i => i.StrReturnCol != "O"))
                {
                    result.Remove(result.FirstOrDefault(i => i.Code.Equals("STAND", StringComparison.InvariantCulture)));
                }
                if (risques.All(i => i.StrReturnCol2 != "O"))
                {
                    result.Remove(result.FirstOrDefault(i => i.Code.Equals("PB", StringComparison.InvariantCulture)));
                }
                if (risques.All(i => i.StrReturnCol2 != "B"))
                {
                    result.Remove(result.FirstOrDefault(i => i.Code.Equals("BNS", StringComparison.InvariantCulture)));
                }
                if (risques.All(i => i.StrReturnCol2 != "U"))
                {
                    result.Remove(result.FirstOrDefault(i => i.Code.Equals("BURNER", StringComparison.InvariantCulture)));
                }
            }
            return result;
        }

        public IdContratDto GetContratInfo(string codeOffre, string version, string type)
        {
            return RegularisationRepository.GetContratInfo(codeOffre, version, type);
        }

        public IEnumerable<RegularisationStateDto> GetCurrentStates(long rgId)
        {
            return this.regularisationManager.GetCurrentStates(rgId);
        }

        public RegularisationComputeData ComputeCotisations(RegularisationComputeData computeData)
        {
            return RegularisationManager.ComputeCotisation(computeData);
        }

        public RegularisationBandeauContratDto GetBandeauContratInfo(string codeOffre, string version, string type, bool lightVersion = false)
        {
            return RegularisationRepository.GetBandeauContratInfo(codeOffre, version, type, lightVersion);
        }

        public List<ParametreDto> GetAvailableUnites()
        {
            return RegularisationRepository.GetListUnites();
        }

        public List<ParametreDto> GetAvailableCodeTaxes()
        {
            return RegularisationRepository.GetListCodesTaxes();
        }

        public SaisieGarantieInfosDto GetGarantiesRCFRHeader(RegularisationContext context)
        {
            var infoHead = RegularisationRepository.GetGarantiesRCFRHeader(new GarantieFilter { GrId = context.RgGrId }) ?? RegularisationRepository.GetGarantiesRCFRHeaderHisto(new GarantieFilter { GrId = context.RgGrId });
            var (canSimplifyStepFlow, nbPeriodes) = regularisationManager.CanSimplifyStepFlow(context);
            infoHead.IsGarantieAuto = infoHead.IsGarantieAuto && !IsSimplifiedReguleFlow(context);
            return infoHead;
        }

        public ListeGarantiesRCDto GetGarantiesRCGroup(RegularisationContext context)
        {
            return RegularisationManager.GetGarantiesRCGroup(context);
        }

        public ListeGarantiesRCDto ComputeGarantiesRC(RegularisationContext context, ListeGarantiesRCDto garanties)
        {
            return RegularisationManager.ComputeGarantiesRC(context, garanties, false);
        }

        public string ValidateGarantiesRC(RegularisationContext context, ListeGarantiesRCDto garanties)
        {
            return RegularisationManager.ValidateRegulGarantieRC(context, garanties);
        }

        public List<LigneMouvtGarantieDto> SupprimerMouvtPeriod(RegularisationContext context)
        {
            return RegularisationManager.SupprimerMouvtPeriod(context);
        }

        public bool IsSimplifiedReguleFlow(RegularisationContext context)
        {
            if (context.Mode == RegularisationMode.Standard)
            {
                var (canSimplifyStepFlow, nbPeriodes) = regularisationManager.CanSimplifyStepFlow(context);

                return canSimplifyStepFlow && (nbPeriodes == 0 || nbPeriodes == 1);
            }
            return false;
        }

        public RegularisationGarInfoDto GetInfoRegularisationGarantie(RegularisationContext context)
        {
            context.HasMultiRC = this.regularisationService.HasMultiRCSelection(context.LotId, context.GrId);
            context.User = WCFHelper.GetFromHeader("UserAS400");
            var batchSelections = this.regularisationService.GetSelectionGaranties(context.LotId);
            context.CodeFormule = batchSelections.First(x => x.IdGarantie == context.GrId).NumeroFormule;
            var selections = batchSelections.Where(x => context.CodeFormule == x.NumeroFormule);
            var mainSelection = context.HasMultiRC ? selections.First(x => x.CodeGarantie == AlbOpConstants.RCFrance) : selections.First(x => x.IdGarantie == context.GrId);
            context.CodeFormule = mainSelection.NumeroFormule;
            var libGarantie = this.garantieService.GetLibelleGarantie(mainSelection.CodeGarantie);
            var errors = new List<string>();
            var folder = mainSelection.AffaireId.Adapt<Folder>();
            if (!context.IsReadOnlyMode)
            {
                var (debut, fin) = this.regularisationService.GetPeriodBounds(mainSelection.AffaireId, context.RgId);
                this.regularisationService.PurgeTempPeriodes(mainSelection.AffaireId);
                if (context.HasMultiRC)
                {
                    selections.Where(x => x.IsRCGarantie).ToList().ForEach(x =>
                        errors.Add(this.as400Repository.ApplyMouvementsGarantie(
                            new PGMFolder(folder) { User = context.User, ActeGestion = context.TypeAvt },
                            x.NumeroRisque, x.NumeroFormule, x.CodeGarantie,
                            (x.DateDebut < debut ? debut : x.DateDebut).ToIntYMD(),
                            (!x.DateFin.HasValue || x.DateFin > fin ? fin : x.DateFin.Value).ToIntYMD())));

                    this.regularisationService.EnsureTempPeriodesRC(
                        mainSelection.AffaireId,
                        context.RgId,
                        mainSelection.NumeroRisque,
                        mainSelection.NumeroFormule,
                        selections.Select(g => g.IdGarantie).ToArray(),
                        context.AccessMode);

                    this.as400Repository.ApplyMouvementsGarantieRC(
                        new PGMFolder(folder) { User = context.User, ActeGestion = context.TypeAvt },
                        mainSelection.NumeroRisque, mainSelection.NumeroFormule,
                        (mainSelection.DateDebut < debut ? debut : mainSelection.DateDebut).ToIntYMD(),
                        (!mainSelection.DateFin.HasValue || mainSelection.DateFin > fin ? fin : mainSelection.DateFin.Value).ToIntYMD());
                }
                else
                {
                    errors.Add(this.as400Repository.ApplyMouvementsGarantie(
                        new PGMFolder(folder) { User = context.User, ActeGestion = context.TypeAvt },
                        mainSelection.NumeroRisque, mainSelection.NumeroFormule, mainSelection.CodeGarantie,
                        (mainSelection.DateDebut < debut ? debut : mainSelection.DateDebut).ToIntYMD(),
                        (!mainSelection.DateFin.HasValue || mainSelection.DateFin > fin ? fin : mainSelection.DateFin.Value).ToIntYMD()));

                    this.regularisationService.EnsureTempPeriodes(
                        mainSelection.AffaireId,
                        context.RgId,
                        mainSelection.NumeroRisque,
                        mainSelection.NumeroFormule,
                        mainSelection.IdGarantie,
                        mainSelection.CodeGarantie,
                        context.AccessMode);
                }

                if (errors.Any(e => e.ContainsChars()))
                {
                    throw new BusinessValidationException(errors.Where(e => e.ContainsChars()).Select(e => new ValidationError(e)));
                }
            }

            var infos = new RegularisationGarInfoDto
            {
                GarantieInfo = new GarantieInfo
                {
                    CodeFormule = mainSelection.NumeroFormule,
                    CodeGarantie = mainSelection.CodeGarantie,
                    CodeOption = 1,
                    Id = context.GrId,
                    Libelle = libGarantie
                },
                RegulPeriodDetail = this.regularisationService.GetDetailsInfoGarantie(context.GrId, context.LotId, context.RgId),
                AppliqueRegule = this.formuleService.GetApplicationsFormule(mainSelection.AffaireId, 1, context.CodeFormule).Select(x =>
                    new WSAS400.DTO.Offres.Risque.RisqueDto
                    {
                        Code = x.Numero,
                        Descriptif = x.Designation,
                        Designation = x.Designation,
                        Objets = x.Objets.Select(o => new WSAS400.DTO.Offres.Risque.Objet.ObjetDto
                        {
                            Code = o.Code,
                            Descriptif = o.Description,
                            Designation = o.Description
                        }).ToList()
                    }).First()
            };
            if (!context.IsReadOnlyMode)
            {
                context.GrId = mainSelection.IdGarantie;
                this.regularisationService.EnsureInsertMouvements(context);
            }
            infos.ListMvtPeriod = this.regularisationService.GetMouvementsGarantie(mainSelection.AffaireId, context.RgId, (int)context.RsqId, mainSelection.NumeroFormule, mainSelection.CodeGarantie).ToList();

            var currentPeriods = this.regularisationService.GetPeriodesGaranties(context.RgId).Where(x => x.NumeroFormule == context.CodeFormule).ToList();
            var rcRgugIds = context.HasMultiRC ? currentPeriods.Where(p => AlbOpConstants.AllRCGar.Contains(p.CodeGarantie)).ToLookup(x => (x.DateDebut, x.DateFin)) : null;
            infos.ListPeriodRegulGar = currentPeriods.Where(g => g.CodeGarantie == mainSelection.CodeGarantie).Select(x => new LigneMouvtGarantieDto
            {
                AssietteValeur = Convert.ToDouble(x.Assiette),
                IdRCFR = context.HasMultiRC ? rcRgugIds[(x.DateDebut, x.DateFin)].First(k => k.CodeGarantie == AlbOpConstants.RCFrance).Id : 0,
                MontantRegulHF = Convert.ToDouble(x.MontantHTHorsCATNAT),
                Code = x.Id,
                PeriodeRegulDeb = x.DateDebut.ToIntYMD(),
                PeriodeRegulFin = x.DateFin.ToIntYMD(),
                Situation = x.IsValidated ? "V" : "N",
                situation = x.IsValidated ? "V" : "N",
                TauxForfaitHTUnite = x.UniteTauxHT,
                TauxForfaitHTValeur = Convert.ToDouble(x.ValeurHT)
            }).ToList();
            var (debMin, finMax) = this.regularisationService.GetMouvementsMinMax(mainSelection.AffaireId, context.RgId, (int)context.RsqId, context.CodeFormule, context.AccessMode);
            infos.MouvementPeriodeDebMin = debMin.ToIntYMD();
            infos.MouvementPeriodeFinMax = finMax.ToIntYMD();
            return infos;
        }

        #region Méthodes privées
        private void InitInfos(Folder folder, AffaireDto affaire, RegularisationInfoDto infosDto, RegularisationContext context, CauseResetRegularisation? causeReset, out string codeErreur)
        {
            codeErreur = string.Empty;
            DateTime? debut, fin;
            var regul = context.RgId != 0 ? this.regularisationService.GetDto(context.RgId) : null;

            if (regul is null || regul.DateDebut.Year == 1 || context.RgId < 1 || causeReset.GetValueOrDefault() > 0)
            {
                var kda300 = this.as400Repository.CheckAndGetPeriodesRegularisation(
                    new PGMFolder(folder)
                    {
                        User = WCFHelper.GetFromHeader("UserAS400"),
                        ActeGestion = infosDto.TypeAvt,
                    },
                    infosDto.Exercice,
                    (
                        infosDto.PeriodeDebInt.HasValue ? infosDto.PeriodeDebInt.Value.YMDToDate() : default,
                        infosDto.PeriodeFinInt.HasValue ? infosDto.PeriodeFinInt.Value.YMDToDate() : default
                    ));

                if (kda300.DernierAvn != affaire.NumeroAvenant)
                {
                    var oldId = affaire.Adapt<AffaireId>();
                    oldId.NumeroAvenant = kda300.DernierAvn;
                    oldId.IsHisto = true;
                    var oldAffaire = this.affaireService.GetAffaire(oldId);
                    infosDto.TauxHCATNAT = (double)oldAffaire.TauxCommission;
                    infosDto.TauxCATNAT = (double)oldAffaire.TauxCommissionCATNAT;
                }
                else
                {
                    infosDto.TauxHCATNAT = (double)affaire.TauxCommission;
                    infosDto.TauxCATNAT = (double)affaire.TauxCommissionCATNAT;
                }

                debut = kda300.AnneeDebut == 0 ? default(DateTime?) : new DateTime(kda300.AnneeDebut, kda300.MoisDebut, kda300.JourDebut);
                fin = kda300.AnneeFin == 0 ? default(DateTime?) : new DateTime(kda300.AnneeFin, kda300.MoisFin, kda300.JourFin);
                infosDto.PeriodeDebInt = debut is null ? null : debut?.ToIntYMD();
                infosDto.PeriodeFinInt = fin is null ? null : fin?.ToIntYMD();
                infosDto.CodeCourtier = (int)kda300.CodeCourtierPayeur;
                infosDto.CodeCourtierCom = (int)kda300.CodeCourtierCommission;
                infosDto.CodeQuittancement = kda300.CodeEncaissement;
                infosDto.LibQuittancement = infosDto.Quittancements.FirstOrDefault(x => x.Code == kda300.CodeEncaissement)?.Libelle ?? string.Empty;
                infosDto.MultiCourtier = kda300.MultiCourtier;
                codeErreur = kda300.CodeErreur;
            }
            else
            {
                if (infosDto.NumAvt > regul.AffaireId.NumeroAvenant.Value)
                {
                    infosDto.IsHisto = true;
                    infosDto.NumAvt = regul.AffaireId.NumeroAvenant.Value;
                    infosDto.NumInterneAvt = regul.AffaireId.NumeroAvenant.Value;
                }

                infosDto.CodeCourtier = regul.IdCourtier;
                infosDto.CodeCourtierCom = regul.IdCourtierCommission;
                infosDto.CodeQuittancement = regul.Encaissement.Code;
                infosDto.LibQuittancement = regul.Encaissement.LibelleLong;
                infosDto.TauxHCATNAT = (double)regul.TauxCommission;
                infosDto.TauxCATNAT = (double)regul.TauxCommissionCATNAT;
                infosDto.PeriodeDebInt = regul.DateDebut.ToIntYMD();
                infosDto.PeriodeFinInt = regul.DateFin.ToIntYMD();
                infosDto.Exercice = regul.Exercice;
                infosDto.MotifAvt = regul.Motif.Code;
                debut = regul.DateDebut;
                fin = regul.DateFin;
            }

            infosDto.Courtiers = this.repository.GetCourtiersRegularisation(folder).Select(x => new ParametreDto { Code = x.id.ToString(), Libelle = x.nom }).ToList();
            infosDto.NomCourtier = infosDto.Courtiers.FirstOrDefault(x => x.Code == infosDto.CodeCourtier.ToString())?.Libelle;
            infosDto.NomCourtierCom = infosDto.Courtiers.FirstOrDefault(x => x.Code == infosDto.CodeCourtierCom.ToString())?.Libelle;
            infosDto.RetourPGM = string.Format("{0}_{1}_{2}", debut?.ToString("dd/MM/yyyy") ?? "0", fin?.ToString("dd/MM/yyyy") ?? "0", codeErreur);
        }

        private void InitBatchData(RegularisationInfoDto infosDto, RegularisationContext context, CauseResetRegularisation? causeReset, IEnumerable<AffaireDto> fullAffaire)
        {
            if (context.LotId < 1 || causeReset.GetValueOrDefault() > 0)
            {
                InitBatchDataRisques(infosDto, context, fullAffaire);
            }
            infosDto.LotId = context.LotId;
            this.regularisationService.CleanUpSelections(context.LotId);
            infosDto.HasSelections = this.regularisationService.GetSelection(context.LotId).Any();
        }

        private void InitBatchDataRisques(RegularisationInfoDto infosDto, RegularisationContext context, IEnumerable<AffaireDto> fullAffaire)
        {
            var currentFolder = fullAffaire.First(a => !infosDto.IsHisto || a.NumeroAvenant == infosDto.NumAvt);
            var affaireDebut = currentFolder.DateEffet.Value;
            if (affaireDebut > infosDto.Fin || fullAffaire.All(x => x.NumeroAvenant <= currentFolder.NumeroAvenant && x.DateFin.HasValue && x.DateFin < infosDto.Debut))
            {
                infosDto.RetourPGM = "__Aucun élément trouvé dans la période";
                return;
            }

            context.LotId = this.regularisationService.ResetByLot(context.LotId);
            var risques = this.risqueService.GetAllRisquesByAffaire(currentFolder.CodeAffaire, currentFolder.NumeroAliment)
                .Where(r => !infosDto.IsHisto || r.AffaireId.NumeroAvenant <= infosDto.NumAvt)
                .Where(r =>
                {
                    r.DateDebutImplicite = r.DateDebut ?? (r.NumeroAvenantCreation == 0 ? affaireDebut : fullAffaire.First(x => x.NumeroAvenant == r.NumeroAvenantCreation).DateEffetAvenant);
                    //r.DateFinImplicite = r.DateFin ?? fullAffaire.First(x => x.NumeroAvenant == r.NumeroAvenantModification).DateFin;
                    r.DateFinImplicite = r.DateFin ?? fullAffaire.OrderByDescending(x => x.NumeroAvenant).First().DateFin;
                    return (r.ParticipationBeneficiaire && context.Mode == RegularisationMode.PB
                            || r.BonnificationNonSinistre && context.Mode == RegularisationMode.BNS
                            || r.ARegulariser && (r.BonnificationNonSinistreIncendie && context.Mode == RegularisationMode.Burner || context.Mode == RegularisationMode.Standard))
                        && r.DateDebutImplicite <= infosDto.Fin
                        && (!r.DateFin.HasValue || r.DateFinImplicite >= infosDto.Debut);
                });

            var groupRisques = risques.GroupBy(x => x.Numero);
            if (context.Mode == RegularisationMode.BNS)
            {
                groupRisques = groupRisques.OrderBy(x => x.Key).Take(1);
            }

            foreach (var groupRsq in groupRisques)
            {
                var dernierRisque = groupRsq.OrderBy(x => x.AffaireId.NumeroAvenant).Last();
                this.regularisationService.AddSelectionRisque(new SelectionRisqueRegularisationDto
                {
                    Risque = dernierRisque,
                    EnCours = true,
                    IdLot = context.LotId
                });
            }

            var objets = new List<ObjetDto>();
            foreach (var groupObj in risques.SelectMany(x => x.Objets.Select(o => new { risque = x, objet = o })).GroupBy(x => new { rsq = x.objet.NumRisque, num = x.objet.Code }))
            {
                var rsqObjList = groupObj
                    .Where(x =>
                    {
                        x.objet.DateDebutImplicite = x.objet.DateDebut ?? x.risque.DateDebutImplicite;
                        x.objet.DateFinImplicite = x.objet.DateFin ?? x.risque.DateFinImplicite;
                        return x.objet.DateDebutImplicite <= infosDto.Fin
                            && (!x.objet.DateFin.HasValue || x.objet.DateFinImplicite >= infosDto.Debut);
                    });
                if (rsqObjList.Any())
                {
                    objets.AddRange(rsqObjList.Select(x => x.objet));
                    var rsq_obj = rsqObjList.OrderBy(x => x.risque.AffaireId.NumeroAvenant).Last();
                    this.regularisationService.AddSelectionObjet(new SelectionObjetRegularisationDto
                    {
                        Objet = rsq_obj.objet,
                        EnCours = true,
                        IdLot = context.LotId
                    });
                }
            }

            InitBatchDataFormules(infosDto, context, fullAffaire, risques, objets);
        }

        private void InitBatchDataFormules(RegularisationInfoDto infosDto, RegularisationContext context, IEnumerable<AffaireDto> fullAffaire, IEnumerable<RisqueDto> allRisquesSelection, IEnumerable<ObjetDto> allObjetsSelection)
        {
            var affaire = fullAffaire.First(a => !infosDto.IsHisto || a.NumeroAvenant == infosDto.NumAvt);
            var allOptions = this.formuleService.GetOptionsAppsWithHisto(affaire.CodeAffaire, affaire.NumeroAliment).Where(o => !infosDto.IsHisto || o.NumeroAvenant <= infosDto.NumAvt);
            var optionSelection = new HashSet<RegulOptionSelection>();
            foreach (var g in allRisquesSelection.GroupBy(x => x.Numero))
            {
                var options = allOptions.Where(o => o.Applications.Any(app => app.NumRisque == g.Key));
                foreach (var opt in options)
                {
                    if (opt.Applications.Any(ap => ap.NumObjet > 0))
                    {
                        g.First(x => x.AffaireId.NumeroAvenant == (opt.NumeroAvenant ?? affaire.NumeroAvenant))
                            .Objets.Where(x => opt.Applications.Any(y => y.NumObjet == x.Code && y.NumRisque == g.Key)
                                && x.DateDebutImplicite <= infosDto.Fin
                                && (!x.DateFin.HasValue || x.DateFinImplicite >= infosDto.Debut))
                            .ToList().ForEach(ob => optionSelection.Add(new RegulOptionSelection
                            {
                                Formule = opt.NumeroFormule,
                                Option = opt.OptionNumber,
                                Risque = g.Key,
                                Objet = ob.Code,
                                Avenant = opt.NumeroAvenant.GetValueOrDefault(affaire.NumeroAvenant)
                            }));
                    }
                    else
                    {
                        optionSelection.Add(new RegulOptionSelection
                        {
                            Formule = opt.NumeroFormule,
                            Option = opt.OptionNumber,
                            Risque = g.Key,
                            Objet = null,
                            Avenant = opt.NumeroAvenant.GetValueOrDefault(affaire.NumeroAvenant)
                        });
                    }
                }
            }

            InitBatchDataOptionsGaranties(infosDto, context, fullAffaire, allRisquesSelection, allObjetsSelection, optionSelection);
        }

        private void InitBatchDataOptionsGaranties(RegularisationInfoDto infosDto, RegularisationContext context, IEnumerable<AffaireDto> fullAffaire, IEnumerable<RisqueDto> allRisquesSelection, IEnumerable<ObjetDto> allObjetsSelection, HashSet<RegulOptionSelection> optionSelection)
        {
            var affaire = fullAffaire.First(a => !infosDto.IsHisto || a.NumeroAvenant == infosDto.NumAvt);
            var selectionGaranties = new HashSet<SelectionRegularisationDto>();
            var garanties = this.regularisationService.GetGaranties(affaire.CodeAffaire, affaire.NumeroAliment, infosDto.Debut.Value, infosDto.Fin.Value);
            var typeRegulGar = ParamGarantieRepository.GetAllTypeRegul().GroupBy(x => x.CodeGarantie);
            foreach (var gf in optionSelection.GroupBy(x => x.Formule))
            {
                var formule0 = gf.First();
                DateTime? minDateFormule = null;
                DateTime? maxDateFormule = null;
                if (gf.Any(x => x.Objet > 0))
                {
                    var objList = allRisquesSelection.Where(x => gf.Any(y => y.Risque == x.Numero)).SelectMany(y => y.Objets).Where(o => gf.Any(f => f.Objet == o.Code && allObjetsSelection.Contains(o)));
                    minDateFormule = objList.Any(o => o.DateDebutImplicite is null) ? null : objList.Min(o => o.DateDebutImplicite);
                    maxDateFormule = objList.Any(o => o.DateFinImplicite is null) ? null : objList.Max(o => o.DateFinImplicite);
                }
                this.regularisationService.AddSelectionFormule(new SelectionFormuleRegularisationDto
                {
                    EnCours = null,
                    IdLot = context.LotId,
                    NumeroFormule = gf.Key,
                    NumeroRisque = gf.First().Risque,
                    AffaireId = fullAffaire.First(x => x.NumeroAvenant == gf.Min(f => f.Avenant)).Adapt<AffaireId>(),
                    DateDebut = minDateFormule ?? allRisquesSelection.First(x => x.Numero == gf.First().Risque).DateDebutImplicite.Value,
                    DateFin = maxDateFormule ?? allRisquesSelection.OrderBy(x => x.Numero).Last().DateFinImplicite
                });
                gf.ToList().ForEach(f =>
                {
                    var avnAffaire = fullAffaire.First(x => x.NumeroAvenant == f.Avenant);

                    // si périodicité R, la prime n'est pas nécessaire
                    var garantiesReguls = garanties.Where(x => x.Formule == gf.Key && x.NumeroAvenant.GetValueOrDefault() == f.Avenant);
                    if (context.Mode != RegularisationMode.Standard || avnAffaire.Periodicite.Code != "R")
                    {
                        garantiesReguls = garantiesReguls.Where(x => x.HasPrime || AlbOpConstants.AllRCGar.Contains(x.CodeGarantie));

                        // si RCFR non tarifées, exclure les autres RC non tarifées
                        if (garantiesReguls.Any(x => x.CodeGarantie == AlbOpConstants.RCFrance && !x.HasPrime))
                        {
                            garantiesReguls = garantiesReguls.Where(x => !AlbOpConstants.AllRCGar.Contains(x.CodeGarantie) || x.HasPrime);
                        }
                    }

                    if (context.Mode == RegularisationMode.BNS)
                    {
                        garantiesReguls = garanties.Where(x => typeRegulGar.Any(y => y.Key == x.CodeGarantie && y.Any(z => z.CodeTypeRegul == "BN")) && x.HasPrime);
                        if (!garantiesReguls.Any())
                        {
                            throw new AlbFoncException("Aucune Garantie BNS n'a été sélectionnée, il n'est donc pas possible de saisir une BNS");
                        }
                    }

                    var debutDates = new HashSet<DateTime?>();
                    var finDates = new HashSet<DateTime?>();
                    var risque = allRisquesSelection.FirstOrDefault(x => x.Numero == f.Risque && x.AffaireId.NumeroAvenant == f.Avenant);

                    if (risque != null)
                    {
                        ObjetDto objet = f.Objet > 0 ? risque.Objets.FirstOrDefault(o => f.Objet == o.Code && allObjetsSelection.Contains(o)) : null;
                        foreach (var g in garantiesReguls.Where(g => g.HasPortees))
                        {
                            debutDates.Clear();
                            finDates.Clear();
                            foreach (var p in g.Portees)
                            {
                                var garapObj = risque.Objets.FirstOrDefault(x => x.Code == p.NumObjet && allObjetsSelection.Contains(x));
                                if (garapObj is null)
                                {
                                    // nothing to add
                                }
                                else
                                {
                                    debutDates.Add(garapObj.DateDebutImplicite);
                                    finDates.Add(garapObj.DateFinImplicite);
                                }
                            }
                            if (debutDates.Any())
                            {
                                selectionGaranties.Add(new SelectionRegularisationDto
                                {
                                    EnCours = true,
                                    IdGarantie = g.Id,
                                    IdLot = context.LotId,
                                    NumeroRisque = gf.First().Risque,
                                    NumeroFormule = gf.Key,
                                    Perimetre = PerimetreSelectionRegul.Garantie,
                                    AffaireId = fullAffaire.First(x => x.NumeroAvenant == f.Avenant).Adapt<AffaireId>(),
                                    NumeroObjet = 0,
                                    DateDebut = debutDates.Min().Value,
                                    DateFin = finDates.Max(),
                                    SequenceGarantie = g.Sequence,
                                    CodeGarantie = g.CodeGarantie
                                });
                            }
                        }

                        foreach (var g in garantiesReguls.Where(g => !g.HasPortees))
                        {
                            selectionGaranties.Add(new SelectionRegularisationDto
                            {
                                AffaireId = fullAffaire.First(x => x.NumeroAvenant == f.Avenant).Adapt<AffaireId>(),
                                EnCours = true,
                                IdGarantie = g.Id,
                                IdLot = context.LotId,
                                Perimetre = PerimetreSelectionRegul.Garantie,
                                NumeroRisque = gf.First().Risque,
                                NumeroFormule = gf.Key,
                                NumeroObjet = 0,
                                DateDebut = g.DateDebut ?? objet?.DateDebutImplicite ?? risque.DateDebutImplicite.GetValueOrDefault(),
                                DateFin = g.DateSortie ?? objet?.DateFinImplicite ?? risque.DateFinImplicite,
                                SequenceGarantie = g.Sequence,
                                HasItsOwnDateSortie = g.DateSortie.HasValue,
                                CodeGarantie = g.CodeGarantie
                            });
                        }
                    }
                });
            }

            foreach (var gs in selectionGaranties.GroupBy(x => new { x.SequenceGarantie, x.NumeroFormule }))
            {
                this.regularisationService.AddSelection(new SelectionRegularisationDto
                {
                    AffaireId = affaire.Adapt<AffaireId>(),
                    EnCours = true,
                    IdGarantie = gs.OrderBy(g => g.AffaireId.NumeroAvenant).Last().IdGarantie,
                    SequenceGarantie = gs.Key.SequenceGarantie,
                    IdLot = context.LotId,
                    Perimetre = PerimetreSelectionRegul.Garantie,
                    NumeroFormule = gs.Key.NumeroFormule,
                    NumeroRisque = gs.First().NumeroRisque,
                    NumeroObjet = 0,
                    DateDebut = gs.Min(x => x.DateDebut),
                    DateFin = gs.Any(x => x.HasItsOwnDateSortie) ? gs.Where(x => x.HasItsOwnDateSortie).Max(x => x.DateFin) : gs.Max(x => x.DateFin),
                    CodeGarantie = gs.First().CodeGarantie
                });
            }
        }

        private void InitializeComputedDataForCalculationBurner(RegularisationComputeData data)
        {
            if (data.SeuilSPRetenu != 0)
            {
                data.PrimeCalculee = data.ChargementSinistres / ((decimal)data.SeuilSPRetenu / 100) * 100;
            }
        }

        private void InitializeComputedDataForCalculationPB(RegularisationComputeData data)
        {
            if ((data.CotisationsRetenues == 0 && data.CotisationPeriode != 0) || (data.MontantCalcule == 0))
            {
                var prcR = Convert.ToDecimal(data.Ristourne) / 100;
                var prcC = Convert.ToDecimal(data.PrcCotisationsRetenues) / 100;
                var prcS = Convert.ToDecimal(data.PrcSeuilSP) / 100;
                var prcT = data.TauxAppel == 0 ? 1 : Convert.ToDecimal(data.TauxAppel) / 100;

                data.CotisationsRetenues = prcT == 1 ? prcC * data.CotisationPeriode : decimal.Round((data.CotisationPeriode / prcT), 2) * prcC;

                if (!string.IsNullOrEmpty(data.IdContrat) && data.IdContrat.StartsWith("TR"))
                {
                    if (data.ChargementSinistres != data.IndemnitesFrais + data.Provisions - (data.Recours + data.Previsions))
                    {
                        data.ChargementSinistres = data.IndemnitesFrais + data.Provisions - (data.Recours + data.Previsions);
                    }

                    var P = data.CotisationPeriode;
                    var O = data.ReportChargesRetenu;
                    var S = data.ChargementSinistres + O;
                    var A = data.MontantRistourneAnticipee;


                    var total = prcR * (prcC * P - S) - A;
                    data.MontantCalcule = total;
                }
                else
                {
                    //MontantCalcule = prcR*(CotisationsRetenues - ChargementSinistres) - MontantRistourneAnticipee;
                    // ARA - 3244
                    if (data.ChargementSinistres <= (data.CotisationPeriode / prcT) * prcS)
                    {
                        data.MontantAffiche = -(prcR * (data.CotisationsRetenues - data.ChargementSinistres));
                    }
                    else
                    {
                        if (data.ChargementSinistres < 0)
                        {
                            data.MontantAffiche = prcR * (prcR * data.CotisationsRetenues);
                        }
                        else
                        {
                            data.MontantAffiche = 0;
                        }

                    }
                    var cotisationEmise = data.CotisationPeriode;
                    data.MontantRistourneAnticipee = cotisationEmise - decimal.Round((data.CotisationPeriode / prcT), 2);
                    data.MontantComptant = data.MontantCalcule - data.MontantRistourneAnticipee;
                    data.IsAnticipee = data.TauxAppel != 100 && data.TauxAppel != 0;
                }
            }
        }

        private RegularisationComputeData InitializeComputedDataForCalculationPBTR(RegularisationComputeData data)
        {
            if ((data.CotisationsRetenues == 0 && data.CotisationPeriode != 0) || (data.MontantCalcule == 0))
            {
                if (data.ChargementSinistres != data.IndemnitesFrais + data.Provisions - (data.Recours + data.Previsions))
                {
                    data.ChargementSinistres = data.IndemnitesFrais + data.Provisions - (data.Recours + data.Previsions);
                }

                data.CotisationsRetenues = data.CotisationPeriode * data.PrcCotisationsRetenues / 100;

                var prcR = Convert.ToDecimal(data.Ristourne) / 100;
                var prcC = Convert.ToDecimal(data.PrcCotisationsRetenues) / 100;
                var P = data.CotisationPeriode;
                var O = data.ReportChargesRetenu;
                var S = data.ChargementSinistres + O;
                var A = data.MontantRistourneAnticipee;


                var total = prcR * (prcC * P - S) - A;
                data.MontantCalcule = total;
            }
            else
            {
                data.Refresh();
            }

            return data;
        }

        private StepRegularisationDto ValidateAndSelectStep(RegularisationContext context, RegularisationStep[] steps, bool cancelMode)
        {
            var result = new StepRegularisationDto();

            switch (context.Step)
            {
                case RegularisationStep.ChoixPeriodeCourtier:
                    result.Message = context.IsReadOnlyMode || cancelMode ? string.Empty : ProcessChoixPeriodeCourtierStep(context);
                    InitRegulSimple(context);
                    if (result.Step != RegularisationStep.Invalid)
                    {
                        result.Step = SelectChoixCourtierNextStep(context, steps);
                    }

                    break;
                case RegularisationStep.ChoixRisques:
                    if (context.ValidationDone)
                    {
                        result.Step = SelectChoixRisquesNextStep(context, steps);
                    }
                    else
                    {
                        result.Step = RegularisationStep.Invalid;
                        result.Message = "Impossible d'accéder à l'écran de cotisation. Aucune validation n'a été effectuée.";
                    }
                    break;
                case RegularisationStep.Regularisation:
                    result.Message = context.IsReadOnlyMode ? string.Empty : ProcessRegularisationStep(context);
                    if (result.Step != RegularisationStep.Invalid)
                    {
                        result.Step = SelectRegularisationNextStep(context, steps);
                    }

                    break;
                case RegularisationStep.Invalid:
                    result.Message = "Impossible d'effectuer l'action demandée";
                    break;
                default:
                    result.Step = StepManagerRegularisation.SelectChoixCourtierNextStep(context, steps, regularisationManager);
                    break;
            }

            SetNextStep(context, result.Step);

            return result;
        }

        private void SetNextStep(RegularisationContext context, RegularisationStep? step)
        {
            if (step.GetValueOrDefault() != RegularisationStep.Invalid)
            {
                if (context.Step == RegularisationStep.ChoixPeriodeCourtier)
                {
                    context.Scope = step == RegularisationStep.ChoixRisques ? RegularisationScope.Risque : RegularisationScope.Contrat;
                    context.IsRisquesHomogenes = context.Scope == RegularisationScope.Contrat;
                }

                context.Step = step.Value;
            }
        }

        private StepRegularisationDto ValidateBeforeRichingStep(RegularisationContext context, RegularisationStep step)
        {
            var result = ValidateAndSelectStep(context, new RegularisationStep[] { step }, false);
            if (CanRichStepAnyway(context, step))
            {
                context.Step = step;
                result.Message = null;
                result.Step = step;
                context.Error = null;
            }
            else
            {
                context.Step = result.Step.GetValueOrDefault();
            }

            return result;
        }

        private bool CanRichStepAnyway(RegularisationContext context, RegularisationStep step)
        {
            if (context.Step == RegularisationStep.Regularisation && !context.ValidationDone)
            {
                return true;
            }

            return false;
        }

        private void PerformBeforePreviousStep(RegularisationContext context)
        {
            PerformAfterSelectStep(context, true);
        }

        private void PerformAfterSelectStep(RegularisationContext context, bool stepToPrevious = false)
        {
            if (stepToPrevious)
            {
                context.Error = null;
            }
            if (context.Step == RegularisationStep.ChoixRisques)
            {
                context.RsqId = default;
            }
            else if (stepToPrevious && context.Step == RegularisationStep.ChoixPeriodeCourtier || !stepToPrevious && context.Step == RegularisationStep.Cotisations)
            {
                if (context.RgId > 0 && (context.KeyValues?.Any() ?? false))
                {
                    var paramVals = context.KeyValues.Last().ToParamDictionary();
                    if (!paramVals.ContainsKey(AlbParameterName.AVNMODE.ToString()))
                    {
                        paramVals.InsetSecondToLast(AlbParameterName.AVNMODE.ToString(), context.AccessMode.ToString());
                    }
                    else if (paramVals[AlbParameterName.AVNMODE.ToString()] != AccessMode.CONSULT.ToString())
                    {
                        paramVals[AlbParameterName.AVNMODE.ToString()] = context.AccessMode.ToString();
                    }
                    paramVals.InsetSecondToLast(AlbParameterName.REGULEID.ToString(), context.RgId.ToString(), true);
                    context.KeyValues[context.KeyValues.Length - 1] = paramVals.RebuildAddParamString();
                }
            }
            context.States = this.regularisationManager.GetCurrentStates(context.RgId).ToList();
        }

        private string ProcessChoixPeriodeCourtierStep(RegularisationContext context)
        {
            var result = this.regularisationManager.CreateOrUpdate(context);
            if (result.CodeErreurAvt.ContainsChars())
            {
                context.Error = new AlbErrorDto() { Code = result.CodeErreurAvt, Label = result.ErreurAvt };
            }
            else
            {
                context.RgId = result.ReguleId;
            }
            return context.Error?.Label;
        }

        private void InitRegulSimple(RegularisationContext context)
        {
            if (context.Mode == RegularisationMode.Standard)
            {
                context.CanSimplifyStepFlow = this.regularisationService.CanSimplifyStandard(context.LotId);
                if (!context.CanSimplifyStepFlow)
                {
                    return;
                }
                var periodes = this.regularisationService.GetPeriodesGaranties(context.LotId);
                if (!periodes.Any())
                {
                    this.regularisationManager.InitializeSimplifiedStepFlow(context);
                }
                else
                {
                    context.IsSimplifiedRegule = context.CanSimplifyStepFlow && periodes.Count() == 1;
                }
            }
        }

        private string ProcessRegularisationStep(RegularisationContext context)
        {
            if (context.Mode == RegularisationMode.PB || context.Mode == RegularisationMode.BNS || context.Mode == RegularisationMode.Burner)
            {
                return RegularisationManager.ValidateRisques(context);
            }

            return string.Empty;
        }

        private RegularisationStep SelectRegularisationNextStep(RegularisationContext context, RegularisationStep[] steps)
        {
            if (steps.Length == 1)
            {
                return steps.First();
            }

            // if multiple steps possible, redefine the step
            if (context.Mode == RegularisationMode.PB)
            {
                return RegularisationNextStepPB(context);
            }

            return 0;
        }

        private RegularisationStep RegularisationNextStepPB(RegularisationContext context)
        {
            return 0;
        }

        private RegularisationStep? SelectChoixRisquesNextStep(RegularisationContext context, RegularisationStep[] steps)
        {
            if (steps.Length == 1)
            {
                return steps.First();
            }

            return 0;
        }

        private RegularisationStep? SelectChoixCourtierNextStep(RegularisationContext context, RegularisationStep[] steps)
        {
            if (steps.Length == 1)
            {
                return steps.First();
            }

            switch (context.Mode)
            {
                case RegularisationMode.Standard:
                    if (context.CanSimplifyStepFlow)
                    {
                        if (context.IsSimplifiedRegule)
                        {
                            return RegularisationStep.Regularisation;
                        }

                        return RegularisationStep.ChoixPeriodesGarantie;
                    }

                    return RegularisationStep.ChoixRisques;
                case RegularisationMode.PB:
                case RegularisationMode.BNS:
                case RegularisationMode.Burner:
                    return context.Scope == RegularisationScope.Contrat ? RegularisationStep.Regularisation : RegularisationStep.ChoixRisques;
            }

            return null;
        }

        private RegularisationContext BuildCurrentContext(RegularisationContext context)
        {
            context = EnsureContext(context);
            return context.Refresh();
        }

        private RegularisationContext BuildInitialContext(RegularisationContext context)
        {
            return EnsureContext(context);
        }

        private RegularisationContext TryPreviousOrNext(RegularisationContext context, RegularisationStep stepToRich)
        {
            RegularisationStep[] steps = new RegularisationStep[] { StepManagerRegularisation.PreviousStep(context, regularisationManager) };
            if (stepToRich == steps.Single())
            {
                return GetPreviousStep(context);
            }

            steps = StepManagerRegularisation.NextAvailableSteps(context);
            if (steps.Length == 1 && stepToRich == steps.Single())
            {
                return ValidateStepAndGetNext(context);
            }

            return null;
        }

        #endregion
    }

    struct RegulOptionSelection
    {
        public int Formule;
        public int Option;
        public int Risque;
        public int? Objet;
        public int Avenant;
    }
}
