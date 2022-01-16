using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KVerrouMapper : EntityMap<KVerrou>   {
        public KVerrouMapper () {
            Map(p => p.Kavid).ToColumn("KAVID");
            Map(p => p.Kavserv).ToColumn("KAVSERV");
            Map(p => p.Kavtyp).ToColumn("KAVTYP");
            Map(p => p.Kavipb).ToColumn("KAVIPB");
            Map(p => p.Kavalx).ToColumn("KAVALX");
            Map(p => p.Kavavn).ToColumn("KAVAVN");
            Map(p => p.Kavsua).ToColumn("KAVSUA");
            Map(p => p.Kavnum).ToColumn("KAVNUM");
            Map(p => p.Kavsbr).ToColumn("KAVSBR");
            Map(p => p.Kavactg).ToColumn("KAVACTG");
            Map(p => p.Kavact).ToColumn("KAVACT");
            Map(p => p.Kavverr).ToColumn("KAVVERR");
            Map(p => p.Kavcru).ToColumn("KAVCRU");
            Map(p => p.Kavcrd).ToColumn("KAVCRD");
            Map(p => p.Kavcrh).ToColumn("KAVCRH");
            Map(p => p.Kavlib).ToColumn("KAVLIB");
        }
    }
  

}
