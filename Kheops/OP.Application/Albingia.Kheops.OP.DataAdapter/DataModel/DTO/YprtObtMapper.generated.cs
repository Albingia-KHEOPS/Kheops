using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YprtObtMapper : EntityMap<YprtObt>   {
        public YprtObtMapper () {
            Map(p => p.Kfipb).ToColumn("KFIPB");
            Map(p => p.Kfalx).ToColumn("KFALX");
            Map(p => p.Kfavn).ToColumn("KFAVN");
            Map(p => p.Kfhin).ToColumn("KFHIN");
            Map(p => p.Kfrsq).ToColumn("KFRSQ");
            Map(p => p.Kfobj).ToColumn("KFOBJ");
            Map(p => p.Kfduv).ToColumn("KFDUV");
            Map(p => p.Kfduu).ToColumn("KFDUU");
            Map(p => p.Kfdda).ToColumn("KFDDA");
            Map(p => p.Kfddm).ToColumn("KFDDM");
            Map(p => p.Kfddj).ToColumn("KFDDJ");
            Map(p => p.Kfdfa).ToColumn("KFDFA");
            Map(p => p.Kfdfm).ToColumn("KFDFM");
            Map(p => p.Kfdfj).ToColumn("KFDFJ");
            Map(p => p.Kfesv).ToColumn("KFESV");
            Map(p => p.Kfesu).ToColumn("KFESU");
            Map(p => p.Kfeda).ToColumn("KFEDA");
            Map(p => p.Kfedm).ToColumn("KFEDM");
            Map(p => p.Kfedj).ToColumn("KFEDJ");
            Map(p => p.Kfefa).ToColumn("KFEFA");
            Map(p => p.Kfefm).ToColumn("KFEFM");
            Map(p => p.Kfefj).ToColumn("KFEFJ");
            Map(p => p.Kftdf).ToColumn("KFTDF");
        }
    }
  

}
