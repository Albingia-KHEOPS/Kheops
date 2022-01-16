using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpDesiMapper : EntityMap<KpDesi>   {
        public KpDesiMapper () {
            Map(p => p.Kadchr).ToColumn("KADCHR");
            Map(p => p.Kadtyp).ToColumn("KADTYP");
            Map(p => p.Kadipb).ToColumn("KADIPB");
            Map(p => p.Kadalx).ToColumn("KADALX");
            Map(p => p.Kadavn).ToColumn("KADAVN");
            Map(p => p.Kadhin).ToColumn("KADHIN");
            Map(p => p.Kadperi).ToColumn("KADPERI");
            Map(p => p.Kadrsq).ToColumn("KADRSQ");
            Map(p => p.Kadobj).ToColumn("KADOBJ");
            Map(p => p.Kaddesi).ToColumn("KADDESI");
        }
    }
  

}
