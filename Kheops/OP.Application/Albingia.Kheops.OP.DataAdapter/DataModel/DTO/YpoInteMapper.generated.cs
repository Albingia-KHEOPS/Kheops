using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YpoInteMapper : EntityMap<YpoInte>   {
        public YpoInteMapper () {
            Map(p => p.Ppipb).ToColumn("PPIPB");
            Map(p => p.Ppalx).ToColumn("PPALX");
            Map(p => p.Ppavn).ToColumn("PPAVN");
            Map(p => p.Pphin).ToColumn("PPHIN");
            Map(p => p.Ppiin).ToColumn("PPIIN");
            Map(p => p.Pptyi).ToColumn("PPTYI");
            Map(p => p.Ppinl).ToColumn("PPINL");
            Map(p => p.Pppol).ToColumn("PPPOL");
            Map(p => p.Ppsym).ToColumn("PPSYM");
        }
    }
  

}
