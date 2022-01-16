using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpRsqMapper : EntityMap<KpRsq>   {
        public KpRsqMapper () {
            Map(p => p.Kabtyp).ToColumn("KABTYP");
            Map(p => p.Kabipb).ToColumn("KABIPB");
            Map(p => p.Kabalx).ToColumn("KABALX");
            Map(p => p.Kabavn).ToColumn("KABAVN");
            Map(p => p.Kabhin).ToColumn("KABHIN");
            Map(p => p.Kabrsq).ToColumn("KABRSQ");
            Map(p => p.Kabcible).ToColumn("KABCIBLE");
            Map(p => p.Kabdesc).ToColumn("KABDESC");
            Map(p => p.Kabdesi).ToColumn("KABDESI");
            Map(p => p.Kabobsv).ToColumn("KABOBSV");
            Map(p => p.Kabrepval).ToColumn("KABREPVAL");
            Map(p => p.Kabrepobl).ToColumn("KABREPOBL");
            Map(p => p.Kabape).ToColumn("KABAPE");
            Map(p => p.Kabtre).ToColumn("KABTRE");
            Map(p => p.Kabclass).ToColumn("KABCLASS");
            Map(p => p.Kabnmc01).ToColumn("KABNMC01");
            Map(p => p.Kabnmc02).ToColumn("KABNMC02");
            Map(p => p.Kabnmc03).ToColumn("KABNMC03");
            Map(p => p.Kabnmc04).ToColumn("KABNMC04");
            Map(p => p.Kabnmc05).ToColumn("KABNMC05");
            Map(p => p.Kabmand).ToColumn("KABMAND");
            Map(p => p.Kabmanf).ToColumn("KABMANF");
            Map(p => p.Kabdspp).ToColumn("KABDSPP");
            Map(p => p.Kablcivalo).ToColumn("KABLCIVALO");
            Map(p => p.Kablcivala).ToColumn("KABLCIVALA");
            Map(p => p.Kablcivalw).ToColumn("KABLCIVALW");
            Map(p => p.Kablciunit).ToColumn("KABLCIUNIT");
            Map(p => p.Kablcibase).ToColumn("KABLCIBASE");
            Map(p => p.Kabkdiid).ToColumn("KABKDIID");
            Map(p => p.Kabfrhvalo).ToColumn("KABFRHVALO");
            Map(p => p.Kabfrhvala).ToColumn("KABFRHVALA");
            Map(p => p.Kabfrhvalw).ToColumn("KABFRHVALW");
            Map(p => p.Kabfrhunit).ToColumn("KABFRHUNIT");
            Map(p => p.Kabfrhbase).ToColumn("KABFRHBASE");
            Map(p => p.Kabkdkid).ToColumn("KABKDKID");
            Map(p => p.Kabnsir).ToColumn("KABNSIR");
            Map(p => p.Kabmandh).ToColumn("KABMANDH");
            Map(p => p.Kabmanfh).ToColumn("KABMANFH");
            Map(p => p.Kabsurf).ToColumn("KABSURF");
            Map(p => p.Kabvmc).ToColumn("KABVMC");
            Map(p => p.Kabprol).ToColumn("KABPROL");
            Map(p => p.Kabpbi).ToColumn("KABPBI");
            Map(p => p.Kabbrnt).ToColumn("KABBRNT");
            Map(p => p.Kabbrnc).ToColumn("KABBRNC");
        }
    }
  

}
