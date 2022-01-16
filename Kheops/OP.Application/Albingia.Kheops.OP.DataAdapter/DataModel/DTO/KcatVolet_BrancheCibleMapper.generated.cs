using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public  class KcatVolet_BrancheCibleMapper : EntityMap<KcatVolet_BrancheCible> {
        public KcatVolet_BrancheCibleMapper () {
            Map(p => p.Kapid).ToColumn("Kapid");
            Map(p => p.Kapbra).ToColumn("Kapbra");
            Map(p => p.Kapcible).ToColumn("Kapcible");
            Map(p => p.Kapkaiid).ToColumn("Kapkaiid");
            Map(p => p.Kapvolet).ToColumn("Kapvolet");
            Map(p => p.Kapkakid).ToColumn("Kapkakid");
            Map(p => p.Kapcar).ToColumn("Kapcar");
            Map(p => p.Kapordre).ToColumn("Kapordre");
            Map(p => p.Kakdesc).ToColumn("Kakdesc");
        }
    }
  

}
