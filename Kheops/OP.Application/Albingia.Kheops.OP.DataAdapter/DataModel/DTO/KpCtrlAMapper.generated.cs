using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpCtrlAMapper : EntityMap<KpCtrlA>   {
        public KpCtrlAMapper () {
            Map(p => p.Kgttyp).ToColumn("KGTTYP");
            Map(p => p.Kgtipb).ToColumn("KGTIPB");
            Map(p => p.Kgtalx).ToColumn("KGTALX");
            Map(p => p.Kgtetape).ToColumn("KGTETAPE");
            Map(p => p.Kgtlib).ToColumn("KGTLIB");
            Map(p => p.Kgtcru).ToColumn("KGTCRU");
            Map(p => p.Kgtcrd).ToColumn("KGTCRD");
            Map(p => p.Kgtcrh).ToColumn("KGTCRH");
            Map(p => p.Kgtmaju).ToColumn("KGTMAJU");
            Map(p => p.Kgtmajd).ToColumn("KGTMAJD");
            Map(p => p.Kgtmajh).ToColumn("KGTMAJH");
        }
    }
  

}
