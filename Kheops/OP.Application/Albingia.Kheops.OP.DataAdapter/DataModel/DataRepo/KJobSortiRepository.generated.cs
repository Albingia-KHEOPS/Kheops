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

    public  partial class  KJobSortiRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            const string select_GetByAffaire=@"SELECT
IPB, ALX, TYP, AVN, RSQ
, OBJ, FORM, OPT, GARAN, DATEDEB
, HEUREDEB, DATEFIN, HEUREFIN, SORTI FROM KJOBSORTI
WHERE IPB = :codeContrat
and ALX = :versionContrat
and TYP = :typeContrat
";
            #endregion

            public KJobSortiRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
            public IEnumerable<KJobSorti> GetByAffaire(string codeContrat, int versionContrat, string typeContrat){
                    return connection.EnsureOpened().Query<KJobSorti>(select_GetByAffaire, new {codeContrat, versionContrat, typeContrat}).ToList();
            }
    }
}
