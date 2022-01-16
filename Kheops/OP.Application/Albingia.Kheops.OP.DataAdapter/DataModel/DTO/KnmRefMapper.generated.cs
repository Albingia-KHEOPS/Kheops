using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KnmRefMapper : EntityMap<KnmRef>   {
        public KnmRefMapper () {
            Map(p => p.Khiid).ToColumn("KHIID");
            Map(p => p.Khitypo).ToColumn("KHITYPO");
            Map(p => p.Khinmc).ToColumn("KHINMC");
            Map(p => p.Khidesi).ToColumn("KHIDESI");
            Map(p => p.Khinord).ToColumn("KHINORD");
        }
    }
  

}
