using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KnmValfMapper : EntityMap<KnmValf>   {
        public KnmValfMapper () {
            Map(p => p.Khkid).ToColumn("KHKID");
            Map(p => p.Khknmg).ToColumn("KHKNMG");
            Map(p => p.Khktypo).ToColumn("KHKTYPO");
            Map(p => p.Khkordr).ToColumn("KHKORDR");
            Map(p => p.Khkniv).ToColumn("KHKNIV");
            Map(p => p.Khkmer).ToColumn("KHKMER");
            Map(p => p.Khkkhiid).ToColumn("KHKKHIID");
        }
    }
  

}
