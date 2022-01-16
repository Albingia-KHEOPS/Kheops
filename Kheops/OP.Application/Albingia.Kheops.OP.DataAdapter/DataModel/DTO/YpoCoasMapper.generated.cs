using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YpoCoasMapper : EntityMap<YpoCoas>   {
        public YpoCoasMapper () {
            Map(p => p.Phipb).ToColumn("PHIPB");
            Map(p => p.Phalx).ToColumn("PHALX");
            Map(p => p.Phavn).ToColumn("PHAVN");
            Map(p => p.Phhin).ToColumn("PHHIN");
            Map(p => p.Phtap).ToColumn("PHTAP");
            Map(p => p.Phcie).ToColumn("PHCIE");
            Map(p => p.Phinl).ToColumn("PHINL");
            Map(p => p.Phpol).ToColumn("PHPOL");
            Map(p => p.Phapp).ToColumn("PHAPP");
            Map(p => p.Phcom).ToColumn("PHCOM");
            Map(p => p.Phtxf).ToColumn("PHTXF");
            Map(p => p.Phafr).ToColumn("PHAFR");
            Map(p => p.Phepa).ToColumn("PHEPA");
            Map(p => p.Phepm).ToColumn("PHEPM");
            Map(p => p.Phepj).ToColumn("PHEPJ");
            Map(p => p.Phfpa).ToColumn("PHFPA");
            Map(p => p.Phfpm).ToColumn("PHFPM");
            Map(p => p.Phfpj).ToColumn("PHFPJ");
            Map(p => p.Phin5).ToColumn("PHIN5");
            Map(p => p.Phtac).ToColumn("PHTAC");
            Map(p => p.Phtaa).ToColumn("PHTAA");
            Map(p => p.Phtam).ToColumn("PHTAM");
            Map(p => p.Phtaj).ToColumn("PHTAJ");
            Map(p => p.Phtyp).ToColumn("PHTYP");
        }
    }
  

}
