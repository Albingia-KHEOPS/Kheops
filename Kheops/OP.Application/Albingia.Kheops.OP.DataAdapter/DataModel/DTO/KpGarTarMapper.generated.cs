using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpGarTarMapper : EntityMap<KpGarTar>   {
        public KpGarTarMapper () {
            Map(p => p.Kdgid).ToColumn("KDGID");
            Map(p => p.Kdgtyp).ToColumn("KDGTYP");
            Map(p => p.Kdgipb).ToColumn("KDGIPB");
            Map(p => p.Kdgalx).ToColumn("KDGALX");
            Map(p => p.Kdgavn).ToColumn("KDGAVN");
            Map(p => p.Kdghin).ToColumn("KDGHIN");
            Map(p => p.Kdgfor).ToColumn("KDGFOR");
            Map(p => p.Kdgopt).ToColumn("KDGOPT");
            Map(p => p.Kdggaran).ToColumn("KDGGARAN");
            Map(p => p.Kdgkdeid).ToColumn("KDGKDEID");
            Map(p => p.Kdgnumtar).ToColumn("KDGNUMTAR");
            Map(p => p.Kdglcimod).ToColumn("KDGLCIMOD");
            Map(p => p.Kdglciobl).ToColumn("KDGLCIOBL");
            Map(p => p.Kdglcivalo).ToColumn("KDGLCIVALO");
            Map(p => p.Kdglcivala).ToColumn("KDGLCIVALA");
            Map(p => p.Kdglcivalw).ToColumn("KDGLCIVALW");
            Map(p => p.Kdglciunit).ToColumn("KDGLCIUNIT");
            Map(p => p.Kdglcibase).ToColumn("KDGLCIBASE");
            Map(p => p.Kdgkdiid).ToColumn("KDGKDIID");
            Map(p => p.Kdgfrhmod).ToColumn("KDGFRHMOD");
            Map(p => p.Kdgfrhobl).ToColumn("KDGFRHOBL");
            Map(p => p.Kdgfrhvalo).ToColumn("KDGFRHVALO");
            Map(p => p.Kdgfrhvala).ToColumn("KDGFRHVALA");
            Map(p => p.Kdgfrhvalw).ToColumn("KDGFRHVALW");
            Map(p => p.Kdgfrhunit).ToColumn("KDGFRHUNIT");
            Map(p => p.Kdgfrhbase).ToColumn("KDGFRHBASE");
            Map(p => p.Kdgkdkid).ToColumn("KDGKDKID");
            Map(p => p.Kdgfmivalo).ToColumn("KDGFMIVALO");
            Map(p => p.Kdgfmivala).ToColumn("KDGFMIVALA");
            Map(p => p.Kdgfmivalw).ToColumn("KDGFMIVALW");
            Map(p => p.Kdgfmiunit).ToColumn("KDGFMIUNIT");
            Map(p => p.Kdgfmibase).ToColumn("KDGFMIBASE");
            Map(p => p.Kdgfmavalo).ToColumn("KDGFMAVALO");
            Map(p => p.Kdgfmavala).ToColumn("KDGFMAVALA");
            Map(p => p.Kdgfmavalw).ToColumn("KDGFMAVALW");
            Map(p => p.Kdgfmaunit).ToColumn("KDGFMAUNIT");
            Map(p => p.Kdgfmabase).ToColumn("KDGFMABASE");
            Map(p => p.Kdgprimod).ToColumn("KDGPRIMOD");
            Map(p => p.Kdgpriobl).ToColumn("KDGPRIOBL");
            Map(p => p.Kdgprivalo).ToColumn("KDGPRIVALO");
            Map(p => p.Kdgprivala).ToColumn("KDGPRIVALA");
            Map(p => p.Kdgprivalw).ToColumn("KDGPRIVALW");
            Map(p => p.Kdgpriunit).ToColumn("KDGPRIUNIT");
            Map(p => p.Kdgpribase).ToColumn("KDGPRIBASE");
            Map(p => p.Kdgmntbase).ToColumn("KDGMNTBASE");
            Map(p => p.Kdgprimpro).ToColumn("KDGPRIMPRO");
            Map(p => p.Kdgtmc).ToColumn("KDGTMC");
            Map(p => p.Kdgtff).ToColumn("KDGTFF");
            Map(p => p.Kdgcmc).ToColumn("KDGCMC");
            Map(p => p.Kdgcht).ToColumn("KDGCHT");
            Map(p => p.Kdgctt).ToColumn("KDGCTT");
        }
    }
  

}
