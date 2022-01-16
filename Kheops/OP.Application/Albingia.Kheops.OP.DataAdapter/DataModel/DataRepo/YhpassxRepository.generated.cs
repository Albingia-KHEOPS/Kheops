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

    public  partial class  YhpassxRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            const string select_GetByAffaire=@"SELECT
PDIPB, PDALX, PDAVN, PDHIN, PDQL1
, PDQL2, PDQL3, PDQLD, PDTYP FROM YHPASSX
WHERE PDIPB = :PDIPB
and PDALX = :PDALX
and PDAVN = :PDAVN
and PDTYP = :PDTYP
";
            #endregion

            public YhpassxRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
            public IEnumerable<YpoAssx> GetByAffaire(string PDIPB, int PDALX, int PDAVN, string PDTYP){
                    return connection.EnsureOpened().Query<YpoAssx>(select_GetByAffaire, new {PDIPB, PDALX, PDAVN, PDTYP}).ToList();
            }
    }
}
