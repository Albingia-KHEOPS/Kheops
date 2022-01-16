using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpObsvMapper : EntityMap<KpObsv>   {
        public KpObsvMapper () {
            Map(p => p.Kajchr).ToColumn("KAJCHR");
            Map(p => p.Kajtyp).ToColumn("KAJTYP");
            Map(p => p.Kajipb).ToColumn("KAJIPB");
            Map(p => p.Kajalx).ToColumn("KAJALX");
            Map(p => p.Kajavn).ToColumn("KAJAVN");
            Map(p => p.Kajhin).ToColumn("KAJHIN");
            Map(p => p.Kajtypobs).ToColumn("KAJTYPOBS");
            Map(p => p.Kajobsv).ToColumn("KAJOBSV");
        }
    }
  

}
