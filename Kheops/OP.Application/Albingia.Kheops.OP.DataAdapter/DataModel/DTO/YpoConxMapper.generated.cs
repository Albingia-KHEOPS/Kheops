using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YpoConxMapper : EntityMap<YpoConx>   {
        public YpoConxMapper () {
            Map(p => p.Pjtyp).ToColumn("PJTYP");
            Map(p => p.Pjccx).ToColumn("PJCCX");
            Map(p => p.Pjcnx).ToColumn("PJCNX");
            Map(p => p.Pjipb).ToColumn("PJIPB");
            Map(p => p.Pjalx).ToColumn("PJALX");
            Map(p => p.Pjavn).ToColumn("PJAVN");
            Map(p => p.Pjhin).ToColumn("PJHIN");
            Map(p => p.Pjbra).ToColumn("PJBRA");
            Map(p => p.Pjsbr).ToColumn("PJSBR");
            Map(p => p.Pjcat).ToColumn("PJCAT");
            Map(p => p.Pjobs).ToColumn("PJOBS");
            Map(p => p.Pjide).ToColumn("PJIDE");
        }
    }
  

}
