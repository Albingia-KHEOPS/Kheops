using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KCanevMapper : EntityMap<KCanev>   {
        public KCanevMapper () {
            Map(p => p.Kgoid).ToColumn("KGOID");
            Map(p => p.Kgotyp).ToColumn("KGOTYP");
            Map(p => p.Kgocnva).ToColumn("KGOCNVA");
            Map(p => p.Kgodesc).ToColumn("KGODESC");
            Map(p => p.Kgokaiid).ToColumn("KGOKAIID");
            Map(p => p.Kgocdef).ToColumn("KGOCDEF");
            Map(p => p.Kgocru).ToColumn("KGOCRU");
            Map(p => p.Kgocrd).ToColumn("KGOCRD");
            Map(p => p.Kgocrh).ToColumn("KGOCRH");
            Map(p => p.Kgomaju).ToColumn("KGOMAJU");
            Map(p => p.Kgomajd).ToColumn("KGOMAJD");
            Map(p => p.Kgomajh).ToColumn("KGOMAJH");
            Map(p => p.Kgosit).ToColumn("KGOSIT");
        }
    }
}
