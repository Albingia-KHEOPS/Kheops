using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KcatvoletMapper : EntityMap<Kcatvolet>   {
        public KcatvoletMapper () {
            Map(p => p.Kapid).ToColumn("KAPID");
            Map(p => p.Kapbra).ToColumn("KAPBRA");
            Map(p => p.Kapcible).ToColumn("KAPCIBLE");
            Map(p => p.Kapkaiid).ToColumn("KAPKAIID");
            Map(p => p.Kapvolet).ToColumn("KAPVOLET");
            Map(p => p.Kapkakid).ToColumn("KAPKAKID");
            Map(p => p.Kapcar).ToColumn("KAPCAR");
            Map(p => p.Kapordre).ToColumn("KAPORDRE");
            Map(p => p.Kapcru).ToColumn("KAPCRU");
            Map(p => p.Kapcrd).ToColumn("KAPCRD");
            Map(p => p.Kapcrh).ToColumn("KAPCRH");
            Map(p => p.Kapmaju).ToColumn("KAPMAJU");
            Map(p => p.Kapmajd).ToColumn("KAPMAJD");
            Map(p => p.Kapmajh).ToColumn("KAPMAJH");
        }
    }
  

}
