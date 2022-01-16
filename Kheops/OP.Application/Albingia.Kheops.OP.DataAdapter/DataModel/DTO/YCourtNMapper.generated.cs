using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YCourtNMapper : EntityMap<YCourtN>   {
        public YCourtNMapper () {
            Map(p => p.Tnict).ToColumn("TNICT");
            Map(p => p.Tninl).ToColumn("TNINL");
            Map(p => p.Tntnm).ToColumn("TNTNM");
            Map(p => p.Tnord).ToColumn("TNORD");
            Map(p => p.Tntyp).ToColumn("TNTYP");
            Map(p => p.Tnnom).ToColumn("TNNOM");
            Map(p => p.Tntit).ToColumn("TNTIT");
            Map(p => p.Tnxn5).ToColumn("TNXN5");
        }
    }
}
