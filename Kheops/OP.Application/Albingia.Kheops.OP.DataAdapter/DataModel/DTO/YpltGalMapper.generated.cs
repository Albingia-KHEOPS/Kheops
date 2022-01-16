using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YpltGalMapper : EntityMap<YpltGal>   {
        public YpltGalMapper () {
            Map(p => p.C4seq).ToColumn("C4SEQ");
            Map(p => p.C4typ).ToColumn("C4TYP");
            Map(p => p.C4bas).ToColumn("C4BAS");
            Map(p => p.C4val).ToColumn("C4VAL");
            Map(p => p.C4unt).ToColumn("C4UNT");
            Map(p => p.C4maj).ToColumn("C4MAJ");
            Map(p => p.C4obl).ToColumn("C4OBL");
            Map(p => p.C4ala).ToColumn("C4ALA");
        }
    }
  

}
