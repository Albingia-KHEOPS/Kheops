using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.DataAccess;
using OP.WSAS400.DTO;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Consts = ALBINGIA.Framework.Common.Constants.AlbConstantesMetiers;

namespace OP.Services.Historization {
    /// <summary>
    /// Defines a class to save data and perform treatments before changing to Avenant
    /// </summary>
    public abstract partial class Archiver {
        protected readonly HistorizationContext context;
        protected readonly HistoriqueRepository repository;
        protected readonly AffaireNouvelleRepository affRepository;
        protected readonly ControleFinRepository ctrlRepository;
        protected readonly ProgramAS400Repository pgm400Repository;
        protected readonly TraceRepository traceRepository;
        protected HistorizationState state;
        protected HistorizationSteps currentStep;

        protected Archiver(IDbConnection connection, HistorizationContext context) {
            this.context = context;
            if (this.context == null) {
                throw new ArgumentNullException($"{nameof(context)}");
            }
            if (context.TypeAvenant.IsEmptyOrNull()) {
                throw new ArgumentException("The current context avenant Type must have a value", $"{nameof(context)}.{nameof(context.TypeAvenant)}");
            }
            this.currentStep = 0;
            this.repository = new HistoriqueRepository(connection, context);
            this.affRepository = new AffaireNouvelleRepository(connection);
            this.ctrlRepository = new ControleFinRepository(connection);
            this.pgm400Repository = new ProgramAS400Repository(connection);
            this.traceRepository = new TraceRepository(connection);
            InitState();
        }

        internal static Archiver Build(IDbConnection connection, HistorizationContext context) {
            if (context.IsModifHorsAvenant) {
                if (context.Mode == 'U') {
                    return new ArchiverUpdateHorsAvn(connection, context);
                }
            }
            else if (context.IsArchived) {
                if (context.Mode == 'C') {
                    return new ArchiverCreation(connection, context);
                }
                else if (context.Mode == 'U') {
                    return new ArchiverUpdate(connection, context);
                }
            }
            else {
                return new ArchiverHistoLess(connection, context);
            }

            throw new ArgumentException("The value of Mode is invalid. Valid values: C, U");
        }

        internal virtual void PerformProcess() {
            bool succeeded = false;
            try {
                if (SetStep(HistorizationSteps.CheckFolderState)) {
                    var folder = this.affRepository.GetFolderLight(this.context.Folder);
                    if (folder.Eta != Consts.ACTEGESTION_VALIDATION) {
                        throw new HistorizationException(new[] { "L'historique ne peut pas être créé sur un contrat non validé" });
                    }
                }
                if (SetStep(HistorizationSteps.CheckHistoAlreadyDone)) {
                    if (this.repository.IsHistoAlreadyDone()) {
                        throw new HistorizationException(new[] { "Historique déjà créé" });
                    }
                }
                if (SetStep(HistorizationSteps.ResetArchive)) {
                    this.repository.PurgeTraces();
                    this.repository.PurgeIds();
                }
                if (SetStep(HistorizationSteps.ArchiveAffaire)) {
                    ArchiveAll();
                }
                if (SetStep(HistorizationSteps.ArchiveExtraTables)) {
                    this.repository.SaveDataExtraTablesKheops();
                }
                if (SetStep(HistorizationSteps.ResetClauses)) {
                    this.repository.ResetClauses();
                }
                if (SetStep(HistorizationSteps.UpdateControleEtape)) {
                    UpdateControles();
                }
                if (SetStep(HistorizationSteps.CreateOrUpdateAvenant)) {
                    UpdateAvenant();
                }
                if (SetStep(HistorizationSteps.ReleaseGaranties)) {
                    this.repository.ReleaseGaranties();
                }
                if (SetStep(HistorizationSteps.DeleteEcheanciers)) {
                    this.repository.PurgeEcheanciers();
                }
                if (SetStep(HistorizationSteps.DeleteClausesRegularisation)) {
                    this.repository.PurgeClausesRegul();
                }
                if (SetStep(HistorizationSteps.DeletePrimes)) {
                    this.repository.PurgePrimes();
                }
                if (SetStep(HistorizationSteps.DeleteTempWordDocuments)) {
                    this.repository.PurgeTempWordDocs();
                }
                succeeded = true;
            }
            catch (Exception) {
                if (!this.repository.HasItsOwnContext) {
                    throw;
                }
            }
            finally {
                if (!succeeded) {
                    this.repository.RollbackChanges();
                }
            }
        }

        /// <summary>
        /// Defines whether the step can be perfomed, if True, <see cref="currentStep" /> is assigned with step value
        /// </summary>
        /// <param name="step"></param>
        /// <returns>Returns True if the step has to be performed, else False</returns>
        protected virtual bool SetStep(HistorizationSteps step) {
            if ((this.state.Steps & step) == step) {
                this.currentStep = step;
                return true;
            }
            return false;
        }

        protected virtual void InitState() {
            this.state = repository.SetState();
            this.state.Steps |= HistorizationSteps.CheckHistoAlreadyDone | HistorizationSteps.DeleteEcheanciers;
        }

        protected virtual void UpdateState() {
            if (this.currentStep == HistorizationSteps.CreateOrUpdateAvenant)
            {
                switch (this.context.TypeAvenant)
                {
                    case Consts.TYPE_AVENANT_MODIF:
                    case Consts.TYPE_AVENANT_REGULMODIF:
                    case Consts.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                        if (this.state.SituationAffaire == 'X' && this.state.EtatAffaire == 'V') {
                            this.state.SituationAffaire = 'A';
                            this.state.DateSituation = this.context.Now;
                        }
                        var result = this.affRepository.GetProchaineEcheanceAffaireNouvelle(this.context.Folder).FirstOrDefault();
                        this.state.DateEffet = DateTime.TryParse(result.EffetGarantie, out var dt) ? dt : default(DateTime?);
                        this.state.Periodicite = result.Periodicite;
                        this.state.EcheancePrincipale = DateTime.TryParse(result.EcheancePrincipale + "/2012", out dt) ? dt : default(DateTime?);
                        this.state.DateFinEffet = this.context.DateFinGarantie.GetValueOrDefault() == default ? (DateTime.TryParse(result.FinGarantie, out dt) ? dt : default(DateTime?)) : this.context.DateFinGarantie;
                        this.state.DateRemiseVigueur = this.context.DateAvenant.GetValueOrDefault() == default ? null : this.context.DateAvenant;
                        break;
                    case Consts.TYPE_AVENANT_RESIL:
                        if (!this.context.IsModifHorsAvenant) {
                            this.state.DateResiliation = this.context.DateFinGarantie.GetValueOrDefault() == default ? null : this.context.DateFinGarantie;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void ArchiveAll() {
            SetCodeAdresse();
            Parallel.Invoke(new ParallelOptions { MaxDegreeOfParallelism = 2 },  new Action[] {
                this.repository.SaveAffaire,
                this.repository.SaveRisques,
                this.repository.SaveObjets,
                this.repository.SaveFormules,
                this.repository.SaveInventaires,
                this.repository.SaveIS,
                this.repository.SaveClauses,
                this.repository.SaveCotisations,
                this.repository.SaveControles,
                this.repository.SaveDesignations,
                this.repository.SaveEngagements,
                this.repository.SaveExpressionsComplexes,
                this.repository.SaveMatrice,
                this.repository.SaveObservations,
                this.repository.SaveDoublesSaisiesOffres,
                this.repository.SaveOppositions,
                this.repository.SaveValidations,
                this.repository.DeleteQuittancesAnnulees,
                this.repository.SaveAdresses,
                this.repository.SaveJobSortis });
        }

        private void SetCodeAdresse() {
            this.state.NewCodeAdresse = this.repository.GetCodeAdresse();
        }

        private void UpdateControles() {
            DateTime? dateAvn = affRepository.GetDateAvenant(this.context.Folder);
            if (this.context.DateAvenant.HasValue && dateAvn.HasValue && this.context.DateAvenant != dateAvn
                && this.context.TypeAvenant.IsIn(Consts.TYPE_AVENANT_MODIF, Consts.TYPE_AVENANT_REGULMODIF, Consts.TYPE_AVENANT_REMISE_EN_VIGUEUR)) {
                if (this.ctrlRepository.SelectNbCtrlModif(this.context.Folder) == 0) {
                    this.ctrlRepository.RemoveEtapeCOT(this.context.Folder);
                    this.ctrlRepository.CreateEtapeDateAvenant(this.context.Folder, this.context.User, this.context.Now);
                }
            }
        }

        private void UpdateAvenant() {
            UpdateState();
            switch (context.TypeAvenant) {
                case Consts.TYPE_AVENANT_MODIF:
                case Consts.TYPE_AVENANT_REGULMODIF:
                    UpdateAvenantModif();
                    break;
                case Consts.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                    UpdateAvenantRmVg();
                    break;
                case Consts.TYPE_AVENANT_RESIL:
                    UpdateAffaireResiliation();
                    break;
                case Consts.TYPE_AVENANT_REGUL:
                    UpdateRegul();
                    break;
                default:
                    break;
            }
        }

        private void UpdateAvenantModif() {
            if (this.context.TypeAvenant == Consts.TYPE_AVENANT_REMISE_EN_VIGUEUR)
                Parallel.Invoke(new ParallelOptions { MaxDegreeOfParallelism = 2 }, new Action[] {
                    this.repository.UpdateAffaireAvenant,
                    this.repository.UpdateObservationsAvn,
                    this.repository.UpdateKpentRemiseEnVigueur,
                    this.repository.UpdateDatesRsqObjGarAvn                
                });
            else
                Parallel.Invoke(new ParallelOptions { MaxDegreeOfParallelism = 2 }, new Action[] {
                    this.repository.UpdateAffaireAvenant,
                    this.repository.UpdateObservationsAvn,
                    this.repository.UpdateFraisAvn,
                    this.repository.UpdateDatesRsqObjGarAvn
                });

            var dataProchEch = this.pgm400Repository.LoadPreavisResiliation(new PGMFolder(this.context.Folder) { User = this.context.User, ActeGestion = this.context.TypeAvenant },
                state.DateEffet, state.DateFinEffet, context.DateAvenant, state.Periodicite, state.EcheancePrincipale, "#**#");

            if (dataProchEch.ContainsChars())
            {
                string[] tDataProchEch = dataProchEch.Split(new[] { "#**#" }, StringSplitOptions.None);
                this.affRepository.UpdateProchaineEcheanceAffaireNouvelle(
                    this.context.Folder,
                    AlbConvert.ConvertStrToDate(tDataProchEch[2]),
                    AlbConvert.ConvertStrToDate(tDataProchEch[0]),
                    AlbConvert.ConvertStrToDate(tDataProchEch[1]));
            }
        }

        private void UpdateAvenantRmVg() {
            UpdateAvenantModif();
            this.repository.AdjustSuspensions(this.context.Folder);
        }

        private void UpdateRegul() {
            this.repository.UpdateAffaireRegul();
            this.repository.UpdateObservationsAvn();
            this.repository.UpdateFraisAvn();
        }
        
        private void UpdateAffaireResiliation() {
            if (this.context.IsModifHorsAvenant) {
                if (this.state.MotifResiliation != this.context.MotifAvenant) {
                    this.repository.UpdateMotifResiliation();
                    this.traceRepository.CreateTraceYpo(this.context.Folder, $"MHA - Changement Motif ({this.state.MotifResiliation})", this.context.User, DateTime.Now, this.context.TypeAvenant, "G");
                }
            }
            else {
                this.repository.UpdateAffaireResil();
                var echeance = this.affRepository.GetProchaineEcheance(this.context.Folder);
                if (!(echeance.jdpea > 0 && echeance.jdpem > 0 && echeance.jdpej > 0))
                    return;

                if (new DateTime(echeance.jdpea, echeance.jdpem, echeance.jdpej) > state.DateResiliation) {
                    this.affRepository.CancelProchaineEcheance(this.context.Folder);
                    var periodes = this.affRepository.GetPeriodeBetween(this.context.Folder, this.state.DateResiliation.Value);
                    if (periodes?.Any() == true) {
                        var periode = periodes.First();
                        this.affRepository.CloturerExcercice(
                            this.context.Folder,
                            new DateTime(periode.jdpea, periode.jdpem, periode.jdpej),
                            this.state.DateResiliation.Value);
                    }
                }
                //this.repository.UpdateObservationsAvn();
            }
        }
    }
}
