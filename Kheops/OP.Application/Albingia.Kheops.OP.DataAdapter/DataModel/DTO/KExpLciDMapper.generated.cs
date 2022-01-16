using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KExpLciDMapper : EntityMap<KExpLciD>   {
        public KExpLciDMapper () {
            Map(p => p.Khhid).ToColumn("KHHID");
            Map(p => p.Khhkhgid).ToColumn("KHHKHGID");
            Map(p => p.Khhordre).ToColumn("KHHORDRE");
            Map(p => p.Khhlcval).ToColumn("KHHLCVAL");
            Map(p => p.Khhlcvau).ToColumn("KHHLCVAU");
            Map(p => p.Khhlcbase).ToColumn("KHHLCBASE");
            Map(p => p.Khhloval).ToColumn("KHHLOVAL");
            Map(p => p.Khhlovau).ToColumn("KHHLOVAU");
            Map(p => p.Khhlobase).ToColumn("KHHLOBASE");
        }
    }
  

}
