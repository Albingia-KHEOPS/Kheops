using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KReavenMapper : EntityMap<KReaven>   {
        public KReavenMapper () {
            Map(p => p.Kgafam).ToColumn("KGAFAM");
            Map(p => p.Kgaven).ToColumn("KGAVEN");
            Map(p => p.Kgalibv).ToColumn("KGALIBV");
            Map(p => p.Kgasepa).ToColumn("KGASEPA");
        }
    }
  

}
