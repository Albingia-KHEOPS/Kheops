using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using ALBINGIA.Framework.Common.Tools;
using Dapper;
using DataAccess.Helpers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class YPrimeRepository {
        static readonly string select_PrimesEnCours = $@"
SELECT P.* , T.* 
FROM (
    SELECT T0.* , CASE WHEN PKEH > PKEM THEN PKEH ELSE PKEM END DATEECHEANCE 
    FROM ( 
        SELECT {OutilsHelper.MakeCastTimestamp("PKEH")} PKEH , {OutilsHelper.MakeCastTimestamp("PKEM")} PKEM , 
            PKIPB , PKALX , PKIPK , 
            PO.PBAVN , PO.PBBRA , PO.PBICT , {OutilsHelper.MakeCastTimestamp("PO.PBST")} DATEVALIDATIONSIT , 
            CAST ( IFNULL(H.PBCRA,PO.PBCRA) || LPAD ( IFNULL(H.PBCRM,PO.PBCRM) , 2, '0' ) || LPAD ( IFNULL(H.PBCRJ,PO.PBCRJ) , 2, '0' ) || '000000' AS TIMESTAMP ) DATEVALIDATION ,
            IFNULL ( TNTNM , '' ) TNTNM , IFNULL ( TNINL , -1 ) TNINL , IFNULL ( TNNOM , '' ) TNNOM 
        FROM YPRIMES 
        INNER JOIN YPOBASE PO ON ( PKIPB , PKALX , PKSIT , 'V' , 'P' ) = ( PO.PBIPB , PO.PBALX , 'A' , PO.PBETA , PO.PBTYP ) AND PO.PBSTA > 0 
        LEFT JOIN YHPBASE H ON ( PO.PBIPB , PO.PBALX , 0 ) = ( H.PBIPB , H.PBALX , H.PBAVN ) 
        LEFT JOIN YCOURTN ON ( TNICT , TNTNM , TNINL , TNXN5 ) = ( PO.PBICT , 'A' , 0 , 0 ) 
    ) AS T0 
) AS T 
INNER JOIN YPRIMES P ON ( P.PKIPK , P.PKIPB , P.PKALX ) = ( T.PKIPK , T.PKIPB , T.PKALX ) AND P.PKMHT >= 0 ";

        static readonly string select_PrimesImpayes = $@"
SELECT P.* , T.* 
FROM (
    SELECT T0.* , CASE WHEN PKEH > PKEM THEN PKEH ELSE PKEM END DATEECHEANCE 
    FROM ( 
        SELECT {OutilsHelper.MakeCastTimestamp("PKEH")} PKEH , {OutilsHelper.MakeCastTimestamp("PKEM")} PKEM , 
            PKIPB , PKALX , PKIPK , 
            PO.PBAVN , PO.PBBRA , PO.PBICT , {OutilsHelper.MakeCastTimestamp("PO.PBST")} DATEVALIDATIONSIT , 
            CAST ( IFNULL(H.PBCRA,PO.PBCRA) || LPAD ( IFNULL(H.PBCRM,PO.PBCRM) , 2, '0' ) || LPAD ( IFNULL(H.PBCRJ,PO.PBCRJ) , 2, '0' ) || '000000' AS TIMESTAMP ) DATEVALIDATION ,
            IFNULL ( TNTNM , '' ) TNTNM , IFNULL ( TNINL , -1 ) TNINL , IFNULL ( TNNOM , '' ) TNNOM 
        FROM YPRIMES 
        INNER JOIN YPOBASE PO ON ( PKIPB , PKALX , 'V' , 'P' ) = ( PO.PBIPB , PO.PBALX , PO.PBETA , PO.PBTYP ) AND PO.PBSTA > 0 
        LEFT JOIN YHPBASE H ON ( PO.PBIPB , PO.PBALX , 0 ) = ( H.PBIPB , H.PBALX , H.PBAVN ) 
        LEFT JOIN YCOURTN ON ( TNICT , TNTNM , TNINL , TNXN5 ) = ( PO.PBICT , 'A' , 0 , 0 ) 
    ) AS T0 
) AS T 
INNER JOIN YPRIMES P ON ( P.PKIPK , P.PKIPB , P.PKALX ) = ( T.PKIPK , T.PKIPB , T.PKALX ) AND P.PKMHT >= 0 ";

        internal PagingList<YpoPrimeRetard> SelectImpayes(string[] branches, int page, int codeAssure) {
            string selection = select_PrimesImpayes + "WHERE P.PKRLC = '5' OR P.PKMOT IN ('X', 'Y', 'R')";
            if (codeAssure > 0) {
                selection = selection.Replace("INNER JOIN YPOBASE PO ON", $"INNER JOIN YPOBASE PO ON PO.PBIAS = {codeAssure} AND");
            }
            string query = selection.Replace("SELECT P.* , T.*", "SELECT COUNT(1) , SUM ( P.PKMHT )");
            var c = this.connection.EnsureOpened();
            // use string in stead of decimal due to dapper who returns an explicit string
            var (count, total) = c.Query<(int, string)>(query).First();
            var paging = new Paging();
            paging.Init(count, page);
            //if (branches.Distinct().Count() == 1 && branches.First() == "**") {
                return new PagingList<YpoPrimeRetard> {
                    NbTotalLines = count,
                    List = c.Query<YpoPrimeRetard>(selection + $" ORDER BY PBBRA , T.DATEECHEANCE DESC , P.PKIPB OFFSET {paging.CurrentOffset} ROWS FETCH FIRST {paging.Size} ROWS ONLY "),
                    PageNumber = paging.CurrentPage,
                    Totals = new Dictionary<string, decimal> { { "MontantTotalHT", count == 0 ? 0 : decimal.Parse(total, CultureInfo.InvariantCulture) } }
                };
            //}

            query = selection + $@" 
ORDER BY CASE WHEN PBBRA IN :list THEN 0 ELSE 1 END , PBBRA , T.DATEECHEANCE DESC , P.PKIPB 
OFFSET {paging.CurrentOffset} ROWS FETCH FIRST {paging.Size} ROWS ONLY ";

            return new PagingList<YpoPrimeRetard> {
                NbTotalLines = count,
                List = c.Query<YpoPrimeRetard>(query, new { list = branches.Distinct().ToList() }),
                PageNumber = paging.CurrentPage
            };
        }

        internal PagingList<YpoPrimeRetard> SelectRetardsPaiement(string[] branches, int page = 0, int codeAssure = 0) {
            string selection = $@"{select_PrimesEnCours} WHERE ( P.PKRLC IN ( 'P' , 'R' , '1' , '3' ) 
OR NOW() > ( T.DATEECHEANCE + ( CAST ( CASE WHEN T.PBBRA = 'RS' THEN 15 ELSE 30 END AS INTEGER ) ) DAYS ) ) ";

            if (codeAssure > 0) {
                selection = selection.Replace("INNER JOIN YPOBASE PO ON", $"INNER JOIN YPOBASE PO ON PO.PBIAS = {codeAssure} AND");
            }
            string query = selection.Replace("SELECT P.* , T.*", "SELECT COUNT(1) , SUM ( P.PKMHT )");
            var c = this.connection.EnsureOpened();
            // use string in stead of decimal due to dapper who returns an explicit string
            var (count, total) = c.Query<(int, string)>(query).First();
            var paging = new Paging();
            paging.Init(count, page);
            //if (branches.Distinct().Count() == 1 && branches.First() == "**") {
                return new PagingList<YpoPrimeRetard> {
                    NbTotalLines = count,
                    List = c.Query<YpoPrimeRetard>(selection + $" ORDER BY PBBRA , T.DATEECHEANCE DESC , P.PKIPB OFFSET {paging.CurrentOffset} ROWS FETCH FIRST {paging.Size} ROWS ONLY "),
                    PageNumber = paging.CurrentPage,
                    Totals = new Dictionary<string, decimal> { { "MontantTotalHT", count == 0 ? 0 : decimal.Parse(total, CultureInfo.InvariantCulture) } }
                };
            //}
            
            query = selection + $@" 
ORDER BY CASE WHEN PBBRA IN :list THEN 0 ELSE 1 END , T.DATEECHEANCE DESC , P.PKIPB 
OFFSET {paging.CurrentOffset} ROWS FETCH FIRST {paging.Size} ROWS ONLY ";
            
            return new PagingList<YpoPrimeRetard> {
                NbTotalLines = count,
                List = c.Query<YpoPrimeRetard>(query, new { list = branches.Distinct().ToList() }),
                PageNumber = paging.CurrentPage
            };
        }
    }
}
