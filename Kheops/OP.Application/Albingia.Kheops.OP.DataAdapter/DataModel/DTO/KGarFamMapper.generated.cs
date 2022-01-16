using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KGarFamMapper : EntityMap<KGarFam>   {
        public KGarFamMapper () {
            Map(p => p.Gvgar).ToColumn("GVGAR");
            Map(p => p.Gvbra).ToColumn("GVBRA");
            Map(p => p.Gvsbr).ToColumn("GVSBR");
            Map(p => p.Gvcat).ToColumn("GVCAT");
            Map(p => p.Gvfam).ToColumn("GVFAM");
        }
    }
  

}
