using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YAssureMapper : EntityMap<YAssure>   {
        public YAssureMapper () {
            Map(p => p.Asias).ToColumn("ASIAS");
            Map(p => p.Asad1).ToColumn("ASAD1");
            Map(p => p.Asad2).ToColumn("ASAD2");
            Map(p => p.Asdep).ToColumn("ASDEP");
            Map(p => p.Ascpo).ToColumn("ASCPO");
            Map(p => p.Asvil).ToColumn("ASVIL");
            Map(p => p.Aspay).ToColumn("ASPAY");
            Map(p => p.Ascom).ToColumn("ASCOM");
            Map(p => p.Asreg).ToColumn("ASREG");
            Map(p => p.Asfdc).ToColumn("ASFDC");
            Map(p => p.Astel).ToColumn("ASTEL");
            Map(p => p.Astlc).ToColumn("ASTLC");
            Map(p => p.Assir).ToColumn("ASSIR");
            Map(p => p.Asape).ToColumn("ASAPE");
            Map(p => p.Asapg).ToColumn("ASAPG");
            Map(p => p.Aslig).ToColumn("ASLIG");
            Map(p => p.Asgrs).ToColumn("ASGRS");
            Map(p => p.Ascra).ToColumn("ASCRA");
            Map(p => p.Ascrm).ToColumn("ASCRM");
            Map(p => p.Ascrj).ToColumn("ASCRJ");
            Map(p => p.Asusr).ToColumn("ASUSR");
            Map(p => p.Asmja).ToColumn("ASMJA");
            Map(p => p.Asmjm).ToColumn("ASMJM");
            Map(p => p.Asmjj).ToColumn("ASMJJ");
            Map(p => p.Asins).ToColumn("ASINS");
            Map(p => p.Aspub).ToColumn("ASPUB");
            Map(p => p.Asadh).ToColumn("ASADH");
            Map(p => p.Asnic).ToColumn("ASNIC");
            Map(p => p.Asap5).ToColumn("ASAP5");
        }
    }
  

}
