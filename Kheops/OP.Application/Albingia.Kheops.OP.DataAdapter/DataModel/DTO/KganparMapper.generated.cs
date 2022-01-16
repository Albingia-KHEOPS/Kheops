using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KganparMapper : EntityMap<Kganpar>   {
        public KganparMapper () {
            Map(p => p.Kaucar).ToColumn("KAUCAR");
            Map(p => p.Kaunat).ToColumn("KAUNAT");
            Map(p => p.Kauaffi).ToColumn("KAUAFFI");
            Map(p => p.Kaumodi).ToColumn("KAUMODI");
            Map(p => p.Kauganc).ToColumn("KAUGANC");
            Map(p => p.Kaugannc).ToColumn("KAUGANNC");
        }
    }
  

}
