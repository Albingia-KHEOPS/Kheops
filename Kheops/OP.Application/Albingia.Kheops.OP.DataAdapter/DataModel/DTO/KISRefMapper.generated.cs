using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KISRefMapper : EntityMap<KISRef>   {
        public KISRefMapper () {
            Map(p => p.Kgbnmid).ToColumn("KGBNMID");
            Map(p => p.Kgbdesc).ToColumn("KGBDESC");
            Map(p => p.Kgblib).ToColumn("KGBLIB");
            Map(p => p.Kgbtypz).ToColumn("KGBTYPZ");
            Map(p => p.Kgbmapp).ToColumn("KGBMAPP");
            Map(p => p.Kgbtypc).ToColumn("KGBTYPC");
            Map(p => p.Kgbpres).ToColumn("KGBPRES");
            Map(p => p.Kgbtypu).ToColumn("KGBTYPU");
            Map(p => p.Kgbobli).ToColumn("KGBOBLI");
            Map(p => p.Kgbscraff).ToColumn("KGBSCRAFF");
            Map(p => p.Kgbscrctr).ToColumn("KGBSCRCTR");
            Map(p => p.Kgbobsv).ToColumn("KGBOBSV");
            Map(p => p.Kgbnmbd).ToColumn("KGBNMBD");
            Map(p => p.Kgblngz).ToColumn("KGBLNGZ");
            Map(p => p.Kgbsaid2).ToColumn("KGBSAID2");
            Map(p => p.Kgbscid2).ToColumn("KGBSCID2");
            Map(p => p.Kgbnref).ToColumn("KGBNREF");
            Map(p => p.Kgbdval).ToColumn("KGBDVAL");
            Map(p => p.Kgbvdec).ToColumn("KGBVDEC");
            Map(p => p.Kgbvdecn).ToColumn("KGBVDECN");
            Map(p => p.Kgbvucon).ToColumn("KGBVUCON");
            Map(p => p.Kgbvdatd).ToColumn("KGBVDATD");
            Map(p => p.Kgbvheud).ToColumn("KGBVHEUD");
            Map(p => p.Kgbvdatf).ToColumn("KGBVDATF");
            Map(p => p.Kgbvheuf).ToColumn("KGBVHEUF");
            Map(p => p.Kgbvtxt).ToColumn("KGBVTXT");
            Map(p => p.Kgbkfbid).ToColumn("KGBKFBID");
            Map(p => p.Kgbvufam).ToColumn("KGBVUFAM");
        }
    }
  

}
