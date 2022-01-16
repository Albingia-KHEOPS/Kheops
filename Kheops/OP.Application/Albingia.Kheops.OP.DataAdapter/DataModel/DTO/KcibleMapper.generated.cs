using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KcibleMapper : EntityMap<Kcible>   {
        public KcibleMapper () {
            Map(p => p.Kahid).ToColumn("KAHID");
            Map(p => p.Kahcible).ToColumn("KAHCIBLE");
            Map(p => p.Kahdesc).ToColumn("KAHDESC");
            Map(p => p.Kahcru).ToColumn("KAHCRU");
            Map(p => p.Kahcrd).ToColumn("KAHCRD");
            Map(p => p.Kahcrh).ToColumn("KAHCRH");
            Map(p => p.Kahmaju).ToColumn("KAHMAJU");
            Map(p => p.Kahmajd).ToColumn("KAHMAJD");
            Map(p => p.Kahmajh).ToColumn("KAHMAJH");
            Map(p => p.Kahnmg).ToColumn("KAHNMG");
            Map(p => p.Kahcon).ToColumn("KAHCON");
            Map(p => p.Kahfam).ToColumn("KAHFAM");
            Map(p => p.Kahaut).ToColumn("KAHAUT");
        }
    }
  

}
