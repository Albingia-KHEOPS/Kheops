using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class KpRsqRepository {
        const string select_GetByAffaire = @"SELECT 
KABTYP, KABIPB, KABALX, KABRSQ, KABCIBLE 
, KABDESC, KABDESI, KABOBSV, KABREPVAL, KABREPOBL 
, KABAPE, KABTRE, KABCLASS, KABNMC01, KABNMC02 
, KABNMC03, KABNMC04, KABNMC05, KABMAND, KABMANF 
, KABDSPP, KABLCIVALO, KABLCIVALA, KABLCIVALW, KABLCIUNIT 
, KABLCIBASE, KABKDIID, KABFRHVALO, KABFRHVALA, KABFRHVALW 
, KABFRHUNIT, KABFRHBASE, KABKDKID, KABNSIR, KABMANDH 
, KABMANFH, KABSURF, KABVMC, KABPROL, KABPBI 
, KABBRNT, KABBRNC FROM KPRSQ 
WHERE KABTYP = :typeAffaire 
and KABIPB = :codeAffaire 
and KABALX = :version ";

        public IEnumerable<KpRsq> GetByAffaire(string typeAffaire, string codeAffaire, int version) {
            try {
                return connection.EnsureOpened().Query<KpRsq>(select_GetByAffaire, new { typeAffaire, codeAffaire, version }).ToList();
            }
            finally {
                connection.EnsureClosed();
            }
        }
    }
}
