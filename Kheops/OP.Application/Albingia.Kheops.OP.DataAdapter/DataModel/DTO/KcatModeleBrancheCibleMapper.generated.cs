using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public  class KcatModeleBrancheCibleMapper : EntityMap<KcatModeleBrancheCible> {
        public KcatModeleBrancheCibleMapper () {
            Map(p => p.Karid).ToColumn("Karid");
            Map(p => p.Karkaqid).ToColumn("Karkaqid");
            Map(p => p.Kardateapp).ToColumn("Kardateapp");
            Map(p => p.Kartypo).ToColumn("Kartypo");
            Map(p => p.Karmodele).ToColumn("Karmodele");
        }
    }
  

}
