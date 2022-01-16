using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpOfOptMapper : EntityMap<KpOfOpt>   {
        public KpOfOptMapper () {
            Map(p => p.Kfjpog).ToColumn("KFJPOG");
            Map(p => p.Kfjalg).ToColumn("KFJALG");
            Map(p => p.Kfjipb).ToColumn("KFJIPB");
            Map(p => p.Kfjalx).ToColumn("KFJALX");
            Map(p => p.Kfjchr).ToColumn("KFJCHR");
            Map(p => p.Kfjteng).ToColumn("KFJTENG");
            Map(p => p.Kfjfor).ToColumn("KFJFOR");
            Map(p => p.Kfjopt).ToColumn("KFJOPT");
            Map(p => p.Kfjkdaid).ToColumn("KFJKDAID");
            Map(p => p.Kfjkdbid).ToColumn("KFJKDBID");
            Map(p => p.Kfjkakid).ToColumn("KFJKAKID");
            Map(p => p.Kfjsel).ToColumn("KFJSEL");
        }
    }
  

}
