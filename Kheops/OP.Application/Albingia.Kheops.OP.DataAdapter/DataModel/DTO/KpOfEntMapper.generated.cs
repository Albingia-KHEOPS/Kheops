using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpOfEntMapper : EntityMap<KpOfEnt>   {
        public KpOfEntMapper () {
            Map(p => p.Kfhpog).ToColumn("KFHPOG");
            Map(p => p.Kfhalg).ToColumn("KFHALG");
            Map(p => p.Kfhipb).ToColumn("KFHIPB");
            Map(p => p.Kfhalx).ToColumn("KFHALX");
            Map(p => p.Kfhnpo).ToColumn("KFHNPO");
            Map(p => p.Kfhefd).ToColumn("KFHEFD");
            Map(p => p.Kfhsad).ToColumn("KFHSAD");
            Map(p => p.Kfhbra).ToColumn("KFHBRA");
            Map(p => p.Kfhcible).ToColumn("KFHCIBLE");
            Map(p => p.Kfhipr).ToColumn("KFHIPR");
            Map(p => p.Kfhalr).ToColumn("KFHALR");
            Map(p => p.Kfhtypo).ToColumn("KFHTYPO");
            Map(p => p.Kfhipm).ToColumn("KFHIPM");
            Map(p => p.Khfsit).ToColumn("KHFSIT");
            Map(p => p.Kfhstu).ToColumn("KFHSTU");
            Map(p => p.Kfhstd).ToColumn("KFHSTD");
        }
    }
  

}
