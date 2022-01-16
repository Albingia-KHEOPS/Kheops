using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YpoAssuMapper : EntityMap<YpoAssu>   {
        public YpoAssuMapper () {
            Map(p => p.Pcipb).ToColumn("PCIPB");
            Map(p => p.Pcalx).ToColumn("PCALX");
            Map(p => p.Pcavn).ToColumn("PCAVN");
            Map(p => p.Pchin).ToColumn("PCHIN");
            Map(p => p.Pcias).ToColumn("PCIAS");
            Map(p => p.Pcpri).ToColumn("PCPRI");
            Map(p => p.Pcql1).ToColumn("PCQL1");
            Map(p => p.Pcql2).ToColumn("PCQL2");
            Map(p => p.Pcql3).ToColumn("PCQL3");
            Map(p => p.Pcqld).ToColumn("PCQLD");
            Map(p => p.Pccnr).ToColumn("PCCNR");
            Map(p => p.Pcass).ToColumn("PCASS");
            Map(p => p.Pcscp).ToColumn("PCSCP");
            Map(p => p.Pctyp).ToColumn("PCTYP");
            Map(p => p.Pcdesi).ToColumn("PCDESI");
        }
    }
  

}
