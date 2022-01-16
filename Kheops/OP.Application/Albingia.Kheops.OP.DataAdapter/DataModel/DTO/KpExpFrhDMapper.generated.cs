using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpExpFrhDMapper : EntityMap<KpExpFrhD>   {
        public KpExpFrhDMapper () {
            Map(p => p.Kdlid).ToColumn("KDLID");
            Map(p => p.Kdlavn).ToColumn("KDLAVN");
            Map(p => p.Kdlhin).ToColumn("KDLHIN");
            Map(p => p.Kdlkdkid).ToColumn("KDLKDKID");
            Map(p => p.Kdlordre).ToColumn("KDLORDRE");
            Map(p => p.Kdlfhval).ToColumn("KDLFHVAL");
            Map(p => p.Kdlfhvau).ToColumn("KDLFHVAU");
            Map(p => p.Kdlfhbase).ToColumn("KDLFHBASE");
            Map(p => p.Kdlind).ToColumn("KDLIND");
            Map(p => p.Kdlivo).ToColumn("KDLIVO");
            Map(p => p.Kdlfhmini).ToColumn("KDLFHMINI");
            Map(p => p.Kdlfhmaxi).ToColumn("KDLFHMAXI");
            Map(p => p.Kdllimdeb).ToColumn("KDLLIMDEB");
            Map(p => p.Kdllimfin).ToColumn("KDLLIMFIN");
        }
    }
  

}
