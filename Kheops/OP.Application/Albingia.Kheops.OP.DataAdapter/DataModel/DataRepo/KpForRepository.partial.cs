using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Formule;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EasycomClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class KpForRepository {
        public void FullDelete(Formule formule, bool fromRisque = false) {
            using (var dbOptions = new DbSPOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = "SP_DELFORM",
                Parameters = new[] {
                    new EacParameter("P_CODEOFFRE", formule.AffaireId.CodeAffaire.ToIPB()),
                    new EacParameter("P_VERSION", DbType.Int32) { Value = formule.AffaireId.NumeroAliment },
                    new EacParameter("P_TYPE", formule.AffaireId.TypeAffaire.AsCode()),
                    new EacParameter("P_CODEFORMULE", DbType.Int32) { Value = formule.FormuleNumber },
                    new EacParameter("P_TYPEDEL", DbType.Int32) { Value = fromRisque ? "R" : "C" }
                }
            }) {
                dbOptions.ExecStoredProcedure();
            }
            Delete(new KpFor { Kdaid = formule.Id });
        }
    }
}
