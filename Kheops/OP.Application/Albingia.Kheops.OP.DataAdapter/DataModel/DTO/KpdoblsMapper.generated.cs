using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpdoblsMapper : EntityMap<Kpdobls>   {
        public KpdoblsMapper () {
            Map(p => p.Kafid).ToColumn("KAFID");
            Map(p => p.Kaftyp).ToColumn("KAFTYP");
            Map(p => p.Kafipb).ToColumn("KAFIPB");
            Map(p => p.Kafalx).ToColumn("KAFALX");
            Map(p => p.Kafict).ToColumn("KAFICT");
            Map(p => p.Kafsou).ToColumn("KAFSOU");
            Map(p => p.Kafsaid).ToColumn("KAFSAID");
            Map(p => p.Kafsaih).ToColumn("KAFSAIH");
            Map(p => p.Kafsit).ToColumn("KAFSIT");
            Map(p => p.Kafsitd).ToColumn("KAFSITD");
            Map(p => p.Kafsith).ToColumn("KAFSITH");
            Map(p => p.Kafsitu).ToColumn("KAFSITU");
            Map(p => p.Kafcrd).ToColumn("KAFCRD");
            Map(p => p.Kafcrh).ToColumn("KAFCRH");
            Map(p => p.Kafcru).ToColumn("KAFCRU");
            Map(p => p.Kafact).ToColumn("KAFACT");
            Map(p => p.Kafmot).ToColumn("KAFMOT");
            Map(p => p.Kafin5).ToColumn("KAFIN5");
            Map(p => p.Kafoct).ToColumn("KAFOCT");
        }
    }
  

}
