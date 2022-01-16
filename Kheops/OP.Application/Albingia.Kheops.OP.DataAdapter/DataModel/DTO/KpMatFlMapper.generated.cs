using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpMatFlMapper : EntityMap<KpMatFl>   {
        public KpMatFlMapper () {
            Map(p => p.Kectyp).ToColumn("KECTYP");
            Map(p => p.Kecipb).ToColumn("KECIPB");
            Map(p => p.Kecalx).ToColumn("KECALX");
            Map(p => p.Kecavn).ToColumn("KECAVN");
            Map(p => p.Kechin).ToColumn("KECHIN");
            Map(p => p.Keckeachr).ToColumn("KECKEACHR");
            Map(p => p.Keckebchr).ToColumn("KECKEBCHR");
            Map(p => p.Kecico).ToColumn("KECICO");
        }
    }
  

}
