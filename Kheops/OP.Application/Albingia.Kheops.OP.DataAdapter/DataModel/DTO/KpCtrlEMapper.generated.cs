using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpCtrlEMapper : EntityMap<KpCtrlE>   {
        public KpCtrlEMapper () {
            Map(p => p.Kevtyp).ToColumn("KEVTYP");
            Map(p => p.Kevipb).ToColumn("KEVIPB");
            Map(p => p.Kevalx).ToColumn("KEVALX");
            Map(p => p.Kevavn).ToColumn("KEVAVN");
            Map(p => p.Kevhin).ToColumn("KEVHIN");
            Map(p => p.Kevetape).ToColumn("KEVETAPE");
            Map(p => p.Kevetord).ToColumn("KEVETORD");
            Map(p => p.Kevordr).ToColumn("KEVORDR");
            Map(p => p.Kevperi).ToColumn("KEVPERI");
            Map(p => p.Kevrsq).ToColumn("KEVRSQ");
            Map(p => p.Kevobj).ToColumn("KEVOBJ");
            Map(p => p.Kevkbeid).ToColumn("KEVKBEID");
            Map(p => p.Kevfor).ToColumn("KEVFOR");
            Map(p => p.Kevopt).ToColumn("KEVOPT");
            Map(p => p.Kevnivm).ToColumn("KEVNIVM");
            Map(p => p.Kevcru).ToColumn("KEVCRU");
            Map(p => p.Kevcrd).ToColumn("KEVCRD");
            Map(p => p.Kevcrh).ToColumn("KEVCRH");
            Map(p => p.Kevmaju).ToColumn("KEVMAJU");
            Map(p => p.Kevmajd).ToColumn("KEVMAJD");
            Map(p => p.Kevmajh).ToColumn("KEVMAJH");
            Map(p => p.Kevtag).ToColumn("KEVTAG");
            Map(p => p.Kevtagc).ToColumn("KEVTAGC");
        }
    }
  

}
