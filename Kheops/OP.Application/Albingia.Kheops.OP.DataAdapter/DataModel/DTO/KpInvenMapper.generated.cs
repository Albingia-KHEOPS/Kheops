using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpInvenMapper : EntityMap<KpInven>   {
        public KpInvenMapper () {
            Map(p => p.Kbeid).ToColumn("KBEID");
            Map(p => p.Kbetyp).ToColumn("KBETYP");
            Map(p => p.Kbeipb).ToColumn("KBEIPB");
            Map(p => p.Kbealx).ToColumn("KBEALX");
            Map(p => p.Kbeavn).ToColumn("KBEAVN");
            Map(p => p.Kbehin).ToColumn("KBEHIN");
            Map(p => p.Kbechr).ToColumn("KBECHR");
            Map(p => p.Kbedesc).ToColumn("KBEDESC");
            Map(p => p.Kbekagid).ToColumn("KBEKAGID");
            Map(p => p.Kbekadid).ToColumn("KBEKADID");
            Map(p => p.Kberepval).ToColumn("KBEREPVAL");
            Map(p => p.Kbeval).ToColumn("KBEVAL");
            Map(p => p.Kbevaa).ToColumn("KBEVAA");
            Map(p => p.Kbevaw).ToColumn("KBEVAW");
            Map(p => p.Kbevat).ToColumn("KBEVAT");
            Map(p => p.Kbevau).ToColumn("KBEVAU");
            Map(p => p.Kbevah).ToColumn("KBEVAH");
            Map(p => p.Kbeivo).ToColumn("KBEIVO");
            Map(p => p.Kbeiva).ToColumn("KBEIVA");
            Map(p => p.Kbeivw).ToColumn("KBEIVW");
        }
    }
  

}
