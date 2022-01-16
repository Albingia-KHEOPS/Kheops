using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpCopidMapper : EntityMap<KpCopid>   {
        public KpCopidMapper () {
            Map(p => p.Kfltyp).ToColumn("KFLTYP");
            Map(p => p.Kflipb).ToColumn("KFLIPB");
            Map(p => p.Kflalx).ToColumn("KFLALX");
            Map(p => p.Kfltab).ToColumn("KFLTAB");
            Map(p => p.Kflido).ToColumn("KFLIDO");
            Map(p => p.Kflidc).ToColumn("KFLIDC");
        }
    }
  

}
