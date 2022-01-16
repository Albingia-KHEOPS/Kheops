using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YpltGaaMapper : EntityMap<YpltGaa>   {
        public YpltGaaMapper () {
            Map(p => p.C5typ).ToColumn("C5TYP");
            Map(p => p.C5seq).ToColumn("C5SEQ");
            Map(p => p.C5sem).ToColumn("C5SEM");
        }
    }
  

}
