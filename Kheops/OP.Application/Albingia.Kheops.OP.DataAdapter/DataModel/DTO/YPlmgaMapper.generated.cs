using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YPlmgaMapper : EntityMap<YPlmga>   {
        public YPlmgaMapper () {
            Map(p => p.D1mga).ToColumn("D1MGA");
            Map(p => p.D1lib).ToColumn("D1LIB");
        }
    }
  

}
