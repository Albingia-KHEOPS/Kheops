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

    public  partial class  YhpconxRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            const string select_GetByAffaire=@"SELECT
PJTYP, PJCCX, PJCNX, PJIPB, PJALX
, PJAVN, PJHIN, PJBRA, PJSBR, PJCAT
, PJOBS, PJIDE FROM YHPCONX
WHERE PJTYP = :PJTYP
and PJIPB = :PJIPB
and PJALX = :PJALX
and PJAVN = :PJAVN
";
            #endregion

            public YhpconxRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
            public IEnumerable<YpoConx> GetByAffaire(string PJTYP, string PJIPB, int PJALX, int PJAVN){
                    return connection.EnsureOpened().Query<YpoConx>(select_GetByAffaire, new {PJTYP, PJIPB, PJALX, PJAVN}).ToList();
            }
    }
}
