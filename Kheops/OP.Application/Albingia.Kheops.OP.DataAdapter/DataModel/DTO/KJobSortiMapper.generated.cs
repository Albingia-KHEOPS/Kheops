using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KJobSortiMapper : EntityMap<KJobSorti>   {
        public KJobSortiMapper () {
            Map(p => p.Ipb).ToColumn("IPB");
            Map(p => p.Alx).ToColumn("ALX");
            Map(p => p.Typ).ToColumn("TYP");
            Map(p => p.Avn).ToColumn("AVN");
            Map(p => p.Hin).ToColumn("HIN");
            Map(p => p.Rsq).ToColumn("RSQ");
            Map(p => p.Obj).ToColumn("OBJ");
            Map(p => p.Form).ToColumn("FORM");
            Map(p => p.Opt).ToColumn("OPT");
            Map(p => p.Garan).ToColumn("GARAN");
            Map(p => p.Datedeb).ToColumn("DATEDEB");
            Map(p => p.Heuredeb).ToColumn("HEUREDEB");
            Map(p => p.Datefin).ToColumn("DATEFIN");
            Map(p => p.Heurefin).ToColumn("HEUREFIN");
            Map(p => p.Sorti).ToColumn("SORTI");
        }
    }
  

}
