using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KExpFrhDMapper : EntityMap<KExpFrhD>   {
        public KExpFrhDMapper () {
            Map(p => p.Khfid).ToColumn("KHFID");
            Map(p => p.Khfkheid).ToColumn("KHFKHEID");
            Map(p => p.Khfordre).ToColumn("KHFORDRE");
            Map(p => p.Khffhval).ToColumn("KHFFHVAL");
            Map(p => p.Khffhvau).ToColumn("KHFFHVAU");
            Map(p => p.Khfbase).ToColumn("KHFBASE");
            Map(p => p.Khfind).ToColumn("KHFIND");
            Map(p => p.Khfivo).ToColumn("KHFIVO");
            Map(p => p.Khffhmini).ToColumn("KHFFHMINI");
            Map(p => p.Khffhmaxi).ToColumn("KHFFHMAXI");
            Map(p => p.Khflimdeb).ToColumn("KHFLIMDEB");
            Map(p => p.Khflimfin).ToColumn("KHFLIMFIN");
        }
    }
  

}
