using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KFiltrLMapper : EntityMap<KFiltrL>   {
        public KFiltrLMapper () {
            Map(p => p.Kghid).ToColumn("KGHID");
            Map(p => p.Kghkggid).ToColumn("KGHKGGID");
            Map(p => p.Kghfilt).ToColumn("KGHFILT");
            Map(p => p.Kghordr).ToColumn("KGHORDR");
            Map(p => p.Kghactf).ToColumn("KGHACTF");
            Map(p => p.Kghbra).ToColumn("KGHBRA");
            Map(p => p.Kghcible).ToColumn("KGHCIBLE");
        }
    }
  

}
