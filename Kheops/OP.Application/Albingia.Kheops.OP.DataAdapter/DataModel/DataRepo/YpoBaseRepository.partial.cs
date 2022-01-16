using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class YpoBaseRepository {
        const string select_OffresByGestOrSous = @"
SELECT PBCRA , PBCRM , PBCRJ , PBBRA , PBIPB , PBALX , IFNULL(PBREF, '') PBREF , PBSTA , PBSTM , PBSTJ , PBSOU , PBGES , PBRLD , PBICT , IFNULL(KAAIPB, PBIPB) KAAIPB , IFNULL(KAAALX, PBIPB) KAAALX , IFNULL(KAAATTDOC, '') KAAATTDOC FROM YPOBASE 
LEFT JOIN KPENT ON ( PBIPB , PBALX ) = ( KAAIPB , KAAALX ) 
WHERE PBTYP = 'O' AND PBETA = 'V' AND PBSIT = 'A' 
AND PBREL = 'O'
AND CAST ( PBSTA || LPAD(PBSTM , 2 , '0' ) || LPAD(PBSTJ , 2 , '0') || '000000' AS TIMESTAMP ) + PBRLD DAYS <= NOW() 
AND ( PBGES = :g OR PBSOU = :s ) 
AND CAST ( PBSTA || LPAD(PBSTM , 2 , '0' ) || LPAD(PBSTJ , 2 , '0') || '000000' AS TIMESTAMP ) >= ( NOW() - :months MONTHS ) ";

        const string select_many = "SELECT * FROM YPOBASE WHERE ( PBIPB , PBALX ) IN ( VALUES <list> )";

        public PagingList<(YpoBase pobase, KpEnt kpent)> GetRelancesByAny(string gestionnaire, string souscripteur, int page = 0, int periodInMonths = 12) {
#if DEBUG
            gestionnaire = souscripteur = "CGI2";
            //gestionnaire = souscripteur = "QUATTRONE";
            periodInMonths = 24;
#endif
            int count = this.connection.EnsureOpened().ExecuteScalar(
                "SELECT COUNT(1) " + select_OffresByGestOrSous.Substring(select_OffresByGestOrSous.IndexOf("FROM YPOBASE")),
                new { g = gestionnaire, s = souscripteur, months = periodInMonths }).ToInteger().Value;

            var paging = new Paging();
            paging.Init(count, page);
            var list = this.connection.EnsureOpened().Query<YpoBase, KpEnt, (YpoBase, KpEnt)>(
                select_OffresByGestOrSous + $" ORDER BY PBCRA , PBCRM , PBCRJ OFFSET {paging.CurrentOffset} ROWS FETCH FIRST {paging.Size} ROWS ONLY ",
                param: new { g = gestionnaire, s = souscripteur, months = periodInMonths },
                map: (y, k) => (y, k),
                splitOn: "KAAIPB");

            var pagingList = new PagingList<(YpoBase, KpEnt)> {
                List = list,
                NbTotalLines = count,
                PageNumber = paging.CurrentPage
            };

            if (pagingList.CurrentPageIsOutOfRange) {
                return GetRelancesByAny(gestionnaire, souscripteur, paging.GetMaxPage(pagingList.NbTotalLines), periodInMonths);
            }

            return pagingList;
        }

        public IEnumerable<YpoBase> SelectMany(IEnumerable<(string ipb, int alx)> keyList) {
            if (!keyList?.Any() ?? true) {
                return Enumerable.Empty<YpoBase>();
            }
            if (keyList.Any(x => x.ipb.IsEmptyOrNull() || !Affaire.IpbRegex.IsMatch(x.ipb.Trim()))) {
                throw new ArgumentException("Un ou plusieurs codes affaires sont invalides");
            }
            string query = select_many.Replace("<list>", string.Join(",", keyList.Select(x => $"('{x.ipb.ToIPB()}',{x.alx})")));
            return this.connection.EnsureOpened().Query<YpoBase>(query).ToList();
        }
    }
}
