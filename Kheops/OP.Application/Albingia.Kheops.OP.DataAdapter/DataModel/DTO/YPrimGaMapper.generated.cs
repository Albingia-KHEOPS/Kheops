using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YPrimGaMapper : EntityMap<YPrimGa>   {
        public YPrimGaMapper () {
            Map(p => p.Plipb).ToColumn("PLIPB");
            Map(p => p.Plalx).ToColumn("PLALX");
            Map(p => p.Plipk).ToColumn("PLIPK");
            Map(p => p.Pltye).ToColumn("PLTYE");
            Map(p => p.Plgar).ToColumn("PLGAR");
            Map(p => p.Plmht).ToColumn("PLMHT");
            Map(p => p.Plmtx).ToColumn("PLMTX");
            Map(p => p.Pltax).ToColumn("PLTAX");
            Map(p => p.Pltxv).ToColumn("PLTXV");
            Map(p => p.Pltxu).ToColumn("PLTXU");
            Map(p => p.Plmtt).ToColumn("PLMTT");
            Map(p => p.Plxf1).ToColumn("PLXF1");
            Map(p => p.Plmx1).ToColumn("PLMX1");
            Map(p => p.Plxf2).ToColumn("PLXF2");
            Map(p => p.Plmx2).ToColumn("PLMX2");
            Map(p => p.Plxf3).ToColumn("PLXF3");
            Map(p => p.Plmx3).ToColumn("PLMX3");
            Map(p => p.Plsup).ToColumn("PLSUP");
            Map(p => p.Plgap).ToColumn("PLGAP");
            Map(p => p.Plkht).ToColumn("PLKHT");
            Map(p => p.Plkhx).ToColumn("PLKHX");
            Map(p => p.Plktt).ToColumn("PLKTT");
            Map(p => p.Plkx1).ToColumn("PLKX1");
            Map(p => p.Plkx2).ToColumn("PLKX2");
            Map(p => p.Plkx3).ToColumn("PLKX3");
            Map(p => p.Plgrh).ToColumn("PLGRH");
            Map(p => p.Plkrh).ToColumn("PLKRH");
        }
    }
}
