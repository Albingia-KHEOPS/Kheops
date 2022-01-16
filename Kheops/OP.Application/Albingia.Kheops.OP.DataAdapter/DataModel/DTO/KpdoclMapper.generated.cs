using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpDocLMapper : EntityMap<KpDocL>   {
        public KpDocLMapper () {
            Map(p => p.Kelid).ToColumn("KELID");
            Map(p => p.Keltyp).ToColumn("KELTYP");
            Map(p => p.Kelipb).ToColumn("KELIPB");
            Map(p => p.Kelalx).ToColumn("KELALX");
            Map(p => p.Kelsua).ToColumn("KELSUA");
            Map(p => p.Kelnum).ToColumn("KELNUM");
            Map(p => p.Kelsbr).ToColumn("KELSBR");
            Map(p => p.Kelserv).ToColumn("KELSERV");
            Map(p => p.Kelactg).ToColumn("KELACTG");
            Map(p => p.Kelactn).ToColumn("KELACTN");
            Map(p => p.Kelavn).ToColumn("KELAVN");
            Map(p => p.Kellib).ToColumn("KELLIB");
            Map(p => p.Kelstu).ToColumn("KELSTU");
            Map(p => p.Kelsit).ToColumn("KELSIT");
            Map(p => p.Kelstd).ToColumn("KELSTD");
            Map(p => p.Kelsth).ToColumn("KELSTH");
            Map(p => p.Kelcru).ToColumn("KELCRU");
            Map(p => p.Kelcrd).ToColumn("KELCRD");
            Map(p => p.Kelcrh).ToColumn("KELCRH");
            Map(p => p.Kelmaju).ToColumn("KELMAJU");
            Map(p => p.Kelmajd).ToColumn("KELMAJD");
            Map(p => p.Kelmajh).ToColumn("KELMAJH");
            Map(p => p.Kelemi).ToColumn("KELEMI");
            Map(p => p.Kelipk).ToColumn("KELIPK");
        }
    }
  

}
