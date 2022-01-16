using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpDocExtMapper : EntityMap<KpDocExt>   {
        public KpDocExtMapper () {
            Map(p => p.Kerid).ToColumn("KERID");
            Map(p => p.Kertyp).ToColumn("KERTYP");
            Map(p => p.Keripb).ToColumn("KERIPB");
            Map(p => p.Keralx).ToColumn("KERALX");
            Map(p => p.Kersua).ToColumn("KERSUA");
            Map(p => p.Kernum).ToColumn("KERNUM");
            Map(p => p.Kersbr).ToColumn("KERSBR");
            Map(p => p.Kerserv).ToColumn("KERSERV");
            Map(p => p.Keractg).ToColumn("KERACTG");
            Map(p => p.Keravn).ToColumn("KERAVN");
            Map(p => p.Kerord).ToColumn("KERORD");
            Map(p => p.Kertypo).ToColumn("KERTYPO");
            Map(p => p.Kerlib).ToColumn("KERLIB");
            Map(p => p.Kernom).ToColumn("KERNOM");
            Map(p => p.Kerchm).ToColumn("KERCHM");
            Map(p => p.Kerstu).ToColumn("KERSTU");
            Map(p => p.Kersit).ToColumn("KERSIT");
            Map(p => p.Kerstd).ToColumn("KERSTD");
            Map(p => p.Kersth).ToColumn("KERSTH");
            Map(p => p.Kercru).ToColumn("KERCRU");
            Map(p => p.Kercrd).ToColumn("KERCRD");
            Map(p => p.Kercrh).ToColumn("KERCRH");
            Map(p => p.Kerref).ToColumn("KERREF");
        }
    }
  

}
