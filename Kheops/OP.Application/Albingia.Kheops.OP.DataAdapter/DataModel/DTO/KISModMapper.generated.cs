using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KISModMapper : EntityMap<KISMod>   {
        public KISModMapper () {
            Map(p => p.Kgcmodid).ToColumn("KGCMODID");
            Map(p => p.Kgcdesc).ToColumn("KGCDESC");
            Map(p => p.Kgcdatd).ToColumn("KGCDATD");
            Map(p => p.Kgcdatf).ToColumn("KGCDATF");
            Map(p => p.Kgcselect).ToColumn("KGCSELECT");
            Map(p => p.Kgcinsert).ToColumn("KGCINSERT");
            Map(p => p.Kgcupdate).ToColumn("KGCUPDATE");
            Map(p => p.Kgcexist).ToColumn("KGCEXIST");
            Map(p => p.Kgccru).ToColumn("KGCCRU");
            Map(p => p.Kgccrd).ToColumn("KGCCRD");
            Map(p => p.Kgcmju).ToColumn("KGCMJU");
            Map(p => p.Kgcmjd).ToColumn("KGCMJD");
            Map(p => p.Kgcsaid2).ToColumn("KGCSAID2");
            Map(p => p.Kgcscid2).ToColumn("KGCSCID2");
        }
    }
  

}
