using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using Dapper;
using DataAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class YSinistRepository {
        readonly static string select_All = $@"
SELECT CAST ( SIOUA || LPAD ( SIOUM , 2 , '0' ) || LPAD ( SIOUJ , 2 , '0' ) || '000000' AS TIMESTAMP ) DTOUV , YSINIST . * , PBAVN , PBBRA , PBEF , PBEFAV
, CAST ( SIOUA || LPAD ( SIOUM , 2 , '0' ) || LPAD ( SIOUJ , 2 , '0' ) || '000000' AS TIMESTAMP ) DTSUR
FROM YSINIST INNER JOIN ( 
    SELECT PBIPB , PBALX , PBAVN , PBBRA , PBIAS , 
    {OutilsHelper.MakeCastTimestamp("PBEF", true)} PBEF , {OutilsHelper.MakeCastTimestamp("PBAV")} PBEFAV 
    FROM YPOBASE  WHERE PBETA = 'V' 
    AND PBTYP = 'P' 
    AND PBSTA > 0 
) AS PO ON ( SIIPB , SIALX ) = ( PBIPB , PBALX ) ";

        public PagingList<YpoSinist> SelectAll(string[] branches, int page = 0, int codeAssure = 0, int nbAnnees = 3) {
            string selection = select_All;
            if (codeAssure > 0) {
                selection += $"AND {codeAssure} = PBIAS ";
            }
            if (nbAnnees > 0) {
                selection += $"AND CAST ( SISUA || LPAD ( SISUM , 2 , '0' ) || LPAD ( CASE SISUJ WHEN 0 THEN 1 ELSE SISUJ END , 2 , '0' ) || '000000' AS TIMESTAMP ) + {nbAnnees} YEARS >= NOW() ";
            }
            int fromIndex = selection.IndexOf("FROM YSINIST");
            string query = "SELECT COUNT(1) , SUM ( SIMTS )  " + selection.Substring(fromIndex);
            var c = this.connection.EnsureOpened();
            var (count, total) = c.Query<(int, string)>(query).First();
            var paging = new Paging();
            paging.Init(count, page);
            if (branches.Distinct().Count() == 1 && branches.First() == "**") {
                return new PagingList<YpoSinist> {
                    NbTotalLines = count,
                    List = c.Query<YpoSinist>(selection + $" ORDER BY 1 DESC OFFSET {paging.CurrentOffset} ROWS FETCH FIRST {paging.Size} ROWS ONLY "),
                    PageNumber = paging.CurrentPage,
                    Totals = new Dictionary<string, decimal> { { "MontantTotalObjets", count == 0 ? 0 : decimal.Parse(total, CultureInfo.InvariantCulture) } }
                };
            }
            
            query = selection + $@" 
ORDER BY CASE WHEN PBBRA IN :list THEN 0 ELSE 1 END , PBBRA , 1 DESC 
OFFSET {paging.CurrentOffset} ROWS FETCH FIRST {paging.Size} ROWS ONLY ";
            
            return new PagingList<YpoSinist> {
                NbTotalLines = count,
                List = c.Query<YpoSinist>(query, new { list = branches.ToList() }),
                PageNumber = paging.CurrentPage
            };
        }

        public IEnumerable<YSinist> SelectAllOfPreneur(int codePreneur, int nbAnnees = 3) {
            if (codePreneur < 1) {
                throw new ArgumentException(nameof(codePreneur));
            }
            string selection = $"{select_All}AND {codePreneur} = PBIAS ";
            if (nbAnnees > 0) {
                selection += $"AND CAST ( SISUA || LPAD ( SISUM , 2 , '0' ) || LPAD ( CASE SISUJ WHEN 0 THEN 1 ELSE SISUJ END , 2 , '0' ) || '000000' AS TIMESTAMP ) + {nbAnnees} YEARS >= NOW() ";
            }
            int fromIndex = selection.IndexOf("FROM YSINIST");
            string query = "SELECT YSINIST.* " + selection.Substring(fromIndex);
            var c = this.connection.EnsureOpened();
            return c.Query<YSinist>(query).ToList();
        }
    }
}
