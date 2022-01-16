using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KPTraceMapper : EntityMap<KPTrace>   {
        public KPTraceMapper () {
            Map(p => p.Kcctyp).ToColumn("KCCTYP");
            Map(p => p.Kccipb).ToColumn("KCCIPB");
            Map(p => p.Kccalx).ToColumn("KCCALX");
            Map(p => p.Kccrsq).ToColumn("KCCRSQ");
            Map(p => p.Kccobj).ToColumn("KCCOBJ");
            Map(p => p.Kccfor).ToColumn("KCCFOR");
            Map(p => p.Kccopt).ToColumn("KCCOPT");
            Map(p => p.Kccgar).ToColumn("KCCGAR");
            Map(p => p.Kcccru).ToColumn("KCCCRU");
            Map(p => p.Kcccrd).ToColumn("KCCCRD");
            Map(p => p.Kcccrh).ToColumn("KCCCRH");
            Map(p => p.Kcclib).ToColumn("KCCLIB");
        }
    }
}
