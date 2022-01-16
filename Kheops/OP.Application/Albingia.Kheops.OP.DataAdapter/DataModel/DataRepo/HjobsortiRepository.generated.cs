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

    public  partial class  HjobsortiRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            const string select_GetByAffaire=@"SELECT
IPB, ALX, TYP, AVN, HIN
, RSQ, OBJ, FORM, OPT, GARAN
, DATEDEB, HEUREDEB, DATEFIN, HEUREFIN, SORTI
 FROM HJOBSORTI
WHERE IPB = :parIPB
and ALX = :parALX
and AVN = :parAVN
";
            #endregion

            public HjobsortiRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
            public IEnumerable<KJobSorti> GetByAffaire(string parIPB, int parALX, int parAVN){
                    return connection.EnsureOpened().Query<KJobSorti>(select_GetByAffaire, new {parIPB, parALX, parAVN}).ToList();
            }
    }
}
