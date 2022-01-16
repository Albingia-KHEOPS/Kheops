using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpMatFfMapper : EntityMap<KpMatFf>   {
        public KpMatFfMapper () {
            Map(p => p.Keatyp).ToColumn("KEATYP");
            Map(p => p.Keaipb).ToColumn("KEAIPB");
            Map(p => p.Keaalx).ToColumn("KEAALX");
            Map(p => p.Keaavn).ToColumn("KEAAVN");
            Map(p => p.Keahin).ToColumn("KEAHIN");
            Map(p => p.Keachr).ToColumn("KEACHR");
            Map(p => p.Keafor).ToColumn("KEAFOR");
            Map(p => p.Keaopt).ToColumn("KEAOPT");
            Map(p => p.Keakdbid).ToColumn("KEAKDBID");
            Map(p => p.Keakdaid).ToColumn("KEAKDAID");
        }
    }
  

}
