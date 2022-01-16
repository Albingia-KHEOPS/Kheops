using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YAssNomMapper : EntityMap<YAssNom>   {
        public YAssNomMapper () {
            Map(p => p.Anias).ToColumn("ANIAS");
            Map(p => p.Aninl).ToColumn("ANINL");
            Map(p => p.Antnm).ToColumn("ANTNM");
            Map(p => p.Anord).ToColumn("ANORD");
            Map(p => p.Annom).ToColumn("ANNOM");
            Map(p => p.Antit).ToColumn("ANTIT");
        }
    }
  

}
