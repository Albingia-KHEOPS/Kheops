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

    public  partial class  YpoCoasRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            const string select_GetByAffaire=@"SELECT
PHTYP, PHIPB, PHALX, PHTAP, PHCIE
, PHINL, PHPOL, PHAPP, PHCOM, PHTXF
, PHAFR, PHEPA, PHEPM, PHEPJ, PHFPA
, PHFPM, PHFPJ, PHIN5, PHTAC, PHTAA
, PHTAM, PHTAJ FROM YPOCOAS
WHERE PHTYP = :PHTYP
and PHIPB = :PHIPB
and PHALX = :PHALX
";
            #endregion

            public YpoCoasRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
            public IEnumerable<YpoCoas> GetByAffaire(string PHTYP, string PHIPB, int PHALX){
                    return connection.EnsureOpened().Query<YpoCoas>(select_GetByAffaire, new {PHTYP, PHIPB, PHALX}).ToList();
            }
    }
}
