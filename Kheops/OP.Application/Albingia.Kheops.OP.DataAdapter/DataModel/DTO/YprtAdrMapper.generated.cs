using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YprtAdrMapper : EntityMap<YprtAdr>   {
        public YprtAdrMapper () {
            Map(p => p.Jfipb).ToColumn("JFIPB");
            Map(p => p.Jfalx).ToColumn("JFALX");
            Map(p => p.Jfavn).ToColumn("JFAVN");
            Map(p => p.Jhhin).ToColumn("JHHIN");
            Map(p => p.Jfrsq).ToColumn("JFRSQ");
            Map(p => p.Jfobj).ToColumn("JFOBJ");
            Map(p => p.Jfad1).ToColumn("JFAD1");
            Map(p => p.Jfad2).ToColumn("JFAD2");
            Map(p => p.Jfdep).ToColumn("JFDEP");
            Map(p => p.Jfcpo).ToColumn("JFCPO");
            Map(p => p.Jfvil).ToColumn("JFVIL");
            Map(p => p.Jfpay).ToColumn("JFPAY");
            Map(p => p.Jfadh).ToColumn("JFADH");
        }
    }
  

}
