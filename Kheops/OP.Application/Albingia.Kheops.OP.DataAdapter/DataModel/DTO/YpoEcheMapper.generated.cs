using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YpoEcheMapper : EntityMap<YpoEche>   {
        public YpoEcheMapper () {
            Map(p => p.Piipb).ToColumn("PIIPB");
            Map(p => p.Pialx).ToColumn("PIALX");
            Map(p => p.Piavn).ToColumn("PIAVN");
            Map(p => p.Pihin).ToColumn("PIHIN");
            Map(p => p.Pieha).ToColumn("PIEHA");
            Map(p => p.Piehm).ToColumn("PIEHM");
            Map(p => p.Piehj).ToColumn("PIEHJ");
            Map(p => p.Piehe).ToColumn("PIEHE");
            Map(p => p.Pipcr).ToColumn("PIPCR");
            Map(p => p.Pipcc).ToColumn("PIPCC");
            Map(p => p.Pipmr).ToColumn("PIPMR");
            Map(p => p.Pipmc).ToColumn("PIPMC");
            Map(p => p.Piafr).ToColumn("PIAFR");
            Map(p => p.Piipk).ToColumn("PIIPK");
            Map(p => p.Piatt).ToColumn("PIATT");
            Map(p => p.Pityp).ToColumn("PITYP");
        }
    }
  

}
