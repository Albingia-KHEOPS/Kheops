using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpSelWMapper : EntityMap<KpSelW>   {
        public KpSelWMapper () {
            Map(p => p.Khvid).ToColumn("KHVID");
            Map(p => p.Khvtyp).ToColumn("KHVTYP");
            Map(p => p.Khvipb).ToColumn("KHVIPB");
            Map(p => p.Khvalx).ToColumn("KHVALX");
            Map(p => p.Khvperi).ToColumn("KHVPERI");
            Map(p => p.Khvrsq).ToColumn("KHVRSQ");
            Map(p => p.Khvobj).ToColumn("KHVOBJ");
            Map(p => p.Khvfor).ToColumn("KHVFOR");
            Map(p => p.Khvkdeid).ToColumn("KHVKDEID");
            Map(p => p.Khvedtb).ToColumn("KHVEDTB");
            Map(p => p.Khvdeb).ToColumn("KHVDEB");
            Map(p => p.Khvfin).ToColumn("KHVFIN");
            Map(p => p.Khveco).ToColumn("KHVECO");
            Map(p => p.Khvavn).ToColumn("KHVAVN");
            Map(p => p.Khvkdeseq).ToColumn("KHVKDESEQ");
            Map(p => p.Khvkdegar).ToColumn("KHVKDEGAR");
        }
    }
  

}
