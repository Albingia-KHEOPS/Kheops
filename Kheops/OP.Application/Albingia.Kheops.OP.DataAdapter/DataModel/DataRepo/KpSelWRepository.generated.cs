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

    public  partial class  KpSelWRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            const string select_GetByAffaire=@"SELECT
KHVID, KHVTYP, KHVIPB, KHVALX, KHVPERI
, KHVRSQ, KHVOBJ, KHVFOR, KHVKDEID, KHVEDTB
, KHVDEB, KHVFIN, KHVECO, KHVAVN, KHVKDESEQ
, KHVKDEGAR FROM KPSELW
WHERE KHVIPB = :KHVIPB
and KHVALX = :KHVALX
";
            const string select_GetByLot=@"SELECT
KHVID, KHVTYP, KHVIPB, KHVALX, KHVPERI
, KHVRSQ, KHVOBJ, KHVFOR, KHVKDEID, KHVEDTB
, KHVDEB, KHVFIN, KHVECO, KHVAVN, KHVKDESEQ
, KHVKDEGAR FROM KPSELW
WHERE KHVID = :parKHVID
";
            #endregion

            public KpSelWRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
            public IEnumerable<KpSelW> GetByAffaire(string KHVIPB, int KHVALX){
                    return connection.EnsureOpened().Query<KpSelW>(select_GetByAffaire, new {KHVIPB, KHVALX}).ToList();
            }
            public IEnumerable<KpSelW> GetByLot(Int64 parKHVID){
                    return connection.EnsureOpened().Query<KpSelW>(select_GetByLot, new {parKHVID}).ToList();
            }
    }
}
