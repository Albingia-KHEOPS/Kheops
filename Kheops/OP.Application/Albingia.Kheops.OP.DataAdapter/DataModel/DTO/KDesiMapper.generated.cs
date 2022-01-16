using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KDesiMapper : EntityMap<KDesi>   {
        public KDesiMapper () {
            Map(p => p.Kdwid).ToColumn("KDWID");
            Map(p => p.Kdwdesi).ToColumn("KDWDESI");
        }
    }
  

}
