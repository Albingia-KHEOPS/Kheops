using ALBINGIA.Framework.Common.Constants;
using OP.WSAS400.DTO;
using System;
using System.Data;

namespace OP.Services.Historization
{
    public class ArchiverUpdate: Archiver {
        public ArchiverUpdate(IDbConnection connection, HistorizationContext context) : base(connection, context) {
            if (context.Mode != 'U') {
                throw new ArgumentException("The current context mode has to be Update ('U')", $"{nameof(context)}.{nameof(context.Mode)}");
            }
        }

        protected override void InitState() {
            base.InitState();
            this.state.Steps |= HistorizationSteps.UpdateControleEtape;

            switch (context.TypeAvenant) {
                case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                    this.state.Steps |= HistorizationSteps.CreateOrUpdateAvenant;
                    break;
            }
        }
    }
}
