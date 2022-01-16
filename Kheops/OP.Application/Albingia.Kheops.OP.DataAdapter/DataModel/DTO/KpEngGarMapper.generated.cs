using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpEngGarMapper : EntityMap<KpEngGar>   {
        public KpEngGarMapper () {
            Map(p => p.Kdsid).ToColumn("KDSID");
            Map(p => p.Kdstyp).ToColumn("KDSTYP");
            Map(p => p.Kdsipb).ToColumn("KDSIPB");
            Map(p => p.Kdsalx).ToColumn("KDSALX");
            Map(p => p.Kdsavn).ToColumn("KDSAVN");
            Map(p => p.Kdshin).ToColumn("KDSHIN");
            Map(p => p.Kdsrsq).ToColumn("KDSRSQ");
            Map(p => p.Kdsfam).ToColumn("KDSFAM");
            Map(p => p.Kdsven).ToColumn("KDSVEN");
            Map(p => p.Kdskdrid).ToColumn("KDSKDRID");
            Map(p => p.Kdsgaran).ToColumn("KDSGARAN");
            Map(p => p.Kdsengok).ToColumn("KDSENGOK");
            Map(p => p.Kdslci).ToColumn("KDSLCI");
            Map(p => p.Kdssmp).ToColumn("KDSSMP");
            Map(p => p.Kdssmpf).ToColumn("KDSSMPF");
            Map(p => p.Kdssmpu).ToColumn("KDSSMPU");
            Map(p => p.Kdssmpr).ToColumn("KDSSMPR");
            Map(p => p.Kdscru).ToColumn("KDSCRU");
            Map(p => p.Kdscrd).ToColumn("KDSCRD");
            Map(p => p.Kdsmaju).ToColumn("KDSMAJU");
            Map(p => p.Kdsmajd).ToColumn("KDSMAJD");
            Map(p => p.Kdskdoid).ToColumn("KDSKDOID");
            Map(p => p.Kdscat).ToColumn("KDSCAT");
        }
    }
  

}
