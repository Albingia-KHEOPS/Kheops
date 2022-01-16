using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KClauseMapper : EntityMap<KClause>   {
        public KClauseMapper () {
            Map(p => p.Kduid).ToColumn("KDUID");
            Map(p => p.Kdunm1).ToColumn("KDUNM1");
            Map(p => p.Kdunm2).ToColumn("KDUNM2");
            Map(p => p.Kdunm3).ToColumn("KDUNM3");
            Map(p => p.Kduver).ToColumn("KDUVER");
            Map(p => p.Kdulib).ToColumn("KDULIB");
            Map(p => p.Kdulir).ToColumn("KDULIR");
            Map(p => p.Kdukdwid).ToColumn("KDUKDWID");
            Map(p => p.Kdukdvid).ToColumn("KDUKDVID");
            Map(p => p.Kdukdxid).ToColumn("KDUKDXID");
            Map(p => p.Kdudatd).ToColumn("KDUDATD");
            Map(p => p.Kdudatf).ToColumn("KDUDATF");
            Map(p => p.Kdudoc).ToColumn("KDUDOC");
            Map(p => p.Kdutdoc).ToColumn("KDUTDOC");
            Map(p => p.Kduserv).ToColumn("KDUSERV");
            Map(p => p.Kduactg).ToColumn("KDUACTG");
            Map(p => p.Kducru).ToColumn("KDUCRU");
            Map(p => p.Kducrd).ToColumn("KDUCRD");
            Map(p => p.Kducrh).ToColumn("KDUCRH");
            Map(p => p.Kdumaju).ToColumn("KDUMAJU");
            Map(p => p.Kdumajd).ToColumn("KDUMAJD");
            Map(p => p.Kdumajh).ToColumn("KDUMAJH");
            Map(p => p.Kdurgp).ToColumn("KDURGP");
            Map(p => p.Kduord).ToColumn("KDUORD");
            Map(p => p.Kduanx).ToColumn("KDUANX");
            Map(p => p.Kduora).ToColumn("KDUORA");
        }
    }
}
