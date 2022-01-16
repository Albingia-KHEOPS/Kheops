using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KFiltreMapper : EntityMap<KFiltre>   {
        public KFiltreMapper () {
            Map(p => p.Kggid).ToColumn("KGGID");
            Map(p => p.Kggfilt).ToColumn("KGGFILT");
            Map(p => p.Kggdesc).ToColumn("KGGDESC");
            Map(p => p.Kggcru).ToColumn("KGGCRU");
            Map(p => p.Kggcrd).ToColumn("KGGCRD");
            Map(p => p.Kggcrh).ToColumn("KGGCRH");
            Map(p => p.Kggmaju).ToColumn("KGGMAJU");
            Map(p => p.Kggmajd).ToColumn("KGGMAJD");
            Map(p => p.Kggmajh).ToColumn("KGGMAJH");
        }
    }
  

}
