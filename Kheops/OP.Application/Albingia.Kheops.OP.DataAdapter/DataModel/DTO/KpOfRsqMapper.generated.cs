using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpOfRsqMapper : EntityMap<KpOfRsq>   {
        public KpOfRsqMapper () {
            Map(p => p.Kfipog).ToColumn("KFIPOG");
            Map(p => p.Kfialg).ToColumn("KFIALG");
            Map(p => p.Kfiipb).ToColumn("KFIIPB");
            Map(p => p.Kfialx).ToColumn("KFIALX");
            Map(p => p.Kfichr).ToColumn("KFICHR");
            Map(p => p.Kfitye).ToColumn("KFITYE");
            Map(p => p.Kfirsq).ToColumn("KFIRSQ");
            Map(p => p.Kfiobj).ToColumn("KFIOBJ");
            Map(p => p.Kfiinv).ToColumn("KFIINV");
            Map(p => p.Kfisel).ToColumn("KFISEL");
        }
    }
  

}
