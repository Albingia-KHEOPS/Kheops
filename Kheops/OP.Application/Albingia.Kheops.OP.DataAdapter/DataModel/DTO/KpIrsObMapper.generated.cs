using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpIrsObMapper : EntityMap<KpIrsOb>   {
        public KpIrsObMapper () {
            Map(p => p.Kfatyp).ToColumn("KFATYP");
            Map(p => p.Kfaipb).ToColumn("KFAIPB");
            Map(p => p.Kfaalx).ToColumn("KFAALX");
            Map(p => p.Kfaavn).ToColumn("KFAAVN");
            Map(p => p.Kfahin).ToColumn("KFAHIN");
            Map(p => p.Kfarsq).ToColumn("KFARSQ");
            Map(p => p.Kfaobj).ToColumn("KFAOBJ");
            Map(p => p.Kfanats).ToColumn("KFANATS");
            Map(p => p.Kfanega).ToColumn("KFANEGA");
            Map(p => p.Kfafrqe).ToColumn("KFAFRQE");
            Map(p => p.Kfanbpa).ToColumn("KFANBPA");
            Map(p => p.Kfanbex).ToColumn("KFANBEX");
            Map(p => p.Kfanbvi).ToColumn("KFANBVI");
            Map(p => p.Kfagn08).ToColumn("KFAGN08");
            Map(p => p.Kfagn09).ToColumn("KFAGN09");
            Map(p => p.Kfagn10).ToColumn("KFAGN10");
            Map(p => p.Kfanbin).ToColumn("KFANBIN");
            Map(p => p.Kfanbpe).ToColumn("KFANBPE");
            Map(p => p.Kfagct).ToColumn("KFAGCT");
            Map(p => p.Kfanbem).ToColumn("KFANBEM");
            Map(p => p.Kfatytn).ToColumn("KFATYTN");
            Map(p => p.Kfanmdf).ToColumn("KFANMDF");
            Map(p => p.Kfafent).ToColumn("KFAFENT");
            Map(p => p.Kfafsvt).ToColumn("KFAFSVT");
            Map(p => p.Kfanmsc).ToColumn("KFANMSC");
            Map(p => p.Kfalabd).ToColumn("KFALABD");
            Map(p => p.Kfanai).ToColumn("KFANAI");
            Map(p => p.Kfalma).ToColumn("KFALMA");
            Map(p => p.Kfaifp).ToColumn("KFAIFP");
            Map(p => p.Kfathf).ToColumn("KFATHF");
            Map(p => p.Kfatu1).ToColumn("KFATU1");
            Map(p => p.Kfatu2).ToColumn("KFATU2");
            Map(p => p.Kfaasc).ToColumn("KFAASC");
            Map(p => p.Kfaautl).ToColumn("KFAAUTL");
            Map(p => p.Kfaqmd).ToColumn("KFAQMD");
            Map(p => p.Kfasurf).ToColumn("KFASURF");
            Map(p => p.Kfavmc).ToColumn("KFAVMC");
            Map(p => p.Kfaprol).ToColumn("KFAPROL");
            Map(p => p.Kfadepd).ToColumn("KFADEPD");
        }
    }
  

}
