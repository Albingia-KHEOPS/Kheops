using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpExpLCIDMapper : EntityMap<KpExpLCID>   {
        public KpExpLCIDMapper () {
            Map(p => p.Kdjid).ToColumn("KDJID");
            Map(p => p.Kdjavn).ToColumn("KDJAVN");
            Map(p => p.Kdjhin).ToColumn("KDJHIN");
            Map(p => p.Kdjkdiid).ToColumn("KDJKDIID");
            Map(p => p.Kdjordre).ToColumn("KDJORDRE");
            Map(p => p.Kdjlcval).ToColumn("KDJLCVAL");
            Map(p => p.Kdjlcvau).ToColumn("KDJLCVAU");
            Map(p => p.Kdjlcbase).ToColumn("KDJLCBASE");
            Map(p => p.Kdjloval).ToColumn("KDJLOVAL");
            Map(p => p.Kdjlovau).ToColumn("KDJLOVAU");
            Map(p => p.Kdjlobase).ToColumn("KDJLOBASE");
        }
    }
  

}
