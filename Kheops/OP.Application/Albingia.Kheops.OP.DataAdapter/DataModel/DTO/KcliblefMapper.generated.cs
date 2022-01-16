using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KcliblefMapper : EntityMap<Kcliblef>   {
        public KcliblefMapper () {
            Map(p => p.Kaiid).ToColumn("KAIID");
            Map(p => p.Kaicible).ToColumn("KAICIBLE");
            Map(p => p.Kaikahid).ToColumn("KAIKAHID");
            Map(p => p.Kaibra).ToColumn("KAIBRA");
            Map(p => p.Kaisbr).ToColumn("KAISBR");
            Map(p => p.Kaicat).ToColumn("KAICAT");
            Map(p => p.Kaicru).ToColumn("KAICRU");
            Map(p => p.Kaicrd).ToColumn("KAICRD");
            Map(p => p.Kaicrh).ToColumn("KAICRH");
            Map(p => p.Kaimaju).ToColumn("KAIMAJU");
            Map(p => p.Kaimajd).ToColumn("KAIMAJD");
            Map(p => p.Kaimajh).ToColumn("KAIMAJH");
        }
    }
  

}
