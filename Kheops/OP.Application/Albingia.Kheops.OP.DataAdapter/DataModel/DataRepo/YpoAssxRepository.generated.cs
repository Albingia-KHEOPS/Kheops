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

    public  partial class  YpoAssxRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            const string select_GetByAffaire=@"SELECT
PDTYP, PDIPB, PDALX, PDQL1, PDQL2
, PDQL3, PDQLD FROM YPOASSX
WHERE PDTYP = :PDTYP
and PDIPB = :PDIPB
and PDALX = :PDALX
";
            #endregion

            public YpoAssxRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
            public IEnumerable<YpoAssx> GetByAffaire(string PDTYP, string PDIPB, int PDALX){
                    return connection.EnsureOpened().Query<YpoAssx>(select_GetByAffaire, new {PDTYP, PDIPB, PDALX}).ToList();
            }
    }
}
