using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpEngMapper : EntityMap<KpEng>   {
        public KpEngMapper () {
            Map(p => p.Kdoid).ToColumn("KDOID");
            Map(p => p.Kdotyp).ToColumn("KDOTYP");
            Map(p => p.Kdoipb).ToColumn("KDOIPB");
            Map(p => p.Kdoalx).ToColumn("KDOALX");
            Map(p => p.Kdoavn).ToColumn("KDOAVN");
            Map(p => p.Kdohin).ToColumn("KDOHIN");
            Map(p => p.Kdoeco).ToColumn("KDOECO");
            Map(p => p.Kdoact).ToColumn("KDOACT");
            Map(p => p.Kdoengid).ToColumn("KDOENGID");
            Map(p => p.Kdodatd).ToColumn("KDODATD");
            Map(p => p.Kdodatf).ToColumn("KDODATF");
            Map(p => p.Kdocru).ToColumn("KDOCRU");
            Map(p => p.Kdocrd).ToColumn("KDOCRD");
            Map(p => p.Kdomaju).ToColumn("KDOMAJU");
            Map(p => p.Kdomajd).ToColumn("KDOMAJD");
            Map(p => p.Kdonpl).ToColumn("KDONPL");
            Map(p => p.Kdoapp).ToColumn("KDOAPP");
            Map(p => p.Kdoeng).ToColumn("KDOENG");
            Map(p => p.Kdoena).ToColumn("KDOENA");
            Map(p => p.Kdoobsv).ToColumn("KDOOBSV");
            Map(p => p.Kdopcv).ToColumn("KDOPCV");
            Map(p => p.Kdolct).ToColumn("KDOLCT");
            Map(p => p.Kdolca).ToColumn("KDOLCA");
            Map(p => p.Kdocat).ToColumn("KDOCAT");
            Map(p => p.Kdocaa).ToColumn("KDOCAA");
        }
    }
  

}
