using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpCopDcMapper : EntityMap<KpCopDc>   {
        public KpCopDcMapper () {
            Map(p => p.Khqtyp).ToColumn("KHQTYP");
            Map(p => p.Khqipb).ToColumn("KHQIPB");
            Map(p => p.Khqalx).ToColumn("KHQALX");
            Map(p => p.Khqavn).ToColumn("KHQAVN");
            Map(p => p.Khqoldc).ToColumn("KHQOLDC");
            Map(p => p.Khqcode).ToColumn("KHQCODE");
            Map(p => p.Khqnomd).ToColumn("KHQNOMD");
            Map(p => p.Khqtable).ToColumn("KHQTABLE");
            Map(p => p.Khqoldid).ToColumn("KHQOLDID");
        }
    }
}
