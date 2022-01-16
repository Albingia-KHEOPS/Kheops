using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpExpFrhMapper : EntityMap<KpExpFrh>   {
        public KpExpFrhMapper () {
            Map(p => p.Kdkid).ToColumn("KDKID");
            Map(p => p.Kdktyp).ToColumn("KDKTYP");
            Map(p => p.Kdkipb).ToColumn("KDKIPB");
            Map(p => p.Kdkalx).ToColumn("KDKALX");
            Map(p => p.Kdkavn).ToColumn("KDKAVN");
            Map(p => p.Kdkhin).ToColumn("KDKHIN");
            Map(p => p.Kdkfhe).ToColumn("KDKFHE");
            Map(p => p.Kdkdesc).ToColumn("KDKDESC");
            Map(p => p.Kdkdesi).ToColumn("KDKDESI");
            Map(p => p.Kdkori).ToColumn("KDKORI");
            Map(p => p.Kdkmodi).ToColumn("KDKMODI");
        }
    }
  

}
