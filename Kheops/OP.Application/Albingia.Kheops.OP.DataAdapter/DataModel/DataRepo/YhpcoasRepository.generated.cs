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

    public  partial class  YhpcoasRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            const string select_GetByAffaire=@"SELECT
PHIPB, PHALX, PHAVN, PHHIN, PHTAP
, PHCIE, PHINL, PHPOL, PHAPP, PHCOM
, PHTXF, PHAFR, PHEPA, PHEPM, PHEPJ
, PHFPA, PHFPM, PHFPJ, PHIN5, PHTAC
, PHTAA, PHTAM, PHTAJ, PHTYP FROM YHPCOAS
WHERE PHIPB = :PHIPB
and PHALX = :PHALX
and PHAVN = :PHAVN
and PHTYP = :PHTYP
";
            #endregion

            public YhpcoasRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
            public IEnumerable<YpoCoas> GetByAffaire(string PHIPB, int PHALX, int PHAVN, string PHTYP){
                    return connection.EnsureOpened().Query<YpoCoas>(select_GetByAffaire, new {PHIPB, PHALX, PHAVN, PHTYP}).ToList();
            }
    }
}
