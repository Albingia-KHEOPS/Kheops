using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpDocLDMapper : EntityMap<KpDocLD>   {
        public KpDocLDMapper () {
            Map(p => p.Kemid).ToColumn("KEMID");
            Map(p => p.Kemkelid).ToColumn("KEMKELID");
            Map(p => p.Kemord).ToColumn("KEMORD");
            Map(p => p.Kemtypd).ToColumn("KEMTYPD");
            Map(p => p.Kemtypl).ToColumn("KEMTYPL");
            Map(p => p.Kemtyenv).ToColumn("KEMTYENV");
            Map(p => p.Kemtamp).ToColumn("KEMTAMP");
            Map(p => p.Kemtyds).ToColumn("KEMTYDS");
            Map(p => p.Kemtyi).ToColumn("KEMTYI");
            Map(p => p.Kemids).ToColumn("KEMIDS");
            Map(p => p.Kemdstp).ToColumn("KEMDSTP");
            Map(p => p.Keminl).ToColumn("KEMINL");
            Map(p => p.Kemnbex).ToColumn("KEMNBEX");
            Map(p => p.Kemdoca).ToColumn("KEMDOCA");
            Map(p => p.Kemtydif).ToColumn("KEMTYDIF");
            Map(p => p.Kemlmai).ToColumn("KEMLMAI");
            Map(p => p.Kemaemo).ToColumn("KEMAEMO");
            Map(p => p.Kemaem).ToColumn("KEMAEM");
            Map(p => p.Kemkesid).ToColumn("KEMKESID");
            Map(p => p.Kemnta).ToColumn("KEMNTA");
            Map(p => p.Kemstu).ToColumn("KEMSTU");
            Map(p => p.Kemsit).ToColumn("KEMSIT");
            Map(p => p.Kemstd).ToColumn("KEMSTD");
            Map(p => p.Kemsth).ToColumn("KEMSTH");
            Map(p => p.Kemenvu).ToColumn("KEMENVU");
            Map(p => p.Kemenvd).ToColumn("KEMENVD");
            Map(p => p.Kemenvh).ToColumn("KEMENVH");
        }
    }
}
