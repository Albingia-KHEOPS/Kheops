using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpExpLCIMapper : EntityMap<KpExpLCI>   {
        public KpExpLCIMapper () {
            Map(p => p.Kdiid).ToColumn("KDIID");
            Map(p => p.Kdityp).ToColumn("KDITYP");
            Map(p => p.Kdiipb).ToColumn("KDIIPB");
            Map(p => p.Kdialx).ToColumn("KDIALX");
            Map(p => p.Kdiavn).ToColumn("KDIAVN");
            Map(p => p.Kdihin).ToColumn("KDIHIN");
            Map(p => p.Kdilce).ToColumn("KDILCE");
            Map(p => p.Kdidesc).ToColumn("KDIDESC");
            Map(p => p.Kdidesi).ToColumn("KDIDESI");
            Map(p => p.Kdiori).ToColumn("KDIORI");
            Map(p => p.Kdimodi).ToColumn("KDIMODI");
        }
    }
  

}
