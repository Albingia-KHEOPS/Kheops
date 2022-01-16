using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KInvTypMapper : EntityMap<KInvTyp>   {
        public KInvTypMapper () {
            Map(p => p.Kagid).ToColumn("KAGID");
            Map(p => p.Kagtyinv).ToColumn("KAGTYINV");
            Map(p => p.Kagdesc).ToColumn("KAGDESC");
            Map(p => p.Kagtmap).ToColumn("KAGTMAP");
            Map(p => p.Kagtable).ToColumn("KAGTABLE");
            Map(p => p.Kagkggid).ToColumn("KAGKGGID");
        }
    }
  

}
