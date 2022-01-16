using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KUsrDrtMapper : EntityMap<KUsrDrt>   {
        public KUsrDrtMapper () {
            Map(p => p.Khrusr).ToColumn("KHRUSR");
            Map(p => p.Khrbra).ToColumn("KHRBRA");
            Map(p => p.Khrtyd).ToColumn("KHRTYD");
        }
    }
  

}
