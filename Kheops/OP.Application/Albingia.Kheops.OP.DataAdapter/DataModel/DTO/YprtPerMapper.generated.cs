using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YprtPerMapper : EntityMap<YprtPer>   {
        public YprtPerMapper () {
            Map(p => p.Kaipb).ToColumn("KAIPB");
            Map(p => p.Kaalx).ToColumn("KAALX");
            Map(p => p.Karsq).ToColumn("KARSQ");
            Map(p => p.Kafor).ToColumn("KAFOR");
            Map(p => p.Katyp).ToColumn("KATYP");
            Map(p => p.Kadpa).ToColumn("KADPA");
            Map(p => p.Kadpm).ToColumn("KADPM");
            Map(p => p.Kadpj).ToColumn("KADPJ");
            Map(p => p.Kafpa).ToColumn("KAFPA");
            Map(p => p.Kafpm).ToColumn("KAFPM");
            Map(p => p.Kapfj).ToColumn("KAPFJ");
            Map(p => p.Katpe).ToColumn("KATPE");
            Map(p => p.Kaiva).ToColumn("KAIVA");
            Map(p => p.Kavaa).ToColumn("KAVAA");
            Map(p => p.Kacop).ToColumn("KACOP");
        }
    }
}
