using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpOptApMapper : EntityMap<KpOptAp>   {
        public KpOptApMapper () {
            Map(p => p.Kddid).ToColumn("KDDID");
            Map(p => p.Kddtyp).ToColumn("KDDTYP");
            Map(p => p.Kddipb).ToColumn("KDDIPB");
            Map(p => p.Kddalx).ToColumn("KDDALX");
            Map(p => p.Kddavn).ToColumn("KDDAVN");
            Map(p => p.Kddhin).ToColumn("KDDHIN");
            Map(p => p.Kddfor).ToColumn("KDDFOR");
            Map(p => p.Kddopt).ToColumn("KDDOPT");
            Map(p => p.Kddkdbid).ToColumn("KDDKDBID");
            Map(p => p.Kddperi).ToColumn("KDDPERI");
            Map(p => p.Kddrsq).ToColumn("KDDRSQ");
            Map(p => p.Kddobj).ToColumn("KDDOBJ");
            Map(p => p.Kddinven).ToColumn("KDDINVEN");
            Map(p => p.Kddinvep).ToColumn("KDDINVEP");
            Map(p => p.Kddcru).ToColumn("KDDCRU");
            Map(p => p.Kddcrd).ToColumn("KDDCRD");
            Map(p => p.Kddmaju).ToColumn("KDDMAJU");
            Map(p => p.Kddmajd).ToColumn("KDDMAJD");
        }
    }
  

}
