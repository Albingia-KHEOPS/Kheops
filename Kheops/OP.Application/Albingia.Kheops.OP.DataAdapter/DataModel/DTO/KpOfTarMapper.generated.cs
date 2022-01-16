using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpOfTarMapper : EntityMap<KpOfTar>   {
        public KpOfTarMapper () {
            Map(p => p.Kfkpog).ToColumn("KFKPOG");
            Map(p => p.Kfkalg).ToColumn("KFKALG");
            Map(p => p.Kfkipb).ToColumn("KFKIPB");
            Map(p => p.Kfkalx).ToColumn("KFKALX");
            Map(p => p.Kfkfor).ToColumn("KFKFOR");
            Map(p => p.Kfkopt).ToColumn("KFKOPT");
            Map(p => p.Kfkgaran).ToColumn("KFKGARAN");
            Map(p => p.Kfknumtar).ToColumn("KFKNUMTAR");
            Map(p => p.Kfkkdgid).ToColumn("KFKKDGID");
            Map(p => p.Kfksel).ToColumn("KFKSEL");
        }
    }
}
