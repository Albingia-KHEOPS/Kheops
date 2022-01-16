using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YparMapper : EntityMap<Ypar>   {
        public YparMapper () {
            Map(p => p.Tcon).ToColumn("TCON");
            Map(p => p.Tfam).ToColumn("TFAM");
            Map(p => p.Tcod).ToColumn("TCOD");
            Map(p => p.Tplib).ToColumn("TPLIB");
            Map(p => p.Tpcn1).ToColumn("TPCN1");
            Map(p => p.Tpcn2).ToColumn("TPCN2");
            Map(p => p.Tpca1).ToColumn("TPCA1");
            Map(p => p.Tpca2).ToColumn("TPCA2");
            Map(p => p.Tptyp).ToColumn("TPTYP");
            Map(p => p.Tplil).ToColumn("TPLIL");
            Map(p => p.Tfilt).ToColumn("TFILT");
        }
    }
}
