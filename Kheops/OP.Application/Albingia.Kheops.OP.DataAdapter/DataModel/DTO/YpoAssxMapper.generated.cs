using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YpoAssxMapper : EntityMap<YpoAssx>   {
        public YpoAssxMapper () {
            Map(p => p.Pdipb).ToColumn("PDIPB");
            Map(p => p.Pdalx).ToColumn("PDALX");
            Map(p => p.Pdavn).ToColumn("PDAVN");
            Map(p => p.Pdhin).ToColumn("PDHIN");
            Map(p => p.Pdql1).ToColumn("PDQL1");
            Map(p => p.Pdql2).ToColumn("PDQL2");
            Map(p => p.Pdql3).ToColumn("PDQL3");
            Map(p => p.Pdqld).ToColumn("PDQLD");
            Map(p => p.Pdtyp).ToColumn("PDTYP");
        }
    }
  

}
