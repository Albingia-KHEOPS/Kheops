using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YprtFooMapper : EntityMap<YprtFoo>   {
        public YprtFooMapper () {
            Map(p => p.Jpipb).ToColumn("JPIPB");
            Map(p => p.Jpalx).ToColumn("JPALX");
            Map(p => p.Jpavn).ToColumn("JPAVN");
            Map(p => p.Jphin).ToColumn("JPHIN");
            Map(p => p.Jprsq).ToColumn("JPRSQ");
            Map(p => p.Jpfor).ToColumn("JPFOR");
            Map(p => p.Jpobj).ToColumn("JPOBJ");
        }
    }
  

}
