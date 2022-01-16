using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KExpFrhMapper : EntityMap<KExpFrh>   {
        public KExpFrhMapper () {
            Map(p => p.Kheid).ToColumn("KHEID");
            Map(p => p.Khefhe).ToColumn("KHEFHE");
            Map(p => p.Khedesc).ToColumn("KHEDESC");
            Map(p => p.Khedesi).ToColumn("KHEDESI");
            Map(p => p.Khemodi).ToColumn("KHEMODI");
        }
    }
  

}
