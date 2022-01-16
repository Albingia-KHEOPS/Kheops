using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KExpLciMapper : EntityMap<KExpLci>   {
        public KExpLciMapper () {
            Map(p => p.Khgid).ToColumn("KHGID");
            Map(p => p.Khglce).ToColumn("KHGLCE");
            Map(p => p.Khgdesc).ToColumn("KHGDESC");
            Map(p => p.Khgdesi).ToColumn("KHGDESI");
            Map(p => p.Khgmodi).ToColumn("KHGMODI");
        }
    }
  

}
