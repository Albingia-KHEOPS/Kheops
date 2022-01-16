using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpAvTrcMapper : EntityMap<KpAvTrc>   {
        public KpAvTrcMapper () {
            Map(p => p.Khoid).ToColumn("KHOID");
            Map(p => p.Khotyp).ToColumn("KHOTYP");
            Map(p => p.Khoipb).ToColumn("KHOIPB");
            Map(p => p.Khoalx).ToColumn("KHOALX");
            Map(p => p.Khoperi).ToColumn("KHOPERI");
            Map(p => p.Khorsq).ToColumn("KHORSQ");
            Map(p => p.Khoobj).ToColumn("KHOOBJ");
            Map(p => p.Khofor).ToColumn("KHOFOR");
            Map(p => p.Khoopt).ToColumn("KHOOPT");
            Map(p => p.Khoetape).ToColumn("KHOETAPE");
            Map(p => p.Khocham).ToColumn("KHOCHAM");
            Map(p => p.Khoact).ToColumn("KHOACT");
            Map(p => p.Khoanv).ToColumn("KHOANV");
            Map(p => p.Khonvv).ToColumn("KHONVV");
            Map(p => p.Khoavo).ToColumn("KHOAVO");
            Map(p => p.Khooef).ToColumn("KHOOEF");
            Map(p => p.Khocru).ToColumn("KHOCRU");
            Map(p => p.Khocrd).ToColumn("KHOCRD");
            Map(p => p.Khocrh).ToColumn("KHOCRH");
        }
    }
  

}
