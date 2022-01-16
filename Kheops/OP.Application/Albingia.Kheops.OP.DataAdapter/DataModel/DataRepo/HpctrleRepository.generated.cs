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

    public  partial class  HpctrleRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            const string select_GetByAffaire=@"SELECT
KEVTYP, KEVIPB, KEVALX, KEVAVN, KEVHIN
, KEVETAPE, KEVETORD, KEVORDR, KEVPERI, KEVRSQ
, KEVOBJ, KEVKBEID, KEVFOR, KEVOPT, KEVNIVM
, KEVCRU, KEVCRD, KEVCRH, KEVMAJU, KEVMAJD
, KEVMAJH, KEVTAG, KEVTAGC FROM HPCTRLE
WHERE KEVTYP = :KEVTYP
and KEVIPB = :KEVIPB
and KEVALX = :KEVALX
and KEVAVN = :KEVAVN
";
            #endregion

            public HpctrleRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
            public IEnumerable<KpCtrlE> GetByAffaire(string KEVTYP, string KEVIPB, int KEVALX, int KEVAVN){
                    return connection.EnsureOpened().Query<KpCtrlE>(select_GetByAffaire, new {KEVTYP, KEVIPB, KEVALX, KEVAVN}).ToList();
            }
    }
}
