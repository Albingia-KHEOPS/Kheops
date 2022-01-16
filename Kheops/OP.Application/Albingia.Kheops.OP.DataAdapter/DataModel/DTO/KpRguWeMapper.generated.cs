using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpRguWeMapper : EntityMap<KpRguWe>   {
        public KpRguWeMapper () {
            Map(p => p.Khztyp).ToColumn("KHZTYP");
            Map(p => p.Khzipb).ToColumn("KHZIPB");
            Map(p => p.Khzalx).ToColumn("KHZALX");
            Map(p => p.Khzrsq).ToColumn("KHZRSQ");
            Map(p => p.Khzfor).ToColumn("KHZFOR");
            Map(p => p.Khzkdeid).ToColumn("KHZKDEID");
            Map(p => p.Khzgaran).ToColumn("KHZGARAN");
            Map(p => p.Khzipk).ToColumn("KHZIPK");
            Map(p => p.Khzmht).ToColumn("KHZMHT");
            Map(p => p.Khzmtx).ToColumn("KHZMTX");
            Map(p => p.Khzaht).ToColumn("KHZAHT");
        }
    }
  

}
