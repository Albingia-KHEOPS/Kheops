using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpSuspMapper : EntityMap<KpSusp>   {
        public KpSuspMapper () {
            Map(p => p.Kicid).ToColumn("KICID");
            Map(p => p.Kictyp).ToColumn("KICTYP");
            Map(p => p.Kicipb).ToColumn("KICIPB");
            Map(p => p.Kicalx).ToColumn("KICALX");
            Map(p => p.Kictye).ToColumn("KICTYE");
            Map(p => p.Kicipk).ToColumn("KICIPK");
            Map(p => p.Kicnur).ToColumn("KICNUR");
            Map(p => p.Kicori).ToColumn("KICORI");
            Map(p => p.Kicdebm).ToColumn("KICDEBM");
            Map(p => p.Kicdebd).ToColumn("KICDEBD");
            Map(p => p.Kicdebh).ToColumn("KICDEBH");
            Map(p => p.Kicfinm).ToColumn("KICFINM");
            Map(p => p.Kicfind).ToColumn("KICFIND");
            Map(p => p.Kicfinh).ToColumn("KICFINH");
            Map(p => p.Kicrsad).ToColumn("KICRSAD");
            Map(p => p.Kicrsah).ToColumn("KICRSAH");
            Map(p => p.Kicrevd).ToColumn("KICREVD");
            Map(p => p.Kicrevh).ToColumn("KICREVH");
            Map(p => p.Kiccru).ToColumn("KICCRU");
            Map(p => p.Kiccrd).ToColumn("KICCRD");
            Map(p => p.Kiccrh).ToColumn("KICCRH");
            Map(p => p.Kicavn).ToColumn("KICAVN");
            Map(p => p.Kicmju).ToColumn("KICMJU");
            Map(p => p.Kicmjd).ToColumn("KICMJD");
            Map(p => p.Kicmjh).ToColumn("KICMJH");
            Map(p => p.Kicsit).ToColumn("KICSIT");
            Map(p => p.Kicstu).ToColumn("KICSTU");
            Map(p => p.Kicstd).ToColumn("KICSTD");
            Map(p => p.Kicsth).ToColumn("KICSTH");
            Map(p => p.Kicaca).ToColumn("KICACA");
        }
    }
  

}
