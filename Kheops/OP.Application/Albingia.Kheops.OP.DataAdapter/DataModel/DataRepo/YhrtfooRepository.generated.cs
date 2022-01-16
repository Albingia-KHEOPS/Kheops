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

    public  partial class  YhrtfooRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            const string select_GetByAffaire=@"SELECT
JPIPB, JPALX, JPAVN, JPHIN, JPRSQ
, JPFOR, JPOBJ FROM YHRTFOO
WHERE JPIPB = :JPIPB
and JPALX = :JPALX
and JPAVN = :JPAVN
";
            #endregion

            public YhrtfooRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
            public IEnumerable<YprtFoo> GetByAffaire(string JPIPB, int JPALX, int JPAVN){
                    return connection.EnsureOpened().Query<YprtFoo>(select_GetByAffaire, new {JPIPB, JPALX, JPAVN}).ToList();
            }
    }
}
