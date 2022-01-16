using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpMatGgMapper : EntityMap<KpMatGg>   {
        public KpMatGgMapper () {
            Map(p => p.Keetyp).ToColumn("KEETYP");
            Map(p => p.Keeipb).ToColumn("KEEIPB");
            Map(p => p.Keealx).ToColumn("KEEALX");
            Map(p => p.Keeavn).ToColumn("KEEAVN");
            Map(p => p.Keehin).ToColumn("KEEHIN");
            Map(p => p.Keechr).ToColumn("KEECHR");
            Map(p => p.Keetye).ToColumn("KEETYE");
            Map(p => p.Keekdcid).ToColumn("KEEKDCID");
            Map(p => p.Keevolet).ToColumn("KEEVOLET");
            Map(p => p.Keekakid).ToColumn("KEEKAKID");
            Map(p => p.Keebloc).ToColumn("KEEBLOC");
            Map(p => p.Keekaeid).ToColumn("KEEKAEID");
            Map(p => p.Keegaran).ToColumn("KEEGARAN");
            Map(p => p.Keeseq).ToColumn("KEESEQ");
            Map(p => p.Keeniv).ToColumn("KEENIV");
            Map(p => p.Keevid).ToColumn("KEEVID");
        }
    }
  

}
