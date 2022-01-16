using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpCtrlMapper : EntityMap<KpCtrl>   {
        public KpCtrlMapper () {
            Map(p => p.Keuid).ToColumn("KEUID");
            Map(p => p.Keutyp).ToColumn("KEUTYP");
            Map(p => p.Keuipb).ToColumn("KEUIPB");
            Map(p => p.Keualx).ToColumn("KEUALX");
            Map(p => p.Keuavn).ToColumn("KEUAVN");
            Map(p => p.Keuhin).ToColumn("KEUHIN");
            Map(p => p.Keuetape).ToColumn("KEUETAPE");
            Map(p => p.Keuetord).ToColumn("KEUETORD");
            Map(p => p.Keuordr).ToColumn("KEUORDR");
            Map(p => p.Keuperi).ToColumn("KEUPERI");
            Map(p => p.Keursq).ToColumn("KEURSQ");
            Map(p => p.Keuobj).ToColumn("KEUOBJ");
            Map(p => p.Keuinven).ToColumn("KEUINVEN");
            Map(p => p.Keuinlgn).ToColumn("KEUINLGN");
            Map(p => p.Keufor).ToColumn("KEUFOR");
            Map(p => p.Keuopt).ToColumn("KEUOPT");
            Map(p => p.Keugar).ToColumn("KEUGAR");
            Map(p => p.Keumsg).ToColumn("KEUMSG");
            Map(p => p.Keunivm).ToColumn("KEUNIVM");
            Map(p => p.Keucru).ToColumn("KEUCRU");
            Map(p => p.Keucrd).ToColumn("KEUCRD");
            Map(p => p.Keucrh).ToColumn("KEUCRH");
        }
    }
  

}
