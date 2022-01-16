using ALBINGIA.Framework.Common.Constants;
using OP.WSAS400.DTO;
using System;
using System.Data;

namespace OP.Services.Historization
{
    public class ArchiverCreation: Archiver {
        public ArchiverCreation(IDbConnection connection, HistorizationContext context) : base(connection, context) {
            if (context.Mode != 'C') {
                throw new ArgumentException("The current context mode has to be Create ('C')", nameof(context));
            }
        }

        protected override void InitState() {
            base.InitState();
            state.Steps |= HistorizationSteps.CheckFolderState
                | HistorizationSteps.ResetArchive
                | HistorizationSteps.ArchiveAffaire
                | HistorizationSteps.ArchiveExtraTables
                | HistorizationSteps.ResetClauses;

            switch (context.TypeAvenant) {
                case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                    state.Steps |= HistorizationSteps.CreateOrUpdateAvenant | HistorizationSteps.ReleaseGaranties;
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                    state.Steps |= HistorizationSteps.CreateOrUpdateAvenant;
                    break;
                default:
                    return;
            }

            state.Steps |= HistorizationSteps.DeleteClausesRegularisation
                | HistorizationSteps.DeletePrimes
                | HistorizationSteps.DeleteTempWordDocuments;
        }
    }
}
