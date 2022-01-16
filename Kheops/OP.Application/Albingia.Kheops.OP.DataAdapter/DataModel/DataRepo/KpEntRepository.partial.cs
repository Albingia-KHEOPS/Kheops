using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using System;
using System.Data;
using System.Data.EasycomClient;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository
{

    public  partial class KpEntRepository {
        public int SaveNewAffair(string codeOffre, int versionOffre, string user, string codeContrat, int version = 0) {
            using (var dbOptions = new DbSPOptions(this.connection == null) {
                SqlText = "SP_AFFNOUV",
                DbConnection = this.connection
            }) {
                dbOptions.Parameters = new[] {
                    new EacParameter("P_CODEOFFRE", codeOffre.ToIPB()),
                    new EacParameter("P_VERSION", DbType.Int32) { Value = versionOffre },
                    new EacParameter("P_TYPE", AlbConstantesMetiers.TYPE_OFFRE),
                    new EacParameter("P_CODECONTRAT", codeContrat.ToIPB()),
                    new EacParameter("P_VERSIONCONTRAT", DbType.Int32) { Value = version },
                    new EacParameter("P_DATESYSTEME", DateTime.Now.ToString("yyyyMMdd")),
                    new EacParameter("P_USER", user),
                    new EacParameter("P_TRAITEMENT", AlbConstantesMetiers.Traitement.Police.AsCode())
                };
                dbOptions.ExecStoredProcedure();
                int result = dbOptions.ReturnedValue;

                try {
                    dbOptions.SqlText = "SP_CANAFNO";
                    dbOptions.Parameters = new[] {
                        new EacParameter("P_CODEOFFRE", codeOffre.ToIPB()),
                        new EacParameter("P_VERSION", DbType.Int32) { Value = versionOffre }
                    };
                    dbOptions.ExecStoredProcedure();
                }
                catch (Exception ex) {
                    AlbLog.Warn($"Impossibnle de supprimer les sélection temporaires pour l'affaire nouvelle {codeContrat}{Environment.NewLine}{ex}");
                }

                return result;
            }
        }
    }
}
