using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using Dapper;
using Dapper.FluentMap.Mapping;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {

    public  partial class  KpRguWeRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            const string select_GetByAffaire=@"SELECT
KHZTYP, KHZIPB, KHZALX, KHZRSQ, KHZFOR
, KHZKDEID, KHZGARAN, KHZIPK, KHZMHT, KHZMTX
, KHZAHT FROM KPRGUWE
WHERE KHZTYP = :typeAffaire
and KHZIPB = :codeAffaire
and KHZALX = :version
";
            #endregion

            public KpRguWeRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
            public IEnumerable<KpRguWe> GetByAffaire(string typeAffaire, string codeAffaire, int version){
                    return connection.EnsureOpened().Query<KpRguWe>(select_GetByAffaire, new {typeAffaire, codeAffaire, version}).ToList();
            }
    }
}
