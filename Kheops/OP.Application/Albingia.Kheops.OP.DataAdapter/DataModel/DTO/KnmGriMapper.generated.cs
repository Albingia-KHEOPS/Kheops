using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KnmGriMapper : EntityMap<KnmGri>   {
        public KnmGriMapper () {
            Map(p => p.Khjnmg).ToColumn("KHJNMG");
            Map(p => p.Khjdesi).ToColumn("KHJDESI");
            Map(p => p.Khjtypo1).ToColumn("KHJTYPO1");
            Map(p => p.Khjlib1).ToColumn("KHJLIB1");
            Map(p => p.Khjlien1).ToColumn("KHJLIEN1");
            Map(p => p.Khjvalf1).ToColumn("KHJVALF1");
            Map(p => p.Khjtypo2).ToColumn("KHJTYPO2");
            Map(p => p.Khjlib2).ToColumn("KHJLIB2");
            Map(p => p.Khjlien2).ToColumn("KHJLIEN2");
            Map(p => p.Khjvalf2).ToColumn("KHJVALF2");
            Map(p => p.Khjtypo3).ToColumn("KHJTYPO3");
            Map(p => p.Khjlib3).ToColumn("KHJLIB3");
            Map(p => p.Khjlien3).ToColumn("KHJLIEN3");
            Map(p => p.Khjvalf3).ToColumn("KHJVALF3");
            Map(p => p.Khjtypo4).ToColumn("KHJTYPO4");
            Map(p => p.Khjlib4).ToColumn("KHJLIB4");
            Map(p => p.Khjlien4).ToColumn("KHJLIEN4");
            Map(p => p.Khjvalf4).ToColumn("KHJVALF4");
            Map(p => p.Khjtypo5).ToColumn("KHJTYPO5");
            Map(p => p.Khjlib5).ToColumn("KHJLIB5");
            Map(p => p.Khjlien5).ToColumn("KHJLIEN5");
            Map(p => p.Khjvalf5).ToColumn("KHJVALF5");
        }
    }
  

}
