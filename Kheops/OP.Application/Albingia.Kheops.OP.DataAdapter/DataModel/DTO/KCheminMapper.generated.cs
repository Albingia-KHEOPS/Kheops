using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KCheminMapper : EntityMap<KChemin>   {
        public KCheminMapper () {
            Map(p => p.Khmcle).ToColumn("KHMCLE");
            Map(p => p.Khmsrv).ToColumn("KHMSRV");
            Map(p => p.Khmrac).ToColumn("KHMRAC");
            Map(p => p.Khmenv).ToColumn("KHMENV");
            Map(p => p.Khmdes).ToColumn("KHMDES");
            Map(p => p.Khmtch).ToColumn("KHMTCH");
            Map(p => p.Khmchm).ToColumn("KHMCHM");
            Map(p => p.Khmcru).ToColumn("KHMCRU");
            Map(p => p.Khmcrd).ToColumn("KHMCRD");
            Map(p => p.Khmmju).ToColumn("KHMMJU");
            Map(p => p.Khmmjd).ToColumn("KHMMJD");
        }
    }
}
