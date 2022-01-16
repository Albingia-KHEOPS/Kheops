using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using OP.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OP.DataAccess {
    public partial class FormuleRepository : RepositoryBase {
        internal static readonly string SelectRsqObjFormule = @"
SELECT KABRSQ , KABDESC , KACOBJ , KACDESC 
FROM KPOPTAP 
INNER JOIN KPRSQ ON KDDTYP = KABTYP AND KDDIPB = KABIPB AND KDDALX = KABALX AND KDDRSQ = KABRSQ 
INNER JOIN KPOBJ ON KDDTYP = KACTYP AND KDDIPB = KACIPB AND KDDALX = KACALX AND KDDRSQ = KACRSQ AND (KDDOBJ = KACOBJ OR KDDOBJ = 0) 
WHERE KDDIPB = :codeContrat AND KDDALX = :version AND KDDTYP = :type AND KDDFOR = :codeFor ;";
        internal static readonly string SelectDateFinRisque = @"SELECT 
CAST ( LPAD ( JGVFA , 4 , '0' ) || LPAD ( JGVFM , 2 , '0' ) || LPAD ( JGVFJ , 2 , '0' ) || '000000' AS TIMESTAMP ) DATEFINRSQ 
FROM KPOPTAP 
INNER JOIN YPRTOBJ ON JGIPB = KDDIPB AND JGRSQ = KDDRSQ AND JGOBJ = KDDOBJ 
WHERE KDDIPB = :IPB AND KDDRSQ = :NUMRSQ AND KDDFOR = :NUMFOR ;";
        internal static readonly string SelectOptionsFormulesAp = @"
SELECT KDDIPB IPB , KDDALX ALX , KDDTYP TYP , KDDRSQ RSQ , KDDOBJ OBJ , KDDFOR FOR , KDADESC LIBFOR , KDDOPT OPT , KABDESC LIBRSQ , IFNULL(KACDESC,'') LIBOBJ
FROM KPOPTAP 
INNER JOIN KPFOR ON ( KDDFOR , KDDIPB , KDDALX , KDDTYP ) = ( KDAFOR , KDAIPB , KDAALX , KDATYP )
AND ( KDDIPB , KDDALX , KDDTYP ) = ( :codeContrat , :version , :type ) 
INNER JOIN KPRSQ ON ( KDDIPB , KDDALX , KDDTYP , KDDRSQ ) = ( KABIPB , KABALX , KABTYP , KABRSQ ) 
LEFT JOIN KPOBJ ON ( KDDIPB , KDDALX , KDDTYP , KDDRSQ , KDDOBJ ) = ( KACIPB , KACALX , KACTYP , KACRSQ , KACOBJ ) ;";

        public FormuleRepository(IDbConnection connection) : base(connection) { }

        public IEnumerable<(int kabrsq, string kabdesc, int kacobj, string kacdesc)> GetRisquesObjets(Folder folder, int codeFormule) {
            return Fetch<(int, string, int, string)>(SelectRsqObjFormule, folder.CodeOffre.ToIPB(), folder.Version, folder.Type, codeFormule);
        }

        public DateTime? GetDateFinRisque(string codeAffaire, int numRisque, int numFormule) {
            var list = Fetch<DateTime?>(SelectDateFinRisque, codeAffaire.ToIPB(), numRisque, numFormule);
            return list.FirstOrDefault(x => x.HasValue);
        }

        public IEnumerable<OptionsFormulesAppData> GetOptionsFormulesAp(Folder folder) {
            return Fetch<OptionsFormulesAppData>(SelectOptionsFormulesAp, folder.CodeOffre.ToIPB(), folder.Version, folder.Type);
        }
    }
}
