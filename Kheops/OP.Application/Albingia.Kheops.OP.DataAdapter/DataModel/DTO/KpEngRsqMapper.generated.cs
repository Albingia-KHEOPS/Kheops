using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpEngRsqMapper : EntityMap<KpEngRsq>   {
        public KpEngRsqMapper () {
            Map(p => p.Kdrid).ToColumn("KDRID");
            Map(p => p.Kdrtyp).ToColumn("KDRTYP");
            Map(p => p.Kdripb).ToColumn("KDRIPB");
            Map(p => p.Kdralx).ToColumn("KDRALX");
            Map(p => p.Kdravn).ToColumn("KDRAVN");
            Map(p => p.Kdrhin).ToColumn("KDRHIN");
            Map(p => p.Kdrrsq).ToColumn("KDRRSQ");
            Map(p => p.Kdrkdqid).ToColumn("KDRKDQID");
            Map(p => p.Kdrfam).ToColumn("KDRFAM");
            Map(p => p.Kdrven).ToColumn("KDRVEN");
            Map(p => p.Kdrlci).ToColumn("KDRLCI");
            Map(p => p.Kdrsmp).ToColumn("KDRSMP");
            Map(p => p.Kdrengc).ToColumn("KDRENGC");
            Map(p => p.Kdrengf).ToColumn("KDRENGF");
            Map(p => p.Kdrengok).ToColumn("KDRENGOK");
            Map(p => p.Kdrcru).ToColumn("KDRCRU");
            Map(p => p.Kdrcrd).ToColumn("KDRCRD");
            Map(p => p.Kdrmaju).ToColumn("KDRMAJU");
            Map(p => p.Kdrmajd).ToColumn("KDRMAJD");
            Map(p => p.Kdrkdoid).ToColumn("KDRKDOID");
            Map(p => p.Kdrcat).ToColumn("KDRCAT");
            Map(p => p.Kdrsmf).ToColumn("KDRSMF");
        }
    }
  

}
