using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KISModlMapper : EntityMap<KISModl>   {
        public KISModlMapper () {
            Map(p => p.Kgdid).ToColumn("KGDID");
            Map(p => p.Kgdmodid).ToColumn("KGDMODID");
            Map(p => p.Kgdnmid).ToColumn("KGDNMID");
            Map(p => p.Kgdlib).ToColumn("KGDLIB");
            Map(p => p.Kgdnumaff).ToColumn("KGDNUMAFF");
            Map(p => p.Kgdsautl).ToColumn("KGDSAUTL");
            Map(p => p.Kgdmodi).ToColumn("KGDMODI");
            Map(p => p.Kgdobli).ToColumn("KGDOBLI");
            Map(p => p.Kgdsql).ToColumn("KGDSQL");
            Map(p => p.Kgdsqlord).ToColumn("KGDSQLORD");
            Map(p => p.Kgdscraffs).ToColumn("KGDSCRAFFS");
            Map(p => p.Kgdscrctrs).ToColumn("KGDSCRCTRS");
            Map(p => p.Kgdparenid).ToColumn("KGDPARENID");
            Map(p => p.Kgdparenc).ToColumn("KGDPARENC");
            Map(p => p.Kgdparene).ToColumn("KGDPARENE");
            Map(p => p.Kgdcru).ToColumn("KGDCRU");
            Map(p => p.Kgdcrd).ToColumn("KGDCRD");
            Map(p => p.Kgdmju).ToColumn("KGDMJU");
            Map(p => p.Kgdmjd).ToColumn("KGDMJD");
            Map(p => p.Kgdpres).ToColumn("KGDPRES");
            Map(p => p.Kgdsaid2).ToColumn("KGDSAID2");
            Map(p => p.Kgdscid2).ToColumn("KGDSCID2");
            Map(p => p.Kgdnref).ToColumn("KGDNREF");
            Map(p => p.Kgdvucon).ToColumn("KGDVUCON");
            Map(p => p.Kgdvufam).ToColumn("KGDVUFAM");
        }
    }
  

}
