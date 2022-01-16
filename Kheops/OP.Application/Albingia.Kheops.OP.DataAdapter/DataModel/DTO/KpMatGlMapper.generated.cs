using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpMatGlMapper : EntityMap<KpMatGl>   {
        public KpMatGlMapper () {
            Map(p => p.Keftyp).ToColumn("KEFTYP");
            Map(p => p.Kefipb).ToColumn("KEFIPB");
            Map(p => p.Kefalx).ToColumn("KEFALX");
            Map(p => p.Kefavn).ToColumn("KEFAVN");
            Map(p => p.Kefhin).ToColumn("KEFHIN");
            Map(p => p.Kefkedchr).ToColumn("KEFKEDCHR");
            Map(p => p.Kefkeechr).ToColumn("KEFKEECHR");
            Map(p => p.Kefico).ToColumn("KEFICO");
        }
    }
  

}
