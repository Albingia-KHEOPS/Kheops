using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.DTO;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Business;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using Mapster;
using OP.DataAccess;
using OP.IOWebService.BLServices;
using OP.Services.Historization;
using OP.WSAS400.DTO;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.Regularisation;
using OPServiceContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OP.Services.BLServices.Regularisations {
    public class RegularisationManager {
        private readonly IDbConnection dbConnection;
        private readonly IRegularisationPort regularisationService;
        private readonly RegularisationRepository repository;
        private readonly ProgramAS400Repository as400Repository;
        private readonly CibleService cibleService;

        public RegularisationManager(IDbConnection connection, IRegularisationPort regularisationService, RegularisationRepository repository, ProgramAS400Repository as400Repository, CibleService cibleService) {
            this.dbConnection = connection;
            this.repository = repository;
            this.as400Repository = as400Repository;
            this.regularisationService = regularisationService;
            this.cibleService = cibleService;
        }

        public static void ReportDataRegulFromRsqToEnt(string codeContrat, string version, string codeAvn) {
            RegularisationRepository.ReportDataRegulFromRsqToEnt(codeContrat, version, codeAvn);
        }

        public static LigneRegularisationDto GetLastRegularisation(string codeContrat, string version, string type, string codeAvn) {
            return RegularisationRepository.GetLastRegularisation(codeContrat, version, type, codeAvn);
        }

        public static LigneRegularisationDto GetRegularisationByID(string codeContrat, string version, string type, long rgId) {
            return RegularisationRepository.GetRegularisationByID(codeContrat, version, type, rgId);
        }

        private bool IsRisquesHomogenes(Folder folder, long lotId) {
            return this.repository.GetDistinctRisques(folder, lotId) == 1;
        }

        public static RegularisationScope SetEnteteOrRiskPageAccess(RegularisationContext context) {
            var nbRsq = RegularisationRepository.GetNumberOfRiskWithDifferentRate(context.IdContrat, context.RgId);

            if (nbRsq == 1) {
                return RegularisationScope.Contrat;
            }

            return RegularisationScope.Risque;
        }

        public static decimal ComputeCotisationRetenues(decimal cotisiationsEmises, decimal pourcentage) {
            return (cotisiationsEmises / 100M) * pourcentage;
        }

        /// <summary>
        /// Validates one ore more Risques according to the RegulType
        /// </summary>
        /// <param name="context"></param>
        public static string ValidateRisques(RegularisationContext context) {
            if (context.Scope == RegularisationScope.Contrat) {
                return RegularisationRepository.ValidateRisquesContrat(context);
            }
            else if (context.Scope == RegularisationScope.Risque) {
                return RegularisationRepository.ValidateSingleRisque(context);
            }

            return "Aucune action n'a été effectuée";
        }

        public static ListeGarantiesRCDto GetGarantiesRCGroup(RegularisationContext context) {
            var baseList = RegularisationRepository.GetGarantiesRCGroup(new GarantieFilter {
                RgId = context.RgId,
                RsqNum = (int)context.RsqId,
                GrId = context.RgGrId
            }).ToList();
            
            baseList.AddRange(RegularisationRepository.GetGarantiesRCGroupHisto(new GarantieFilter
            {
                RgId = context.RgId,
                RsqNum = (int)context.RsqId,
                GrId = context.RgGrId
            }));

            if (context != null) {
                var paramValues = context.KeyValues.Last().ToParamDictionary();

                if (context.IsReadOnlyMode) {
                    baseList.ToList().ForEach(g => g.IsReadOnlyRCUS = true);
                }
                else {
                    if (paramValues[AlbParameterName.REGULMOD.ToString()].ParseCode<RegularisationMode>() != RegularisationMode.Standard) {
                        baseList.ToList().ForEach(g => g.IsReadOnlyRCUS = false);
                    }
                }
            }

            var list = new ListeGarantiesRCDto();
            var rcfr = baseList.First(rc => rc.CodeGarantie == AlbOpConstants.RCFrance);
            var rcus = baseList.FirstOrDefault(rc => rc.CodeGarantie == AlbOpConstants.RCUSA);
            var rcex = baseList.FirstOrDefault(rc => rc.CodeGarantie == AlbOpConstants.RCExport);

            list.AddRCFR(rcfr);

            bool addRCEX = true;
            bool addRCUS = true;
            if (rcex == null || rcus == null) {
                List<string> otherRC = RegularisationRepository.FindOtherRCThanFR(context.LotId, (int)context.RsqId);
                addRCEX = rcex != null || otherRC.Contains(AlbOpConstants.RCExport);
                addRCUS = rcus != null || otherRC.Contains(AlbOpConstants.RCUSA);
            }

            if (addRCEX) {
                list.AddRC(rcex, AlbOpConstants.RCExport);
            }

            if (addRCUS) {
                list.AddRC(rcus, AlbOpConstants.RCUSA);
            }

            bool isFirstAccess = RegularisationRepository.IsFirstAccess(context.IdContrat.CodeOffre);
            list = ComputeGarantiesRC(context, list, isFirstAccess);
            var garRCEX = (rcex != null) ? list.Garanties.Find(g => g.Definitif.Label == rcex.Libelle) : list.Garanties.Find(g => g.Definitif.Label == AlbOpConstants.RCExport);
            var garRCFR = (rcfr != null) ? list.Garanties.Find(g => g.Definitif.Label == rcfr.Libelle) : list.Garanties.Find(g => g.Definitif.Label == AlbOpConstants.RCFrance);

            if (garRCEX != null && (garRCEX.Definitif.BasicValues.TauxMontant == 0
                || string.IsNullOrEmpty(garRCEX.Definitif.BasicValues.Unite.Code))) {
                list.Garanties.Where(g => g.Definitif.Label == garRCEX.Definitif.Label).ToList().ForEach(g => {
                    g.Definitif.BasicValues.Unite = garRCFR.Previsionnel.BasicValues.Unite;
                    g.Definitif.BasicValues.TauxMontant = garRCFR.Previsionnel.BasicValues.TauxMontant;
                    g.Definitif.BasicValues.CodeTaxes.Code = "0";
                });
            }

            return list;
        }

        public static RegularisationGarInfoDto GetInfoRegularisationGarantie(RegularisationContext context) {
            var garantie = FormuleRepository.GetInfoGarantieById(context.GrId.ToString());
            context.IsMultiRC = garantie.CodeGarantie == AlbOpConstants.RCFrance && RegularisationRepository.HasGarantieRCGrouped(new GarantieFilter { LotId = context.LotId, RgId = context.RgId, RsqNum = (int)context.RsqId });
            RegularisationGarInfoDto garInfo = RegularisationRepository.GetInfoRegularisationGarantie(
                garantie,
                context.IsMultiRC,
                context.IdContrat.CodeOffre,
                context.IdContrat.Version.ToString(),
                context.IdContrat.Type,
                context.ModeleAvtRegul.NumAvt.ToString(),
                context.LotId.ToString(),
                context.RsqId.ToString(),
                context.RgId.ToString(),
                context.IsReadOnlyMode);

            if (context.IsMultiRC) {
                foreach (var p in garInfo.ListPeriodRegulGar) {
                    IncludeGarantieInRCFRGroup(
                        new GarantieFilter {
                            RgId = context.RgId,
                            RsqNum = (int)context.RsqId,
                            DateDebut = ((int)p.PeriodeRegulDeb).YMDToDate().Value,
                            DateFin = ((int)p.PeriodeRegulFin).YMDToDate().Value
                        },
                        p);
                }
            }

            return garInfo;
        }

        public static ListeGarantiesRCDto ComputeGarantiesRC(RegularisationContext context, ListeGarantiesRCDto list, bool isFirstAccess) {
            foreach (var g in list.Garanties) {
                if (isFirstAccess) {
                    RegularisationRepository.UpdateInfosCotisRegulGarantie(g);
                }
                g.Definitif.CalculError = string.Empty;
                g.RegulCalcule = g.ComputeRegulAmount();
            }

            if (!list.Garanties.Any(x => x.Definitif.BasicValues.IsValid)) {
                list.Garanties.ForEach(g => {
                    g.Definitif.CalculError = string.Empty;
                    g.Definitif.CalculAssiette = 0;
                    g.Definitif.CalculTaxesAssiette = 0;
                });
            }
            else {
                foreach (var g in list.Garanties.Where(x => x.Definitif.BasicValues.IsValid)) {
                    if (g.Previsionnel.BasicValues.IsValidCompute) {
                        var calculationPrev = RegularisationRepository.ComputeGarantieFigures(context, g.Previsionnel.BasicValues);
                        g.Previsionnel.CalculAssiette = calculationPrev.amount;
                        g.Previsionnel.CalculTaxesAssiette = calculationPrev.tax;
                    }

                    if (g.Definitif.BasicValues.IsValidCompute) {
                        var calculationDef = RegularisationRepository.ComputeGarantieFigures(context, g.Definitif.BasicValues);
                        if (calculationDef.error.IsEmptyOrNull()) {
                            g.Definitif.CalculError = string.Empty;
                            g.Definitif.CalculAssiette = calculationDef.amount;
                            g.Definitif.CalculTaxesAssiette = calculationDef.tax;
                        }
                        else {
                            g.Definitif.CalculError = calculationDef.error;
                            g.Definitif.CalculAssiette = default;
                            g.Definitif.CalculTaxesAssiette = default;
                        }
                    }
                    else {
                        g.Definitif.CalculError = string.Empty;
                        g.Definitif.CalculAssiette = default;
                        g.Definitif.CalculTaxesAssiette = default;
                    }
                    g.RegulCalcule = g.ComputeRegulAmount();
                }
            }
            if (!isFirstAccess) {
                list.ComputingRefresh();
            }
            list.FirstAccess = isFirstAccess;
            return list;
        }

        public static string ValidateRegulGarantieRC(RegularisationContext context, ListeGarantiesRCDto list) {
            if (context.IsReadOnlyMode) {
                return string.Empty;
            }

            var errors = new List<string>();
            foreach (var g in list.Garanties) {
                (string error, double taxeForcee) = ComputeMontantTxForce(context, g.RegulForcee);
                if (error.IsEmptyOrNull()) {
                    RegularisationRepository.UpdateInfosCotisRegulGarantieForValidation(g, taxeForcee);
                    g.Definitif.CalculError = string.Empty;
                }
                else {
                    errors.Add(error);
                }
            }

            if (errors.Any()) {
                return string.Join(Environment.NewLine, errors);
            }
            else {
                RegularisationRepository.ValidateRisqueForGarantie((int)context.RsqId, list.Garanties.First().Definitif.Id);
            }

            return string.Empty;
        }

        public static List<LigneMouvtGarantieDto> SupprimerMouvtPeriod(RegularisationContext context) {
            bool isMultiRC = RegularisationRepository.HasGarantieRCGrouped(new GarantieFilter {
                LotId = context.LotId,
                RgId = context.RgId,
                RsqNum = (int)context.RsqId
            });

            var result = RegularisationRepository.SupprimerMouvtPeriod(
                context.IdContrat.CodeOffre,
                context.IdContrat.Version.ToString(),
                context.IdContrat.Type,
                context.RsqId.ToString(),
                context.CodeFormule.ToString(),
                context.GrId.ToString(),
                context.RgId.ToString(),
                context.RgGrId.ToString(),
                isMultiRC);

            if (isMultiRC) {
                result?.ForEach(m => m.IdRCFR = context.GrId);
            }

            return result;
        }

        internal static RegularisationComputeData ComputeCotisation(RegularisationComputeData computeData) {
            computeData.ComputeCotisationsRetenues();
            computeData.ComputeMontant();
            computeData.UpdateLabelMontant();
            return computeData;
        }

        internal IEnumerable<RegularisationStateDto> GetCurrentStates(long id) {
            return this.repository.GetCurrentStates(id);
        }

        /// <summary>
        /// Fetches regul data matrix if it's not already done
        /// </summary>
        /// <param name="context">The target instance</param>
        internal void InitMatrix(RegularisationContext context) {
            if (context.LotId == 0) {
                context.Matrix = this.repository.GetMatrice(context.IdContrat.ToFolder()).ToList();
            }
            else {
                context.Matrix = this.repository.GetTempMatrice(
                    context.IdContrat.ToFolder(),
                    context.LotId,
                    context.RgId).ToList();
            }
        }

        /// <summary>
        /// Tries to reference the RCFR identifier
        /// </summary>
        /// <param name="filter">The search filter</param>
        /// <param name="group">Garantie list</param>
        /// <param name="rcid">Either KDEID or KHXID</param>
        internal static void TryIncludeGarantiesInRCFRGroup(GarantieFilter filter, IEnumerable<IRCFRGroupItem> group, long rcid = 0) {
            if (group?.Any(item => item != null) != true || filter == null) {
                return;
            }

            if (RegularisationRepository.HasGarantieRCGrouped(filter)) {
                if (rcid == 0) {
                    rcid = RegularisationRepository.FindGarantieRCFRId(filter);
                }

                foreach (var item in group.Where(i => i != null)) {
                    IncludeGarantieInRCFRGroup(filter, item, rcid);
                }
            }
        }

        internal static void IncludeGarantieInRCFRGroup(GarantieFilter filter, IRCFRGroupItem item, long rcid = 0) {
            if (rcid == 0) {
                rcid = RegularisationRepository.FindGarantieRCFRId(filter);
            }

            item.IdRCFR = rcid;
        }

        internal static (string error, double taxe) ComputeMontantTxForce(RegularisationContext context, double montantForce) {
            var result = RegularisationRepository.ComputeGarantieFigures(
                context,
                new GarantieValuesDto {
                    Assiette = 0,
                    TauxMontant = montantForce,
                    Unite = new UniteTauxMontantDto { Code = "D" },
                    CodeTaxes = new CodeTaxesDto { Code = string.Empty }
                });

            return (result.error, result.tax);
        }

        public void EnsureScope(RegularisationContext context) {
            context.Refresh();
            if (context.NbRisques != 1) {
                if (context.AccessMode == AccessMode.CREATE) {
                    if (IsRisquesHomogenes(context.IdContrat.ToFolder(), context.LotId)) {
                        context.Scope = RegularisationScope.Contrat;
                    }
                    else {
                        context.Scope = RegularisationScope.Risque;
                    }
                }
                else {
                    context.Scope = this.repository.GetCurrentScope(context.RgId);
                }
            }
        }

        public RegularisationRsqDto CreateOrUpdate(RegularisationContext context) {
            string mode = context.AccessMode.ToString();
            var dto = new RegularisationRsqDto { ReguleId = context.RgId };
            if (!string.IsNullOrEmpty(mode)) {
                if (!IsAtLeastRegRiskExists(context.IdContrat.ToFolder(), context.LotId, context.Mode)) {
                    dto.ErreurAvt = "Aucun risque régularisable dans cette affaire.";
                    dto.CodeErreurAvt = "NORISKREGUL";
                }
                else if (!IsAtLeastRegGarExists(context.IdContrat.ToFolder(), context.LotId, context.Mode)) {
                    dto.ErreurAvt = "Aucune garantie régularisable dans cette affaire.";
                    dto.CodeErreurAvt = "NOGARREGUL";
                }

                if (dto.ErreurAvt.IsEmptyOrNull()) {
                    context.ModeleAvtRegul.HasHisto = context.RgHisto == 'O';
                    DateTime? dateEffet = context.TypeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF ? AlbConvert.ConvertStrToDate(context.DateFin) : null;
                    context.ModeleAvtRegul.DateFin = dateEffet.HasValue ? dateEffet.Value.AddDays(1) : default(DateTime?);
                    var folder = context.IdContrat.ToFolder();
                    folder.NumeroAvenant = Convert.ToInt32(context.ModeleAvtRegul.NumAvt);
                    try {
                        var histoService = new HistorizationService();
                        histoService.Archive(this.dbConnection, new HistorizationContext(context.ModeleAvtRegul) {
                            Folder = folder,
                            User = context.User,
                            Mode = context.AccessMode.IsIn(AccessMode.CREATE, AccessMode.UPDATE) ? context.AccessMode.ToString()[0] : default
                        });
                    }
                    catch (HistorizationException histoEx) {
                        dto.ErreurAvt = string.Join(Environment.NewLine, histoEx.Errors);
                    }
                    if (context.ModeleAvtRegul.TypeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF) {
                        this.cibleService.UpdateSousBranche(folder, context.User);
                        this.repository.UpdateAvenantNumDate(folder, context.ModeleAvtRegul.DateFin);
                    }
                }

                if (dto.ErreurAvt.ContainsChars()) {
                    if (dto.CodeErreurAvt.IsEmptyOrNull()) {
                        dto.CodeErreurAvt = "CREATEAVNREGUL";
                    }
                    return dto;
                }

                switch (context.Mode) {
                    case RegularisationMode.PB:
                    case RegularisationMode.Burner:
                        EnsureScope(context);
                        break;
                    case RegularisationMode.BNS:
                        context.Scope = RegularisationScope.Contrat;
                        break;
                }

                dto.ReguleId = SetRegulListes(context);
            }

            return dto;
        }

        private long SetRegulListes(RegularisationContext context) {
            switch (context.Mode) {
                case RegularisationMode.Standard:
                    if (!context.IsSetReguleAlreadyCalled && context.AccessMode != AccessMode.CONSULT) {
                        var dto = this.regularisationService.AddOrUpdateWithRsq(context);
                        context.RgId = dto.Id;
                    }
                    return context.RgId;
                case RegularisationMode.PB:
                    SetRegulPBListes(context);
                    break;
                case RegularisationMode.BNS:
                    this.repository.SetListRsqBNSRegule(context);
                    break;
                case RegularisationMode.Burner:
                    this.repository.SetListRsqBurnerRegule(context);
                    break;
                default:
                    return 0;
            }
            return context.RgId;
        }

        private void SetRegulPBListes(RegularisationContext context) {
            this.repository.SetListRsqPBRegule(context);
            if (context.IdContrat.CodeOffre.ToUpper().StartsWith("TR")) {
                ReportChargePBTR(context);
            }
        }

        private void ReportChargePBTR(RegularisationContext context) {
            if (this.repository.GetNbReportCharge(context.IdContrat.ToFolder(), "A") > 0) {
                this.repository.ModifyReportCharge(context.IdContrat.ToFolder(), "A", context.RgId);
            }
            else if (this.repository.GetNbReportCharge(context.IdContrat.ToFolder(), "V") > 0) {
                this.repository.ModifyReportCharge(context.IdContrat.ToFolder(), "V", context.RgId);
            }
        }

        private bool IsAtLeastRegRiskExists(Folder folder, long lotId, RegularisationMode mode) {
            if (mode.IsIn(RegularisationMode.PB, RegularisationMode.BNS)) {
                // no check for BNS / PB
                return true;
            }
            return this.repository.GetNbRegRisques(mode, folder, lotId) > 0;
        }

        /// <summary>
        /// Verifie l'existance d'une garantie du même type que la régul KGARTRG {KIHTRG} {PB --> 'PB'; BNS --> 'BN'; BURNER --> 'BU'}
        /// </summary>
        /// <param name="folder">Code Affaire</param>
        /// <param name="lotId">N Lot</param>
        /// <param name="mode">PB,BNS,BURNER, etc.</param>
        /// <returns>true si une garantie existe et est de meme type que regulMode</returns>
        private bool IsAtLeastRegGarExists(Folder folder, long lotId, RegularisationMode mode) {
            if (mode.IsIn(RegularisationMode.PB, RegularisationMode.BNS))
            {
                // no check for BNS / PB
                return true;
            }
            return this.repository.GetNbRegGaranties(mode, folder, lotId) > 0;
        }

        internal (bool ok, int nbPeriodes) CanSimplifyStepFlow(RegularisationContext context) {
            if (context != null && (context.ModeleAvtRegul == null || context.ModeleAvtRegul.MotifAvt == null)) {
                context.ModeleAvtRegul = new AvenantRegularisationDto { MotifAvt = RegularisationRepository.GetMotifRegularisation(context.RgId) };
            }
            long nbRisques = this.repository.GetNbRegRisques(context.Mode, context.IdContrat.ToFolder(), context.LotId);
            long nbGaranties = this.repository.GetNbRegGaranties(context.Mode, context.IdContrat.ToFolder(), context.LotId);
            int nbPrdGaranties = this.repository.GetNbRegPeriodesGaranties(context.Mode, context.RgId);

            return (nbRisques == 1 && nbGaranties == 1, nbPrdGaranties);
        }

        internal void InitializeSimplifiedStepFlow(RegularisationContext context) {
            var regulsSimples = this.regularisationService.GetSelectionGaranties(context.LotId);
            var isMultiRC = regulsSimples.Any(x => x.CodeGarantie == AlbOpConstants.RCFrance)
                && regulsSimples.Count() > 1
                && regulsSimples.All(x => x.IsRCGarantie)
                && regulsSimples.Select(x => x.NumeroFormule).Distinct().Count() == 1;

            // assumes there is only one Risque
            InitializeSingleRsqStepFlow(context, regulsSimples);

            var periodesGaranties = this.regularisationService.GetPeriodesGaranties(context.RgId);
            context.IsSimplifiedRegule = periodesGaranties.Count() == 1;
            if (context.IsSimplifiedRegule) {
                context.RgGrId = periodesGaranties.First(x => !isMultiRC || x.CodeGarantie == AlbOpConstants.RCFrance).Id;
            }
        }

        private void InitializeSingleRsqStepFlow(RegularisationContext context, IEnumerable<SelectionRegularisationDto> regulsSimples) {
            var formuleGroup = regulsSimples.GroupBy(x => x.NumeroFormule);
            context.IsSimplifiedRegule = formuleGroup.Count() == 1;
            formuleGroup.ToList().ForEach(g => {
                context.IsMultiRC = g.Any(x => x.CodeGarantie == AlbOpConstants.RCFrance)
                    && g.Count() > 1
                    && g.All(x => x.IsRCGarantie);

                context.HasMultiRC = g.Any(x => x.CodeGarantie == AlbOpConstants.RCFrance)
                    && g.Any(x => x.IsRCGarantie && x.CodeGarantie != AlbOpConstants.RCFrance);

                var regulSimple = g.First();
                var rgRCFR = g.FirstOrDefault(x => x.CodeGarantie == AlbOpConstants.RCFrance);
                var affaireId = regulSimple.AffaireId;
                var (debut, fin) = this.regularisationService.GetPeriodBounds(affaireId, context.RgId);
                var errors = new List<string>();
                var list = g.ToList();
                context.RsqId = regulSimple.NumeroRisque;
                context.CodeFormule = regulSimple.NumeroFormule;
                context.GrId = context.IsMultiRC && context.IsSimplifiedRegule ? rgRCFR.IdGarantie : 0;

                if (!context.IsReadOnlyMode) {
                    list.ForEach(x =>
                        errors.Add(this.as400Repository.ApplyMouvementsGarantie(
                            new PGMFolder(affaireId.Adapt<Folder>()) { User = context.User, ActeGestion = context.TypeAvt },
                            x.NumeroRisque, x.NumeroFormule, x.CodeGarantie,
                            (x.DateDebut < debut ? debut : x.DateDebut).ToIntYMD(),
                            (!x.DateFin.HasValue || x.DateFin > fin ? fin : x.DateFin.Value).ToIntYMD())));

                    if (errors.Any(e => e.ContainsChars())) {
                        throw new BusinessValidationException(errors.Where(e => e.ContainsChars()).Select(e => new ValidationError(e)));
                    }

                    if (context.HasMultiRC) {
                    
                    this.regularisationService.EnsureTempPeriodesRC(
                        rgRCFR.AffaireId,
                        context.RgId,
                        rgRCFR.NumeroRisque,
                        rgRCFR.NumeroFormule,
                        g.Select(k => k.IdGarantie).ToArray(),
                        context.AccessMode);

                        this.as400Repository.ApplyMouvementsGarantieRC(
                            new PGMFolder(affaireId.Adapt<Folder>()) { User = context.User, ActeGestion = context.TypeAvt },
                            rgRCFR.NumeroRisque, rgRCFR.NumeroFormule,
                            (rgRCFR.DateDebut < debut ? debut : rgRCFR.DateDebut).ToIntYMD(),
                            (!rgRCFR.DateFin.HasValue || rgRCFR.DateFin > fin ? fin : rgRCFR.DateFin.Value).ToIntYMD());

                        bool noGrId = context.GrId == 0;
                        context.GrId = rgRCFR.IdGarantie;
                        this.repository.EnsureMouvementGarantieRCPeriod(context);
                        if (noGrId) {
                            context.GrId = 0;
                        }
                    }

                    g.Where(x => !context.HasMultiRC || !x.IsRCGarantie).ToList().ForEach(x => {
                        context.GrId = x.IdGarantie;
                        this.repository.EnsureMouvementGarantiePeriod(context);
                    });
                }
            });
            if (formuleGroup.Count() > 1) {
                context.IsMultiRC = false;
                context.CodeFormule = 0;
                context.GrId = 0;
            }
        }

        private void InitializeSingleGarStepFlow(RegularisationContext context, IEnumerable<SelectionRegularisationDto> regulsSimples) {
            var regulSimple = regulsSimples.Any(x => x.IsRCGarantie) ? regulsSimples.First(x => x.CodeGarantie == AlbOpConstants.RCFrance) : regulsSimples.First();
            var affaireId = regulsSimples.First().AffaireId;
            var (debut, fin) = this.regularisationService.GetPeriodBounds(affaireId, context.RgId);
            var errors = new List<string>();
            context.IsSimplifiedRegule = true;
            context.SimpleRegule = new RegularisationSimplifieeDto {
                CodeGar = regulSimple.CodeGarantie,
                IdGar = regulSimple.IdGarantie,
                IdRsq = regulSimple.NumeroRisque,
                IsGarRC = regulSimple.IsRCGarantie,
                LotId = regulSimple.IdLot,
                SequenceGar = regulSimple.SequenceGarantie
            };
            context.RsqId = regulSimple.NumeroRisque;
            context.GrId = regulSimple.IdGarantie;
            context.CodeFormule = regulSimple.NumeroFormule;
            context.IsMultiRC = regulsSimples.Count(x => x.IsRCGarantie) > 1;
            if (!context.IsReadOnlyMode) {
                if (context.IsMultiRC) {
                    regulsSimples.ToList().ForEach(x =>
                        errors.Add(this.as400Repository.ApplyMouvementsGarantie(
                            new PGMFolder(affaireId.Adapt<Folder>()) { User = context.User, ActeGestion = context.TypeAvt },
                            x.NumeroRisque, x.NumeroFormule, x.CodeGarantie,
                            (x.DateDebut < debut ? debut : x.DateDebut).ToIntYMD(),
                            (!x.DateFin.HasValue || x.DateFin > fin ? fin : x.DateFin.Value).ToIntYMD())));

                    if (!errors.Any()) {
                        this.regularisationService.EnsureTempPeriodesRC(
                            regulSimple.AffaireId,
                            context.RgId,
                            regulSimple.NumeroRisque,
                            regulSimple.NumeroFormule,
                            regulsSimples.Select(x => x.IdGarantie).ToArray(),
                            context.AccessMode);

                        this.as400Repository.ApplyMouvementsGarantieRC(
                          new PGMFolder(affaireId.Adapt<Folder>()) { User = context.User, ActeGestion = context.TypeAvt },
                          regulSimple.NumeroRisque, regulSimple.NumeroFormule,
                          (regulSimple.DateDebut < debut ? debut : regulSimple.DateDebut).ToIntYMD(),
                          (!regulSimple.DateFin.HasValue || regulSimple.DateFin > fin ? fin : regulSimple.DateFin.Value).ToIntYMD());
                    }
                }
                else {
                    errors.Add(this.as400Repository.ApplyMouvementsGarantie(
                        new PGMFolder(affaireId.Adapt<Folder>()) { User = context.User, ActeGestion = context.TypeAvt },
                        regulSimple.NumeroRisque, regulSimple.NumeroFormule, regulSimple.CodeGarantie,
                        (regulSimple.DateDebut < debut ? debut : regulSimple.DateDebut).ToIntYMD(),
                        (!regulSimple.DateFin.HasValue || regulSimple.DateFin > fin ? fin : regulSimple.DateFin.Value).ToIntYMD()));

                    if (!errors.Any()) {
                        this.regularisationService.EnsureTempPeriodes(
                            regulSimple.AffaireId,
                            context.RgId,
                            regulSimple.NumeroRisque,
                            regulSimple.NumeroFormule,
                            regulSimple.IdGarantie,
                            regulSimple.CodeGarantie,
                            context.AccessMode);
                    }
                }
                if (errors.Any(e => e.ContainsChars())) {
                    throw new BusinessValidationException(errors.Where(e => e.ContainsChars()).Select(e => new ValidationError(e)));
                }

                if (context.IsMultiRC) {
                    this.repository.EnsureMouvementGarantieRCPeriod(context);
                }
            }

            if (!context.IsMultiRC) {
                this.repository.EnsureMouvementGarantiePeriod(context);
            }
        }
    }
}
