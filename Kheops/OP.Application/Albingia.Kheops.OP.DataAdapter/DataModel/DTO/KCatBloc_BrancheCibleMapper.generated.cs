using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public  class KCatBloc_BrancheCibleMapper : EntityMap<KCatBloc_BrancheCible> {
        public KCatBloc_BrancheCibleMapper () {
            Map(p => p.Kaqid).ToColumn("Kaqid");
            Map(p => p.Kaqbra).ToColumn("Kaqbra");
            Map(p => p.Kaqcible).ToColumn("Kaqcible");
            Map(p => p.Kaqbloc).ToColumn("Kaqbloc");
            Map(p => p.Kaqkaeid).ToColumn("Kaqkaeid");
            Map(p => p.Kaqcar).ToColumn("Kaqcar");
            Map(p => p.Kaqordre).ToColumn("Kaqordre");
            Map(p => p.Kaedesc).ToColumn("Kaedesc");
            Map(p => p.Kaqkapid).ToColumn("Kaqkapid");
        }
    }
  

}
