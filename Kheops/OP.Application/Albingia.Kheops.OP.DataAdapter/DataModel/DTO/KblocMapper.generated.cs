using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KblocMapper : EntityMap<Kbloc>   {
        public KblocMapper () {
            Map(p => p.Kaeid).ToColumn("KAEID");
            Map(p => p.Kaebloc).ToColumn("KAEBLOC");
            Map(p => p.Kaedesc).ToColumn("KAEDESC");
            Map(p => p.Kaecru).ToColumn("KAECRU");
            Map(p => p.Kaecrd).ToColumn("KAECRD");
            Map(p => p.Kaecrh).ToColumn("KAECRH");
            Map(p => p.Kaemaju).ToColumn("KAEMAJU");
            Map(p => p.Kaemajd).ToColumn("KAEMAJD");
            Map(p => p.Kaemajh).ToColumn("KAEMAJH");
        }
    }
  

}
