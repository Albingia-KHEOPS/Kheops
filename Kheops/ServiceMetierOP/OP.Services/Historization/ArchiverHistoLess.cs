using OP.WSAS400.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.Services.Historization {
    public class ArchiverHistoLess : Archiver {
        public ArchiverHistoLess(IDbConnection connection, HistorizationContext context) : base(connection, context) {
            if (context.Mode != 'C' && context.Mode != 'U') {
                throw new ArgumentException("The current context mode is invalid", nameof(context));
            }
        }

        protected override void InitState() {
            this.state = this.repository.SetState();
            if (this.context.Mode == 'C') {
                this.state.Steps |= HistorizationSteps.DeleteClausesRegularisation
                    | HistorizationSteps.DeletePrimes
                    | HistorizationSteps.DeleteTempWordDocuments;
            }
        }
    }
}
