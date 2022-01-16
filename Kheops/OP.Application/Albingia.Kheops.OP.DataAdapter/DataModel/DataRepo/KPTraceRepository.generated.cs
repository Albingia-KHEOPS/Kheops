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

    public  partial class  KPTraceRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            const string select_GetByAffaire=@"SELECT
KCCTYP, KCCIPB, KCCALX, KCCRSQ, KCCOBJ
, KCCFOR, KCCOPT, KCCGAR, KCCCRU, KCCCRD
, KCCCRH, KCCLIB FROM KPTRACE
WHERE KCCTYP = :KCCTYP
and KCCIPB = :KCCIPB
and KCCALX = :KCCALX
";
            #endregion

            public KPTraceRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
            public IEnumerable<KPTrace> GetByAffaire(string KCCTYP, string KCCIPB, int KCCALX){
                    return connection.EnsureOpened().Query<KPTrace>(select_GetByAffaire, new {KCCTYP, KCCIPB, KCCALX}).ToList();
            }
    }
}
