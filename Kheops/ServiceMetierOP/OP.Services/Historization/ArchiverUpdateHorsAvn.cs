using ALBINGIA.Framework.Common.Constants;
using OP.WSAS400.DTO;
using System;
using System.Data;

namespace OP.Services.Historization
{
    public class ArchiverUpdateHorsAvn : Archiver {
        public ArchiverUpdateHorsAvn(IDbConnection connection, HistorizationContext context) : base(connection, context) {
            if (context.Mode != 'U') {
                throw new ArgumentException("The current context mode has to be Update ('U')", $"{nameof(context)}.{nameof(context.Mode)}");
            }
        }

        protected override void InitState() {
            base.InitState();
            switch (this.context.TypeAvenant) {
                case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                    this.state.Steps = HistorizationSteps.CheckFolderState | HistorizationSteps.CreateOrUpdateAvenant;
                    break;
            }
        }
    }
}
