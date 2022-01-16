using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpForMapper : EntityMap<KpFor>   {
        public KpForMapper () {
            Map(p => p.Kdaid).ToColumn("KDAID");
            Map(p => p.Kdatyp).ToColumn("KDATYP");
            Map(p => p.Kdaipb).ToColumn("KDAIPB");
            Map(p => p.Kdaalx).ToColumn("KDAALX");
            Map(p => p.Kdaavn).ToColumn("KDAAVN");
            Map(p => p.Kdahin).ToColumn("KDAHIN");
            Map(p => p.Kdafor).ToColumn("KDAFOR");
            Map(p => p.Kdacch).ToColumn("KDACCH");
            Map(p => p.Kdaalpha).ToColumn("KDAALPHA");
            Map(p => p.Kdabra).ToColumn("KDABRA");
            Map(p => p.Kdacible).ToColumn("KDACIBLE");
            Map(p => p.Kdakaiid).ToColumn("KDAKAIID");
            Map(p => p.Kdadesc).ToColumn("KDADESC");
            Map(p => p.Kdacru).ToColumn("KDACRU");
            Map(p => p.Kdacrd).ToColumn("KDACRD");
            Map(p => p.Kdamaju).ToColumn("KDAMAJU");
            Map(p => p.Kdamajd).ToColumn("KDAMAJD");
            Map(p => p.Kdafgen).ToColumn("KDAFGEN");
        }
    }
}
