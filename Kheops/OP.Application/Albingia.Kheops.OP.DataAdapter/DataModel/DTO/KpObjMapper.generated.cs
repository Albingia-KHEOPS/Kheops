using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpObjMapper : EntityMap<KpObj>   {
        public KpObjMapper () {
            Map(p => p.Kactyp).ToColumn("KACTYP");
            Map(p => p.Kacipb).ToColumn("KACIPB");
            Map(p => p.Kacalx).ToColumn("KACALX");
            Map(p => p.Kacavn).ToColumn("KACAVN");
            Map(p => p.Kachin).ToColumn("KACHIN");
            Map(p => p.Kacrsq).ToColumn("KACRSQ");
            Map(p => p.Kacobj).ToColumn("KACOBJ");
            Map(p => p.Kaccible).ToColumn("KACCIBLE");
            Map(p => p.Kacinven).ToColumn("KACINVEN");
            Map(p => p.Kacdesc).ToColumn("KACDESC");
            Map(p => p.Kacdesi).ToColumn("KACDESI");
            Map(p => p.Kacobsv).ToColumn("KACOBSV");
            Map(p => p.Kacape).ToColumn("KACAPE");
            Map(p => p.Kactre).ToColumn("KACTRE");
            Map(p => p.Kacclass).ToColumn("KACCLASS");
            Map(p => p.Kacnmc01).ToColumn("KACNMC01");
            Map(p => p.Kacnmc02).ToColumn("KACNMC02");
            Map(p => p.Kacnmc03).ToColumn("KACNMC03");
            Map(p => p.Kacnmc04).ToColumn("KACNMC04");
            Map(p => p.Kacnmc05).ToColumn("KACNMC05");
            Map(p => p.Kacmand).ToColumn("KACMAND");
            Map(p => p.Kacmanf).ToColumn("KACMANF");
            Map(p => p.Kacdspp).ToColumn("KACDSPP");
            Map(p => p.Kaclcivalo).ToColumn("KACLCIVALO");
            Map(p => p.Kaclcivala).ToColumn("KACLCIVALA");
            Map(p => p.Kaclcivalw).ToColumn("KACLCIVALW");
            Map(p => p.Kaclciunit).ToColumn("KACLCIUNIT");
            Map(p => p.Kaclcibase).ToColumn("KACLCIBASE");
            Map(p => p.Kackdiid).ToColumn("KACKDIID");
            Map(p => p.Kacfrhvalo).ToColumn("KACFRHVALO");
            Map(p => p.Kacfrhvala).ToColumn("KACFRHVALA");
            Map(p => p.Kacfrhvalw).ToColumn("KACFRHVALW");
            Map(p => p.Kacfrhunit).ToColumn("KACFRHUNIT");
            Map(p => p.Kacfrhbase).ToColumn("KACFRHBASE");
            Map(p => p.Kackdkid).ToColumn("KACKDKID");
            Map(p => p.Kacnsir).ToColumn("KACNSIR");
            Map(p => p.Kacmandh).ToColumn("KACMANDH");
            Map(p => p.Kacmanfh).ToColumn("KACMANFH");
            Map(p => p.Kacsurf).ToColumn("KACSURF");
            Map(p => p.Kacvmc).ToColumn("KACVMC");
            Map(p => p.Kacprol).ToColumn("KACPROL");
            Map(p => p.Kacpbi).ToColumn("KACPBI");
            Map(p => p.Kacbrnt).ToColumn("KACBRNT");
            Map(p => p.Kacbrnc).ToColumn("KACBRNC");
        }
    }
  

}
