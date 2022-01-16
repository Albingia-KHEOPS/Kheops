using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using Dapper;
using Dapper.FluentMap.Mapping;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using ALBINGIA.Framework.Common.Tools;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {

    public partial class KPTraceRepository {
        const string create = @"INSERT INTO KPTRACE 
( KCCTYP, KCCIPB, KCCALX, KCCRSQ, KCCOBJ , KCCFOR, KCCOPT, KCCGAR, KCCCRU, KCCCRD , KCCCRH, KCCLIB ) 
VALUES 
( :KCCTYP , :KCCIPB , :KCCALX , :KCCRSQ , :KCCOBJ , :KCCFOR, :KCCOPT, :KCCGAR, :KCCCRU, :KCCCRD , :KCCCRH, :KCCLIB ) ";

        const string delete = @"DELETE FROM KPTRACE 
WHERE ( KCCTYP, KCCIPB, KCCALX, KCCRSQ, KCCOBJ , KCCFOR, KCCOPT, KCCGAR, KCCCRU, KCCCRD , KCCCRH, KCCLIB ) = 
( :KCCTYP , :KCCIPB , :KCCALX , :KCCRSQ , :KCCOBJ , :KCCFOR, :KCCOPT, :KCCGAR, :KCCCRU, :KCCCRD , :KCCCRH, :KCCLIB ) ";

        const string select_ByCommentEnd = @"
SELECT 
KCCTYP, KCCIPB, KCCALX, KCCRSQ, KCCOBJ , KCCFOR, KCCOPT, KCCGAR, KCCCRU, KCCCRD , KCCCRH, KCCLIB 
FROM KPTRACE 
WHERE KCCLIB LIKE '%' || :endString AND KCCCRD = :date ";

        public int Create(KPTrace trace) {
            return this.connection.EnsureOpened().Execute(
                create,
                new {
                    trace.Kcctyp,
                    trace.Kccipb,
                    trace.Kccalx,
                    trace.Kccrsq,
                    trace.Kccobj,
                    trace.Kccfor,
                    trace.Kccopt,
                    trace.Kccgar,
                    trace.Kcccru,
                    trace.Kcccrd,
                    trace.Kcccrh,
                    trace.Kcclib
                });
        }

        public IEnumerable<KPTrace> FindByCommentEnd(DateTime date, string endString) {
            return this.connection.EnsureOpened().Query<KPTrace>(select_ByCommentEnd, new { endString, date = date.ToIntYMD() }).ToList();
        }

        public void Delete(KPTrace trace) {
            this.connection.EnsureOpened().Execute(
                delete,
                new {
                    trace.Kcctyp,
                    trace.Kccipb,
                    trace.Kccalx,
                    trace.Kccrsq,
                    trace.Kccobj,
                    trace.Kccfor,
                    trace.Kccopt,
                    trace.Kccgar,
                    trace.Kcccru,
                    trace.Kcccrd,
                    trace.Kcccrh,
                    trace.Kcclib
                });
        }
    }
}
