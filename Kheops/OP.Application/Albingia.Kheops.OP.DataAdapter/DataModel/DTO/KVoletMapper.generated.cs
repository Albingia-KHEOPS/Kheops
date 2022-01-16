using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KVoletMapper : EntityMap<KVolet>   {
        public KVoletMapper () {
            Map(p => p.Kakid).ToColumn("KAKID");
            Map(p => p.Kakvolet).ToColumn("KAKVOLET");
            Map(p => p.Kakdesc).ToColumn("KAKDESC");
            Map(p => p.Kakcru).ToColumn("KAKCRU");
            Map(p => p.Kakcrd).ToColumn("KAKCRD");
            Map(p => p.Kakcrh).ToColumn("KAKCRH");
            Map(p => p.Kakmaju).ToColumn("KAKMAJU");
            Map(p => p.Kakmajd).ToColumn("KAKMAJD");
            Map(p => p.Kakmajh).ToColumn("KAKMAJH");
            Map(p => p.Kakbra).ToColumn("KAKBRA");
            Map(p => p.Kakfgen).ToColumn("KAKFGEN");
            Map(p => p.Kakpres).ToColumn("KAKPRES");
        }
    }
  

}
