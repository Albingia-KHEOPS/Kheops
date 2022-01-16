using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.DataAccess {
    public class CourtierRepository : RepositoryBase {
        internal static readonly string CountByIPB = "SELECT COUNT(*) FROM YPOCOUR WHERE PFIPB = :IPB AND PFALX = :ALX";

        public CourtierRepository(IDbConnection connection) : base(connection) { }

        public int GetNbCourtrier(Folder folder, ModeConsultation mode) {
            try {
                SetHistoryMode(mode == ModeConsultation.Historique ? ActivityMode.Active : 0);
                using (var options = new DbCountOptions(this.connection == null) {
                    DbConnection = this.connection,
                    SqlText = FormatQuery(CountByIPB) + (mode == ModeConsultation.Standard ? " AND PFTYP = :TYP ;" : " ;")
                }) {
                    if (mode == ModeConsultation.Standard) {
                        options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version, folder.Type);
                    }
                    else {
                        options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version);
                    }
                    options.PerformCount();
                    return options.Count;
                }
            }
            finally {
                ResetHistoryMode();
            }
        }
    }
}
