using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpMatFrMapper : EntityMap<KpMatFr>   {
        public KpMatFrMapper () {
            Map(p => p.Kebtyp).ToColumn("KEBTYP");
            Map(p => p.Kebipb).ToColumn("KEBIPB");
            Map(p => p.Kebalx).ToColumn("KEBALX");
            Map(p => p.Kebavn).ToColumn("KEBAVN");
            Map(p => p.Kebhin).ToColumn("KEBHIN");
            Map(p => p.Kebchr).ToColumn("KEBCHR");
            Map(p => p.Kebtye).ToColumn("KEBTYE");
            Map(p => p.Kebrsq).ToColumn("KEBRSQ");
            Map(p => p.Kebobj).ToColumn("KEBOBJ");
            Map(p => p.Kebinv).ToColumn("KEBINV");
            Map(p => p.Kebvid).ToColumn("KEBVID");
        }
    }
  

}
