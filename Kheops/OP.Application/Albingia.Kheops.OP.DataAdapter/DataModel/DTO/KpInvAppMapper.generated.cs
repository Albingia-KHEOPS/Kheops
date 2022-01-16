using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpInvAppMapper : EntityMap<KpInvApp>   {
        public KpInvAppMapper () {
            Map(p => p.Kbgtyp).ToColumn("KBGTYP");
            Map(p => p.Kbgipb).ToColumn("KBGIPB");
            Map(p => p.Kbgalx).ToColumn("KBGALX");
            Map(p => p.Kbgavn).ToColumn("KBGAVN");
            Map(p => p.Kbghin).ToColumn("KBGHIN");
            Map(p => p.Kbgnum).ToColumn("KBGNUM");
            Map(p => p.Kbgkbeid).ToColumn("KBGKBEID");
            Map(p => p.Kbgperi).ToColumn("KBGPERI");
            Map(p => p.Kbgrsq).ToColumn("KBGRSQ");
            Map(p => p.Kbgobj).ToColumn("KBGOBJ");
            Map(p => p.Kbgfor).ToColumn("KBGFOR");
            Map(p => p.Kbggar).ToColumn("KBGGAR");
        }
    }
  

}
