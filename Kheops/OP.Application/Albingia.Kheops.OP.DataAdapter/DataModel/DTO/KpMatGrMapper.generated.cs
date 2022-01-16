using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpMatGrMapper : EntityMap<KpMatGr>   {
        public KpMatGrMapper () {
            Map(p => p.Kedtyp).ToColumn("KEDTYP");
            Map(p => p.Kedipb).ToColumn("KEDIPB");
            Map(p => p.Kedalx).ToColumn("KEDALX");
            Map(p => p.Kedavn).ToColumn("KEDAVN");
            Map(p => p.Kedhin).ToColumn("KEDHIN");
            Map(p => p.Kedchr).ToColumn("KEDCHR");
            Map(p => p.Kedrsq).ToColumn("KEDRSQ");
        }
    }
  

}
