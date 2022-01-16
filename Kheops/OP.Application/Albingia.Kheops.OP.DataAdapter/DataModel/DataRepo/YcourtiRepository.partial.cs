using ALBINGIA.Framework.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class YcourtiRepository {
        internal readonly static string SelectDelegationInspecteur = @"SELECT BUDBU , ACLUIN 
FROM YCOURTI 
LEFT JOIN YCOURTN ON TCICT = TNICT AND TNXN5 = 0 AND TNTNM = 'A' 
LEFT JOIN YBUREAU ON BUIBU = TCBUR 
LEFT JOIN YSECTEU ON ABHSEC = TCSEC 
LEFT JOIN YINSPEC ON ACLINS = ABHINS 
WHERE TCICT = :ID ";

        public (string delegation, string inspecteur) GetDelegationInspecteur(int idCourtier) {
            using (var options = new DbSelectOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = SelectDelegationInspecteur
            }) {
                options.BuildParameters(idCourtier);
                var (d, i) = options.PerformSelect<(string d, string i)>().FirstOrDefault();
                return (d ?? string.Empty, i ?? string.Empty);
            }
        }
    }
}
