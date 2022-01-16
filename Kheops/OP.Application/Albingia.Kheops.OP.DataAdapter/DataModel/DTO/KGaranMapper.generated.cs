using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KGaranMapper : EntityMap<KGaran>   {
        public KGaranMapper () {
            Map(p => p.Gagar).ToColumn("GAGAR");
            Map(p => p.Gades).ToColumn("GADES");
            Map(p => p.Gadea).ToColumn("GADEA");
            Map(p => p.Gatax).ToColumn("GATAX");
            Map(p => p.Gacnx).ToColumn("GACNX");
            Map(p => p.Gacom).ToColumn("GACOM");
            Map(p => p.Gacar).ToColumn("GACAR");
            Map(p => p.Gadfg).ToColumn("GADFG");
            Map(p => p.Gaifc).ToColumn("GAIFC");
            Map(p => p.Gafam).ToColumn("GAFAM");
            Map(p => p.Garge).ToColumn("GARGE");
            Map(p => p.Gatrg).ToColumn("GATRG");
            Map(p => p.Gainv).ToColumn("GAINV");
            Map(p => p.Gatyi).ToColumn("GATYI");
            Map(p => p.Garut).ToColumn("GARUT");
            Map(p => p.Gasta).ToColumn("GASTA");
            Map(p => p.Gaatg).ToColumn("GAATG");
        }
    }
  

}
